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

using System.Collections.Generic;
using ClearCanvas.Dicom.Iod.Macros;
using ClearCanvas.Dicom.Iod.Macros.ModalityLut;

namespace ClearCanvas.Dicom.Iod.Modules
{
	/// <summary>
	/// ModalityLut Module
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.11.1 (Table C.11-1)</remarks>
	public class ModalityLutModuleIod : IodBase, IModalityLutMacro
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ModalityLutModuleIod"/> class.
		/// </summary>	
		public ModalityLutModuleIod() : base() {}

		//TODO: Modality lut module should take ???

		/// <summary>
		/// Initializes a new instance of the <see cref="ModalityLutModuleIod"/> class.
		/// </summary>
		public ModalityLutModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider) { }

		DicomSequenceItem IIodMacro.DicomSequenceItem
		{
			get { return base.DicomAttributeProvider as DicomSequenceItem; }
			set { base.DicomAttributeProvider = value; }
		}

		/// <summary>
		/// Initializes the underlying collection to implement the module or sequence using default values.
		/// </summary>
		public void InitializeAttributes() {}

		/// <summary>
		/// Gets or sets the value of ModalityLutSequence in the underlying collection. Type 1C.
		/// </summary>
		public ModalityLutSequenceItem ModalityLutSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ModalityLutSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
				{
					return null;
				}
				return new ModalityLutSequenceItem(((DicomSequenceItem[]) dicomAttribute.Values)[0]);
			}
			set
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ModalityLutSequence];
				if (value == null)
				{
					base.DicomAttributeProvider[DicomTags.ModalityLutSequence] = null;
					return;
				}
				dicomAttribute.Values = new DicomSequenceItem[] {value.DicomSequenceItem};
			}
		}

		/// <summary>
		/// Gets or sets the value of RescaleIntercept in the underlying collection. Type 1C.
		/// </summary>
		public double? RescaleIntercept
		{
			get
			{
				double result;
				if (base.DicomAttributeProvider[DicomTags.RescaleIntercept].TryGetFloat64(0, out result))
					return result;
				return null;
			}
			set
			{
				if (!value.HasValue)
				{
					base.DicomAttributeProvider[DicomTags.RescaleIntercept] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.RescaleIntercept].SetFloat64(0, value.Value);
			}
		}

		/// <summary>
		/// Gets or sets the value of RescaleSlope in the underlying collection. Type 1C.
		/// </summary>
		public double? RescaleSlope
		{
			get
			{
				double result;
				if (base.DicomAttributeProvider[DicomTags.RescaleSlope].TryGetFloat64(0, out result))
					return result;
				return null;
			}
			set
			{
				if (!value.HasValue)
				{
					base.DicomAttributeProvider[DicomTags.RescaleSlope] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.RescaleSlope].SetFloat64(0, value.Value);
			}
		}

		/// <summary>
		/// Gets or sets the value of RescaleType in the underlying collection. Type 1C.
		/// </summary>
		public string RescaleType
		{
			get { return base.DicomAttributeProvider[DicomTags.RescaleType].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.RescaleType] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.RescaleType].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets an enumeration of <see cref="DicomTag"/>s used by this module.
		/// </summary>
		public static IEnumerable<uint> DefinedTags {
			get {
				yield return DicomTags.ModalityLutSequence;
				yield return DicomTags.RescaleIntercept;
				yield return DicomTags.RescaleSlope;
				yield return DicomTags.RescaleType;
			}
		}
	}
}