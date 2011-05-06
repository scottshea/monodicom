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
using ClearCanvas.Dicom.Iod.Macros;
using ClearCanvas.Dicom.Iod.Sequences;
using ClearCanvas.Dicom.Utilities;

namespace ClearCanvas.Dicom.Iod.Modules
{
	/// <summary>
	/// PresentationStateIdentification Module
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.11.10 (Table C.11.10-1)</remarks>
	public class PresentationStateIdentificationModuleIod : IodBase, IContentIdentificationMacro
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PresentationStateIdentificationModuleIod"/> class.
		/// </summary>	
		public PresentationStateIdentificationModuleIod() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="PresentationStateIdentificationModuleIod"/> class.
		/// </summary>
		public PresentationStateIdentificationModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider) { }

		/// <summary>
		/// Gets the dicom attribute collection as a dicom sequence item.
		/// </summary>
		/// <value>The dicom sequence item.</value>
		DicomSequenceItem IIodMacro.DicomSequenceItem
		{
			get { return base.DicomAttributeProvider as DicomSequenceItem; }
			set { base.DicomAttributeProvider = value; }
		}

		/// <summary>
		/// Initializes the underlying collection to implement the module or sequence using default values.
		/// </summary>
		public virtual void InitializeAttributes()
		{
			this.PresentationCreationDateTime = DateTime.Now;
			this.InstanceNumber = 1;
			this.ContentLabel = " ";
			this.ContentDescription = null;
			this.ContentCreatorsName = null;
			this.ContentCreatorsIdentificationCodeSequence = null;
		}

		/// <summary>
		/// Gets or sets the value of PresentationCreationDate and PresentationCreationTime in the underlying collection.  Type 1.
		/// </summary>
		public DateTime? PresentationCreationDateTime
		{
			get
			{
				string date = base.DicomAttributeProvider[DicomTags.PresentationCreationDate].GetString(0, string.Empty);
				string time = base.DicomAttributeProvider[DicomTags.PresentationCreationTime].GetString(0, string.Empty);
				return DateTimeParser.ParseDateAndTime(string.Empty, date, time);
			}
			set
			{
				if (!value.HasValue)
					throw new ArgumentNullException("value", "PresentationCreation is Type 1 Required.");
				DicomAttribute date = base.DicomAttributeProvider[DicomTags.PresentationCreationDate];
				DicomAttribute time = base.DicomAttributeProvider[DicomTags.PresentationCreationTime];
				DateTimeParser.SetDateTimeAttributeValues(value, date, time);
			}
		}

		/// <summary>
		/// Gets or sets the value of InstanceNumber in the underlying collection. Type 1.
		/// </summary>
		public int InstanceNumber {
			get { return base.DicomAttributeProvider[DicomTags.InstanceNumber].GetInt32(0, 0); }
			set { base.DicomAttributeProvider[DicomTags.InstanceNumber].SetInt32(0, value); }
		}

		/// <summary>
		/// Gets or sets the value of ContentLabel in the underlying collection. Type 1.
		/// </summary>
		public string ContentLabel {
			get { return base.DicomAttributeProvider[DicomTags.ContentLabel].GetString(0, string.Empty); }
			set {
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException("value", "ContentLabel is Type 1 Required.");
				base.DicomAttributeProvider[DicomTags.ContentLabel].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of ContentDescription in the underlying collection. Type 2.
		/// </summary>
		public string ContentDescription {
			get { return base.DicomAttributeProvider[DicomTags.ContentDescription].GetString(0, string.Empty); }
			set {
				if (string.IsNullOrEmpty(value)) {
					base.DicomAttributeProvider[DicomTags.ContentDescription].SetNullValue();
					return;
				}
				base.DicomAttributeProvider[DicomTags.ContentDescription].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of ContentCreatorsName in the underlying collection. Type 2.
		/// </summary>
		public string ContentCreatorsName {
			get { return base.DicomAttributeProvider[DicomTags.ContentCreatorsName].GetString(0, string.Empty); }
			set {
				if (string.IsNullOrEmpty(value)) {
					base.DicomAttributeProvider[DicomTags.ContentCreatorsName].SetNullValue();
					return;
				}
				base.DicomAttributeProvider[DicomTags.ContentCreatorsName].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of ContentCreatorsIdentificationCodeSequence in the underlying collection. Type 3.
		/// </summary>
		public PersonIdentificationMacro ContentCreatorsIdentificationCodeSequence {
			get {
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ContentCreatorsIdentificationCodeSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0) {
					return null;
				}
				return new PersonIdentificationMacro(((DicomSequenceItem[])dicomAttribute.Values)[0]);
			}
			set {
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ContentCreatorsIdentificationCodeSequence];
				if (value == null) {
					base.DicomAttributeProvider[DicomTags.ContentCreatorsIdentificationCodeSequence] = null;
					return;
				}
				dicomAttribute.Values = new DicomSequenceItem[] { value.DicomSequenceItem };
			}
		}

		/// <summary>
		/// Gets an enumeration of <see cref="DicomTag"/>s used by this module.
		/// </summary>
		public static IEnumerable<uint> DefinedTags {
			get {
				yield return DicomTags.ContentCreatorsIdentificationCodeSequence;
				yield return DicomTags.ContentCreatorsName;
				yield return DicomTags.ContentDescription;
				yield return DicomTags.ContentLabel;
				yield return DicomTags.InstanceNumber;
				yield return DicomTags.PresentationCreationDate;
				yield return DicomTags.PresentationCreationTime;
			}
		}
	}
}
