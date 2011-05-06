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
	/// SopInstanceReference Macro
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section 10.8 (Table 10-11)</remarks>
	public interface ISopInstanceReferenceMacro : IIodMacro
	{
		/// <summary>
		/// Gets or sets the value of ReferencedSopClassUid in the underlying collection. Type 1.
		/// </summary>
		string ReferencedSopClassUid { get; set; }

		/// <summary>
		/// Gets or sets the value of ReferencedSopInstanceUid in the underlying collection. Type 1.
		/// </summary>
		string ReferencedSopInstanceUid { get; set; }
	}

	/// <summary>
	/// SopInstanceReference Macro Base Implementation
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section 10.8 (Table 10-11)</remarks>
	internal class SopInstanceReferenceMacro : SequenceIodBase, ISopInstanceReferenceMacro
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SopInstanceReferenceMacro"/> class.
		/// </summary>
		public SopInstanceReferenceMacro() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="SopInstanceReferenceMacro"/> class.
		/// </summary>
		/// <param name="dicomSequenceItem">The dicom sequence item.</param>
		public SopInstanceReferenceMacro(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

		/// <summary>
		/// Initializes the underlying collection to implement the module using default values.
		/// </summary>
		public virtual void InitializeAttributes()
		{
			this.ReferencedSopClassUid = "1";
			this.ReferencedSopInstanceUid = "1";
		}

		/// <summary>
		/// Gets or sets the value of ReferencedSopClassUid in the underlying collection. Type 1.
		/// </summary>
		public string ReferencedSopClassUid
		{
			get { return base.DicomAttributeProvider[DicomTags.ReferencedSopClassUid].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException("value", "ReferencedSopClassUid is Type 1 Required.");
				base.DicomAttributeProvider[DicomTags.ReferencedSopClassUid].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of ReferencedSopInstanceUid in the underlying collection. Type 1.
		/// </summary>
		public string ReferencedSopInstanceUid
		{
			get { return base.DicomAttributeProvider[DicomTags.ReferencedSopInstanceUid].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException("value", "ReferencedSopInstanceUid is Type 1 Required.");
				base.DicomAttributeProvider[DicomTags.ReferencedSopInstanceUid].SetString(0, value);
			}
		}
	}
}