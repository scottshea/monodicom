#region License

// Copyright (c) 2010, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Dicom.Utilities;

namespace ClearCanvas.Dicom.DataStore
{
	public partial class DataAccessLayer
	{
		internal static class QueryBuilder
		{
			public static string BuildHqlQuery<T>(QueryCriteria queryCriteria)
			{
				StringBuilder hqlQuery = new StringBuilder(1024);
				hqlQuery.AppendFormat(" FROM {0} ", typeof(T).Name);

				bool whereClauseAdded = false;

				foreach (KeyValuePair<DicomTagPath, string> criteria in queryCriteria)
				{
					if (criteria.Value.Length > 0)
					{
						QueryablePropertyInfo property = QueryableProperties<Study>.GetProperty(criteria.Key);

						if (property == null || property.IsHigherLevelUnique || property.PostFilterOnly)
							continue;

						string hqlCriteria = ConvertCriteria(criteria.Value, property);
						if (hqlCriteria.Length == 0)
							continue;

						if (whereClauseAdded)
						{
							hqlQuery.AppendFormat(" AND ");
						}
						else
						{
							hqlQuery.AppendFormat("WHERE ");
							whereClauseAdded = true;
						}

						hqlQuery.AppendFormat("({0})", hqlCriteria);
					}
				}

				return hqlQuery.ToString();
			}

			private static string ConvertCriteria(string criteria, QueryablePropertyInfo property)
			{
				if (String.IsNullOrEmpty(criteria)) //Universal matching.
					return "";

				StandardizeCriteria(ref criteria);

				if (property.Path.Equals(DicomTags.ModalitiesInStudy))
				{
					return ConvertModalitiesInStudyCriteria(criteria, property.ColumnName);
				}
				else if (property.Path.ValueRepresentation.Name == "UI")
				{
					return ConvertListOfUidCriteria(criteria, property.ColumnName);
				}
				else if (property.Path.ValueRepresentation.Name == "DA")
				{
					return ConvertDateCriteria(criteria, property.ColumnName);
				}
				else if (property.Path.ValueRepresentation.Name == "TM")
				{
					// Unsupported at this time.
					//ConvertTimeCriteria(...)
				}
				else if (property.Path.ValueRepresentation.Name == "DT")
				{
					// Unsupported at this time.
					//ConvertDateTimeCriteria(...)
				}
				else if (IsWildCardCriteria(criteria, property))
				{
					return ConvertWildCardCriteria(criteria, property.ColumnName);
				}
				else
				{
					return ConvertSingleValueCriteria(criteria, property.ColumnName);
				}

				return "";
			}

			// NOTE: this method is not bulletproof in the (very, very, very!) unusual case where there are
			// wildcards in the modalities in study criteria, but we still try to narrow down the result set
			// that is going to be post-filtered.
			private static string ConvertModalitiesInStudyCriteria(string criteria, string columnName)
			{
				//Modalities in study is a special case, where we allow list matching (not dicom compliant).
				string[] criteriaValues = DicomStringHelper.GetStringArray(criteria);
				StringBuilder builder = new StringBuilder();

				int i = 0;
				foreach (string criteriaValue in criteriaValues)
				{
					criteria = criteriaValue;
					string filter = "";
					if (!ContainsWildcardCharacters(criteria))
					{
						filter = String.Format("{0} = '{1}' OR ", columnName, criteria);
					}
					else
					{
						ReplaceWildcardCharacters(ref criteria);
						filter = String.Format("{0} LIKE '{1}' OR ", columnName, criteria);
					}

					filter += String.Format(@"{0} LIKE '{1}\%' " +
											@" OR {0} LIKE '%\{1}' " +
											@" OR {0} LIKE '%\{1}\%' ", columnName, criteria);

					if (i++ > 0)
						builder.Append(" OR ");

					builder.AppendFormat(" ({0}) ", filter);
				}

				return builder.ToString();
			}

			private static string ConvertListOfUidCriteria(string listOfUidCriteria, string columnName)
			{
				string uids = StringUtilities.Combine(DicomStringHelper.GetStringArray(listOfUidCriteria), ", ",
					delegate(string value) { return String.Format("'{0}'", value); });

				return String.Format("{0} in ( {1} )", columnName, uids);
			}

			private static string ConvertDateCriteria(string dateCriteria, string columnName)
			{
				string fromDate, toDate;
				bool isRange;

				DateRangeHelper.Parse(dateCriteria, out fromDate, out toDate, out isRange);
				StringBuilder dateRangeCriteria = new StringBuilder();

				if (fromDate != "")
				{
					//When a dicom date is specified with no '-', it is to be taken as an exact date.
					if (!isRange)
					{
						dateRangeCriteria.AppendFormat("( {0} = '{1}' )", columnName, fromDate);
					}
					else
					{
						dateRangeCriteria.AppendFormat("( {0} IS NOT NULL AND {0} >= '{1}' )", columnName, fromDate);
					}
				}

				if (toDate != "")
				{
					if (fromDate == "")
						dateRangeCriteria.AppendFormat("( {0} IS NULL OR ", columnName);
					else
						dateRangeCriteria.AppendFormat(" AND (");

					dateRangeCriteria.AppendFormat("{0} <= '{1}' )", columnName, toDate);
				}

				//will only happen if the query string is bad.
				string returnCriteria = dateRangeCriteria.ToString();
				if (String.IsNullOrEmpty(returnCriteria))
					return returnCriteria;

				return String.Format("({0})", returnCriteria);
			}

			private static string ConvertWildCardCriteria(string wildCardCriteria, string columnName)
			{
				ReplaceWildcardCharacters(ref wildCardCriteria);

				// don't forget to append the trailing underscore for column names
				return String.Format("{0} LIKE '{1}'", columnName, wildCardCriteria);
			}

			private static string ConvertSingleValueCriteria(string singleValueCriteria, string columnName)
			{
				return String.Format("{0} = '{1}'", columnName, singleValueCriteria);
			}

			private static void ReplaceWildcardCharacters(ref string criteria)
			{
				criteria = criteria.Replace("*", "%");
			}

			private static void StandardizeCriteria(ref string criteria)
			{
				criteria = criteria.Replace("'", "''");
			}
		}
	}
}