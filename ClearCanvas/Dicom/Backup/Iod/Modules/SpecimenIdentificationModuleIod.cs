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
using ClearCanvas.Dicom.Iod.Sequences;

namespace ClearCanvas.Dicom.Iod.Modules
{
	/// <summary>
	/// SpecimenIdentification Module
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.7.1.2 (Table C.7-2a)</remarks>
	public class SpecimenIdentificationModuleIod : IodBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SpecimenIdentificationModuleIod"/> class.
		/// </summary>	
		public SpecimenIdentificationModuleIod() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="SpecimenIdentificationModuleIod"/> class.
		/// </summary>
		public SpecimenIdentificationModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider) {}

		/// <summary>
		/// Initializes the underlying collection to implement the module or sequence using default values.
		/// </summary>
		public void InitializeAttributes()
		{
			this.SpecimenAccessionNumber = "1";
			this.SpecimenSequence = null;
		}

		/// <summary>
		/// Checks if this module appears to be non-empty.
		/// </summary>
		/// <returns>True if the module appears to be non-empty; False otherwise.</returns>
		public bool HasValues()
		{
			if (this.SpecimenSequence == null && string.IsNullOrEmpty(this.SpecimenAccessionNumber))
				return false;
			return true;
		}

		/// <summary>
		/// Gets or sets the value of SpecimenAccessionNumber in the underlying collection. Type 1.
		/// </summary>
		public string SpecimenAccessionNumber
		{
			get { return base.DicomAttributeProvider[DicomTags.SpecimenAccessionNumber].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException("value", "SpecimenAccessionNumber is Type 1 Required.");
				base.DicomAttributeProvider[DicomTags.SpecimenAccessionNumber].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of SpecimenSequence in the underlying collection. Type 2.
		/// </summary>
		public SpecimenSequence[] SpecimenSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.SpecimenSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
				{
					return null;
				}

				SpecimenSequence[] result = new SpecimenSequence[dicomAttribute.Count];
				DicomSequenceItem[] items = (DicomSequenceItem[]) dicomAttribute.Values;
				for (int n = 0; n < items.Length; n++)
					result[n] = new SpecimenSequence(items[n]);

				return result;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					base.DicomAttributeProvider[DicomTags.SpecimenSequence].SetNullValue();
					return;
				}

				DicomSequenceItem[] result = new DicomSequenceItem[value.Length];
				for (int n = 0; n < value.Length; n++)
					result[n] = value[n].DicomSequenceItem;

				base.DicomAttributeProvider[DicomTags.SpecimenSequence].Values = result;
			}
		}
	}
}