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

namespace ClearCanvas.Common.Statistics
{
    /// <summary>
    /// Provides helper method to generate <see cref="TimeSpanStatistics"/>
    /// </summary>
    public class TimeSpanStatisticsHelper
    {
        /// <summary>
        /// Defines the delegate to a code block whose execution time will be measured using <see cref="Measure"/>.
        /// </summary>
        public delegate void ExecutationBlock();

        /// <summary>
        /// Measures the elapsed time taken by an code block.
        /// </summary>
        /// <param name="executionBlock">Delegate to the code block which will be executed by this method and its elapsed will be measured</param>
        /// <returns>The <see cref="TimeSpanStatistics"/> object contains the elapsed time of the execution</returns>
        static public TimeSpanStatistics Measure(ExecutationBlock executionBlock)
        {
            TimeSpanStatistics stat = new TimeSpanStatistics();

            stat.Start();
            try
            {
                executionBlock();
                return stat;
            }
            finally
            {
                stat.End();
            }
        }
    }
}
