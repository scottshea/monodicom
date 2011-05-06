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

namespace ClearCanvas.Common.Shreds
{
    /// <summary>
	/// Specialization of <see cref="Shred"/> for running code packaged in <see cref="QueueProcessor"/>
    /// derived classes.
    /// </summary>
    /// <remarks>
    /// This class must be sub-classed, and the <see cref="GetProcessors"/> method overridden
    /// to return the set of processors to execute (a single instance of this class may run multiple
    ///  queue processors in parallel).
    /// </remarks>
	public abstract class QueueProcessorShred : Shred
	{
		private bool _isStarted = false;
		private readonly List<QueueProcessor> _processors = new List<QueueProcessor>();
        private readonly List<Thread> _processorThreads = new List<Thread>();

        private readonly TimeSpan _shutDownTimeOut = new TimeSpan(0, 0, 60);

        /// <summary>
        /// Obtains a set of processors to be executed by this shred.
        /// </summary>
        /// <remarks>
        /// This method will be called every time the shred is started.  Subsequent
        /// invocations need not return the same processor instances.
        /// </remarks>
        /// <returns></returns>
		protected abstract IList<QueueProcessor> GetProcessors();

		#region Shred overrides

    	/// <summary>
    	/// Called to start the shred.
    	/// </summary>
    	public override void Start()
		{
            if (!_isStarted)
			{
				StartUp();
			}
		}

    	/// <summary>
    	/// Called to stop the shred.
    	/// </summary>
    	public override void Stop()
		{
            if (_isStarted)
			{
				ShutDown();
			}
		}

		#endregion

        #region Helpers

        /// <summary>
        /// Starts all processors returned by <see cref="GetProcessors"/>.  This method is transactional:
        /// either all processors start, or none do (those that have already started are stopped). This method
        /// will not throw under any circumstances.  Failure is silent. (TODO: is this desirable??)
        /// </summary>
        private void StartUp()
		{
            if (_isStarted)
                return;

            // set flag immediately so we can't be re-entered
            _isStarted = true;

            Platform.Log(LogLevel.Info, string.Format(SR.ShredStarting, this.GetDisplayName()));

            _processorThreads.Clear();
            _processors.Clear();

			try
			{

                // attempt to start all processors - if any throws an exception, abort
				foreach (QueueProcessor processor in GetProcessors())
				{
                    Thread thread = StartProcessorThread(processor);

                    // if thread started successfully, add to lists
                    _processorThreads.Add(thread);
                    _processors.Add(processor);
				}

				Platform.Log(LogLevel.Info, string.Format(SR.ShredStartedSuccessfully, this.GetDisplayName()));
			}
			catch (Exception e)
			{
				Platform.Log(LogLevel.Error, e, string.Format(SR.ShredFailedToStart, this.GetDisplayName()));

                // stop any processors that have already started
                ShutDown();
			}
		}

        /// <summary>
        /// Stops all running processors.  This method is guaranteed to succeed,
        /// in the sense that it will not throw under any circumstances.  However,
        /// if a processor thread does not stop within the specified time out, it 
        /// will be abandoned.
        /// </summary>
		private void ShutDown()
		{
            if (!_isStarted)
                return;

			Platform.Log(LogLevel.Info, string.Format(SR.ShredStopping, this.GetDisplayName()));

            // request all processors to stop
			foreach (QueueProcessor processor in _processors)
			{
                try
                {
                    // ask processor to stop
                    // this call is not expected to throw under any circumstances
                    processor.RequestStop();
                }
                catch (Exception e)
                {
                    // in case it throws for some reason, although it is not supposed to
                    Platform.Log(LogLevel.Error, e);
                }
            }

            // wait for all processor threads to stop
            foreach (Thread thread in _processorThreads)
            {
                try
                {
                    thread.Join(_shutDownTimeOut);
                }
                catch (ThreadStateException e)
                {
                    // can get here if the thread was not started properly
                    Platform.Log(LogLevel.Error, e);
                }
            }

            Platform.Log(LogLevel.Info, string.Format(SR.ShredStoppedSuccessfully, this.GetDisplayName()));

            // only set the flag here
            _isStarted = false;
		}

        /// <summary>
        /// Starts the specified processor on a dedicated thread, and returns the
        /// <see cref="Thread"/> object.
        /// </summary>
        /// <param name="processor"></param>
        /// <returns></returns>
		private static Thread StartProcessorThread(QueueProcessor processor)
        {
            Thread thread = new Thread(
                delegate()
                {
                    try
                    {
                        processor.Run();
                    }
                    catch (Exception e)
                    {
                        Platform.Log(LogLevel.Error, e);
                    }
                });

            thread.Start();
            return thread;
        }

        #endregion

    }
}
