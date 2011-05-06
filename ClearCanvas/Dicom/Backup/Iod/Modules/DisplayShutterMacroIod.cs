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
using ClearCanvas.Dicom.Utilities;
using System.Drawing;
using System.Text;

namespace ClearCanvas.Dicom.Iod.Modules
{
	/// <summary>
	/// DisplayShutter Module
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.7.6.11 (Table ?)</remarks>
	public class DisplayShutterModuleIod : DisplayShutterMacroIod
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DisplayShutterModuleIod"/> class.
		/// </summary>	
		public DisplayShutterModuleIod() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="DisplayShutterModuleIod"/> class.
		/// </summary>
		public DisplayShutterModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider) { }
	}

	/// <summary>
	/// DisplayShutter Macro
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.7.6.11 (Table ?)</remarks>
	public class DisplayShutterMacroIod : IodBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DisplayShutterModuleIod"/> class.
		/// </summary>	
		public DisplayShutterMacroIod() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="DisplayShutterModuleIod"/> class.
		/// </summary>
		public DisplayShutterMacroIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider) { }

		/// <summary>
		/// Gets or sets the shutter shape.  Type 1.
		/// </summary>
		public ShutterShape ShutterShape
		{
			get
			{
				ShutterShape returnValue = ShutterShape.None;
				string[] values = base.DicomAttributeProvider[DicomTags.ShutterShape].Values as string[];
				if (values != null && values.Length > 0)
				{
					foreach (string value in values)
					{
						string upperValue = value.ToUpperInvariant();
						if (upperValue == "CIRCULAR")
							returnValue |= Iod.ShutterShape.Circular;
						else if (upperValue == "RECTANGULAR")
							returnValue |= Iod.ShutterShape.Rectangular;
						else if (upperValue == "POLYGONAL")
							returnValue |= Iod.ShutterShape.Polygonal;
						else if (upperValue == "BITMAP")
							returnValue |= Iod.ShutterShape.Bitmap;
					}
				}

				return returnValue;
			}
			set
			{
				if (value == ShutterShape.None)
				{
					base.DicomAttributeProvider[DicomTags.ShutterShape] = null;
				}
				else if ((value & ShutterShape.Bitmap) == ShutterShape.Bitmap)
				{
					throw new ArgumentException("Bitmap is not supported on this module.", "value");
				}
				else
				{
					List<string> shapes = new List<string>(3);
					if ((value & ShutterShape.Circular) == ShutterShape.Circular)
						shapes.Add(ShutterShape.Circular.ToString().ToUpperInvariant());
					if ((value & ShutterShape.Polygonal) == ShutterShape.Polygonal)
						shapes.Add(ShutterShape.Polygonal.ToString().ToUpperInvariant());
					if ((value & ShutterShape.Rectangular) == ShutterShape.Rectangular)
						shapes.Add(ShutterShape.Rectangular.ToString().ToUpperInvariant());
					base.DicomAttributeProvider[DicomTags.ShutterShape].SetStringValue(string.Join("\\", shapes.ToArray()));
				}
			}
		}

		/// <summary>
		/// Gets or sets the left vertical edge of a rectangular shutter.  Type 1C.
		/// </summary>
		public int? ShutterLeftVerticalEdge
		{
			get
			{
				DicomAttribute attribute;
				if (base.DicomAttributeProvider.TryGetAttribute(DicomTags.ShutterLeftVerticalEdge, out attribute))
					return attribute.GetInt32(0, 0);
				else
					return null;
			}
			set
			{
				if (!value.HasValue)
					base.DicomAttributeProvider[DicomTags.ShutterLeftVerticalEdge] = null;
				else
					base.DicomAttributeProvider[DicomTags.ShutterLeftVerticalEdge].SetInt32(0, value.Value);
			}
		}

		/// <summary>
		/// Gets or sets the right vertical edge of a rectangular shutter.  Type 1C.
		/// </summary>
		public int? ShutterRightVerticalEdge
		{
			get
			{
				DicomAttribute attribute;
				if (base.DicomAttributeProvider.TryGetAttribute(DicomTags.ShutterRightVerticalEdge, out attribute))
					return attribute.GetInt32(0, 0);
				else
					return null;
			}
			set
			{
				if (!value.HasValue)
					base.DicomAttributeProvider[DicomTags.ShutterRightVerticalEdge] = null;
				else
					base.DicomAttributeProvider[DicomTags.ShutterRightVerticalEdge].SetInt32(0, value.Value);
			}
		}

		/// <summary>
		/// Gets or sets the upper horizontal edge of a rectangular shutter.  Type 1C.
		/// </summary>
		public int? ShutterUpperHorizontalEdge
		{
			get
			{
				DicomAttribute attribute;
				if (base.DicomAttributeProvider.TryGetAttribute(DicomTags.ShutterUpperHorizontalEdge, out attribute))
					return attribute.GetInt32(0, 0);
				else
					return null;
			}
			set
			{
				if (!value.HasValue)
					base.DicomAttributeProvider[DicomTags.ShutterUpperHorizontalEdge] = null;
				else
					base.DicomAttributeProvider[DicomTags.ShutterUpperHorizontalEdge].SetInt32(0, value.Value);
			}
		}

		/// <summary>
		/// Gets or sets the lower horizontal edge of a rectangular shutter.  Type 1C.
		/// </summary>
		public int? ShutterLowerHorizontalEdge
		{
			get
			{
				DicomAttribute attribute;
				if (base.DicomAttributeProvider.TryGetAttribute(DicomTags.ShutterLowerHorizontalEdge, out attribute))
					return attribute.GetInt32(0, 0);
				else
					return null;
			}
			set
			{
				if (!value.HasValue)
					base.DicomAttributeProvider[DicomTags.ShutterLowerHorizontalEdge] = null;
				else
					base.DicomAttributeProvider[DicomTags.ShutterLowerHorizontalEdge].SetInt32(0, value.Value);
			}
		}

		/// <summary>
		/// Gets or sets the center of a circular shutter.  Type 1C.
		/// </summary>
		public Point? CenterOfCircularShutter
		{
			get
			{
				DicomAttribute attribute;
				if (base.DicomAttributeProvider.TryGetAttribute(DicomTags.CenterOfCircularShutter, out attribute))
				{
					if (attribute.Count >= 2)
						return new Point(attribute.GetInt32(1, 0), attribute.GetInt32(0, 0));
				}

				return null;
			}
			set
			{
				if (!value.HasValue)
				{
					base.DicomAttributeProvider[DicomTags.CenterOfCircularShutter] = null;
				}
				else
				{
					string val = String.Format("{0}\\{1}", value.Value.Y, value.Value.X);
					base.DicomAttributeProvider[DicomTags.CenterOfCircularShutter].SetStringValue(val);
				}
			}
		}

		/// <summary>
		/// Gets or sets the radius of a circular shutter.  Type 1C.
		/// </summary>
		public int? RadiusOfCircularShutter
		{
			get
			{
				DicomAttribute attribute;
				if (base.DicomAttributeProvider.TryGetAttribute(DicomTags.RadiusOfCircularShutter, out attribute))
					return attribute.GetInt32(0, 0);
				else
					return null;
			}
			set
			{
				if (!value.HasValue)
					base.DicomAttributeProvider[DicomTags.RadiusOfCircularShutter] = null;
				else
					base.DicomAttributeProvider[DicomTags.RadiusOfCircularShutter].SetInt32(0, value.Value);
			}
		}

		/// <summary>
		/// Gets or sets the vertices of a polygonal shutter.  Type 1C.
		/// </summary>
		public Point[] VerticesOfThePolygonalShutter
		{
			get
			{
				int[] values;
				DicomAttribute attribute = base.DicomAttributeProvider[DicomTags.VerticesOfThePolygonalShutter];
				if (DicomStringHelper.TryGetIntArray(attribute, out values))
				{
					long count = attribute.Count & 0x7ffffffffffffffe; // rounds down to nearest multiple of 2
					Point[] points = new Point[count / 2];
					
					int j = 0;
					for (int i = 0; i < count; i+= 2)
					{
						int row = attribute.GetInt32(i, 0);
						int column = attribute.GetInt32(i + 1, 0);
						points[j++] = new Point(column, row);
					}

					return points;
				}
				else
				{
					return new Point[0];
				}
			}
			set
			{
				if (value == null)
				{
					base.DicomAttributeProvider[DicomTags.VerticesOfThePolygonalShutter] = null;
				}
				else  if (value.Length == 0)
				{
					base.DicomAttributeProvider[DicomTags.VerticesOfThePolygonalShutter].SetNullValue();
				}
				else
				{
					string[] points = new string[value.Length];
					for (int n = 0; n < points.Length; n++)
						points[n] = string.Format("{0}\\{1}", value[n].Y, value[n].X);
					base.DicomAttributeProvider[DicomTags.VerticesOfThePolygonalShutter].SetStringValue(string.Join("\\", points));
				}
			}
		}

		/// <summary>
		/// Gets or sets the shutter presentation value.  Type 3.
		/// </summary>
		public ushort? ShutterPresentationValue
		{
			get
			{
				DicomAttribute attribute;
				if (base.DicomAttributeProvider.TryGetAttribute(DicomTags.ShutterPresentationValue, out attribute))
					return attribute.GetUInt16(0, 0);
				else
					return null;
			}
			set
			{
				if (!value.HasValue)
					base.DicomAttributeProvider[DicomTags.ShutterPresentationValue] = null;
				else
					base.DicomAttributeProvider[DicomTags.ShutterPresentationValue].SetUInt16(0, value.Value);
			}
		}

		/// <summary>
		/// Gets or sets the shutter presentation color value.  Type 3.
		/// </summary>
		public CIELabColor? ShutterPresentationColorCielabValue
		{
			get
			{
				DicomAttribute attribute = base.DicomAttributeProvider[DicomTags.ShutterPresentationColorCielabValue];
				if (attribute.IsEmpty || attribute.IsNull)
					return null;

				ushort[] values = attribute.Values as ushort[];
				if (values != null && values.Length >= 3)
					return new CIELabColor(values[0], values[1], values[2]);
				else
					return null;
			}
			set
			{
				if (!value.HasValue)
					base.DicomAttributeProvider[DicomTags.ShutterPresentationColorCielabValue] = null;
				else
					base.DicomAttributeProvider[DicomTags.ShutterPresentationColorCielabValue].Values = value.Value.ToArray();
			}
		}

		/// <summary>
		/// Gets an enumeration of <see cref="DicomTag"/>s used by this module.
		/// </summary>
		public static IEnumerable<uint> DefinedTags 
		{
			get 
			{
				yield return DicomTags.ShutterShape;
				yield return DicomTags.ShutterLeftVerticalEdge;
				yield return DicomTags.ShutterRightVerticalEdge;
				yield return DicomTags.ShutterLowerHorizontalEdge;
				yield return DicomTags.ShutterUpperHorizontalEdge;
				yield return DicomTags.CenterOfCircularShutter;
				yield return DicomTags.RadiusOfCircularShutter;
				yield return DicomTags.VerticesOfThePolygonalShutter;
				yield return DicomTags.ShutterPresentationValue;
				yield return DicomTags.ShutterPresentationColorCielabValue;
				yield break;
			}
		}
	}
}
