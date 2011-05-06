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
using System.Collections;
using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using NHibernate;
using NHibernate.Criterion;

namespace ClearCanvas.Dicom.DataStore
{
	public sealed partial class DataAccessLayer
	{
		private class DataStoreReader : SessionConsumer, IDataStoreReader
		{
			public DataStoreReader(ISessionManager sessionManager)
				: base(sessionManager)
			{
			}

			#region IDataStoreReader Members

			public long GetStudyCount()
			{
				string queryString = "select count(*) from Study";
				try
				{
					SessionManager.BeginReadTransaction();
					IEnumerable results = SessionManager.Session.CreateQuery(queryString).Enumerable();
					foreach (long number in results)
						return number;

					return 0;
				}
				catch (Exception e)
				{
					string message = String.Format("An error occurred while attempting to get the number of studies in the data store.");
					throw new DataStoreException(message, e);
				}
			}

			public IStudy GetStudy(string studyInstanceUid)
			{
				Platform.CheckForEmptyString(studyInstanceUid, "studyInstanceUid");

				try
				{
					SessionManager.BeginReadTransaction();

					IList listOfStudies = Session.CreateCriteria(typeof(Study))
						.Add(NHibernate.Criterion.Expression.Eq("StudyInstanceUid", studyInstanceUid))
						.List();

					if (null != listOfStudies && listOfStudies.Count > 0)
						return (Study)listOfStudies[0];

					return null;
				}
				catch (Exception e)
				{
					string message = String.Format("An error occurred while attempting to retrieve the study ({0}) from the data store.", studyInstanceUid);
					throw new DataStoreException(message, e);
				}
			}

			public IEnumerable<IStudy> GetStudies()
			{
				try
				{
					SessionManager.BeginReadTransaction();
					IQuery query = Session.CreateQuery("FROM Study");
					return Convert.Cast<IStudy>(query.List());
				}
				catch (Exception e)
				{
					throw new DataStoreException("An error occurred while attempting to retrieve all studies from the data store.", e);
				}
			}

			public IEnumerable<IStudy> GetStudiesByStoreTime(bool descending)
			{
				try
				{
					Order order = descending ? Order.Desc("StoreTime") : Order.Asc("StoreTime");
					SessionManager.BeginReadTransaction();
					ICriteria query = Session.CreateCriteria(typeof(Study)).AddOrder(order);
					return Convert.Cast<IStudy>(query.List());
				}
				catch (Exception e)
				{
					throw new DataStoreException("An error occurred while attempting to retrieve all studies from the data store.", e);
				}
			}

			public IEnumerable<DicomAttributeCollection> Query(DicomAttributeCollection queryCriteria)
			{
				Platform.CheckForNullReference(queryCriteria, "queryCriteria");

				QueryCriteria convertedCriteria = new QueryCriteria(queryCriteria);

				string level = queryCriteria[DicomTags.QueryRetrieveLevel].ToString();
				switch(level)
				{
					case "STUDY":
						return StudyQuery(convertedCriteria);
					case "SERIES":
						return SeriesQuery(convertedCriteria);
					case "IMAGE":
						return ImageQuery(convertedCriteria);
					default:
						throw new ArgumentException(String.Format("Invalid query level: {0}", level));
				}
			}

			#endregion

			private List<DicomAttributeCollection> StudyQuery(QueryCriteria queryCriteria)
			{
				try
				{
					string hqlQuery = QueryBuilder.BuildHqlQuery<Study>(queryCriteria);
					SessionManager.BeginReadTransaction();
					IList studiesFound = Session.CreateQuery(hqlQuery).List();

					QueryResultFilter<Study> filter =
						new QueryResultFilter<Study>(queryCriteria, Convert.Cast<Study>(studiesFound), GetSpecificCharacterSet);
					return filter.GetResults();
				}
				catch(DataStoreException)
				{
					throw;
				}
				catch (Exception e)
				{
					throw new DataStoreException("An error occurred while performing the study root query.", e);
				}
			}

			private List<DicomAttributeCollection> SeriesQuery(QueryCriteria queryCriteria)
			{
				string studyUid = queryCriteria[DicomTags.StudyInstanceUid];
				if (String.IsNullOrEmpty(studyUid))
					throw new ArgumentException("The study uid must be specified for a series level query.");

				IStudy study = GetStudy(studyUid);
				if (study == null)
					throw new ArgumentException(String.Format("No study exists with the given study uid ({0}).", studyUid));

				try
				{
					QueryResultFilter<Series> filter =
						new QueryResultFilter<Series>(queryCriteria, Convert.Cast<Series>(study.GetSeries()), GetSpecificCharacterSet);
					return filter.GetResults();
				}
				catch(DataStoreException)
				{
					throw;
				}
				catch(Exception e)
				{
					throw new DataStoreException("An error occurred while performing the series query.", e);
				}
			}

			private List<DicomAttributeCollection> ImageQuery(QueryCriteria queryCriteria)
			{
				string studyUid = queryCriteria[DicomTags.StudyInstanceUid];
				string seriesUid = queryCriteria[DicomTags.SeriesInstanceUid];

				if (String.IsNullOrEmpty(studyUid) || String.IsNullOrEmpty(seriesUid))
					throw new ArgumentException("The study and series uids must be specified for an image level query.");

				IStudy study = GetStudy(studyUid);
				if (study == null)
					throw new ArgumentException(String.Format("No study exists with the given study uid ({0}).", studyUid));
					
				ISeries series = CollectionUtils.SelectFirst(study.GetSeries(),
									delegate(ISeries test) { return test.SeriesInstanceUid == seriesUid; });

				if (series == null)
				{
					string message = String.Format("No series exists with the given study and series uids ({0}, {1})", studyUid, seriesUid);
					throw new ArgumentException(message);
				}

				try
				{
					QueryResultFilter<SopInstance> filter =
						new QueryResultFilter<SopInstance>(queryCriteria, Convert.Cast<SopInstance>(series.GetSopInstances()), GetSpecificCharacterSet);
					return filter.GetResults();
				}
				catch (DataStoreException)
				{
					throw;
				}
				catch (Exception e)
				{
					throw new DataStoreException("An error occurred while performing the image query.", e);
				}
			}

			private static string GetSpecificCharacterSet(Study study)
			{
				return study.SpecificCharacterSet;
			}

			private static string GetSpecificCharacterSet(Series series)
			{
				return series.SpecificCharacterSet;
			}

			private static string GetSpecificCharacterSet(SopInstance sop)
			{
				return sop.SpecificCharacterSet;
			}
			
			protected override void Dispose(bool disposing)
			{
				SessionManager.Commit();
				base.Dispose(disposing);
			}
		}
	}
}
