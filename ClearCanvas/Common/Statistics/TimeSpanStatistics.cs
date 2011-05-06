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

#pragma warning disable 1591

using System;
using System.Diagnostics;

namespace ClearCanvas.Common.Statistics
{
    /// <summary>
    /// Statistics class to track a timespan.
    /// </summary>
    /// <remarks>
    /// <see cref="IStatistics.FormattedValue"/> of the <see cref="TimeSpanStatistics"/> has unit of "sec", "ms", "min", "hr" depending on the elapsed time
    /// between <see cref="Start"/> and <see cref="End"/> calls. The number of ticks between these calls is stored in <see cref="Statistics{T}.Value"/>.
    /// 
    /// <example>
    /// <code>
    ///     TimeSpanStatistics ts = new TimeSpanStatistics("Time");
    ///     transferSpeed.Begin();
    ///     ....
    ///     transferSpeed.End();
    /// </code>
    /// 
    /// <para>If the time elapsed between Begin() and End() is 90 second, then ts.FormattedValue = "1 min 30 sec"</para>
    /// <para>If the time elapsed is 300 miliseconds, then ts.FormattedValue = "300ms"</para>
    /// 
    /// <para>
    /// <see cref="TimeSpanStatistics"/> also supports <see cref="Add(TimeSpanStatistics)"/> and <see cref="Subtract(TimeSpanStatistics)"/> for timespan 
    /// arithmetics.
    /// </para>
    /// 
    /// </example>
    /// 
    /// 
    /// 
    /// </remarks>
    /// 
    public class TimeSpanStatistics : Statistics<TimeSpan>
    {
        #region Private members

        private readonly Stopwatch _stopWatch = new Stopwatch();

        #endregion Private members

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="TimeSpanStatistics"/>
        /// </summary>
        public TimeSpanStatistics()
            : this("TimeSpanStatistics")
        {
            Value = TimeSpan.Zero;
        }

        /// <summary>
        /// Creates an instance of <see cref="TimeSpanStatistics"/> with a specified name.
        /// </summary>
        /// <param name="name">Name of the <see cref="TimeSpanStatistics"/> instance to be created</param>
        public TimeSpanStatistics(string name)
            : base(name)
        {
            Value = TimeSpan.Zero;

            ValueFormatter = TimeSpanFormatter.Format;
        }

        /// <summary>
        /// Creates a copy of the orginal <see cref="TimeSpanStatistics"/> object.
        /// </summary>
        /// <param name="copy"></param>
        public TimeSpanStatistics(TimeSpanStatistics copy)
            : base(copy)
        {
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets a value indiating whether or not the value of the statistics has been set.
        /// </summary>
        public bool IsSet
        {
            get { return Value != TimeSpan.Zero; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the statistics is still being recorded 
        /// </summary>
        /// <remarks>
        /// After after <see cref="Start"/> is called, the statistics will be in running state until <see cref="End"/> is called.
        /// </remarks>
        public bool IsRunning
        {
            get { return _stopWatch.IsRunning; }
        }

        /// <summary>
        /// Gets the total elapsed time measured, in timer ticks.
        /// </summary>
        public long ElapsedTick
        {
            get { return _stopWatch.ElapsedTicks; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Resets the current statistics.
        /// </summary>
        public void Reset()
        {
            Value = TimeSpan.Zero;
            _stopWatch.Reset();

            Debug.Assert(IsSet == false);
            Debug.Assert(IsRunning == false);
        }

        /// <summary>
        /// Adds a duration to the current <see cref="TimeSpanStatistics"/>.
        /// </summary>
        /// <param name="duration"></param>
        /// <remarks>
        /// Duration set using <see cref="Add(TimeSpanStatistics)"/> will
        /// be lost if <see cref="Start"/> and <see cref="End"/> are called.
        /// </remarks>
        public void Add(TimeSpanStatistics duration)
        {
            Platform.CheckForNullReference(duration, "duration");
            Value = Value.Add(duration.Value);
        }

        /// <summary>
        /// Executes and measures the duration of the specified call.
        /// </summary>
        /// <param name="codeDelegate">a delegate to an executation block</param>
        /// <remarks>
        /// This method provides a convenient mean to capture the elapsed time of the code block. 
        /// </remarks>
        public void Add(TimeSpanStatisticsHelper.ExecutationBlock codeDelegate)
        {
            try
            {
                Start();
                codeDelegate();
            }
            finally
            {
                End();
            }
        }

        /// <summary>
        /// Adds a number of ticks to the current <see cref="TimeSpanStatistics"/>.
        /// </summary>
        /// <param name="ticks"></param>
        /// <remarks>
        /// Duration set using <see cref="Add(long)"/> will
        /// be lost if <see cref="Start"/> and <see cref="End"/> are called.
        /// </remarks>
        public void Add(long ticks)
        {
            Value = Value.Add(new TimeSpan(ticks));
        }


        /// <summary>
        /// Subtract a number of ticks from the current <see cref="TimeSpanStatistics"/>.
        /// </summary>
        /// <param name="ticks"></param>
        /// <remarks>
        /// Duration set using <see cref="Subtract(long)"/> will
        /// be lost if <see cref="Start"/> and <see cref="End"/> are called.
        /// </remarks>
        public void Subtract(long ticks)
        {
            Value = Value.Subtract(new TimeSpan(ticks));
        }

        /// <summary>
        /// Adds a duration to the current <see cref="TimeSpanStatistics"/>.
        /// </summary>
        /// <param name="duration">The duration </param>
        /// <remarks>
        /// Duration set using <see cref="Subtract(TimeSpanStatistics)"/> will
        /// be lost if <see cref="Start"/> and <see cref="End"/> are called.
        /// </remarks>
        public void Subtract(TimeSpanStatistics duration)
        {
            Platform.CheckForNullReference(duration, "duration");
            Value = Value.Subtract(duration.Value);
        }

        /// <summary>
        /// Signals the beginning of the period to be measured.
        /// </summary>
        public void Start()
        {
            _stopWatch.Start();
        }

        /// <summary>
        /// Signals the end of the period to be measured.
        /// </summary>
        /// <remarks>
        /// Any value set using <see cref="Add(TimeSpanStatistics)"/> and <see cref="Subtract(TimeSpanStatistics)"/> will
        /// be lost. The <see cref="Statistics{T}.Value"/>  will be the elapsed time between <see cref="Start"/> and <see cref="End"/>
        /// </remarks>
        public void End()
        {
            _stopWatch.Stop();
            Debug.Assert(_stopWatch.ElapsedTicks > 0);
            Value = _stopWatch.Elapsed;
        }

        public override TimeSpan Value
        {
            get
            {
                if (_stopWatch.IsRunning)
                    return _stopWatch.Elapsed;
                else
                    return base.Value;
            }
            set
            {
                if (_stopWatch.IsRunning)
                    _stopWatch.Stop();

                base.Value = value;
            }
        }

        #endregion

        #region Overridden Public Methods

        public override IAverageStatistics NewAverageStatistics()
        {
            return new AverageTimeSpanStatistics(this);
        }

        public override object Clone()
        {
            TimeSpanStatistics copy = new TimeSpanStatistics(this);

            return copy;
        }

        #endregion
    }
}