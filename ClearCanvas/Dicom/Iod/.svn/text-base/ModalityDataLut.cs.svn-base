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

namespace ClearCanvas.Dicom.Iod
{
	public class ModalityDataLut : DataLut
	{
		#region Private Fields

		private readonly string _modalityLutType;
		
		#endregion

		#region Constructors

		public ModalityDataLut(int firstMappedPixelValue, int bitsPerEntry, int[] data, string modalityLutType)
			: this(firstMappedPixelValue, bitsPerEntry, data, modalityLutType, null)
		{
		}

		public ModalityDataLut(int firstMappedPixelValue, int bitsPerEntry, int[] data, string modalityLutType, string explanation)
			: base(firstMappedPixelValue, bitsPerEntry, data, explanation)
		{
			_modalityLutType = modalityLutType;
		}

		public ModalityDataLut(ModalityDataLut item)
			: base(item)
		{
			_modalityLutType = item.ModalityLutType;
		}

		protected ModalityDataLut(DataLut dataLut, string modalityLutType)
			: base(dataLut.FirstMappedPixelValue, dataLut.BitsPerEntry, dataLut.Data,
					dataLut.Explanation, dataLut.MinOutputValue, dataLut.MaxOutputValue)
		{
			_modalityLutType = modalityLutType;
		}

		#endregion

		#region Public Properties

		public string ModalityLutType
		{
			get { return _modalityLutType; }
		}

		#endregion

		#region Internal/Public Static Factory Methods
		
		internal static ModalityDataLut Create(DicomAttributeSQ modalityLutSequence, int pixelRepresentation)
		{
			List<DataLut> data = DataLut.Create(modalityLutSequence, pixelRepresentation != 0, false);
			if (data.Count == 0)
				return null;

			string modalityLutType = ((DicomSequenceItem[]) modalityLutSequence.Values)[0][DicomTags.ModalityLutType].ToString();
			return new ModalityDataLut(data[0], modalityLutType);
		}

		public static ModalityDataLut Create(IDicomAttributeProvider dicomAttributeProvider)
		{
			DicomAttributeSQ modalityLutSequence = (DicomAttributeSQ)dicomAttributeProvider[DicomTags.ModalityLutSequence];
			int pixelRepresentation = GetPixelRepresentation(dicomAttributeProvider);

			return Create(modalityLutSequence, pixelRepresentation);
		}

		#endregion
	}
}