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
	/// Represents the position of the image pixel at (0, 0) in the patient coordinate system.
	/// </summary>
	public class ImagePositionPatient : IEquatable<ImagePositionPatient>
	{
		#region Private Members
		
		private double _x;
		private double _y;
		private double _z;

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public ImagePositionPatient(double x, double y, double z)
		{
			_x = x;
			_y = y;
			_z = z;
		}

		/// <summary>
		/// Protected constructor.
		/// </summary>
		protected ImagePositionPatient()
		{
		}

		#region Public Properties

		/// <summary>
		/// Gets whether or not this object represents a null value.
		/// </summary>
		public bool IsNull
		{
			get { return _x == 0 && _y == 0 && _z == 0; }	
		}

		/// <summary>
		/// Gets the x component.
		/// </summary>
		public virtual double X
		{
			get { return _x; }
			protected set { _x = value; }
		}

		/// <summary>
		/// Gets the y component.
		/// </summary>
		public virtual double Y
		{
			get { return _y; }
			protected set { _y = value; }
		}

		/// <summary>
		/// Gets the z component.
		/// </summary>
		public virtual double Z
		{
			get { return _z; }
			protected set { _z = value; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets a string suitable for direct insertion into a <see cref="DicomAttributeMultiValueText"/> attribute.
		/// </summary>
		public override string ToString()
		{
			return String.Format(@"{0:G12}\{1:G12}\{2:G12}", _x, _y, _z);
		}

		/// <summary>
		/// Creates an <see cref="ImagePositionPatient"/> object from a dicom multi-valued string.
		/// </summary>
		/// <returns>
		/// Null if there are not exactly 3 parsed values in the input string.
		/// </returns>
		public static ImagePositionPatient FromString(string multiValuedString)
		{
			double[] values;
			if (DicomStringHelper.TryGetDoubleArray(multiValuedString, out values) && values.Length == 3)
					return new ImagePositionPatient(values[0], values[1], values[2]);

			return null;
		}

		#region IEquatable<ImagePositionPatient> Members

		public bool Equals(ImagePositionPatient other)
		{
			if (other == null)
				return false;

			return other._x	== _x && other._y == _y && other._z == _z;
		}

		#endregion

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			return Equals(obj as ImagePositionPatient);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		
		#endregion
	}
}
