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

namespace ClearCanvas.Dicom.Iod.Iods
{
    /// <summary>
    /// IOD for common Series Query Retrieve items.
    /// </summary>
    public class SeriesQueryIod : QueryIodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesQueryIod"/> class.
        /// </summary>
        public SeriesQueryIod()
        {
            SetAttributeFromEnum(DicomAttributeProvider[DicomTags.QueryRetrieveLevel], QueryRetrieveLevel.Series);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesQueryIod"/> class.
        /// </summary>
		public SeriesQueryIod(IDicomAttributeProvider dicomAttributeProvider)
			: base(dicomAttributeProvider)
        {
            SetAttributeFromEnum(DicomAttributeProvider[DicomTags.QueryRetrieveLevel], QueryRetrieveLevel.Series);
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
		/// Gets or sets the modality.
		/// </summary>
		/// <value>The modality.</value>
		public string Modality
		{
			get { return DicomAttributeProvider[DicomTags.Modality].GetString(0, String.Empty); }
			set { DicomAttributeProvider[DicomTags.Modality].SetString(0, value); }
		}

		/// <summary>
        /// Gets or sets the series description.
        /// </summary>
        /// <value>The series description.</value>
        public string SeriesDescription
        {
            get { return DicomAttributeProvider[DicomTags.SeriesDescription].GetString(0, String.Empty); }
            set { DicomAttributeProvider[DicomTags.SeriesDescription].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the series number.
        /// </summary>
        /// <value>The series number.</value>
        public string SeriesNumber
        {
            get { return DicomAttributeProvider[DicomTags.SeriesNumber].GetString(0, String.Empty); }
            set { DicomAttributeProvider[DicomTags.SeriesNumber].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the number of series related instances.
        /// </summary>
        /// <value>The number of series related instances.</value>
        public uint NumberOfSeriesRelatedInstances
        {
            get { return DicomAttributeProvider[DicomTags.NumberOfSeriesRelatedInstances].GetUInt32(0, 0); }
            set { DicomAttributeProvider[DicomTags.NumberOfSeriesRelatedInstances].SetUInt32(0, value); }
        }

        /// <summary>
        /// Gets or sets the series date.
        /// </summary>
        /// <value>The series date.</value>
        public DateTime? SeriesDate
        {
            get { return DateTimeParser.ParseDateAndTime(String.Empty, 
                    DicomAttributeProvider[DicomTags.SeriesDate].GetString(0, String.Empty), 
                    DicomAttributeProvider[DicomTags.SeriesTime].GetString(0, String.Empty)); }

            set { DateTimeParser.SetDateTimeAttributeValues(value, DicomAttributeProvider[DicomTags.SeriesDate], DicomAttributeProvider[DicomTags.SeriesTime]); }
        }

        /// <summary>
        /// Gets or sets the performed procedure step start date.
        /// </summary>
        /// <value>The performed procedure step start date.</value>
        public DateTime? PerformedProcedureStepStartDate
        {
            get { return DateTimeParser.ParseDateAndTime(String.Empty, 
                    DicomAttributeProvider[DicomTags.PerformedProcedureStepStartDate].GetString(0, String.Empty), 
                    DicomAttributeProvider[DicomTags.PerformedProcedureStepStartTime].GetString(0, String.Empty)); }

            set { DateTimeParser.SetDateTimeAttributeValues(value, DicomAttributeProvider[DicomTags.PerformedProcedureStepStartDate], DicomAttributeProvider[DicomTags.PerformedProcedureStepStartTime]); }
        }

		/// <summary>
		/// Gets the request attributes sequence list.
		/// </summary>
		/// <value>The request attributes sequence list.</value>
		public SequenceIodList<RequestAttributesSequenceIod> RequestAttributesSequence
		{
			get
			{
				return new SequenceIodList<RequestAttributesSequenceIod>(DicomAttributeProvider[DicomTags.RequestAttributesSequence] as DicomAttributeSQ);
			}
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
			SetAttributeFromEnum(dicomAttributeProvider[DicomTags.QueryRetrieveLevel], QueryRetrieveLevel.Series);

			dicomAttributeProvider[DicomTags.SeriesInstanceUid].SetNullValue();
			dicomAttributeProvider[DicomTags.Modality].SetNullValue();
			dicomAttributeProvider[DicomTags.SeriesDescription].SetNullValue();
			dicomAttributeProvider[DicomTags.NumberOfSeriesRelatedInstances].SetNullValue();
			dicomAttributeProvider[DicomTags.SeriesNumber].SetNullValue();
			dicomAttributeProvider[DicomTags.SeriesDate].SetNullValue();
			dicomAttributeProvider[DicomTags.SeriesTime].SetNullValue();
			dicomAttributeProvider[DicomTags.RequestAttributesSequence].SetNullValue();
			dicomAttributeProvider[DicomTags.PerformedProcedureStepStartDate].SetNullValue();
			dicomAttributeProvider[DicomTags.PerformedProcedureStepStartTime].SetNullValue();
        }
        #endregion
    }

}
