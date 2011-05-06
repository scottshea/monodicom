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
	public interface IStudyRootStudyIdentifier : IStudyRootData, IIdentifier
	{ }

	/// <summary>
	/// Study Root Query identifier for a study.
	/// </summary>
	[DataContract(Namespace = QueryNamespace.Value)]
	public class StudyRootStudyIdentifier : StudyIdentifier, IStudyRootStudyIdentifier
	{
		#region Private Fields

		private string _patientId;
		private string _patientsName;
		private string _patientsBirthDate;
		private string _patientsBirthTime;
		private string _patientsSex;

		#endregion

		#region Public Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public StudyRootStudyIdentifier()
		{
		}

		public StudyRootStudyIdentifier(IStudyRootStudyIdentifier other)
			: base(other)
		{
			CopyFrom(other);
		}

		public StudyRootStudyIdentifier(IStudyRootData other, IIdentifier identifier)
			: base(other, identifier)
		{
			CopyFrom(other);
		}

		public StudyRootStudyIdentifier(IStudyRootData other)
			: base(other)
		{
			CopyFrom(other);
		}

		/// <summary>
		/// Creates an instance of <see cref="StudyRootStudyIdentifier"/> from a <see cref="DicomAttributeCollection"/>.
		/// </summary>
		public StudyRootStudyIdentifier(DicomAttributeCollection attributes)
			: base(attributes)
		{
		}

		#endregion

		private void CopyFrom(IPatientData other)
		{
			PatientId = other.PatientId;
			PatientsName = other.PatientsName;
			PatientsBirthDate = other.PatientsBirthDate;
			PatientsBirthTime = other.PatientsBirthTime;
			PatientsSex = other.PatientsSex;
		}

		#region Public Properties

		/// <summary>
		/// Gets or sets the patient id of the identified study.
		/// </summary>
		[DicomField(DicomTags.PatientId, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string PatientId
		{
			get { return _patientId; }
			set { _patientId = value; }
		}

		/// <summary>
		/// Gets or sets the patient's name for the identified study.
		/// </summary>
		[DicomField(DicomTags.PatientsName, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string PatientsName
		{
			get { return _patientsName; }
			set { _patientsName = value; }
		}

		/// <summary>
		/// Gets or sets the patient's birth date for the identified study.
		/// </summary>
		[DicomField(DicomTags.PatientsBirthDate, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string PatientsBirthDate
		{
			get { return _patientsBirthDate; }
			set { _patientsBirthDate = value; }
		}

		/// <summary>
		/// Gets or sets the patient's birth time for the identified study.
		/// </summary>
		[DicomField(DicomTags.PatientsBirthTime, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string PatientsBirthTime
		{
			get { return _patientsBirthTime; }
			set { _patientsBirthTime = value; }
		}

		/// <summary>
		/// Gets or sets the patient's sex for the identified study.
		/// </summary>
		[DicomField(DicomTags.PatientsSex, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string PatientsSex
		{
			get { return _patientsSex; }
			set { _patientsSex = value; }
		}

		#endregion
	}
}
