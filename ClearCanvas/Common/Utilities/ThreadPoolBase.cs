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

namespace ClearCanvas.Common.Utilities
{
	/// <summary>
	/// A base class for thread pools that makes no assumptions about how each
	/// thread in the thread pool behaves.
	/// </summary>
	/// <remarks>
	/// It is unlikely you will ever need to derive from this class.  In most
	/// cases, either the <see cref="SimpleBlockingThreadPool"/> or the 
	/// <see cref="BlockingThreadPool{T}"/> will be adequate to suit your needs.
	/// </remarks>
	public abstract class ThreadPoolBase
	{
		/// <summary>
		/// The minimum (and also the default) concurrency for the thread pool.
		/// </summary>
		public static readonly int MinConcurrency = 1;

		/// <summary>
		/// The maximum concurrency for the thread pool.
		/// </summary>
		public static readonly int MaxConcurrency = 100;

		/// <summary>
		/// An enum used to indicate the current running state of the thread pool.
		/// </summary>
		public enum StartStopState
		{
			/// <summary>
			/// The thread pool is starting up.
			/// </summary>
			Starting, 

			/// <summary>
			/// The thread pool is running.
			/// </summary>
			Started, 

			/// <summary>
			/// The thread pool is stopping.
			/// </summary>
			Stopping, 

			/// <summary>
			/// The thread pool has stopped.
			/// </summary>
			Stopped
		};

		#region Private Fields

		private readonly object _startStopSyncLock = new object();
		private StartStopState _state;
		private bool _completeBeforeStop;
		
		private readonly List<Thread> _threads;
		private event EventHandler<ItemEventArgs<StartStopState>> _startStopEvent;

		private int _concurrency = MinConcurrency;
		private ThreadPriority _threadPriority;

		private string _threadPoolName = "Pool";
		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="concurrency">The number of concurrent threads to start.</param>
		protected ThreadPoolBase(int concurrency)
			: this()
		{
			Concurrency = concurrency;
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <remarks>
		/// The default number of threads in the thread pool is 1.
		/// </remarks>
		protected ThreadPoolBase()
		{
			_state = StartStopState.Stopped;
			_completeBeforeStop = false;
			_threads = new List<Thread>();
			_threadPriority = ThreadPriority.Normal;
		}

		/// <summary>
		/// Fired when the <see cref="State"/> has changed.
		/// </summary>
		public event EventHandler<ItemEventArgs<StartStopState>> StartStopStateChangedEvent
		{
			add 
			{
				lock (_startStopSyncLock)
				{
					_startStopEvent += value;
				}
			}
			remove 
			{
				lock (_startStopSyncLock)
				{
					_startStopEvent -= value;
				}
			}
		}
		
		/// <summary>
		/// Gets whether or not the thread pool is currently running.
		/// </summary>
		public bool Active
		{
			get 
			{
				lock (_startStopSyncLock)
				{
					return _state != StartStopState.Stopped;
				}
			}
		}

		/// <summary>
		/// Gets or sets the number of concurrently running threads in the thread pool.
		/// </summary>
		/// <remarks>
		/// This property can only be set when the thread pool is in the <see cref="StartStopState.Stopped"/> state.
		/// </remarks>
		/// <exception cref="InvalidOperationException">Thrown if the thread pool is not in the stopped state when
		/// the property is set.</exception>
		public int Concurrency
		{
			get { return _concurrency; }
			set
			{
				if (Active)
					throw new InvalidOperationException(String.Format(SR.ExceptionThreadPoolMustBeStopped, "Concurrency"));

				Platform.CheckPositive(value, "Concurrency");
				Platform.CheckArgumentRange(value, MinConcurrency, MaxConcurrency, "Concurrency");

				_concurrency = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the thread pool.
		/// </summary>
		/// <remarks>
		/// The name of the thread pool is used when naming the individual threads within the pool.
		/// </remarks>
		public string ThreadPoolName
		{
			get { return _threadPoolName; }
			set { _threadPoolName = value; }
		}

		/// <summary>
		/// Gets or sets the <see cref="ThreadPriority"/> of the threads in the thread pool.
		/// </summary>
		/// <remarks>
		/// This property can only be set when the thread pool is in the <see cref="StartStopState.Stopped"/> state.
		/// </remarks>
		/// <exception cref="InvalidOperationException">Thrown if the thread pool is not in the stopped state when
		/// the property is set.</exception>
		public ThreadPriority ThreadPriority
		{
			get { return _threadPriority; }
			set
			{
				if (Active)
					throw new InvalidOperationException(String.Format(SR.ExceptionThreadPoolMustBeStopped, "ThreadPriority"));

				_threadPriority = value;
			}
		}

		/// <summary>
		/// Indicates whether or not the thread pool should finish processing all items before stopping.
		/// </summary>
		protected bool CompleteBeforeStop
		{
			get 
			{
				lock (_startStopSyncLock)
				{
					return _completeBeforeStop;
				}
			}
		}

		/// <summary>
		/// Gets the current state (<see cref="StartStopState"/>) of the thread pool.
		/// </summary>
		protected StartStopState State
		{
			get
			{
				lock (_startStopSyncLock)
				{
					return _state;
				}
			}
		}

		/// <summary>
		/// Called before the thread pool is started.
		/// </summary>
		/// <remarks>
		/// Inheritors that override this method must first call the base method and 
		/// cannot return true if the base method returns false.
		/// </remarks>
		/// <returns>
		/// False if the thread pool is not in the <see cref="StartStopState.Stopped"/> 
		/// state, and thus cannot be started.
		/// </returns>
		protected virtual bool OnStart()
		{
			//lock other threads out of the Start function by checking/setting the state.
			lock (_startStopSyncLock)
			{
				if (_state != StartStopState.Stopped)
					return false;

				_state = StartStopState.Starting;
				EventsHelper.Fire(_startStopEvent, this, new ItemEventArgs<StartStopState>(_state));
			}

			return true;
		}

		/// <summary>
		/// Called when the thread pool has started.
		/// </summary>
		/// <remarks>
		/// Inheritors that override this method must ensure that the base method gets called
		/// in order for the thread pool to function correctly.
		/// </remarks>
		protected virtual void OnStarted()
		{
			lock (_startStopSyncLock)
			{
				_state = StartStopState.Started;
				EventsHelper.Fire(_startStopEvent, this, new ItemEventArgs<StartStopState>(_state));
			}
		}

		/// <summary>
		/// Called before the thread pool is stopped.
		/// </summary>
		/// <remarks>
		/// Inheritors that override this method must first call the base method and 
		/// cannot return true if the base method returns false.
		/// </remarks>
		/// <returns>
		/// False if the thread pool is not in the <see cref="StartStopState.Started"/> 
		/// state, and thus cannot be stopped.
		/// </returns>
		protected virtual bool OnStop(bool completeBeforeStop)
		{
			//lock other threads out of the Stop function by checking/setting the state.
			lock (_startStopSyncLock)
			{
				if (_state != StartStopState.Started)
					return false;

				_completeBeforeStop = completeBeforeStop;
				_state = StartStopState.Stopping;
				EventsHelper.Fire(_startStopEvent, this, new ItemEventArgs<StartStopState>(_state));
			}
			
			return true;
		}

		/// <summary>
		/// Called when the thread pool has stopped.
		/// </summary>
		/// <remarks>
		/// Inheritors that override this method must ensure that the base method gets called
		/// in order for the thread pool to function correctly.
		/// </remarks>
		protected virtual void OnStopped()
		{
			lock (_startStopSyncLock)
			{
				_state = StartStopState.Stopped;
				EventsHelper.Fire(_startStopEvent, this, new ItemEventArgs<StartStopState>(_state));
			}
		}

		/// <summary>
		/// Starts the thread pool.
		/// </summary>
		/// <remarks>
		/// Repeated calls to this method will do nothing, and no exceptions will be thrown.
		/// </remarks>
		public void Start()
		{
			if (!OnStart())
				return;

			for (int i = 0; i < _concurrency; ++i)
			{
				ThreadStart threadStart = new ThreadStart(this.RunThread);
				Thread thread = new Thread(threadStart);
				thread.Name = String.Format("{0}:{1}", _threadPoolName, thread.ManagedThreadId);
				thread.IsBackground = true;
				thread.Priority = _threadPriority;

				thread.Start();
				_threads.Add(thread);
			}

			OnStarted();
		}

		/// <summary>
		/// Stops the thread pool.
		/// </summary>
		/// <remarks>
		/// Repeated calls to this method will do nothing, and no exceptions will be thrown.
		/// </remarks>
		public void Stop()
		{
			Stop(false);
		}

		/// <summary>
		/// Stops the thread pool, indicating whether or not the thread pool should process
		/// any remaining items first before stopping (via the parameter <param name="completeBeforeStop"/>).
		/// </summary>
		/// <remarks>
		/// Repeated calls to this method will do nothing, and no exceptions will be thrown.
		/// </remarks>
		public void Stop(bool completeBeforeStop)
		{
			if (!OnStop(completeBeforeStop))
				return;

			foreach (Thread thread in _threads)
				thread.Join();

			_threads.Clear();

			OnStopped();
		}

		/// <summary>
		/// The method that each thread in the thread pool will run on.
		/// </summary>
		/// <remarks>
		/// This method must be overridden by inheritors.  The method should not exit until the
		/// <see cref="StartStopState"/> has changed to <see cref="StartStopState.Stopping"/>.
		/// </remarks>
		protected abstract void RunThread();
	}
}