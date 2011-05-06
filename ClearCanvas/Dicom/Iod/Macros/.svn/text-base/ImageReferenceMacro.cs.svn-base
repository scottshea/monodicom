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
using ClearCanvas.Dicom.Iod.Macros.ImageReference;

namespace ClearCanvas.Dicom.Iod.Macros
{
	/// <summary>
	/// ImageReference Macro
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.18.4 (Table C.18.4-1)</remarks>
	public interface IImageReferenceMacro : ICompositeObjectReferenceMacro
	{
		/// <summary>
		/// Gets or sets the value of ReferencedSopSequence in the underlying collection. Type 1.
		/// </summary>
		new IReferencedSopSequence ReferencedSopSequence { get; set; }

		/// <summary>
		/// Creates the value of ReferencedSopSequence in the underlying collection. Type 1.
		/// </summary>
		new IReferencedSopSequence CreateReferencedSopSequence();
	}

	namespace ImageReference
	{
		/// <summary>
		/// ReferencedSop Sequence of the ImageReference Macro
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.18.4 (Table C.18.4-1)</remarks>
		public interface IReferencedSopSequence : ISopInstanceReferenceMacro
		{
			/// <summary>
			/// Gets or sets the value of ReferencedFrameNumber in the underlying collection. Type 1C.
			/// </summary>
			string ReferencedFrameNumber { get; set; }

			/// <summary>
			/// Gets or sets the value of ReferencedSegmentNumber in the underlying collection. Type 1C.
			/// </summary>
			string ReferencedSegmentNumber { get; set; }

			/// <summary>
			/// Gets or sets the value of ReferencedSopSequence in the underlying collection. Type 3.
			/// </summary>
			ISopInstanceReferenceMacro ReferencedSopSequence { get; set; }

			/// <summary>
			/// Creates the value of ReferencedSopSequence in the underlying collection. Type 3.
			/// </summary>
			ISopInstanceReferenceMacro CreateReferencedSopSequence();

			/// <summary>
			/// Gets or sets the value of ReferencedRealWorldValueMappingInstanceSequence in the underlying collection. Type 3.
			/// </summary>
			ISopInstanceReferenceMacro ReferencedRealWorldValueMappingInstanceSequence { get; set; }

			/// <summary>
			/// Creates the value of ReferencedRealWorldValueMappingInstanceSequence in the underlying collection. Type 3.
			/// </summary>
			ISopInstanceReferenceMacro CreateReferencedRealWorldValueMappingInstanceSequence();

			/// <summary>
			/// Gets or sets the value of ImageIconSequence in the underlying collection. Type 3.
			/// </summary>
			object ImageIconSequence { get; set; }
		}
	}

	/// <summary>
	/// ImageReference Macro Base Implementation
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.18.4 (Table C.18.4-1)</remarks>
	internal class ImageReferenceMacro : SequenceIodBase, IImageReferenceMacro
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ImageReferenceMacro"/> class.
		/// </summary>
		public ImageReferenceMacro() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageReferenceMacro"/> class.
		/// </summary>
		/// <param name="dicomSequenceItem">The dicom sequence item.</param>
		public ImageReferenceMacro(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

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
		ISopInstanceReferenceMacro ICompositeObjectReferenceMacro.ReferencedSopSequence
		{
			get { return this.ReferencedSopSequence; }
			set { this.ReferencedSopSequence = (IReferencedSopSequence) value; }
		}

		/// <summary>
		/// Gets or sets the value of ReferencedSopSequence in the underlying collection. Type 1.
		/// </summary>
		public IReferencedSopSequence ReferencedSopSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ReferencedSopSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
				{
					return null;
				}
				return new ReferencedSopSequenceType(((DicomSequenceItem[]) dicomAttribute.Values)[0]);
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
		ISopInstanceReferenceMacro ICompositeObjectReferenceMacro.CreateReferencedSopSequence()
		{
			return this.CreateReferencedSopSequence();
		}

		/// <summary>
		/// Creates the value of ReferencedSopSequence in the underlying collection. Type 1.
		/// </summary>
		public IReferencedSopSequence CreateReferencedSopSequence()
		{
			DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ReferencedSopSequence];
			if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
			{
				DicomSequenceItem dicomSequenceItem = new DicomSequenceItem();
				dicomAttribute.Values = new DicomSequenceItem[] {dicomSequenceItem};
				ReferencedSopSequenceType iodBase = new ReferencedSopSequenceType(dicomSequenceItem);
				iodBase.InitializeAttributes();
				return iodBase;
			}
			return new ReferencedSopSequenceType(((DicomSequenceItem[]) dicomAttribute.Values)[0]);
		}

		/// <summary>
		/// ReferencedSop Sequence of the ImageReference Macro Base Implementation
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.18.4 (Table C.18.4-1)</remarks>
		internal class ReferencedSopSequenceType : SopInstanceReferenceMacro, IReferencedSopSequence
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="ReferencedSopSequenceType"/> class.
			/// </summary>
			public ReferencedSopSequenceType() : base() {}

			/// <summary>
			/// Initializes a new instance of the <see cref="ReferencedSopSequenceType"/> class.
			/// </summary>
			/// <param name="dicomSequenceItem">The dicom sequence item.</param>
			public ReferencedSopSequenceType(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

			/// <summary>
			/// Initializes the underlying collection to implement the module using default values.
			/// </summary>
			public override void InitializeAttributes()
			{
				base.InitializeAttributes();
				this.ReferencedFrameNumber = null;
				this.ReferencedSegmentNumber = null;
				this.ReferencedSopSequence = null;
				this.ReferencedRealWorldValueMappingInstanceSequence = null;
				this.ImageIconSequence = null;
			}

			/// <summary>
			/// Gets or sets the value of ReferencedFrameNumber in the underlying collection. Type 1C.
			/// </summary>
			public string ReferencedFrameNumber
			{
				get { return base.DicomAttributeProvider[DicomTags.ReferencedFrameNumber].ToString(); }
				set
				{
					if (string.IsNullOrEmpty(value))
						base.DicomAttributeProvider[DicomTags.ReferencedFrameNumber] = null;
					base.DicomAttributeProvider[DicomTags.ReferencedFrameNumber].SetStringValue(value);
				}
			}

			/// <summary>
			/// Gets or sets the value of ReferencedSegmentNumber in the underlying collection. Type 1C.
			/// </summary>
			public string ReferencedSegmentNumber
			{
				get { return base.DicomAttributeProvider[DicomTags.ReferencedSegmentNumber].ToString(); }
				set
				{
					if (string.IsNullOrEmpty(value))
						base.DicomAttributeProvider[DicomTags.ReferencedSegmentNumber] = null;
					base.DicomAttributeProvider[DicomTags.ReferencedSegmentNumber].SetStringValue(value);
				}
			}

			/// <summary>
			/// Gets or sets the value of ReferencedSopSequence in the underlying collection. Type 3.
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
					{
						base.DicomAttributeProvider[DicomTags.ReferencedSopSequence] = null;
						return;
					}
					base.DicomAttributeProvider[DicomTags.ReferencedSopSequence].Values = new DicomSequenceItem[] {value.DicomSequenceItem};
				}
			}

			/// <summary>
			/// Creates the value of ReferencedSopSequence in the underlying collection. Type 3.
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

			/// <summary>
			/// Gets or sets the value of ReferencedRealWorldValueMappingInstanceSequence in the underlying collection. Type 3.
			/// </summary>
			public ISopInstanceReferenceMacro ReferencedRealWorldValueMappingInstanceSequence
			{
				get
				{
					DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ReferencedRealWorldValueMappingInstanceSequence];
					if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
					{
						return null;
					}
					return new SopInstanceReferenceMacro(((DicomSequenceItem[]) dicomAttribute.Values)[0]);
				}
				set
				{
					if (value == null)
					{
						base.DicomAttributeProvider[DicomTags.ReferencedRealWorldValueMappingInstanceSequence] = null;
						return;
					}
					base.DicomAttributeProvider[DicomTags.ReferencedRealWorldValueMappingInstanceSequence].Values = new DicomSequenceItem[] {value.DicomSequenceItem};
				}
			}

			/// <summary>
			/// Creates the value of ReferencedRealWorldValueMappingInstanceSequence in the underlying collection. Type 3.
			/// </summary>
			public ISopInstanceReferenceMacro CreateReferencedRealWorldValueMappingInstanceSequence()
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ReferencedRealWorldValueMappingInstanceSequence];
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

			/// <summary>
			/// NOT IMPLEMENTED. Gets or sets the value of ImageIconSequence in the underlying collection. Type 3.
			/// </summary>
			public object ImageIconSequence
			{
				// TODO - Implement this.
				get { throw new NotImplementedException(); }
				set { 
					//throw new NotImplementedException(); 
				}
			}
		}
	}
}