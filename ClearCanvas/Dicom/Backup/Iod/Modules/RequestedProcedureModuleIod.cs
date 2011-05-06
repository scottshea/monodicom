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
using ClearCanvas.Dicom.Utilities;

namespace ClearCanvas.Dicom.Iod.Modules
{
    /// <summary>
    /// As per Dicom Doc 3, Table C.4-11 (pg 248)
    /// </summary>
    public class RequestedProcedureModuleIod : IodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the PatientModule class.
        /// </summary>
        public RequestedProcedureModuleIod()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Iod class.
        /// </summary>
        /// <param name="_dicomAttributeCollection"></param>
		public RequestedProcedureModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider)
        {
        }
        #endregion

        #region Public Properties
        public string RequestedProcedureId
        {
            get { return base.DicomAttributeProvider[DicomTags.RequestedProcedureId].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.RequestedProcedureId].SetString(0, value); }
        }
        public string ReasonForTheRequestedProcedure
        {
            get { return base.DicomAttributeProvider[DicomTags.ReasonForTheRequestedProcedure].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.ReasonForTheRequestedProcedure].SetString(0, value); }
        }

        public string RequestedProcedureComments
        {
            get { return base.DicomAttributeProvider[DicomTags.RequestedProcedureComments].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.RequestedProcedureComments].SetString(0, value); }
        }

        public string StudyInstanceUid
        {
            get { return base.DicomAttributeProvider[DicomTags.StudyInstanceUid].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.StudyInstanceUid].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the study date.
        /// </summary>
        /// <value>The study date.</value>
        public DateTime? StudyDate
        {
            get
            {
                return DateTimeParser.ParseDateAndTime(String.Empty,
                  base.DicomAttributeProvider[DicomTags.StudyDate].GetString(0, String.Empty),
                  base.DicomAttributeProvider[DicomTags.StudyTime].GetString(0, String.Empty));
            }

            set { DateTimeParser.SetDateTimeAttributeValues(value, base.DicomAttributeProvider[DicomTags.StudyDate], base.DicomAttributeProvider[DicomTags.StudyTime]); }
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

        // TODO: make one with the RequestedProcedurePriority enum
        public RequestedProcedurePriority RequestedProcedurePriority
        {
            get { return IodBase.ParseEnum<RequestedProcedurePriority>(base.DicomAttributeProvider[DicomTags.RequestedProcedurePriority].GetString(0, String.Empty), RequestedProcedurePriority.None); }
            set 
            {
                string stringValue = value == RequestedProcedurePriority.None ? String.Empty : value.ToString().ToUpperInvariant();
                base.DicomAttributeProvider[DicomTags.RequestedProcedurePriority].SetString(0, stringValue); 
            }
        }
        
        #endregion

    }

    /// <summary>
    /// 
    /// </summary>
    public enum RequestedProcedurePriority
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 
        /// </summary>
        Stat,
        /// <summary>
        /// 
        /// </summary>
        High,
        /// <summary>
        /// 
        /// </summary>
        Routine,
        /// <summary>
        /// 
        /// </summary>
        Medium,
        /// <summary>
        /// 
        /// </summary>
        Low
    }
}
