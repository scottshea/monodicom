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
using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Dicom.Network.Scu;

namespace ClearCanvas.Dicom.Audit
{

	/// <summary>
	/// Object use for adding SOP Class information to the ParticipatingObjectDescription field in an Audit Message.
	/// </summary>
	public class AuditSopClass
	{
		private readonly string _uid;
		private int _numberOfInstances;

		public AuditSopClass(string uid, int numberOfInstances)
		{
			_uid = uid;
			_numberOfInstances = numberOfInstances;
		}

		public string UID
		{
			get { return _uid; }
		}
		public int NumberOfInstances
		{
			get { return _numberOfInstances; }
			set { _numberOfInstances = value; }
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// <para>
	/// From RFC: 3881
	/// The following data assist the auditing process by indicating specific
	/// instances of data or objects that have been accessed.
	/// </para>
	/// <para>
	/// These data are required unless the values for Event Identification,
	/// Active Participant Identification, and Audit Source Identification
	/// are sufficient to document the entire auditable event.  Production of
	/// audit records containing these data may be enabled or suppressed, as
	/// determined by healthcare organization policy and regulatory
	/// requirements.
	/// </para>
	/// <para>
	///    Because events may have more than one participant object, this group
	///    can be a repeating set of values.  For example, depending on
	///    institutional policies and implementation choices:
	///</para>
	/// <list>
	/// <item>
	///    -  Two participant object value-sets can be used to identify access
	///       to patient data by medical record number plus the specific health
	///       care encounter or episode for the patient.
	/// </item><item>
	///    -  A patient participant and his authorized representative may be
	///       identified concurrently.
	/// </item><item>
	///    -  An attending physician and consulting referrals may be identified
	///       concurrently.
	/// </item><item>
	///    -  All patients identified on a worklist may be identified.
	/// </item><item>
	///    -  For radiological studies, a set of related participant objects
	///       identified by accession number or study number, may be identified.
	/// </item>
	/// </list>
	/// <para>
	///    Note, though, that each audit message documents only a single usage
	///    instance of such participant object relationships and does not serve
	///    to document all relationships that may be present or possible.
	/// </para>
	/// </remarks>
	public abstract class AuditParticipantObject
	{
		protected ParticipantObjectTypeCodeEnum? _typeCode = null;
		protected ParticipantObjectTypeCodeRoleEnum? _typeCodeRole = null;
		protected ParticipantObjectDataLifeCycleEnum? _dataLifeCycle = null;
		protected ParticipantObjectIdTypeCodeEnum? _objectIdTypeCode;
		protected string _participantObjectSensitivity = null;
		protected string _participantObjectId = null;
		protected string _participantObjectName = null;
		protected string _participantObjectQuery = null;
		protected string _participantObjectDetail = null;
		protected CodedValueType _typeCodeCodedValue;
		protected Dictionary<string,AuditSopClass> _sopClassList = new Dictionary<string, AuditSopClass>();
		protected string _accession = null;
		protected string _mppsUid = null;
		private string _participantObjectDescription;

		protected AuditParticipantObject()
		{}

		/// <summary>
		/// Code for the participant object type being audited.  This value is distinct 
		/// from the user's role or any user relationship to the participant object.
		/// </summary>
		public ParticipantObjectTypeCodeEnum? ParticipantObjectTypeCode
		{
			get { return _typeCode;}
			set { _typeCode = value;}
		}

		/// <summary>
		/// Code representing the functional application role of Participant Object being audited
		/// </summary>
		public ParticipantObjectTypeCodeRoleEnum? ParticipantObjectTypeCodeRole
		{
			get { return _typeCodeRole; }
			set { _typeCodeRole = value; }
		}

		/// <summary>
		/// Identifier for the data life-cycle stage for the participant
		/// object.  This can be used to provide an audit trail for data, over
		/// time, as it passes through the system.
		/// </summary>
		public ParticipantObjectDataLifeCycleEnum? ParticipantObjectDataLifeCycle
		{
			get { return _dataLifeCycle; }
			set { _dataLifeCycle = value; }
		}

		/// <summary>
		///  Describes the identifier that is contained in Participant Object ID.
		/// </summary>
		/// <remarks>
		/// From DICOM Supplement 95:
		/// Values may be drawn from those listed in RFC 3881 and DCID (ccc5),as specified in the individual message descriptions.
		/// </remarks>
		public ParticipantObjectIdTypeCodeEnum? ParticipantObjectIdTypeCode
		{
			get { return _objectIdTypeCode; }
			set { _objectIdTypeCode = value; }
		}

		/// <summary>
		/// Coded value representing the participating object type being audited.  Can be
		/// used instead of 
		/// </summary>
		public CodedValueType ParticipantObjectIdTypeCodedValue
		{
			get { return _typeCodeCodedValue; }
			set { _typeCodeCodedValue = value; }
		}
		/// <summary>
		/// Denotes policy-defined sensitivity for the Participant Object ID
		/// such as VIP, HIV status, mental health status, or similar topics.
		/// </summary>
		public string ParticipantObjectSensitivity
		{
			get { return _participantObjectSensitivity; }
		}

		/// <summary>
		/// Identifies a specific instance of the participant object.
		/// </summary>
		public string ParticipantObjectId
		{
			get { return _participantObjectId; }
		}

		/// <summary>
		/// An instance-specific descriptor of the Participant Object ID
		/// audited, such as a person's name.
		/// </summary>
		public string ParticipantObjectName
		{
			get { return _participantObjectName; }
		}

		/// <summary>
		///    The actual query for a query-type participant object.
		/// </summary>
		public string ParticipantObjectQuery
		{
			get { return _participantObjectQuery; }
		}

		/// <summary>
		/// Implementation-defined data about specific details of the object
		/// accessed or used.
		/// </summary>
		/// <remarks>
		/// From DICOM Supplement 95:
		/// Used as defined in RFC 3881.  DICOM does not specify any additional use for this attribute.
		/// Note: The value field is base64 encoded, making this attribute suitable for conveying binary data.
		/// </remarks>
		public string ParticipantObjectDetail
		{
			get { return _participantObjectDetail; }
			set { _participantObjectDetail = value; }
		}

		public string ParticipantObjectDescription
		{
			get { return _participantObjectDescription; }
			set { _participantObjectDescription = value; }
		}

		public Dictionary<string, AuditSopClass> SopClassDictionary
		{
			get { return _sopClassList; }
		}

		/// <summary>
		/// Accession Number
		/// </summary>
		public string AccessionNumber
		{
			get { return _accession; }
		}

		/// <summary>
		/// MPPS UID
		/// </summary>
		public string MppsUid
		{
			get { return _mppsUid; }
		}
	}

	public class AuditPatientParticipantObject : AuditParticipantObject
	{
		public AuditPatientParticipantObject(string patientName, string patientId)
		{
			Platform.CheckForNullReference(patientName, "patientName");
			Platform.CheckForNullReference(patientId, "patientId");

			ParticipantObjectTypeCode = ParticipantObjectTypeCodeEnum.Person;
			ParticipantObjectTypeCodeRole = ParticipantObjectTypeCodeRoleEnum.Patient;
			ParticipantObjectIdTypeCode = ParticipantObjectIdTypeCodeEnum.PatientNumber;
			_participantObjectId = patientId;
			_participantObjectName = patientName;			
		}

		public AuditPatientParticipantObject(DicomAttributeCollection collection)
		{
			Platform.CheckForNullReference(collection, "collection");

			ParticipantObjectTypeCode = ParticipantObjectTypeCodeEnum.Person;
			ParticipantObjectTypeCodeRole = ParticipantObjectTypeCodeRoleEnum.Patient;
			ParticipantObjectIdTypeCode = ParticipantObjectIdTypeCodeEnum.PatientNumber;
			_participantObjectId = collection[DicomTags.PatientId].ToString();
			_participantObjectName = collection[DicomTags.PatientsName].ToString();
		}

		public string PatientId
		{
			get { return _participantObjectId; }
		}

		public string PatientsName
		{
			get { return _participantObjectName; }
		}
	}

	public class AuditStudyParticipantObject : AuditParticipantObject
	{
		public AuditStudyParticipantObject(string studyInstanceUid)
		{
			Platform.CheckForNullReference(studyInstanceUid, "studyInstanceUid");
			ParticipantObjectIdTypeCodedValue = CodedValueType.StudyInstanceUID;
			_participantObjectId = studyInstanceUid;
		}

		public AuditStudyParticipantObject(string studyInstanceUid, string accession)
		{
			Platform.CheckForNullReference(studyInstanceUid, "studyInstanceUid");

			ParticipantObjectIdTypeCodedValue = CodedValueType.StudyInstanceUID;
			_participantObjectId = studyInstanceUid;
			_accession = accession;
		}

		public AuditStudyParticipantObject(string studyInstanceUid, string accession, string mppsUid)
		{
			Platform.CheckForNullReference(studyInstanceUid, "studyInstanceUid");
			Platform.CheckForNullReference(mppsUid, "mppsUid");

			ParticipantObjectIdTypeCodedValue = CodedValueType.StudyInstanceUID;
			_participantObjectId = studyInstanceUid;
			_accession = accession;
			_mppsUid = mppsUid;
		}

		public void AddSopClass(string uid, int numberOfInstances)
		{
			SopClassDictionary.Add(uid, new AuditSopClass(uid, numberOfInstances));
		}

		public string StudyInstanceUid
		{
			get { return _participantObjectId; }
		}

		public void AddStorageInstance(StorageInstance instance)
		{
			AuditSopClass sopClass;
			if (SopClassDictionary.TryGetValue(instance.SopClass.Uid,out sopClass))
			{
				sopClass.NumberOfInstances++;
			}
			else
			{
				sopClass = new AuditSopClass(instance.SopClass.Uid, 1);
				SopClassDictionary.Add(sopClass.UID, sopClass);
			}
		}
	}

	public class AuditSecurityAlertParticipantObject : AuditParticipantObject
	{
		public AuditSecurityAlertParticipantObject(ParticipantObjectTypeCodeRoleEnum role, ParticipantObjectIdTypeCodeEnum objectIdType, string participantObjectId)
		{
			Platform.CheckForNullReference(participantObjectId,"participantObjectId");

			ParticipantObjectTypeCode = ParticipantObjectTypeCodeEnum.SystemObject;
			ParticipantObjectTypeCodeRole = role;
			ParticipantObjectIdTypeCode = objectIdType;
			_participantObjectId = participantObjectId;
		}
		public AuditSecurityAlertParticipantObject(ParticipantObjectTypeCodeRoleEnum role, ParticipantObjectIdTypeCodeEnum objectIdType, string participantObjectId, string participantObjectName)
		{
			Platform.CheckForNullReference(participantObjectId, "participantObjectId");

			ParticipantObjectTypeCode = ParticipantObjectTypeCodeEnum.SystemObject;
			ParticipantObjectTypeCodeRole = role;
			ParticipantObjectIdTypeCode = objectIdType;
			_participantObjectId = participantObjectId;
			_participantObjectName = participantObjectName;
		}
	}
}
