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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.Network;
using System.Threading;

namespace ClearCanvas.Dicom.TestTools.TestScp
{
	public partial class Form1 : Form, IDicomServerHandler
	{
		
		private ServerAssociationParameters _parms;
		
		public Form1()
		{
			InitializeComponent();
		}

		private IDicomServerHandler StartAssociation(DicomServer server, ServerAssociationParameters assoc)
		{
			return this;
		}

		#region IDicomServerHandler Members

		public void OnReceiveAssociateRequest(DicomServer server, ServerAssociationParameters association)
		{
			if (_delayAssociationAccept.Checked)
				Thread.Sleep(TimeSpan.FromSeconds(35));

			if (_rejectAssociation.Checked)
				server.SendAssociateReject(DicomRejectResult.Permanent, DicomRejectSource.ServiceUser, DicomRejectReason.CallingAENotRecognized);
			else
				server.SendAssociateAccept(association);
		}

		public void OnReceiveRequestMessage(DicomServer server, ServerAssociationParameters association, byte presentationID, ClearCanvas.Dicom.DicomMessage message)
		{
			foreach (byte pcid in association.GetPresentationContextIDs())
			{
				DicomPresContext context = association.GetPresentationContext(pcid);
				if (context.Result == DicomPresContextResult.Accept)
				{
					if (context.AbstractSyntax == SopClass.StudyRootQueryRetrieveInformationModelFind)
					{
						DicomMessage response = new DicomMessage();
						response.DataSet[DicomTags.StudyInstanceUid].SetStringValue("1.2.3");
						response.DataSet[DicomTags.PatientId].SetStringValue("1");
						response.DataSet[DicomTags.PatientsName].SetStringValue("test");
						response.DataSet[DicomTags.StudyId].SetStringValue("1");
						response.DataSet[DicomTags.StudyDescription].SetStringValue("dummy");
						server.SendCFindResponse(presentationID, message.MessageId, response, DicomStatuses.Pending);

						DicomMessage finalResponse = new DicomMessage();
						server.SendCFindResponse(presentationID, message.MessageId, finalResponse, DicomStatuses.Success);
					}
					else if (context.AbstractSyntax == SopClass.VerificationSopClass)
					{
						server.SendCEchoResponse(presentationID, message.MessageId, DicomStatuses.Success);
					}
				}
			}
		}

		public void OnReceiveResponseMessage(DicomServer server, ServerAssociationParameters association, byte presentationID, ClearCanvas.Dicom.DicomMessage message)
		{
			server.SendAssociateAbort(DicomAbortSource.ServiceUser, DicomAbortReason.UnexpectedPDU);
		}

		public void OnReceiveReleaseRequest(DicomServer server, ServerAssociationParameters association)
		{
			if (_delayAssociationRelease.Checked)
				Thread.Sleep(TimeSpan.FromSeconds(35));
		}

		public void OnReceiveAbort(DicomServer server, ServerAssociationParameters association, DicomAbortSource source, DicomAbortReason reason)
		{
		}

		public void OnNetworkError(DicomServer server, ServerAssociationParameters association, Exception e)
		{
		}

		public void OnDimseTimeout(DicomServer server, ServerAssociationParameters association)
		{
		}

		#endregion

		private void _startStop_Click(object sender, EventArgs e)
		{
			if (_parms == null)
			{
				_parms = new ServerAssociationParameters("TEST", new IPEndPoint(IPAddress.Loopback, 105));
				_parms.AddPresentationContext(1, SopClass.VerificationSopClass);
				_parms.AddTransferSyntax(1, TransferSyntax.ExplicitVrLittleEndian);
				_parms.AddTransferSyntax(1, TransferSyntax.ImplicitVrLittleEndian);

				_parms.AddPresentationContext(2, SopClass.StudyRootQueryRetrieveInformationModelMove);
				_parms.AddTransferSyntax(2, TransferSyntax.ExplicitVrLittleEndian);
				_parms.AddTransferSyntax(2, TransferSyntax.ImplicitVrLittleEndian);

				_parms.AddPresentationContext(3, SopClass.StudyRootQueryRetrieveInformationModelFind);
				_parms.AddTransferSyntax(3, TransferSyntax.ExplicitVrLittleEndian);
				_parms.AddTransferSyntax(3, TransferSyntax.ImplicitVrLittleEndian);

				DicomServer.StartListening(_parms, StartAssociation);
				_startStop.Text = "Stop Listening";
			}
			else
			{
				DicomServer.StopListening(_parms);
				_parms = null;
				_startStop.Text = "Stop Listening";
			}
		}
	}
}