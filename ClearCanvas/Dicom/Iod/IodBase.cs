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

namespace ClearCanvas.Dicom.Iod
{
    /// <summary>
    /// Abstract base class for an Iod and Modules, which is a strongly typed wrapper to attributes in a dicom attribute collection.
    /// </summary>
    public abstract class IodBase
    {

        #region Private Variables
        /// <summary>
        /// Contains dicom attribute collection which contains all the dicom tags
        /// </summary>
        IDicomAttributeProvider _dicomAttributeProvider;

        /// <summary>
        /// Contains a dictionary of module Iods.  Subclassed Iods can use dictionary for a lazy-loaded way of loading modules, this way
        /// if the <see cref="DicomAttributeProvider"/> gets updated, the new attribute collection will be used (lazy loaded).
        /// </summary>
        private Dictionary<Type, IodBase> _moduleIods = new Dictionary<Type, IodBase>();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="IodBase"/> class.
        /// </summary>
        protected IodBase()
            :this(new DicomAttributeCollection())
        {
        }

		protected IodBase(IDicomAttributeProvider dicomAttributeProvider)
		{
			_dicomAttributeProvider = dicomAttributeProvider;
		}

    	#endregion

        #region Protected Properties
        /// <summary>
        /// Gets the dicom attribute collection.
        /// </summary>
        /// <value>The dicom attribute collection.</value>
        public IDicomAttributeProvider DicomAttributeProvider
        {
            get { return _dicomAttributeProvider; }
            set 
            { 
                _dicomAttributeProvider = value;
                // Clear the moduleIods so next time accessed it gets rebuilt cos of lazy loading
                _moduleIods.Clear();
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the specified <paramref name="dicomTag"/>'s attribute value to be null.
        /// </summary>
        /// <param name="dicomTag">The dicom tag.</param>
        public void SetAttributeNull(DicomTag dicomTag)
        {
            this.DicomAttributeProvider[dicomTag].SetNullValue();
        }

        /// <summary>
        /// Sets the specified <paramref name="dicomTag"/>'s attribute value to be null.
        /// </summary>
        /// <param name="dicomTag">The dicom tag.</param>
        public void SetAttributeNull(uint dicomTag)
        {
            this.DicomAttributeProvider[dicomTag].SetNullValue();
        }

        /// <summary>
        /// Sets the list of dicom tags' attributes null.
        /// </summary>
        /// <param name="dicomTags">The dicom tags.</param>
        public void SetAttributesNull(IList<DicomTag> dicomTags)
        {
            foreach (DicomTag dicomTag in dicomTags)
                SetAttributeNull(dicomTag);

        }

        /// <summary>
        /// Sets the list of dicom tags' attributes null.
        /// </summary>
        /// <param name="dicomTags">The dicom tags.</param>
        public void SetAttributesNull(IList<uint> dicomTags)
        {
            foreach (uint dicomTag in dicomTags)
                SetAttributeNull(dicomTag);

        }

        #endregion

        #region Protected Methods
        /// <summary>
        /// Gets the module iod for the specified T.  This is to be used by a subclassed IO for lazy loading of a Module IOD within that IOD.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetModuleIod<T>()
            where T : IodBase, new()
        {
            if (!_moduleIods.ContainsKey(typeof(T)))
            {
                T newT = new T();
                (newT as IodBase).DicomAttributeProvider = DicomAttributeProvider;

                _moduleIods.Add(typeof(T), newT);
            }
            return _moduleIods[typeof(T)] as T;
        }
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Parses an enum value for the enum type T, automatically converting to Pascal if necessary since enum names don't have spaces.  Returns <paramref name="defaultValue"/> if string not found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static T ParseEnum<T>(string input, T defaultValue)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            // First check for empty/null and default value to avoid exception performance hit
            if (String.IsNullOrEmpty(input) || input.ToUpperInvariant() == defaultValue.ToString().ToUpperInvariant())
                return defaultValue;

            try
            {
				if (input.Contains(" "))
					input = input.Replace(" ", "");
                return (T) Enum.Parse(typeof(T), input, true);
            }
            catch (Exception)
            {
            }
            return defaultValue;
        }

        /// <summary>
        /// Sets the dicom attribute value from enum value.
        /// </summary>
        /// <param name="dicomAttribute">The dicom attribute.</param>
        /// <param name="value">The value.</param>
        public static void SetAttributeFromEnum(DicomAttribute dicomAttribute, object value)
        {
            SetAttributeFromEnum(dicomAttribute, value, false);
        }

        /// <summary>
        /// Sets the dicom attribute value from enum.  Sets it to upper case as per dicom Standard.  
        /// If <paramref name="formatFromPascal"/> is true, then it formats the <paramref name="value"/> from Pascal - ie, MammoClear would 
        /// be set as MAMMO CLEAR .
        /// </summary>
        /// <param name="dicomAttribute">The dicom attribute.</param>
        /// <param name="value">The value.</param>
        /// <param name="formatFromPascal">if set to <c>true</c> [format from pascal].</param>
        public static void SetAttributeFromEnum(DicomAttribute dicomAttribute, object value, bool formatFromPascal)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()) || String.Compare(value.ToString(), "None", StringComparison.OrdinalIgnoreCase) == 0 || String.Compare(value.ToString(), "Unknown", StringComparison.OrdinalIgnoreCase) == 0)
                dicomAttribute.SetNullValue();
            else
            {
                if (formatFromPascal)
                    value = FormatFromPascal(value.ToString());
                dicomAttribute.SetStringValue(value.ToString().ToUpperInvariant());
            }
        }

        /// <summary>
        /// Formats a string from pascal notation.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string FormatFromPascal(string value)
        {
            if (value == null)
                return null;

            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (char c in value)
            {
                if (i > 0 && char.IsUpper(c) && !IsPrevCharWhiteSpace(value, i) && !char.IsWhiteSpace(c) && !IsPrevCharUpper(value, i))
                    sb.AppendFormat(" {0}", c.ToString());
                else if (char.IsWhiteSpace(c) && IsNextCharWhiteSpace(value, i))
                    sb.Append(""); // don't append 2 spaces
                else
                    sb.Append(c);
                i++;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Determines whether [is prev char white space] [the specified value].  Helper function for FormatFromPascal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="currentIndex">Index of the current.</param>
        /// <returns>
        /// 	<c>true</c> if [is prev char white space] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsPrevCharWhiteSpace(string value, int currentIndex)
        {
            return currentIndex > 0 && char.IsWhiteSpace(value.Substring(currentIndex - 1, 1).ToCharArray()[0]);
        }

        /// <summary>
        /// Determines whether [is next char white space] [the specified value].  Helper function for FormatFromPascal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="currentIndex">Index of the current.</param>
        /// <returns>
        /// 	<c>true</c> if [is next char white space] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsNextCharWhiteSpace(string value, int currentIndex)
        {
            return currentIndex < value.Length && char.IsWhiteSpace(value.Substring(currentIndex + 1, 1).ToCharArray()[0]);
        }

        /// <summary>
        /// Determines whether [is prev char upper] [the specified value].  Helper function for FormatFromPascal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="currentIndex">Index of the current.</param>
        /// <returns>
        /// 	<c>true</c> if [is prev char upper] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsPrevCharUpper(string value, int currentIndex)
        {
            return currentIndex > 0 && char.IsUpper(value.Substring(currentIndex - 1, 1).ToCharArray()[0]);
        }
        #endregion


    }
}
