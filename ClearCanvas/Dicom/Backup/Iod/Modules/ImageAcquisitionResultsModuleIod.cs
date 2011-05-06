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
using ClearCanvas.Dicom.Iod.Sequences;

namespace ClearCanvas.Dicom.Iod.Modules
{
	/// <summary>
	/// As per Dicom DOC 3 C.4.15 (pg 256)
	/// </summary>
	public class ImageAcquisitionResultsModuleIod : IodBase
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageAcquisitionResultsModuleIod"/> class.
		/// </summary>
		public ImageAcquisitionResultsModuleIod() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageAcquisitionResultsModuleIod"/> class.
		/// </summary>
		public ImageAcquisitionResultsModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider) { }

		#endregion

		#region Public Properties

		public Modality Modality
		{
			get { return ParseEnum<Modality>(base.DicomAttributeProvider[DicomTags.Modality].GetString(0, String.Empty), Modality.None); }
			set { SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.Modality], value); }
		}

		public string StudyId
		{
			get { return base.DicomAttributeProvider[DicomTags.StudyId].GetString(0, String.Empty); }
			set { base.DicomAttributeProvider[DicomTags.StudyId].SetString(0, value); }
		}

		/// <summary>
		/// Gets the performed protocol code sequence list.
		/// Sequence describing the Protocol performed for this Procedure Step. This sequence 
		/// may have zero or more Items.
		/// </summary>
		/// <value>The performed protocol code sequence list.</value>
		public SequenceIodList<CodeSequenceMacro> PerformedProtocolCodeSequenceList
		{
			get { return new SequenceIodList<CodeSequenceMacro>(base.DicomAttributeProvider[DicomTags.PerformedProtocolCodeSequence] as DicomAttributeSQ); }
		}

		/// <summary>
		/// Gets the protocol context sequence list.
		/// Sequence that specifies the context for the Performed Protocol Code Sequence Item. 
		/// One or more items may be included in this sequence. See Section C.4.10.1.
		/// </summary>
		/// <value>The protocol context sequence list.</value>
		public SequenceIodList<ContentItemMacro> ProtocolContextSequenceList
		{
			get { return new SequenceIodList<ContentItemMacro>(base.DicomAttributeProvider[DicomTags.ProtocolContextSequence] as DicomAttributeSQ); }
		}

		/// <summary>
		/// Sequence that specifies modifiers for a Protocol Context Content Item. One or 
		/// more items may be included in this sequence. See Section C.4.10.1.
		/// </summary>
		/// <value>The content item modifier sequence list.</value>
		public SequenceIodList<ContentItemMacro> ContentItemModifierSequenceList
		{
			get { return new SequenceIodList<ContentItemMacro>(base.DicomAttributeProvider[DicomTags.ContentItemModifierSequence] as DicomAttributeSQ); }
		}

		public SequenceIodList<PerformedSeriesSequenceIod> PerformedSeriesSequenceList
		{
			get { return new SequenceIodList<PerformedSeriesSequenceIod>(base.DicomAttributeProvider[DicomTags.PerformedSeriesSequence] as DicomAttributeSQ); }
		}

		#endregion
	}
}