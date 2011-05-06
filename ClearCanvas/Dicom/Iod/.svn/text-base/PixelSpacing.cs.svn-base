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
	/// Represents the pixel spacing of an image.
	/// </summary>
	public class PixelSpacing : IEquatable<PixelSpacing>
    {
		#region Private Members
		
		double _row;
		double _column;

		#endregion
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public PixelSpacing(double row, double column)
		{
			_row = row;
			_column = column;
		}

		/// <summary>
		/// Protected constructor.
		/// </summary>
		protected PixelSpacing()
		{
		}

		#region Public Properties

		/// <summary>
		/// Gets whether or not this object represents a null value.
		/// </summary>
		public bool IsNull
		{
			get { return _row == 0 || _column == 0; }
		}

		/// <summary>
		/// Gets the spacing of the rows in the image, in millimetres.
		/// </summary>
		public virtual double Row
        {
            get { return _row; }
			protected set { _row = value; }
        }

		/// <summary>
		/// Gets the spacing of the columns in the image, in millimetres.
		/// </summary>
		public virtual double Column
        {
            get { return _column; }
			protected set { _column = value; }
		}

		/// <summary>
		/// Gets the pixel aspect ratio as a floating point value, or zero if <see cref="IsNull"/> is true.
		/// </summary>
		/// <remarks>
		/// The aspect ratio of a pixel is defined as the ratio of it's vertical and horizontal
		/// size(s), or <see cref="Row"/> divided by <see cref="Column"/>.
		/// </remarks>
		public double AspectRatio
		{
			get
			{
				if (IsNull)
					return 0;
				
				return Row / Column;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets a string suitable for direct insertion into a <see cref="DicomAttributeMultiValueText"/> attribute.
		/// </summary>
		public override string ToString()
		{
			return String.Format(@"{0:G12}\{1:G12}", _row, _column);
		}

		public static PixelSpacing FromString(string multiValuedString)
		{
			double[] values;
			if (DicomStringHelper.TryGetDoubleArray(multiValuedString, out values) && values.Length == 2)
					return new PixelSpacing(values[0], values[1]);

			return null;
		}

		#region IEquatable<PixelSpacing> Members

		public bool Equals(PixelSpacing other)
		{
			if (other == null)
				return false;

			return _row == other._row && _column == other._column;
		}

		#endregion

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			return this.Equals(obj as PixelSpacing);
		}

		/// <summary>
		/// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion
	}
}
