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
using ClearCanvas.Common;

namespace ClearCanvas.Dicom.DataStore
{
    //TODO: Get rid of this?  It's always just converted to/from a string anyway.
	public class DicomUri : IEquatable<DicomUri>
    {
		private Uri _internalUriObject;

		//for NHibernate.
		private DicomUri()
		{
		}

    	/// <summary>
        /// Constructor.
        /// </summary>
		public DicomUri(string uri)
        {
        	SetInternalUri(uri);
        }

		/// <summary>
		/// Constructor.
		/// </summary>
		public DicomUri(Uri newUri)
		{
			Platform.CheckForNullReference(newUri, "newUri");
			InternalUriObject = newUri;
		}

		/// <summary>
		/// NHibernate Property.
		/// </summary>
		protected virtual string InternalUri
		{
			get { return InternalUriObject.AbsoluteUri; }
			set { SetInternalUri(value); }
		}

		private void SetInternalUri(string uri)
		{
			if (String.IsNullOrEmpty(uri))
				throw new ArgumentNullException("uri", "Uri cannot be null or empty.");

			InternalUriObject = new Uri(uri);
		}

		private Uri InternalUriObject
		{
			get { return _internalUriObject; }
			set { _internalUriObject = value; }
		}
	
        public bool IsFile
        {
            get 
            {
                if (null != InternalUriObject)
                    return InternalUriObject.IsFile;
                else
                    return false;
            }
        }

        public string LocalPath
        {
            get 
            {
                if (null != InternalUriObject)
                    return InternalUriObject.LocalPath;
                else
                    return null;
            }
        }

        public string LocalDiskPath
        {
            get
            {
                if (null != InternalUriObject && InternalUriObject.IsFile)
                    return InternalUriObject.LocalPath.Substring(12);      // remove the "\\localhost\" part
                else
                    return null;
            }
        }

    	#region IEquatable<DicomUri> Members

    	public bool Equals(DicomUri other)
    	{
			if (other == null)
				return false;

			return InternalUriObject.Equals(other.InternalUriObject);
    	}

    	#endregion

    	public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

			if (obj is DicomUri)
				return Equals((DicomUri) obj);

            return false;
        }

		public override int GetHashCode()
		{
			return _internalUriObject.GetHashCode();
		}

        public override string ToString()
        {
            return InternalUriObject.AbsoluteUri;
        }

        /// <summary>
        /// Implicit cast to a String object, for ease of use.
        /// </summary>
        public static implicit operator string(DicomUri uri)
        {
            return uri.ToString();
        }
    }
}
