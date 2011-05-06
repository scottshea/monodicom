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
using ClearCanvas.Dicom.Iod.Macros;

namespace ClearCanvas.Dicom.Iod.Sequences
{
	/// <summary>
	/// RelatedSeries Sequence
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.7.3.1 (Table C.7-5a)</remarks>
	public class RelatedSeriesSequence : SequenceIodBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RelatedSeriesSequence"/> class.
		/// </summary>
		public RelatedSeriesSequence() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="RelatedSeriesSequence"/> class.
		/// </summary>
		/// <param name="dicomSequenceItem">The dicom sequence item.</param>
		public RelatedSeriesSequence(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

		/// <summary>
		/// Gets or sets the value of StudyInstanceUid in the underlying collection. Type 1.
		/// </summary>
		public string StudyInstanceUid
		{
			get { return base.DicomAttributeProvider[DicomTags.StudyInstanceUid].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException("value", "StudyInstanceUid is Type 1 Required.");
				base.DicomAttributeProvider[DicomTags.StudyInstanceUid].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of SeriesInstanceUid in the underlying collection. Type 1.
		/// </summary>
		public string SeriesInstanceUid
		{
			get { return base.DicomAttributeProvider[DicomTags.SeriesInstanceUid].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException("value", "SeriesInstanceUid is Type 1 Required.");
				base.DicomAttributeProvider[DicomTags.SeriesInstanceUid].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of PurposeOfReferenceCodeSequence in the underlying collection. Type 2.
		/// </summary>
		public CodeSequenceMacro[] PurposeOfReferenceCodeSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.PurposeOfReferenceCodeSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
				{
					return null;
				}

				CodeSequenceMacro[] result = new CodeSequenceMacro[dicomAttribute.Count];
				DicomSequenceItem[] items = (DicomSequenceItem[]) dicomAttribute.Values;
				for (int n = 0; n < items.Length; n++)
					result[n] = new CodeSequenceMacro(items[n]);

				return result;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					base.DicomAttributeProvider[DicomTags.PurposeOfReferenceCodeSequence].SetNullValue();
					return;
				}

				DicomSequenceItem[] result = new DicomSequenceItem[value.Length];
				for (int n = 0; n < value.Length; n++)
					result[n] = value[n].DicomSequenceItem;

				base.DicomAttributeProvider[DicomTags.PurposeOfReferenceCodeSequence].Values = result;
			}
		}
	}
}