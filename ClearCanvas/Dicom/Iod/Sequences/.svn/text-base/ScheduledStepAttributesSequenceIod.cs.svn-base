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

namespace ClearCanvas.Dicom.Iod.Sequences
{
    /// <summary>
    /// Scheduled Step Attributes Sequence (0040,0270)
    /// </summary>
    /// <remarks>As per Dicom Doc 3, C.4-13 (pg 253)</remarks>
    public class ScheduledStepAttributesSequenceIod : SequenceIodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledStepAttributesSequuenceIod"/> class.
        /// </summary>
        public ScheduledStepAttributesSequenceIod()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduledStepAttributesSequuenceIod"/> class.
        /// </summary>
        /// <param name="dicomSequenceItem">The dicom sequence item.</param>
        public ScheduledStepAttributesSequenceIod(DicomSequenceItem dicomSequenceItem)
            : base(dicomSequenceItem)
        {
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the study instance uid.
        /// </summary>
        /// <value>The study instance uid.</value>
        public string StudyInstanceUid
        {
            get { return base.DicomAttributeProvider[DicomTags.StudyInstanceUid].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.StudyInstanceUid].SetString(0, value); }
        }

        //TODO: Referenced Study Sequence

        /// <summary>
        /// Gets or sets the accession number.
        /// </summary>
        /// <value>The accession number.</value>
        public string AccessionNumber
        {
            get { return base.DicomAttributeProvider[DicomTags.AccessionNumber].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.AccessionNumber].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the placer order number imaging service request.
        /// </summary>
        /// <value>The placer order number imaging service request.</value>
        public string PlacerOrderNumberImagingServiceRequest
        {
            get { return base.DicomAttributeProvider[DicomTags.PlacerOrderNumberImagingServiceRequest].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PlacerOrderNumberImagingServiceRequest].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the filler order number imaging service request.
        /// </summary>
        /// <value>The filler order number imaging service request.</value>
        public string FillerOrderNumberImagingServiceRequest
        {
            get { return base.DicomAttributeProvider[DicomTags.FillerOrderNumberImagingServiceRequest].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.FillerOrderNumberImagingServiceRequest].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the requested procedure id.
        /// </summary>
        /// <value>The requested procedure id.</value>
        public string RequestedProcedureId
        {
            get { return base.DicomAttributeProvider[DicomTags.RequestedProcedureId].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.RequestedProcedureId].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the requested procedure description.
        /// </summary>
        /// <value>The requested procedure description.</value>
        public string RequestedProcedureDescription
        {
            get { return base.DicomAttributeProvider[DicomTags.RequestedProcedureDescription].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.RequestedProcedureDescription].SetString(0, value); }
        }

        // TODO: Requested Procedure Code Sequence

        /// <summary>
        /// Gets or sets the scheduled procedure step id.
        /// </summary>
        /// <value>The scheduled procedure step id.</value>
        public string ScheduledProcedureStepId
        {
            get { return base.DicomAttributeProvider[DicomTags.ScheduledProcedureStepId].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.ScheduledProcedureStepId].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the scheduled procedure step description.
        /// </summary>
        /// <value>The scheduled procedure step description.</value>
        public string ScheduledProcedureStepDescription
        {
            get { return base.DicomAttributeProvider[DicomTags.ScheduledProcedureStepDescription].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.ScheduledProcedureStepDescription].SetString(0, value); }
        }

        //TODO: >Scheduled Protocol Code Sequence
        
        #endregion

    }


}
