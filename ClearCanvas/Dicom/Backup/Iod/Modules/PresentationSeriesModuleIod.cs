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
using ClearCanvas.Dicom.Utilities;

namespace ClearCanvas.Dicom.Iod.Modules
{
	/// <summary>
	/// PresentationSeries Module
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.11.9 (Table C.11.9-1)</remarks>
	public class PresentationSeriesModuleIod : IodBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PresentationSeriesModuleIod"/> class.
		/// </summary>	
		public PresentationSeriesModuleIod() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="PresentationSeriesModuleIod"/> class.
		/// </summary>
		public PresentationSeriesModuleIod(IDicomAttributeProvider provider) : base(provider) {}

		/// <summary>
		/// Initializes the underlying collection to implement the module or sequence using default values.
		/// </summary>
		public void InitializeAttributes()
		{
			this.Modality = Modality.PR;
		}

		/// <summary>
		/// Gets or sets the value of Modality in the underlying collection. Type 1.
		/// </summary>
		public Modality Modality
		{
			get { return ParseEnum(base.DicomAttributeProvider[DicomTags.Modality].GetString(0, string.Empty), Modality.None); }
			set
			{
				if (value != Modality.PR)
					throw new ArgumentOutOfRangeException("value", "Modality must be PR.");
				SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.Modality], value);
			}
		}

		/// <summary>
		/// Gets an enumeration of <see cref="DicomTag"/>s used by this module.
		/// </summary>
		public static IEnumerable<uint> DefinedTags {
			get {
				yield return DicomTags.Modality;
			}
		}
	}
}