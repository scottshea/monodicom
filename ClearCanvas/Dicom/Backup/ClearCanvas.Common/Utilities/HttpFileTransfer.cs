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
using System.Net;
using System.IO;

namespace ClearCanvas.Common.Utilities
{
	/// <summary>
	/// Provide access to remote files with Http scheme.
	/// </summary>
	/// <remarks>
	/// This provider class does not create remote directory before uploading files.
	/// </remarks>
	public class HttpFileTransfer : IRemoteFileTransfer
	{
		private readonly string _userId;
		private readonly string _password;

		/// <summary>
		/// Default constructor with no authentication.
		/// </summary>
		public HttpFileTransfer()
		{
		}

		/// <summary>
		/// Constructor with authentication provided.
		/// </summary>
		public HttpFileTransfer(string userId, string password)
		{
			_userId = userId;
			_password = password;
		}

		/// <summary>
		/// Upload one file from local to remote.
		/// </summary>
		/// <param name="request"></param>
		/// <remarks>
		/// The remote directories are not created before uploading files.
		/// </remarks>
		public void Upload(FileTransferRequest request)
		{
			try
			{
				using (var webClient = new WebClient())
				{
					if (!string.IsNullOrEmpty(_userId) && !string.IsNullOrEmpty(_password))
						webClient.Credentials = new NetworkCredential(_userId, _password);

					webClient.UploadFile(request.RemoteFile, request.LocalFile);
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

		/// <summary>
		/// Download one file from remote to local
		/// </summary>
		/// <param name="request"></param>
		public void Download(FileTransferRequest request)
		{
			try
			{
				using (var webClient = new WebClient())
				{
					if (!string.IsNullOrEmpty(_userId) && !string.IsNullOrEmpty(_password))
						webClient.Credentials = new NetworkCredential(_userId, _password);

					var downloadDirectory = Path.GetDirectoryName(request.LocalFile);
					if (!Directory.Exists(downloadDirectory))
						Directory.CreateDirectory(downloadDirectory);

					webClient.DownloadFile(request.RemoteFile, request.LocalFile);
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
	}
}
