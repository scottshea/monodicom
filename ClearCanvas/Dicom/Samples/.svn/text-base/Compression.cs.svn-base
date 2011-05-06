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
using System.IO;
using ClearCanvas.Dicom.Iod.Modules;

namespace ClearCanvas.Dicom.Samples
{
	public class Compression
	{
		private readonly string _sourceFilename;
		private DicomFile _dicomFile;

		public Compression(string file)
		{
			_sourceFilename = file;
		}

		public DicomFile DicomFile
		{
			get { return _dicomFile; }
		}

		public void Load()
		{
			_dicomFile = new DicomFile(_sourceFilename);
			try
			{
				_dicomFile.Load();
			}
			catch (Exception e)
			{
				Logger.LogErrorException(e, "Unexpected exception loading DICOM file: {0}", _sourceFilename);
			}
		}

		public void ChangeSyntax(TransferSyntax syntax)
		{
			try
			{
				if (!_dicomFile.TransferSyntax.Encapsulated)
				{
					// Check if Overlay is embedded in pixels
					OverlayPlaneModuleIod overlayIod = new OverlayPlaneModuleIod(_dicomFile.DataSet);
					for (int i = 0; i < 16; i++)
					{
						if (overlayIod.HasOverlayPlane(i))
						{
							OverlayPlane overlay = overlayIod[i];
							if (overlay.OverlayData == null)
							{
								DicomUncompressedPixelData pd = new DicomUncompressedPixelData(_dicomFile);
								overlay.ConvertEmbeddedOverlay(pd);	
							}
						}
					}
				}
				else if (syntax.Encapsulated)
				{
					// Must decompress first.
					_dicomFile.ChangeTransferSyntax(TransferSyntax.ExplicitVrLittleEndian);
				}

				_dicomFile.ChangeTransferSyntax(syntax);
			}
			catch (Exception e)
			{
				Logger.LogErrorException(e, "Unexpected exception compressing/decompressing DICOM file");
			}
		}

		public void Save(string filename)
		{
			try
			{
				_dicomFile.Save(filename);
			}
			catch (Exception e)
			{
				Logger.LogErrorException(e, "Unexpected exception saving dicom file: {0}", filename);
			}
		}

		public void SavePixels(string filename)
		{
			DicomPixelData pd = DicomPixelData.CreateFrom(_dicomFile);

			if (File.Exists(filename))
				File.Delete(filename);

			using (FileStream fs = new FileStream(filename, FileMode.CreateNew))
			{
				byte[] ba;
				DicomCompressedPixelData compressed = pd as DicomCompressedPixelData;
				if (compressed != null) 
					ba = compressed.GetFrameFragmentData(0);
				else
					ba = pd.GetFrame(0);

				fs.Write(ba, 0, ba.Length);
				fs.Flush();
				fs.Close();
			}
		}
	}
}
