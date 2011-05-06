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
using ClearCanvas.Dicom.Iod.Macros.PresentationStateRelationship;

namespace ClearCanvas.Dicom.Iod.Macros
{
	/// <summary>
	/// PresentationStateRelationship Macro
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.11.11 (Table C.11.11-1b)</remarks>
	public interface IPresentationStateRelationshipMacro : IIodMacro
	{
		/// <summary>
		/// Gets or sets the value of ReferencedSeriesSequence in the underlying collection. Type 1.
		/// </summary>
		IReferencedSeriesSequence[] ReferencedSeriesSequence { get; set; }

		/// <summary>
		/// Creates a single instance of a ReferencedSeriesSequence item. Does not modify the ReferencedSeriesSequence in the underlying collection.
		/// </summary>
		IReferencedSeriesSequence CreateReferencedSeriesSequence();
	}

	/// <summary>
	/// PresentationStateRelationship Macro
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.11.11 (Table C.11.11-1b)</remarks>
	internal class PresentationStateRelationshipMacro : SequenceIodBase, IPresentationStateRelationshipMacro
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PresentationStateRelationshipMacro"/> class.
		/// </summary>
		public PresentationStateRelationshipMacro() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="PresentationStateRelationshipMacro"/> class.
		/// </summary>
		/// <param name="dicomSequenceItem">The dicom sequence item.</param>
		public PresentationStateRelationshipMacro(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

		/// <summary>
		/// Initializes the underlying collection to implement the module or sequence using default values.
		/// </summary>
		public void InitializeAttributes() {}

		/// <summary>
		/// Gets or sets the value of ReferencedSeriesSequence in the underlying collection. Type 1.
		/// </summary>
		public IReferencedSeriesSequence[] ReferencedSeriesSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ReferencedSeriesSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
					return null;

				IReferencedSeriesSequence[] result = new IReferencedSeriesSequence[dicomAttribute.Count];
				DicomSequenceItem[] items = (DicomSequenceItem[]) dicomAttribute.Values;
				for (int n = 0; n < items.Length; n++)
					result[n] = new ReferencedSeriesSequenceItem(items[n]);

				return result;
			}
			set
			{
				if (value == null || value.Length == 0)
					throw new ArgumentNullException("value", "ReferencedSeriesSequence is Type 1 Required.");

				DicomSequenceItem[] result = new DicomSequenceItem[value.Length];
				for (int n = 0; n < value.Length; n++)
					result[n] = value[n].DicomSequenceItem;

				base.DicomAttributeProvider[DicomTags.ReferencedSeriesSequence].Values = result;
			}
		}

		/// <summary>
		/// Creates a single instance of a ReferencedSeriesSequence item. Does not modify the ReferencedSeriesSequence in the underlying collection.
		/// </summary>
		public IReferencedSeriesSequence CreateReferencedSeriesSequence()
		{
			IReferencedSeriesSequence iodBase = new ReferencedSeriesSequenceItem(new DicomSequenceItem());
			iodBase.InitializeAttributes();
			return iodBase;
		}

		/// <summary>
		/// ReferencedSeries Sequence
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.11.11 (Table C.11.11-1b)</remarks>
		internal class ReferencedSeriesSequenceItem : SequenceIodBase, IReferencedSeriesSequence
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="ReferencedSeriesSequenceItem"/> class.
			/// </summary>
			public ReferencedSeriesSequenceItem() : base() {}

			/// <summary>
			/// Initializes a new instance of the <see cref="ReferencedSeriesSequenceItem"/> class.
			/// </summary>
			/// <param name="dicomSequenceItem">The dicom sequence item.</param>
			public ReferencedSeriesSequenceItem(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

			/// <summary>
			/// Initializes the underlying collection to implement the module or sequence using default values.
			/// </summary>
			public void InitializeAttributes() {}

			/// <summary>
			/// Gets or sets the value of SeriesInstanceUid in the underlying collection. Type 1.
			/// </summary>
			public string SeriesInstanceUid
			{
				get { return base.DicomAttributeProvider[DicomTags.SeriesInstanceUid].GetString(0, string.Empty); }
				set
				{
					if (string.IsNullOrEmpty(value))
						throw new ArgumentNullException("value", "SeriesInstanceUid is Type 1 Required.");
					base.DicomAttributeProvider[DicomTags.SeriesInstanceUid].SetString(0, value);
				}
			}

			/// <summary>
			/// Gets or sets the value of ReferencedImageSequence in the underlying collection. Type 1.
			/// </summary>
			public ImageSopInstanceReferenceMacro[] ReferencedImageSequence
			{
				get
				{
					DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ReferencedImageSequence];
					if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
						return null;

					ImageSopInstanceReferenceMacro[] result = new ImageSopInstanceReferenceMacro[dicomAttribute.Count];
					DicomSequenceItem[] items = (DicomSequenceItem[]) dicomAttribute.Values;
					for (int n = 0; n < items.Length; n++)
						result[n] = new ImageSopInstanceReferenceMacro(items[n]);

					return result;
				}
				set
				{
					if (value == null || value.Length == 0)
						throw new ArgumentNullException("value", "ReferencedImageSequence is Type 1 Required.");

					DicomSequenceItem[] result = new DicomSequenceItem[value.Length];
					for (int n = 0; n < value.Length; n++)
						result[n] = value[n].DicomSequenceItem;

					base.DicomAttributeProvider[DicomTags.ReferencedImageSequence].Values = result;
				}
			}
		}
	}

	namespace PresentationStateRelationship
	{
		public interface IReferencedSeriesSequence : IIodMacro
		{
			/// <summary>
			/// Gets or sets the value of SeriesInstanceUid in the underlying collection. Type 1.
			/// </summary>
			string SeriesInstanceUid { get; set; }

			/// <summary>
			/// Gets or sets the value of ReferencedImageSequence in the underlying collection. Type 1.
			/// </summary>
			ImageSopInstanceReferenceMacro[] ReferencedImageSequence { get; set; }
		}
	}
}