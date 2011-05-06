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
using System.Collections.Generic;
using System.Text;

namespace ClearCanvas.Dicom.Iod.Sequences
{

	public class ViewCodeSequenceIod : SequenceIodBase
	{
		public ViewCodeSequenceIod()
		{
		}

		public ViewCodeSequenceIod(DicomSequenceItem sequenceItem)
			: base(sequenceItem)
		{
		}

		public string CodeValue
		{
			get { return base.DicomSequenceItem[DicomTags.CodeValue].GetString(0, ""); }
			set { base.DicomSequenceItem[DicomTags.CodeValue].SetString(0, value); }
		}

		public string CodingSchemeDesignator
		{
			get { return base.DicomSequenceItem[DicomTags.CodingSchemeDesignator].GetString(0, ""); }
			set { base.DicomSequenceItem[DicomTags.CodingSchemeDesignator].SetString(0, value); }
		}

		public string CodingSchemeVersion
		{
			get { return base.DicomSequenceItem[DicomTags.CodingSchemeVersion].GetString(0, ""); }
			set { base.DicomSequenceItem[DicomTags.CodingSchemeVersion].SetString(0, value); }
		}

		public string CodeMeaning
		{
			get { return base.DicomSequenceItem[DicomTags.CodeMeaning].GetString(0, ""); }
			set { base.DicomSequenceItem[DicomTags.CodeMeaning].SetString(0, value); }
		}

		public string ContextIdentifier
		{
			get { return base.DicomSequenceItem[DicomTags.ContextIdentifier].GetString(0, ""); }
			set { base.DicomSequenceItem[DicomTags.ContextIdentifier].SetString(0, value); }
		}

		public string MappingResource
		{
			get { return base.DicomSequenceItem[DicomTags.MappingResource].GetString(0, ""); }
			set { base.DicomSequenceItem[DicomTags.MappingResource].SetString(0, value); }
		}

		public DateTime? ContextGroupVersion
		{
			get { return base.DicomSequenceItem[DicomTags.ContextGroupVersion].GetDateTime(0); }
			set { base.DicomSequenceItem[DicomTags.ContextGroupVersion].SetDateTime(0, value); }
		}

		public string ContextGroupExtensionFlag
		{
			get { return base.DicomSequenceItem[DicomTags.ContextGroupExtensionFlag].GetString(0, ""); }
			set { base.DicomSequenceItem[DicomTags.ContextGroupExtensionFlag].SetString(0, value); }
		}

		public DateTime? ContextGroupLocalVersion
		{
			get { return base.DicomSequenceItem[DicomTags.ContextGroupLocalVersion].GetDateTime(0); }
			set { base.DicomSequenceItem[DicomTags.ContextGroupLocalVersion].SetDateTime(0, value); }
		}

		public string ContextGroupExtensionCreatorUid
		{
			get { return base.DicomSequenceItem[DicomTags.ContextGroupExtensionCreatorUid].GetString(0, ""); }
			set { base.DicomSequenceItem[DicomTags.ContextGroupExtensionCreatorUid].SetString(0, value); }
		}
	}
}
