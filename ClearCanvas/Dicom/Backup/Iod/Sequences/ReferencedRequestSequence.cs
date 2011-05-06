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

using ClearCanvas.Dicom.Iod.Macros;

namespace ClearCanvas.Dicom.Iod.Sequences
{
	/// <summary>
	/// ReferencedRequest Sequence
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.17.6.2 (Table dcmtable)</remarks>
	public class ReferencedRequestSequence : SequenceIodBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ReferencedRequestSequence"/> class.
		/// </summary>
		public ReferencedRequestSequence() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReferencedRequestSequence"/> class.
		/// </summary>
		/// <param name="dicomSequenceItem">The dicom sequence item.</param>
		public ReferencedRequestSequence(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

		/// <summary>
		/// Gets or sets the value of StudyInstanceUid in the underlying collection.
		/// </summary>
		public string StudyInstanceUid
		{
			get { return base.DicomAttributeProvider[DicomTags.StudyInstanceUid].GetString(0, string.Empty); }
			set { base.DicomAttributeProvider[DicomTags.StudyInstanceUid].SetString(0, value); }
		}

		/// <summary>
		/// Gets or sets the value of ReferencedStudySequence in the underlying collection.
		/// </summary>
		public ISopInstanceReferenceMacro ReferencedStudySequence
		{
			get
			{
				if (base.DicomAttributeProvider[DicomTags.ReferencedStudySequence].Count == 0)
				{
					return null;
				}
				return new SopInstanceReferenceMacro(((DicomSequenceItem[]) base.DicomAttributeProvider[DicomTags.ReferencedStudySequence].Values)[0]);
			}
			set
			{
				if (value == null)
				{
					base.DicomAttributeProvider[DicomTags.ReferencedStudySequence] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.ReferencedStudySequence].Values = new DicomSequenceItem[] {value.DicomSequenceItem};
			}
		}

		/// <summary>
		/// Gets or sets the value of AccessionNumber in the underlying collection.
		/// </summary>
		public string AccessionNumber
		{
			get { return base.DicomAttributeProvider[DicomTags.AccessionNumber].GetString(0, string.Empty); }
			set { base.DicomAttributeProvider[DicomTags.AccessionNumber].SetString(0, value); }
		}

		/// <summary>
		/// Gets or sets the value of PlacerOrderNumberImagingServiceRequest in the underlying collection.
		/// </summary>
		public string PlacerOrderNumberImagingServiceRequest
		{
			get { return base.DicomAttributeProvider[DicomTags.PlacerOrderNumberImagingServiceRequest].GetString(0, string.Empty); }
			set { base.DicomAttributeProvider[DicomTags.PlacerOrderNumberImagingServiceRequest].SetString(0, value); }
		}

		/// <summary>
		/// Gets or sets the value of FillerOrderNumberImagingServiceRequest in the underlying collection.
		/// </summary>
		public string FillerOrderNumberImagingServiceRequest
		{
			get { return base.DicomAttributeProvider[DicomTags.FillerOrderNumberImagingServiceRequest].GetString(0, string.Empty); }
			set { base.DicomAttributeProvider[DicomTags.FillerOrderNumberImagingServiceRequest].SetString(0, value); }
		}

		/// <summary>
		/// Gets or sets the value of RequestedProcedureId in the underlying collection.
		/// </summary>
		public string RequestedProcedureId
		{
			get { return base.DicomAttributeProvider[DicomTags.RequestedProcedureId].GetString(0, string.Empty); }
			set { base.DicomAttributeProvider[DicomTags.RequestedProcedureId].SetString(0, value); }
		}

		/// <summary>
		/// Gets or sets the value of RequestedProcedureDescription in the underlying collection.
		/// </summary>
		public string RequestedProcedureDescription
		{
			get { return base.DicomAttributeProvider[DicomTags.RequestedProcedureDescription].GetString(0, string.Empty); }
			set { base.DicomAttributeProvider[DicomTags.RequestedProcedureDescription].SetString(0, value); }
		}

		/// <summary>
		/// Gets or sets the value of RequestedProcedureCodeSequence in the underlying collection.
		/// </summary>
		public CodeSequenceMacro RequestedProcedureCodeSequence
		{
			get
			{
				if (base.DicomAttributeProvider[DicomTags.RequestedProcedureCodeSequence].Count == 0)
				{
					return null;
				}
				return new CodeSequenceMacro(((DicomSequenceItem[]) base.DicomAttributeProvider[DicomTags.RequestedProcedureCodeSequence].Values)[0]);
			}
			set
			{
				if (value == null)
				{
					base.DicomAttributeProvider[DicomTags.RequestedProcedureCodeSequence] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.RequestedProcedureCodeSequence].Values = new DicomSequenceItem[] {value.DicomSequenceItem};
			}
		}
	}
}