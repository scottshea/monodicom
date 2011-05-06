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
using System.Text;

namespace ClearCanvas.Common.Utilities
{
	/// <summary>
	/// A static string helper class.
	/// </summary>
	public static class StringUtilities
	{
		/// <summary>
		/// A delegate used by <see cref="StringUtilities"/> to format output strings.
		/// </summary>
		public delegate string FormatDelegate<T>(T value);

		/// <summary>
		/// Combines the input <paramref name="values"/> into a string, separated by <paramref name="separator"/>,
		/// using the given <paramref name="formatSpecifier"/> to format each entry in the string.
		/// </summary>
		/// <remarks>
		/// <typeparam name="T">Must implement <see cref="IFormattable"/>.</typeparam>
		/// </remarks>
		public static string Combine<T>(IEnumerable<T> values, string separator, string formatSpecifier)
			where T : IFormattable
		{
			return Combine(values, separator, formatSpecifier, null);
		}

		/// <summary>
		/// Combines the input <paramref name="values"/> into a string, separated by <paramref name="separator"/>,
		/// using the given <paramref name="formatSpecifier"/> to format each entry in the string.
		/// </summary>
		/// <remarks>
		/// <typeparam name="T">Must implement <see cref="IFormattable"/>.</typeparam>
		/// </remarks>
		public static string Combine<T>(IEnumerable<T> values, string separator, string formatSpecifier, IFormatProvider formatProvider)
			where T : IFormattable
		{
			return Combine(values, separator,
				delegate(T value)
				{
					if (String.IsNullOrEmpty(formatSpecifier))
						return value.ToString();

					return value.ToString(formatSpecifier, formatProvider);
				});
		}

		/// <summary>
		/// Combines the input <paramref name="values"/> into a string separated by the <paramref name="separator"/>.
		/// </summary>
		/// <remarks>
		/// Empty values are skipped.
		/// </remarks>
		public static string Combine<T>(IEnumerable<T> values, string separator)
		{
			return Combine(values, separator, true);
		}

		/// <summary>
		/// Combines the input <paramref name="values"/> into a string separated by the <paramref name="separator"/>;
		/// empty values are skipped when <paramref name="skipEmptyValues"/> is true.
		/// </summary>
		public static string Combine<T>(IEnumerable<T> values, string separator, bool skipEmptyValues)
		{
			return Combine(values, separator, null, skipEmptyValues);
		}

		/// <summary>
		/// Combines the input <paramref name="values"/> into a string separated by the <paramref name="separator"/> 
		/// and formatted using <paramref name="formatDelegate"/>.
		/// </summary>
		public static string Combine<T>(IEnumerable<T> values, string separator, FormatDelegate<T> formatDelegate)
		{
			return Combine(values, separator, formatDelegate, true);
		}

		/// <summary>
		/// Combines the input <paramref name="values"/> into a string separated by <paramref name="separator"/> 
		/// and formatted using <paramref name="formatDelegate"/>; empty values are skipped when <paramref name="skipEmptyValues"/> is true.
		/// </summary>
		public static string Combine<T>(IEnumerable<T> values, string separator, FormatDelegate<T> formatDelegate, bool skipEmptyValues)
		{
			if (values == null)
				return "";

			if (separator == null)
				separator = "";

			StringBuilder builder = new StringBuilder();
			int count = 0;
			foreach (T value in values)
			{
				string stringValue;
				if (formatDelegate == null)
					stringValue = (value == null) ? "" : value.ToString();
				else
					stringValue = formatDelegate(value) ?? "";

				if (String.IsNullOrEmpty(stringValue) && skipEmptyValues)
					continue;

				if (count++ > 0)
					builder.Append(separator);

				builder.Append(stringValue);
			}

			return builder.ToString();
		}

        /// <summary>
        /// Splits any string into sub-strings using the specified <paramref name="delimiters"/>, 
        /// ignoring delimiters inside double quotes.
        /// </summary>
        /// <remarks>
		/// This is different from the <b>String.Split</b> methods 
		/// as we ignore delimiters inside double quotes.
		/// </remarks>
        /// <param name="text">The string to split.</param>
        /// <param name="delimiters">The characters to split on.</param>
        /// <returns></returns>
        public static string[] SplitQuoted(string text, string delimiters)
        {
            ArrayList res = new ArrayList();

            StringBuilder tokenBuilder = new StringBuilder();
            bool insideQuote = false;

            foreach (char c in text.ToCharArray())
            {
                if (!insideQuote && delimiters.Contains(c.ToString()))
                {
                    res.Add(tokenBuilder.ToString());
                    tokenBuilder.Length = 0;
                }
                else if (c.Equals('\"'))
                {
                    insideQuote = !insideQuote;
                }
                else
                {
                    tokenBuilder.Append(c);
                }
            }

            // add the last token
            res.Add(tokenBuilder.ToString());

            return (string[])res.ToArray(typeof(string)); ;
        }

        /// <summary>
        /// Converts an empty string to a null string, otherwise returns the argument unchanged.
        /// </summary>
        public static string NullIfEmpty(string s)
        {
            return string.IsNullOrEmpty(s) ? null : s;
        }

        /// <summary>
        /// Converts a null argument to an empty string, otherwise returns the argument unchanged.
        /// </summary>
        public static string EmptyIfNull(string s)
        {
            return s ?? "";
        }
    }
}
