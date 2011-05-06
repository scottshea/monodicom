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
using ClearCanvas.Common;
using NHibernate;

namespace ClearCanvas.Dicom.DataStore
{
	public sealed partial class DataAccessLayer
	{
		private abstract class SessionConsumer : IDisposable
		{
			private ISessionManager _sessionManager;

			protected SessionConsumer(ISessionManager sessionManager)
			{
				_sessionManager = sessionManager;
			}

			protected ISessionManager SessionManager
			{
				get { return _sessionManager; }
			}

			protected ISession Session
			{
				get { return SessionManager.Session; }
			}


			protected virtual void Dispose(bool disposing)
			{
				if (_sessionManager != null)
				{
					_sessionManager.Dispose();
					_sessionManager = null;
				}
			}

			#region IDisposable Members

			public void Dispose()
			{
				try
				{
					Dispose(true);
					GC.SuppressFinalize(this);
				}
				catch (Exception e)
				{
					Platform.Log(LogLevel.Warn, e);
				}
			}

			#endregion
		}
	}
}