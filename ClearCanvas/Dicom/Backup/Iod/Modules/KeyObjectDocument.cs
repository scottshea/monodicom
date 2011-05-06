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
using ClearCanvas.Dicom.Iod.Macros.HierarchicalSeriesInstanceReference;
using ClearCanvas.Dicom.Iod.Sequences;
using ClearCanvas.Dicom.Utilities;

namespace ClearCanvas.Dicom.Iod.Modules
{
	/// <summary>
	/// KeyObjectDocument Module
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.17.6.2 (Table C.17.6-2)</remarks>
	public class KeyObjectDocumentModuleIod : IodBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="KeyObjectDocumentModuleIod"/> class.
		/// </summary>	
		public KeyObjectDocumentModuleIod() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="KeyObjectDocumentModuleIod"/> class.
		/// </summary>
		public KeyObjectDocumentModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider) {}

		/// <summary>
		/// Initializes the underlying collection to implement the module using default values.
		/// </summary>
		public void InitializeAttributes()
		{
			this.InstanceNumber = 1;
			this.ContentDateTime = DateTime.Now;
			this.ReferencedRequestSequence = null;
			this.CreateCurrentRequestedProcedureEvidenceSequence();
			this.IdenticalDocumentsSequence = null;
		}

		/// <summary>
		/// Gets or sets the value of InstanceNumber in the underlying collection. Type 1.
		/// </summary>
		public int InstanceNumber
		{
			get { return base.DicomAttributeProvider[DicomTags.InstanceNumber].GetInt32(0, 0); }
			set { base.DicomAttributeProvider[DicomTags.InstanceNumber].SetInt32(0, value); }
		}

		/// <summary>
		/// Gets or sets the value of ContentDate and ContentTime in the underlying collection.  Type 1.
		/// </summary>
		public DateTime? ContentDateTime
		{
			get
			{
				string date = base.DicomAttributeProvider[DicomTags.ContentDate].GetString(0, string.Empty);
				string time = base.DicomAttributeProvider[DicomTags.ContentTime].GetString(0, string.Empty);
				return DateTimeParser.ParseDateAndTime(string.Empty, date, time);
			}
			set
			{
				if (!value.HasValue)
					throw new ArgumentNullException("value", "Content is Type 1 Required.");
				DicomAttribute date = base.DicomAttributeProvider[DicomTags.ContentDate];
				DicomAttribute time = base.DicomAttributeProvider[DicomTags.ContentTime];
				DateTimeParser.SetDateTimeAttributeValues(value, date, time);
			}
		}

		/// <summary>
		/// Gets or sets the value of ReferencedRequestSequence in the underlying collection. Type 1C.
		/// </summary>
		public ReferencedRequestSequence[] ReferencedRequestSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ReferencedRequestSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
				{
					return null;
				}

				ReferencedRequestSequence[] result = new ReferencedRequestSequence[dicomAttribute.Count];
				DicomSequenceItem[] items = (DicomSequenceItem[]) dicomAttribute.Values;
				for (int n = 0; n < items.Length; n++)
					result[n] = new ReferencedRequestSequence(items[n]);

				return result;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					base.DicomAttributeProvider[DicomTags.ReferencedRequestSequence] = null;
					return;
				}

				DicomSequenceItem[] result = new DicomSequenceItem[value.Length];
				for (int n = 0; n < value.Length; n++)
					result[n] = value[n].DicomSequenceItem;

				base.DicomAttributeProvider[DicomTags.ReferencedRequestSequence].Values = result;
			}
		}

		/// <summary>
		/// Gets or sets the value of CurrentRequestedProcedureEvidenceSequence in the underlying collection. Type 1.
		/// </summary>
		/// <remarks>
		/// The helper class <see cref="HierarchicalSopInstanceReferenceDictionary"/> can be used to assist in creating
		/// an evidence sequence with minimal repetition.
		/// </remarks>
		public IHierarchicalSopInstanceReferenceMacro[] CurrentRequestedProcedureEvidenceSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.CurrentRequestedProcedureEvidenceSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
					return null;

				IHierarchicalSopInstanceReferenceMacro[] result = new IHierarchicalSopInstanceReferenceMacro[dicomAttribute.Count];
				DicomSequenceItem[] items = (DicomSequenceItem[]) dicomAttribute.Values;
				for (int n = 0; n < items.Length; n++)
					result[n] = new HierarchicalSopInstanceReferenceMacro(items[n]);

				return result;
			}
			set
			{
				if (value == null || value.Length == 0)
					throw new ArgumentNullException("value", "CurrentRequestedProcedureEvidenceSequence is Type 1 Required.");

				DicomSequenceItem[] result = new DicomSequenceItem[value.Length];
				for (int n = 0; n < value.Length; n++)
					result[n] = value[n].DicomSequenceItem;

				base.DicomAttributeProvider[DicomTags.CurrentRequestedProcedureEvidenceSequence].Values = result;
			}
		}

		/// <summary>
		/// Creates a single instance of a CurrentRequestedProcedureEvidenceSequence item. Does not modify the CurrentRequestedProcedureEvidenceSequence in the underlying collection.
		/// </summary>
		public IHierarchicalSopInstanceReferenceMacro CreateCurrentRequestedProcedureEvidenceSequence()
		{
			IHierarchicalSopInstanceReferenceMacro iodBase = new HierarchicalSopInstanceReferenceMacro(new DicomSequenceItem());
			iodBase.InitializeAttributes();
			return iodBase;
		}

		/// <summary>
		/// Gets or sets the value of IdenticalDocumentsSequence in the underlying collection. Type 1C.
		/// </summary>
		public IHierarchicalSopInstanceReferenceMacro[] IdenticalDocumentsSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.IdenticalDocumentsSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
				{
					return null;
				}

				IHierarchicalSopInstanceReferenceMacro[] result = new IHierarchicalSopInstanceReferenceMacro[dicomAttribute.Count];
				DicomSequenceItem[] items = (DicomSequenceItem[]) dicomAttribute.Values;
				for (int n = 0; n < items.Length; n++)
					result[n] = new HierarchicalSopInstanceReferenceMacro(items[n]);

				return result;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					base.DicomAttributeProvider[DicomTags.IdenticalDocumentsSequence] = null;
					return;
				}

				DicomSequenceItem[] result = new DicomSequenceItem[value.Length];
				for (int n = 0; n < value.Length; n++)
					result[n] = value[n].DicomSequenceItem;

				base.DicomAttributeProvider[DicomTags.IdenticalDocumentsSequence].Values = result;
			}
		}

		/// <summary>
		/// Creates a single instance of a IdenticalDocumentsSequence item. Does not modify the IdenticalDocumentsSequence in the underlying collection.
		/// </summary>
		public IHierarchicalSopInstanceReferenceMacro CreateIdenticalDocumentsSequence()
		{
			IHierarchicalSopInstanceReferenceMacro iodBase = new HierarchicalSopInstanceReferenceMacro(new DicomSequenceItem());
			iodBase.InitializeAttributes();
			return iodBase;
		}

		/// <summary>
		/// Creates a single instance of a IdenticalDocumentsSequence item. Does not modify the IdenticalDocumentsSequence in the underlying collection.
		/// </summary>
		public IHierarchicalSopInstanceReferenceMacro CreateIdenticalDocumentsSequence(string studyInstanceUid, string seriesInstanceUid, string sopClassUid, string sopInstanceUid)
		{
			IHierarchicalSopInstanceReferenceMacro identicalDocument;
			IHierarchicalSeriesInstanceReferenceMacro seriesReference;
			IReferencedSopSequence sopReference;

			identicalDocument = this.CreateIdenticalDocumentsSequence();
			identicalDocument.InitializeAttributes();
			identicalDocument.StudyInstanceUid = studyInstanceUid;
			identicalDocument.ReferencedSeriesSequence = new IHierarchicalSeriesInstanceReferenceMacro[] {seriesReference = identicalDocument.CreateReferencedSeriesSequence()};

			seriesReference.InitializeAttributes();
			seriesReference.SeriesInstanceUid = seriesInstanceUid;
			seriesReference.ReferencedSopSequence = new IReferencedSopSequence[] {sopReference = seriesReference.CreateReferencedSopSequence()};

			sopReference.InitializeAttributes();
			sopReference.ReferencedSopClassUid = sopClassUid;
			sopReference.ReferencedSopInstanceUid = sopInstanceUid;

			return identicalDocument;
		}
	}
}