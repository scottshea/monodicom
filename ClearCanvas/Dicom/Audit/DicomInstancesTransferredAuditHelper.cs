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

using ClearCanvas.Common;
using ClearCanvas.Dicom.Network;
using ClearCanvas.Dicom.Network.Scu;

namespace ClearCanvas.Dicom.Audit
{
	/// <summary>
	/// DICOM Instances Transferred
	/// </summary>
	/// <remarks>
	/// <para>
	/// This message describes the event of the completion of transferring DICOM SOP Instances between two
	/// Application Entities. This message may only include information about a single patient.
	/// </para>
	/// <para>
	/// Note: This message may have been preceded by a Begin Transferring Instances message. The Begin
	/// Transferring Instances message conveys the intent to store SOP Instances, while the Instances
	/// Transferred message records the completion of the transfer. Any disagreement between the two
	/// messages might indicate a potential security breach.
	/// </para>
	/// </remarks>
	public class DicomInstancesTransferredAuditHelper : DicomAuditHelper
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public DicomInstancesTransferredAuditHelper(DicomAuditSource auditSource, EventIdentificationTypeEventOutcomeIndicator outcome,
			EventIdentificationTypeEventActionCode action,
			AssociationParameters parms)
			: base("DicomInstancesTransferred")
		{
			AuditMessage.EventIdentification = new EventIdentificationType();
			AuditMessage.EventIdentification.EventID = CodedValueType.DICOMInstancesTransferred;
			AuditMessage.EventIdentification.EventActionCode = action;
			AuditMessage.EventIdentification.EventActionCodeSpecified = true;
			AuditMessage.EventIdentification.EventDateTime = Platform.Time.ToUniversalTime();
			AuditMessage.EventIdentification.EventOutcomeIndicator = outcome;

			InternalAddActiveDicomParticipant(parms);

			InternalAddAuditSource(auditSource);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public DicomInstancesTransferredAuditHelper(DicomAuditSource auditSource, EventIdentificationTypeEventOutcomeIndicator outcome,
			EventIdentificationTypeEventActionCode action,
			string sourceAE, string sourceHost, string destinationAE, string destinationHost)
			: base("DicomInstancesTransferred")
		{
			AuditMessage.EventIdentification = new EventIdentificationType();
			AuditMessage.EventIdentification.EventID = CodedValueType.DICOMInstancesTransferred;
			AuditMessage.EventIdentification.EventActionCode = action;
			AuditMessage.EventIdentification.EventActionCodeSpecified = true;
			AuditMessage.EventIdentification.EventDateTime = Platform.Time.ToUniversalTime();
			AuditMessage.EventIdentification.EventOutcomeIndicator = outcome;

			InternalAddActiveDicomParticipant(sourceAE, sourceHost, destinationAE, destinationHost);

			InternalAddAuditSource(auditSource);
		}

		/// <summary>
		/// (Optional) The identity of any other participants that might be involved andknown, especially third parties that are the requestor
		/// </summary>
		/// <param name="participant">The participant</param>
		public void AddOtherParticipants(AuditActiveParticipant participant)
		{
			InternalAddActiveParticipant(participant);
		}

		/// <summary>
		/// Add details of a Patient.
		/// </summary>
		/// <param name="study"></param>
		public void AddPatientParticipantObject(AuditPatientParticipantObject patient)
		{
			InternalAddParticipantObject(patient.PatientId + patient.PatientsName, patient);
		}

		/// <summary>
		/// Add details of a study.
		/// </summary>
		/// <param name="study"></param>
		public void AddStudyParticipantObject(AuditStudyParticipantObject study)
		{
			InternalAddParticipantObject(study.StudyInstanceUid, study);
		}

		/// <summary>
		/// Add details of images within a study.  SOP Class information is automatically updated.
		/// </summary>
		/// <param name="instance">Descriptive object being audited</param>
		public void AddStorageInstance(StorageInstance instance)
		{
			InternalAddStorageInstance(instance);
		}
	}
}
