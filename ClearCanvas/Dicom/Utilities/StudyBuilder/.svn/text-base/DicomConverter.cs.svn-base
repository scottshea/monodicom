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

namespace ClearCanvas.Dicom.Utilities.StudyBuilder
{
	/// <summary>
	/// Internal data conversion class for dates, times, datetimes, and patient sex codestrings
	/// </summary>
	internal static class DicomConverter
	{
		/// <summary>
		/// Combines separate date and time values into a single datetime, using a default value if both components are null
		/// </summary>
		/// <param name="date"></param>
		/// <param name="time"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static DateTime GetDateTime(DateTime? date, DateTime? time, DateTime defaultValue)
		{
			if (date.HasValue)
			{
				if (time.HasValue)
					return date.Value.Add(time.Value.TimeOfDay);
				else
					return date.Value;
			}
			else
			{
				if (time.HasValue)
					return defaultValue.Add(time.Value.TimeOfDay);
				else
					return defaultValue;
			}
		}

		/// <summary>
		/// Combines separate date and time values into a single datetime, using null if both components are null 
		/// </summary>
		/// <param name="date"></param>
		/// <param name="time"></param>
		/// <returns></returns>
		public static DateTime? GetDateTime(DateTime? date, DateTime? time)
		{
			if (date.HasValue)
			{
				if (time.HasValue)
					return date.Value.Add(time.Value.TimeOfDay);
				else
					return date.Value;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Gets a <see cref="PatientSex"/> enumeration based on a CS attribute value, using <see cref="PatientSex.Undefined"/> for any unrecognized code strings.
		/// </summary>
		/// <param name="codestring"></param>
		/// <returns></returns>
		public static PatientSex GetSex(string codestring)
		{
			if (codestring == null)
				return PatientSex.Undefined;
			switch (codestring.PadRight(1).Substring(0, 1).ToUpperInvariant())
			{
				case "M":
					return PatientSex.Male;
				case "F":
					return PatientSex.Female;
				case "O":
					return PatientSex.Other;
				default:
					return PatientSex.Undefined;
			}
		}

		/// <summary>
		/// Gets a patient sex CS string based on a <see cref="PatientSex"/> enumeration, using an empty string for <see cref="PatientSex.Undefined"/>
		/// </summary>
		/// <param name="sex"></param>
		/// <returns></returns>
		public static string SetSex(PatientSex sex)
		{
			switch (sex)
			{
				case PatientSex.Male:
					return "M";
				case PatientSex.Female:
					return "F";
				case PatientSex.Other:
					return "O";
				case PatientSex.Undefined:
				default:
					return "";
			}
		}

		/// <summary>
		/// Temporary workaround code in place of DicomAttribute.SetDateTime(0, value) which can throw a IndexOutOfRangeException if attribute is null
		/// </summary>
		/// <param name="attrib"></param>
		/// <param name="value"></param>
		public static void SetDate(DicomAttribute attrib, DateTime? value)
		{
			//TODO - replace this with DicomAttribute.SetDateTime(0, value) when it is fixed (ticket #1411)
			if(value.HasValue)
				attrib.SetString(0, DateParser.ToDicomString(value.Value));
			else
				attrib.SetNullValue();
		}

		/// <summary>
		/// Temporary workaround code in place of DicomAttribute.SetDateTime(0, value) which can throw a IndexOutOfRangeException if attribute is null
		/// </summary>
		/// <param name="attrib"></param>
		/// <param name="value"></param>
		public static void SetTime(DicomAttribute attrib, DateTime? value) {
			//TODO - replace this with DicomAttribute.SetDateTime(0, value) when it is fixed (ticket #1411)
			if (value.HasValue)
				attrib.SetString(0, string.Format(TimeParser.DicomFullTimeFormat, value.Value));
			else
				attrib.SetNullValue();
		}

		/// <summary>
		/// Temporary workaround code in place of DicomAttribute.SetInt32(0, value) which can throw a IndexOutOfRangeException if attribute is null
		/// </summary>
		/// <param name="attrib"></param>
		/// <param name="value"></param>
		public static void SetInt32(DicomAttribute attrib, int value)
		{
			//TODO - replace this with DicomAttribute.SetInt32(0, value) when it is fixed (ticket #1411)
			attrib.SetString(0, value.ToString());
		}
	}
}