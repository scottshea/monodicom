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
using System.Net;
using System.Runtime.Remoting.Messaging;
using ClearCanvas.Dicom.Network;

namespace ClearCanvas.Dicom.Samples
{
    /// <summary>
    /// DICOM Verification SCU Sample Application
    /// </summary>
    public class VerificationScu : IDicomClientHandler
    {
        #region Private Members
        private ClientAssociationParameters _assocParams = null;
        private DicomClient _dicomClient = null;
        private VerificationResult _verificationResult;
        #endregion

        #region Protected Properties...
        private readonly System.Threading.AutoResetEvent _progressEvent = new System.Threading.AutoResetEvent(false);
        protected System.Threading.AutoResetEvent ProgressEvent
        {
            get { return _progressEvent; }
        }
        #endregion

        #region Constructors

    	#endregion

        #region Public Methods
        /// <summary>
        /// Adds the verification presentation context.
        /// </summary>
        private void SetPresentationContexts()
        {
            byte pcid = _assocParams.FindAbstractSyntax(SopClass.VerificationSopClass);
            if (pcid == 0)
            {
                pcid = _assocParams.AddPresentationContext(SopClass.VerificationSopClass);

                _assocParams.AddTransferSyntax(pcid, TransferSyntax.ExplicitVrLittleEndian);
                _assocParams.AddTransferSyntax(pcid, TransferSyntax.ImplicitVrLittleEndian);
            }
        }

        /// <summary>
        /// Sends verification request to specified Remote Dicom Host.
        /// </summary>
        /// <param name="clientAETitle"></param>
        /// <param name="remoteAE"></param>
        /// <param name="remoteHost"></param>
        /// <param name="remotePort"></param>
        /// <returns></returns>
        public VerificationResult Verify(string clientAETitle, string remoteAE, string remoteHost, int remotePort)
        {
            if (_dicomClient == null)
            {
                // TODO: Dispose...
                _dicomClient = null;
            }

            Logger.LogInfo("Preparing to connect to AE {0} on host {1} on port {2} for verification.", remoteAE, remoteHost, remotePort);
            try
            {
                IPAddress addr = null;
                foreach (IPAddress dnsAddr in Dns.GetHostAddresses(remoteHost))
                    if (dnsAddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        addr = dnsAddr;
                        break;
                    }
                if (addr == null)
                {
                    Logger.LogError("No Valid IP addresses for host {0}", remoteHost);
                    _verificationResult = VerificationResult.Failed;
                }
                else
                {
                    _assocParams = new ClientAssociationParameters(clientAETitle, remoteAE, new IPEndPoint(addr, remotePort));

                    SetPresentationContexts();

                    _verificationResult = VerificationResult.Failed;
                    _dicomClient = DicomClient.Connect(_assocParams, this);
                    ProgressEvent.WaitOne();
                }
            }
            catch (Exception e)
            {
                Logger.LogErrorException(e, "Unexpected exception trying to connect to Remote AE {0} on host {1} on port {2}", remoteAE, remoteHost, remotePort);
            }
            return _verificationResult;
        }

        public IAsyncResult BeginVerify(string clientAETitle, string remoteAE, string remoteHost, int remotePort, AsyncCallback callback, object asyncState)
        {
            VerifyDelegate verifyDelegate = this.Verify;

            return verifyDelegate.BeginInvoke(clientAETitle, remoteAE, remoteHost, remotePort, callback, asyncState);
        }

        public void Cancel()
        {
            if (_verificationResult != VerificationResult.Canceled)
            {
                Logger.LogInfo("Canceling verify...");
                _verificationResult = VerificationResult.Canceled;
                if (_dicomClient != null)
                    _dicomClient.Abort();
                ProgressEvent.Set();
            }
        }

        /// <summary>
        /// Ends the verify.
        /// </summary>
        /// <param name="ar">The ar.</param>
        /// <returns></returns>
        public VerificationResult EndVerify(IAsyncResult ar)
        {
            VerifyDelegate verifyDelegate = ((AsyncResult)ar).AsyncDelegate as VerifyDelegate;
            if (verifyDelegate != null)
            {
                return verifyDelegate.EndInvoke(ar);
            }
            else
                throw new InvalidOperationException("cannot get results, asynchresult is null");
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Generic routine to send the next C-STORE-RQ message in the _fileList.
        /// </summary>
        /// <param name="client">DICOM Client class</param>
        /// <param name="association">Association Parameters</param>
        private void SendVerificationRequest(DicomClient client, ClientAssociationParameters association)
        {

            byte pcid = association.FindAbstractSyntax(SopClass.VerificationSopClass);

            client.SendCEchoRequest(pcid, client.NextMessageID());
        }
        #endregion

        #region IDicomClientHandler Members

        public void OnReceiveAssociateAccept(DicomClient client, ClientAssociationParameters association)
        {
            Logger.LogInfo("Association Accepted when {0} connected to remote AE {1}", association.CallingAE, association.CalledAE);
            SendVerificationRequest(client, association);
        }

        public void OnReceiveAssociateReject(DicomClient client, ClientAssociationParameters association, DicomRejectResult result, DicomRejectSource source, DicomRejectReason reason)
        {
            Logger.LogInfo("Association Rejection when {0} connected to remote AE {1}", association.CallingAE, association.CalledAE);
            _verificationResult = VerificationResult.Failed;
            ProgressEvent.Set();
        }

        public void OnReceiveRequestMessage(DicomClient client, ClientAssociationParameters association, byte presentationID, DicomMessage message)
        {
            Logger.LogError("Unexpected OnReceiveRequestMessage callback on client.");

            throw new Exception("The method or operation is not implemented.");
        }

        public void OnReceiveResponseMessage(DicomClient client, ClientAssociationParameters association, byte presentationID, DicomMessage message)
        {
            if (message.Status.Status != DicomState.Success)
            {
                Logger.LogError("Failure status received in sending verification: {0}", message.Status.Description);
                _verificationResult = VerificationResult.Failed;
            }
            else if (_verificationResult == VerificationResult.Canceled)
            {
                Logger.LogInfo("Verification was canceled");
            }
            else
            {
                Logger.LogInfo("Success status received in sending verification!");
                _verificationResult = VerificationResult.Success;
            }
            client.SendReleaseRequest();
            ProgressEvent.Set();
        }

        public void OnReceiveReleaseResponse(DicomClient client, ClientAssociationParameters association)
        {
            Logger.LogInfo("Association released to {0}", association.CalledAE);
            ProgressEvent.Set();
        }

        public void OnReceiveAbort(DicomClient client, ClientAssociationParameters association, DicomAbortSource source, DicomAbortReason reason)
        {
            Logger.LogError("Unexpected association abort received from {0}", association.CalledAE);
            ProgressEvent.Set();
        }

        public void OnNetworkError(DicomClient client, ClientAssociationParameters association, Exception e)
        {
            if (_verificationResult != VerificationResult.Canceled)
            {
                Logger.LogErrorException(e, "Unexpected network error");
                _verificationResult = VerificationResult.Failed;
            }
            ProgressEvent.Set();
        }

        public void OnDimseTimeout(DicomClient client, ClientAssociationParameters association)
        {
            Logger.LogInfo("Timeout waiting for response message, continuing.");
            _verificationResult = VerificationResult.TimeoutExpired;
            ProgressEvent.Set();
        }

        #endregion

    }

    #region VerifyDelegate
    public delegate VerificationResult VerifyDelegate(string clientAETitle, string remoteAE, string remoteHost, int remotePort);
    #endregion

    #region VerificationResult Enum
    public enum VerificationResult
    {
        /// <summary>
        /// 
        /// </summary>
        Failed = 0,
        /// <summary>
        /// 
        /// </summary>
        Success = 1,
        /// <summary>
        /// 
        /// </summary>
        TimeoutExpired = 2,
        /// <summary>
        /// 
        /// </summary>
        Canceled = 3
    }
    #endregion

}
