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

namespace ClearCanvas.Common.Statistics
{
    /// <summary>
    /// Transmission rate formatter class.
    /// </summary>
    public static class TransmissionRateFormatter
    {
        #region Constants

        private const double GIGABYTES = 1024*MEGABYTES;
        private const double KILOBYTES = 1024;
        private const double MEGABYTES = 1024*KILOBYTES;

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Formats a transmission rate in appropriate units
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static string Format(double rate)
        {
            if (rate > GIGABYTES)
                return String.Format("{0:0.00} GB/s", rate/GIGABYTES);
            if (rate > MEGABYTES)
                return String.Format("{0:0.00} MB/s", rate/MEGABYTES);
            if (rate > KILOBYTES)
                return String.Format("{0:0.00} KB/s", rate/KILOBYTES);

            return String.Format("{0:0} bytes/s", rate);
        }

        #endregion
    }
}