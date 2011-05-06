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
    /// <see cref="TimeSpan"/> formatter class.
    /// </summary>
    public static class TimeSpanFormatter
    {
        #region Constants

        private const double TICKSPERHOUR = TICKSPERMINUTE*60;
        private const double TICKSPERMICROECONDS = 10;
        private const double TICKSPERMILISECONDS = TICKSPERMICROECONDS*1000;
        private const double TICKSPERMINUTE = TICKSPERSECONDS*60;
        private const double TICKSPERNANOSECONDS = 1/100.0;
        private const double TICKSPERSECONDS = TICKSPERMILISECONDS*1000;

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Formats a <see cref="TimeSpan"/> in appropriate units, with option to round up.
        /// </summary>
        /// <param name="duration">The duration to be formatted</param>
        /// <param name="roundUp">Indicates whether the duration should be rounded up (eg, '3 sec' instead of '3.232 sec')</param>
        /// <returns>A formatted string representation of the duration</returns>
        public static string Format(TimeSpan duration, bool roundUp)
        {
            if (roundUp)
            {
                if (duration == TimeSpan.Zero)
                    return "N/A";
                else if (duration.Ticks > TICKSPERHOUR)
                    return String.Format("{0} hr {1} min", duration.Hours, duration.Minutes);
                if (duration.Ticks > TICKSPERMINUTE)
                    return String.Format("{0:0} min", duration.TotalMinutes);
                if (duration.Ticks > TICKSPERSECONDS)
                    return String.Format("{0:0} sec", duration.TotalSeconds);
                if (duration.Ticks > TICKSPERMILISECONDS)
                    return String.Format("{0:0} ms", duration.TotalMilliseconds);
                if (duration.Ticks > TICKSPERMICROECONDS)
                    return String.Format("{0:0} µs", duration.Ticks / TICKSPERMICROECONDS);
                else
                    return String.Format("{0:0} ns", duration.Ticks / TICKSPERNANOSECONDS);
            }
            else
            {
                if (duration == TimeSpan.Zero)
                    return "N/A";
                else if (duration.Ticks > TICKSPERHOUR)
                    return String.Format("{0} hr {1} min", duration.Hours, duration.Minutes);
                if (duration.Ticks > TICKSPERMINUTE)
                    return String.Format("{0:0.00} min", duration.TotalMinutes);
                if (duration.Ticks > TICKSPERSECONDS)
                    return String.Format("{0:0.00} sec", duration.TotalSeconds);
                if (duration.Ticks > TICKSPERMILISECONDS)
                    return String.Format("{0:0.00} ms", duration.TotalMilliseconds);
                if (duration.Ticks > TICKSPERMICROECONDS)
                    return String.Format("{0:0.00} µs", duration.Ticks / TICKSPERMICROECONDS);
                else
                    return String.Format("{0:0.00} ns", duration.Ticks / TICKSPERNANOSECONDS);
                
            }
        }

        /// <summary>
        /// Formats a <see cref="TimeSpan"/> in appropriate units.
        /// </summary>
        /// <param name="duration">The duration to be formatted</param>
        /// <returns>A formatted string representation of the duration</returns>
        public static string Format(TimeSpan duration)
        {
            return Format(duration, false);
        }

        #endregion
    }
}