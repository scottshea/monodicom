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
using System.Drawing;
using ClearCanvas.Dicom.Iod.Macros;

namespace ClearCanvas.Dicom.Iod.Modules
{
	/// <summary>
	/// DisplayedArea Module
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.10.4 (Table C.10-4)</remarks>
	public class DisplayedAreaModuleIod : IodBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DisplayedAreaModuleIod"/> class.
		/// </summary>	
		public DisplayedAreaModuleIod() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="DisplayedAreaModuleIod"/> class.
		/// </summary>
		public DisplayedAreaModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider) { }

		/// <summary>
		/// Initializes the underlying collection to implement the module or sequence using default values.
		/// </summary>
		public void InitializeAttributes() {}

		/// <summary>
		/// Gets or sets the value of DisplayedAreaSelectionSequence in the underlying collection. Type 1.
		/// </summary>
		public DisplayedAreaSelectionSequenceItem[] DisplayedAreaSelectionSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.DisplayedAreaSelectionSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
					return null;

				DisplayedAreaSelectionSequenceItem[] result = new DisplayedAreaSelectionSequenceItem[dicomAttribute.Count];
				DicomSequenceItem[] items = (DicomSequenceItem[]) dicomAttribute.Values;
				for (int n = 0; n < items.Length; n++)
					result[n] = new DisplayedAreaSelectionSequenceItem(items[n]);

				return result;
			}
			set
			{
				if (value == null || value.Length == 0)
					throw new ArgumentNullException("value", "DisplayedAreaSelectionSequence is Type 1 Required.");

				DicomSequenceItem[] result = new DicomSequenceItem[value.Length];
				for (int n = 0; n < value.Length; n++)
					result[n] = value[n].DicomSequenceItem;

				base.DicomAttributeProvider[DicomTags.DisplayedAreaSelectionSequence].Values = result;
			}
		}

		/// <summary>
		/// DisplayedAreaSelection Sequence
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.10.4 (Table C.10-4)</remarks>
		public class DisplayedAreaSelectionSequenceItem : SequenceIodBase
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="DisplayedAreaSelectionSequenceItem"/> class.
			/// </summary>
			public DisplayedAreaSelectionSequenceItem() : base() {}

			/// <summary>
			/// Initializes a new instance of the <see cref="DisplayedAreaSelectionSequenceItem"/> class.
			/// </summary>
			/// <param name="dicomSequenceItem">The dicom sequence item.</param>
			public DisplayedAreaSelectionSequenceItem(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

			/// <summary>
			/// Initializes the underlying collection to implement the module or sequence using default values.
			/// </summary>
			public void InitializeAttributes()
			{
				this.ReferencedImageSequence = null;
				this.PresentationPixelSpacing = null;
				this.PresentationPixelAspectRatio = null;
				this.PresentationPixelMagnificationRatio = null;
			}

			/// <summary>
			/// Gets or sets the value of ReferencedImageSequence in the underlying collection. Type 1C.
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
					{
						base.DicomAttributeProvider[DicomTags.ReferencedImageSequence] = null;
						return;
					}

					DicomSequenceItem[] result = new DicomSequenceItem[value.Length];
					for (int n = 0; n < value.Length; n++)
						result[n] = value[n].DicomSequenceItem;

					base.DicomAttributeProvider[DicomTags.ReferencedImageSequence].Values = result;
				}
			}

			/// <summary>
			/// Gets or sets the value of DisplayedAreaTopLeftHandCorner in the underlying collection. Type 1.
			/// </summary>
			public Point DisplayedAreaTopLeftHandCorner
			{
				get
				{
					int x, y;
					if (base.DicomAttributeProvider[DicomTags.DisplayedAreaTopLeftHandCorner].TryGetInt32(0, out x))
						if (base.DicomAttributeProvider[DicomTags.DisplayedAreaTopLeftHandCorner].TryGetInt32(1, out y))
							return new Point(x, y);
					return Point.Empty;
				}
				set
				{
					base.DicomAttributeProvider[DicomTags.DisplayedAreaTopLeftHandCorner].SetInt32(0, value.X);
					base.DicomAttributeProvider[DicomTags.DisplayedAreaTopLeftHandCorner].SetInt32(1, value.Y);
				}
			}

			/// <summary>
			/// Gets or sets the value of DisplayedAreaBottomRightHandCorner in the underlying collection. Type 1.
			/// </summary>
			public Point DisplayedAreaBottomRightHandCorner
			{
				get
				{
					int x, y;
					if (base.DicomAttributeProvider[DicomTags.DisplayedAreaBottomRightHandCorner].TryGetInt32(0, out x))
						if (base.DicomAttributeProvider[DicomTags.DisplayedAreaBottomRightHandCorner].TryGetInt32(1, out y))
							return new Point(x, y);
					return Point.Empty;
				}
				set
				{
					base.DicomAttributeProvider[DicomTags.DisplayedAreaBottomRightHandCorner].SetInt32(0, value.X);
					base.DicomAttributeProvider[DicomTags.DisplayedAreaBottomRightHandCorner].SetInt32(1, value.Y);
				}
			}

			/// <summary>
			/// Gets or sets the value of PresentationSizeMode in the underlying collection. Type 1.
			/// </summary>
			public PresentationSizeMode PresentationSizeMode
			{
				get { return ParseEnum(base.DicomAttributeProvider[DicomTags.PresentationSizeMode].GetString(0, string.Empty), PresentationSizeMode.None); }
				set
				{
					if (value == PresentationSizeMode.None)
						throw new ArgumentOutOfRangeException("value", "PresentationSizeMode is Type 1 Required.");
					SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.PresentationSizeMode], value, true);
				}
			}

			/// <summary>
			/// Gets or sets the value of PresentationPixelSpacing in the underlying collection. Type 1C.
			/// </summary>
			public PixelSpacing PresentationPixelSpacing
			{
				get
				{
					DicomAttribute attribute = base.DicomAttributeProvider[DicomTags.PresentationPixelSpacing];
					if (attribute.IsEmpty || attribute.IsNull)
						return null;
					return PixelSpacing.FromString(attribute.ToString());
				}
				set
				{
					if (value == null || value.IsNull)
					{
						base.DicomAttributeProvider[DicomTags.PresentationPixelSpacing] = null;
						return;
					}
					base.DicomAttributeProvider[DicomTags.PresentationPixelSpacing].SetStringValue(value.ToString());
				}
			}

			/// <summary>
			/// Gets or sets the value of PresentationPixelAspectRatio in the underlying collection. Type 1C.
			/// </summary>
			public PixelAspectRatio PresentationPixelAspectRatio
			{
				get
				{
					DicomAttribute attribute = base.DicomAttributeProvider[DicomTags.PresentationPixelAspectRatio];
					if (attribute.IsEmpty || attribute.IsNull)
						return null;
					return PixelAspectRatio.FromString(attribute.ToString());
				}
				set
				{
					if (value == null || value.IsNull)
					{
						base.DicomAttributeProvider[DicomTags.PresentationPixelAspectRatio] = null;
						return;
					}
					base.DicomAttributeProvider[DicomTags.PresentationPixelAspectRatio].SetStringValue(value.ToString());
				}
			}

			/// <summary>
			/// Gets or sets the value of PresentationPixelMagnificationRatio in the underlying collection. Type 1C.
			/// </summary>
			public double? PresentationPixelMagnificationRatio
			{
				get
				{
					double result;
					if (base.DicomAttributeProvider[DicomTags.PresentationPixelMagnificationRatio].TryGetFloat64(0, out result))
						return result;
					return null;
				}
				set
				{
					if (!value.HasValue)
					{
						base.DicomAttributeProvider[DicomTags.PresentationPixelMagnificationRatio] = null;
						return;
					}
					base.DicomAttributeProvider[DicomTags.PresentationPixelMagnificationRatio].SetFloat64(0, value.Value);
				}
			}
		}

		/// <summary>
		/// Gets an enumeration of <see cref="DicomTag"/>s used by this module.
		/// </summary>
		public static IEnumerable<uint> DefinedTags {
			get {
				yield return DicomTags.DisplayedAreaSelectionSequence;
			}
		}

		/// <summary>
		/// Enumerated values for the <see cref="DicomTags.PresentationSizeMode"/> attribute .
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.10.4 (Table C.10-4)</remarks>
		public enum PresentationSizeMode
		{
			ScaleToFit,
			TrueSize,
			Magnify,

			/// <summary>
			/// Represents the null value, which is equivalent to the unknown status.
			/// </summary>
			None
		}
	}
}