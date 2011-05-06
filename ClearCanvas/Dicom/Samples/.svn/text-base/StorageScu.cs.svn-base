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
using System.Text;
using System.Net;
using System.IO;

using ClearCanvas.Dicom;
using ClearCanvas.Dicom.Network;

namespace ClearCanvas.Dicom.Samples
{
    /// <summary>
    /// DICOM Storage SCU Sample Application
    /// </summary>
    public class StorageScu : IDicomClientHandler
    {
        #region Private Classes and Structures
        struct FileToSend
        {
            public String filename;
            public SopClass sopClass;
            public TransferSyntax transferSyntax;
        }
        #endregion

        #region Private Members
        private List<FileToSend> _fileList = new List<FileToSend>();
        private int _fileListIndex = 0;
        private ClientAssociationParameters _assocParams = null;
        private DicomClient _dicomClient = null;
        #endregion

        #region Constructors
        public StorageScu()
        {
        }
        #endregion

        #region Private Methods
        private bool LoadDirectory(DirectoryInfo dir)
        {
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                AddFileToSend(file.FullName);
            }

            String[] subdirectories = Directory.GetDirectories(dir.FullName);
            foreach (String subPath in subdirectories)
            {
                DirectoryInfo subDir = new DirectoryInfo(subPath);
                LoadDirectory(subDir);
            }

            return true;
        }
        #endregion

        #region Public Methods

        public void ClearFiles()
        {
            _fileList.Clear();
        }

        public bool AddFileToSend(String file)
        {

            try
            {
                DicomFile dicomFile = new DicomFile(file);

                // Only load to sopy instance uid to reduce amount of data read from file
                dicomFile.Load(DicomTags.SopInstanceUid, DicomReadOptions.Default | DicomReadOptions.DoNotStorePixelDataInDataSet);

                FileToSend fileStruct = new FileToSend();

                fileStruct.filename = file;
                string sopClassInFile = dicomFile.DataSet[DicomTags.SopClassUid].ToString();
                if (sopClassInFile.Length == 0)
                    return false;

                if (!sopClassInFile.Equals(dicomFile.SopClass.Uid))
                {
                    Logger.LogError("SOP Class in Meta Info does not match SOP Class in DataSet");
                    fileStruct.sopClass = SopClass.GetSopClass(sopClassInFile);
                }
                else
                    fileStruct.sopClass = dicomFile.SopClass;

                fileStruct.transferSyntax = dicomFile.TransferSyntax;

                _fileList.Add(fileStruct);
            }
            catch (DicomException e)
            {
                Logger.LogErrorException(e, "Unexpected exception when loading file for sending: {0}", file);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Add all the files to send.
        /// </summary>
        /// <param name="directory">The path of the directory to scan for DICOM files</param>
        /// <returns></returns>
        public bool AddDirectoryToSend(String directory)
        {
            DirectoryInfo dir = new DirectoryInfo(directory);

			return LoadDirectory(dir);
		}

        /// <summary>
        /// Scan the files to send, and create presentation contexts for each abstract syntax to send.
        /// </summary>
        public void SetPresentationContexts()
        {
            foreach (FileToSend sendStruct in _fileList)
            {
                byte pcid =
                    _assocParams.FindAbstractSyntaxWithTransferSyntax(sendStruct.sopClass, sendStruct.transferSyntax);

                if (pcid == 0)
                {
					pcid = _assocParams.AddPresentationContext(sendStruct.sopClass);
                    _assocParams.AddTransferSyntax(pcid, sendStruct.transferSyntax);
					if (sendStruct.transferSyntax.Equals(TransferSyntax.ImplicitVrLittleEndian))
						_assocParams.AddTransferSyntax(pcid, TransferSyntax.ExplicitVrLittleEndian);
                }
            }
        }

        public void Send(String localAE, String remoteAE, String host, int port)
        {
            if (_dicomClient == null)
            {
                if (_fileList.Count == 0)
                {
                    Logger.LogInfo("Not sending, no files to send.");
                    return;
                }

                Logger.LogInfo("Preparing to connect to AE {0} on host {1} on port {2} and sending {3} images.", remoteAE, host, port, _fileList.Count);

                try
                {
                    IPAddress addr = null;
                    foreach (IPAddress dnsAddr in Dns.GetHostAddresses(host))
                        if (dnsAddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            addr = dnsAddr;
                            break;
                        }
                    if (addr == null)
                    {
                        Logger.LogError("No Valid IP addresses for host {0}", host);
                        return;
                    }
                    _assocParams = new ClientAssociationParameters(localAE, remoteAE, new IPEndPoint(addr, port));

                    SetPresentationContexts();

                    _dicomClient = DicomClient.Connect(_assocParams, this);
                }
                catch (Exception e)
                {
                    Logger.LogErrorException(e, "Unexpected exception trying to connect to Remote AE {0} on host {1} on port {2}", remoteAE, host, port);
                }
            }
        }

        /// <summary>
        /// Generic routine to send the next C-STORE-RQ message in the _fileList.
        /// </summary>
        /// <param name="client">DICOM Client class</param>
        /// <param name="association">Association Parameters</param>
        public bool SendCStore(DicomClient client, ClientAssociationParameters association)
        {
            FileToSend fileToSend = _fileList[_fileListIndex];

            DicomFile dicomFile = new DicomFile(fileToSend.filename);

            try
            {
                dicomFile.Load(DicomReadOptions.Default);
            }
            catch (DicomException e)
            {
                Logger.LogErrorException(e, "Unexpected exception when loading DICOM file {0}",fileToSend.filename);

                return false;
            }

            DicomMessage msg = new DicomMessage(dicomFile);

            byte pcid = association.FindAbstractSyntaxWithTransferSyntax(fileToSend.sopClass, dicomFile.TransferSyntax);
			if (pcid == 0)
			{
				if (dicomFile.TransferSyntax.Equals(TransferSyntax.ImplicitVrLittleEndian))
					pcid = association.FindAbstractSyntaxWithTransferSyntax(fileToSend.sopClass, TransferSyntax.ExplicitVrLittleEndian);
				if (pcid == 0)
				{
					Logger.LogError(
						"Unable to find matching negotiated presentation context for sop {0} and syntax {1}",
						dicomFile.SopClass.Name, dicomFile.TransferSyntax.Name);
					return false;
				}
			}
        	client.SendCStoreRequest(pcid, client.NextMessageID(), DicomPriority.Medium, msg);
            return true;
        }
        #endregion

        #region IDicomClientHandler Members

        public void OnReceiveAssociateAccept(DicomClient client, ClientAssociationParameters association)
        {
            Logger.LogInfo("Association Accepted:\r\n{0}", association.ToString());

            _fileListIndex = 0;

            bool ok = SendCStore(client, association);
            while (ok == false)
            {
                _fileListIndex++;
                if (_fileListIndex >= _fileList.Count)
                {
                    Logger.LogInfo("Completed sending C-STORE-RQ messages, releasing association.");
                    client.SendReleaseRequest();
                    return;
                }
                ok = SendCStore(client, association);
            }
        }

        public void OnReceiveAssociateReject(DicomClient client, ClientAssociationParameters association, DicomRejectResult result, DicomRejectSource source, DicomRejectReason reason)
        {
            Logger.LogInfo("Association Rejection when {0} connected to remote AE {1}", association.CallingAE, association.CalledAE);
            _dicomClient = null;
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
                Logger.LogError("Failure status received in sending C-STORE: {0}", message.Status.Description);
            }

            bool ok = false;
            while (ok == false)
            {
                _fileListIndex++;
                if (_fileListIndex >= _fileList.Count)
                {
                    Logger.LogInfo("Completed sending C-STORE-RQ messages, releasing association.");
                    client.SendReleaseRequest();
                    return;
                }

                ok = SendCStore(client, association);
            }
        }

        public void OnReceiveReleaseResponse(DicomClient client, ClientAssociationParameters association)
        {
            Logger.LogInfo("Association released to {0}", association.CalledAE);
            _dicomClient = null;
        }

        public void OnReceiveAbort(DicomClient client, ClientAssociationParameters association, DicomAbortSource source, DicomAbortReason reason)
        {
            Logger.LogError("Unexpected association abort received from {0}", association.CalledAE);
            _dicomClient = null;
        }

        public void OnNetworkError(DicomClient client, ClientAssociationParameters association, Exception e)
        {
            Logger.LogErrorException(e, "Unexpected network error");
            _dicomClient = null;
        }

        public void OnDimseTimeout(DicomClient client, ClientAssociationParameters association)
        {
            Logger.LogInfo("Timeout waiting for response message, continuing.");
        }

        #endregion
    }
}
