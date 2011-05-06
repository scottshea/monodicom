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

using ClearCanvas.Common;
using ClearCanvas.Common.Statistics;
using ClearCanvas.Dicom.Network;

namespace ClearCanvas.Dicom.Utilities.Statistics
{
    /// <summary>
    /// Wrapper class to generate transmission statistics for a DICOM association.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class AssociationStatisticsRecorder
    {
        
        #region private members
        // The tranmission statistics.
        private TransmissionStatistics _assocStats = null;
    	private bool _logInformation;
        #endregion

        #region Public Properties
        /// <summary>
        ///  Gets the statistics of the transmissions.
        /// </summary>
        public TransmissionStatistics Statistics
        {
            get { return _assocStats;  }
        }
        #endregion

        #region constructors
        /// <summary>
        /// Creates an instance of <seealso cref="AssociationStatisticsRecorder"/>
        /// </summary>
        /// <param name="network"></param>
        public AssociationStatisticsRecorder(NetworkBase network)
        {
        	_logInformation = network.LogInformation;

            // hookup network events
            network.AssociationEstablished+=OnAssociationEstablished;
            network.MessageReceived += OnDicomMessageReceived;
            network.MessageSent += OnDicomMessageSent;
            network.AssociationReleased+=OnAssociationReleased;

            string description;
            if (network is DicomClient)
                description = string.Format("DICOM association from {0} [{1}:{2}] to {3} [{4}:{5}]",
                                            network.AssociationParams.CallingAE,
                                            network.AssociationParams.LocalEndPoint.Address,
                                            network.AssociationParams.LocalEndPoint.Port,
                                            network.AssociationParams.CalledAE,
                                            network.AssociationParams.RemoteEndPoint.Address,
                                            network.AssociationParams.RemoteEndPoint.Port);
            else
                description = string.Format("DICOM association from {0} [{1}:{2}] to {3} [{4}:{5}]",
                                            network.AssociationParams.CallingAE,
                                            network.AssociationParams.RemoteEndPoint.Address,
                                            network.AssociationParams.RemoteEndPoint.Port,
                                            network.AssociationParams.CalledAE,
                                            network.AssociationParams.LocalEndPoint.Address,
                                            network.AssociationParams.LocalEndPoint.Port);

            _assocStats = new TransmissionStatistics(description);
        }


        #endregion

        #region protected methods
        /// <summary>
        /// Event handlers called when association has been established.
        /// </summary>
        /// <param name="assoc">The association</param>
        protected void OnAssociationEstablished(AssociationParameters assoc)
        {
            if (_assocStats == null)
                _assocStats = new TransmissionStatistics(string.Format("DICOM association from {0} [{1}:{2}] to {3}", 
                                    assoc.CallingAE, 
                                    assoc.RemoteEndPoint.Address, 
                                    assoc.RemoteEndPoint.Port,
                                    assoc.CalledAE));
                      
            // start recording
            _assocStats.Begin(); 
        }

        /// <summary>
        /// Event handler called when an association has been released.
        /// </summary>
        /// <param name="assoc">The association</param>
        protected void OnAssociationReleased(AssociationParameters assoc)
        {
            if (_assocStats == null)
                return;

            // update the association statistics
            _assocStats.IncomingBytes = assoc.TotalBytesRead;
            _assocStats.OutgoingBytes = assoc.TotalBytesSent; 
            
            // signal stop recording.. the statistic object will fill out whatever 
            // it needs at this point based on what we have set
            _assocStats.End();

			if (_logInformation)
				StatisticsLogger.Log(LogLevel.Info, _assocStats);
        }

        /// <summary>
        /// Event handler called when an association has been aborted.
        /// </summary>
        /// <param name="assoc">The aborted association</param>
        /// <param name="reason">The abort reason</param>
        protected void OnAssociationAborted(AssociationParameters assoc, DicomAbortReason reason)
        {
            if (_assocStats == null)
                return;

            // update the association statistics
            _assocStats.IncomingBytes = assoc.TotalBytesRead;
            _assocStats.OutgoingBytes = assoc.TotalBytesSent; 
            
            // signal stop recording.. the statistic object will fill out whatever 
            // it needs at this point based on what we have set
            _assocStats.End();           
        }

        /// <summary>
        /// Event handler called while a DICOM message has been received.
        /// </summary>
        /// <param name="assoc">The association</param>
        /// <param name="dcmMsg">The received DICOM message</param>
        private void OnDicomMessageReceived(
                                AssociationParameters assoc, 
                                DicomMessage dcmMsg)
        {
            if (_assocStats == null)
                return;

            // update the association stats
            _assocStats.IncomingBytes = assoc.TotalBytesRead;
            _assocStats.OutgoingBytes = assoc.TotalBytesSent; 
            
            _assocStats.IncomingMessages++;            
        }

        /// <summary>
        /// Event handler called while a DICOM message has been sent.
        /// </summary>
        /// <param name="assoc">The association</param>
        /// <param name="dcmMsg">The request DICOM message sent</param>
        private void OnDicomMessageSent(
                                AssociationParameters assoc,
                                DicomMessage dcmMsg)
        {
            if (_assocStats == null)
                return;

            // update the association stats
            _assocStats.IncomingBytes = assoc.TotalBytesRead;
            _assocStats.OutgoingBytes = assoc.TotalBytesSent; 
            
            _assocStats.OutgoingMessages++;
        }

        #endregion
    }
}
