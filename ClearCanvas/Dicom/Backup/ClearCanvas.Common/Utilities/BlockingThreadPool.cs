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
using System.Threading;

namespace ClearCanvas.Common.Utilities
{
	#region ItemProcessingThreadPool class

	/// <summary>
	/// A strongly typed delegate for use with <see cref="ItemProcessingThreadPool{T}"/>.
	/// </summary>
	/// <param name="item">A specific item to be passed to the delegate.</param>
	public delegate void ProcessItemDelegate<T>(T item);

	/// <summary>
	/// A class used with <see cref="ItemProcessingThreadPool{T}"/> to pass an item to a delgate
	/// </summary>
	public class ProcessItemCommand<T>
	{
		private readonly T _parameter;
		private readonly ProcessItemDelegate<T> _del;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parameter">The item to pass to <paramref name="del"/></param>
		/// <param name="del">The delegate to call.</param>
		public ProcessItemCommand(T parameter, ProcessItemDelegate<T> del)
		{
			_parameter = parameter;
			_del = del;
		}

		/// <summary>
		/// Execute the delegate
		/// </summary>
		public void Execute()
		{
			_del(_parameter);
		}
	}

	/// <summary>
	/// An implementation of <see cref="BlockingThreadPool{T}"/> that processes <see cref="ProcessItemDelegate{T}"/>s.
	/// </summary>
	/// <remarks>
	/// <para>
	/// ItemProcessingThreadPool allows for the creation of a thread pools for passing a strongly typed
	/// item to the delegate.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">A specific type for passing to the delegate.</typeparam>
	public class ItemProcessingThreadPool<T> : BlockingThreadPool<ProcessItemCommand<T>>
	{
		/// <summary>
		/// Adds an item to the queue.
		/// </summary>
		/// <param name="del">The delegate to enqueue.</param>
		/// <param name="parameter">The parameter to pass to the delegate when it is executed.</param>
		public void Enqueue(T parameter, ProcessItemDelegate<T> del)
		{
			Enqueue(new ProcessItemCommand<T>(parameter, del));
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="concurrency">Defines the number of concurrent threads that will process enqueued items.</param>
		/// <param name="allowInactiveAdd">Specifies whether or not items can be added while the threads are not running.</param>
		public ItemProcessingThreadPool(int concurrency, bool allowInactiveAdd)
			: base(concurrency, allowInactiveAdd)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="concurrency">Defines the number of concurrent threads that will process enqueued items.</param>
		public ItemProcessingThreadPool(int concurrency)
			: base(concurrency)
		{
		}

		/// <summary>
		/// Protected constructor.
		/// </summary>
		protected ItemProcessingThreadPool()
		{
		}

		/// <summary>
		/// Called by one of the threads in the pool when a <see cref="ProcessItemDelegate{T}"/> is about to be processed.
		/// </summary>
		/// <param name="parm">The delegate that will be executed by this method call.</param>
		protected override void ProcessItem(ProcessItemCommand<T> parm)
		{
			//execute the delegate.
			parm.Execute();
		}
	}
	#endregion

	#region SimpleBlockingThreadPool class

	/// <summary>
	/// A simple delegate for use in a <see cref="SimpleBlockingThreadPool"/>.
	/// </summary>
	public delegate void SimpleBlockingThreadPoolDelegate();

	/// <summary>
	/// An implementation of <see cref="BlockingThreadPool{T}"/> that processes <see cref="SimpleBlockingThreadPoolDelegate"/>s.
	/// </summary>
	public class SimpleBlockingThreadPool : BlockingThreadPool<SimpleBlockingThreadPoolDelegate>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="concurrency">Defines the number of concurrent threads that will process enqueued items.</param>
		/// <param name="allowInactiveAdd">Specifies whether or not items can be added while the threads are not running.</param>
		public SimpleBlockingThreadPool(int concurrency, bool allowInactiveAdd)
			: base(concurrency, allowInactiveAdd)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="concurrency">Defines the number of concurrent threads that will process enqueued items.</param>
		public SimpleBlockingThreadPool(int concurrency)
			: base(concurrency)
		{
		}

		/// <summary>
		/// Protected constructor.
		/// </summary>
		protected SimpleBlockingThreadPool()
		{
		}

		/// <summary>
		/// Called by one of the threads in the pool when a <see cref="SimpleBlockingThreadPoolDelegate"/> is about to be processed.
		/// </summary>
		/// <param name="del">The delegate that will be executed by this method call.</param>
		protected override void ProcessItem(SimpleBlockingThreadPoolDelegate del)
		{
			//execute the delegate.
			del();
		}
	}

	#endregion

	/// <summary>
	/// A blocking thread pool.
	/// </summary>
	/// <remarks>
	/// This class uses a <see cref="BlockingQueue{T}"/> internally and processes
	/// items of type <typeparamref name="T"/> concurrently on multiple threads.
	/// </remarks>
	/// <typeparam name="T">The type of object to be processed by the thread pool.</typeparam>
	public abstract class BlockingThreadPool<T> : ThreadPoolBase
		where T : class
	{
		private readonly BlockingQueue<T> _blockingQueue;
		private bool _allowInactiveAdd;
		private volatile int _sleepTime;
		private int _activeCount = 0;
		private readonly object _syncLock = new object();

		/// <summary>
		/// Protected constructor.
		/// </summary>
		/// <param name="concurrency">Defines the number of concurrent threads that will process enqueued items.</param>
		/// <param name="allowInactiveAdd">Specifies whether or not items can be added while the threads are not running.</param>
		protected BlockingThreadPool(int concurrency, bool allowInactiveAdd)
			: base(concurrency)
		{
			_blockingQueue = new BlockingQueue<T>();
			_allowInactiveAdd = allowInactiveAdd;
			_sleepTime = 0; //will at least give up the remainder of the time slice.
		}

		/// <summary>
		/// Protected constructor.
		/// </summary>
		/// <param name="concurrency">Defines the number of concurrent threads that will process enqueued items.</param>
		protected BlockingThreadPool(int concurrency)
			: this(concurrency, false)
		{ 
		}

		/// <summary>
		/// Protected constructor.
		/// </summary>
		protected BlockingThreadPool()
			: this(MinConcurrency, false)
		{
		}

		/// <summary>
		/// Specifies whether or not items to be processed can be added while the thread pool is not running.
		/// </summary>
		public bool AllowInactiveAdd
		{
			get { return _allowInactiveAdd; }
			set
			{
				if (Active)
					throw new InvalidOperationException(String.Format(SR.ExceptionThreadPoolMustBeStopped, "AllowInactiveAdd"));

				_allowInactiveAdd = value;
			}
		}

		/// <summary>
		/// Specifies an amount of time, in milliseconds, for each thread to sleep between processing items.
		/// </summary>
		public int SleepTimeMilliseconds
		{
			get { return _sleepTime; }
			set { _sleepTime = value; }
		}

		/// <summary>
		/// Gets the number of items remaining in the queue.
		/// </summary>
		public int QueueCount
		{
			get { return _blockingQueue.Count; }
		}

		/// <summary>
		/// The current number of active threads in the queue.
		/// </summary>
		public int ActiveCount
		{
			get
			{
				lock (_syncLock)
				{
					return _activeCount;
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
		/// False if the thread pool is not in the <see cref="ThreadPoolBase.StartStopState.Stopped"/> 
		/// state, and thus cannot be started.
		/// </returns>
		protected override bool OnStart()
		{
			if (!base.OnStart())
				return false;

			_blockingQueue.ContinueBlocking = true;
			return true;
		}

		/// <summary>
		/// Called before the thread pool is stopped.
		/// </summary>
		/// <remarks>
		/// Inheritors that override this method must first call the base method and 
		/// cannot return true if the base method returns false.
		/// </remarks>
		/// <returns>
		/// False if the thread pool is not in the <see cref="ThreadPoolBase.StartStopState.Started"/> 
		/// state, and thus cannot be stopped.
		/// </returns>
		protected override bool OnStop(bool completeBeforeStop)
		{
			if (!base.OnStop(completeBeforeStop))
				return false;

			_blockingQueue.ContinueBlocking = false;
			return true;
		}

		/// <summary>
		/// Adds an item of type <typeparamref name="T"/> to the queue.
		/// </summary>
		/// <param name="item"></param>
		public void Enqueue(T item)
		{
			if (!_allowInactiveAdd && !Active)
				throw new InvalidOperationException(SR.ExceptionThreadPoolNotStarted);

			_blockingQueue.Enqueue(item);
		}

		/// <summary>
		/// The method that each thread in the thread pool will run on.
		/// </summary>
		protected sealed override void RunThread()
		{
			while (true)
			{
				T item;
				bool queueEmpty = !_blockingQueue.Dequeue(out item);

				if (!queueEmpty)
				{
					lock (_syncLock) { _activeCount++; }
					try
					{
						ProcessItem(item);
					}
					catch (Exception e)
					{
						Platform.Log(LogLevel.Error, e, "Unexpected exception processing thread pool item.");
					}
					finally
					{
						lock (_syncLock) { _activeCount--; }
					}
				}
				if (State == StartStopState.Stopping)
				{
					if (queueEmpty || !CompleteBeforeStop)
						break;
				}

				Thread.Sleep(_sleepTime);
			}
		}

		/// <summary>
		/// Inheritors must override this method in order to perform processing on items.
		/// </summary>
		/// <remarks>
		/// This method is called from within the <see cref="RunThread"/> 
		/// method to process each item that has been taken from the queue.
		/// </remarks>
		protected abstract void ProcessItem(T item);
	}
}
