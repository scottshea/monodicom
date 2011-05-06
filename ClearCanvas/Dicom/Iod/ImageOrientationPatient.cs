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
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.Utilities;

namespace ClearCanvas.Dicom.Iod
{
	/// <summary>
	/// The ImageOrientationPatient class is quite simple, basically providing a centralized place to store the
	/// row/column direction cosines from the Dicom Header.  One additional piece of functionality is the primary
	/// and secondary row/column directions, which are transformed (using the cosines) into more meaningful 
	/// values (Anterior, Left, Head, etc).
	/// 
	/// The components of each of the cosine vectors (row/column, x,y,z) corresponds to the patient based coordinate
	/// system as follows (e.g. it is a right-handed system): 
	///
	/// +x --> Left,			-x --> Right
	/// +y --> Posterior,		-y --> Anterior 
	/// +z --> Head,			-z --> Foot
	/// 
	/// The primary and secondary directions of a cosine vector correspond directly to the 2 largest
	/// values in the cosine vector, disregarding the sign.  The sign determines the direction along 
	/// a particular axis in the patient based system as described above.
	///
	/// The row cosine vector completely describes the direction, in the patient based system, of the first row
	/// in the image (increasing x).  Similarly, the column cosine vector completely describes the 
	/// direction of the first column in the image in the patient based system.
	/// </summary>
	public class ImageOrientationPatient : IEquatable<ImageOrientationPatient>
	{
		/// <summary>
		/// Defines the direction of the axes in the patient coordinate system.
		/// </summary>
		public enum Directions { None = 0, Left = 1, Right = -1, Posterior = 2, Anterior = -2, Head = 3, Foot = -3 };

		#region Private Members

		private double _rowX;
		private double _rowY;
		private double _rowZ;
		private double _columnX;
		private double _columnY;
		private double _columnZ;

		private int _primaryRowDirection;
		private int _secondaryRowDirection;
		private int _primaryColumnDirection;
		private int _secondaryColumnDirection;

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public ImageOrientationPatient(double rowX, double rowY, double rowZ, double columnX, double columnY, double columnZ)
		{
			_rowX = rowX;
			_rowY = rowY;
			_rowZ = rowZ;
			_columnX = columnX;
			_columnY = columnY;
			_columnZ = columnZ;

			Recalculate();
		}

		/// <summary>
		/// Protected constructor.
		/// </summary>
		protected ImageOrientationPatient()
		{
			_rowX = _rowY = _rowZ = _columnX = _columnY = _columnZ = 0;
			Recalculate();
		}

		#region Public Properties

		/// <summary>
		/// Gets whether or not this object represents a null value.
		/// </summary>
		public bool IsNull
		{
			get { return	(_rowX == 0 && _rowY == 0 && _rowZ == 0) || 
							(_columnX == 0 && _columnY == 0 && _columnZ == 0) ; }
		}

		/// <summary>
		/// Gets the x component of the direction cosine for the first row in the image.
		/// </summary>
		public virtual double RowX
		{
			get { return _rowX; }
			protected set
			{
				if (_rowX != value)
				{
					_rowX = value;
					Recalculate();
				}
			}
		}

		/// <summary>
		/// Gets the y component of the direction cosine for the first row in the image.
		/// </summary>
		public virtual double RowY
		{
			get { return _rowY; }
			protected set
			{
				if (_rowY != value)
				{
					_rowY = value;
					Recalculate();
				}
			}
		}

		/// <summary>
		/// Gets the z component of the direction cosine for the first row in the image.
		/// </summary>
		public virtual double RowZ
		{
			get { return _rowZ; }
			protected set
			{
				if (_rowZ != value)
				{
					_rowZ = value;
					Recalculate();
				}
			}
		}

		/// <summary>
		/// Gets the x component of the direction cosine for the first column in the image.
		/// </summary>
		public virtual double ColumnX
		{
			get { return _columnX; }
			protected set
			{
				if (_columnX != value)
				{
					_columnX = value;
					Recalculate();
				}
			}
		}

		/// <summary>
		/// Gets the y component of the direction cosine for the first column in the image.
		/// </summary>
		public virtual double ColumnY
		{
			get { return _columnY; }
			protected set
			{
				if (_columnY != value)
				{
					_columnY = value;
					Recalculate();
				}
			}
		}

		/// <summary>
		/// Gets the z component of the direction cosine for the first column in the image.
		/// </summary>
		public virtual double ColumnZ
		{
			get { return _columnZ; }
			protected set
			{
				if (_columnZ != value)
				{
					_columnZ = value;
					Recalculate();
				}
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets a string suitable for direct insertion into a <see cref="DicomAttributeMultiValueText"/> attribute.
		/// </summary>
		public override string ToString()
		{
			return String.Format(@"{0:G12}\{1:G12}\{2:G12}\{3:G12}\{4:G12}\{5:G12}", _rowX, _rowY, _rowZ, _columnX, _columnY, _columnZ);
		}

		/// <summary>
		/// Creates an <see cref="ImageOrientationPatient"/> object from a dicom multi-valued string.
		/// </summary>
		/// <returns>
		/// Null if there are not exactly six parsed values in the input string.
		/// </returns>
		public static ImageOrientationPatient FromString(string multiValuedString)
		{
			double[] values;
			if (DicomStringHelper.TryGetDoubleArray(multiValuedString, out values) && values.Length == 6)
				return new ImageOrientationPatient(values[0], values[1], values[2], values[3], values[4], values[5]);

			return null;
		}

		/// <summary>
		/// Gets the primary direction, in terms of the Patient based coordinate system, of the first row of the Image (increasing x).
		/// </summary>
		/// <param name="opposingDirection">indicates the opposite direction to the primary direction should be returned.
		/// For example, if the primary direction is Anterior, then Posterior will be returned if this parameter is true.</param>
		/// <returns>the direction, in terms of the Patient based coordinate system</returns>
		public Directions GetPrimaryRowDirection(bool opposingDirection)
		{
			return (Directions)(_primaryRowDirection * (opposingDirection ? -1 : 1));
		}

		/// <summary>
		/// Gets the primary direction, in terms of the Patient based coordinate system, of the first column of the Image (increasing y).
		/// </summary>
		/// <param name="opposingDirection">indicates the opposite direction to the primary direction should be returned.
		/// For example, if the primary direction is Anterior, then Posterior will be returned if this parameter is true.</param>
		/// <returns>the direction, in terms of the Patient based coordinate system</returns>
		public Directions GetPrimaryColumnDirection(bool opposingDirection)
		{
			return (Directions)(_primaryColumnDirection * (opposingDirection ? -1 : 1));
		}

		/// <summary>
		/// Gets the secondary direction, in terms of the Patient based coordinate system, of the first row of the Image (increasing x).
		/// </summary>
		/// <param name="opposingDirection">indicates the opposite direction to the secondary direction should be returned.
		/// For example, if the secondary direction is Anterior, then Posterior will be returned if this parameter is true.</param>
		/// <returns>the direction, in terms of the Patient based coordinate system</returns>
		public Directions GetSecondaryRowDirection(bool opposingDirection)
		{
			return (Directions)(_secondaryRowDirection * (opposingDirection ? -1 : 1));
		}

		/// <summary>
		/// Gets the secondary direction, in terms of the Patient based coordinate system, of the first row of the Image (increasing x).
		/// </summary>
		/// <param name="opposingDirection">indicates the opposite direction to the secondary direction should be returned.
		/// For example, if the secondary direction is Anterior, then Posterior will be returned if this parameter is true.</param>
		/// <param name="degreesTolerance">Specifies the angular tolerance in degrees. If the secondary directional cosine
		/// does not exceed this value, then the result will be <see cref="Directions.None"/>.</param>
		/// <returns>the direction, in terms of the Patient based coordinate system</returns>
		public Directions GetSecondaryRowDirection(bool opposingDirection, double degreesTolerance)
		{
			// JY: Note that the same tolerance functionality is purposefully not available to the primary direction
			if (degreesTolerance < 0 || degreesTolerance > 10)
				throw new ArgumentOutOfRangeException("degreesTolerance", degreesTolerance, "Value must be between 0 and 10.");
			if (_secondaryRowDirection == 0)
				return Directions.None;

			double[] rowCosines = new double[] {_rowX, _rowY, _rowZ};
			double secondaryCosine = rowCosines[Math.Abs(_secondaryRowDirection) - 1];

			// report no secondary direction if the secondary cosine is within degrees of 90 - that is, nearly perpendicular to the given direction
			if (90 - 180*Math.Acos(Math.Abs(secondaryCosine))/Math.PI < degreesTolerance)
				return Directions.None;
			return this.GetSecondaryRowDirection(opposingDirection);
		}

		/// <summary>
		/// Gets the secondary direction, in terms of the Patient based coordinate system, of the first column of the Image (increasing y).
		/// </summary>
		/// <param name="opposingDirection">indicates the opposite direction to the secondary direction should be returned.
		/// For example, if the secondary direction is Anterior, then Posterior will be returned if this parameter is true.</param>
		/// <returns>the direction, in terms of the Patient based coordinate system</returns>
		public Directions GetSecondaryColumnDirection(bool opposingDirection)
		{
			return (Directions)(_secondaryColumnDirection * (opposingDirection ? -1 : 1));
		}

		/// <summary>
		/// Gets the secondary direction, in terms of the Patient based coordinate system, of the first column of the Image (increasing y).
		/// </summary>
		/// <param name="opposingDirection">indicates the opposite direction to the secondary direction should be returned.
		/// For example, if the secondary direction is Anterior, then Posterior will be returned if this parameter is true.</param>
		/// <param name="degreesTolerance">Specifies the angular tolerance in degrees. If the secondary directional cosine
		/// does not exceed this value, then the result will be <see cref="Directions.None"/>.</param>
		/// <returns>the direction, in terms of the Patient based coordinate system</returns>
		public Directions GetSecondaryColumnDirection(bool opposingDirection, double degreesTolerance)
		{
			// JY: Note that the same tolerance functionality is purposefully not available to the primary direction
			if (degreesTolerance < 0 || degreesTolerance > 10)
				throw new ArgumentOutOfRangeException("degreesTolerance", degreesTolerance, "Value must be between 0 and 10.");
			if (_secondaryColumnDirection == 0)
			    return Directions.None;

			double[] columnCosines = new double[] {_columnX, _columnY, _columnZ};
			double secondaryCosine = columnCosines[Math.Abs(_secondaryColumnDirection) - 1];

			// report no secondary direction if the secondary cosine is within degrees of 90 - that is, nearly perpendicular to the given direction
			if (90 - 180 * Math.Acos(Math.Abs(secondaryCosine)) / Math.PI < degreesTolerance)
				return Directions.None;
			return this.GetSecondaryColumnDirection(opposingDirection);
		}

		#region IEquatable<ImageOrientationPatient> Members

		public bool Equals(ImageOrientationPatient other)
		{
			if (other == null)
				return false;

			return other._rowX == _rowX && other._rowY == _rowY && other._rowZ == _rowZ &&
			       other._columnX == _columnX && other._columnY == _columnY && other._columnZ == _columnZ;
		}

		#endregion

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			return this.Equals(obj as ImageOrientationPatient);
		}

		public bool EqualsWithinTolerance(ImageOrientationPatient other, float withinTolerance)
		{
			if (other == null)
				return false;

			return EqualsWithinTolerance(other._rowX, _rowX, withinTolerance) && 
					EqualsWithinTolerance(other._rowY, _rowY, withinTolerance) &&
					EqualsWithinTolerance(other._rowZ, _rowZ, withinTolerance) &&
					EqualsWithinTolerance(other._columnX, _columnX, withinTolerance) &&
					EqualsWithinTolerance(other._columnY, _columnY, withinTolerance) &&
					EqualsWithinTolerance(other._columnZ, _columnZ, withinTolerance);
		}

		private static bool EqualsWithinTolerance(double d1, double d2, float tolerance)
		{
			return Math.Abs(d1 - d2) < tolerance;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion

		/// <summary>
		/// Recalculates the primary/secondary directions (in patient based system) for the first row and first column.
		/// </summary>
		private void Recalculate()
		{
			double[] rowCosines = new double[] { _rowX, _rowY, _rowZ };
			double[] columnCosines = new double[] { _columnX, _columnY, _columnZ };

			int[] rowCosineSortedIndices = BubbleSortCosineIndices(rowCosines);
			int[] columnCosineSortedIndices = BubbleSortCosineIndices(columnCosines);

			SetDirectionValue(ref _primaryRowDirection, rowCosines[rowCosineSortedIndices[0]], rowCosineSortedIndices[0]);
			SetDirectionValue(ref _secondaryRowDirection, rowCosines[rowCosineSortedIndices[1]], rowCosineSortedIndices[1]);
			SetDirectionValue(ref _primaryColumnDirection, columnCosines[columnCosineSortedIndices[0]], columnCosineSortedIndices[0]);
			SetDirectionValue(ref _secondaryColumnDirection, columnCosines[columnCosineSortedIndices[1]], columnCosineSortedIndices[1]);
		}

		/// <summary>
		/// Sets one of the member primary/secondary direction variables.
		/// </summary>
		/// <param name="member">the member to set</param>
		/// <param name="cosineValue">the cosine value</param>
		/// <param name="cosineIndex">the index of the cosine value in the original direction cosine vector</param>
		private static void SetDirectionValue(ref int member, double cosineValue, int cosineIndex)
		{
			member = 0;
			if (Math.Abs(cosineValue) > float.Epsilon)
				member = (cosineIndex + 1) * Math.Sign(cosineValue);
		}

		/// <summary>
		/// Bubble sorts an array of cosines in descending order (largest to smallest), ignoring the sign.
		/// This helps to determine the primary/secondary directions for the cosines.
		/// </summary>
		/// <param name="cosineArray">the array of cosines (row or column)</param>
		/// <returns>an array of indices into the input array (cosineArray), that when applied would sort the cosines appropriately.</returns>
		private static int[] BubbleSortCosineIndices(double[] cosineArray)
		{
			int[] indexArray = new int[] { 0, 1, 2 };

			for (int i = 2; i > 0; --i)
			{
				for (int j = 0; j < i; ++j)
				{
					if (Math.Abs(cosineArray[indexArray[j + 1]]) > Math.Abs(cosineArray[indexArray[j]]))
					{
						int tempint = indexArray[j];
						indexArray[j] = indexArray[j + 1];
						indexArray[j + 1] = tempint;
					}
				}
			}

			return indexArray;
		}
	}
}