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
using ClearCanvas.Dicom.Iod.Macros;
using ClearCanvas.Dicom.Iod.Sequences;

namespace ClearCanvas.Dicom.Iod.Modules
{
	/// <summary>
	/// SpatialTransform Module
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.10.6 (Table C.10-6)</remarks>
	public class SpatialTransformModuleIod : IodBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SpatialTransformModuleIod"/> class.
		/// </summary>	
		public SpatialTransformModuleIod() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="SpatialTransformModuleIod"/> class.
		/// </summary>
		public SpatialTransformModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider) { }

		/// <summary>
		/// Gets or sets the value of ImageRotation in the underlying collection. Type 1.
		/// </summary>
		public int ImageRotation
		{
			get { return base.DicomAttributeProvider[DicomTags.ImageRotation].GetInt32(0, 0); }
			set
			{
				if (value % 90 != 0)
					throw new ArgumentOutOfRangeException("value", "ImageRotation must be one of 0, 90, 180 or 270.");
				base.DicomAttributeProvider[DicomTags.ImageRotation].SetInt32(0, ((value % 360) + 360) % 360); // this ensures that the value stored is positive and < 360
			}
		}

		/// <summary>
		/// Gets or sets the value of ImageHorizontalFlip in the underlying collection. Type 1.
		/// </summary>
		public ImageHorizontalFlip ImageHorizontalFlip
		{
			get { return ParseEnum(base.DicomAttributeProvider[DicomTags.ImageHorizontalFlip].GetString(0, string.Empty), ImageHorizontalFlip.None); }
			set
			{
				if (value == ImageHorizontalFlip.None)
					throw new ArgumentOutOfRangeException("value", "ImageHorizontalFlip is Type 1 Required.");
				SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.ImageHorizontalFlip], value);
			}
		}

		/// <summary>
		/// Gets an enumeration of <see cref="DicomTag"/>s used by this module.
		/// </summary>
		public static IEnumerable<uint> DefinedTags {
			get {
				yield return DicomTags.ImageHorizontalFlip;
				yield return DicomTags.ImageRotation;
			}
		}
	}

	/// <summary>
	/// Enumerated values for the <see cref="DicomTags.ImageHorizontalFlip"/> attribute describing whether or not to flip the image horizontally.
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.10.6 (Table C.10-6)</remarks>
	public enum ImageHorizontalFlip {
		Y,
		N,

		/// <summary>
		/// Represents the null value, which is equivalent to the unknown status.
		/// </summary>
		None
	}
}
