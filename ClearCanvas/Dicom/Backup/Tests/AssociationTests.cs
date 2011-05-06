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

#if UNIT_TESTS

using System;
using System.Net;
using System.Threading;
using ClearCanvas.Dicom.Network;
using NUnit.Framework;
using ClearCanvas.Common;

namespace ClearCanvas.Dicom.Tests
{
    public enum TestTypes
    {
        AssociationReject,
        AssociationAbort,
        SendMR,
		Receive
    }

    public class ClientHandler : IDicomClientHandler
    {
        private AbstractTest _test;
        private TestTypes _type;
        public ManualResetEvent _threadStop = new ManualResetEvent(false);

        public bool OnClientConnectedCalled = false;
        public bool OnClientClosedCalled = false;

        public ClientHandler(AbstractTest test, TestTypes type)
        {
            _test = test;
            _type = type;
        }

        #region IDicomClientHandler Members

        public void OnDimseTimeout(DicomClient client, ClientAssociationParameters association)
        {
        }

        public void OnClientClosed(DicomClient client, ClientAssociationParameters association)
        {
            OnClientClosedCalled = true;
        }

        public void OnNetworkError(DicomClient client, ClientAssociationParameters association, Exception e)
        {
            Assert.Fail("Incorrectly received OnNetworkError callback");
        }

        public void OnReceiveAssociateAccept(DicomClient client, ClientAssociationParameters association)
        {
            if (_type == TestTypes.AssociationReject)
            {
                Assert.Fail("Unexpected negotiated association on reject test.");
            }
            else if (_type == TestTypes.SendMR)
            {
                DicomMessage msg = new DicomMessage();

                _test.SetupMR(msg.DataSet);
                byte id = association.FindAbstractSyntaxWithTransferSyntax(msg.SopClass, TransferSyntax.ExplicitVrLittleEndian);

                client.SendCStoreRequest(id, client.NextMessageID(), DicomPriority.Medium, msg);
            }
            else
            {
                Assert.Fail("Unexpected test type");
            }
        }

        public void OnReceiveRequestMessage(DicomClient client, ClientAssociationParameters association, byte presentationID, DicomMessage message)
        {
            Assert.Fail("Incorrectly received OnReceiveRequestMessage callback");
        }

        public void OnReceiveResponseMessage(DicomClient client, ClientAssociationParameters association, byte presentationID, DicomMessage message)
        {
            client.SendReleaseRequest();
            Assert.AreEqual(message.Status.Code, DicomStatuses.Success.Code, "Incorrect DICOM status returned");
        }

        public void OnReceiveReleaseResponse(DicomClient client, ClientAssociationParameters association)
        {
            // Signal the main thread we're exiting
            _threadStop.Set();
        }

        public void OnReceiveAssociateReject(DicomClient client, ClientAssociationParameters association, DicomRejectResult result, DicomRejectSource source, DicomRejectReason reason)
        {
            if (_type == TestTypes.AssociationReject)
            {
                Assert.IsTrue(source == DicomRejectSource.ServiceProviderACSE);
                Assert.IsTrue(result == DicomRejectResult.Permanent);
                Assert.IsTrue(reason == DicomRejectReason.NoReasonGiven);
                _threadStop.Set();
            }
            else
                Assert.Fail("Incorrectly received OnReceiveAssociateReject callback");
        }

        public void OnReceiveAbort(DicomClient client, ClientAssociationParameters association, DicomAbortSource source, DicomAbortReason reason)
        {
            Assert.Fail("Incorrectly received OnReceiveAbort callback");
        }

        #endregion
    }

    public class ServerHandler : IDicomServerHandler
    {
        AbstractTest _test;
        TestTypes _type;

        public ServerHandler(AbstractTest test, TestTypes type)
        {
            _test = test;
            _type = type;
        }
        #region IDicomServerHandler Members

        public void OnClientConnected(DicomServer server, ServerAssociationParameters association)
        {

        }

        public void OnClientClosed(DicomServer server, ServerAssociationParameters association)
        {
        }

        public void OnNetworkError(DicomServer server, ServerAssociationParameters association, Exception e)
        {
            Assert.Fail("Unexpected network error: " + e.Message);
        }

        public void OnDimseTimeout(DicomServer client, ServerAssociationParameters association)
        {
        }

        public void OnReceiveAssociateRequest(DicomServer server, ServerAssociationParameters association)
        {
            server.SendAssociateAccept(association);
        }

        public void OnReceiveRequestMessage(DicomServer server, ServerAssociationParameters association, byte presentationID, DicomMessage message)
        {
        	if (_type == TestTypes.SendMR)
        	{
        		DicomAttributeCollection testSet = new DicomAttributeCollection();

        		_test.SetupMR(testSet);

        		bool same = testSet.Equals(message.DataSet);


        		string studyId = message.DataSet[DicomTags.StudyId].GetString(0, "");
        		Assert.AreEqual(studyId, "1933");


        		DicomUid sopInstanceUid;
        		bool ok = message.DataSet[DicomTags.SopInstanceUid].TryGetUid(0, out sopInstanceUid);
        		if (!ok)
        		{
        			server.SendAssociateAbort(DicomAbortSource.ServiceUser, DicomAbortReason.NotSpecified);
        			return;
        		}

        		server.SendCStoreResponse(presentationID, message.MessageId, sopInstanceUid.UID, DicomStatuses.Success);
        	}
        	else if (_type == TestTypes.Receive)
        	{
				DicomUid sopInstanceUid;
				bool ok = message.DataSet[DicomTags.SopInstanceUid].TryGetUid(0, out sopInstanceUid);
				if (!ok)
				{
					server.SendAssociateAbort(DicomAbortSource.ServiceUser, DicomAbortReason.InvalidPDUParameter);
					return;
				}

				server.SendCStoreResponse(presentationID, message.MessageId, sopInstanceUid.UID, DicomStatuses.Success);
			}
        	else
        	{
				Platform.Log(LogLevel.Error,"Unexpected test type mode");
				server.SendAssociateAbort(DicomAbortSource.ServiceUser, DicomAbortReason.InvalidPDUParameter);
				return;        		
        	}
    }

        public void OnReceiveResponseMessage(DicomServer server, ServerAssociationParameters association, byte presentationID, DicomMessage message)
        {
            server.SendAssociateAbort(DicomAbortSource.ServiceUser, DicomAbortReason.NotSpecified);
            Assert.Fail("Unexpected OnReceiveResponseMessage");
        }

        public void OnReceiveReleaseRequest(DicomServer server, ServerAssociationParameters association)
        {

        }

        public void OnReceiveAbort(DicomServer server, ServerAssociationParameters association, DicomAbortSource source, DicomAbortReason reason)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

    [TestFixture]
    public class AssociationTests : AbstractTest
    {
        TestTypes _serverType;

        public IDicomServerHandler ServerHandlerCreator(DicomServer server, ServerAssociationParameters assoc)
        {
            return new ServerHandler(this,_serverType);
        }  

        [Test]
        public void RejectTests()
        {
            int port = 2112;

            /* Setup the Server */
            ServerAssociationParameters serverParameters = new ServerAssociationParameters("AssocTestServer", new IPEndPoint(IPAddress.Any, port));
            byte pcid = serverParameters.AddPresentationContext(SopClass.MrImageStorage);
            serverParameters.AddTransferSyntax(pcid, TransferSyntax.ExplicitVrLittleEndian);
            serverParameters.AddTransferSyntax(pcid, TransferSyntax.ExplicitVrBigEndian);
            serverParameters.AddTransferSyntax(pcid, TransferSyntax.ImplicitVrLittleEndian);

            _serverType = TestTypes.AssociationReject;
            DicomServer.StartListening(serverParameters, ServerHandlerCreator);

            /* Setup the client */
            ClientAssociationParameters clientParameters = new ClientAssociationParameters("AssocTestClient", "AssocTestServer",
                                                                                           new System.Net.IPEndPoint(IPAddress.Loopback, port));
            pcid = clientParameters.AddPresentationContext(SopClass.CtImageStorage);
            clientParameters.AddTransferSyntax(pcid, TransferSyntax.ExplicitVrLittleEndian);
            clientParameters.AddTransferSyntax(pcid, TransferSyntax.ImplicitVrLittleEndian);

            /* Open the association */
            ClientHandler handler = new ClientHandler(this, TestTypes.AssociationReject);
            DicomClient client = DicomClient.Connect(clientParameters, handler);


            handler._threadStop.WaitOne();
            client.Dispose();

            _serverType = TestTypes.AssociationReject;

            /* Setup the client */
            clientParameters = new ClientAssociationParameters("AssocTestClient", "AssocTestServer",
                                                               new System.Net.IPEndPoint(IPAddress.Loopback, port));
            pcid = clientParameters.AddPresentationContext(SopClass.MrImageStorage);
            clientParameters.AddTransferSyntax(pcid, TransferSyntax.Jpeg2000ImageCompressionLosslessOnly);


            /* Open the association */
            ClientHandler clientHandler = new ClientHandler(this, TestTypes.AssociationReject);
            client = DicomClient.Connect(clientParameters, clientHandler);

            handler._threadStop.WaitOne();
            client.Dispose();


            DicomServer.StopListening(serverParameters);

        }

       

        [Test]
        public void ServerTest()
        {
            int port = 2112;

            /* Setup the Server */
            ServerAssociationParameters serverParameters = new ServerAssociationParameters("AssocTestServer",new IPEndPoint(IPAddress.Any,port));
            byte pcid = serverParameters.AddPresentationContext(SopClass.MrImageStorage);
            serverParameters.AddTransferSyntax(pcid, TransferSyntax.ExplicitVrLittleEndian);
            serverParameters.AddTransferSyntax(pcid, TransferSyntax.ExplicitVrBigEndian);
            serverParameters.AddTransferSyntax(pcid, TransferSyntax.ImplicitVrLittleEndian);

            _serverType = TestTypes.SendMR;
            DicomServer.StartListening(serverParameters, ServerHandlerCreator);

            /* Setup the client */
            ClientAssociationParameters clientParameters = new ClientAssociationParameters("AssocTestClient","AssocTestServer",
                                                                                           new System.Net.IPEndPoint(IPAddress.Loopback,port));
            pcid = clientParameters.AddPresentationContext(SopClass.MrImageStorage);
            clientParameters.AddTransferSyntax(pcid, TransferSyntax.ExplicitVrLittleEndian);
            clientParameters.AddTransferSyntax(pcid, TransferSyntax.ImplicitVrLittleEndian);

            pcid = clientParameters.AddPresentationContext(SopClass.CtImageStorage);
            clientParameters.AddTransferSyntax(pcid, TransferSyntax.ExplicitVrLittleEndian);
            clientParameters.AddTransferSyntax(pcid, TransferSyntax.ImplicitVrLittleEndian);

            /* Open the association */
            ClientHandler handler = new ClientHandler(this,TestTypes.SendMR);
            DicomClient client = DicomClient.Connect(clientParameters, handler);


            handler._threadStop.WaitOne();

            client.Dispose();

            DicomServer.StopListening(serverParameters);
        }

    }
}

#endif
