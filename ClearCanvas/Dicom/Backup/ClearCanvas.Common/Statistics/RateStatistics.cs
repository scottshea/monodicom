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
using System.Diagnostics;

namespace ClearCanvas.Common.Statistics
{
    /// <summary>
    /// The type of information used in <see cref="RateStatistics"/>.
    /// </summary>
    public enum RateType
    {
        ///<summary>
        /// Rate statistics in number of bytes within a period
        ///</summary>
        BYTES, 
        /// <summary>
        /// Rate statistics in number of message within a period
        /// </summary>
        MESSAGES,

		/// <summary>
		/// Custom rate statistics.
		/// </summary>
        CUSTOM
    } ;

    /// <summary>
    /// Statistics class to store the rate of changes of the underlying information.
    /// </summary>
    /// <remarks>
    /// The information being supported include: Byte rates or Message rate. The number of bytes or messages is set by calling <see cref="SetData"/>.
    /// The rate will be calculated based on the value set by calling <see cref="SetData"/> in between <see cref="Start"/> and <see cref="End"/> calls.
    /// 
    /// <see cref="IStatistics.FormattedValue"/> of the <see cref="RateStatistics"/> has unit of "GB/s", "MB/s", "KB/s" or "msg/s"
    /// depending on type of value specified in the constructor.
    /// 
    /// 
    /// <example>
    /// <code>
    ///     RateStatistics transferSpeed = new RateStatistics("Speed", RateType.BYTES);
    ///     transferSpeed.Begin();
    ///     transferSpeed.SetData(2408);
    ///     transferSpeed.End();    
    /// </code>
    /// 
    /// <para>If the time elapsed between Begin() and End() is one second, then transferSpeed.FormattedValue = "2 KB/s"</para>
    /// <para>If the time elapsed is 5 seconds, then transferSpeed.FormattedValue = "0.4 KB/s"</para>
    /// 
    /// </example>
    /// 
    /// 
    /// 
    /// </remarks>
    public class RateStatistics : Statistics<double>
    {

        #region private members

        private double _data = 0;
        private readonly Stopwatch _stopWatch = new Stopwatch();
        private RateType _type;

        #endregion private members

        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="RateStatistics"/> for the specified <see cref="RateType"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rateType"></param>
        public RateStatistics(string name, RateType rateType)
            : base(name)
        {
            Type = rateType;

            switch (Type)
            {
                case RateType.BYTES:
                    ValueFormatter = TransmissionRateFormatter.Format;
                    break;

                case RateType.MESSAGES:
                    ValueFormatter = MessageRateFormatter.Format;
                    break;
            }
        }

		/// <summary>
		///Creates an instance of <see cref="RateStatistics"/> for type <see cref="RateType.CUSTOM"/>.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="unit"></param>
        public RateStatistics(string name, string unit)
            : base(name)
        {
            Type = RateType.CUSTOM;

            ValueFormatter = delegate(double rate)
                                 {
                                     return String.Format("{0:0.0} {1}/s", rate, unit);
                                 };
        }

        /// <summary>
        /// Creates a copy of the original <see cref="RateStatistics"/> object.
        /// </summary>
        /// <param name="source">The original <see cref="RateStatistics"/> to copy </param>
        public RateStatistics(RateStatistics source)
            : base(source)
        {
            Type = source.Type;
        }

        /// <summary>
        /// Gets or sets the type of the rate being measured
        /// </summary>
        public RateType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        #endregion Constructors
		/// <summary>
		/// The rate value.
		/// </summary>
        public override double Value
        {
            get
            {
                if (_stopWatch.ElapsedTicks > 0)
                    return _data / ((double)_stopWatch.ElapsedTicks / Stopwatch.Frequency);
                else
                    return 0;
            }
            set
            {
                base.Value = value;
            }
        }

        #region Public methods

        /// <summary>
        /// Gets the elapsed time being measured, in ticks.
        /// </summary>
        public long ElapsedTicks
        {
            get { return _stopWatch.ElapsedTicks; }
        }

        /// <summary>
        /// Gets the elapsed time being measured, in ticks.
        /// </summary>
        public TimeSpan ElapsedTime
        {
            get { return _stopWatch.Elapsed; }
        }


        /// <summary>
        /// Sets the value of the underlying data.
        /// </summary>
        /// <param name="value"></param>
        public void SetData(double value)
        {
            _data = value;
        }

        /// <summary>
        /// Signals the begining of the measurement.
        /// </summary>
        public void Start()
        {
            _stopWatch.Start();
        }

        /// <summary>
        /// Signals the end of the measurement.
        /// </summary>
        public void End()
        {
            Debug.Assert(_stopWatch.IsRunning);

            _stopWatch.Stop();
            Debug.Assert(_stopWatch.ElapsedTicks > 0);
        }

        #endregion

        #region Overridden Public Methods

        /// <summary>
        /// Creates a copy of the current statistics
        /// </summary>
        /// <returns>A copy of the current <see cref="RateStatistics"/> object</returns>
        public override object Clone()
        {
            return new RateStatistics(this);
        }

        /// <summary>
        /// Returns a new average statistics object corresponding to the current statistics
        /// </summary>
        /// <returns>A <see cref="AverageRateStatistics"/> object</returns>
        public override IAverageStatistics NewAverageStatistics()
        {
            return new AverageRateStatistics(this);
        }

        #endregion
    }
}