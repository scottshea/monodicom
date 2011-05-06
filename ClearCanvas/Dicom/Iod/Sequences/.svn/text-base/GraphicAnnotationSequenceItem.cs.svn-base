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
using System.Drawing;
using ClearCanvas.Common;
using ClearCanvas.Dicom.Iod.Macros;

namespace ClearCanvas.Dicom.Iod.Sequences
{
	/// <summary>
	/// GraphicAnnotation Sequence
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.10.5 (Table C.10-5)</remarks>
	public class GraphicAnnotationSequenceItem : SequenceIodBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GraphicAnnotationSequenceItem"/> class.
		/// </summary>
		public GraphicAnnotationSequenceItem() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="GraphicAnnotationSequenceItem"/> class.
		/// </summary>
		/// <param name="dicomSequenceItem">The dicom sequence item.</param>
		public GraphicAnnotationSequenceItem(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

		/// <summary>
		/// Gets or sets the value of ReferencedImageSequence in the underlying collection. Type 1C.
		/// </summary>
		public ImageSopInstanceReferenceMacro[] ReferencedImageSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ReferencedImageSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
				{
					return null;
				}

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
		/// Gets or sets the value of GraphicLayer in the underlying collection. Type 1.
		/// </summary>
		public string GraphicLayer
		{
			get { return base.DicomAttributeProvider[DicomTags.GraphicLayer].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException("value", "GraphicLayer is Type 1 Required.");
				base.DicomAttributeProvider[DicomTags.GraphicLayer].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of TextObjectSequence in the underlying collection. Type 1C.
		/// </summary>
		public TextObjectSequenceItem[] TextObjectSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.TextObjectSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
				{
					return null;
				}

				TextObjectSequenceItem[] result = new TextObjectSequenceItem[dicomAttribute.Count];
				DicomSequenceItem[] items = (DicomSequenceItem[]) dicomAttribute.Values;
				for (int n = 0; n < items.Length; n++)
					result[n] = new TextObjectSequenceItem(items[n]);

				return result;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					base.DicomAttributeProvider[DicomTags.TextObjectSequence] = null;
					return;
				}

				DicomSequenceItem[] result = new DicomSequenceItem[value.Length];
				for (int n = 0; n < value.Length; n++)
					result[n] = value[n].DicomSequenceItem;

				base.DicomAttributeProvider[DicomTags.TextObjectSequence].Values = result;
			}
		}

		/// <summary>
		/// Appends a value to the TextObjectSequence in the underlying collection. Type 1C.
		/// </summary>
		public void AppendTextObjectSequence(TextObjectSequenceItem text)
		{
			Platform.CheckForNullReference(text, "text");
			base.DicomAttributeProvider[DicomTags.TextObjectSequence].AddSequenceItem(text.DicomSequenceItem);
		}

		/// <summary>
		/// Gets or sets the value of GraphicObjectSequence in the underlying collection. Type 1C.
		/// </summary>
		public GraphicObjectSequenceItem[] GraphicObjectSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.GraphicObjectSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
				{
					return null;
				}

				GraphicObjectSequenceItem[] result = new GraphicObjectSequenceItem[dicomAttribute.Count];
				DicomSequenceItem[] items = (DicomSequenceItem[]) dicomAttribute.Values;
				for (int n = 0; n < items.Length; n++)
					result[n] = new GraphicObjectSequenceItem(items[n]);

				return result;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					base.DicomAttributeProvider[DicomTags.GraphicObjectSequence] = null;
					return;
				}

				DicomSequenceItem[] result = new DicomSequenceItem[value.Length];
				for (int n = 0; n < value.Length; n++)
					result[n] = value[n].DicomSequenceItem;

				base.DicomAttributeProvider[DicomTags.GraphicObjectSequence].Values = result;
			}
		}

		/// <summary>
		/// Appends a value to the GraphicObjectSequence in the underlying collection. Type 1C.
		/// </summary>
		public void AppendGraphicObjectSequence(GraphicObjectSequenceItem graphic) {
			Platform.CheckForNullReference(graphic, "graphic");
			base.DicomAttributeProvider[DicomTags.GraphicObjectSequence].AddSequenceItem(graphic.DicomSequenceItem);
		}

		/// <summary>
		/// TextObject Sequence
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.10.5 (Table C.10-5)</remarks>
		public class TextObjectSequenceItem : SequenceIodBase
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="TextObjectSequenceItem"/> class.
			/// </summary>
			public TextObjectSequenceItem() : base() {}

			/// <summary>
			/// Initializes a new instance of the <see cref="TextObjectSequenceItem"/> class.
			/// </summary>
			/// <param name="dicomSequenceItem">The dicom sequence item.</param>
			public TextObjectSequenceItem(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

			/// <summary>
			/// Gets or sets the value of BoundingBoxAnnotationUnits in the underlying collection. Type 1C.
			/// </summary>
			public BoundingBoxAnnotationUnits BoundingBoxAnnotationUnits
			{
				get { return ParseEnum(base.DicomAttributeProvider[DicomTags.BoundingBoxAnnotationUnits].GetString(0, string.Empty), BoundingBoxAnnotationUnits.None); }
				set
				{
					if (value == BoundingBoxAnnotationUnits.None)
					{
						base.DicomAttributeProvider[DicomTags.BoundingBoxAnnotationUnits] = null;
						return;
					}
					SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.BoundingBoxAnnotationUnits], value);
				}
			}

			/// <summary>
			/// Gets or sets the value of AnchorPointAnnotationUnits in the underlying collection. Type 1C.
			/// </summary>
			public AnchorPointAnnotationUnits AnchorPointAnnotationUnits
			{
				get { return ParseEnum(base.DicomAttributeProvider[DicomTags.AnchorPointAnnotationUnits].GetString(0, string.Empty), AnchorPointAnnotationUnits.None); }
				set
				{
					if (value == AnchorPointAnnotationUnits.None)
					{
						base.DicomAttributeProvider[DicomTags.AnchorPointAnnotationUnits] = null;
						return;
					}
					SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.AnchorPointAnnotationUnits], value);
				}
			}

			/// <summary>
			/// Gets or sets the value of UnformattedTextValue in the underlying collection. Type 1.
			/// </summary>
			public string UnformattedTextValue
			{
				get { return base.DicomAttributeProvider[DicomTags.UnformattedTextValue].ToString(); }
				set
				{
					if (string.IsNullOrEmpty(value))
						throw new ArgumentNullException("value", "UnformattedTextValue is Type 1 Required.");
					base.DicomAttributeProvider[DicomTags.UnformattedTextValue].SetStringValue(value);
				}
			}

			/// <summary>
			/// Gets or sets the value of BoundingBoxTopLeftHandCorner in the underlying collection. Type 1C.
			/// </summary>
			public PointF? BoundingBoxTopLeftHandCorner
			{
				get
				{
					float[] result = new float[2];
					if (base.DicomAttributeProvider[DicomTags.BoundingBoxTopLeftHandCorner].TryGetFloat32(0, out result[0]))
						if (base.DicomAttributeProvider[DicomTags.BoundingBoxTopLeftHandCorner].TryGetFloat32(1, out result[1]))
							return new PointF(result[0], result[1]);
					return null;
				}
				set
				{
					if (!value.HasValue)
					{
						base.DicomAttributeProvider[DicomTags.BoundingBoxTopLeftHandCorner] = null;
						return;
					}
					base.DicomAttributeProvider[DicomTags.BoundingBoxTopLeftHandCorner].SetFloat32(0, value.Value.X);
					base.DicomAttributeProvider[DicomTags.BoundingBoxTopLeftHandCorner].SetFloat32(1, value.Value.Y);
				}
			}

			/// <summary>
			/// Gets or sets the value of BoundingBoxBottomRightHandCorner in the underlying collection. Type 1C.
			/// </summary>
			public PointF? BoundingBoxBottomRightHandCorner
			{
				get
				{
					float[] result = new float[2];
					if (base.DicomAttributeProvider[DicomTags.BoundingBoxBottomRightHandCorner].TryGetFloat32(0, out result[0]))
						if (base.DicomAttributeProvider[DicomTags.BoundingBoxBottomRightHandCorner].TryGetFloat32(1, out result[1]))
							return new PointF(result[0], result[1]);
					return null;
				}
				set
				{
					if (!value.HasValue)
					{
						base.DicomAttributeProvider[DicomTags.BoundingBoxBottomRightHandCorner] = null;
						return;
					}
					base.DicomAttributeProvider[DicomTags.BoundingBoxBottomRightHandCorner].SetFloat32(0, value.Value.X);
					base.DicomAttributeProvider[DicomTags.BoundingBoxBottomRightHandCorner].SetFloat32(1, value.Value.Y);
				}
			}

			/// <summary>
			/// Gets or sets the value of BoundingBoxTextHorizontalJustification in the underlying collection. Type 1C.
			/// </summary>
			public BoundingBoxTextHorizontalJustification BoundingBoxTextHorizontalJustification
			{
				get { return ParseEnum(base.DicomAttributeProvider[DicomTags.BoundingBoxTextHorizontalJustification].GetString(0, string.Empty), BoundingBoxTextHorizontalJustification.None); }
				set
				{
					if (value == BoundingBoxTextHorizontalJustification.None)
					{
						base.DicomAttributeProvider[DicomTags.BoundingBoxTextHorizontalJustification] = null;
						return;
					}
					SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.BoundingBoxTextHorizontalJustification], value);
				}
			}

			/// <summary>
			/// Gets or sets the value of AnchorPoint in the underlying collection. Type 1C.
			/// </summary>
			public PointF? AnchorPoint
			{
				get
				{
					float[] result = new float[2];
					if (base.DicomAttributeProvider[DicomTags.AnchorPoint].TryGetFloat32(0, out result[0]))
						if (base.DicomAttributeProvider[DicomTags.AnchorPoint].TryGetFloat32(1, out result[1]))
							return new PointF(result[0], result[1]);
					return null;
				}
				set
				{
					if (!value.HasValue)
					{
						base.DicomAttributeProvider[DicomTags.AnchorPoint] = null;
						return;
					}
					base.DicomAttributeProvider[DicomTags.AnchorPoint].SetFloat32(0, value.Value.X);
					base.DicomAttributeProvider[DicomTags.AnchorPoint].SetFloat32(1, value.Value.Y);
				}
			}

			/// <summary>
			/// Gets or sets the value of AnchorPointVisibility in the underlying collection. Type 1C.
			/// </summary>
			public AnchorPointVisibility AnchorPointVisibility
			{
				get { return ParseEnum(base.DicomAttributeProvider[DicomTags.AnchorPointVisibility].GetString(0, string.Empty), AnchorPointVisibility.None); }
				set
				{
					if (value == AnchorPointVisibility.None)
					{
						base.DicomAttributeProvider[DicomTags.AnchorPointVisibility] = null;
						return;
					}
					SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.AnchorPointVisibility], value);
				}
			}
		}

		/// <summary>
		/// GraphicObject Sequence
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.10.5 (Table C.10-5)</remarks>
		public class GraphicObjectSequenceItem : SequenceIodBase
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="GraphicObjectSequenceItem"/> class.
			/// </summary>
			public GraphicObjectSequenceItem() : base() {}

			/// <summary>
			/// Initializes a new instance of the <see cref="GraphicObjectSequenceItem"/> class.
			/// </summary>
			/// <param name="dicomSequenceItem">The dicom sequence item.</param>
			public GraphicObjectSequenceItem(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

			/// <summary>
			/// Gets or sets the value of GraphicAnnotationUnits in the underlying collection. Type 1.
			/// </summary>
			public GraphicAnnotationUnits GraphicAnnotationUnits
			{
				get { return ParseEnum(base.DicomAttributeProvider[DicomTags.GraphicAnnotationUnits].GetString(0, string.Empty), GraphicAnnotationUnits.None); }
				set
				{
					if (value == GraphicAnnotationUnits.None)
						throw new ArgumentOutOfRangeException("value", "GraphicAnnotationUnits is Type 1 Required.");
					SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.GraphicAnnotationUnits], value);
				}
			}

			/// <summary>
			/// Gets or sets the value of GraphicDimensions in the underlying collection. Type 1.
			/// </summary>
			public int GraphicDimensions
			{
				get { return base.DicomAttributeProvider[DicomTags.GraphicDimensions].GetInt32(0, 2); }
				set
				{
					if (value != 2)
						throw new ArgumentOutOfRangeException("value", "GraphicDimensions must be 2.");
					base.DicomAttributeProvider[DicomTags.GraphicDimensions].SetInt32(0, value);
				}
			}

			/// <summary>
			/// Gets or sets the value of NumberOfGraphicPoints in the underlying collection. Type 1.
			/// </summary>
			public int NumberOfGraphicPoints
			{
				get { return base.DicomAttributeProvider[DicomTags.NumberOfGraphicPoints].GetInt32(0, 0); }
				set { base.DicomAttributeProvider[DicomTags.NumberOfGraphicPoints].SetInt32(0, value); }
			}

			/// <summary>
			/// Gets or sets the value of GraphicData in the underlying collection. Type 1.
			/// </summary>
			public PointF[] GraphicData
			{
				get
				{
					DicomAttribute attribute = base.DicomAttributeProvider[DicomTags.GraphicData];
					if (attribute.IsEmpty || attribute.IsNull || attribute.Count == 0)
						return null;
					PointF[] points = new PointF[attribute.Count / 2];
					float[] values = (float[]) attribute.Values;
					for (int n = 0; n < points.Length; n++)
						points[n] = new PointF(values[2*n], values[2*n + 1]);
					return points;
				}
				set
				{
					if (value == null || value.Length == 0)
						throw new ArgumentNullException("value", "GraphicData is Type 1 Required.");
					float[] floats = new float[2*value.Length];
					for (int n = 0; n < value.Length; n++)
					{
						floats[2*n] = value[n].X;
						floats[2*n + 1] = value[n].Y;
					}
					base.DicomAttributeProvider[DicomTags.GraphicData].Values = floats;
				}
			}

			/// <summary>
			/// Gets or sets the value of GraphicType in the underlying collection. Type 1.
			/// </summary>
			public GraphicType GraphicType
			{
				get { return ParseEnum(base.DicomAttributeProvider[DicomTags.GraphicType].GetString(0, string.Empty), GraphicType.None); }
				set
				{
					if (value == GraphicType.None)
						throw new ArgumentOutOfRangeException("value", "GraphicType is Type 1 Required.");
					SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.GraphicType], value);
				}
			}

			/// <summary>
			/// Gets or sets the value of GraphicFilled in the underlying collection. Type 1C.
			/// </summary>
			public GraphicFilled GraphicFilled
			{
				get { return ParseEnum(base.DicomAttributeProvider[DicomTags.GraphicFilled].GetString(0, string.Empty), GraphicFilled.None); }
				set
				{
					if (value == GraphicFilled.None)
					{
						base.DicomAttributeProvider[DicomTags.GraphicFilled] = null;
						return;
					}
					SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.GraphicFilled], value);
				}
			}
		}

		/// <summary>
		/// Enumerated values for the <see cref="DicomTags.BoundingBoxAnnotationUnits"/> attribute defining whether or not the annotation is Image or Displayed Area relative.
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.10.5 (Table C.10-5)</remarks>
		public enum BoundingBoxAnnotationUnits
		{
			Pixel,
			Display,

			/// <summary>
			/// Represents the null value, which is equivalent to the unknown status.
			/// </summary>
			None
		}

		/// <summary>
		/// Enumerated values for the <see cref="DicomTags.AnchorPointAnnotationUnits"/> attribute defining whether or not the annotation is Image or Displayed Area relative.
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.10.5 (Table C.10-5)</remarks>
		public enum AnchorPointAnnotationUnits
		{
			Pixel,
			Display,

			/// <summary>
			/// Represents the null value, which is equivalent to the unknown status.
			/// </summary>
			None
		}

		/// <summary>
		/// Enumerated values for the <see cref="DicomTags.GraphicAnnotationUnits"/> attribute defining whether or not the annotation is Image or Displayed Area relative.
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.10.5 (Table C.10-5)</remarks>
		public enum GraphicAnnotationUnits
		{
			Pixel,
			Display,

			/// <summary>
			/// Represents the null value, which is equivalent to the unknown status.
			/// </summary>
			None
		}

		/// <summary>
		/// Enumerated values for the <see cref="DicomTags.BoundingBoxTextHorizontalJustification"/> attribute describing the location of the text relative to the vertical edges of the bounding box.
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.10.5 (Table C.10-5)</remarks>
		public enum BoundingBoxTextHorizontalJustification
		{
			Left,
			Right,
			Center,

			/// <summary>
			/// Represents the null value, which is equivalent to the unknown status.
			/// </summary>
			None
		}

		/// <summary>
		/// Enumerated values for the <see cref="DicomTags.AnchorPointVisibility"/> attribute.
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.10.5 (Table C.10-5)</remarks>
		public enum AnchorPointVisibility
		{
			Y,
			N,

			/// <summary>
			/// Represents the null value, which is equivalent to the unknown status.
			/// </summary>
			None
		}

		/// <summary>
		/// Enumerated values for the <see cref="DicomTags.GraphicType"/> attribute describing the shape of the graphic that is to be drawn.
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.10.5 (Table C.10-5)</remarks>
		public enum GraphicType
		{
			Point,
			Polyline,
			Interpolated,
			Circle,
			Ellipse,

			/// <summary>
			/// Represents the null value, which is equivalent to the unknown status.
			/// </summary>
			None
		}

		/// <summary>
		/// Enumerated values for the <see cref="DicomTags.GraphicFilled"/> attribute.
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.10.5 (Table C.10-5)</remarks>
		public enum GraphicFilled
		{
			Y,
			N,

			/// <summary>
			/// Represents the null value, which is equivalent to the unknown status.
			/// </summary>
			None
		}
	}
}