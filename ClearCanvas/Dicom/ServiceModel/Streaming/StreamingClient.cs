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
using System.Net;
using System.Text;
using System.Web;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using System.Diagnostics;
using System.Net.Cache;

namespace ClearCanvas.Dicom.ServiceModel.Streaming
{
	public class RetrievePixelDataResult
	{
		private readonly FrameStreamingResultMetaData _metaData;
		private DicomCompressedPixelData _compressedPixelData;
		private byte[] _pixelData;

		internal RetrievePixelDataResult(byte[] uncompressedPixelData, FrameStreamingResultMetaData resultMetaData)
		{
			_pixelData = uncompressedPixelData;
			_metaData = resultMetaData;
		}

		internal RetrievePixelDataResult(DicomCompressedPixelData compressedPixelData, FrameStreamingResultMetaData resultMetaData)
		{
			_compressedPixelData = compressedPixelData;
			_metaData = resultMetaData;
		}

		public FrameStreamingResultMetaData MetaData
		{
			get { return _metaData; }	
		}

		public byte[] GetPixelData()
		{
			if (_compressedPixelData != null)
			{
				try
				{
					byte[] uncompressed = _compressedPixelData.GetFrame(0);

					_pixelData = uncompressed;
					_compressedPixelData = null;
				}
				catch (Exception ex)
				{
					throw new Exception(String.Format("Error occurred while decompressing the pixel data: {0}", ex.Message));
				}
			}

			return _pixelData;
		}
	}

    /// <summary>
    /// Represents a web client that can be used to retrieve study images or pixel data from a streaming server using WADO protocol.
    /// </summary>
    public class StreamingClient
    {
        private readonly Uri _baseUri;

        /// <summary>
        /// Creates an instance of <see cref="StreamingClient"/> to connect to a streaming server.
        /// </summary>
        /// <param name="baseUri">Base Uri to the location where the streaming server is located (eg http://localhost:1000/wado)</param>
        public StreamingClient(Uri baseUri)
        {
            _baseUri = baseUri;
        }

        #region Public Methods

		public RetrievePixelDataResult RetrievePixelData(string serverAE, string studyInstanceUID, string seriesInstanceUID, string sopInstanceUid, int frame)
        {
			try
			{
				CodeClock clock = new CodeClock();
				clock.Start();

				FrameStreamingResultMetaData result = new FrameStreamingResultMetaData();
				StringBuilder url = new StringBuilder();

				if (_baseUri.ToString().EndsWith("/"))
				{
					url.AppendFormat("{0}{1}", _baseUri, serverAE);
				}
				else
				{
					url.AppendFormat("{0}/{1}", _baseUri, serverAE);
				}

				url.AppendFormat("?requesttype=WADO&studyUID={0}&seriesUID={1}&objectUID={2}", studyInstanceUID, seriesInstanceUID, sopInstanceUid);
				url.AppendFormat("&frameNumber={0}", frame);
				url.AppendFormat("&contentType={0}", HttpUtility.HtmlEncode("application/clearcanvas"));

				result.Speed.Start();

				HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url.ToString());
				request.Accept = "application/dicom,application/clearcanvas,image/jpeg";
				request.Timeout = (int) TimeSpan.FromSeconds(StreamingSettings.Default.ClientTimeoutSeconds).TotalMilliseconds;
				request.KeepAlive = false;

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();

				if (response.StatusCode != HttpStatusCode.OK)
				{
					throw new StreamingClientException(response.StatusCode, HttpUtility.HtmlDecode(response.StatusDescription));
				}

				Stream responseStream = response.GetResponseStream();
				BinaryReader reader = new BinaryReader(responseStream);
				byte[] buffer = reader.ReadBytes((int) response.ContentLength);
				reader.Close();
				responseStream.Close();
				response.Close();

				result.Speed.SetData(buffer.Length);
				result.Speed.End();

				result.ResponseMimeType = response.ContentType;
				result.Status = response.StatusCode;
				result.StatusDescription = response.StatusDescription;
				result.Uri = response.ResponseUri;
				result.ContentLength = buffer.Length;
				result.IsLast = (response.Headers["IsLast"] != null && bool.Parse(response.Headers["IsLast"]));

				clock.Stop();
				PerformanceReportBroker.PublishReport("Streaming", "RetrievePixelData", clock.Seconds);

				RetrievePixelDataResult pixelDataResult;
				if (response.Headers["Compressed"] != null && bool.Parse(response.Headers["Compressed"]))
					pixelDataResult = new RetrievePixelDataResult(CreateCompressedPixelData(response, buffer), result);
				else
					pixelDataResult = new RetrievePixelDataResult(buffer, result);

				return pixelDataResult;
			}
			catch (WebException ex)
			{
				if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response is HttpWebResponse)
				{
					HttpWebResponse response = (HttpWebResponse) ex.Response;
					throw new StreamingClientException(response.StatusCode, HttpUtility.HtmlDecode(response.StatusDescription));
				}
				throw new StreamingClientException(StreamingClientExceptionType.Network, ex);
			}
		}

		public Stream RetrieveImageHeader(string serverAE, string studyInstanceUID, string seriesInstanceUID, string sopInstanceUid)
		{
			StreamingResultMetaData metaInfo;
			return RetrieveImageHeader(serverAE, studyInstanceUID, seriesInstanceUID, sopInstanceUid, DicomTags.PixelData, out metaInfo);
		}

		public Stream RetrieveImageHeader(string serverAE, string studyInstanceUID, string seriesInstanceUID, string sopInstanceUid, uint stopTag, out StreamingResultMetaData metaInfo)
		{
        	string imageUrl = BuildImageUrl(serverAE, studyInstanceUID, seriesInstanceUID, sopInstanceUid);
			imageUrl = imageUrl + String.Format("&stopTag={0:x8}", stopTag);
			imageUrl = imageUrl + String.Format("&contentType={0}", HttpUtility.HtmlEncode("application/clearcanvas-header"));
        	return RetrieveImageData(imageUrl, out metaInfo);
		}
		
		public Stream RetrieveImage(string serverAE, string studyInstanceUID, string seriesInstanceUID, string sopInstanceUid)
        {
            StreamingResultMetaData result;
            return RetrieveImage(serverAE, studyInstanceUID, seriesInstanceUID, sopInstanceUid, out result);
        }

        public Stream RetrieveImage(string serverAE, string studyInstanceUID, string seriesInstanceUID, string sopInstanceUid, out StreamingResultMetaData metaInfo)
        {
        	string imageUrl = BuildImageUrl(serverAE, studyInstanceUID, seriesInstanceUID, sopInstanceUid);
			imageUrl = imageUrl + String.Format("&contentType={0}", HttpUtility.HtmlEncode("application/dicom"));
        	return RetrieveImageData(imageUrl, out metaInfo);
        }

        #endregion Public Methods

		#region Private Methods

		private string BuildImageUrl(string serverAE, string studyInstanceUid, string seriesInstanceUid, string sopInstanceUid)
		{
			Platform.CheckForEmptyString(serverAE, "serverAE");
			Platform.CheckForEmptyString(studyInstanceUid, "studyInstanceUid");
			Platform.CheckForEmptyString(seriesInstanceUid, "seriesInstanceUid");
			Platform.CheckForEmptyString(sopInstanceUid, "sopInstanceUid");

			StringBuilder url = new StringBuilder();
			if (_baseUri.ToString().EndsWith("/"))
			{
				url.AppendFormat("{0}{1}", _baseUri, serverAE);
			}
			else
			{
				url.AppendFormat("{0}/{1}", _baseUri, serverAE);
			}

			url.AppendFormat("?requesttype=WADO&studyUID={0}&seriesUID={1}&objectUID={2}", studyInstanceUid, seriesInstanceUid, sopInstanceUid);
			return url.ToString();
		}

		private static MemoryStream RetrieveImageData(string url, out StreamingResultMetaData result)
		{
			try
			{
				result = new StreamingResultMetaData();

				result.Speed.Start();

				HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url.ToString());
				request.Accept = "application/dicom,application/clearcanvas,application/clearcanvas-header,image/jpeg";
				request.Timeout = (int) TimeSpan.FromSeconds(StreamingSettings.Default.ClientTimeoutSeconds).TotalMilliseconds;
				request.KeepAlive = false;

				HttpWebResponse response = (HttpWebResponse) request.GetResponse();
				if (response.StatusCode != HttpStatusCode.OK)
				{
					throw new StreamingClientException(response.StatusCode, HttpUtility.HtmlDecode(response.StatusDescription));
				}

				Stream responseStream = response.GetResponseStream();
				BinaryReader reader = new BinaryReader(responseStream);
				byte[] buffer = reader.ReadBytes((int) response.ContentLength);
				reader.Close();
				responseStream.Close();
				response.Close();

				result.Speed.SetData(buffer.Length);
				result.Speed.End();

				result.ResponseMimeType = response.ContentType;
				result.Status = response.StatusCode;
				result.StatusDescription = response.StatusDescription;
				result.Uri = response.ResponseUri;
				result.ContentLength = buffer.Length;

				return new MemoryStream(buffer);
			}
			catch (WebException ex)
			{
				if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response is HttpWebResponse)
				{
					HttpWebResponse response = (HttpWebResponse) ex.Response;
					throw new StreamingClientException(response.StatusCode, HttpUtility.HtmlDecode(response.StatusDescription));
				}
				throw new StreamingClientException(StreamingClientExceptionType.Network, ex);
			}
		}

		private static DicomCompressedPixelData CreateCompressedPixelData(HttpWebResponse response, byte[] pixelDataBuffer)
		{
			string transferSyntaxUid = response.Headers["TransferSyntaxUid"];
			TransferSyntax transferSyntax = TransferSyntax.GetTransferSyntax(transferSyntaxUid);
			ushort bitsAllocated = ushort.Parse(response.Headers["BitsAllocated"]);
			ushort bitsStored = ushort.Parse(response.Headers["BitsStored"]);
			ushort height = ushort.Parse(response.Headers["ImageHeight"]);
			ushort width = ushort.Parse(response.Headers["ImageWidth"]);
			ushort samples = ushort.Parse(response.Headers["SamplesPerPixel"]);

			DicomAttributeCollection collection = new DicomAttributeCollection();
			collection[DicomTags.BitsAllocated].SetUInt16(0, bitsAllocated);
			collection[DicomTags.BitsStored].SetUInt16(0, bitsStored);
			collection[DicomTags.HighBit].SetUInt16(0, ushort.Parse(response.Headers["HighBit"]));
			collection[DicomTags.Rows].SetUInt16(0, height);
			collection[DicomTags.Columns].SetUInt16(0, width);
			collection[DicomTags.PhotometricInterpretation].SetStringValue(response.Headers["PhotometricInterpretation"]);
			collection[DicomTags.PixelRepresentation].SetUInt16(0, ushort.Parse(response.Headers["PixelRepresentation"]));
			collection[DicomTags.SamplesPerPixel].SetUInt16(0, samples);
			collection[DicomTags.DerivationDescription].SetStringValue(response.Headers["DerivationDescription"]);
			collection[DicomTags.LossyImageCompression].SetStringValue(response.Headers["LossyImageCompression"]);
			collection[DicomTags.LossyImageCompressionMethod].SetStringValue(response.Headers["LossyImageCompressionMethod"]);
			collection[DicomTags.LossyImageCompressionRatio].SetFloat32(0, float.Parse(response.Headers["LossyImageCompressionRatio"]));
			collection[DicomTags.PixelData] = new DicomFragmentSequence(DicomTags.PixelData);

			ushort planar;
			if (ushort.TryParse(response.Headers["PlanarConfiguration"], out planar))
				collection[DicomTags.PlanarConfiguration].SetUInt16(0, planar);

			DicomCompressedPixelData cpd = new DicomCompressedPixelData(collection);
			cpd.TransferSyntax = transferSyntax;
			cpd.AddFrameFragment(pixelDataBuffer);

			return cpd;
		}

		#endregion
	}
}