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
using System.Threading;
using ClearCanvas.Common;
using NHibernate;

namespace ClearCanvas.Dicom.DataStore
{
	public sealed partial class DataAccessLayer
	{
		private interface ISessionManager : IDisposable
		{
			ISession Session { get; }
			
			void BeginReadTransaction();
			void BeginWriteTransaction();

			void Rollback();
			void Commit();
		}

		private sealed class SessionManager : ISessionManager
		{
			private static readonly object _syncLock = new object();
			private static readonly List<SessionManager> _sessionManagers = new List<SessionManager>();

			private readonly Thread _thread;
			private ISession _session;
			private int _referenceCount;
			private ITransaction _transaction;
			private bool _writeTransaction;

			private SessionManager()
			{
				_thread = Thread.CurrentThread;
				_referenceCount = 0;
				_writeTransaction = false;
			}

			private Thread Thread
			{
				get { return _thread; }
			}

			private void IncrementReferenceCount()
			{
				Interlocked.Increment(ref _referenceCount);
			}

			private void DecrementReferenceCount()
			{
				Interlocked.Decrement(ref _referenceCount);
			}

			private int ReferenceCount
			{
				get { return Thread.VolatileRead(ref _referenceCount); }
			}

			private void DisconnectSession()
			{
				_writeTransaction = false;

				if (_transaction != null)
				{
					_transaction.Dispose();
					_transaction = null;
				}

				if (_session != null)
				{
					_session.Disconnect();
					_session.Clear();
					_session.Close();
					_session.Dispose();
					_session = null;
				}
			}

			public static ISessionManager Get()
			{
				SessionManager manager;
				lock (_syncLock)
				{
					manager = _sessionManagers.Find(
					   delegate(SessionManager test) { return test.Thread.Equals(Thread.CurrentThread); });

					if (manager == null)
					{
						manager = new SessionManager();
						_sessionManagers.Add(manager);
					}
				}

				manager.IncrementReferenceCount();
				return manager;
			}

			#region ISessionManager Members

			public ISession Session
			{
				get
				{
					if (!Thread.Equals(Thread.CurrentThread))
						throw new DataStoreException("Sessions can only be used from a single thread.");

					if (_session == null)
					{
						_session = SessionFactory.OpenSession();
						_session.FlushMode = FlushMode.Commit;
					}

					return _session;
				}
			}

			public void BeginReadTransaction()
			{
				if (_writeTransaction)
				{
					throw new DataStoreException("All write transactions must be committed before reading.");
				}
				
				if (_transaction == null)
				{
					_transaction = Session.Transaction;
					Session.Transaction.Begin();
				}
			}

			public void BeginWriteTransaction()
			{
				_writeTransaction = true;
				if (_transaction == null)
				{
					_transaction = Session.Transaction;
					Session.Transaction.Begin();
				}
			}

			public void Rollback()
			{
				if (!_writeTransaction)
					return;

				_writeTransaction = false;
				if (_transaction != null)
				{
					_transaction.Rollback();
					_transaction = null;
				}
			}

			public void Commit()
			{
				if (!_writeTransaction)
					return;

				_writeTransaction = false;
				if (_transaction != null)
				{
					_transaction.Commit();
					_transaction = null;
				}
			}

			#endregion

			#region IDisposable Members

			public void Dispose()
			{
				try
				{
					DecrementReferenceCount();
					if (ReferenceCount <= 0)
					{
						lock (_syncLock)
						{
							_sessionManagers.Remove(this);
						}

						DisconnectSession();
					}
				}
				catch (Exception e)
				{
					Platform.Log(LogLevel.Error, e);
				}
			}

			#endregion
		}
	}
}