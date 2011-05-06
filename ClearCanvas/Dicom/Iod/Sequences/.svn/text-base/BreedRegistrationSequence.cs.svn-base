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

namespace ClearCanvas.Dicom.Iod.Sequences
{
	/// <summary>
	/// BreedRegistration Sequence
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.7.1.1 (Table C.7-1)</remarks>
	public class BreedRegistrationSequence : SequenceIodBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BreedRegistrationSequence"/> class.
		/// </summary>
		public BreedRegistrationSequence() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="BreedRegistrationSequence"/> class.
		/// </summary>
		/// <param name="dicomSequenceItem">The dicom sequence item.</param>
		public BreedRegistrationSequence(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) { }

		/// <summary>
		/// Initializes the underlying collection to implement the module or sequence using default values.
		/// </summary>
		public void InitializeAttributes() {
			this.BreedRegistrationNumber = " ";
			this.BreedRegistryCodeSequence = new BreedRegistryCodeSequence();
		}

		/// <summary>
		/// Gets or sets the value of BreedRegistrationNumber in the underlying collection. Type 1.
		/// </summary>
		public string BreedRegistrationNumber
		{
			get { return base.DicomAttributeProvider[DicomTags.BreedRegistrationNumber].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException("value", "BreedRegistrationNumber is Type 1 Required.");
				base.DicomAttributeProvider[DicomTags.BreedRegistrationNumber].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of BreedRegistryCodeSequence in the underlying collection. Type 1.
		/// </summary>
		public BreedRegistryCodeSequence BreedRegistryCodeSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.BreedRegistryCodeSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
					return null;
				return new BreedRegistryCodeSequence(((DicomSequenceItem[]) dicomAttribute.Values)[0]);
			}
			set
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.BreedRegistryCodeSequence];
				if (value == null)
					throw new ArgumentNullException("value", "BreedRegistryCodeSequence is Type 1 Required.");
				dicomAttribute.Values = new DicomSequenceItem[] {value.DicomSequenceItem};
			}
		}
	}
}
