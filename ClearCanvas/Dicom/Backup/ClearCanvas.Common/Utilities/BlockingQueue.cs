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
    /// Basic producer-consumer queue, taken from here:
    /// http://blogs.msdn.com/toub/archive/2006/04/12/575103.aspx 
	/// but slightly modified so the thread being blocked can exit and re-enter.
    /// </summary>
    /// <typeparam name="T">The type to be used in the queue.</typeparam>
    public class BlockingQueue<T>
    {
		private object _syncLock = new object();
		private Queue<T> _queue;
		private bool _continueBlocking;

		/// <summary>
		/// Constructor.
		/// </summary>
		public BlockingQueue()
		{
			 _queue = new Queue<T>();
			_continueBlocking = true;
		}

		/// <summary>
		/// Removes the item at the head of the queue.
		/// </summary>
		/// <remarks>
		/// <para>
		/// If no items are available, this call will block until an item becomes available, 
		/// unless the <see cref="ContinueBlocking"/> member has been set to false.
		/// </para>
		/// <para>
		/// This method will not throw an exception.
		/// </para>
		/// </remarks>
		/// <param name="value">The value of the next item in the queue, or <b>default(T)</b> 
		/// if <see cref="ContinueBlocking"/> is false and the queue is empty.</param>
		/// <returns>True if the item returned (via the out parameter) was in the queue, otherwise false.</returns>
		public bool Dequeue(out T value)
		{
			value = default(T);

			lock (_syncLock)
			{
				while (_continueBlocking && _queue.Count == 0)
					Monitor.Wait(_syncLock);

				if (_queue.Count == 0)
					return false;

				value = _queue.Dequeue();
			}

			return true;
		}
		
		/// <summary>
        /// Adds the specified item to the end of the queue.
        /// </summary>
		/// <exception cref="ArgumentNullException">Thrown when the input item is null.</exception>
		/// <param name="item">The item to enqueue.</param>
        public void Enqueue(T item)
        {
			Platform.CheckForNullReference(item, "item");
            lock (_syncLock)
            {
				_queue.Enqueue(item);
				Monitor.Pulse(_syncLock);
            }
        }

		/// <summary>
		/// Indicates whether or not the <b>Dequeue</b> methods should block until the queue
		/// becomes non-empty.
		/// </summary>
		/// <remarks>
		/// When set to false, all actively waiting threads 
		/// (e.g. currently blocked, calling <b>Dequeue</b>) are  released so they 
		/// can determine whether or not they should quit.
		/// </remarks>
		public bool ContinueBlocking
		{
			get 
			{
				lock (_syncLock)
				{
					return _continueBlocking;
				}
			}
			set
			{
				lock (_syncLock)
				{
					_continueBlocking = value;
					if (!_continueBlocking)
					{
						//release all waiting threads.
						Monitor.PulseAll(_syncLock);
					}
				}
			}
		}

		/// <summary>
		/// Returns the number of items remaining in the queue.
		/// </summary>
		public int Count
		{
			get
			{
				lock (_syncLock)
				{
					return _queue.Count;
				}
			}
		}
	}
}
