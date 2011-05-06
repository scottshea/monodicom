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
using System.Runtime.Remoting.Messaging;
using ClearCanvas.Dicom.Iod.Modules;

namespace ClearCanvas.Dicom.Network.Scu
{

    /// <summary>
    /// Scu class for getting printer status.
    /// </summary>
    /// <example>
    /// <code>
    /// PrinterStatusScu printScu = new PrinterStatusScu();
    /// PrinterModuleIod printerModuleIod = printScu.GetPrinterStatus("myClientAeTitle", "MyServerAE", "127.0.0.1", 104);
    /// </code>
    /// <para>
    /// Asynch:
    /// <code>
    /// PrinterStatusScu printerStatusScu = new PrinterStatusScu();
    /// printerStatusScu.BeginGetPrinterStatus("myClientAeTitle", "SnagIt", "127.0.0.1", 104, new AsyncCallback(GetPrinterStatusComplete), printerStatusScu);
    /// 
    /// private void GetPrinterStatusComplete(IAsyncResult ar)
    /// {
    ///     PrinterStatusScu printerStatusScu = (PrinterStatusScu)ar.AsyncState;
    ///     printerStatusScu.EndGetPrinterStatus(ar);
    ///     // Now do whatever we want with all the results, for example:
    ///     System.Diagnostics.Debug.Write(printerStatusScu.PrinterModuleResults.PrinterStatus);
    /// }
    /// </code>
    /// </para>
    /// </example>
    public class PrinterStatusScu : ScuBase
    {

        #region Public Events/Delegates
        /// <summary>
        /// Delegate for asynchronous execution of <see cref="GetPrinterStatus"/>
        /// </summary>
        public delegate PrinterModuleIod GetPrinterStatusDelegate(string clientAETitle, string remoteAE, string remoteHost, int remotePort);
        #endregion

        #region Private Variables
        /// <summary>
        /// Results 
        /// </summary>
        private DicomAttributeCollection _results;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FindScuBase"/> class.
        /// </summary>
        public PrinterStatusScu()
            :base()
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the results of the find request.
        /// </summary>
        /// <value>The results.</value>
        public DicomAttributeCollection Results
        {
            get { return _results; }
        }

        /// <summary>
        /// Gets the results as a <see cref="PrinterModuleIod"/>.
        /// </summary>
        /// <value>The modality worklist results.</value>
        public PrinterModuleIod PrinterModuleResults
        {
            get
            {
                if (_results != null)
                {
                    return new PrinterModuleIod(_results);
                }
                return null;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the printer status.
        /// </summary>
        /// <param name="clientAETitle">The client AE title.</param>
        /// <param name="remoteAE">The remote AE.</param>
        /// <param name="remoteHost">The remote host.</param>
        /// <param name="remotePort">The remote port.</param>
        /// <returns></returns>
        public PrinterModuleIod GetPrinterStatus(string clientAETitle, string remoteAE, string remoteHost, int remotePort)
        {
            this._results = null;
            Connect(clientAETitle, remoteAE, remoteHost, remotePort);
            return PrinterModuleResults;
        }

        /// <summary>
        /// Begins the get printer status in asynchronous mode.
        /// </summary>
        /// <param name="clientAETitle">The client AE title.</param>
        /// <param name="remoteAE">The remote AE.</param>
        /// <param name="remoteHost">The remote host.</param>
        /// <param name="remotePort">The remote port.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <returns></returns>
        public IAsyncResult BeginGetPrinterStatus(string clientAETitle, string remoteAE, string remoteHost, int remotePort, AsyncCallback callback, object asyncState)
        {
            GetPrinterStatusDelegate getPrinterStatusDelegate = new GetPrinterStatusDelegate(this.GetPrinterStatus);

            return getPrinterStatusDelegate.BeginInvoke(clientAETitle, remoteAE, remoteHost, remotePort, callback, asyncState);
        }

        /// <summary>
        /// Ends the get printer status.
        /// </summary>
        /// <param name="ar">The ar.</param>
        /// <returns></returns>
        public PrinterModuleIod EndGetPrinterStatus(IAsyncResult ar)
        {
            GetPrinterStatusDelegate getPrinterStatusDelegate = ((AsyncResult)ar).AsyncDelegate as GetPrinterStatusDelegate;
            if (getPrinterStatusDelegate != null)
            {
                return getPrinterStatusDelegate.EndInvoke(ar) as PrinterModuleIod;
            }
            else
                throw new InvalidOperationException("cannot get results, asynchresult is null");
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sends the find request.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="association">The association.</param>
        private static void SendRequest(DicomClient client, ClientAssociationParameters association)
        {
            DicomMessage newRequestMessage = new DicomMessage();
            PrinterModuleIod.SetCommonTags(newRequestMessage.DataSet);
            byte pcid = association.FindAbstractSyntax(SopClass.PrinterSopClass);
            if (pcid > 0)
            {
                client.SendNGetRequest(DicomUids.PrinterSOPInstance, pcid, client.NextMessageID(), newRequestMessage);
            }
        }




        #endregion

        #region Overridden Methods
        /// <summary>
        /// Called when received associate accept.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="association">The association.</param>
        public override void OnReceiveAssociateAccept(DicomClient client, ClientAssociationParameters association)
        {
            base.OnReceiveAssociateAccept(client, association);
            if (Canceled)
                client.SendAssociateAbort(DicomAbortSource.ServiceUser, DicomAbortReason.NotSpecified);
            else
            {
                SendRequest(client, association);
            }
        }

        /// <summary>
        /// Called when received response message.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="association">The association.</param>
        /// <param name="presentationID">The presentation ID.</param>
        /// <param name="message">The message.</param>
        public override void OnReceiveResponseMessage(DicomClient client, ClientAssociationParameters association, byte presentationID, DicomMessage message)
        {
            base.ResultStatus = message.Status.Status;
            if (message.Status.Status == DicomState.Success)
            {
                this._results = message.DataSet;
            }
            base.ReleaseConnection(client);
        }

        /// <summary>
        /// Adds the appropriate Patient Root presentation context.
        /// </summary>
        protected override void SetPresentationContexts()
        {
            AddSopClassToPresentationContext(SopClass.PrinterSopClass);
        }

        #endregion

        #region IDisposable Members

        private bool _disposed = false;
        /// <summary>
        /// Disposes the specified disposing.
        /// </summary>
        /// <param name="disposing">if set to <c>true</c> [disposing].</param>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                // Dispose of other Managed objects, ie

            }
            // FREE UNMANAGED RESOURCES
            base.Dispose(true);
            _disposed = true;
        }
        #endregion

    }


}
