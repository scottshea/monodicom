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
using System.Text;
using ClearCanvas.Dicom.IO;

namespace ClearCanvas.Dicom.Audit
{
	public partial class AuditMessage
	{

	}

	public partial class CodedValueType
	{
		public static CodedValueType ApplicationActivity = new CodedValueType("110100", "DCM", "Application Activity", "Application audit event");
		public static CodedValueType AuditLogUsed = new CodedValueType("110101", "DCM", "	Audit Log Used	", "Audit Log Used audit event");
		public static CodedValueType BeginTransferringDICOMInstances = new CodedValueType("110102", "DCM", "Begin Transferring DICOM Instances", "Begin Storing DICOM Instances audit event");
		public static CodedValueType DICOMInstancesAccessed = new CodedValueType("110103", "DCM", "DICOM Instances Accessed", "DICOM Instances created, read, updated, or deleted audit event");
		public static CodedValueType DICOMInstancesTransferred = new CodedValueType("110104", "DCM", "DICOM Instances Transferred", "Storage of DICOM Instances complete audit event");
		public static CodedValueType DICOMStudyDeleted = new CodedValueType("110105", "DCM", "DICOM Study Deleted", "Deletion of Entire Study audit event");
		public static CodedValueType Export = new CodedValueType("110106", "DCM", "Export", "Data exported out of the system audit event");
		public static CodedValueType Import = new CodedValueType("110107", "DCM", "Import", "Data imported into the system audit event");
		public static CodedValueType NetworkEntry = new CodedValueType("110108", "DCM", "Network Entry", "Network Entry audit event");
		public static CodedValueType OrderRecord = new CodedValueType("110109", "DCM", "Order Record", "Order Record audit event");
		public static CodedValueType PatientRecord = new CodedValueType("110110", "DCM", "Patient Record", "Patient Record audit event");
		public static CodedValueType ProcedureRecord = new CodedValueType("110111", "DCM", "Procedure Record", "Procedure Record audit event");
		public static CodedValueType Query = new CodedValueType("110112", "DCM", "Query", "Query requested audit event");
		public static CodedValueType SecurityAlert = new CodedValueType("110113", "DCM", "Security Alert", "SecurityAlert audit event");
		public static CodedValueType UserAuthentication = new CodedValueType("110114", "DCM", "User Authentication", "User Authentication audit event");
		public static CodedValueType ApplicationStart = new CodedValueType("110120", "DCM", "Application Start", "Application Entity Started audit event type");
		public static CodedValueType ApplicationStop = new CodedValueType("110121", "DCM", "Application Stop", "Application Entity Stopped audit event type");
		public static CodedValueType Login = new CodedValueType("110122", "DCM", "Login", "User login attempted audit event type");
		public static CodedValueType Logout = new CodedValueType("110123", "DCM", "Logout", "User logout attempted audit event type");
		public static CodedValueType Attach = new CodedValueType("110124", "DCM", "Attach", "Node attaches to a network audit event type");
		public static CodedValueType Detach = new CodedValueType("110125", "DCM", "Detach", "Node detaches from a network audit event type");
		public static CodedValueType NodeAuthentication = new CodedValueType("110126", "DCM", "Node Authentication", "Node Authentication audit event type");
		public static CodedValueType EmergencyOverride = new CodedValueType("110127", "DCM", "Emergency Override", "Emergency Override audit event type");
		public static CodedValueType NetworkConfiguration = new CodedValueType("110128", "DCM", "Network Configuration", "Network configuration change audit event type");
		public static CodedValueType SecurityConfiguration = new CodedValueType("110129", "DCM", "Security Configuration", "Security configuration change audit event type");
		public static CodedValueType HardwareConfiguration = new CodedValueType("110130", "DCM", "Hardware Configuration", "Hardware configuration change audit event type");
		public static CodedValueType SoftwareConfiguration = new CodedValueType("110131", "DCM", "Software Configuration", "Software configuration change audit event type");
		public static CodedValueType UseOfRestrictedFunction = new CodedValueType("110132", "DCM", "Use of Restricted Function", "A use of a restricted function audit event type");
		public static CodedValueType AuditRecordingStopped = new CodedValueType("110133", "DCM", "Audit Recording Stopped", "Audit recording stopped audit event type");
		public static CodedValueType AuditRecordingStarted = new CodedValueType("110134", "DCM", "Audit Recording Started", "Audit recording started audit event type");
		public static CodedValueType ObjectSecurityAttributesChanged = new CodedValueType("110135", "DCM", "Object Security Attributes Changed", "The security attributes of an object changed audit event type");
		public static CodedValueType SecurityRolesChanged = new CodedValueType("110136", "DCM", "Security Roles Changed", "Security roles changed audit event type");
		public static CodedValueType UserSecurityAttributesChanged = new CodedValueType("110137", "DCM", "User security Attributes Changed", "The security attributes of a user changed audit event type");
		public static CodedValueType Application = new CodedValueType("110150", "DCM", "Application", "Audit participant role ID of DICOM application");
		public static CodedValueType ApplicationLauncher = new CodedValueType("110151", "DCM", "Application Launcher", "Audit participant role ID of DICOM application launcher, i.e., the entity that started or stopped an application.");
		public static CodedValueType Destination = new CodedValueType("110152", "DCM", "Destination", "Audit participant role ID of the receiver of data");
		public static CodedValueType Source = new CodedValueType("110153", "DCM", "Source", "Audit participant role ID of the sender of data");
		public static CodedValueType DestinationMedia = new CodedValueType("110154", "DCM", "Destination Media", "Audit participant role ID of media receiving data during an export.");
		public static CodedValueType SourceMedia = new CodedValueType("110155", "DCM", "Source Media", "Audit participant role ID of media providing data during an import.");
		public static CodedValueType StudyInstanceUID = new CodedValueType("110180", "DCM", "Study Instance UID", "Study Instance UID Participant Object ID type");
		public static CodedValueType ClassUID = new CodedValueType("110181", "DCM", "Class UID", "SOP Class UID Participant Object ID type");
		public static CodedValueType NodeID = new CodedValueType("110182", "DCM", "Node ID", "ID of a node that is a participant object of an audit message");

		public CodedValueType(string _code, string _codeSystemNameField,
			string _displayNameField, string description)
		{
			codeSystemName = _codeSystemNameField;
			code = _code;
			displayNameField = _displayNameField;
			_description = description;
		}

		private readonly string _description;

		public CodedValueType()
		{
		}

		public string Description
		{
			get { return String.IsNullOrEmpty(_description) ? String.Empty : _description; }
		}
	}

	public partial class AuditMessageActiveParticipant
	{
		public AuditMessageActiveParticipant()
		{}

		public AuditMessageActiveParticipant(AuditActiveParticipant participant)
		{
			if (participant.RoleIdCode != null)
			{
				RoleIDCode = new CodedValueType[1];
				RoleIDCode[0] = participant.RoleIdCode;
			}
			if (participant.UserIsRequestor.HasValue)
			{
				UserIsRequestor = participant.UserIsRequestor.Value;
			}
			if (participant.UserId != null)
			{
				UserID = participant.UserId;
			}
			if (participant.AlternateUserId != null)
			{
				AlternativeUserID = participant.AlternateUserId;
			}
			if (participant.UserName != null)
				UserName = participant.UserName;

			if (participant.NetworkAccessPointId != null)
				NetworkAccessPointID = participant.NetworkAccessPointId;

			if (participant.NetworkAccessPointType.HasValue)
			{
				NetworkAccessPointTypeCode = (byte) participant.NetworkAccessPointType.Value;
				NetworkAccessPointTypeCodeSpecified = true;
			}
			else
				NetworkAccessPointTypeCodeSpecified = false;
		}

		public AuditMessageActiveParticipant(CodedValueType roleId, string _userIDField, string alternateUserId, string _userNameField, string _networkAccessPointIDField, NetworkAccessPointTypeEnum? _networkAccessPointTypeCode, bool? userIsRequestor)
		{
			if (roleId != null)
			{
				RoleIDCode = new CodedValueType[1];
				RoleIDCode[0] = roleId;
			}

			if (userIsRequestor.HasValue)
				UserIsRequestor = userIsRequestor.Value;

			if (_userIDField != null)
				UserID = _userIDField;

			if (alternateUserId != null)
				AlternativeUserID = alternateUserId;

			if (_userNameField != null)
				UserName = _userNameField;

			if (_networkAccessPointIDField != null)
				NetworkAccessPointID = _networkAccessPointIDField;

			if (_networkAccessPointTypeCode.HasValue)
			{
				NetworkAccessPointTypeCode = (byte)_networkAccessPointTypeCode.Value;
				NetworkAccessPointTypeCodeSpecified = true;
			}
			else
				NetworkAccessPointTypeCodeSpecified = false;
		}
	}
	public partial class ParticipantObjectIdentificationTypeParticipantObjectIDTypeCode
	{
		public ParticipantObjectIdentificationTypeParticipantObjectIDTypeCode()
		{}
		public ParticipantObjectIdentificationTypeParticipantObjectIDTypeCode(CodedValueType value)
		{
			code = value.code;
			codeSystem = value.codeSystem;
			codeSystemName = value.codeSystemName;
			displayName = value.displayName;
			originalText = value.originalText;
		}
	}

	public partial class AuditSourceIdentificationType
	{
		public AuditSourceIdentificationType()
		{}

		public AuditSourceIdentificationType(DicomAuditSource auditSource)
		{
			if (!String.IsNullOrEmpty(auditSource.EnterpriseSiteId))
				AuditEnterpriseSiteID = auditSource.EnterpriseSiteId;
			AuditSourceID = auditSource.AuditSourceId;

			if (auditSource.AuditSourceType.HasValue)
			{
				AuditSourceTypeCode = new AuditSourceIdentificationTypeAuditSourceTypeCode[1];
				AuditSourceTypeCode[0] = new AuditSourceIdentificationTypeAuditSourceTypeCode(auditSource.AuditSourceType.Value);
			}
		}
		public AuditSourceIdentificationType(string sourceId)
		{
			AuditSourceID = sourceId;
		}
	}

	public partial class AuditSourceIdentificationTypeAuditSourceTypeCode
	{
		public AuditSourceIdentificationTypeAuditSourceTypeCode()
		{}

		public AuditSourceIdentificationTypeAuditSourceTypeCode(AuditSourceTypeCodeEnum e)
		{
			code = ((byte)e).ToString();
		}
	}

	public partial class ParticipantObjectIdentificationTypeParticipantObjectIDTypeCode
	{
		public ParticipantObjectIdentificationTypeParticipantObjectIDTypeCode(ParticipantObjectIdTypeCodeEnum e)
		{
			code = ((byte)e).ToString();
		}
	}

	public partial class ParticipantObjectIdentificationType
	{
		public ParticipantObjectIdentificationType()
		{}

		public ParticipantObjectIdentificationType(ParticipantObjectTypeCodeEnum? type, 
			ParticipantObjectTypeCodeRoleEnum? role,
			ParticipantObjectDataLifeCycleEnum? lifeCycle, 
			string participantObjectId, 
			ParticipantObjectIdTypeCodeEnum typeCode)
		{
			if (type.HasValue)
			{
				ParticipantObjectTypeCode = (byte)type.Value;
				ParticipantObjectTypeCodeSpecified = true;
			}
			else ParticipantObjectTypeCodeSpecified = false;

			if (role.HasValue)
			{
				ParticipantObjectTypeCodeRole = (byte)role.Value;
				ParticipantObjectTypeCodeRoleSpecified = true;
			}
			else
				ParticipantObjectTypeCodeRoleSpecified = false;

			if (lifeCycle.HasValue)
			{
				ParticipantObjectDataLifeCycle = (byte)lifeCycle.Value;
				ParticipantObjectDataLifeCycleSpecified = true;
			}
			else
				ParticipantObjectDataLifeCycleSpecified = false;

			ParticipantObjectID = participantObjectId;

			ParticipantObjectIDTypeCode = new ParticipantObjectIdentificationTypeParticipantObjectIDTypeCode(typeCode);
		}

		public ParticipantObjectIdentificationType(AuditParticipantObject item)
		{
			if (item.ParticipantObjectTypeCode.HasValue)
			{
				ParticipantObjectTypeCode = (byte) item.ParticipantObjectTypeCode.Value;
				ParticipantObjectTypeCodeSpecified = true;
			}
			else ParticipantObjectTypeCodeSpecified = false;

			if (item.ParticipantObjectTypeCodeRole.HasValue)
			{
				ParticipantObjectTypeCodeRole = (byte) item.ParticipantObjectTypeCodeRole.Value;
				ParticipantObjectTypeCodeRoleSpecified = true;
			}
			else
				ParticipantObjectTypeCodeRoleSpecified = false;

			if (item.ParticipantObjectDataLifeCycle.HasValue)
			{
				ParticipantObjectDataLifeCycle = (byte) item.ParticipantObjectDataLifeCycle.Value;
				ParticipantObjectDataLifeCycleSpecified = true;
			}
			else
				ParticipantObjectDataLifeCycleSpecified = false;

			if (item.ParticipantObjectIdTypeCode.HasValue)
			{
				ParticipantObjectIDTypeCode =
					new ParticipantObjectIdentificationTypeParticipantObjectIDTypeCode(item.ParticipantObjectIdTypeCode.Value);
			}
			else if (item.ParticipantObjectIdTypeCodedValue != null)
			{
				ParticipantObjectIDTypeCode =
					new ParticipantObjectIdentificationTypeParticipantObjectIDTypeCode(item.ParticipantObjectIdTypeCodedValue);
			}

			if (!string.IsNullOrEmpty(item.ParticipantObjectDetail))
			{
				ParticipantObjectDetailString = new string[] {item.ParticipantObjectDetail};
			}

			if (!string.IsNullOrEmpty(item.ParticipantObjectId))
				ParticipantObjectID = item.ParticipantObjectId;

			if (!string.IsNullOrEmpty(item.ParticipantObjectName))
				Item = item.ParticipantObjectName;

			ParticipantObjectDescriptionType description = new ParticipantObjectDescriptionType();
			if (!String.IsNullOrEmpty(item.AccessionNumber))
				description.Accession = new ParticipantObjectDescriptionTypeAccession[] { new ParticipantObjectDescriptionTypeAccession(item.AccessionNumber) };
			if (!String.IsNullOrEmpty(item.MppsUid))
				description.MPPS = new ParticipantObjectDescriptionTypeMPPS[] { new ParticipantObjectDescriptionTypeMPPS(item.MppsUid) };

			if (item.SopClassDictionary != null && item.SopClassDictionary.Count > 0)
			{
				description.SOPClass = new ParticipantObjectDescriptionTypeSOPClass[item.SopClassDictionary.Count];
				List<AuditSopClass> list = new List<AuditSopClass>(item.SopClassDictionary.Values);
				for (int i = 0; i < item.SopClassDictionary.Count; i++)
				{
					description.SOPClass[i] =
						new ParticipantObjectDescriptionTypeSOPClass(list[i].UID, list[i].NumberOfInstances);
				}
			}

			ParticipantObjectDescription = new ParticipantObjectDescriptionType[] { description };

		}
	}

	public partial class ParticipantObjectDescriptionTypeAccession
	{
		public ParticipantObjectDescriptionTypeAccession()
		{}

		public ParticipantObjectDescriptionTypeAccession(string accessionNumber)
		{
			Number = accessionNumber;
		}
	}

	public partial class ParticipantObjectDescriptionTypeMPPS
	{
		public ParticipantObjectDescriptionTypeMPPS()
		{}

		public ParticipantObjectDescriptionTypeMPPS(string mppsUid)
		{
			UID = mppsUid;
		}
	}

	public partial class ParticipantObjectDescriptionTypeSOPClass
	{
		public ParticipantObjectDescriptionTypeSOPClass()
		{}

		public ParticipantObjectDescriptionTypeSOPClass(string sopClassUid, int numberOfInstances)
		{
			UID = sopClassUid;
			NumberOfInstances = numberOfInstances.ToString();
		}
	}
   
}