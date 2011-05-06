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
using ClearCanvas.Dicom.Utilities;

namespace ClearCanvas.Dicom.Iod
{
	/// <summary>
	/// Represents the orientation of the image in the patient using dicom enumerated values
	/// to indicate the direction of the first row and column in the image.
	/// </summary>
	public class PatientOrientation : IEquatable<PatientOrientation>
	{
		#region Private Members

		private string _row;
		private string _column;

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public PatientOrientation(string row, string column)
		{
			_row = row ?? "";
			_column = column ?? "";
		}

		/// <summary>
		/// Protected constructor.
		/// </summary>
		protected PatientOrientation()
			: this("", "")
		{
		}

		#region Public Properties

		/// <summary>
		/// Gets whether or not this <see cref="PatientOrientation"/> object is empty.
		/// </summary>
		public bool IsEmpty
		{
			get { return _row == "" && _column == ""; }	
		}

		/// <summary>
		/// Gets the direction of the first row in the image.
		/// </summary>
		public virtual string Row
		{
			get { return _row; }
			protected set { _row = value ?? ""; }
		}

		/// <summary>
		/// Gets the direction of the first column in the image.
		/// </summary>
		public virtual string Column
		{
			get { return _column; }
			protected set { _column = value ?? ""; }
		}

		#endregion

		/// <summary>
		/// Gets a string suitable for direct insertion into a <see cref="DicomAttributeMultiValueText"/> attribute.
		/// </summary>
		public override string ToString()
		{
			return String.Format(@"{0}\{1}", _row, _column);
		}

		/// <summary>
		/// Creates a <see cref="PatientOrientation"/> object from a dicom multi-valued string.
		/// </summary>
		/// <returns>
		/// Null if there are not exactly 2 parsed values in the input string.
		/// </returns>
		public static PatientOrientation FromString(string multiValuedString)
		{
			string[] values = DicomStringHelper.GetStringArray(multiValuedString);
			if (values != null && values.Length == 2)
				return new PatientOrientation(values[0], values[1]);

			return null;
		}

		#region IEquatable<PatientOrientation> Members

		public bool Equals(PatientOrientation other)
		{
			if (other == null)
				return false;

			return other._row == _row && other._column == _column;
		}

		#endregion

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			return Equals(obj as PatientOrientation);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
