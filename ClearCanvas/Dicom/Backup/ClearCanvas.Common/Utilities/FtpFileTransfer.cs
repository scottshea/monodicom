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
using System.Net;
using System.IO;

namespace ClearCanvas.Common.Utilities
{
	/// <summary>
	/// Provide access to remote files using the FTP protocol.
	/// </summary>
	public class FtpFileTransfer : IRemoteFileTransfer
	{
		private readonly string _userId;
		private readonly string _password;
		private readonly bool _usePassive;

		// Keep track of the url created to reduce the number of FTP mkdir call
		private readonly List<string> _urlCreated;

		/// <summary>
		/// Get the base Uri for the FTP site.
		/// </summary>
		public Uri BaseUri { get; private set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public FtpFileTransfer(string userId, string password, string baseUri, bool usePassive)
		{
			_userId = userId;
			_password = password;
			_usePassive = usePassive;
			BaseUri = CreateProperBaseUri(baseUri);
			_urlCreated = new List<string>();
		}

		/// <summary>
		/// Upload one file from local to remote.
		/// </summary>
		/// <param name="request"></param>
		public void Upload(FileTransferRequest request)
		{
			try
			{
				CreateRemoteDirectoryForFile(request.RemoteFile);

				// Create a FTP request to upload file
				var localFileInf = new FileInfo(request.LocalFile);
				var ftpRequest = (FtpWebRequest)WebRequest.Create(request.RemoteFile);
				ftpRequest.Credentials = new NetworkCredential(_userId, _password);
				ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
				ftpRequest.UseBinary = true;
				ftpRequest.UsePassive = _usePassive;
				ftpRequest.ContentLength = localFileInf.Length;

				// Open ftp and local streams
				using (var localFileStream = localFileInf.OpenRead())
				using (var ftpRequestStream = ftpRequest.GetRequestStream())
				{
					// Write Content from the file stream to the FTP Upload Stream
					const int bufferLength = 2048;
					var buffer = new byte[bufferLength];
					var localFileContentLength = localFileStream.Read(buffer, 0, bufferLength);
					while (localFileContentLength != 0)
					{
						ftpRequestStream.Write(buffer, 0, localFileContentLength);
						localFileContentLength = localFileStream.Read(buffer, 0, bufferLength);
					}
				}
			}
			catch (Exception e)
			{
				//TODO (cr Oct 2009): we're not supposed to use SR for exception messages.
				//Throw a different type of exception and use an ExceptionHandler if it's supposed to be a user message.
				var message = string.Format(SR.ExceptionFailedToTransferFile, request.LocalFile, request.RemoteFile);
				throw new Exception(message, e);
			}
		}

		/// <summary>
		/// Download one file from remote to local
		/// </summary>
		/// <param name="request"></param>
		public void Download(FileTransferRequest request)
		{
			try
			{
				// Create a FTP request to download file
				var ftpRequest = (FtpWebRequest) WebRequest.Create(request.RemoteFile);
				ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
				ftpRequest.UseBinary = true;
				ftpRequest.UsePassive = _usePassive;
				ftpRequest.Credentials = new NetworkCredential(_userId, _password);

				// Create download directory if not already exist
				var downloadDirectory = Path.GetDirectoryName(request.LocalFile);
				if (!Directory.Exists(downloadDirectory))
					Directory.CreateDirectory(downloadDirectory);

				// Open ftp and local streams

				using (var ftpResponse = (FtpWebResponse) ftpRequest.GetResponse())
				using (var ftpResponseStream = ftpResponse.GetResponseStream())
				using (var localFileStream = new FileStream(request.LocalFile, FileMode.Create))
				{
					// Write Content from the FTP download stream to local file stream
					const int bufferSize = 2048;
					var buffer = new byte[bufferSize];
					var readCount = ftpResponseStream.Read(buffer, 0, bufferSize);
					while (readCount > 0)
					{
						localFileStream.Write(buffer, 0, readCount);
						readCount = ftpResponseStream.Read(buffer, 0, bufferSize);
					}
				}
			}
			catch (Exception e)
			{
				//TODO (cr Oct 2009): we're not supposed to use SR for exception messages.
				//Throw a different type of exception and use an ExceptionHandler if it's supposed to be a user message.
				var message = string.Format(SR.ExceptionFailedToTransferFile, request.RemoteFile, request.LocalFile);
				throw new Exception(message, e);
			}
		}

		private void CreateRemoteDirectoryForFile(Uri urlToCreate)
		{
			var uri = new Uri(this.BaseUri.ToString());

			// Continues to build uri and create them
			foreach (var segment in urlToCreate.Segments)
			{
				uri = new Uri(uri, segment);

				var isFileSegment = segment == urlToCreate.Segments[urlToCreate.Segments.Length - 1];

				if (_urlCreated.Contains(uri.ToString()) || 
					Equals(this.BaseUri, uri) ||	// The base Uri should already exist
					isFileSegment)					// Skip the file segment, so we don't create a directory with the same name as the file
					continue;

				try
				{
					var ftpRequest = (FtpWebRequest)WebRequest.Create(uri);
					ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
					ftpRequest.UseBinary = true;
					ftpRequest.UsePassive = _usePassive;
					ftpRequest.Credentials = new NetworkCredential(_userId, _password);
					
					var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
					ftpResponse.Close();
				}
				catch (Exception)
				{
					// Purposely swallowing exception here because the remote folders may already exist.
					// But this is okay because if there is a real problem creating folders, 
					// another exception will be thrown when transfering files
				}

				_urlCreated.Add(uri.ToString());
			}
		}

		private static Uri CreateProperBaseUri(string baseUri)
		{
			// Construct a temporary Uri object.  The first segment is always the slash
			var tempUri = new Uri(baseUri);
			var segmentDelimiter = tempUri.Segments[0];

			// Make sure the baseUri always ends with a trailing slash.
			return baseUri.EndsWith(segmentDelimiter) 
				? new Uri(baseUri) 
				: new Uri(string.Concat(baseUri, segmentDelimiter));
		}
	}
}