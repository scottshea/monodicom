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
	/// Query Audit Message Helper
	/// </summary>
	/// <remarks>
	/// <para>
	/// This message describes the event of a Query being issued or received. The message does not record the
	/// response to the query, but merely records the fact that a query was issued. For example, this would report
	/// queries using the DICOM SOP Classes:
	/// <list>
	/// <item>
	/// a. Modality Worklist
	/// </item>
	/// <item>
	/// b. General Purpose Worklist
	/// </item>
	/// <item>
	/// c. Composite Instance Query
	/// </item>
	/// </list>
	/// </para>
	/// <para>
	/// Notes: 1. The response to a query may result in one or more Instances Transferred or Instances Accessed
	/// messages, depending on what events transpire after the query. If there were security-related failures,
	/// such as access violations, when processing a query, those failures should show up in other audit
	/// messages, such as a Security Alert message.
	/// </para>
	/// <para>
	/// 2. Non-DICOM queries may also be captured by this message. The Participant Object ID Type Code,
	/// the Participant Object ID, and the Query fields may have values related to such non-DICOM queries.
	/// </para>
	/// </remarks>
	public class QueryAuditHelper : DicomAuditHelper
	{
		public QueryAuditHelper(DicomAuditSource auditSource,
			EventIdentificationTypeEventOutcomeIndicator outcome, 
			AssociationParameters parms)
			: base("Query")
		{
			AuditMessage.EventIdentification = new EventIdentificationType();
			AuditMessage.EventIdentification.EventID = CodedValueType.Query;
			AuditMessage.EventIdentification.EventActionCode = EventIdentificationTypeEventActionCode.E;
			AuditMessage.EventIdentification.EventActionCodeSpecified = true;
			AuditMessage.EventIdentification.EventDateTime = Platform.Time.ToUniversalTime();
			AuditMessage.EventIdentification.EventOutcomeIndicator = outcome;

			InternalAddActiveDicomParticipant(parms);

			InternalAddAuditSource(auditSource);
		}

		public QueryAuditHelper(DicomAuditSource auditSource,
			EventIdentificationTypeEventOutcomeIndicator outcome,
			string sourceAE, string sourceHost, string destinationAE, string destinationHost) : base("Query")
		{
			AuditMessage.EventIdentification = new EventIdentificationType();
			AuditMessage.EventIdentification.EventID = CodedValueType.Query;
			AuditMessage.EventIdentification.EventActionCode = EventIdentificationTypeEventActionCode.E;
			AuditMessage.EventIdentification.EventActionCodeSpecified = true;
			AuditMessage.EventIdentification.EventDateTime = Platform.Time.ToUniversalTime();
			AuditMessage.EventIdentification.EventOutcomeIndicator = outcome;

			InternalAddActiveDicomParticipant(sourceAE, sourceHost, destinationAE, destinationHost);

			InternalAddAuditSource(auditSource);
		}

		/// <summary>
		/// The identity of any other participants that might be involved and
		/// known, especially third parties that are the requestor
		/// </summary>
		/// <param name="participant">The participant to add.</param>
		public void AddOtherParticipant(AuditActiveParticipant participant)
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
			base.InternalAddStorageInstance(instance);
		}
	}
}
