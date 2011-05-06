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
    /// Patient Identification Module, as per Part 3, C.2.2
    /// </summary>
    public class PatientIdentificationModuleIod : IodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientIdentificationModuleIod"/> class.
        /// </summary>
        public PatientIdentificationModuleIod()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientIdentificationModuleIod"/> class.
        /// </summary>
		public PatientIdentificationModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider)
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the name of the patient.
        /// </summary>
        /// <value>The name of the patient.</value>
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

        //TODO: Other Patient IDs
        //TODO: Other Patient IDs Sequence
        //TODO: Other Patient Names 

        /// <summary>
        /// Gets or sets the name of the patients birth.
        /// </summary>
        /// <value>The name of the patients birth.</value>
        public PersonName PatientsBirthName
        {
            get { return new PersonName(base.DicomAttributeProvider[DicomTags.PatientsBirthName].GetString(0, String.Empty)); }
            set { base.DicomAttributeProvider[DicomTags.PatientsBirthName].SetString(0, value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the birth name of the patient's mother.
        /// </summary>
        /// <value>The birth name of the patient's mother.</value>
        public PersonName PatientsMothersBirthName
        {
            get { return new PersonName(base.DicomAttributeProvider[DicomTags.PatientsMothersBirthName].GetString(0, String.Empty)); }
            set { base.DicomAttributeProvider[DicomTags.PatientsMothersBirthName].SetString(0, value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the medical record locator.
        /// </summary>
        /// <value>The medical record locator.</value>
        public string MedicalRecordLocator
        {
            get { return base.DicomAttributeProvider[DicomTags.MedicalRecordLocator].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.MedicalRecordLocator].SetString(0, value); }
        }

        //TODO: Patient's Age

        /// <summary>
        /// Gets or sets the occupation.
        /// </summary>
        /// <value>The occupation.</value>
        public string Occupation
        {
            get { return base.DicomAttributeProvider[DicomTags.Occupation].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.Occupation].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the confidentiality constraint on patient data description.
        /// </summary>
        /// <value>The confidentiality constraint on patient data description.</value>
        public string ConfidentialityConstraintOnPatientDataDescription
        {
            get { return base.DicomAttributeProvider[DicomTags.ConfidentialityConstraintOnPatientDataDescription].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.ConfidentialityConstraintOnPatientDataDescription].SetString(0, value); }
        }
        

        /// <summary>
        /// Gets or sets the patients birth date.
        /// </summary>
        /// <value>The patients birth date.</value>
        public DateTime? PatientsBirthDate
        {
        	get { return DateTimeParser.ParseDateAndTime(String.Empty, 
        					base.DicomAttributeProvider[DicomTags.PatientsBirthDate].GetString(0, String.Empty), 
                  base.DicomAttributeProvider[DicomTags.PatientsBirthTime].GetString(0, String.Empty)); }

            set { DateTimeParser.SetDateTimeAttributeValues(value, base.DicomAttributeProvider[DicomTags.PatientsBirthDate], base.DicomAttributeProvider[DicomTags.PatientsBirthTime]); }
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
        
        //TODO: Patient's Insurance Plan Code Sequence
        //TODO: Patient�s Primary Language Code Sequence
        //TODO: Patient�s Primary Language Code Modifier Sequence

        /// <summary>
        /// Gets or sets the size of the patients (in meters)
        /// </summary>
        /// <value>The size of the patients.</value>
        public float PatientsSize
        {
            get { return base.DicomAttributeProvider[DicomTags.PatientsSize].GetFloat32(0, 0.0F); }
            set { base.DicomAttributeProvider[DicomTags.PatientsSize].SetFloat32(0, value); }
        }

        /// <summary>
        /// Gets or sets the patients weight (in KG)
        /// </summary>
        /// <value>The patients weight.</value>
        public float PatientsWeight
        {
            get { return base.DicomAttributeProvider[DicomTags.PatientsWeight].GetFloat32(0, 0.0F); }
            set { base.DicomAttributeProvider[DicomTags.PatientsWeight].SetFloat32(0, value); }
        }

        /// <summary>
        /// Gets or sets the patients address.
        /// </summary>
        /// <value>The patients address.</value>
        public string PatientsAddress
        {
            get { return base.DicomAttributeProvider[DicomTags.PatientsAddress].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PatientsAddress].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the military rank.
        /// </summary>
        /// <value>The military rank.</value>
        public string MilitaryRank
        {
            get { return base.DicomAttributeProvider[DicomTags.MilitaryRank].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.MilitaryRank].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the branch of service.
        /// </summary>
        /// <value>The branch of service.</value>
        public string BranchOfService
        {
            get { return base.DicomAttributeProvider[DicomTags.BranchOfService].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.BranchOfService].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the country of residence.
        /// </summary>
        /// <value>The country of residence.</value>
        public string CountryOfResidence
        {
            get { return base.DicomAttributeProvider[DicomTags.CountryOfResidence].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.CountryOfResidence].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the region of residence.
        /// </summary>
        /// <value>The region of residence.</value>
        public string RegionOfResidence
        {
            get { return base.DicomAttributeProvider[DicomTags.RegionOfResidence].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.RegionOfResidence].SetString(0, value); }
        }


        /// <summary>
        /// Gets or sets the patients telephone number. TODO: Way to specify more than 1...
        /// </summary>
        /// <value>The patients telephone numbers.</value>
        public string PatientsTelephoneNumbers
        {
            get { return base.DicomAttributeProvider[DicomTags.PatientsTelephoneNumbers].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PatientsTelephoneNumbers].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the ethnic group.
        /// </summary>
        /// <value>The ethnic group.</value>
        public string EthnicGroup
        {
            get { return base.DicomAttributeProvider[DicomTags.EthnicGroup].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.EthnicGroup].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the patients religious preference.
        /// </summary>
        /// <value>The patients religious preference.</value>
        public string PatientsReligiousPreference
        {
            get { return base.DicomAttributeProvider[DicomTags.PatientsReligiousPreference].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PatientsReligiousPreference].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the patient comments.
        /// </summary>
        /// <value>The patient comments.</value>
        public string PatientComments
        {
            get { return base.DicomAttributeProvider[DicomTags.PatientComments].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PatientComments].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the responsible person.
        /// </summary>
        /// <value>The responsible person.</value>
        public PersonName ResponsiblePerson
        {
            get { return new PersonName(base.DicomAttributeProvider[DicomTags.ResponsiblePerson].GetString(0, String.Empty)); }
            set { base.DicomAttributeProvider[DicomTags.ResponsiblePerson].SetString(0, value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the responsible person role.
        /// </summary>
        /// <value>The responsible person role.</value>
        public ResponsiblePersonRole ResponsiblePersonRole
        {
            get { return IodBase.ParseEnum<ResponsiblePersonRole>(base.DicomAttributeProvider[DicomTags.ResponsiblePersonRole].GetString(0, String.Empty), ResponsiblePersonRole.None); }
            set { IodBase.SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.ResponsiblePersonRole], value); }
        }

        /// <summary>
        /// Gets or sets the responsible organization.
        /// </summary>
        /// <value>The responsible organization.</value>
        public string ResponsibleOrganization
        {
            get { return base.DicomAttributeProvider[DicomTags.ResponsibleOrganization].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.ResponsibleOrganization].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the patient species description.
        /// </summary>
        /// <value>The patient species description.</value>
        public string PatientSpeciesDescription
        {
            get { return base.DicomAttributeProvider[DicomTags.PatientSpeciesDescription].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PatientSpeciesDescription].SetString(0, value); }
        }

        //TODO: Patient Species Code Sequence

        /// <summary>
        /// Gets or sets the patient breed description.
        /// </summary>
        /// <value>The patient breed description.</value>
        public string PatientBreedDescription
        {
            get { return base.DicomAttributeProvider[DicomTags.PatientBreedDescription].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PatientBreedDescription].SetString(0, value); }
        }
        
        //TODO: Patient Breed Code Sequence

        //TODO: Patient Breed Registration Sequence
        #endregion

    }    
}
