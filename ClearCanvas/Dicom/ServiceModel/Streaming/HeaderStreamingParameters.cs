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

using System.Runtime.Serialization;

namespace ClearCanvas.Dicom.ServiceModel.Streaming
{
    /// <summary>
    /// Encapsulates the parameters passed by the client to header streaming service.
    /// </summary>
    /// <remarks>
    /// <see cref="HeaderStreamingParameters"/> is passed to the service that implements <see cref="IHeaderStreamingService"/> 
    /// when the client wants to retrieve header information of a study. The study is identified by the <see cref="StudyInstanceUID"/>
    /// and the <see cref="ServerAETitle"/> where it is located.
    /// </remarks>
    [DataContract]
    public class HeaderStreamingParameters
    {
        #region Private members

        private string _serverAETitle;
        private string _studyInstanceUID;
        private string _referenceID;        
        #endregion Private members

        #region Public Properties

        /// <summary>
        /// Study instance UID of the study whose header will be retrieved.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string StudyInstanceUID
        {
            get { return _studyInstanceUID; }
            set { _studyInstanceUID = value; }
        }

        /// <summary>
        /// AE title of the server where the study is located.
        /// </summary>
        [DataMember(IsRequired=true)]
        public string ServerAETitle
        {
            get { return _serverAETitle; }
            set { _serverAETitle = value; }
        }

        /// <summary>
        /// A ticket for tracking purposes.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string ReferenceID
        {
            get { return _referenceID; }
            set { _referenceID = value; }
        }

        #endregion Public Properties
    }
}