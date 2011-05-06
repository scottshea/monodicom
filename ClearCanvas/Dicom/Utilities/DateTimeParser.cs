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
using System.Globalization;

namespace ClearCanvas.Dicom.Utilities
{
	public static class DateTimeParser
	{
		public static readonly string DicomFullDateTimeFormatWithTimeZone = "yyyyMMddHHmmss.ffffff&zzzz";
		public static readonly string DicomFullDateTimeFormat = "yyyyMMddHHmmss.ffffff";

		private static readonly char[] _plusMinus = { '+', '-' };

		/// <summary>
		/// Attempts to parse the time string exactly, according to accepted Dicom datetime format(s).
		/// Will *not* throw an exception if the format is invalid (better for when performance is needed).
		/// </summary>
		/// <param name="dateTimeString">the dicom datetime string</param>
		/// <returns>a nullable DateTime</returns>
		public static DateTime? Parse(string dateTimeString)
		{
			DateTime dateTime;
			if (!Parse(dateTimeString, out dateTime))
				return null;

			return dateTime;
		}

		/// <summary>
		/// Parses a dicom Date/Time string using the DateParser and TimeParser
		/// (TryParseExact) functions.  The Hour/Minute adjustment factor (as
		/// specified in Dicom for universal time adjustment) is accounted for 
		/// (and parsed) by this function.
		/// </summary>
		/// <param name="dicomDateTime">the dicom date/time string</param>
		/// <param name="dateTime">the date/time as a DateTime object</param>
		/// <returns>true on success, false otherwise</returns>
		public static bool Parse(string dicomDateTime, out DateTime dateTime)
		{
			dateTime = new DateTime(); 
			
			if (String.IsNullOrEmpty(dicomDateTime))
				return false;

			int plusMinusIndex = dicomDateTime.IndexOfAny(_plusMinus);

			string dateTimeString = dicomDateTime;

			string offsetString = String.Empty;
			if (plusMinusIndex > 0)
			{
				offsetString = dateTimeString.Substring(plusMinusIndex);
				dateTimeString = dateTimeString.Remove(plusMinusIndex);
			}

			string dateString;
			if (dateTimeString.Length >= 8)
				dateString = dateTimeString.Substring(0, 8);
			else
				return false;

			string timeString = String.Empty;
			if (dateTimeString.Length > 8)
				timeString = dateTimeString.Substring(8);

			int hourOffset = 0;
			int minuteOffset = 0;
			if (!String.IsNullOrEmpty(offsetString))
			{
				if (offsetString.Length > 3)
				{
					if (!Int32.TryParse(offsetString.Substring(3), NumberStyles.Integer, CultureInfo.InvariantCulture, out minuteOffset))
						return false;

					if (!Int32.TryParse(offsetString.Remove(3), NumberStyles.Integer, CultureInfo.InvariantCulture, out hourOffset))
						return false;
				}
				else
				{
					if (!Int32.TryParse(offsetString, NumberStyles.Integer, CultureInfo.InvariantCulture, out hourOffset))
						return false;
				}

				minuteOffset *= Math.Sign(hourOffset);
			}

			DateTime date;
			if (!DateParser.Parse(dateString, out date))
				return false;

			DateTime time = new DateTime(); //zero datetime
			if (!String.IsNullOrEmpty(timeString))
			{
				if (!TimeParser.Parse(timeString, out time))
					return false;
			}

			dateTime = date;
			dateTime = dateTime.AddTicks(time.Ticks);
			dateTime = dateTime.AddHours(hourOffset);
			dateTime = dateTime.AddMinutes(minuteOffset);

			return true;
		}

		/// <summary>
		/// Convert a datetime object into a DT string
		/// </summary>
		/// <param name="datetime"></param>
		/// <returns></returns>
		public static string ToDicomString(DateTime datetime, bool toUTC)
		{
			if (toUTC)
			{
				DateTime utc = datetime.ToUniversalTime();
				return utc.ToString(DicomFullDateTimeFormatWithTimeZone, System.Globalization.CultureInfo.InvariantCulture);
			}
			else
			{
				return datetime.ToString(DicomFullDateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
			}
            
		}

		/// <summary>
		/// Parses Dicom Date/Time tags.  The <paramref name="dicomDateTime"/> would be a DateTime tag value - such as AcquisitionDatetime,
		/// the <paramref name="dicomDate"/> would be just a Date tag value - such as AcquisitionDate; and <paramref name="dicomTime"/> would
		/// be just the Time tag value - such as AcquisitionTime.  So, this method will parse the <paramref name="dicomDateTime"/> if it is not empty,
		/// otherwise it will parse the <paramref name="dicomDate"/> and <paramref name="dicomTimee/> together.
		/// </summary>
		/// <param name="dicomDateTime">The dicom date time.</param>
		/// <param name="dicomDate">The dicom date.</param>
		/// <param name="dicomTime">The dicom time.</param>
		/// <param name="dateTime">The date time.</param>
		/// <returns></returns>
		public static bool ParseDateAndTime(string dicomDateTime, string dicomDate, string dicomTime, out DateTime outDateTime)
		{
			outDateTime = DateTime.MinValue; // set default value
			try
			{
				string dateTimeConcat = null;

				string dateValue = dicomDate == null ? String.Empty : dicomDate.Trim();
				string timeValue = dicomTime == null ? String.Empty : dicomTime.Trim();
				if (timeValue == "000000") timeValue = String.Empty; // might as well be blank

				string dateTimeValue = dicomDateTime == null ? String.Empty : dicomDateTime.Trim();

				if (dateTimeValue == String.Empty && dateValue == String.Empty && timeValue == String.Empty)
					return false;

				// First try to do dateValue and timeValue separately - if both are there then set 
				// dateTimeConcat, and then parse dateTimeConcat the same as if dateTimeValue was set
				if (dateTimeValue == String.Empty)
				{
					// use separate date and time values
					// first get rid of .'s in date value, if any
					dateValue = dateValue.Replace(".", "");

					// see if only the date or time was sent in - if so, then good, parse it immediately, else set dateTimeConcat and parse it later 
					if (dateValue == String.Empty)
					{
						if (timeValue.IndexOf(".") == -1)
							outDateTime = DateTime.ParseExact(timeValue, "HHmmss", CultureInfo.InvariantCulture);
						else
							outDateTime = DateTime.ParseExact(timeValue, "HHmmss.ffffff", CultureInfo.InvariantCulture);
					}
					else if (timeValue == String.Empty)
					{
						outDateTime = DateTime.ParseExact(dateValue, "yyyyMMdd", CultureInfo.InvariantCulture);
					}
					else
					{
						dateTimeConcat = dateValue + timeValue;
					}
				}
				else
				{
					// dateTimeValue was set, set dateTimeConcat and parse it later
					dateTimeConcat = dateTimeValue;
				}

				if (dateTimeConcat != null)
				{
					string[] date_formats = new string[15];
					date_formats[0] = "yyyyMMdd";
					date_formats[1] = "yyyy.MM.dd";
					date_formats[2] = "yyyy";
					date_formats[3] = "yyyyMM";
					date_formats[4] = "yyyy.MM";
					date_formats[5] = "yyyyMMddHHmmss";
					date_formats[6] = "yyyyMMddHHmmss.f";
					date_formats[6] = "yyyyMMddHHmmss.ff";
					date_formats[7] = "yyyyMMddHHmmss.ffffff";
					date_formats[8] = "yyyyMMddHHmmss.ffffffzzz";
					date_formats[9] = "yyyyMMddHHmmss.fff";
					date_formats[10] = "MM/dd/yyyy HH:mm:ss tt";
					date_formats[11] = "MM/dd/yyyy HH:mm:ss t";
					date_formats[12] = "MM/dd/yyyy HH:mm:ss";
					date_formats[13] = "MM/dd/yyyy";
					outDateTime = DateTime.ParseExact(dateTimeConcat, date_formats, CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault);

				}

				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Parses Dicom Date/Time tags.  The <paramref name="dicomDateTime"/> would be a DateTime tag value - such as AcquisitionDatetime,
		/// the <paramref name="dicomDate"/> would be just a Date tag value - such as AcquisitionDate; and <paramref name="dicomTime"/> would
		/// be just the Time tag value - such as AcquisitionTime.  So, this method will parse the <paramref name="dicomDateTime"/> if it is not empty,
		/// otherwise it will parse the <paramref name="dicomDate"/> and <paramref name="dicomTime/> together.
		/// </summary>
		/// <param name="dicomDateTime">The dicom date time.</param>
		/// <param name="dicomDate">The dicom date.</param>
		/// <param name="dicomTime">The dicom time.</param>
		/// <returns></returns>
		public static DateTime? ParseDateAndTime(string dicomDateTime, string dicomDate, string dicomTime)
		{
			DateTime dateTime;
			if (!ParseDateAndTime(dicomDateTime, dicomDate, dicomTime, out dateTime))
				return null;

			return dateTime;
		}

		/// <summary>
		/// Parses the date and time.  Gets the values for each tag from the attribute colllection. The <paramref name="dicomDateTime"/> would be a DateTime tag value - such as AcquisitionDatetime,
		/// the <paramref name="dicomDate"/> would be just a Date tag value - such as AcquisitionDate; and <paramref name="dicomTime"/> would
		/// be just the Time tag value - such as AcquisitionTime.  So, this method will parse the <paramref name="dicomDateTime"/> if it is not empty,
		/// otherwise it will parse the <paramref name="dicomDate"/> and <paramref name="dicomTime"/> together.
		/// </summary>
		/// <param name="dicomAttributeCollection">The dicom attribute collection.</param>
		/// <param name="dicomDateTimeTag">The dicom date time tag.</param>
		/// <param name="dicomDateTag">The dicom date tag.</param>
		/// <param name="dicomTimeTag">The dicom time tag.</param>
		/// <returns></returns>
		public static DateTime? ParseDateAndTime(IDicomAttributeProvider dicomAttributeProvider, uint dicomDateTimeTag, uint dicomDateTag, uint dicomTimeTag)
		{
			if (dicomAttributeProvider == null)
				throw new ArgumentNullException("dicomAttributeProvider");

			string dicomDateTime = dicomDateTimeTag == 0 ? String.Empty : dicomAttributeProvider[dicomDateTimeTag].GetString(0, String.Empty);
			string dicomDate = dicomDateTag == 0 ? String.Empty : dicomAttributeProvider[dicomDateTag].GetString(0, String.Empty);
			string dicomTime = dicomTimeTag == 0 ? String.Empty : dicomAttributeProvider[dicomTimeTag].GetString(0, String.Empty);

			return ParseDateAndTime(dicomDateTime, dicomDate, dicomTime);
		}

		/// <summary>
		/// Sets the specified date time attribute values based on the specified <paramref name="value">Date Time value</paramref>.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="dateAttribute">The date attribute.</param>
		/// <param name="timeAttribute">The time attribute.</param>
		public static void SetDateTimeAttributeValues(DateTime? value, DicomAttribute dateAttribute, DicomAttribute timeAttribute)
		{
			if (value.HasValue)
			{
				dateAttribute.SetDateTime(0, value.Value);
				timeAttribute.SetDateTime(0, value.Value);
			}
			else
			{
				dateAttribute.SetNullValue();
				timeAttribute.SetNullValue();
			}
		}

		/// <summary>
		/// Sets the specified date attribute values based on the specified <paramref name="value">Date value</paramref>.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="dateAttribute">The date attribute.</param>
		public static void SetDateAttributeValues(DateTime? value, DicomAttribute dateAttribute)
		{
			if (value.HasValue)
			{
				dateAttribute.SetDateTime(0, value.Value);
			}
			else
			{
				dateAttribute.SetNullValue();
			}
		}

		/// <summary>
		/// Sets the date time attribute values for the specified dicom attributes.
		/// Will first attempt to write to the <paramref name="dateTimeAttribute"/> if it is not null, otherwise
		/// it will write the values to the separate date and time attributes.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="dateTimeAttribute">The date time attribute.</param>
		/// <param name="dateAttribute">The date attribute.</param>
		/// <param name="timeAttribute">The time attribute.</param>
		public static void SetDateTimeAttributeValues(DateTime? value, DicomAttribute dateTimeAttribute, DicomAttribute dateAttribute, DicomAttribute timeAttribute)
		{
			if (dateTimeAttribute != null)
			{
				if (value.HasValue)
					dateTimeAttribute.SetDateTime(0, value.Value);
				else
					dateTimeAttribute.SetNullValue();
			}
			else
			{
				SetDateTimeAttributeValues(value, dateAttribute, timeAttribute);
			}
		}

		/// <summary>
		/// Sets the date time attribute values for the specified attributes in the specified <paramref name="dicomAttributeCollection"/>.
		/// Will first attempt to write to the <paramref name="dicomDateTimeTag"/> if it is non zero, otherwise
		/// it will write the values to the separate date and time tags.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="dicomAttributeProvider">The dicom attribute provider.</param>
		/// <param name="dicomDateTimeTag">The dicom date time tag.</param>
		/// <param name="dicomDateTag">The dicom date tag.</param>
		/// <param name="dicomTimeTag">The dicom time tag.</param>
		public static void SetDateTimeAttributeValues(DateTime? value, IDicomAttributeProvider dicomAttributeProvider, uint dicomDateTimeTag, uint dicomDateTag, uint dicomTimeTag)
		{
			if (dicomAttributeProvider == null)
				throw new ArgumentNullException("dicomAttributeProvider");
			if (dicomDateTimeTag != 0)
			{
				DicomAttribute dateTimeAttribute = dicomAttributeProvider[dicomDateTimeTag];
				SetDateTimeAttributeValues(value, dateTimeAttribute, null, null);
			}
			else
			{
				if (dicomTimeTag == 0)
					SetDateAttributeValues(value,dicomAttributeProvider[dicomDateTag]);
				else
					SetDateTimeAttributeValues(value, dicomAttributeProvider[dicomDateTag], dicomAttributeProvider[dicomTimeTag]);
			}
		}

	}
}