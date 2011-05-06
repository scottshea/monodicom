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

namespace ClearCanvas.Dicom.Iod.Sequences
{
	/// <summary>
	/// Specimen Sequence
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.7.1.2 (Table C.7-2a)</remarks>
	public class SpecimenSequence : SequenceIodBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SpecimenSequence"/> class.
		/// </summary>
		public SpecimenSequence() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="SpecimenSequence"/> class.
		/// </summary>
		/// <param name="dicomSequenceItem">The dicom sequence item.</param>
		public SpecimenSequence(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

		/// <summary>
		/// Gets or sets the value of SpecimenIdentifier in the underlying collection. Type 2.
		/// </summary>
		public string SpecimenIdentifier
		{
			get { return base.DicomAttributeProvider[DicomTags.SpecimenIdentifier].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.SpecimenIdentifier].SetNullValue();
					return;
				}
				base.DicomAttributeProvider[DicomTags.SpecimenIdentifier].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of SpecimenTypeCodeSequence in the underlying collection. Type 2C.
		/// </summary>
		public SpecimenTypeCodeSequence SpecimenTypeCodeSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.SpecimenTypeCodeSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
				{
					return null;
				}
				return new SpecimenTypeCodeSequence(((DicomSequenceItem[]) dicomAttribute.Values)[0]);
			}
			set
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.SpecimenTypeCodeSequence];
				if (value == null)
				{
					base.DicomAttributeProvider[DicomTags.SpecimenTypeCodeSequence] = null;
					return;
				}
				dicomAttribute.Values = new DicomSequenceItem[] {value.DicomSequenceItem};
			}
		}

		/// <summary>
		/// Gets or sets the value of SlideIdentifier in the underlying collection. Type 2C.
		/// </summary>
		public string SlideIdentifier
		{
			get { return base.DicomAttributeProvider[DicomTags.SlideIdentifier].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.SlideIdentifier] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.SlideIdentifier].SetString(0, value);
			}
		}
	}
}