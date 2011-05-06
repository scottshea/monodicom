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
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Dicom.Iod
{
	/// TODO: add functionality to create a component group from the individual components.
	/// 
	/// <summary>
	/// Represents one component group of a person name (VR PN).
	/// </summary>
	/// <remarks>
	/// This class assumes that the ComponentGroup has already been decoded 
	/// from any native character set into a Unicode string
	/// </remarks>
    public class ComponentGroup
    {
		#region Private Fields

		private readonly string _rawString;
		private string _familyName;
		private string _givenName;
		private string _middleName;
		private string _prefix;
		private string _suffix;
		
		#endregion

		/// <summary>
        /// Constructor.
        /// </summary>
		public ComponentGroup(string componentGroupString)
        {
            _rawString = componentGroupString;
            BreakApartIntoComponents();
        }

		#region Public Properties

		/// <summary>
		/// Gets whether or not this <see cref="ComponentGroup"/> is empty.
		/// </summary>
		public bool IsEmpty
		{
			get { return _rawString == null || _rawString == String.Empty; }
		}

		public string Suffix
		{
			get { return _suffix; }
		}

		public string Prefix
		{
			get { return _prefix; }
		}

		public string MiddleName
		{
			get { return _middleName; }
		}

		public string GivenName
		{
			get { return _givenName; }
		}

		public string FamilyName
		{
			get { return _familyName; }
		}

		#endregion

		/// <summary>
		/// Creates and returns an empty <see cref="ComponentGroup"/>.
		/// </summary>
		/// <returns></returns>
		public static ComponentGroup GetEmptyComponentGroup()
		{
			return new ComponentGroup("");
		}

		/// <summary>
		/// Gets the entire component group as a string.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return _rawString;
		}

		/// <summary>
		/// Converts a <see cref="ComponentGroup"/> to a string.
		/// </summary>
		public static implicit operator String(ComponentGroup componentGroup)
		{
			return componentGroup.ToString();
		}

		private void BreakApartIntoComponents()
		{
			string[] components = _rawString.Split('^');

			if (components.GetUpperBound(0) >= 0 && components[0] != string.Empty)
			{
				_familyName = components[0];
			}

			if (components.GetUpperBound(0) > 0 && components[1] != string.Empty)
			{
				_givenName = components[1];
			}

			if (components.GetUpperBound(0) > 1 && components[2] != string.Empty)
			{
				_middleName = components[2];
			}

			if (components.GetUpperBound(0) > 2 && components[3] != string.Empty)
			{
				_prefix = components[3];
			}

			if (components.GetUpperBound(0) > 3 && components[4] != string.Empty)
			{
				_suffix = components[4];
			}
		}



        static private bool AreSame(string x, string y, PersonNameComparisonOptions options)
        {
            if (String.IsNullOrEmpty(x))
                return String.IsNullOrEmpty(y);
            else
            {
                switch (options)
                {
                    case PersonNameComparisonOptions.CaseSensitive:
                        return x.Equals(y);
                    case PersonNameComparisonOptions.CaseInsensitive:
                        return x.Equals(y, StringComparison.InvariantCultureIgnoreCase);
                }

                return false;
            }

        }

        #region IEquatable<ComponentGroup> Members

        /// <summary>
        /// Returns a value indicating whether two <see cref="ComponentGroup"/> are the same.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public bool AreSame(ComponentGroup other, PersonNameComparisonOptions options)
        {
            if (other == null)
                return false;

            if (IsEmpty)
                return other.IsEmpty;
            else
            {
                return AreSame(FamilyName, other.FamilyName, options) &&
                       AreSame(GivenName, other.GivenName, options) &&
                       AreSame(MiddleName, other.MiddleName, options) &&
                       AreSame(Prefix, other.Prefix, options) &&
                       AreSame(Suffix, other.Suffix, options);                
            }

        }


	    #endregion
    }
}
