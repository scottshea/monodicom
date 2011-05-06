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
    /// Patient Medical Module, as per Part 3, C.2.4
    /// </summary>
    public class PatientMedicalModule : IodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientMedicalModule"/> class.
        /// </summary>
        public PatientMedicalModule()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientMedicalModule"/> class.
        /// </summary>
        /// <param name="dicomAttributeCollection">The dicom attribute collection.</param>
        public PatientMedicalModule(DicomAttributeCollection dicomAttributeCollection)
            : base(dicomAttributeCollection)
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the medical alerts.
        /// </summary>
        /// <value>The medical alerts.</value>
        public string MedicalAlerts
        {
            get { return base.DicomAttributeProvider[DicomTags.MedicalAlerts].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.MedicalAlerts].SetString(0, value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the patient allergies
        /// </summary>
        /// <value>The allergies.</value>
        public string Allergies
        {
            get { return base.DicomAttributeProvider[DicomTags.Allergies].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.Allergies].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the smoking status.
        /// </summary>
        /// <value>The smoking status.</value>
        public SmokingStatus SmokingStatus
        {
            get { return IodBase.ParseEnum<SmokingStatus>(base.DicomAttributeProvider[DicomTags.SmokingStatus].GetString(0, String.Empty), SmokingStatus.Unknown); }
            set { IodBase.SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.SmokingStatus], value); }
        }


        /// <summary>
        /// Gets or sets the additional patient history.
        /// </summary>
        /// <value>The additional payment history.</value>
        public string AdditionalPatientHistory
        {
            get { return base.DicomAttributeProvider[DicomTags.AdditionalPatientHistory].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.AdditionalPatientHistory].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the responsible person role.
        /// </summary>
        /// <value>The responsible person role.</value>
        public PregnancyStatus PregnancyStatus
        {
            get { return IodBase.ParseEnum<PregnancyStatus>(base.DicomAttributeProvider[DicomTags.PregnancyStatus].GetString(0, String.Empty), PregnancyStatus.Unknown); }
            set { IodBase.SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.PregnancyStatus], value); }
        }


        /// <summary>
        /// Gets or sets the patients last Menstrual date (if applicable).
        /// </summary>
        /// <value>The patients last Menstrual date.</value>
        public DateTime? LastMenstrualDate
        {
            get
            {
                return DateTimeParser.ParseDateAndTime(String.Empty,
                          base.DicomAttributeProvider[DicomTags.LastMenstrualDate].GetString(0, String.Empty),
                base.DicomAttributeProvider[DicomTags.LastMenstrualDate].GetString(0, String.Empty));
            }

            set { DateTimeParser.SetDateTimeAttributeValues(value, base.DicomAttributeProvider[DicomTags.LastMenstrualDate], base.DicomAttributeProvider[DicomTags.LastMenstrualDate]); }
        }


        /// <summary>
        /// Gets or sets the Patient's Sex Neutered value.
        /// </summary>
        /// <value>The Patient's Sex Neutered value</value>
        public PatientsSexNeutered PatientsSexNeutered
        {
            get { return IodBase.ParseEnum<PatientsSexNeutered>(base.DicomAttributeProvider[DicomTags.PatientsSexNeutered].GetString(0, String.Empty), PatientsSexNeutered.Unaltered); }
            set { IodBase.SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.PatientsSexNeutered], value); }
        }

        /// <summary>
        /// Gets or sets the special needs field.
        /// </summary>
        /// <value>Special Needs.</value>
        public string SpecialNeeds
        {
            get { return base.DicomAttributeProvider[DicomTags.SpecialNeeds].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.SpecialNeeds].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the patient's state.
        /// </summary>
        /// <value>Patient's State.</value>
        public string PatientState
        {
            get { return base.DicomAttributeProvider[DicomTags.PatientState].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PatientState].SetString(0, value); }
        }

 
        //TODO: Patient's Pertinent Documents Sequence
        //TODO: Purposes of Code Reference Sequence
        

        /// <summary>
        /// Gets or sets the Document Title.
        /// </summary>
        /// <value>Title of Referenced Document.</value>
        public string DocumentTitle
        {
            get { return base.DicomAttributeProvider[DicomTags.DocumentTitle].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.DocumentTitle].SetString(0, value); }
        }

        //TODO: Patient Clinical Trials Participation Sequence.

        /// <summary>
        /// Gets or sets the Clinical Trial Sponsor Name.
        /// </summary>
        /// <value>The name of the clinical trial sponsor.</value>
        public string ClinicalTrialSponsorName
        {
            get { return base.DicomAttributeProvider[DicomTags.ClinicalTrialSponsorName].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.ClinicalTrialSponsorName].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the Document Title.
        /// </summary>
        /// <value>Identifier for the Noted Protocol.</value>
        public string ClinicalTrialProtocolId
        {
            get { return base.DicomAttributeProvider[DicomTags.ClinicalTrialProtocolId].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.ClinicalTrialProtocolId].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the Clinical Trial Protocol Name.
        /// </summary>
        /// <value>The name or title of the clinical trial protocol.</value>
        public string ClinicalTrialProtocolName
        {
            get { return base.DicomAttributeProvider[DicomTags.ClinicalTrialProtocolName].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.ClinicalTrialProtocolName].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the Clinical Trial Site Id.
        /// </summary>
        /// <value>Identifier of the clinical trial site</value>
        public string ClinicalTrialSiteId
        {
            get { return base.DicomAttributeProvider[DicomTags.ClinicalTrialSiteId].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.ClinicalTrialSiteId].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the Clinical Trial Site Name.
        /// </summary>
        /// <value>Clinical Trial Site Name.</value>
        public string ClinicalTrialSiteName
        {
            get { return base.DicomAttributeProvider[DicomTags.ClinicalTrialSiteName].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.ClinicalTrialSiteName].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the Clinical Trial Subject Id.
        /// </summary>
        /// <value>Clinical Trial Subject Id.</value>
        public string ClinicalTrialSubjectId
        {
            get { return base.DicomAttributeProvider[DicomTags.ClinicalTrialSubjectId].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.ClinicalTrialSubjectId].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the Clinical Trial Subject Reading Id.
        /// </summary>
        /// <value>Clinical Trial Subject Reading Id.</value>
        public string ClinicalTrialSubjectReadingId
        {
            get { return base.DicomAttributeProvider[DicomTags.ClinicalTrialSubjectReadingId].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.ClinicalTrialSubjectReadingId].SetString(0, value); }
        }

        #endregion

    }

    #region SmokingStatus Enum
    /// <summary>
    /// SmokingStatus Enumeration
    /// </summary>
    public enum SmokingStatus
    {
        /// <summary>
        /// Yes
        /// </summary>
        Yes,
        /// <summary>
        /// No
        /// </summary>
        No,
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown
        
    }
    #endregion


    #region PregnancyStatus Enum
    /// <summary>
    /// PregnancyStatus Enumeration
    /// </summary>
    public enum PregnancyStatus
    {
        /// <summary>
        /// Not Pregnant
        /// </summary>
        NotPregnant,
        /// <summary>
        /// Possibly Pregnant
        /// </summary>
        PossiblyPregnant,
        /// <summary>
        /// Definitely Pregnant
        /// </summary>
        DefinitelyPregnant,
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown

    }
    #endregion


}

