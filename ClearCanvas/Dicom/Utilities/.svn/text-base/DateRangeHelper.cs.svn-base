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

namespace ClearCanvas.Dicom.Utilities
{
	/// <summary>
	/// Will parse a date range adhering to the dicom format.  For example:
	/// 
	///		DateRange				From (parsed)		To (parsed)			Range?
	/// ----------------------------------------------------------------------------
	///		20070606				20070606			-					No
	/// 	20070606-				20070606			-					Yes
	/// 	-20070606				Beginning of time	20070606			Yes
	///		20060101-20070606		20060101			20070606			Yes
	/// </summary>
	public sealed class DateRangeHelper
	{
		private DateRangeHelper()
		{ 
		}

		/// <summary>
		/// The semantics of the fromDate and toDate, is:
		/// <table>
		/// <tr><td>fromDate</td><td>toDate</td><td>Query</td></tr>
		/// <tr><td>null</td><td>null</td><td>Empty</td></tr>
		/// <tr><td>20060608</td><td>null</td><td>Since: "20060608-"</td></tr>
		/// <tr><td>20060608</td><td>20060610</td><td>Between: "20060608-20060610"</td></tr>
		/// <tr><td>null</td><td>20060610</td><td>Prior to: "-20060610"</td></tr>
		/// </table>
		/// </summary>
		/// <param name="fromDate"></param>
		/// <param name="toDate"></param>
		public static string GetDicomDateRangeQueryString(DateTime? fromDate, DateTime? toDate)
		{
			if (null == fromDate && null == toDate)
			{
				return "";
			}
			else if (fromDate == toDate)
			{
				return ((DateTime)fromDate).ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
			}
			else if (null != fromDate && null == toDate)
			{
				return ((DateTime)fromDate).ToString("yyyyMMdd-", System.Globalization.CultureInfo.InvariantCulture);
			}
			else if (null != fromDate && null != toDate)
			{
				return ((DateTime)fromDate).ToString("yyyyMMdd-", System.Globalization.CultureInfo.InvariantCulture)
				       + ((DateTime)toDate).ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
			}
			else if (null == fromDate && null != toDate)
			{
				return ((DateTime)toDate).ToString("-yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
			}

			return "";
		}

		/// <summary>
		/// Will parse a date range adhering to the dicom date format, returning the dates as <see cref="DateTime"/> objects.
		/// </summary>
		/// <param name="dateRange">the string to be parsed</param>
		/// <param name="fromDate">the "from date", or null</param>
		/// <param name="toDate">the "to date" or null</param>
		/// <param name="isRange">whether or not the input value was actually a range.  If not, then the "from date" value should be taken 
		/// to be an exact value, not a range, depending on the application.</param>
		/// <exception cref="InvalidOperationException">if the input range is poorly formatted</exception>
		public static void Parse(string dateRange, out DateTime? fromDate, out DateTime? toDate, out bool isRange)
		{
			try
			{
				fromDate = null;
				toDate = null;
				isRange = false;

				if (dateRange == null)
					return;

				string fromDateString = "", toDateString = "";
				string[] splitRange = dateRange.Split('-');

				if (splitRange.Length == 1)
				{
					fromDateString = splitRange[0];
				}
				else if (splitRange.Length == 2)
				{
					fromDateString = splitRange[0];
					toDateString = splitRange[1];
					isRange = true;
				}
				else
				{
					throw new InvalidOperationException(string.Format(SR.ExceptionPoorlyFormattedDateRange, dateRange));
				}

				DateTime outDate;

				if (fromDateString == "")
				{
					fromDate = null;
				}
				else
				{
					if (!DateParser.Parse(fromDateString, out outDate))
						throw new InvalidOperationException(string.Format(SR.ExceptionPoorlyFormattedDateRange, dateRange));

					fromDate = outDate;
				}


				if (toDateString == "")
				{
					toDate = null;
				}
				else
				{
					if (!DateParser.Parse(toDateString, out outDate))
						throw new InvalidOperationException(string.Format(SR.ExceptionPoorlyFormattedDateRange, dateRange));

					toDate = outDate;
				}

				if (fromDate != null && toDate != null)
				{
					if (fromDate > toDate)
						throw new InvalidOperationException(string.Format(SR.ExceptionPoorlyFormattedDateRange, dateRange));
				}
			}
			catch
			{
				fromDate = toDate = null;
				throw;
			}
		}

		/// <summary>
		/// Will parse a date range adhering to the dicom date format, returning the dates as integers.
		/// </summary>
		/// <param name="dateRange">the string to be parsed</param>
		/// <param name="fromDate">the "from date", or null</param>
		/// <param name="toDate">the "to date" or null</param>
		/// <param name="isRange">whether or not the input value was actually a range.  If not, then the "from date" value should be taken 
		/// to be an exact value, not a range, depending on the application.</param>
		public static void Parse(string dateRange, out int fromDate, out int toDate, out bool isRange)
		{
			string fromDateString, toDateString;
			Parse(dateRange, out fromDateString, out toDateString, out isRange);

			if (fromDateString == "")
			{
				fromDate = 0;
			}
			else
			{
				//the string is guaranteed to be formatted like "yyyyMMdd", so this is safe.
				fromDate = Convert.ToInt32(fromDateString, System.Globalization.CultureInfo.InvariantCulture);
			}

			if (toDateString == "")
			{
				toDate = 0;
			}
			else
			{
				//the string is guaranteed to be formatted like "yyyyMMdd", so this is safe.
				toDate = Convert.ToInt32(toDateString, System.Globalization.CultureInfo.InvariantCulture);
			}
		}

		/// <summary>
		/// Will parse a date range adhering to the dicom date format, returning the dates as strings.  In the case where the input
		/// dates are formatted according to the old Dicom Standard (yyyy.MM.dd), the resulting strings will be reformatted according
		/// to the current Dicom Standard.
		/// </summary>
		/// <param name="dateRange">the string to be parsed</param>
		/// <param name="fromDate">the "from date", or null</param>
		/// <param name="toDate">the "to date" or null</param>
		/// <param name="isRange">whether or not the input value was actually a range.  If not, then the "from date" value should be taken 
		/// to be an exact value, not a range, depending on the application.</param>
		public static void Parse(string dateRange, out string fromDate, out string toDate, out bool isRange)
		{
			DateTime? from, to;
			Parse(dateRange, out from, out to, out isRange);

			fromDate = (from != null) ? ((DateTime)from).ToString(DateParser.DicomDateFormat, System.Globalization.CultureInfo.InvariantCulture) : "";
			toDate = (to != null) ? ((DateTime)to).ToString(DateParser.DicomDateFormat, System.Globalization.CultureInfo.InvariantCulture) : "";
		}
	}
}