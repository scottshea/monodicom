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
	/// <summary>
	/// The DateParser class parses dates that are formatted correctly according to Dicom.
	/// 
	/// We use the TryParseExact function to parse the dates because it is far more efficient
	/// than ParseExact since it does not throw exceptions.
	/// 
	/// See http://blogs.msdn.com/ianhu/archive/2005/12/19/505702.aspx for a good profile
	/// comparision of the different Parse/Convert methods.
	/// </summary>
	public static class DateParser
	{
		public const string DicomDateFormat = "yyyyMMdd";

		/// <summary>
		/// Attempts to parse the date string exactly, according to accepted Dicom format(s).
		/// Will *not* throw an exception if the format is invalid.
		/// </summary>
		/// <param name="dicomDate">the dicom date string</param>
		/// <returns>a nullable DateTime</returns>
		public static DateTime? Parse(string dicomDate)
		{
			DateTime date;
			if (!Parse(dicomDate, out date))
				return null;

			return date;
		}

		/// <summary>
		/// Attempts to parse the date string exactly, according to accepted Dicom format(s).
		/// Will *not* throw an exception if the format is invalid.
		/// </summary>
		/// <param name="dicomDate">the dicom date string</param>
		/// <param name="date">returns the date as a DateTime object</param>
		/// <returns>true on success, false otherwise</returns>
		public static bool Parse(string dicomDate, out DateTime date)
		{
			// This method is used in DicomAttribute Get/TryGet,
			// which allow leading/trailing spaces in the string
			// They are considered valid DICOM date/time.
			if (dicomDate!=null)
				dicomDate = dicomDate.Trim();

			return DateTime.TryParseExact(dicomDate, DicomDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
		}

		/// <summary>
		/// Convert a DateTime object into a DA string
		/// </summary>
		/// <param name="datetime"></param>
		/// <returns>The DICOM formatted string</returns>
		public static string ToDicomString(DateTime datetime)
		{
			return datetime.ToString(DicomDateFormat, CultureInfo.InvariantCulture);
		}
	}
}