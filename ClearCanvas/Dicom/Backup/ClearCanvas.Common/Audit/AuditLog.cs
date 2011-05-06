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
using System.Net;
using System.Threading;

namespace ClearCanvas.Common.Audit
{
	/// <summary>
	/// Defines an extension point for audit sinks.
	/// </summary>
	[ExtensionPoint]
	public class AuditSinkExtensionPoint : ExtensionPoint<IAuditSink>
	{
	}

	/// <summary>
	/// Represents an audit log.
	/// </summary>
	/// <remarks>
	/// 
	/// </remarks>
	public class AuditLog
	{
		private readonly string _application;
		private readonly string _category;
		private readonly IAuditSink[] _sinks;

		#region Constructors
		
		/// <summary>
		/// Constructs an audit log with the specified category.
		/// </summary>
		/// <param name="application"></param>
		/// <param name="category"></param>
		public AuditLog(string application, string category)
			:this(application, category, new []{ CreateSink() })
		{
		}

		/// <summary>
		/// Constructs an audit log with the specified category and sinks.
		/// </summary>
		/// <param name="application"></param>
		/// <param name="category"></param>
		/// <param name="sinks"></param>
		private AuditLog(string application, string category, IAuditSink[] sinks)
		{
			_application = application;
			_category = category;
			_sinks = sinks;			
		}
 
		#endregion	
		
		#region Public API

		/// <summary>
		/// Writes an entry to the audit log containing the specified information,
		/// on behalf of the current application user.
		/// </summary>
		/// <param name="operation"></param>
		/// <param name="details"></param>
		public void WriteEntry(string operation, string details)
		{
			WriteEntry(operation, details, GetUserName(), GetUserSessionId());
		}

		/// <summary>
		/// Writes an entry to the audit log containing the specified information,
		/// on behalf of the specified application user.
		/// </summary>
		/// <param name="operation"></param>
		/// <param name="details"></param>
		/// <param name="user"></param>
		/// <param name="userSessionId"></param>
		public void WriteEntry(string operation, string details, string user, string userSessionId)
		{
			var entry = new AuditEntryInfo(
				_category,
				Platform.Time,
				Dns.GetHostName(),
				_application,
				user,
				userSessionId,
				operation,
				details);

			foreach (var sink in _sinks)
			{
				sink.WriteEntry(entry);
			}
		}

		#endregion

		#region Helpers

		/// <summary>
		/// Gets the identity of the current thread or null if not established.
		/// </summary>
		/// <returns></returns>
		private static string GetUserName()
		{
			var p = Thread.CurrentPrincipal;
			return (p != null && p.Identity != null) ? p.Identity.Name : null;
		}

		/// <summary>
		/// Gets the session token ID of the current thread or null if not established.
		/// </summary>
		/// <returns></returns>
		private static string GetUserSessionId()
		{
			var p = Thread.CurrentPrincipal as IUserCredentialsProvider;
			return (p != null) ? p.SessionTokenId : null;
		}

		/// <summary>
		/// Creates the a single audit sink via the <see cref="AuditSinkExtensionPoint"/>.
		/// </summary>
		/// <returns></returns>
		private static IAuditSink CreateSink()
		{
			try
			{
				return (IAuditSink)(new AuditSinkExtensionPoint()).CreateExtension();
			}
			catch(NotSupportedException)
			{
				//TODO: should there be some kind of default audit sink that just writes to a local log file or something
				throw;
			}
		}

		#endregion
	}
}
