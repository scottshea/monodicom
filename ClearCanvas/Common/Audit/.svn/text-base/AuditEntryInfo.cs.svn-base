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
using System.Runtime.Serialization;
using System.Text;

namespace ClearCanvas.Common.Audit
{
	/// <summary>
	/// Contains all information about an audit log entry.
	/// </summary>
	[DataContract]
	public class AuditEntryInfo
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="category"></param>
		/// <param name="timeStamp"></param>
		/// <param name="hostName"></param>
		/// <param name="application"></param>
		/// <param name="user"></param>
		/// <param name="userSessionId"></param>
		/// <param name="operation"></param>
		/// <param name="details"></param>
		public AuditEntryInfo(string category, DateTime timeStamp, string hostName, string application, string user, string userSessionId, string operation, string details)
		{
			TimeStamp = timeStamp;
			HostName = hostName;
			Application = application;
			User = user;
			UserSessionId = userSessionId;
			Category = category;
			Operation = operation;
			Details = details;
		}

		/// <summary>
		/// Gets or sets the time at which this log entry was created.
		/// </summary>
		[DataMember]
		public DateTime TimeStamp { get; private set; }

		/// <summary>
		/// Gets or sets the hostname of the computer that generated this log entry.
		/// </summary>
		[DataMember]
		public string HostName { get; private set; }

		/// <summary>
		/// Gets or sets the the name of the application that created this log entry.
		/// </summary>
		[DataMember]
		public string Application { get; private set; }

		/// <summary>
		/// Gets or sets the user of the application on whose behalf this log entry was created.
		/// </summary>
		[DataMember]
		public string User { get; private set; }

		/// <summary>
		/// Gets or sets the user session ID on whose behalf this log entry was created.
		/// </summary>
		[DataMember(IsRequired = false)]
		public string UserSessionId { get; private set; }

		/// <summary>
		/// Gets or sets the name of the operation that caused this log entry to be created.
		/// </summary>
		[DataMember]
		public string Operation { get; private set; }

		/// <summary>
		/// Gets or sets the contents of this log entry, which may be text or XML based.
		/// </summary>
		[DataMember]
		public string Details { get; private set; }

		/// <summary>
		/// Gets or sets the category of this audit log entry.
		/// </summary>
		[DataMember]
		public string Category { get; private set; }
	}
}
