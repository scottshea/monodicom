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
	/// Content Template Sequence
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.18.8 (Table C.18.8-1)</remarks>
	public class ContentTemplateSequence : SequenceIodBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ContentTemplateSequence"/> class.
		/// </summary>
		public ContentTemplateSequence() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="ContentTemplateSequence"/> class.
		/// </summary>
		/// <param name="dicomSequenceItem">The dicom sequence item.</param>
		public ContentTemplateSequence(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

		public void InitializeAttributes()
		{
			this.MappingResource = "DCMR";
			this.TemplateIdentifier = "1";
		}

		/// <summary>
		/// Gets or sets the value of MappingResource in the underlying collection. Type 1.
		/// </summary>
		public string MappingResource
		{
			get { return base.DicomAttributeProvider[DicomTags.MappingResource].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException("value", "MappingResource is Type 1 Required.");
				base.DicomAttributeProvider[DicomTags.MappingResource].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of TemplateIdentifier in the underlying collection. Type 1.
		/// </summary>
		public string TemplateIdentifier
		{
			get { return base.DicomAttributeProvider[DicomTags.TemplateIdentifier].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException("value", "Template Identifier is Type 1 Required.");
				base.DicomAttributeProvider[DicomTags.TemplateIdentifier].SetString(0, value);
			}
		}
	}
}