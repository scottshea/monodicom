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

#region mDCM License
// mDCM: A C# DICOM library
//
// Copyright (c) 2008  Colby Dillion
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Author:
//    Colby Dillion (colby.dillion@gmail.com)
#endregion

using System.Xml;

namespace ClearCanvas.Dicom.Codec
{
	//TODO: empty class - just return an object?

	/// <summary>
    /// Abstract base class for parameters to codecs.
    /// </summary>
    public abstract class DicomCodecParameters
    {
        #region Private Members

		#endregion

        #region Public Members
        #endregion

        #region Public Properties

		/// <summary>
		/// Specifies if Palette Color images should be converted to RGB for compression.
		/// </summary>
		public bool ConvertPaletteToRGB { get; set; }

		#endregion
    }

    /// <summary>
    /// Interface for Dicom Compressor/Decompressors.
    /// </summary>
    public interface IDicomCodec
    {
        /// <summary>
        /// The name of the Codec
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The <see cref="CodecTransferSyntax"/> the codec supports.
        /// </summary>
        TransferSyntax CodecTransferSyntax { get; }
        /// <summary>
        /// Encode (compress) the entire pixel data.
        /// </summary>
        /// <param name="oldPixelData">The uncompressed pixel data</param>
        /// <param name="newPixelData">The output compressed pixel data</param>
        /// <param name="parameters">The codec parameters</param>
        void Encode(DicomUncompressedPixelData oldPixelData, DicomCompressedPixelData newPixelData, DicomCodecParameters parameters);
        /// <summary>
        /// Decode (decompress) the entire pixel data.
        /// </summary>
        /// <param name="oldPixelData">The source compressed pixel data.</param>
        /// <param name="newPixelData">The output pixel data.</param>
        /// <param name="parameters">The codec parameters.</param>
        void Decode(DicomCompressedPixelData oldPixelData, DicomUncompressedPixelData newPixelData, DicomCodecParameters parameters);

        /// <summary>
        /// Decode a single frame of pixel data.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Note this method is strictly used with the <see cref="DicomCompressedPixelData"/> class's
        /// GetFrame() method to compress data frame by frame.  It is expected that the frame data will be output into 
        /// <paramref name="newPixelData"/> as a single frame of data.
        /// </para>
        /// <para>
        /// If a DICOM file is loaded with the <see cref="DicomReadOptions.StorePixelDataReferences"/>
        /// option set, this method in conjunction with the <see cref="DicomCompressedPixelData"/> 
        /// class can allow the library to only load a frame of data at a time.
        /// </para>
        /// </remarks>
        /// <param name="frame">A zero offset frame number</param>
        /// <param name="oldPixelData">The input pixel data (including all frames)</param>
        /// <param name="newPixelData">The output pixel data is stored here</param>
        /// <param name="parameters">The codec parameters</param>
        void DecodeFrame(int frame, DicomCompressedPixelData oldPixelData, DicomUncompressedPixelData newPixelData, DicomCodecParameters parameters);	

    }

    /// <summary>
    /// Interface for factory for creating DICOM Compressors/Decompressors.
    /// </summary>
    public interface IDicomCodecFactory
    {
        /// <summary>
        /// The name of the factory.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The transfer syntax associated with the factory.
        /// </summary>
        TransferSyntax CodecTransferSyntax { get; }
        /// <summary>
        /// Get the codec parameters.
        /// </summary>
        /// <param name="dataSet">The data set to get codec parameters for.  Note that this value may be null.</param>
        /// <returns>The codec parameters.</returns>
        DicomCodecParameters GetCodecParameters(DicomAttributeCollection dataSet);
		/// <summary>
		/// Get the codec parameters.
		/// </summary>
		/// <param name="parms">XML based codec parameters.</param>
		/// <returns>The codec parameters.</returns>
		DicomCodecParameters GetCodecParameters(XmlDocument parms);
        /// <summary>
        /// Get an <see cref="IDicomCodec"/> codec.
        /// </summary>
        /// <returns></returns>
        IDicomCodec GetDicomCodec();
    }
}
