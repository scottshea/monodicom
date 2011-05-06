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

namespace ClearCanvas.Dicom.Iod
{
	public class PaletteColorLut
	{
		private readonly int _firstMappedPixelValue;
		private readonly Color[] _data;

		public PaletteColorLut(int size,
			int firstMappedPixel,
			int bitsPerLutEntry,
			byte[] redLut,
			byte[] greenLut,
			byte[] blueLut)

		{
			Platform.CheckForNullReference(redLut, "redLut");
			Platform.CheckForNullReference(greenLut, "greenLut");
			Platform.CheckForNullReference(blueLut, "blueLut");
			Platform.CheckTrue(redLut.Length == greenLut.Length, "redLut.Length == greenLut.Length");
			Platform.CheckTrue(redLut.Length == blueLut.Length, "redLut.Length == blueLut.Length");
			Platform.CheckTrue(redLut.Length == size || (redLut.Length == 2 * size && bitsPerLutEntry > 8), "Valid Lut Size");

			_firstMappedPixelValue = firstMappedPixel;
			_data = Create(size, bitsPerLutEntry, redLut, greenLut, blueLut);
		}

		public int FirstMappedPixelValue
		{
			get { return _firstMappedPixelValue; }
		}
		
		public Color[] Data
		{
			get { return _data; }	
		}

		public Color this[int index]
		{
			get
			{
				if (index < _firstMappedPixelValue)
					return _data[0];

				if (index > _firstMappedPixelValue + _data.Length - 1)
					return _data[_data.Length - 1];

				return _data[index - _firstMappedPixelValue];
			}
		}

		private Color[] Create(int size, int bitsPerLutEntry, byte[] redLut, byte[] greenLut, byte[] blueLut)
		{
			Color[] lut = new Color[size];

			if (bitsPerLutEntry == 8)
			{
				// Account for case where an 8-bit entry is encoded in a 16 bits allocated
				// i.e., 8 bits of padding per entry
				if (redLut.Length == 2 * size)
				{
					int offset = 0;
					for (int i = 0; i < size; i++)
					{
						// Get the low byte of the 16-bit entry
						lut [i] = Color.FromArgb(255, redLut[offset], greenLut[offset], blueLut[offset]);
					}
				}
				else
				{
					// The regular 8-bit case
					int offset = 0;
					for (int i = 0; i < size; i++)
					{
						lut[i] = Color.FromArgb(255, redLut[offset], greenLut[offset], blueLut[offset]);
						++offset;
					}
				}
			}
			// 16 bit entries
			else
			{
				int offset = 1;
				for (int i = 0; i < size; i++)
				{
					// Just get the high byte, since we'd have to right shift the
					// 16-bit value by 8 bits to scale it to an 8 bit value anyway.
					lut[i] = Color.FromArgb(255, redLut[offset], greenLut[offset], blueLut[offset]);
					offset += 2;
				}
			}

			return lut;
		}

		public static PaletteColorLut Create(IDicomAttributeProvider dataSource)
		{
			int lutSize, firstMappedPixel, bitsPerLutEntry;

			DicomAttribute attribDescriptor = dataSource[DicomTags.RedPaletteColorLookupTableDescriptor];

			bool tagExists = attribDescriptor.TryGetInt32(0, out lutSize);
			if (!tagExists)
				throw new Exception("LUT Size missing.");

			tagExists = attribDescriptor.TryGetInt32(1, out firstMappedPixel);

			if (!tagExists)
				throw new Exception("First Mapped Pixel missing.");

			tagExists = attribDescriptor.TryGetInt32(2, out bitsPerLutEntry);

			if (!tagExists)
				throw new Exception("Bits Per Entry missing.");

			byte[] redLut = dataSource[DicomTags.RedPaletteColorLookupTableData].Values as byte[];
			if (redLut == null)
				throw new Exception("Red Palette Color LUT missing.");

			byte[] greenLut = dataSource[DicomTags.GreenPaletteColorLookupTableData].Values as byte[];
			if (greenLut == null)
				throw new Exception("Green Palette Color LUT missing.");

			byte[] blueLut = dataSource[DicomTags.BluePaletteColorLookupTableData].Values as byte[];
			if (blueLut == null)
				throw new Exception("Blue Palette Color LUT missing.");

			// The DICOM standard says that if the LUT size is 0, it means that it's 65536 in size.
			if (lutSize == 0)
				lutSize = 65536;

			return new PaletteColorLut(lutSize,
				firstMappedPixel,
				bitsPerLutEntry,
				redLut,
				greenLut,
				blueLut);

		}

	}
}
