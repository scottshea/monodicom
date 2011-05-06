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
	/// Enum for use with <see cref="ApplicationActivityAuditHelper"/>.
	/// </summary>
	public enum ApplicationActivityType
	{
		ApplicationStarted,
		ApplicationStopped
	}

	/// <summary>
	/// Helper for Application Activity Audit Log
	/// </summary>
	/// <remarks>
	/// This audit message describes the event of an Application Entity starting or stoping.
	/// </remarks>
	public class ApplicationActivityAuditHelper : DicomAuditHelper
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="auditSource"></param>
		/// <param name="outcome"></param>
		/// <param name="type"></param>
		/// <param name="idOfApplicationStarted">Add the ID of the Application Started, should be called once.</param>
		public ApplicationActivityAuditHelper(DicomAuditSource auditSource,
			EventIdentificationTypeEventOutcomeIndicator outcome, 
			ApplicationActivityType type,
			AuditProcessActiveParticipant idOfApplicationStarted) : base("ApplicationActivity")
		{
			AuditMessage.EventIdentification = new EventIdentificationType();
			AuditMessage.EventIdentification.EventID = CodedValueType.ApplicationActivity;
			AuditMessage.EventIdentification.EventActionCode = EventIdentificationTypeEventActionCode.E;
			AuditMessage.EventIdentification.EventActionCodeSpecified = true;
			AuditMessage.EventIdentification.EventDateTime = Platform.Time.ToUniversalTime();
			AuditMessage.EventIdentification.EventOutcomeIndicator = outcome;

			InternalAddAuditSource(auditSource);

			if (type == ApplicationActivityType.ApplicationStarted)
				AuditMessage.EventIdentification.EventTypeCode = new CodedValueType[] { CodedValueType.ApplicationStart };
			else
				AuditMessage.EventIdentification.EventTypeCode = new CodedValueType[] { CodedValueType.ApplicationStop };

			idOfApplicationStarted.UserIsRequestor = false;
			idOfApplicationStarted.RoleIdCode = CodedValueType.Application;

			InternalAddActiveParticipant(idOfApplicationStarted);

		}

		/// <summary>
		/// Add the ID of person or process that started or stopped the Application.  Can be called multiple times.
		/// </summary>
		/// <param name="participant">The participant.</param>
		public void AddUserParticipant(AuditActiveParticipant participant)
		{
			participant.RoleIdCode = CodedValueType.ApplicationLauncher;
			participant.UserIsRequestor = true;

			InternalAddActiveParticipant(participant);
		}
	}
}
