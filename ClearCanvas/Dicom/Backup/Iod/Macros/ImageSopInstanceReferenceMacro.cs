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

namespace ClearCanvas.Dicom.Iod.Macros
{
	/// <summary>
	/// Image SOP Instance Reference Macro
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section 10.3 (Table 10-3)</remarks>
	public class ImageSopInstanceReferenceMacro : SequenceIodBase
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageSopInstanceReferenceMacro"/> class.
		/// </summary>
		public ImageSopInstanceReferenceMacro() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageSopInstanceReferenceMacro"/> class.
		/// </summary>
		/// <param name="dicomSequenceItem">The dicom sequence item.</param>
		public ImageSopInstanceReferenceMacro(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

		#endregion

		#region Public Properties

		/// <summary>
		/// Uniquely identifies the referenced SOP Class
		/// </summary>
		/// <value>The referenced sop class uid.</value>
		public string ReferencedSopClassUid
		{
			get { return base.DicomAttributeProvider[DicomTags.ReferencedSopClassUid].GetString(0, String.Empty); }
			set { base.DicomAttributeProvider[DicomTags.ReferencedSopClassUid].SetString(0, value); }
		}

		/// <summary>
		/// Uniquely identifies the referenced SOP Instance.
		/// </summary>
		/// <value>The referenced sop instance uid.</value>
		public string ReferencedSopInstanceUid
		{
			get { return base.DicomAttributeProvider[DicomTags.ReferencedSopInstanceUid].GetString(0, String.Empty); }
			set { base.DicomAttributeProvider[DicomTags.ReferencedSopInstanceUid].SetString(0, value); }
		}

		/// <summary>
		/// Identifies the frame numbers within the Referenced SOP Instance to which the 
		/// reference applies. The first frame shall be denoted as frame number 1. 
		/// <para>Note: This Attribute may be multi-valued. </para> 
		/// <para>
		/// Required if the Referenced SOP Instance is a multi-frame image and the reference 
		/// does not apply to all frames, and Referenced Segment Number (0062,000B) is not present.
		/// </para> 
		/// </summary>
		/// <value>The referenced frame number.</value>
		public DicomAttributeIS ReferencedFrameNumber
		{
			get { return base.DicomAttributeProvider[DicomTags.ReferencedFrameNumber] as DicomAttributeIS; }
		}

		/// <summary>
		/// Identifies the Segment Number to which the reference applies. Required if the Referenced
		///  SOP Instance is a Segmentation and the reference does not apply to all segments and
		///  Referenced Frame Number (0008,1160) is not present.
		/// </summary>
		/// <value>The referenced segment number.</value>
		public DicomAttributeUS ReferencedSegmentNumber
		{
			get { return base.DicomAttributeProvider[DicomTags.ReferencedSegmentNumber] as DicomAttributeUS; }
		}

		#endregion
	}
}