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
using ClearCanvas.Dicom.Iod.Sequences;
using ClearCanvas.Dicom.Utilities;

namespace ClearCanvas.Dicom.Iod.Modules
{
    /// <summary>
    /// Patient Identification Module, as per Part 3, C.4.13
    /// </summary>
    public class PerformedProcedureStepRelationshipModuleIod : IodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PerformedProcedureStepRelationshipModuleIod"/> class.
        /// </summary>
        public PerformedProcedureStepRelationshipModuleIod()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformedProcedureStepRelationshipModuleIod"/> class.
        /// </summary>
		public PerformedProcedureStepRelationshipModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider)
        {
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the patients.
        /// </summary>
        /// <value>The name of the patients.</value>
        public PersonName PatientsName
        {
            get { return new PersonName(base.DicomAttributeProvider[DicomTags.PatientsName].GetString(0, String.Empty)); }
            set { base.DicomAttributeProvider[DicomTags.PatientsName].SetString(0, value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the patient id.
        /// </summary>
        /// <value>The patient id.</value>
        public string PatientId
        {
            get { return base.DicomAttributeProvider[DicomTags.PatientId].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PatientId].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the issuer of patient id.
        /// </summary>
        /// <value>The issuer of patient id.</value>
        public string IssuerOfPatientId
        {
            get { return base.DicomAttributeProvider[DicomTags.IssuerOfPatientId].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.IssuerOfPatientId].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the patients birth date (only, no time).
        /// </summary>
        /// <value>The patients birth date.</value>
        public DateTime? PatientsBirthDate
        {
        	get { return DateTimeParser.ParseDateAndTime(base.DicomAttributeProvider, 0, DicomTags.PatientsBirthDate, 0);  }
        
            set { DateTimeParser.SetDateTimeAttributeValues(value, base.DicomAttributeProvider, 0, DicomTags.PatientsBirthDate, 0); }
        }

        /// <summary>
        /// Gets or sets the patients sex.
        /// </summary>
        /// <value>The patients sex.</value>
        public PatientsSex PatientsSex
        {
            get { return IodBase.ParseEnum<PatientsSex>(base.DicomAttributeProvider[DicomTags.PatientsSex].GetString(0, String.Empty), PatientsSex.None); }
            set { IodBase.SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.PatientsSex], value); }
        }

        /// <summary>
        /// Gets the referenced patient sequence list.
        /// </summary>
        /// <value>The referenced patient sequence list.</value>
        public SequenceIodList<ReferencedInstanceSequenceIod> ReferencedPatientSequenceList
        {
            get
            {
                return new SequenceIodList<ReferencedInstanceSequenceIod>(base.DicomAttributeProvider[DicomTags.ReferencedPatientSequence] as DicomAttributeSQ);
            }
        }

        /// <summary>
        /// Gets the scheduled step attributes sequence list.
        /// </summary>
        /// <value>The scheduled step attributes sequence list.</value>
        public SequenceIodList<ScheduledStepAttributesSequenceIod> ScheduledStepAttributesSequenceList
        {
            get
            {
                return new SequenceIodList<ScheduledStepAttributesSequenceIod>(base.DicomAttributeProvider[DicomTags.ScheduledStepAttributesSequence] as DicomAttributeSQ);
            }
        }

        #endregion

    }

    
}
