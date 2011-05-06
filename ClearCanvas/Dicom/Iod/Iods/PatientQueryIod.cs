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
    /// IOD for common Patient Query Retrieve items.
    /// </summary>
    public class PatientQueryIod : QueryIodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientQueryIod"/> class.
        /// </summary>
        public PatientQueryIod()
        {
            SetAttributeFromEnum(DicomAttributeProvider[DicomTags.QueryRetrieveLevel], QueryRetrieveLevel.Patient);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientQueryIod"/> class.
        /// </summary>
		public PatientQueryIod(IDicomAttributeProvider dicomAttributeProvider)
            :base(dicomAttributeProvider)
        {
            SetAttributeFromEnum(DicomAttributeProvider[DicomTags.QueryRetrieveLevel], QueryRetrieveLevel.Patient);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the patient id.
        /// </summary>
        /// <value>The patient id.</value>
        public string PatientId
        {
            get { return DicomAttributeProvider[DicomTags.PatientId].GetString(0, String.Empty); }
            set { DicomAttributeProvider[DicomTags.PatientId].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the name of the patient.
        /// </summary>
        /// <value>The name of the patients.</value>
        public PersonName PatientsName
        {
            get { return new PersonName(DicomAttributeProvider[DicomTags.PatientsName].GetString(0, String.Empty)); }
            set { DicomAttributeProvider[DicomTags.PatientsName].SetString(0, value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the patients birth date.
        /// </summary>
        /// <value>The patients birth date.</value>
        public DateTime PatientsBirthDate
        {
            get { return DicomAttributeProvider[DicomTags.PatientsBirthDate].GetDateTime(0, DateTime.MinValue); }
            set { DicomAttributeProvider[DicomTags.PatientsBirthDate].SetDateTime(0, value); }
        }

        /// <summary>
        /// Gets or sets the patients sex.
        /// </summary>
        /// <value>The patients sex.</value>
        public string PatientsSex
        {
            get { return DicomAttributeProvider[DicomTags.PatientsSex].GetString(0, String.Empty); }
            set { DicomAttributeProvider[DicomTags.PatientsSex].SetString(0, value); }
        }

		/// <summary>
		/// Gets or sets the number of patient related instances.
		/// </summary>
		/// <value>The number of patient related instances.</value>
		public uint NumberOfPatientRelatedInstances
		{
			get { return DicomAttributeProvider[DicomTags.NumberOfPatientRelatedInstances].GetUInt32(0, 0); }
			set { DicomAttributeProvider[DicomTags.NumberOfPatientRelatedInstances].SetUInt32(0, value); }
		}

		/// <summary>
		/// Gets or sets the number of patient related series.
		/// </summary>
		/// <value>The number of patient related series.</value>
		public uint NumberOfPatientRelatedSeries
		{
			get { return DicomAttributeProvider[DicomTags.NumberOfPatientRelatedSeries].GetUInt32(0, 0); }
			set { DicomAttributeProvider[DicomTags.NumberOfPatientRelatedSeries].SetUInt32(0, value); }
		}

		/// <summary>
		/// Gets or sets the number of patient related studies.
		/// </summary>
		/// <value>The number of patient related studies.</value>
		public uint NumberOfPatientRelatedStudies
		{
			get { return DicomAttributeProvider[DicomTags.NumberOfPatientRelatedStudies].GetUInt32(0, 0); }
			set { DicomAttributeProvider[DicomTags.NumberOfPatientRelatedStudies].SetUInt32(0, value); }
		}

        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the common tags for a patient query retrieve request.
        /// </summary>
         public void SetCommonTags()
        {
            SetCommonTags(DicomAttributeProvider);
        }

        /// <summary>
        /// Sets the common tags for a patient query retrieve request.
        /// </summary>
        public static void SetCommonTags(IDicomAttributeProvider dicomAttributeProvider)
        {
			SetAttributeFromEnum(dicomAttributeProvider[DicomTags.QueryRetrieveLevel], QueryRetrieveLevel.Patient);

			// Always set the Patient 
			dicomAttributeProvider[DicomTags.PatientsName].SetString(0, "*");
			dicomAttributeProvider[DicomTags.PatientId].SetNullValue();
			dicomAttributeProvider[DicomTags.PatientsBirthDate].SetNullValue();
			dicomAttributeProvider[DicomTags.PatientsBirthTime].SetNullValue();
			dicomAttributeProvider[DicomTags.PatientsSex].SetNullValue();
			dicomAttributeProvider[DicomTags.NumberOfPatientRelatedStudies].SetNullValue();
			dicomAttributeProvider[DicomTags.NumberOfPatientRelatedSeries].SetNullValue();
			dicomAttributeProvider[DicomTags.NumberOfPatientRelatedInstances].SetNullValue();
		}
        #endregion
    }

}
