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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using ClearCanvas.Common;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.IO;

namespace ClearCanvas.Dicom.Iod.Modules
{
	/// <summary>
	/// OverlayPlane Module and MultiFrameOverlay Module
	/// </summary>
	/// <seealso cref="OverlayPlaneModuleIod.this"/>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Sections C.9.2 (Table C.9-2) and C.9.3 (Table C.9-3)</remarks>
	public class OverlayPlaneModuleIod : IodBase, IEnumerable<OverlayPlane>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OverlayPlaneModuleIod"/> class.
		/// </summary>	
		public OverlayPlaneModuleIod() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="OverlayPlaneModuleIod"/> class.
		/// </summary>
		public OverlayPlaneModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider) {}

		/// <summary>
		/// Gets the Overlays in the underlying collection. The index must be between 0 and 15 inclusive.
		/// </summary>
		/// <remarks>
		/// The implementation of the Overlay Plane module involving repeating groups is a holdover
		/// from previous versions of the DICOM Standard. For each of the 16 allowed overlays, there
		/// exists a separate set of tags bearing the same element numbers but with a group number
		/// of the form 60xx, where xx is an even number from 00 to 1E inclusive. In order to make
		/// these IOD classes easier to use, each of these 16 sets of tags are represented as
		/// separate items of a collection, and may be addressed by an index between 0 and 15
		/// inclusive (mapping to the even groups between 6000 and 601E).
		/// </remarks>
		public OverlayPlane this[int index]
		{
			get
			{
				Platform.CheckArgumentRange(index, 0, 15, "index");
				return new OverlayPlane(index, this.DicomAttributeProvider);
			}
		}

		/// <summary>
		/// Removes all the tags associated with a particular group from the underlying data source.
		/// </summary>
		/// <param name="index">The index of the group to remove.</param>
		public void Delete(int index)
		{
			Platform.CheckArgumentRange(index, 0, 15, "index");
			foreach (uint tag in this[index].DefinedTags)
				base.DicomAttributeProvider[tag] = null;
		}

		public bool HasOverlayPlane(int index)
		{
			if (index < 0 || index >= 16)
				return false;
			DicomAttribute attrib;
			if (!base.DicomAttributeProvider.TryGetAttribute(ComputeTagOffset(index) + DicomTags.OverlayBitPosition, out attrib))
				return false;
			else if (attrib != null)
				return !attrib.IsEmpty;
			else 
				return false;
		}

		internal static uint ComputeTagOffset(int index)
		{
			return (uint) index*2*0x10000;
		}

		/// <summary>
		/// Gets an enumeration of <see cref="DicomTag"/>s used by this module.
		/// </summary>
		public static IEnumerable<uint> DefinedTags
		{
			get
			{
				for (int n = 0; n < 16; n++)
				{
					uint tagOffset = ComputeTagOffset(n);
					yield return tagOffset + DicomTags.OverlayBitPosition;
					yield return tagOffset + DicomTags.OverlayBitsAllocated;
					yield return tagOffset + DicomTags.OverlayColumns;
					yield return tagOffset + DicomTags.OverlayData;
					yield return tagOffset + DicomTags.OverlayDescription;
					yield return tagOffset + DicomTags.OverlayLabel;
					yield return tagOffset + DicomTags.OverlayOrigin;
					yield return tagOffset + DicomTags.OverlayRows;
					yield return tagOffset + DicomTags.OverlaySubtype;
					yield return tagOffset + DicomTags.OverlayType;
					yield return tagOffset + DicomTags.RoiArea;
					yield return tagOffset + DicomTags.RoiMean;
					yield return tagOffset + DicomTags.RoiStandardDeviation;
					yield return tagOffset + DicomTags.NumberOfFramesInOverlay;
					yield return tagOffset + DicomTags.ImageFrameOrigin;
				}
			}
		}

		#region IEnumerable<OverlayPlane> Members

		public IEnumerator<OverlayPlane> GetEnumerator()
		{
			for (int n = 0; n < 16; n++)
			{
				if (this.HasOverlayPlane(n))
					yield return this[n];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}

	/// <summary>
	/// Enumerated values for the <see cref="DicomTags.OverlayType"/> attribute indicating whether this overlay represents a region of interest or other graphics.
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.9.2 (Table C.9-2)</remarks>
	public enum OverlayType
	{
		/// <summary>
		/// Graphics
		/// </summary>
		G,

		/// <summary>
		/// ROI
		/// </summary>
		R,

		/// <summary>
		/// Represents the null value, which is equivalent to the unknown status.
		/// </summary>
		None
	}

	/// <summary>
	/// Defined terms for the <see cref="DicomTags.OverlaySubtype"/> attribute identifying the intended purpose of the <see cref="OverlayType"/>.
	/// </summary>
	/// <remarks>
	/// <para>As defined in the DICOM Standard 2008, Part 3, Section C.9.2.1.3</para>
	/// <para>
	/// Additional or alternative Defined Terms may be specified in modality specific Modules,
	/// such as in the Ultrasound Image Module, C.8.5.6.1.11.
	/// </para>
	/// </remarks>
	public class OverlaySubtype
	{
		/// <summary>
		/// User created graphic annotation (e.g. operator).
		/// </summary>
		public static readonly OverlaySubtype User = new OverlaySubtype("USER");

		/// <summary>
		/// Machine or algorithm generated graphic annotation, such as output of a Computer Assisted Diagnosis algorithm.
		/// </summary>
		public static readonly OverlaySubtype Automated = new OverlaySubtype("AUTOMATED");

		/// <summary>
		/// Gets the <see cref="OverlaySubtype"/> matching the given defined term.
		/// </summary>
		/// <param name="definedTerm">The defined term.</param>
		/// <returns>The defined term.</returns>
		public static OverlaySubtype FromDefinedTerm(string definedTerm)
		{
			foreach (OverlaySubtype term in DefinedTerms)
			{
				if (term.DefinedTerm == definedTerm)
					return term;
			}
			return null;
		}

		/// <summary>
		/// Enumerates the defined terms.
		/// </summary>
		public static IEnumerable<OverlaySubtype> DefinedTerms
		{
			get
			{
				yield return User;
				yield return Automated;
			}
		}

		/// <summary>
		/// Gets the defined term this object represents.
		/// </summary>
		public readonly string DefinedTerm;

		/// <summary>
		/// Constructs a new object with the given defined term.
		/// </summary>
		/// <param name="definedTerm">The defined term.</param>
		protected OverlaySubtype(string definedTerm)
		{
			if (string.IsNullOrEmpty(definedTerm))
				throw new ArgumentNullException("definedTerm");
			this.DefinedTerm = definedTerm;
		}

		public sealed override int GetHashCode()
		{
			return this.DefinedTerm.GetHashCode() ^ -0x2D417CC3;
		}

		public override sealed bool Equals(object obj)
		{
			return (obj is OverlaySubtype && ((OverlaySubtype) obj).DefinedTerm.Equals(this.DefinedTerm));
		}

		public override sealed string ToString()
		{
			return this.DefinedTerm;
		}
	}

	/// <summary>
	/// Overlay Plane Group
	/// </summary>
	/// <seealso cref="OverlayPlaneModuleIod.this"/>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Sections C.9.2 (Table C.9-2) and C.9.3 (Table C.9-3)</remarks>
	public class OverlayPlane : IodBase
	{
		private readonly int _index;
		private readonly uint _tagOffset;

		/// <summary>
		/// Initializes a new instance of the <see cref="OverlayPlane"/> class.
		/// </summary>
		/// <param name="index">The zero-based index of this overlay.</param>
		/// <param name="dicomAttributeProvider">The underlying collection.</param>
		internal OverlayPlane(int index, IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider)
		{
			_index = index;
			_tagOffset = OverlayPlaneModuleIod.ComputeTagOffset(_index);
		}

		/// <summary>
		/// Gets the zero-based index of the overlay to which this group refers (0-15).
		/// </summary>
		public int Index
		{
			get { return _index; }	
		}

		/// <summary>
		/// Gets the DICOM tag group number.
		/// </summary>
		public ushort Group
		{
			get { return (ushort) (_index*2 + 0x6000); }
		}

		/// <summary>
		/// Gets the DICOM tag value offset from the defined base tags (such as <see cref="DicomTags.OverlayActivationLayer"/>).
		/// </summary>
		public uint TagOffset
		{
			get { return _tagOffset; }
		}

		/// <summary>
		/// Gets a value indicating whether or not the <see cref="OverlayData"/> is stored in 16-bit big-endian words.
		/// </summary>
		public bool IsBigEndianOW
		{
			get { return ByteBuffer.LocalMachineEndian == Endian.Big && base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayData] is DicomAttributeOW; }
		}

		/// <summary>
		/// Gets a value indicating if the overlay data for this plane is embedded in the unused bits of the <see cref="DicomTags.PixelData"/>.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This determination algorithm checks for non-existence of <see cref="DicomTags.OverlayData"/>,
		/// existence of <see cref="DicomTags.PixelData"/>, and that <see cref="DicomTags.OverlayBitPosition"/>
		/// is valid given <see cref="DicomTags.BitsAllocated"/>, <see cref="DicomTags.BitsStored"/>, and
		/// <see cref="DicomTags.HighBit"/>.
		/// </para>
		/// <para>
		/// If <b>any</b> of these tags (which, it should be noted, are not part of the Overlay Plane Module)
		/// may not be in the same dataset, then it is highly recommended that a custom determination be made
		/// instead of using this property.
		/// </para>
		/// </remarks>
		public bool IsEmbedded
		{
			get
			{
				// OverlayData exists => not embedded
				DicomAttribute overlayData = base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayData];
				if (overlayData.IsEmpty || overlayData.IsNull)
				{
					// embedded => PixelData exists, not empty
					DicomAttribute pixelData = base.DicomAttributeProvider[DicomTags.PixelData];
					if (!pixelData.IsEmpty && !pixelData.IsNull)
					{
						// embedded => BitsAllocated={8|16}, OverlayBitPosition in [0, BitsAllocated)
						int overlayBitPosition = base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayBitPosition].GetInt32(0, -1);
						int bitsAllocated = base.DicomAttributeProvider[DicomTags.BitsAllocated].GetInt32(0, 0);
						if (overlayBitPosition >= 0 && overlayBitPosition < bitsAllocated && (bitsAllocated == 8 || bitsAllocated == 16))
						{
							// embedded => OverlayBitPosition in (HighBit, BitsAllocated) or [0, HighBit - BitsStored + 1)
							int highBit = base.DicomAttributeProvider[DicomTags.HighBit].GetInt32(0, 0);
							int bitsStored = base.DicomAttributeProvider[DicomTags.BitsStored].GetInt32(0, 0);
							return (overlayBitPosition > highBit || overlayBitPosition < highBit - bitsStored + 1);
						}
					}
				}
				return false;
			}
		}

		/// <summary>
		/// Gets a value indicating if the <see cref="DicomTags.OverlayData"/> exists for this plane.
		/// </summary>
		public bool HasOverlayData
		{
			get
			{
				DicomAttribute attribute = base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayData];
				return !attribute.IsEmpty && !attribute.IsNull;
			}
		}

		/// <summary>
		/// Gets or sets the value of OverlayRows in the underlying collection. Type 1.
		/// </summary>
		public int OverlayRows
		{
			get { return base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayRows].GetInt32(0, 0); }
			set { base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayRows].SetInt32(0, value); }
		}

		/// <summary>
		/// Gets or sets the value of OverlayColumns in the underlying collection. Type 1.
		/// </summary>
		public int OverlayColumns
		{
			get { return base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayColumns].GetInt32(0, 0); }
			set { base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayColumns].SetInt32(0, value); }
		}

		/// <summary>
		/// Gets or sets the value of OverlayType in the underlying collection. Type 1.
		/// </summary>
		public OverlayType OverlayType
		{
			get { return ParseEnum(base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayType].GetString(0, string.Empty), OverlayType.None); }
			set
			{
				if (value == OverlayType.None)
					throw new ArgumentOutOfRangeException("value", "OverlayType is Type 1 Required.");
				SetAttributeFromEnum(base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayType], value, true);
			}
		}

		/// <summary>
		/// Gets or sets the value of OverlayOrigin in the underlying collection. Type 1.
		/// </summary>
		public Point? OverlayOrigin
		{
			get
			{
				DicomAttribute attribute = base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayOrigin];
				int[] result = new int[2];
				if (attribute.TryGetInt32(0, out result[0]))
					if (attribute.TryGetInt32(1, out result[1]))
						return new Point(result[0], result[1]);
				return null;
			}
			set
			{
				if (!value.HasValue)
					throw new ArgumentNullException("value", "OverlayOrigin is Type 1 Required.");
				DicomAttribute attribute = base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayOrigin];
				attribute.SetInt32(0, value.Value.X);
				attribute.SetInt32(1, value.Value.Y);
			}
		}

		/// <summary>
		/// Gets or sets the value of OverlayBitsAllocated in the underlying collection. Type 1.
		/// </summary>
		public int OverlayBitsAllocated
		{
			get { return base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayBitsAllocated].GetInt32(0, 0); }
			set
			{
				if (value != 1)
					throw new ArgumentOutOfRangeException("value", "OverlayBitsAllocated must be 1. Encoding overlay data in the unused bits of PixelData is not supported.");
				base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayBitsAllocated].SetInt32(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of OverlayBitPosition in the underlying collection. Type 1.
		/// </summary>
		public int OverlayBitPosition
		{
			get { return base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayBitPosition].GetInt32(0, 0); }
			set {
				if (value != 0)
					throw new ArgumentOutOfRangeException("value", "OverlayBitPosition must be 0. Encoding overlay data in the unused bits of PixelData is not supported.");
				base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayBitPosition].SetInt32(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of OverlayData in the underlying collection. Type 1.
		/// </summary>
		public byte[] OverlayData
		{
			get { return (byte[]) base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayData].Values; }
			set
			{
				if (value == null || value.Length == 0)
					throw new ArgumentNullException("value", "OverlayData is Type 1 Required.");
				base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayData].Values = value;
			}
		}

		/// <summary>
		/// Gets or sets the value of OverlayDescription in the underlying collection. Type 3.
		/// </summary>
		public string OverlayDescription
		{
			get { return base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayDescription].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayDescription] = null;
					return;
				}
				base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayDescription].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of OverlaySubtype in the underlying collection. Type 3.
		/// </summary>
		public OverlaySubtype OverlaySubtype
		{
			get { return Modules.OverlaySubtype.FromDefinedTerm(base.DicomAttributeProvider[_tagOffset + DicomTags.OverlaySubtype].GetString(0, string.Empty)); }
			set
			{
				if (value == null)
				{
					base.DicomAttributeProvider[_tagOffset + DicomTags.OverlaySubtype] = null;
					return;
				}
				base.DicomAttributeProvider[_tagOffset + DicomTags.OverlaySubtype].SetString(0, value.DefinedTerm);
			}
		}

		/// <summary>
		/// Gets or sets the value of OverlayLabel in the underlying collection. Type 3.
		/// </summary>
		public string OverlayLabel
		{
			get { return base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayLabel].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayLabel] = null;
					return;
				}
				base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayLabel].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of RoiArea in the underlying collection. Type 3.
		/// </summary>
		public int? RoiArea
		{
			get
			{
				int result;
				if (base.DicomAttributeProvider[_tagOffset + DicomTags.RoiArea].TryGetInt32(0, out result))
					return result;
				return null;
			}
			set
			{
				if (!value.HasValue)
				{
					base.DicomAttributeProvider[_tagOffset + DicomTags.RoiArea] = null;
					return;
				}
				base.DicomAttributeProvider[_tagOffset + DicomTags.RoiArea].SetInt32(0, value.Value);
			}
		}

		/// <summary>
		/// Gets or sets the value of RoiMean in the underlying collection. Type 3.
		/// </summary>
		public double? RoiMean
		{
			get
			{
				double result;
				if (base.DicomAttributeProvider[_tagOffset + DicomTags.RoiMean].TryGetFloat64(0, out result))
					return result;
				return null;
			}
			set
			{
				if (!value.HasValue)
				{
					base.DicomAttributeProvider[_tagOffset + DicomTags.RoiMean] = null;
					return;
				}
				base.DicomAttributeProvider[_tagOffset + DicomTags.RoiMean].SetFloat64(0, value.Value);
			}
		}

		/// <summary>
		/// Gets or sets the value of RoiStandardDeviation in the underlying collection. Type 3.
		/// </summary>
		public double? RoiStandardDeviation
		{
			get
			{
				double result;
				if (base.DicomAttributeProvider[_tagOffset + DicomTags.RoiStandardDeviation].TryGetFloat64(0, out result))
					return result;
				return null;
			}
			set
			{
				if (!value.HasValue)
				{
					base.DicomAttributeProvider[_tagOffset + DicomTags.RoiStandardDeviation] = null;
					return;
				}
				base.DicomAttributeProvider[_tagOffset + DicomTags.RoiStandardDeviation].SetFloat64(0, value.Value);
			}
		}

		#region Support for Multi-Frame Overlays

		/// <summary>
		/// Gets or sets the value of NumberOfFramesInOverlay in the underlying collection. Type 1C - Required if the overlay has multiple frames.
		/// </summary>
		public int? NumberOfFramesInOverlay
		{
			get
			{
				int result;
				if (base.DicomAttributeProvider[_tagOffset + DicomTags.NumberOfFramesInOverlay].TryGetInt32(0, out result))
					return result;
				return null;
			}
			set
			{
				if (!value.HasValue)
				{
					base.DicomAttributeProvider[_tagOffset + DicomTags.NumberOfFramesInOverlay] = null;
					return;
				}
				base.DicomAttributeProvider[_tagOffset + DicomTags.NumberOfFramesInOverlay].SetInt32(0, value.Value);
			}
		}

		/// <summary>
		/// Gets or sets the value of ImageFrameOrigin in the underlying collection. Type 3C - Optional if the overlay has multiple frames.
		/// </summary>
		public int? ImageFrameOrigin
		{
			get
			{
				int result;
				if (base.DicomAttributeProvider[_tagOffset + DicomTags.ImageFrameOrigin].TryGetInt32(0, out result))
					return result;
				return null;
			}
			set
			{
				if (!value.HasValue)
				{
					base.DicomAttributeProvider[_tagOffset + DicomTags.ImageFrameOrigin] = null;
					return;
				}
				base.DicomAttributeProvider[_tagOffset + DicomTags.ImageFrameOrigin].SetInt32(0, value.Value);
			}
		}

		/// <summary>
		/// Gets a value indicating if this overlay has multiple frames.
		/// </summary>
		public bool IsMultiFrame
		{
			get { return this.NumberOfFramesInOverlay.HasValue; }
		}

		/// <summary>
		/// Enumerates the overlay frame indices that are applicable to a given frame of an image.
		/// </summary>
		/// <param name="imageFrameNumber">The zero-based index of the image frame.</param>
		/// <param name="countImageFrames">The total number of frames in the image.</param>
		/// <returns>An enumeration of zero-based indices of the overlay frame(s) that are applicable to the frame.</returns>
		public IEnumerable<int> GetRelevantOverlayFrames(int imageFrameNumber, int countImageFrames)
		{
			// if the overlay is not multi-frame, then this overlay is applicable to all frames
			if (!this.IsMultiFrame)
			{
				yield return 0;
				yield break;
			}

			int origin = this.ImageFrameOrigin ?? 0;
			int count = this.NumberOfFramesInOverlay ?? 1;

			// if image is not multi-frame, then all overlay frames are applicable to the image separately
			if (countImageFrames <= 1)
			{
				for (int n = 0; n < count; n++)
					yield return n;
				yield break;
			}

			// otherwise the origin and count specify the range of image frames for which a 1-to-1 relation exists to the overlay frames
			if (imageFrameNumber < origin || imageFrameNumber >= origin + count)
				yield break;

			yield return imageFrameNumber - origin;
		}

		/// <summary>
		/// Computes the bit offset in the <see cref="OverlayData"/> from which to read the overlay data for a specific frame.
		/// </summary>
		/// <param name="overlayFrameNumber">The zero-based frame number for which to compute the bit offset in the <see cref="OverlayData"/>.</param>
		/// <returns>The offset from the beginning of the <see cref="OverlayData"/> in bits.</returns>
		/// <exception cref="NotSupportedException">Thrown if this overlay plane does is not multi-frame.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if no overlay frame exists at the index.</exception>
		public int ComputeOverlayDataBitOffset(int overlayFrameNumber)
		{
			if (!this.IsMultiFrame)
				throw new NotSupportedException("This is not a multi-frame overlay.");

			Platform.CheckNonNegative(overlayFrameNumber, "overlayFrameNumber");

			int result;
			if (!this.TryComputeOverlayDataBitOffset(overlayFrameNumber, out result))
				throw new ArgumentOutOfRangeException("overlayFrameNumber", string.Format("No overlay frame exists at the index {0}.", overlayFrameNumber));

			return result;
		}

		/// <summary>
		/// Computes the bit offset in the <see cref="OverlayData"/> from which to read the overlay data for a specific frame.
		/// </summary>
		/// <param name="overlayFrameNumber">The zero-based frame number for which to compute the bit offset in the <see cref="OverlayData"/>.</param>
		/// <param name="bitOffset">The offset from the beginning of the <see cref="OverlayData"/> in bits.</param>
		/// <returns>True if a valid bit offset was computed; False otherwise.</returns>
		public bool TryComputeOverlayDataBitOffset(int overlayFrameNumber, out int bitOffset)
		{
			bitOffset = 0;

			if (!this.IsMultiFrame)
				return true;
			if (overlayFrameNumber <= 0)
				return false;

			int origin = this.ImageFrameOrigin ?? 0;
			int count = this.NumberOfFramesInOverlay ?? 1;
			if (overlayFrameNumber < origin || overlayFrameNumber >= origin + count)
				return false;

			bitOffset = this.OverlayRows*this.OverlayColumns*(overlayFrameNumber - origin);
			return true;
		}

		/// <summary>
		/// Computes the length of each overlay frame in bits.
		/// </summary>
		/// <returns>The length of each overlay frame in bits.</returns>
		public int GetOverlayFrameLength()
		{
			return this.OverlayRows*this.OverlayColumns;
		}

		#endregion

		/// <summary>
		/// Gets an enumeration of <see cref="DicomTag"/>s used by this group.
		/// </summary>
		public IEnumerable<uint> DefinedTags
		{
			get
			{
				yield return _tagOffset + DicomTags.OverlayBitPosition;
				yield return _tagOffset + DicomTags.OverlayBitsAllocated;
				yield return _tagOffset + DicomTags.OverlayColumns;
				yield return _tagOffset + DicomTags.OverlayData;
				yield return _tagOffset + DicomTags.OverlayDescription;
				yield return _tagOffset + DicomTags.OverlayLabel;
				yield return _tagOffset + DicomTags.OverlayOrigin;
				yield return _tagOffset + DicomTags.OverlayRows;
				yield return _tagOffset + DicomTags.OverlaySubtype;
				yield return _tagOffset + DicomTags.OverlayType;
				yield return _tagOffset + DicomTags.RoiArea;
				yield return _tagOffset + DicomTags.RoiMean;
				yield return _tagOffset + DicomTags.RoiStandardDeviation;
				yield return _tagOffset + DicomTags.NumberOfFramesInOverlay;
				yield return _tagOffset + DicomTags.ImageFrameOrigin;
			}
		}

		/// <summary>
		/// Fills the <see cref="OverlayData"/> property with the overlay that had been encoded
		/// in the <see cref="DicomTags.PixelData"/> of the SOP Instance. 
		/// </summary>
		/// <param name="pd">The pixel data that contains the encoded overlay.</param>
		/// <exception cref="DicomException">Thrown if <paramref name="pd"/> is not a valid source of embedded overlay data.</exception>
		/// <returns>True if the <see cref="OverlayData"/> was populated with data encoded in the pixel data; False if <see cref="OverlayData"/> is not empty.</returns>
		public unsafe bool ConvertEmbeddedOverlay(DicomUncompressedPixelData pd)
		{
			byte[] oldOverlayData = this.OverlayData;
			if (oldOverlayData != null && oldOverlayData.Length > 0)
				return false;

			// General sanity checks
			if (pd.SamplesPerPixel > 1)
				throw new DicomException("Unable to convert embedded overlays when Samples Per Pixel > 1");
			if (pd.BitsStored == 8 && pd.BitsAllocated == 8)
				throw new DicomException("Unable to remove overlay with 8 Bits Stored and 8 Bits Allocated");
			if (pd.BitsStored == 16 && pd.BitsAllocated == 16)
				throw new DicomException("Unable to remove overlay with 16 Bits Stored and 16 Bits Allocated");

			int frameSize = pd.UncompressedFrameSize;
			int overlaySize = frameSize/pd.BitsAllocated;
			if (frameSize%pd.BitsAllocated > 0)
				overlaySize++;

			int numValues = frameSize/pd.BytesAllocated;

			byte[] overlay = new byte[overlaySize];
			int overlayOffset = 0;
			// Embededded overlays must exist for all frames, they can't be for a subset
			for (int i = 0; i < pd.NumberOfFrames; i++)
			{
				byte[] frameData = pd.GetFrame(i);

				if (pd.BitsAllocated <= 8)
				{
					byte pixelMask = ((byte)(0x1 << this.OverlayBitPosition ));
					byte overlayMask = 0x01;

					fixed (byte* pFrameData = frameData)
					{
						byte* pixelData = pFrameData;
						for (int p = 0; p < numValues; p++, pixelData++)
						{
							if ((*pixelData & pixelMask) != 0)
							{
								overlay[overlayOffset] |= overlayMask;
								*pixelData &= (byte)~pixelMask;
							}

							if (overlayMask == 0x80)
							{
								overlayMask = 0x01;
								overlayOffset++;
							}
							else
								overlayMask <<= 1;
						}
					}
				}
				else
				{
					fixed (byte* pFrameData = frameData)
					{
						ushort pixelMask = ((ushort)(0x1 << OverlayBitPosition));
						byte overlayMask = 0x01;

						ushort* pixelData = (ushort*) pFrameData;
						for (int p = 0; p < numValues; p++, pixelData++)
						{
							if ((*pixelData & pixelMask) != 0)
							{
								overlay[overlayOffset] |= overlayMask;
								*pixelData &= (ushort)~pixelMask;
							}

							if (overlayMask == 0x80)
							{
								overlayMask = 0x01;
								overlayOffset++;
							}
							else
								overlayMask <<= 1;
						}
					}
				}
			}

			// Assign the new overlay tags
			this.OverlayBitPosition = 0;
			this.OverlayBitsAllocated = 1;
			if (this.IsBigEndianOW)
			{
				// Just do a bulk swap, performance isn't much of an issue.
				ByteBuffer buffer = new ByteBuffer(overlay, Endian.Little);
				buffer.Swap2();
				this.OverlayData = buffer.ToBytes();
			}
			else
				this.OverlayData = overlay;

			// Cleanup Rows/Columns if necessary
			if (this.OverlayColumns == 0)
				this.OverlayColumns = pd.ImageWidth;
			if (this.OverlayRows == 0)
				this.OverlayRows = pd.ImageHeight;

			return true;
		}
	}
}