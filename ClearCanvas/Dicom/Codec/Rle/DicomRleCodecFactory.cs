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
using System.Xml;
using ClearCanvas.Common;

namespace ClearCanvas.Dicom.Codec.Rle
{
    /// <summary>
    /// Default codec factory for the DICOM RLE Transfer Syntax.
    /// </summary>
	[ExtensionOf(typeof(DicomCodecFactoryExtensionPoint))]
    public class DicomRleCodecFactory : IDicomCodecFactory
    {
        private readonly string _name = TransferSyntax.RleLossless.Name;
        private readonly TransferSyntax _transferSyntax = TransferSyntax.RleLossless;

        public string Name
        {
            get { return _name; }
        }

        public TransferSyntax CodecTransferSyntax
        {
            get { return _transferSyntax; }
        }

        virtual public DicomCodecParameters GetCodecParameters(DicomAttributeCollection dataSet)
        {
			DicomRleCodecParameters codecParms = new DicomRleCodecParameters { ConvertPaletteToRGB = true };

			return codecParms;
		}
		public DicomCodecParameters GetCodecParameters(XmlDocument parms)
		{
			DicomRleCodecParameters codecParms = new DicomRleCodecParameters();

			XmlElement element = parms.DocumentElement;

			if (element != null && element.Attributes["convertFromPalette"]!=null)
			{
				String boolString = element.Attributes["convertFromPalette"].Value;
				bool convert;
				if (false == bool.TryParse(boolString, out convert))
					throw new ApplicationException("Invalid convertFromPalette value specified for RLE: " + boolString);
				codecParms.ConvertPaletteToRGB = convert;
			}
			else
				codecParms.ConvertPaletteToRGB = true;

			return codecParms;
		}
        public IDicomCodec GetDicomCodec()
        {
            return new DicomRleCodec();
        }
    }
}
