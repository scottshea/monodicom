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
	/// CompositeObjectReference Macro
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.18.3 (Table C.18.3-1)</remarks>
	public interface ICompositeObjectReferenceMacro : IIodMacro
	{
		/// <summary>
		/// Gets or sets the value of ReferencedSopSequence in the underlying collection. Type 1.
		/// </summary>
		ISopInstanceReferenceMacro ReferencedSopSequence { get; set; }

		/// <summary>
		/// Creates the value of ReferencedSopSequence in the underlying collection. Type 1.
		/// </summary>
		ISopInstanceReferenceMacro CreateReferencedSopSequence();
	}

	/// <summary>
	/// CompositeObjectReference Macro Base Implementation
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.18.3 (Table C.18.3-1)</remarks>
	internal class CompositeObjectReferenceMacro : SequenceIodBase, ICompositeObjectReferenceMacro
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CompositeObjectReferenceMacro"/> class.
		/// </summary>
		public CompositeObjectReferenceMacro() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="CompositeObjectReferenceMacro"/> class.
		/// </summary>
		/// <param name="dicomSequenceItem">The dicom sequence item.</param>
		public CompositeObjectReferenceMacro(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

		/// <summary>
		/// Initializes the underlying collection to implement the module or sequence using default values.
		/// </summary>
		public void InitializeAttributes()
		{
			this.CreateReferencedSopSequence();
			this.ReferencedSopSequence.InitializeAttributes();
		}

		/// <summary>
		/// Gets or sets the value of ReferencedSopSequence in the underlying collection. Type 1.
		/// </summary>
		public ISopInstanceReferenceMacro ReferencedSopSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ReferencedSopSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
				{
					return null;
				}
				return new SopInstanceReferenceMacro(((DicomSequenceItem[]) dicomAttribute.Values)[0]);
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("value", "ReferencedSopSequence is Type 1 Required.");
				base.DicomAttributeProvider[DicomTags.ReferencedSopSequence].Values = new DicomSequenceItem[] {value.DicomSequenceItem};
			}
		}

		/// <summary>
		/// Creates the value of ReferencedSopSequence in the underlying collection. Type 1.
		/// </summary>
		public ISopInstanceReferenceMacro CreateReferencedSopSequence()
		{
			DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ReferencedSopSequence];
			if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
			{
				DicomSequenceItem dicomSequenceItem = new DicomSequenceItem();
				dicomAttribute.Values = new DicomSequenceItem[] {dicomSequenceItem};
				SopInstanceReferenceMacro iodBase = new SopInstanceReferenceMacro(dicomSequenceItem);
				iodBase.InitializeAttributes();
				return iodBase;
			}
			return new SopInstanceReferenceMacro(((DicomSequenceItem[]) dicomAttribute.Values)[0]);
		}
	}
}