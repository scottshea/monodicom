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
using ClearCanvas.Dicom.Iod.Macros.DocumentRelationship;
using ClearCanvas.Dicom.Iod.Macros.ImageReference;
using ClearCanvas.Dicom.Iod.Sequences;

namespace ClearCanvas.Dicom.Iod.Macros
{
	/// <summary> 
	/// DocumentRelationship Macro
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.17.3 (Table C.17-6)</remarks>
	public interface IDocumentRelationshipMacro : IIodMacro
	{
		/// <summary>
		/// Gets or sets the value of ObservationDateTime in the underlying collection. Type 1C.
		/// </summary>
		DateTime? ObservationDateTime { get; set; }

		/// <summary>
		/// Gets or sets the value of ContentSequence in the underlying collection. Type 1C.
		/// </summary>
		IContentSequence[] ContentSequence { get; set; }

		/// <summary>
		/// Creates a single instance of a ContentSequence item. Does not modify the ContentSequence in the underlying collection.
		/// </summary>
		IContentSequence CreateContentSequence();
	}

	/// <summary> 
	/// DocumentRelationship Macro Base Implementation
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.17.3 (Table C.17-6)</remarks>
	internal class DocumentRelationshipMacro : SequenceIodBase, IDocumentRelationshipMacro
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentRelationshipMacro"/> class.
		/// </summary>
		public DocumentRelationshipMacro() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentRelationshipMacro"/> class.
		/// </summary>
		/// <param name="dicomSequenceItem">The dicom sequence item.</param>
		public DocumentRelationshipMacro(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

		/// <summary>
		/// Initializes the underlying collection to implement the module or sequence using default values.
		/// </summary>
		public void InitializeAttributes()
		{
			this.ObservationDateTime = null;
			this.ContentSequence = null;
		}

		/// <summary>
		/// Gets or sets the value of ObservationDateTime in the underlying collection. Type 1C.
		/// </summary>
		public DateTime? ObservationDateTime
		{
			get { return base.DicomAttributeProvider[DicomTags.ObservationDateTime].GetDateTime(0); }
			set
			{
				if (!value.HasValue)
				{
					base.DicomAttributeProvider[DicomTags.ObservationDateTime] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.ObservationDateTime].SetDateTime(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of ContentSequence in the underlying collection. Type 1C.
		/// </summary>
		public IContentSequence[] ContentSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ContentSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
				{
					return null;
				}

				IContentSequence[] result = new IContentSequence[dicomAttribute.Count];
				DicomSequenceItem[] items = (DicomSequenceItem[]) dicomAttribute.Values;
				for (int n = 0; n < items.Length; n++)
					result[n] = new ContentSequenceType(items[n]);

				return result;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					base.DicomAttributeProvider[DicomTags.ContentSequence] = null;
					return;
				}

				DicomSequenceItem[] result = new DicomSequenceItem[value.Length];
				for (int n = 0; n < value.Length; n++)
					result[n] = value[n].DicomSequenceItem;

				base.DicomAttributeProvider[DicomTags.ContentSequence].Values = result;
			}
		}

		/// <summary>
		/// Creates a single instance of a ContentSequence item. Does not modify the ContentSequence in the underlying collection.
		/// </summary>
		public IContentSequence CreateContentSequence()
		{
			ContentSequenceType iodBase = new ContentSequenceType(new DicomSequenceItem());
			iodBase.InitializeAttributes();
			return iodBase;
		}
	}

	namespace DocumentRelationship
	{
		public interface IContentSequence : IIodMacro, IDocumentContentMacro, IDocumentRelationshipMacro
		{
			/// <summary>
			/// Gets or sets the value of RelationshipTyp in the underlying collection. Type 1.
			/// </summary>
			RelationshipType RelationshipType { get; set; }

			/// <summary>
			/// Gets or sets the value of ReferencedContentItemIdentifier in the underlying collection. Type 1C.
			/// </summary>
			uint[] ReferencedContentItemIdentifier { get; set; }
		}

		internal class ContentSequenceType : SequenceIodBase, IContentSequence
		{
			public ContentSequenceType() : base() {}
			public ContentSequenceType(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

			/// <summary>
			/// Initializes the underlying collection to implement the module or sequence using default values.
			/// </summary>
			public void InitializeAttributes()
			{
				this.RelationshipType = RelationshipType.Contains;
				this.ReferencedContentItemIdentifier = null;
				this.ConceptNameCodeSequence = null;
				this.TextValue = null;
				this.DateTime = null;
				this.Date = null;
				this.Time = null;
				this.PersonName = null;
				this.Uid = null;
				this.ObservationDateTime = null;
				this.ContentSequence = null;
			}

			/// <summary>
			/// Gets or sets the value of RelationshipTyp in the underlying collection. Type 1.
			/// </summary>
			public RelationshipType RelationshipType
			{
				get { return ParseEnum(base.DicomAttributeProvider[DicomTags.RelationshipType].GetString(0, string.Empty), RelationshipType.Unknown); }
				set
				{
					if (value == RelationshipType.Unknown)
						throw new ArgumentOutOfRangeException("value", "RelationshipType is Type 1 Required.");
					SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.RelationshipType], value, true);
				}
			}

			/// <summary>
			/// Gets or sets the value of ReferencedContentItemIdentifier in the underlying collection. Type 1C.
			/// </summary>
			public uint[] ReferencedContentItemIdentifier
			{
				get { return (uint[])base.DicomAttributeProvider[DicomTags.ReferencedContentItemIdentifier].Values; }
				set
				{
					if (value == null || value.Length == 0)
					{
						base.DicomAttributeProvider[DicomTags.ReferencedContentItemIdentifier] = null;
						return;
					}
					base.DicomAttributeProvider[DicomTags.ReferencedContentItemIdentifier].Values = value;
				}
			}

			#region IDocumentContentMacro Members

			/// <summary>
			/// Gets or sets the value of ValueType in the underlying collection. Type 1.
			/// </summary>
			public virtual ValueType ValueType
			{
				get { return ParseEnum(base.DicomAttributeProvider[DicomTags.ValueType].GetString(0, string.Empty), ValueType.None); }
				set
				{
					if (value == ValueType.None)
						throw new ArgumentNullException("value", "ValueType is a required Type 1.");
					SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.ValueType], value);
				}
			}

			/// <summary>
			/// Gets or sets the value of ConceptNameCodeSequence in the underlying collection. Type 1C.
			/// </summary>
			public CodeSequenceMacro ConceptNameCodeSequence
			{
				get
				{
					DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ConceptNameCodeSequence];
					if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
					{
						return null;
					}
					return new CodeSequenceMacro(((DicomSequenceItem[]) dicomAttribute.Values)[0]);
				}
				set
				{
					if (value == null)
					{
						base.DicomAttributeProvider[DicomTags.ConceptNameCodeSequence] = null;
						return;
					}
					base.DicomAttributeProvider[DicomTags.ConceptNameCodeSequence].Values = new DicomSequenceItem[] {value.DicomSequenceItem};
				}
			}

			/// <summary>
			/// Gets or sets the value of TextValue in the underlying collection. Type 1C.
			/// </summary>
			public string TextValue
			{
				get { return base.DicomAttributeProvider[DicomTags.TextValue].ToString(); }
				set
				{
					if (string.IsNullOrEmpty(value))
					{
						base.DicomAttributeProvider[DicomTags.TextValue] = null;
						return;
					}
					base.DicomAttributeProvider[DicomTags.TextValue].SetStringValue(value);
				}
			}

			/// <summary>
			/// Gets or sets the value of DateTime in the underlying collection. Type 1C.
			/// </summary>
			public DateTime? DateTime
			{
				get { return base.DicomAttributeProvider[DicomTags.Datetime].GetDateTime(0); }
				set
				{
					if (!value.HasValue)
					{
						base.DicomAttributeProvider[DicomTags.Datetime] = null;
						return;
					}
					base.DicomAttributeProvider[DicomTags.Datetime].SetDateTime(0, value);
				}
			}

			/// <summary>
			/// Gets or sets the value of Date in the underlying collection. Type 1C.
			/// </summary>
			public DateTime? Date
			{
				get { return base.DicomAttributeProvider[DicomTags.Date].GetDateTime(0); }
				set
				{
					if (!value.HasValue)
					{
						base.DicomAttributeProvider[DicomTags.Date] = null;
						return;
					}
					base.DicomAttributeProvider[DicomTags.Date].SetDateTime(0, value);
				}
			}

			/// <summary>
			/// Gets or sets the value of Time in the underlying collection. Type 1C.
			/// </summary>
			public DateTime? Time
			{
				get { return base.DicomAttributeProvider[DicomTags.Time].GetDateTime(0); }
				set
				{
					if (!value.HasValue)
					{
						base.DicomAttributeProvider[DicomTags.Time] = null;
						return;
					}
					base.DicomAttributeProvider[DicomTags.Time].SetDateTime(0, value);
				}
			}

			/// <summary>
			/// Gets or sets the value of PersonName in the underlying collection. Type 1C.
			/// </summary>
			public string PersonName
			{
				get { return base.DicomAttributeProvider[DicomTags.PersonName].GetString(0, string.Empty); }
				set
				{
					if (string.IsNullOrEmpty(value))
					{
						base.DicomAttributeProvider[DicomTags.PersonName] = null;
						return;
					}
					base.DicomAttributeProvider[DicomTags.PersonName].SetString(0, value);
				}
			}

			/// <summary>
			/// Gets or sets the value of Uid in the underlying collection. Type 1C.
			/// </summary>
			public string Uid
			{
				get { return base.DicomAttributeProvider[DicomTags.Uid].GetString(0, string.Empty); }
				set
				{
					if (string.IsNullOrEmpty(value))
					{
						base.DicomAttributeProvider[DicomTags.Uid] = null;
						return;
					}
					base.DicomAttributeProvider[DicomTags.Uid].SetString(0, value);
				}
			}

			#endregion

			#region IImageReferenceMacro Members

			/// <summary>
			/// Initializes the underlying collection to implement the module or sequence with a value type of IMAGE using default values.
			/// </summary>
			public IImageReferenceMacro InitializeImageReferenceAttributes() {
				this.InitializeAttributes();
				this.ValueType = ValueType.Image;
				((IImageReferenceMacro)this).CreateReferencedSopSequence();
				((IImageReferenceMacro)this).ReferencedSopSequence.InitializeAttributes();
				return this;
			}

			/// <summary>
			/// Gets or sets the value of ReferencedSopSequence in the underlying collection. Type 1.
			/// </summary>
			ISopInstanceReferenceMacro ICompositeObjectReferenceMacro.ReferencedSopSequence {
				get { return ((IImageReferenceMacro)this).ReferencedSopSequence; }
				set { ((IImageReferenceMacro)this).ReferencedSopSequence = (IReferencedSopSequence)value; }
			}

			/// <summary>
			/// Gets or sets the value of ReferencedSopSequence in the underlying collection. Type 1.
			/// </summary>
			IReferencedSopSequence IImageReferenceMacro.ReferencedSopSequence {
				get {
					DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ReferencedSopSequence];
					if (dicomAttribute.IsNull || dicomAttribute.Count == 0) {
						return null;
					}
					return new ImageReferenceMacro.ReferencedSopSequenceType(((DicomSequenceItem[])dicomAttribute.Values)[0]);
				}
				set {
					if (value == null)
						throw new ArgumentNullException("value", "ReferencedSopSequence is Type 1 Required.");
					base.DicomAttributeProvider[DicomTags.ReferencedSopSequence].Values = new DicomSequenceItem[] { value.DicomSequenceItem };
				}
			}

			/// <summary>
			/// Creates the value of ReferencedSopSequence in the underlying collection. Type 1.
			/// </summary>
			ISopInstanceReferenceMacro ICompositeObjectReferenceMacro.CreateReferencedSopSequence() {
				return ((IImageReferenceMacro)this).CreateReferencedSopSequence();
			}

			/// <summary>
			/// Creates the value of ReferencedSopSequence in the underlying collection. Type 1.
			/// </summary>
			IReferencedSopSequence IImageReferenceMacro.CreateReferencedSopSequence() {
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ReferencedSopSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0) {
					DicomSequenceItem dicomSequenceItem = new DicomSequenceItem();
					dicomAttribute.Values = new DicomSequenceItem[] { dicomSequenceItem };
					ImageReferenceMacro.ReferencedSopSequenceType iodBase = new ImageReferenceMacro.ReferencedSopSequenceType(dicomSequenceItem);
					iodBase.InitializeAttributes();
					return iodBase;
				}
				return new ImageReferenceMacro.ReferencedSopSequenceType(((DicomSequenceItem[])dicomAttribute.Values)[0]);
			}

			#endregion

			#region IContainerMacro Members

			/// <summary>
			/// Initializes the underlying collection to implement the module or sequence with a value type of CONTAINER using default values.
			/// </summary>
			public IContainerMacro InitializeContainerAttributes() {
				this.InitializeAttributes();
				this.ValueType = ValueType.Container;
				((IContainerMacro)this).ContinuityOfContent = ContinuityOfContent.Separate;
				((IContainerMacro)this).ContentTemplateSequence = null;
				return this;
			}

			/// <summary>
			/// Gets or sets the value of ContinuityOfContent in the underlying collection. Type 1.
			/// </summary>
			ContinuityOfContent IContainerMacro.ContinuityOfContent {
				get { return ParseEnum(base.DicomAttributeProvider[DicomTags.ContinuityOfContent].GetString(0, string.Empty), ContinuityOfContent.Unknown); }
				set {
					if (value == ContinuityOfContent.Unknown)
						throw new ArgumentNullException("value", "Continuity of Content is Type 1 Required.");
					SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.ContinuityOfContent], value);
				}
			}

			/// <summary>
			/// Gets or sets the value of ContentTemplateSequence in the underlying collection. Type 1C.
			/// </summary>
			ContentTemplateSequence IContainerMacro.ContentTemplateSequence {
				get {
					DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ContentTemplateSequence];
					if (dicomAttribute.IsNull || dicomAttribute.Count == 0) {
						return null;
					}
					return new ContentTemplateSequence(((DicomSequenceItem[])dicomAttribute.Values)[0]);
				}
				set {
					if (value == null) {
						base.DicomAttributeProvider[DicomTags.ContentTemplateSequence] = null;
						return;
					}
					base.DicomAttributeProvider[DicomTags.ContentTemplateSequence].Values = new DicomSequenceItem[] { value.DicomSequenceItem };
				}
			}

			/// <summary>
			/// Creates the value of ContentTemplateSequence in the underlying collection. Type 1C.
			/// </summary>
			ContentTemplateSequence IContainerMacro.CreateContentTemplateSequence() {
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ContentTemplateSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0) {
					DicomSequenceItem dicomSequenceItem = new DicomSequenceItem();
					dicomAttribute.Values = new DicomSequenceItem[] { dicomSequenceItem };
					ContentTemplateSequence iodBase = new ContentTemplateSequence(dicomSequenceItem);
					iodBase.InitializeAttributes();
					return iodBase;
				}
				return new ContentTemplateSequence(((DicomSequenceItem[])dicomAttribute.Values)[0]);
			}

			#endregion

			#region IDocumentRelationshipMacro Members

			/// <summary>
			/// Gets or sets the value of ObservationDateTime in the underlying collection. Type 1C.
			/// </summary>
			public DateTime? ObservationDateTime
			{
				get { return base.DicomAttributeProvider[DicomTags.ObservationDateTime].GetDateTime(0); }
				set
				{
					if (!value.HasValue)
					{
						base.DicomAttributeProvider[DicomTags.ObservationDateTime] = null;
						return;
					}
					base.DicomAttributeProvider[DicomTags.ObservationDateTime].SetDateTime(0, value);
				}
			}

			/// <summary>
			/// Gets or sets the value of ContentSequence in the underlying collection. Type 1C.
			/// </summary>
			public IContentSequence[] ContentSequence
			{
				get
				{
					DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ContentSequence];
					if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
					{
						return null;
					}

					IContentSequence[] result = new IContentSequence[dicomAttribute.Count];
					DicomSequenceItem[] items = (DicomSequenceItem[]) dicomAttribute.Values;
					for (int n = 0; n < items.Length; n++)
						result[n] = new ContentSequenceType(items[n]);

					return result;
				}
				set
				{
					if (value == null || value.Length == 0)
					{
						base.DicomAttributeProvider[DicomTags.ContentSequence] = null;
						return;
					}

					DicomSequenceItem[] result = new DicomSequenceItem[value.Length];
					for (int n = 0; n < value.Length; n++)
						result[n] = value[n].DicomSequenceItem;

					base.DicomAttributeProvider[DicomTags.ContentSequence].Values = result;
				}
			}

			/// <summary>
			/// Creates a single instance of a ContentSequence item. Does not modify the ContentSequence in the underlying collection.
			/// </summary>
			public IContentSequence CreateContentSequence()
			{
				ContentSequenceType iodBase = new ContentSequenceType(new DicomSequenceItem());
				iodBase.InitializeAttributes();
				return iodBase;
			}

			#endregion
		}

		public enum RelationshipType
		{
			Contains,
			HasProperties,
			HasObsContext,
			HasAcqContext,
			InferredFrom,
			SelectedFrom,
			HasConceptMod,
			Unknown
		}
	}
}