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
using System.Runtime.Remoting.Messaging;
using ClearCanvas.Common;
using ClearCanvas.Dicom.Iod.Modules;
using ClearCanvas.Dicom.Iod.Sequences;

namespace ClearCanvas.Dicom.Network.Scu
{

    /// <summary>
    /// Scu class for printing a grayscale image.
    /// <para>
    /// <example>
    /// <code><![CDATA[
    /// private void SendPrintRequest(string clientAETitle, string remoteAE, string remoteHost, int remotePort, string file1)
    /// {
    ///     BasicGrayscalePrintScu printScu = new BasicGrayscalePrintScu();
    ///     printScu.Timeout = 100000;
    ///     BasicFilmSessionModuleIod basicFilmSessionModuleIod = new BasicFilmSessionModuleIod();
    ///     basicFilmSessionModuleIod.NumberOfCopies = 1;
    ///     
    ///     BasicFilmBoxModuleIod basicFilmBoxModuleIod = new BasicFilmBoxModuleIod();
    ///     // Set it to print 1 image
    ///     basicFilmBoxModuleIod.ImageDisplayFormat = @"STANDARD\1,1";
    ///     basicFilmBoxModuleIod.FilmSizeId = FilmSize.IN8x10;
    ///     basicFilmBoxModuleIod.MagnificationType = MagnificationType.None;
    ///     basicFilmBoxModuleIod.FilmOrientation = FilmOrientation.Portrait;
    ///     
    ///     IList<ImageBoxPixelModuleIod> imageBoxPixelModuleIods = new List<ImageBoxPixelModuleIod>();
    ///     
    ///     // Configure 1st Image
    ///     ImageBoxPixelModuleIod imageBoxPixelModuleIod = new ImageBoxPixelModuleIod();
    ///     imageBoxPixelModuleIod.ImageBoxPosition = 1;

    ///     // Add grayscale image sequence and add the file
    ///     var basicGrayscaleImageSequence = new BasicGrayscaleImageSequenceIod();
    ///     basicGrayscaleImageSequence.AddDicomFileValues(file1);
    ///     imageBoxPixelModuleIod.BasicGrayscaleImageSequenceList.Add(basicGrayscaleImageSequence);
    ///     imageBoxPixelModuleIods.Add(imageBoxPixelModuleIod);

    ///     DicomState expected = DicomState.Success;
    ///     DicomState actual = printScu.Print(clientAETitle, remoteAE, remoteHost, remotePort,
    ///     basicFilmSessionModuleIod, basicFilmBoxModuleIod, imageBoxPixelModuleIods);
    /// }
    /// ]]></code>
    /// </example>
    /// </para>
    /// </summary>
    /// <remarks>TODO: have a status event so client can know where we are in the print process... </remarks>
    public class BasicGrayscalePrintScu : ScuBase
    {
        #region RequestType Private Enum
        /// <summary>
        /// Private enum for knowing the state of the print request, so we know what to send to the SCP
        /// </summary>
        private enum RequestType
        {
            /// <summary>
            /// 
            /// </summary>
            None,
            /// <summary>
            /// 
            /// </summary>
            FilmSession,
            /// <summary>
            /// 
            /// </summary>
            FilmBox,
            /// <summary>
            /// 
            /// </summary>
            ImageBox,
            /// <summary>
            /// 
            /// </summary>
            PrintAction,
            /// <summary>
            /// 
            /// </summary>
            DeleteFilmBox,
            /// <summary>
            /// 
            /// </summary>
            DeleteFilmSession,
            /// <summary>
            /// 
            /// </summary>
            Close
        }
        #endregion

        #region Public Events/Delegates
        /// <summary>
        /// Print delegate for printing ASynch 
        /// </summary>
        public delegate DicomState PrintDelegate(string clientAETitle, string remoteAE, string remoteHost, int remotePort, BasicFilmSessionModuleIod basicFilmSessionModuleIod, BasicFilmBoxModuleIod basicFilmBoxModuleIod, IList<ImageBoxPixelModuleIod> imageBoxPixelModuleIods);

        #endregion

        #region Private Variables
        /// <summary>
        /// Results 
        /// </summary>
        private DicomAttributeCollection _results;

        /// <summary>
        /// 
        /// </summary>
        private RequestType _nextRequestType;

        /// <summary>
        /// This stores the film Session Uid that in the Film Session create request, we need it later on...
        /// </summary>
        string _filmSessionUid = null;

        /// <summary>
        /// This stores the film BOx Uid that in the Film Box create request, we need it later on...
        /// </summary>
        private IList<string> _filmBoxUids = new List<string>();

        /// <summary>
        /// Need to remember film box response message because this contains info about the image boxes... note
        /// only supports one film box for now...
        /// </summary>
        IDictionary<string, DicomAttributeCollection> _filmBoxResponseMessages = new Dictionary<string, DicomAttributeCollection>();

        /// <summary>
        /// Keeps track of which image is sent
        /// </summary>
        private int _currentImageBoxIndex;

        /// <summary>
        /// Basic Film Session request
        /// </summary>
        BasicFilmSessionModuleIod _basicFilmSessionModuleIod;

        /// <summary>
        /// Basic Film Box request
        /// </summary>
        BasicFilmBoxModuleIod _basicFilmBoxModuleIod;

        /// <summary>
        /// Image Box Pixel List (list of images to be printed)
        /// </summary>
        IList<ImageBoxPixelModuleIod> _imageBoxPixelModuleIods;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicGrayscalePrintScu"/> class.
        /// </summary>
        public BasicGrayscalePrintScu()
            : base()
        {
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Prints with the specified parameters.
        /// </summary>
        /// <param name="clientAETitle">The client AE title.</param>
        /// <param name="remoteAE">The remote AE.</param>
        /// <param name="remoteHost">The remote host.</param>
        /// <param name="remotePort">The remote port.</param>
        /// <param name="basicFilmSessionModuleIod">The basic film session module iod.</param>
        /// <param name="basicFilmBoxModuleIod">The basic film box module iod.</param>
        /// <param name="imageBoxPixelModuleIods">The image box pixel module iods.</param>
        public DicomState Print(string clientAETitle, string remoteAE, string remoteHost, int remotePort, BasicFilmSessionModuleIod basicFilmSessionModuleIod, BasicFilmBoxModuleIod basicFilmBoxModuleIod, IList<ImageBoxPixelModuleIod> imageBoxPixelModuleIods)
        {
            _results = null;
            _filmSessionUid = null;
            _basicFilmSessionModuleIod = basicFilmSessionModuleIod;
            _basicFilmBoxModuleIod = basicFilmBoxModuleIod;
            _imageBoxPixelModuleIods = imageBoxPixelModuleIods;
            _filmBoxResponseMessages.Clear();
            _currentImageBoxIndex = 0;
            _filmBoxUids.Clear();
            Connect(clientAETitle, remoteAE, remoteHost, remotePort);
            if (Status == ScuOperationStatus.AssociationRejected || Status == ScuOperationStatus.Failed || Status == ScuOperationStatus.ConnectFailed ||
                Status == ScuOperationStatus.NetworkError || Status == ScuOperationStatus.TimeoutExpired)
                return DicomState.Failure;
            return ResultStatus;
        }

        /// <summary>
        /// Begins the print asynchronously.
        /// </summary>
        /// <param name="clientAETitle">The client AE title.</param>
        /// <param name="remoteAE">The remote AE.</param>
        /// <param name="remoteHost">The remote host.</param>
        /// <param name="remotePort">The remote port.</param>
        /// <param name="basicFilmSessionModuleIod">The basic film session module iod.</param>
        /// <param name="basicFilmBoxModuleIod">The basic film box module iod.</param>
        /// <param name="imageBoxPixelModuleIods">The image box pixel module iods.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <returns></returns>
        public IAsyncResult BeginPrint(string clientAETitle, string remoteAE, string remoteHost, int remotePort, BasicFilmSessionModuleIod basicFilmSessionModuleIod, BasicFilmBoxModuleIod basicFilmBoxModuleIod, IList<ImageBoxPixelModuleIod> imageBoxPixelModuleIods, AsyncCallback callback, object asyncState)
        {
            PrintDelegate printDelegate = new PrintDelegate(this.Print);

            return printDelegate.BeginInvoke(clientAETitle, remoteAE, remoteHost, remotePort, basicFilmSessionModuleIod, basicFilmBoxModuleIod, imageBoxPixelModuleIods, callback, asyncState);
        }

        /// <summary>
        /// Ends the asynchronous print.
        /// </summary>
        /// <param name="ar">The ar.</param>
        /// <returns></returns>
        public DicomState EndPrint(IAsyncResult ar)
        {
            PrintDelegate printDelegate = ((AsyncResult)ar).AsyncDelegate as PrintDelegate;

            if (printDelegate != null)
            {
                return printDelegate.EndInvoke(ar);
            }
            else
                throw new InvalidOperationException("cannot get results, asynchresult is null");
        }
        #endregion

        #region Private Methods
        private void SendCreateFilmSessionRequest(DicomClient client, ClientAssociationParameters association)
        {
            DicomMessage newRequestMessage = new DicomMessage(null, (DicomAttributeCollection)_basicFilmSessionModuleIod.DicomAttributeProvider);

            byte pcid = association.FindAbstractSyntaxOrThrowException(SopClass.BasicGrayscalePrintManagementMetaSopClass);
            _nextRequestType = RequestType.FilmBox;
            client.SendNCreateRequest(DicomUid.GenerateUid(), pcid, client.NextMessageID(), newRequestMessage, DicomUids.BasicFilmSession);
        }

        private void SendCreateFilmBoxRequest(DicomClient client, ClientAssociationParameters association, DicomMessage responseMessage)
        {

            ReferencedInstanceSequenceIod referencedFilmSessionSequence = new ReferencedInstanceSequenceIod();
            referencedFilmSessionSequence.ReferencedSopClassUid = SopClass.BasicFilmSessionSopClassUid;
            referencedFilmSessionSequence.ReferencedSopInstanceUid = responseMessage.AffectedSopInstanceUid;
            _basicFilmBoxModuleIod.ReferencedFilmSessionSequenceList.Add(referencedFilmSessionSequence);

            DicomMessage newRequestMessage = new DicomMessage(null, (DicomAttributeCollection)_basicFilmBoxModuleIod.DicomAttributeProvider);

            byte pcid = association.FindAbstractSyntaxOrThrowException(SopClass.BasicGrayscalePrintManagementMetaSopClass);

            _nextRequestType = RequestType.ImageBox;
            client.SendNCreateRequest(DicomUid.GenerateUid(), pcid, client.NextMessageID(), newRequestMessage, DicomUids.BasicFilmBoxSOP);
        }

        private void SendSetImageBoxRequest(DicomClient client, ClientAssociationParameters association)
        {
            if (_currentImageBoxIndex >= _imageBoxPixelModuleIods.Count)
            {
                // done sending images box - send print request
                _nextRequestType = RequestType.PrintAction;
                SendActionPrintRequest(client, association);
            }
            else
            {
                // want to get first film box response - although not sure if CC is using .net 3.5.. prolly not so do it old way
                IEnumerator<DicomAttributeCollection> filmBoxResponseEnumerator = _filmBoxResponseMessages.Values.GetEnumerator();
                filmBoxResponseEnumerator.Reset();
                filmBoxResponseEnumerator.MoveNext();

                BasicFilmBoxModuleIod basicFilmBoxModuleIod = new BasicFilmBoxModuleIod(filmBoxResponseEnumerator.Current);

                if (_currentImageBoxIndex > basicFilmBoxModuleIod.ReferencedImageBoxSequenceList.Count)
                {
                    throw new DicomException("Current Image Box Index is greater than number of Referenced ImageBox Sequences - set image box data");
                }

                ImageBoxPixelModuleIod imageBoxPixelModuleIod = _imageBoxPixelModuleIods[_currentImageBoxIndex];

                DicomMessage newRequestMessage = new DicomMessage(null, (DicomAttributeCollection)imageBoxPixelModuleIod.DicomAttributeProvider);
                newRequestMessage.RequestedSopClassUid = SopClass.BasicGrayscaleImageBoxSopClassUid;
                newRequestMessage.RequestedSopInstanceUid = basicFilmBoxModuleIod.ReferencedImageBoxSequenceList[_currentImageBoxIndex].ReferencedSopInstanceUid;

                byte pcid = association.FindAbstractSyntax(SopClass.BasicGrayscalePrintManagementMetaSopClass);

                _currentImageBoxIndex++;
                client.SendNSetRequest(pcid, client.NextMessageID(), newRequestMessage);
            }

        }

        /// <summary>
        /// Sends the action print request for the film box.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="association">The association.</param>
        /// <param name="responseMessage">The response message.</param>
        private void SendActionPrintRequest(DicomClient client, ClientAssociationParameters association)
        {
            DicomMessage newRequestMessage = new DicomMessage(null, null);
            newRequestMessage.RequestedSopInstanceUid = _filmBoxUids[0];
            newRequestMessage.RequestedSopClassUid = SopClass.BasicFilmBoxSopClassUid;
            newRequestMessage.ActionTypeId = 1;
            _nextRequestType = RequestType.DeleteFilmBox;

            byte pcid = association.FindAbstractSyntaxOrThrowException(SopClass.BasicGrayscalePrintManagementMetaSopClass);
            client.SendNActionRequest(pcid, client.NextMessageID(), newRequestMessage);
        }

        private void SendDeleteFilmBoxRequest(DicomClient client, ClientAssociationParameters association, DicomMessage responseMessage)
        {
            if (_filmBoxUids.Count == 0)
            {
                // no more film boxes left to delete - so send delete film session
                SendDeleteFilmSessionRequest(client, association);
            }
            else
            {
                string currentFilmBoxUid = _filmBoxUids[0];
                _filmBoxUids.Remove(currentFilmBoxUid);

                DicomMessage newRequestMessage = new DicomMessage(null, null);
                newRequestMessage.RequestedSopInstanceUid = currentFilmBoxUid;
                newRequestMessage.RequestedSopClassUid = SopClass.BasicFilmBoxSopClassUid;
                newRequestMessage.Priority = DicomPriority.Medium;

                _nextRequestType = RequestType.DeleteFilmBox;

                byte pcid = association.FindAbstractSyntaxOrThrowException(SopClass.BasicGrayscalePrintManagementMetaSopClass);
                client.SendNDeleteRequest(pcid, client.NextMessageID(), newRequestMessage);
            }
        }

        private void SendDeleteFilmSessionRequest(DicomClient client, ClientAssociationParameters association)
        {
            DicomMessage newRequestMessage = new DicomMessage(null, null);
            newRequestMessage.RequestedSopInstanceUid = _filmSessionUid;
            newRequestMessage.RequestedSopClassUid = SopClass.BasicFilmSessionSopClassUid;

            _nextRequestType = RequestType.Close;
            byte pcid = association.FindAbstractSyntaxOrThrowException(SopClass.BasicGrayscalePrintManagementMetaSopClass);
            client.SendNDeleteRequest(pcid, client.NextMessageID(), newRequestMessage);
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
            {
                client.SendAssociateAbort(DicomAbortSource.ServiceUser, DicomAbortReason.NotSpecified);
            }
            else
            {
                SendCreateFilmSessionRequest(client, association);
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
            try
            {
                base.ResultStatus = message.Status.Status;
                if (message.Status.Status == DicomState.Success)
                {
                    if (message.CommandField == DicomCommandField.NCreateResponse && message.AffectedSopClassUid == SopClass.BasicFilmSessionSopClassUid)
                    {
                        _filmSessionUid = message.AffectedSopInstanceUid;
                    }

                    else if (message.CommandField == DicomCommandField.NCreateResponse && message.AffectedSopClassUid == SopClass.BasicFilmBoxSopClassUid)
                    {
                        _filmBoxUids.Add(message.AffectedSopInstanceUid);
                        _filmBoxResponseMessages.Add(message.AffectedSopInstanceUid, message.DataSet);
                    }

                    Platform.Log(LogLevel.Info, "Success status received in Printer Status Scu!");
                    _results = message.DataSet;
                    switch (_nextRequestType)
                    {
                        case RequestType.FilmBox:
                            SendCreateFilmBoxRequest(client, association, message);
                            break;

                        case RequestType.ImageBox:
                            SendSetImageBoxRequest(client, association);
                            break;

                        case RequestType.PrintAction:
                            SendActionPrintRequest(client, association);
                            break;

                        case RequestType.DeleteFilmBox:
                            SendDeleteFilmBoxRequest(client, association, message);
                            break;

                        case RequestType.DeleteFilmSession:
                            SendDeleteFilmSessionRequest(client, association);
                            break;

                        case RequestType.Close:
                            base.ReleaseConnection(client);
                            break;

                        case RequestType.None:
                        default:
                            // TODO: throw error....
                            break;
                    }
                }
                else
                {
                    // TODO: Handle this... check for warnings - they are OK?  throw exception on errors... ?
                }

            }
            catch (Exception ex)
            {
                Platform.Log(LogLevel.Error, ex.ToString());
                base.ReleaseConnection(client);
                throw;
            }
        }

        /// <summary>
        /// Adds the appropriate Patient Root presentation context.
        /// </summary>
        protected override void SetPresentationContexts()
        {
            base.AddSopClassToPresentationContext(SopClass.BasicGrayscalePrintManagementMetaSopClass);
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
