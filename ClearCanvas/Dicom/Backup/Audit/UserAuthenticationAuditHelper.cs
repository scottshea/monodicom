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
	/// Enum for use with <see cref="UserAuthenticationAuditHelper"/>.
	/// </summary>
	public enum UserAuthenticationEventType
	{
		Login,
		Logout
	}

	/// <summary>
	/// User Authentication Audit Message
	/// </summary>
	/// <remarks>
	/// <para>
	/// This message describes the event of a user has attempting to log on or log off, whether successful or not.
	/// No Participant Objects are needed for this message.
	/// </para>
	/// </remarks>
	public class UserAuthenticationAuditHelper : DicomAuditHelper
	{
		public UserAuthenticationAuditHelper(DicomAuditSource auditSource,
			EventIdentificationTypeEventOutcomeIndicator outcome, UserAuthenticationEventType type)
			: base("UserAuthentication")
		{
			AuditMessage.EventIdentification = new EventIdentificationType();
			AuditMessage.EventIdentification.EventID = CodedValueType.UserAuthentication;
			AuditMessage.EventIdentification.EventActionCode = EventIdentificationTypeEventActionCode.E;
			AuditMessage.EventIdentification.EventActionCodeSpecified = true;
			AuditMessage.EventIdentification.EventDateTime = Platform.Time.ToUniversalTime();
			AuditMessage.EventIdentification.EventOutcomeIndicator = outcome;

			InternalAddAuditSource(auditSource);

			if (type == UserAuthenticationEventType.Login)
				AuditMessage.EventIdentification.EventTypeCode = new CodedValueType[] { CodedValueType.Login };
			else
				AuditMessage.EventIdentification.EventTypeCode = new CodedValueType[] { CodedValueType.Logout };
		}

		/// <summary>
		/// The identity of the person authenticated if successful. Asserted identity if not successful.
		/// </summary>
		/// <param name="participant">The participant.</param>
		public void AddUserParticipant(AuditPersonActiveParticipant participant)
		{
			participant.UserIsRequestor = true;
			InternalAddActiveParticipant(participant);
		}

		/// <summary>
		/// The identity of the node that is authenticating the user. This is to
		/// identify another node that is performing enterprise wide
		/// authentication, e.g. Kerberos authentication.
		/// </summary>
		/// <param name="participant">The participant.</param>
		public void AddNode(AuditProcessActiveParticipant participant)
		{
			participant.UserIsRequestor = false;
			InternalAddActiveParticipant(participant);
		}
	}
}
