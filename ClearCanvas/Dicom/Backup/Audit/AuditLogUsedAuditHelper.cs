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
	/// This message describes the event of a person or process accessing a log of audit trail information.
	/// </summary>
	/// <remarks>
	/// For example, an implementation that maintains a local cache of audit information that has not been
	/// transferred to a central collection point might generate this message if its local cache were accessed by
	/// a user.
	/// </remarks>
	public class AuditLogUsedAuditHelper : DicomAuditHelper
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="auditSource">The source of the audit</param>
		/// <param name="outcome">The outcome</param>
		/// <param name="uriOfAuditLog">Add the Identity of the Audit Log.  </param>
		public AuditLogUsedAuditHelper(DicomAuditSource auditSource, EventIdentificationTypeEventOutcomeIndicator outcome,
			string uriOfAuditLog)
			: base("AuditLogUsed")
		{
			AuditMessage.EventIdentification = new EventIdentificationType();
			AuditMessage.EventIdentification.EventID = CodedValueType.AuditLogUsed;
			AuditMessage.EventIdentification.EventActionCode = EventIdentificationTypeEventActionCode.R;
			AuditMessage.EventIdentification.EventActionCodeSpecified = true;
			AuditMessage.EventIdentification.EventDateTime = Platform.Time.ToUniversalTime();
			AuditMessage.EventIdentification.EventOutcomeIndicator = outcome;

			InternalAddAuditSource(auditSource);

			AuditSecurityAlertParticipantObject o =
				new AuditSecurityAlertParticipantObject(ParticipantObjectTypeCodeRoleEnum.SecurityResource,
														ParticipantObjectIdTypeCodeEnum.URI, uriOfAuditLog,
														"Security Audit Log");
			// Only one can be included.
			_participantObjectList.Clear();
			_participantObjectList.Add(uriOfAuditLog, o);
		}

		/// <summary>
		/// Add the ID of person or process that started or stopped the Application.  Can be called twice.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// <param name="userId">The person or process accessing the audit trail. If both are known,
		/// then two active participants shall be included (both the person and the process).</param>
		public void AddActiveParticipant(AuditActiveParticipant participant)
		{
			participant.UserIsRequestor = true;
			InternalAddActiveParticipant(participant);
		}
	}
}
