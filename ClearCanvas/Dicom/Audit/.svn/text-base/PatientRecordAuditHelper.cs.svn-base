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

namespace ClearCanvas.Dicom.Audit
{
	/// <summary>
	/// Patient Record Audit Message Helper
	/// </summary>
	/// <remarks>
	/// <para>
	/// This message describes the event of a patient record being created, modified, accessed, or deleted.
	/// </para>
	/// <para>
	/// Note: There are several types of patient records managed by both DICOM and non-DICOM system. DICOM
	/// applications often manipulate patient records managed by a variety of systems, and thus may be
	/// obligated by site security policies to record such events in the audit logs. This audit event can be used to
	/// record the access or manipulation of patient records where specific DICOM SOP Instances are not
	/// involved.
	/// </para>
	/// </remarks>
	public class PatientRecordAuditHelper : DicomAuditHelper
	{
		public PatientRecordAuditHelper(DicomAuditSource auditSource, EventIdentificationTypeEventOutcomeIndicator outcome,
			EventIdentificationTypeEventActionCode code)
			: base("PatientRecord")
		{
			AuditMessage.EventIdentification = new EventIdentificationType();
			AuditMessage.EventIdentification.EventID = CodedValueType.PatientRecord;
			AuditMessage.EventIdentification.EventActionCode = code;
			AuditMessage.EventIdentification.EventActionCodeSpecified = true;
			AuditMessage.EventIdentification.EventDateTime = Platform.Time.ToUniversalTime();
			AuditMessage.EventIdentification.EventOutcomeIndicator = outcome;

			InternalAddAuditSource(auditSource);
		}

		/// <summary>
		/// The identity of the person or process manipulating the data. If both
		/// the person and the process are known, both shall be included.
		/// </summary>
		/// <param name="participant">The participant to add.</param>
		public void AddUserParticipant(AuditActiveParticipant participant)
		{
			participant.UserIsRequestor = true;
			InternalAddActiveParticipant(participant);
		}

		/// <summary>
		/// Add details of a Patient.
		/// </summary>
		/// <param name="study"></param>
		public void AddPatientParticipantObject(AuditPatientParticipantObject patient)
		{
			InternalAddParticipantObject(patient.PatientId + patient.PatientsName,patient);
		}
	}
}
