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
using System.ComponentModel;
using ClearCanvas.Dicom.Utilities;

namespace ClearCanvas.Dicom.DataStore
{
	internal static class Convert
	{
		internal static IEnumerable<T> Cast<T>(IEnumerable original)
		{
			foreach (T item in original)
				yield return item;
		}

		private static string ToString(object value, TypeConverter converter)
		{
			if (value is string)
				return (string)value;
			else
				return converter.ConvertToInvariantString(value);
		}

		public static string[] ToStringArray(object value, TypeConverter converter)
		{
			if (value == null)
				return new string[]{ };

			if (value.GetType().IsArray)
			{
				Array array = value as Array;
				if (array == null)
					return new string[]{ };

				string[] stringArray = new string[array.Length];
				int i = 0;
				foreach (object arrayValue in array)
					stringArray[i++] = ToString(arrayValue, converter);

				return stringArray;
			}
			else if (value is string)
			{
				//Assume strings are (potentially) multi-valued.  If they're not, then this has no effect anyway.
				return DicomStringHelper.GetStringArray(((string)value) ?? "");
			}
			else
			{
				return new string[] { ToString(value, converter) };
			}
		}
	}
}
