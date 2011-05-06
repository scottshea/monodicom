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
using ClearCanvas.Dicom.Iod.Modules.PresentationStateMask;

namespace ClearCanvas.Dicom.Iod.Modules
{
	/// <summary>
	/// PresentationStateMask Module
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.11.13 (Table C.11.13-1)</remarks>
	public class PresentationStateMaskModuleIod : IodBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PresentationStateMaskModuleIod"/> class.
		/// </summary>	
		public PresentationStateMaskModuleIod() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="PresentationStateMaskModuleIod"/> class.
		/// </summary>
		public PresentationStateMaskModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider) { }

		/// <summary>
		/// Initializes the underlying collection to implement the module or sequence using default values.
		/// </summary>
		public void InitializeAttributes()
		{
			this.MaskSubtractionSequence = null;
			this.RecommendedViewingMode = RecommendedViewingMode.None;
		}

		/// <summary>
		/// Gets or sets the value of MaskSubtractionSequence in the underlying collection. Type 3.
		/// </summary>
		public IMaskSubtractionSequence MaskSubtractionSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.MaskSubtractionSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
				{
					return null;
				}
				return new MaskSubtractionSequenceItem(((DicomSequenceItem[]) dicomAttribute.Values)[0]);
			}
			set
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.MaskSubtractionSequence];
				if (value == null)
				{
					base.DicomAttributeProvider[DicomTags.MaskSubtractionSequence] = null;
					return;
				}
				dicomAttribute.Values = new DicomSequenceItem[] {value.DicomSequenceItem};
			}
		}

		/// <summary>
		/// Creates the MaskSubtractionSequence in the underlying collection. Type 3.
		/// </summary>
		public IMaskSubtractionSequence CreateMaskSubtractionSequence()
		{
			DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.MaskSubtractionSequence];
			if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
			{
				DicomSequenceItem dicomSequenceItem = new DicomSequenceItem();
				dicomAttribute.Values = new DicomSequenceItem[] {dicomSequenceItem};
				IMaskSubtractionSequence sequenceType = new MaskSubtractionSequenceItem(dicomSequenceItem);
				sequenceType.InitializeAttributes();
				return sequenceType;
			}
			return new MaskSubtractionSequenceItem(((DicomSequenceItem[]) dicomAttribute.Values)[0]);
		}

		/// <summary>
		/// Gets or sets the value of RecommendedViewingMode in the underlying collection. Type 1C.
		/// </summary>
		public RecommendedViewingMode RecommendedViewingMode
		{
			get { return ParseEnum(base.DicomAttributeProvider[DicomTags.RecommendedViewingMode].GetString(0, string.Empty), RecommendedViewingMode.None); }
			set
			{
				if (value == RecommendedViewingMode.None)
				{
					base.DicomAttributeProvider[DicomTags.RecommendedViewingMode] = null;
					return;
				}
				SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.RecommendedViewingMode], value);
			}
		}

		/// <summary>
		/// MaskSubtraction Sequence
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.11.13 (Table C.11.13-1)</remarks>
		internal class MaskSubtractionSequenceItem : SequenceIodBase, IMaskSubtractionSequence
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="MaskSubtractionSequence"/> class.
			/// </summary>
			public MaskSubtractionSequenceItem() : base() {}

			/// <summary>
			/// Initializes a new instance of the <see cref="MaskSubtractionSequence"/> class.
			/// </summary>
			/// <param name="dicomSequenceItem">The dicom sequence item.</param>
			public MaskSubtractionSequenceItem(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

			/// <summary>
			/// Initializes the underlying collection to implement the module or sequence using default values.
			/// </summary>
			public void InitializeAttributes() {}

			/// <summary>
			/// Gets or sets the value of MaskOperation in the underlying collection. Type 1.
			/// </summary>
			public MaskOperation MaskOperation
			{
				get { return ParseEnum(base.DicomAttributeProvider[DicomTags.MaskOperation].GetString(0, string.Empty), MaskOperation.None); }
				set
				{
					if (value == MaskOperation.None)
						throw new ArgumentOutOfRangeException("value", "MaskOperation is Type 1 Required.");
					SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.MaskOperation], value);
				}
			}

			/// <summary>
			/// Gets or sets the value of ContrastFrameAveraging in the underlying collection. Type 1C.
			/// </summary>
			public int? ContrastFrameAveraging
			{
				get
				{
					int result;
					if (base.DicomAttributeProvider[DicomTags.ContrastFrameAveraging].TryGetInt32(0, out result))
						return result;
					return null;
				}
				set
				{
					if (!value.HasValue)
					{
						base.DicomAttributeProvider[DicomTags.ContrastFrameAveraging] = null;
						return;
					}
					base.DicomAttributeProvider[DicomTags.ContrastFrameAveraging].SetInt32(0, value.Value);
				}
			}
		}

		/// <summary>
		/// Gets an enumeration of <see cref="DicomTag"/>s used by this module.
		/// </summary>
		public static IEnumerable<uint> DefinedTags {
			get {
				yield return DicomTags.MaskSubtractionSequence;
				yield return DicomTags.RecommendedViewingMode;
			}
		}
	}

	namespace PresentationStateMask
	{
		/// <summary>
		/// MaskSubtraction Sequence
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.11.13 (Table C.11.13-1)</remarks>
		public interface IMaskSubtractionSequence : IIodMacro
		{
			/// <summary>
			/// Gets or sets the value of MaskOperation in the underlying collection. Type 1.
			/// </summary>
			MaskOperation MaskOperation { get; set; }

			/// <summary>
			/// Gets or sets the value of ContrastFrameAveraging in the underlying collection. Type 1C.
			/// </summary>
			int? ContrastFrameAveraging { get; set; }
		}

		/// <summary>
		/// Enumerated values for the <see cref="DicomTags.RecommendedViewingMode"/> attribute .
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.11.13 (Table C.11-13-1)</remarks>
		public enum RecommendedViewingMode {
			Sub,

			/// <summary>
			/// Represents the null value, which is equivalent to the unknown status.
			/// </summary>
			None
		}

		/// <summary>
		/// Enumerated values for the <see cref="DicomTags.MaskOperation"/> attribute .
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.11.13 (Table C.11.13-1)</remarks>
		public enum MaskOperation {
			Avg_Sub,
			Tid,

			/// <summary>
			/// Represents the null value, which is equivalent to the unknown status.
			/// </summary>
			None
		}
	}
}
