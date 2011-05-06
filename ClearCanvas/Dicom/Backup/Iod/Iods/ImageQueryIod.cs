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

namespace ClearCanvas.Dicom.Iod.Iods
{
    /// <summary>
    /// IOD for common Image Query Retrieve items.  
    /// </summary>
    public class ImageQueryIod : QueryIodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageQueryIod"/> class.
        /// </summary>
        public ImageQueryIod()
        {
            SetAttributeFromEnum(DicomAttributeProvider[DicomTags.QueryRetrieveLevel], QueryRetrieveLevel.Image);
        }

		public ImageQueryIod(IDicomAttributeProvider dicomAttributeProvider)
			: base(dicomAttributeProvider)
		{
			SetAttributeFromEnum(DicomAttributeProvider[DicomTags.QueryRetrieveLevel], QueryRetrieveLevel.Image);
		}

    	#endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the study instance uid.
        /// </summary>
        /// <value>The study instance uid.</value>
        public string StudyInstanceUid
        {
            get { return DicomAttributeProvider[DicomTags.StudyInstanceUid].GetString(0, String.Empty); }
            set { DicomAttributeProvider[DicomTags.StudyInstanceUid].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the series instance uid.
        /// </summary>
        /// <value>The series instance uid.</value>
        public string SeriesInstanceUid
        {
            get { return DicomAttributeProvider[DicomTags.SeriesInstanceUid].GetString(0, String.Empty); }
            set { DicomAttributeProvider[DicomTags.SeriesInstanceUid].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the sop instance uid.
        /// </summary>
        /// <value>The sop instance uid.</value>
        public string SopInstanceUid
        {
            get { return DicomAttributeProvider[DicomTags.SopInstanceUid].GetString(0, String.Empty); }
            set { DicomAttributeProvider[DicomTags.SopInstanceUid].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the instance number.
        /// </summary>
        /// <value>The instance number.</value>
        public string InstanceNumber
        {
            get { return DicomAttributeProvider[DicomTags.InstanceNumber].GetString(0, String.Empty); }
            set { DicomAttributeProvider[DicomTags.InstanceNumber].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the sop class uid.
        /// </summary>
        /// <value>The sop class uid.</value>
        public string SopClassUid
        {
            get { return DicomAttributeProvider[DicomTags.SopClassUid].GetString(0, String.Empty); }
            set { DicomAttributeProvider[DicomTags.SopClassUid].SetString(0, value); }
        }

		/// <summary>
		///  Get the number of Rows
		/// </summary>
		public ushort Rows
		{
			get { return DicomAttributeProvider[DicomTags.Rows].GetUInt16(0, 0); }
		}

		/// <summary>
		/// Get the number of columns
		/// </summary>
		public ushort Columns
		{
			get { return DicomAttributeProvider[DicomTags.Columns].GetUInt16(0, 0); }
		}

		/// <summary>
		/// Get the Bits Allocated
		/// </summary>
		public ushort BitsAllocated
		{
			get { return DicomAttributeProvider[DicomTags.BitsAllocated].GetUInt16(0, 0); }
		}

		/// <summary>
		/// Get the number of frames
		/// </summary>
		public string NumberOfFrames
		{
			get { return DicomAttributeProvider[DicomTags.NumberOfFrames].GetString(0, String.Empty); }
		}

		/// <summary>
		/// Get the content label
		/// </summary>
		public string ContentLabel
		{
			get { return DicomAttributeProvider[DicomTags.ContentLabel].GetString(0, String.Empty); }
		}

		/// <summary>
		/// Get the content description
		/// </summary>
		public string ContentDescription
		{
			get { return DicomAttributeProvider[DicomTags.ContentDescription].GetString(0, String.Empty); }
		}

    	#endregion

        #region Public Methods
        /// <summary>
        /// Sets the common tags for a query retrieve request.
        /// </summary>
        public void SetCommonTags()
        {
            SetCommonTags(DicomAttributeProvider);
        }

		public static void SetCommonTags(IDicomAttributeProvider dicomAttributeProvider)
		{
			SetAttributeFromEnum(dicomAttributeProvider[DicomTags.QueryRetrieveLevel], QueryRetrieveLevel.Image);

			// Set image level..
			dicomAttributeProvider[DicomTags.SopInstanceUid].SetNullValue();
			dicomAttributeProvider[DicomTags.InstanceNumber].SetNullValue();
			dicomAttributeProvider[DicomTags.SopClassUid].SetNullValue();
			// IHE specified Image Query Keys
			dicomAttributeProvider[DicomTags.Rows].SetNullValue();
			dicomAttributeProvider[DicomTags.Columns].SetNullValue();
			dicomAttributeProvider[DicomTags.BitsAllocated].SetNullValue();
			dicomAttributeProvider[DicomTags.NumberOfFrames].SetNullValue();
			// IHE specified Presentation State Query Keys
			dicomAttributeProvider[DicomTags.ContentLabel].SetNullValue();
			dicomAttributeProvider[DicomTags.ContentDescription].SetNullValue();
			dicomAttributeProvider[DicomTags.PresentationCreationDate].SetNullValue();
			dicomAttributeProvider[DicomTags.PresentationCreationTime].SetNullValue();
			// IHE specified Report Query Keys
			dicomAttributeProvider[DicomTags.ReferencedRequestSequence].SetNullValue();
			dicomAttributeProvider[DicomTags.ContentDate].SetNullValue();
			dicomAttributeProvider[DicomTags.ContentTime].SetNullValue();
			dicomAttributeProvider[DicomTags.ConceptNameCodeSequence].SetNullValue();
		}

    	#endregion
    }

}
