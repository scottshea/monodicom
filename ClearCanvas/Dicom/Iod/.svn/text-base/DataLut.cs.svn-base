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
using System.Runtime.InteropServices;

namespace ClearCanvas.Dicom.Iod
{
	public class DataLut
	{
		#region Private Fields

		private readonly int _firstMappedPixelValue;
		private int _bitsPerEntry;
		private readonly int[] _data;
		private readonly string _explanation;

		private int _minOutputValue;
		private int _maxOutputValue;

		#endregion

		#region Constructors

		public DataLut(int firstMappedPixelValue, int bitsPerEntry, int[] data)
			: this(firstMappedPixelValue, bitsPerEntry, data, null)
		{
		}

		public DataLut(int firstMappedPixelValue, int bitsPerEntry, int[] data, string explanation)
			: this(firstMappedPixelValue, bitsPerEntry, data, explanation, 0, (int)Math.Pow(2, bitsPerEntry) - 1)
		{
		}

		public DataLut(int firstMappedPixelValue, int bitsPerEntry, int[] data, string explanation, int minOutputValue, int maxOutputValue)
		{
			_firstMappedPixelValue = firstMappedPixelValue;
			_bitsPerEntry = bitsPerEntry;
			_data = data;
			_explanation = explanation ?? "";
			_minOutputValue = minOutputValue;
			_maxOutputValue = maxOutputValue;
		}

		public DataLut(DataLut item)
			: this(	item.FirstMappedPixelValue, item.BitsPerEntry, (int[])item.Data.Clone(),
					item.Explanation, item.MinOutputValue, item.MaxOutputValue)
		{
		}

		#endregion

		#region Public Properties/Indexers

		public int this[int pixelValue]
		{
			get
			{
				if (pixelValue < FirstMappedPixelValue)
					return _data[0];
				else if (pixelValue > LastMappedPixelValue)
					return _data[_data.Length - 1];
				else
					return _data[pixelValue - FirstMappedPixelValue];
			}
		}

		public int NumberOfEntries
		{
			get { return _data.Length; }
		}

		public int BitsPerEntry
		{
			get { return _bitsPerEntry; }
			protected set { _bitsPerEntry = value; }
		}

		public int[] Data
		{
			get { return _data; }
		}

		public string Explanation
		{
			get { return _explanation; }
		}

		public int FirstMappedPixelValue
		{
			get { return _firstMappedPixelValue; }
		}

		public int LastMappedPixelValue
		{
			get { return _firstMappedPixelValue + NumberOfEntries - 1; }
		}

		public int MinOutputValue
		{
			get { return _minOutputValue; }
			protected set { _minOutputValue = value; }
		}

		public int MaxOutputValue
		{
			get { return _maxOutputValue; }
			protected set { _maxOutputValue = value; }
		}

		#endregion

		#region Public Methods

		//TODO: implement this.
		//public virtual DicomSequenceItem ToSequenceItem()
		//{
		//}

		#endregion

		#region Static Methods

		#region Private

		private unsafe static int[] ExtractData<T>(T[] original, int numberOfEntries, int bitsPerEntry, bool isLutDataSigned) where T : struct
		{
			int lengthInBytes = original.Length * Marshal.SizeOf(typeof(T));
			//some implementations store 8-bit entries in 16-bit format, padding the extra bits.
			if (bitsPerEntry <= 8 && lengthInBytes >= 2 * numberOfEntries)
				bitsPerEntry = 16;

			if (bitsPerEntry <= 8)
			{
				if (lengthInBytes < numberOfEntries)
					throw new DicomDataException("The length of the data buffer does not match the number of entries.");

				GCHandle handle = GCHandle.Alloc(original, GCHandleType.Pinned);
				try
				{
					return ExtractData((byte*)handle.AddrOfPinnedObject(), numberOfEntries, bitsPerEntry, isLutDataSigned);
				}
				finally
				{
					handle.Free();
				}
			}
			else if (bitsPerEntry <= 16)
			{
				if (lengthInBytes/2 < numberOfEntries)
					throw new DicomDataException("The length of the data buffer does not match the number of entries.");

				GCHandle handle = GCHandle.Alloc(original, GCHandleType.Pinned);
				try
				{
					return ExtractData((ushort*)handle.AddrOfPinnedObject(), numberOfEntries, bitsPerEntry, isLutDataSigned);
				}
				finally
				{
					handle.Free();
				}
			}
			else
			{
				throw new DicomDataException(String.Format("Invalid bits per entry: {0}", bitsPerEntry));
			}
		}

		private unsafe static int[] ExtractData(ushort* original, int numberOfEntries, int bitsPerEntry, bool isLutDataSigned)
		{
			ushort* pOriginal = original;
			
			int[] data = new int[numberOfEntries];
			fixed (int* target = data)
			{
				int* pTarget = target;
				int shift = 16 - bitsPerEntry;
				if (isLutDataSigned && shift > 0)
				{
					for (int i = 0; i < numberOfEntries; ++i)
					{
						*pTarget = ((short)(*pOriginal << shift)) >> shift;
						++pOriginal;
						++pTarget;
					}
				}
				else if (isLutDataSigned)
				{
					for (int i = 0; i < numberOfEntries; ++i)
					{
						*pTarget = (short)*pOriginal;
						++pOriginal;
						++pTarget;
					}
				}
				else
				{
					for (int i = 0; i < numberOfEntries; ++i)
					{
						*pTarget = (ushort)*pOriginal;
						++pOriginal;
						++pTarget;
					}
				}
			}

			return data;
		}

		private unsafe static int[] ExtractData(byte* original, int numberOfEntries, int bitsPerEntry, bool isLutDataSigned)
		{
			byte* pOriginal = original;

			int[] data = new int[numberOfEntries];
			fixed (int* target = data)
			{
				int* pTarget = target;
				int shift = 8 - bitsPerEntry;
				if (isLutDataSigned && shift > 0)
				{
					for (int i = 0; i < numberOfEntries; ++i)
					{
						*pTarget = ((sbyte)(*pOriginal << shift)) >> shift;
						++pOriginal;
						++pTarget;
					}
				}
				else if (isLutDataSigned)
				{
					for (int i = 0; i < numberOfEntries; ++i)
					{
						*pTarget = (sbyte)*pOriginal;
						++pOriginal;
						++pTarget;
					}
				}
				else
				{
					for (int i = 0; i < numberOfEntries; ++i)
					{
						*pTarget = (byte)*pOriginal;
						++pOriginal;
						++pTarget;
					}
				}
			}

			return data;
		}

		#endregion

		#region Protected Helper

		protected static int GetBitsStored(IDicomAttributeProvider attributeProvider)
		{
			DicomAttribute bitsStoredAttribute = attributeProvider[DicomTags.BitsStored];
			if (!IsValidAttribute(bitsStoredAttribute))
				throw new DicomDataException("Bits Stored must exist and have a valid value.");

			return bitsStoredAttribute.GetInt32(0, 0);
		}

		protected static int GetPixelRepresentation(IDicomAttributeProvider dicomAttributeProvider)
		{
			DicomAttribute pixelRepresentationAttribute = dicomAttributeProvider[DicomTags.PixelRepresentation];
			if (pixelRepresentationAttribute == null)
				return 0;
			else
				return pixelRepresentationAttribute.GetInt32(0, 0);
		}

		protected static bool IsValidAttribute(DicomAttribute attribute)
		{
			return attribute != null && !attribute.IsEmpty && !attribute.IsNull;
		}
		
		#endregion

		#region Public Factory

		public static DataLut Create(DicomSequenceItem item, bool isFirstMappedPixelSigned, bool isLutDataSigned)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			DicomAttribute descriptor = item[DicomTags.LutDescriptor];
			if (descriptor.Count < 3)
				throw new DicomDataException("Invalid Lut Descriptor.");

			ushort numEntries;
			if (!descriptor.TryGetUInt16(0, out numEntries))
				throw new DicomDataException("Failed to get number of lut entries from Lut Descriptor.");

			int numberOfEntries = numEntries;
			if (numberOfEntries == 0)
				numberOfEntries = 65536; //2^16

			int firstMappedPixelValue;
			if (isFirstMappedPixelSigned)
			{
				short firstMappedValue;
				if (!descriptor.TryGetInt16(1, out firstMappedValue))
					throw new DicomDataException("Failed to get first mapped pixel value from Lut Descriptor.");

				firstMappedPixelValue = firstMappedValue;
			}
			else
			{
				ushort firstMappedValue;
				if (!descriptor.TryGetUInt16(1, out firstMappedValue))
					throw new DicomDataException("Failed to get first mapped pixel value from Lut Descriptor.");

				firstMappedPixelValue = firstMappedValue;
			}

			ushort bitsPerEntry;
			if (!descriptor.TryGetUInt16(2, out bitsPerEntry))
				throw new DicomDataException("Failed to get bits per entry from Lut Descriptor.");

			int[] data;
			DicomAttribute lutData = item[DicomTags.LutData];
			if (!IsValidAttribute(lutData))
				throw new DicomDataException("Lut Data attribute must exist and have a valid value.");

			if (lutData is DicomAttributeSS)
				data = ExtractData(lutData.Values as short[], numberOfEntries, bitsPerEntry, isLutDataSigned);
			else if (lutData is DicomAttributeUS)
				data = ExtractData(lutData.Values as ushort[], numberOfEntries, bitsPerEntry, isLutDataSigned);
			else if (lutData is DicomAttributeOW)
				data = ExtractData(lutData.Values as byte[], numberOfEntries, bitsPerEntry, isLutDataSigned);
			else
				throw new DicomDataException("Invalid VR for LutData.");

			return new DataLut(firstMappedPixelValue, bitsPerEntry, data, item[DicomTags.LutExplanation].ToString());
		}

		public static List<DataLut> Create(IEnumerable<DicomSequenceItem> items, bool isFirstMappedPixelSigned, bool isLutDataSigned)
		{
			if (items == null)
				throw new ArgumentNullException("items");

			List<DataLut> dataList = new List<DataLut>();

			foreach (DicomSequenceItem item in items)
				dataList.Add(Create(item, isFirstMappedPixelSigned, isLutDataSigned));

			return dataList;
		}

		public static List<DataLut> Create(DicomAttributeSQ sequence, bool isFirstMappedPixelSigned, bool isLutDataSigned)
		{
			if (sequence == null)
				throw new ArgumentNullException("sequence");
			
			if (sequence.IsEmpty || sequence.IsNull)
				return new List<DataLut>();

			DicomSequenceItem[] sequenceItems = sequence.Values as DicomSequenceItem[];
			if (sequenceItems == null || sequenceItems.Length == 0)
				return new List<DataLut>();

			return Create(sequenceItems, isFirstMappedPixelSigned, isLutDataSigned);
		}

		#endregion
		#endregion
	}
}
