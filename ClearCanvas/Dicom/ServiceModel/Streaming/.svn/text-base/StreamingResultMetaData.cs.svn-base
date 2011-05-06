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
using ClearCanvas.Common.Statistics;

namespace ClearCanvas.Dicom.ServiceModel.Streaming
{
    /// <summary>
    /// Represents the result of an image streaming operation
    /// </summary>
    public class StreamingResultMetaData
    {
        private HttpStatusCode _status;
        private string _statusDescription;
        private string _mimeType;
        private long _contentLength;
        private Uri _uri;

        private RateStatistics _speed = new RateStatistics("Speed", RateType.BYTES);
        
        public HttpStatusCode Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string StatusDescription
        {
            get { return _statusDescription; }
            set { _statusDescription = value; }
        }

        public string ResponseMimeType
        {
            get { return _mimeType; }
            set { _mimeType = value; }
        }


        public Uri Uri
        {
            get { return _uri; }
            set { _uri = value; }
        }

        public RateStatistics Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public long ContentLength
        {
            get { return _contentLength; }
            set { _contentLength = value; }
        }

    }

    /// <summary>
    /// Represents the result of a frame streaming operation
    /// </summary>
    public class FrameStreamingResultMetaData : StreamingResultMetaData
    {
        private bool _isLast = false;

        /// <summary>
        /// Indicates whether the current frame is the last frame in the image.
        /// </summary>
        public bool IsLast
        {
            get { return _isLast; }
            set { _isLast = value; }
        }
    }
}
