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

using ClearCanvas.Common.Statistics;

#pragma warning disable 1591

namespace ClearCanvas.Dicom.Utilities.Statistics
{
    /// <summary>
    /// Transmission statistics class.
    /// </summary>
    public class TransmissionStatistics : StatisticsSet
    {
        #region Private members

        private TimeSpanStatistics _timeSpan = new TimeSpanStatistics("TimeSpan");

        #endregion Private members

        #region Constructors

        public TransmissionStatistics(string name)
            : base("Transmission", name)
        {
        }

        #endregion Constructors

        #region Public Properties

        public RateStatistics Speed
        {
            get
            {
                if (this["Speed"] == null)
                {
                    this["Speed"] = new RateStatistics("Speed", RateType.BYTES);
                }
                return this["Speed"] as RateStatistics;
            }
            set
            {
                this["Speed"] = value;
            }
        }

        public RateStatistics MessageRate
        {
            get
            {
                if (this["MessageRate"] == null)
                {
                    this["MessageRate"] = new RateStatistics("MessageRate", RateType.MESSAGES);
                }
                return this["MessageRate"] as RateStatistics;
            }
            set
            {
                this["MessageRate"] = value;
            }
        }

        public ulong IncomingBytes
        {
            get
            {
                if (this["IncomingBytes"] == null)
                {
                    this["IncomingBytes"] = new ByteCountStatistics("IncomingBytes");
                }
                return (this["IncomingBytes"] as ByteCountStatistics).Value;
            }
            set
            {
                this["IncomingBytes"] = new ByteCountStatistics("IncomingBytes", value); 
            }
        }

        public ulong IncomingMessages
        {
            get
            {
                if (this["IncomingMessages"] == null)
                {
                    this["IncomingMessages"] = new MessageCountStatistics("IncomingMessages");
                }
                return (this["IncomingMessages"] as MessageCountStatistics).Value;
            }
            set { this["IncomingMessages"] = new MessageCountStatistics("IncomingMessages", value); }
        }

        public ulong OutgoingBytes
        {
            get
            {
                if (this["OutgoingBytes"] == null)
                {
                    this["OutgoingBytes"] = new ByteCountStatistics("OutgoingBytes");
                }
                return (this["OutgoingBytes"] as ByteCountStatistics).Value;
            }
            set { this["OutgoingBytes"] = new ByteCountStatistics("OutgoingBytes", value); }
        }

        public ulong OutgoingMessages
        {
            get
            {
                if (this["OutgoingMessages"] == null)
                {
                    this["OutgoingMessages"] = new MessageCountStatistics("OutgoingMessages");
                }
                return (this["OutgoingMessages"] as MessageCountStatistics).Value;
            }
            set { this["OutgoingMessages"] = new MessageCountStatistics("OutgoingMessages", value); }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Signals the start of the transmission.
        /// </summary>
        public void Begin()
        {
            MessageRate.Start();
            Speed.Start();
        }

        /// <summary>
        /// Signals the end of the transmission.
        /// </summary>
        public void End()
        {
            if (OutgoingBytes > IncomingBytes)
            {
                Speed.SetData(OutgoingBytes);
                Speed.End();

                MessageRate.SetData(OutgoingMessages);
                MessageRate.End();
            }
            else
            {
                Speed.SetData(IncomingBytes);
                Speed.End();

                MessageRate.SetData(IncomingMessages);
                MessageRate.End();
            }
        }

        #endregion Public Methods
    }
}
