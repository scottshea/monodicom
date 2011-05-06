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

namespace ClearCanvas.Dicom.Iod.Macros
{
	/// <summary>
	/// Container Macro
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.18.8 (Table C.18.8-1)</remarks>
	public interface IContainerMacro : IIodMacro
	{
		/// <summary>
		/// Gets or sets the value of ContinuityOfContent in the underlying collection. Type 1.
		/// </summary>
		ContinuityOfContent ContinuityOfContent { get; set; }

		/// <summary>
		/// Gets or sets the value of ContentTemplateSequence in the underlying collection. Type 1C.
		/// </summary>
		ContentTemplateSequence ContentTemplateSequence { get; set; }

		/// <summary>
		/// Creates the value of ContentTemplateSequence in the underlying collection. Type 1C.
		/// </summary>
		ContentTemplateSequence CreateContentTemplateSequence();
	}

	/// <summary>
	/// Container Macro Base Implementation
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.18.8 (Table C.18.8-1)</remarks>
	internal class ContainerMacro : SequenceIodBase, IContainerMacro
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ContainerMacro"/> class.
		/// </summary>
		public ContainerMacro() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="ContainerMacro"/> class.
		/// </summary>
		/// <param name="dicomSequenceItem">The dicom sequence item.</param>
		public ContainerMacro(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

		/// <summary>
		/// Initializes the underlying collection to implement the module or sequence using default values.
		/// </summary>
		public void InitializeAttributes()
		{
			this.ContinuityOfContent = ContinuityOfContent.Separate;
			this.ContentTemplateSequence = null;
		}

		/// <summary>
		/// Gets or sets the value of ContinuityOfContent in the underlying collection. Type 1.
		/// </summary>
		public ContinuityOfContent ContinuityOfContent
		{
			get { return ParseEnum(base.DicomAttributeProvider[DicomTags.ContinuityOfContent].GetString(0, string.Empty), ContinuityOfContent.Unknown); }
			set
			{
				if (value == ContinuityOfContent.Unknown)
					throw new ArgumentNullException("value", "Continuity of Content is Type 1 Required.");
				SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.ContinuityOfContent], value);
			}
		}

		/// <summary>
		/// Gets or sets the value of ContentTemplateSequence in the underlying collection. Type 1C.
		/// </summary>
		public ContentTemplateSequence ContentTemplateSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ContentTemplateSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
				{
					return null;
				}
				return new ContentTemplateSequence(((DicomSequenceItem[]) dicomAttribute.Values)[0]);
			}
			set
			{
				if (value == null)
				{
					base.DicomAttributeProvider[DicomTags.ContentTemplateSequence] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.ContentTemplateSequence].Values = new DicomSequenceItem[] {value.DicomSequenceItem};
			}
		}

		/// <summary>
		/// Creates the value of ContentTemplateSequence in the underlying collection. Type 1C.
		/// </summary>
		public ContentTemplateSequence CreateContentTemplateSequence()
		{
			DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ContentTemplateSequence];
			if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
			{
				DicomSequenceItem dicomSequenceItem = new DicomSequenceItem();
				dicomAttribute.Values = new DicomSequenceItem[] {dicomSequenceItem};
				ContentTemplateSequence iodBase = new ContentTemplateSequence(dicomSequenceItem);
				iodBase.InitializeAttributes();
				return iodBase;
			}
			return new ContentTemplateSequence(((DicomSequenceItem[]) dicomAttribute.Values)[0]);
		}
	}
}