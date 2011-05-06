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

using System.Runtime.Serialization;
using ClearCanvas.Dicom.Iod;

namespace ClearCanvas.Dicom.ServiceModel.Query
{
	//NOTE: internal for now because we don't actually implement IPatientRootQuery anywhere.

	internal interface IPatientRootPatientIdentifier : IPatientRootData, IIdentifier
	{ }

	[DataContract(Namespace = QueryNamespace.Value)]
	internal class PatientRootPatientIdentifier : Identifier, IPatientRootData
	{
		#region Private Fields

		private string _patientId;
		private string _patientsName;
		private string _patientsBirthDate;
		private string _patientsBirthTime;
		private string _patientsSex;
		private int? _numberOfPatientRelatedStudies;
		private int? _numberOfPatientRelatedSeries;
		private int? _numberOfPatientRelatedInstances;

		#endregion

		#region Public Constructors

		public PatientRootPatientIdentifier()
		{
		}

		public PatientRootPatientIdentifier(IPatientRootPatientIdentifier other)
			: base(other)
		{
			CopyFrom(other);
		}

		public PatientRootPatientIdentifier(IPatientRootData other, IIdentifier identifier)
			: base(identifier)
		{
			CopyFrom(other);
		}

		public PatientRootPatientIdentifier(IPatientRootData other)
		{
			CopyFrom(other);
		}

		private void CopyFrom(IPatientRootData other)
		{
			PatientId = other.PatientId;
			PatientsName = other.PatientsName;
			PatientsBirthDate = other.PatientsBirthDate;
			PatientsBirthTime = other.PatientsBirthTime;
			PatientsSex = other.PatientsSex;
			NumberOfPatientRelatedStudies = other.NumberOfPatientRelatedStudies;
			NumberOfPatientRelatedSeries = other.NumberOfPatientRelatedSeries;
			NumberOfPatientRelatedInstances = other.NumberOfPatientRelatedInstances;
		}

		public PatientRootPatientIdentifier(DicomAttributeCollection attributes)
			: base(attributes)
		{
		}

		#endregion

		#region Public Properties

		public override string QueryRetrieveLevel
		{
			get { return "PATIENT"; }
		}

		[DicomField(DicomTags.PatientId, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string PatientId
		{
			get { return _patientId; }
			set { _patientId = value; }
		}

		[DicomField(DicomTags.PatientsName, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string PatientsName
		{
			get { return _patientsName; }
			set { _patientsName = value; }
		}

		[DicomField(DicomTags.PatientsBirthDate, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string PatientsBirthDate
		{
			get { return _patientsBirthDate; }
			set { _patientsBirthDate = value; }
		}

		[DicomField(DicomTags.PatientsBirthTime, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string PatientsBirthTime
		{
			get { return _patientsBirthTime; }
			set { _patientsBirthTime = value; }
		}

		[DicomField(DicomTags.PatientsSex, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string PatientsSex
		{
			get { return _patientsSex; }
			set { _patientsSex = value; }
		}

		[DicomField(DicomTags.NumberOfPatientRelatedStudies, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public int? NumberOfPatientRelatedStudies
		{
			get { return _numberOfPatientRelatedStudies; }
			set { _numberOfPatientRelatedStudies = value; }
		}

		[DicomField(DicomTags.NumberOfPatientRelatedSeries, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public int? NumberOfPatientRelatedSeries
		{
			get { return _numberOfPatientRelatedSeries; }
			set { _numberOfPatientRelatedSeries = value; }
		}

		[DicomField(DicomTags.NumberOfPatientRelatedInstances, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public int? NumberOfPatientRelatedInstances
		{
			get { return _numberOfPatientRelatedInstances; }
			set { _numberOfPatientRelatedInstances = value; }
		}

		#endregion
	}
}
