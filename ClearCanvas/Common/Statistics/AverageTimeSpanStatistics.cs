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
    /// Average time span statistics.
    /// </summary>
    public class AverageTimeSpanStatistics : AverageStatistics<TimeSpan>
    {
        #region Constructors

        /// <summary>
        /// Creates an instance of <see cref="AverageTimeSpanStatistics"/>
        /// </summary>
        public AverageTimeSpanStatistics()
            : this("AverageTimeSpanStatistics")
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="AverageTimeSpanStatistics"/> with specified name.
        /// </summary>
        /// <param name="name"></param>
        public AverageTimeSpanStatistics(string name)
            : base(name)
        {
            Value = new TimeSpan();
            ValueFormatter = TimeSpanFormatter.Format;
        }

        /// <summary>
        /// Creates a copy of the original <see cref="AverageTimeSpanStatistics"/> object.
        /// </summary>
        /// <param name="source"></param>
        public AverageTimeSpanStatistics(TimeSpanStatistics source)
            : base(source)
        {
        }

        #endregion

        #region Overridden Public Methods

        /// <summary>
        /// Adds a sample to the <see cref="AverageStatistics{T}.Samples"/> list.
        /// </summary>
        /// <typeparam name="TSample">Type of the sample value to be inserted</typeparam>
        /// <param name="sample"></param>
        public override void AddSample<TSample>(TSample sample)
        {
            if (sample is TimeSpan)
            {
                TimeSpan ts = (TimeSpan) (object) sample;
                Samples.Add(new TimeSpan(ts.Ticks));
                NewSamepleAdded = true;
            }
            else if (sample is TimeSpanStatistics)
            {
                TimeSpanStatistics stat = (TimeSpanStatistics) (object) sample;
                Samples.Add(stat.Value);
                NewSamepleAdded = true;
            }
            else
            {
                base.AddSample(sample);
            }
        }

        #endregion

        #region Overridden Protected Methods

        /// <summary>
        /// Computes the average for the samples in <see cref="AverageStatistics{T}.Samples"/> list.
        /// </summary>
        protected override void ComputeAverage()
        {
            if (NewSamepleAdded)
            {
                Debug.Assert(Samples.Count > 0);

                double sum = 0;
                foreach (TimeSpan sample in Samples)
                {
                    sum += sample.Ticks;
                }
                Value = new TimeSpan((long) sum/Samples.Count);
                NewSamepleAdded = false;
            }
        }

        #endregion
    }
}