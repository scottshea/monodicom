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

namespace ClearCanvas.Dicom.Utilities.Xml
{
	/// <summary>
	/// Enumerated value for setting how tags are included in the <see cref="StudyXml"/> file
	/// </summary>
	public enum StudyXmlTagInclusion
	{
		/// <summary>
		/// If a tag is encountered, its value will be ignored and not placed in the <see cref="StudyXml"/> file.
		/// </summary>
		IgnoreTag,
		/// <summary>
		/// If a tag is encountered, its value will be included in the <see cref="StudyXml"/> file.
		/// </summary>
		IncludeTagValue,
		/// <summary>
		/// If a tag is encountered, a flag will be set telling the value was in the header, but its not included in the <see cref="StudyXml"/> file.
		/// </summary>
		IncludeTagExclusion,
	}

	

    /// <summary>
    /// Output settings for <see cref="StudyXml"/> when creating the Xml.
    /// </summary>
    public class StudyXmlOutputSettings
    {
        #region Constants

        public const long MAX_TAG_LENGTH = 1024*100; //100KB

        #endregion

        #region Private Members

		private StudyXmlTagInclusion _includePrivateValues = StudyXmlTagInclusion.IgnoreTag;
		private StudyXmlTagInclusion _includeUnknownTags = StudyXmlTagInclusion.IgnoreTag;
		private StudyXmlTagInclusion _includeLargeTags = StudyXmlTagInclusion.IncludeTagExclusion;
		private bool _includeSourceFileName = false;
		private ulong _maxTagLength = MAX_TAG_LENGTH;

        #endregion

        #region Public Static Properties
        /// <summary>
        /// Represents an empty settings. This field is readonly.
        /// </summary>
        static public StudyXmlOutputSettings None
        {
            get
            {
                return null;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
		/// Specifies whether or not to include UN tags in the header.
        /// </summary>
		public StudyXmlTagInclusion IncludeUnknownTags
        {
            get { return _includeUnknownTags; }
            set { _includeUnknownTags = value; }
        }

		/// <summary>
		/// Specifies whether or not to include large tags, where large tags are defined by <see cref="MaxTagLength"/>
		/// </summary>
    	public StudyXmlTagInclusion IncludeLargeTags
    	{
			get { return _includeLargeTags; }
			set { _includeLargeTags = value; }
    	}

        /// <summary>
		/// Specifies the maximum allowed length of the tag values in the header if <see cref="IncludeLargeTags"/> is 
		/// not set to <see cref="StudyXmlTagInclusion.IncludeTagValue"/>.
        /// </summary>
        public ulong MaxTagLength
        {
            get { return _maxTagLength; }
            set { _maxTagLength = value; }
        }

        /// <summary>
        /// Specifies whether or not to include private tags.
        /// </summary>
        /// <remarks>
        /// If the private tag VR is UN, its presence in the header is determined by <see cref="IncludeUnknownTags"/>.
        /// </remarks>
		public StudyXmlTagInclusion IncludePrivateValues
        {
            get { return _includePrivateValues; }
            set { _includePrivateValues = value; }
        }

		/// <summary>
		/// Specifies whether or not to include the source filename in the xml.
		/// </summary>
    	public bool IncludeSourceFileName
    	{
			get { return _includeSourceFileName; }
			set { _includeSourceFileName = value; }
    	}

        #endregion
    }
}