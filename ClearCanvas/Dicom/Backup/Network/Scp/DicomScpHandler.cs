﻿#region License

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
using ClearCanvas.Common;
using ClearCanvas.Dicom.Network.Scu;
using ClearCanvas.Dicom.Utilities.Statistics;

namespace ClearCanvas.Dicom.Network.Scp
{
    /// <summary>
    /// Class used to handle incoming SCP associations.
    /// </summary>
    internal class DicomScpHandler<TContext> : IDicomServerHandler
    {
        #region Private Members
        private readonly TContext _context;
        private readonly IDictionary<byte, IDicomScp<TContext>> _extensionList = new Dictionary<byte, IDicomScp<TContext>>();
        private readonly DicomScp<TContext>.AssociationVerifyCallback _verifier;
    	private readonly DicomScp<TContext>.AssociationComplete _complete;
    	private readonly List<StorageInstance> _instances = new List<StorageInstance>();
        private AssociationStatisticsRecorder _statsRecorder ;
        #endregion

        #region Contructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>
        /// The constructor creates a dictionary of each presentation context negotiated for the 
        /// association, and the plugin that will handle it.  This is used later when incoming request
        /// messages are processed.
        /// </remarks>
        /// <param name="server">The server.</param>
        /// <param name="parameters">Association parameters for the negotiated association.</param>
        /// <param name="userParms">User parameters to be passed to the plugins called by the class.</param>
        /// <param name="verifier">Delegate to call to verify an association before its accepted.</param>
        /// <param name="complete">Delegate to call when the association is closed/complete.  Can be null.</param>
        public DicomScpHandler(DicomServer server, ServerAssociationParameters parameters, TContext userParms, DicomScp<TContext>.AssociationVerifyCallback verifier, DicomScp<TContext>.AssociationComplete complete)
        {
            _context = userParms;
            _verifier = verifier;
        	_complete = complete;

            DicomScpExtensionPoint<TContext> ep = new DicomScpExtensionPoint<TContext>();
            object[] scps = ep.CreateExtensions();

            // First set the user parms for each of the extensions before we do anything with them.
            foreach (object obj in scps)
            {
                IDicomScp<TContext> scp = obj as IDicomScp<TContext>;
                scp.SetContext(_context);
            }

            // Now, create a dictionary with the extension to be used for each presentation context.
            foreach (byte pcid in parameters.GetPresentationContextIDs())
            {
                if (parameters.GetPresentationContextResult(pcid) == DicomPresContextResult.Accept)
                {
                    SopClass acceptedSop = SopClass.GetSopClass(parameters.GetAbstractSyntax(pcid).UID);
                    TransferSyntax acceptedSyntax = parameters.GetAcceptedTransferSyntax(pcid);
                    foreach (object obj in scps)
                    {
                        IDicomScp<TContext> scp = obj as IDicomScp<TContext>;

                        IList<SupportedSop> sops = scp.GetSupportedSopClasses();
                        foreach (SupportedSop sop in sops)
                        {
                            if (sop.SopClass.Equals(acceptedSop))
                            {
                                if (sop.SyntaxList.Contains(acceptedSyntax))
                                {
                                    if (!_extensionList.ContainsKey(pcid))
                                    {
                                        _extensionList.Add(pcid, scp);
                                        break;
                                    }
                                    else
                                        Platform.Log(LogLevel.Error, "SOP Class {0} supported by more than one extension", sop.SopClass.Name);
                                }
                            }
                        }
                    }
                }
            }

            _statsRecorder = new AssociationStatisticsRecorder(server); 

        }
        #endregion

        #region IDicomServerHandler Members

        void IDicomServerHandler.OnReceiveAssociateRequest(DicomServer server, ServerAssociationParameters association)
        {
            if (_verifier != null)
            {
                DicomRejectResult result;
                DicomRejectReason reason;
                bool verified = _verifier(_context, association, out result, out reason);
                if (verified == false)
                {
                    server.SendAssociateReject(result,DicomRejectSource.ServiceUser,reason);
                    Platform.Log(LogLevel.Info,"Association rejected from {0} to {1}", association.CallingAE, association.CalledAE);
                    return;
                }
            }

            // Let the extensions have its say on whether a presentation context is really acceptable.
            //
            bool atLeastOneAccepted = false;
            foreach (byte pcid in association.GetPresentationContextIDs())
            {
                if (association.GetPresentationContextResult(pcid)==DicomPresContextResult.Accept)
                {
                    IDicomScp<TContext> scp = _extensionList[pcid];
                    DicomPresContextResult res = scp.VerifyAssociation(association, pcid);
                    if (res!=DicomPresContextResult.Accept)
                    {
                        association.GetPresentationContext(pcid).ClearTransfers();
                        association.SetPresentationContextResult(pcid, res);
                    }
                    else
                    {
                        atLeastOneAccepted = true;
                    }

                }
            }

            if (!atLeastOneAccepted)
            {
                Platform.Log(LogLevel.Info, "None of the proposed presentation context is accepted. Rejecting association from {0} to {1}", association.CallingAE, association.CalledAE);
                server.SendAssociateReject(DicomRejectResult.Permanent, DicomRejectSource.ServiceUser, DicomRejectReason.NoReasonGiven);
                return;
            }
           

            server.SendAssociateAccept(association);

            Platform.Log(LogLevel.Info, "Received association:\r\n{0}", association.ToString());      
        }

        void IDicomServerHandler.OnReceiveRequestMessage(DicomServer server, ServerAssociationParameters association, byte presentationID, DicomMessage message)
        {
            IDicomScp<TContext> scp = _extensionList[presentationID];

            bool ok = scp.OnReceiveRequest(server, association, presentationID, message);
            if (!ok)
            {
                Platform.Log(LogLevel.Error, "Unexpected error processing message of type {0}.  Aborting association.", message.SopClass.Name);

                server.SendAssociateAbort(DicomAbortSource.ServiceProvider, DicomAbortReason.NotSpecified);

            }
			else if (_complete != null)
            {
				// Only save C-STORE-RQ messages
				if (message.CommandField == DicomCommandField.CStoreRequest)
            		_instances.Add(new StorageInstance(message));
            }
        }

        void IDicomServerHandler.OnReceiveResponseMessage(DicomServer server, ServerAssociationParameters association, byte presentationID, DicomMessage message)
        {
            Platform.Log(LogLevel.Error, "Unexpectedly received OnReceiveResponseMessage callback from {0} to {1}.  Aborting association.", association.CallingAE, association.CalledAE);
            server.SendAssociateAbort(DicomAbortSource.ServiceUser, DicomAbortReason.UnexpectedPDU);
        }


        void IDicomServerHandler.OnReceiveReleaseRequest(DicomServer server, ServerAssociationParameters association)
        {
            Platform.Log(LogLevel.Info, "Received association release request from {0} to {1}.", association.CallingAE, association.CalledAE);
			if (_complete != null)
				_complete(_context, association, _instances);
        }

        void IDicomServerHandler.OnReceiveAbort(DicomServer server, ServerAssociationParameters association, DicomAbortSource source, DicomAbortReason reason)
        {
            Platform.Log(LogLevel.Error, "Received association abort from {0} to {1}", association.CallingAE, association.CalledAE);
			if (_complete != null)
				_complete(_context, association, _instances);
		}

        void IDicomServerHandler.OnNetworkError(DicomServer server, ServerAssociationParameters association, Exception e)
        {
            Platform.Log(LogLevel.Error, "Unexpectedly received OnNetworkError callback from {0} to {1}.  Aborting association.", association.CallingAE, association.CalledAE);
			if (_complete != null)
				_complete(_context, association, _instances);
        }

        void IDicomServerHandler.OnDimseTimeout(DicomServer server, ServerAssociationParameters association)
        {
            // Annoying log
            //Platform.Log(LogLevel.Info, "Unexpected timeout waiting for activity on association from {0} to {1}.", association.CallingAE, association.CalledAE);
        }

        #endregion
    }
}
