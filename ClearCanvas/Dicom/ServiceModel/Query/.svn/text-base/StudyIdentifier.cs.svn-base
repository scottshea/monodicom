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
	public interface IStudyIdentifier : IStudyData, IIdentifier
	{ }

	/// <summary>
	/// Query identifier for a study.
	/// </summary>
	[DataContract(Namespace = QueryNamespace.Value)]
	public class StudyIdentifier : Identifier, IStudyIdentifier
	{
		#region Private Fields

		private string _studyInstanceUid;
		private string[] _modalitiesInStudy;
		private string _studyDescription;
		private string _studyId;
		private string _studyDate;
		private string _studyTime;
		private string _accessionNumber;
		private string _referringPhysiciansName;
		private int? _numberOfStudyRelatedSeries;
		private int? _numberOfStudyRelatedInstances;

		#endregion

		#region Public Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public StudyIdentifier()
		{
		}

		public StudyIdentifier(IStudyIdentifier other)
			: base(other)
		{
			CopyFrom(other);
		}

		public StudyIdentifier(IStudyData other, IIdentifier identifier)
			: base(identifier)
		{
			CopyFrom(other);
		}

		public StudyIdentifier(IStudyData other)
		{
			CopyFrom(other);
		}

		/// <summary>
		/// Creates an instance of <see cref="StudyIdentifier"/> from a <see cref="DicomAttributeCollection"/>.
		/// </summary>
		public StudyIdentifier(DicomAttributeCollection attributes)
			: base(attributes)
		{
		}

		#endregion

		private void CopyFrom(IStudyData other)
		{
			ReferringPhysiciansName = other.ReferringPhysiciansName;
			AccessionNumber = other.AccessionNumber;
			StudyDescription = other.StudyDescription;
			StudyId = other.StudyId;
			StudyDate = other.StudyDate;
			StudyTime = other.StudyTime;
			ModalitiesInStudy = other.ModalitiesInStudy;
			StudyInstanceUid = other.StudyInstanceUid;
			NumberOfStudyRelatedSeries = other.NumberOfStudyRelatedSeries;
			NumberOfStudyRelatedInstances = other.NumberOfStudyRelatedInstances;
		}

		#region Public Properties

		/// <summary>
		/// Gets the level of the query - STUDY.
		/// </summary>
		public override string QueryRetrieveLevel
		{
			get { return "STUDY"; }
		}

		/// <summary>
		/// Gets or sets the Study Instance Uid of the identified study.
		/// </summary>
		[DicomField(DicomTags.StudyInstanceUid, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = true)]
		public string StudyInstanceUid
		{
			get { return _studyInstanceUid; }
			set { _studyInstanceUid = value; }
		}

		/// <summary>
		/// Gets or sets the modalities in the identified study.
		/// </summary>
		[DicomField(DicomTags.ModalitiesInStudy, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string[] ModalitiesInStudy
		{
			get { return _modalitiesInStudy; }
			set { _modalitiesInStudy = value; }
		}

		/// <summary>
		/// Gets or sets the study description of the identified study.
		/// </summary>
		[DicomField(DicomTags.StudyDescription, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string StudyDescription
		{
			get { return _studyDescription; }
			set { _studyDescription = value; }
		}

		/// <summary>
		/// Gets or sets the study id of the identified study.
		/// </summary>
		[DicomField(DicomTags.StudyId, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string StudyId
		{
			get { return _studyId; }
			set { _studyId = value; }
		}

		/// <summary>
		/// Gets or sets the study date of the identified study.
		/// </summary>
		[DicomField(DicomTags.StudyDate, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string StudyDate
		{
			get { return _studyDate; }
			set { _studyDate = value; }
		}

		/// <summary>
		/// Gets or sets the study time of the identified study.
		/// </summary>
		[DicomField(DicomTags.StudyTime, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string StudyTime
		{
			get { return _studyTime; }
			set { _studyTime = value; }
		}

		/// <summary>
		/// Gets or sets the accession number of the identified study.
		/// </summary>
		[DicomField(DicomTags.AccessionNumber, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string AccessionNumber
		{
			get { return _accessionNumber; }
			set { _accessionNumber = value; }
		}

		[DicomField(DicomTags.ReferringPhysiciansName, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string ReferringPhysiciansName
		{
			get { return _referringPhysiciansName; }
			set { _referringPhysiciansName = value; }
		}

		/// <summary>
		/// Gets or sets the number of series belonging to the identified study.
		/// </summary>
		[DicomField(DicomTags.NumberOfStudyRelatedSeries, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public int? NumberOfStudyRelatedSeries
		{
			get { return _numberOfStudyRelatedSeries; }
			set { _numberOfStudyRelatedSeries = value; }
		}

		/// <summary>
		/// Gets or sets the number of composite object instances belonging to the identified study.
		/// </summary>
		[DicomField(DicomTags.NumberOfStudyRelatedInstances, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public int? NumberOfStudyRelatedInstances
		{
			get { return _numberOfStudyRelatedInstances; }
			set { _numberOfStudyRelatedInstances = value; }
		}

		#endregion
	}
}
