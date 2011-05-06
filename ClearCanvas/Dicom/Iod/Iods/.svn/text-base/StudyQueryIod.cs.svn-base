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
using ClearCanvas.Common;

namespace ClearCanvas.Dicom.Iod.Iods
{
    /// <summary>
    /// IOD for common Query Retrieve items.  This is a replacement for the <see cref="ClearCanvas.Dicom.QueryResult"/>
    /// </summary>
    public class StudyQueryIod : QueryIodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="StudyQueryIod"/> class.
        /// </summary>
        public StudyQueryIod()
            :base()
        {
            SetAttributeFromEnum(DicomAttributeProvider[DicomTags.QueryRetrieveLevel], QueryRetrieveLevel.Study);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StudyQueryIod"/> class.
        /// </summary>
		public StudyQueryIod(IDicomAttributeProvider dicomAttributeProvider)
			: base(dicomAttributeProvider)
        {
            SetAttributeFromEnum(DicomAttributeProvider[DicomTags.QueryRetrieveLevel], QueryRetrieveLevel.Study);
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
        /// Gets or sets the modalities in study.
        /// </summary>
        /// <value>The modalities in study.</value>
        public string ModalitiesInStudy
        {
            get { return DicomAttributeProvider[DicomTags.ModalitiesInStudy].GetString(0, String.Empty); }
            set { DicomAttributeProvider[DicomTags.ModalitiesInStudy].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the study description.
        /// </summary>
        /// <value>The study description.</value>
        public string StudyDescription
        {
            get { return DicomAttributeProvider[DicomTags.StudyDescription].GetString(0, String.Empty); }
            set { DicomAttributeProvider[DicomTags.StudyDescription].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the study id.
        /// </summary>
        /// <value>The study id.</value>
        public string StudyId
        {
            get { return DicomAttributeProvider[DicomTags.StudyId].GetString(0, String.Empty); }
            set { DicomAttributeProvider[DicomTags.StudyId].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the study date.
        /// </summary>
        /// <value>The study date.</value>
        public DateTime? StudyDate
        {
            get { return DateTimeParser.ParseDateAndTime(String.Empty, 
                    DicomAttributeProvider[DicomTags.StudyDate].GetString(0, String.Empty), 
                    DicomAttributeProvider[DicomTags.StudyTime].GetString(0, String.Empty)); }

            set { DateTimeParser.SetDateTimeAttributeValues(value, DicomAttributeProvider[DicomTags.StudyDate], DicomAttributeProvider[DicomTags.StudyTime]); }
        }

        /// <summary>
        /// Gets or sets the accession number.
        /// </summary>
        /// <value>The accession number.</value>
        public string AccessionNumber
        {
            get { return DicomAttributeProvider[DicomTags.AccessionNumber].GetString(0, String.Empty); }
            set { DicomAttributeProvider[DicomTags.AccessionNumber].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the number of study related instances.
        /// </summary>
        /// <value>The number of study related instances.</value>
        public uint NumberOfStudyRelatedInstances
        {
            get { return DicomAttributeProvider[DicomTags.NumberOfStudyRelatedInstances].GetUInt32(0, 0); }
            set { DicomAttributeProvider[DicomTags.NumberOfStudyRelatedInstances].SetUInt32(0, value); }
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
			Platform.CheckForNullReference(dicomAttributeProvider, "dicomAttributeProvider");

			PatientQueryIod.SetCommonTags(dicomAttributeProvider);

			SetAttributeFromEnum(dicomAttributeProvider[DicomTags.QueryRetrieveLevel], QueryRetrieveLevel.Study);

			dicomAttributeProvider[DicomTags.StudyInstanceUid].SetNullValue();
			dicomAttributeProvider[DicomTags.StudyId].SetNullValue();
			dicomAttributeProvider[DicomTags.StudyDate].SetNullValue();
			dicomAttributeProvider[DicomTags.StudyTime].SetNullValue();
			dicomAttributeProvider[DicomTags.StudyDescription].SetNullValue();
			dicomAttributeProvider[DicomTags.AccessionNumber].SetNullValue();
			dicomAttributeProvider[DicomTags.NumberOfStudyRelatedInstances].SetNullValue();
			dicomAttributeProvider[DicomTags.NumberOfStudyRelatedSeries].SetNullValue();
			dicomAttributeProvider[DicomTags.ModalitiesInStudy].SetNullValue();
			dicomAttributeProvider[DicomTags.RequestingPhysician].SetNullValue();
			dicomAttributeProvider[DicomTags.ReferringPhysiciansName].SetNullValue();
        }
        #endregion
    }

}
