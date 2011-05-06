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
using ClearCanvas.Dicom.Iod.Macros;
using ClearCanvas.Dicom.Utilities;

namespace ClearCanvas.Dicom.Iod.Modules
{
    /// <summary>
    /// Patient Identification Module, as per Part 3, C.4.14 (pg 255)
    /// </summary>
    public class PerformedProcedureStepInformationModuleIod : IodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PerformedProcedureStepInformationModuleIod"/> class.
        /// </summary>
        public PerformedProcedureStepInformationModuleIod()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformedProcedureStepInformationModuleIod"/> class.
        /// </summary>
		public PerformedProcedureStepInformationModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider)
        {
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the performed station ae title.
        /// </summary>
        /// <value>The performed station ae title.</value>
        public string PerformedStationAeTitle
        {
            get { return base.DicomAttributeProvider[DicomTags.PerformedStationAeTitle].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PerformedStationAeTitle].SetString(0, value); }
        }

        public string PerformedStationName
        {
            get { return base.DicomAttributeProvider[DicomTags.PerformedStationName].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PerformedStationName].SetString(0, value); }
        }

        public string PerformedLocation
        {
            get { return base.DicomAttributeProvider[DicomTags.PerformedLocation].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PerformedLocation].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the performed procedure step start date.
        /// </summary>
        /// <value>The performed procedure step start date.</value>
        public DateTime? PerformedProcedureStepStartDate
        {
            get { return DateTimeParser.ParseDateAndTime(base.DicomAttributeProvider, 0, DicomTags.PerformedProcedureStepStartDate, DicomTags.PerformedProcedureStepStartTime); }

            set { DateTimeParser.SetDateTimeAttributeValues(value, base.DicomAttributeProvider, 0, DicomTags.PerformedProcedureStepStartDate, DicomTags.PerformedProcedureStepStartTime); }
        }

        /// <summary>
        /// Gets or sets the performed procedure step id.
        /// </summary>
        /// <value>The performed procedure step id.</value>
        public string PerformedProcedureStepId
        {
            get { return base.DicomAttributeProvider[DicomTags.PerformedProcedureStepId].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PerformedProcedureStepId].SetString(0, value); }
        }

        public DateTime? PerformedProcedureStepEndDate
        {
            get { return DateTimeParser.ParseDateAndTime(base.DicomAttributeProvider, 0, DicomTags.PerformedProcedureStepEndDate, DicomTags.PerformedProcedureStepEndTime); }
        
            set { DateTimeParser.SetDateTimeAttributeValues(value, base.DicomAttributeProvider, 0, DicomTags.PerformedProcedureStepEndDate, DicomTags.PerformedProcedureStepEndTime); }
        }

        /// <summary>
        /// Gets or sets the performed procedure step status.
        /// </summary>
        /// <value>The performed procedure step status.</value>
        public PerformedProcedureStepStatus PerformedProcedureStepStatus
        {
            get { return IodBase.ParseEnum<PerformedProcedureStepStatus>(base.DicomAttributeProvider[DicomTags.PerformedProcedureStepStatus].GetString(0, String.Empty), PerformedProcedureStepStatus.None); }
            set { IodBase.SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.PerformedProcedureStepStatus], value, true); }
        }

        /// <summary>
        /// Gets or sets the performed procedure step description.
        /// </summary>
        /// <value>The performed procedure step description.</value>
        public string PerformedProcedureStepDescription
        {
            get { return base.DicomAttributeProvider[DicomTags.PerformedProcedureStepDescription].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PerformedProcedureStepDescription].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the comments on the performed procedure step.
        /// </summary>
        /// <value>The comments on the performed procedure step.</value>
        public string CommentsOnThePerformedProcedureStep
        {
            get { return base.DicomAttributeProvider[DicomTags.CommentsOnThePerformedProcedureStep].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.CommentsOnThePerformedProcedureStep].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the performed procedure type description.
        /// </summary>
        /// <value>The performed procedure type description.</value>
        public string PerformedProcedureTypeDescription
        {
            get { return base.DicomAttributeProvider[DicomTags.PerformedProcedureTypeDescription].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PerformedProcedureTypeDescription].SetString(0, value); }
        }

        /// <summary>
        /// Gets the procedure code sequence list.
        /// </summary>
        /// <value>The procedure code sequence list.</value>
        public SequenceIodList<CodeSequenceMacro> ProcedureCodeSequenceList
        {
            get
            {
                return new SequenceIodList<CodeSequenceMacro>(base.DicomAttributeProvider[DicomTags.ProcedureCodeSequence] as DicomAttributeSQ);
            }
        }

        /// <summary>
        /// Gets the performed procedure step discontinuation reason code sequence list.
        /// </summary>
        /// <value>
        /// The performed procedure step discontinuation reason code sequence list.
        /// </value>
        public SequenceIodList<CodeSequenceMacro> PerformedProcedureStepDiscontinuationReasonCodeSequenceList
        {
            get
            {
                return new SequenceIodList<CodeSequenceMacro>(base.DicomAttributeProvider[DicomTags.PerformedProcedureStepDiscontinuationReasonCodeSequence] as DicomAttributeSQ);
            }
        }
        

       
        #endregion

    }

    #region PerformedProcedureStepStatus Enum
    /// <summary>
    /// Enumeration for PerformedProcedureStepStatus
    /// </summary>
    public enum PerformedProcedureStepStatus
    {
        /// <summary>
        /// None, or blank value
        /// </summary>
        None,
        /// <summary>
        /// Started but not complete
        /// </summary>
        InProgress,
        /// <summary>
        /// Canceled or unsuccessfully terminated
        /// </summary>
        Discontinued,
        /// <summary>
        /// Successfully completed
        /// </summary>
        Completed
    }
    #endregion
    
    
}
