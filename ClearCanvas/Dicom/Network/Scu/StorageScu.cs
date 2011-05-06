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
using ClearCanvas.Common;
using ClearCanvas.Dicom.Codec;

namespace ClearCanvas.Dicom.Network.Scu
{
	/// <summary>
	/// Storage SCU Component for sending DICOM instances (images).
	/// </summary>
	/// <remarks>You can use <see cref="Send"/> to call it synchronously, 
	/// and <see cref="BeginSend"/> to call it Asyncronously.  See example below for calling it asynchronously.</remarks>
	///<example>Here is an example to use send files Asynchronously.
	///<code><![CDATA[
	///   StorageScu _storageScu = null;
	///   private void ClientCalling()
	///   {
	///       _storageScu = new StorageScu("localAe", "remoteAe", "remoteHost", 1000);
	///       _storageScu.ImageStoreCompleted += new EventHandler<StorageInstance>(storageScu_StoreCompleted);
	///       _storageScu.AddFile(@"C:\somefile.dcm"); 
	///       _storageScu.AddStorageInstance(new StorageInstance("AnotherFile.dcm"));
	///       // Can also use AddStorageInstanceList and pass in a list.
	///       IAsyncResult asyncResult = _storageScu.BeginSend(new AsyncCallback(SendComplete), _storageScu);
	///   }
	///
	///   /// <summary>gets called when each file completes</summary>
	///   void storageScu_StoreCompleted(object sender, StorageInstance e)
	///   {
	///       StorageScu storageScu = (StorageScu)sender;
	///       //Now Do whatever with each store detail, for example:
	///       System.Diagnostics.Debug.Write(e.SendStatus);
	///   }
	///
	///   /// <summary>gets called when the send is totally finished</summary>
	///   private void SendComplete(IAsyncResult ar)
	///   {
	///       StorageScu storageScu = (StorageScu)ar.AsyncState;
	///       // Now do whatever we want with all the results, for example:
	///       System.Diagnostics.Debug.Write(storageScu.SuccessSubOperations);
	///   }
	///   ]]></code></example>
	public class StorageScu : ScuBase
	{
		#region Public Events/Delegates...
		/// <summary>
		/// Event called when a SOP Instance has completed being sent over the network.
		/// </summary>
		/// <remarks>
		/// Note that the event will be called on both failure and sucessful sending
		/// of a DICOM file.
		/// </remarks>
		/// <returns></returns>
		public event EventHandler<StorageInstance> ImageStoreCompleted;
		/// <summary>
		/// Occurs when an SOP Instance has started being sent over the network.
		/// </summary>
		public event EventHandler<StorageInstance> ImageStoreStarted;
		/// <summary>
		/// Delegate for starting Send in ASynch mode with <see cref="BeginSend"/>.
		/// </summary>
		public delegate void SendDelegate();

		#endregion

		#region Private Variables...
		private readonly List<StorageInstance> _storageInstanceList = new List<StorageInstance>();
		private Dictionary<string, TransferSyntax> _preferredSyntaxes;
		private int _fileListIndex;
		private readonly string _moveOriginatorAe;
		private readonly ushort _moveOriginatorMessageId;
		private int _warningSubOperations = 0;
		private int _failureSubOperations = 0;
		private int _successSubOperations = 0;
		private int _totalSubOperations = 0;
		private int _remainingSubOperations = 0;
		#endregion

		#region Constructors...
		/// <summary>
		/// Constructor for Storage SCU Component.
		/// </summary>
		/// <param name="localAe">The local AE title.</param>
		/// <param name="remoteAe">The remote AE title being connected to.</param>
		/// <param name="remoteHost">The hostname or IP address of the remote AE.</param>
		/// <param name="remotePort">The listen port of the remote AE.</param>
		public StorageScu(string localAe, string remoteAe, string remoteHost, int remotePort)
		{
			ClientAETitle = localAe;
			RemoteAE = remoteAe;
			RemoteHost = remoteHost;
			RemotePort = remotePort;
		}

		/// <summary>
		/// Constructor for Storage SCU Component.
		/// </summary>
		/// <param name="localAe">The local AE title.</param>
		/// <param name="remoteAe">The remote AE title being connected to.</param>
		/// <param name="remoteHost">The hostname or IP address of the remote AE.</param>
		/// <param name="remotePort">The listen port of the remote AE.</param>
		/// <param name="moveOriginatorAe">The Application Entity Title of the application that orginated this C-STORE association.</param>
		/// <param name="moveOrginatorMessageId">The Message ID of the C-MOVE-RQ message that orginated this C-STORE association.</param>
		public StorageScu(string localAe, string remoteAe, string remoteHost, int remotePort, string moveOriginatorAe, ushort moveOrginatorMessageId)
			: this(localAe, remoteAe, remoteHost, remotePort)
		{
			_moveOriginatorAe = moveOriginatorAe;
			_moveOriginatorMessageId = moveOrginatorMessageId;
		}
		#endregion

		#region Public Properties...
		/// <summary>
		/// Gets or sets the file list, which contains a list of all the files.
		/// </summary>
		/// <value>The file list.</value>
		public List<StorageInstance> StorageInstanceList
		{
			get { return _storageInstanceList; }
		}

		/// <summary>
		/// The number of tranferred SOP Instances which had a warning status.
		/// </summary>
		public int WarningSubOperations
		{
			get { return _warningSubOperations; }
		}

		/// <summary>
		/// The number of transferred SOP Instances that had a failure status.
		/// </summary>
		public int FailureSubOperations
		{
			get { return _failureSubOperations; }
		}

		/// <summary>
		/// The number of transferred SOP Instances that had a success status.
		/// </summary>
		public int SuccessSubOperations
		{
			get { return _successSubOperations; }
		}

		/// <summary>
		/// The total number of SOP Instances to transfer.
		/// </summary>
		public int TotalSubOperations
		{
			get { return _totalSubOperations; }
		}

		/// <summary>
		/// The number of remaining SOP Instances to transfer.
		/// </summary>
		public int RemainingSubOperations
		{
			get { return _remainingSubOperations; }
		}

		#endregion

		#region Public Methods...

		/// <summary>
		/// Set a list of preferred SOP Class and Transfer Syntax combinations for the association.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method supplies a list of preferred transfer syntaxes for SOP Classes. It is assumed a
		/// SOP Class will only be in the list once and that only one transfer syntax will be supplied
		/// per SOP Class.
		/// </para>
		/// <para>
		/// When the preferred syntax list is set, the <see cref="StorageScu"/> component will check for 
		/// preferred syntaxes for each SOP Class that is negotiated.  It will add a proposed presentation
		/// context for the preferred <see cref="TransferSyntax"/> and attempt to use this preferred syntax
		/// for transfering the object.  It will default to the encoding of the object if a preferred syntax
		/// is not negotiated successfully.
		/// </para>
		/// </remarks>
		/// <param name="list"></param>
		public void SetPreferredSyntaxList(IList<SupportedSop> list)
		{
			if (_preferredSyntaxes == null)
				_preferredSyntaxes = new Dictionary<string, TransferSyntax>();

			// Store the preferred syntaxs in a dictionary to make them easier to 
			// lookup later.
			foreach (SupportedSop sop in list)
			{
				_preferredSyntaxes.Add(sop.SopClass.Uid,sop.SyntaxList[0]);
			}
		}

		/// <summary>
		/// Sends the SOP Instances in the <see cref="StorageInstanceList"/>.
		/// </summary>
        public virtual void Send()
		{
			if (_storageInstanceList == null)
			{
				string message =
					String.Format(
						"Attempting to open a C-STORE SCU association from {0} to {1} without setting the files to send.",
						ClientAETitle, RemoteAE);
				Platform.Log(LogLevel.Error, message);
				throw new ApplicationException(message);
			}

			if (_storageInstanceList.Count == 0)
			{
				string message =
					String.Format("Not creating DICOM C-STORE SCU connection from {0} to {1}, no files to send.",
					              ClientAETitle, RemoteAE);
				Platform.Log(LogLevel.Error, message);
				throw new ApplicationException(message);
			}

			_totalSubOperations = _storageInstanceList.Count;
			_remainingSubOperations = _totalSubOperations;
			_failureSubOperations = 0;
			_successSubOperations = 0;
			_warningSubOperations = 0;

			LoadStorageInstanceInfo();

			if (!HasFilesToSend())
			{
				_remainingSubOperations = 0;
				_failureSubOperations = _storageInstanceList.Count;

				string message =
					String.Format("Not creating DICOM C-STORE SCU connection from {0} to {1}, no existing and valid files to send.",
					              ClientAETitle, RemoteAE);
				Platform.Log(LogLevel.Error, message);
				throw new ApplicationException(message);
			}
			else
			{
				Platform.Log(LogLevel.Info, "Preparing to connect to AE {0} on host {1} on port {2} and sending {3} images.",
				             RemoteAE, RemoteHost, RemotePort, _storageInstanceList.Count);

				try
				{
					// the connect launches the actual send in a background thread.
					Connect();
				}
				catch
				{
					FailRemaining(DicomStatuses.QueryRetrieveMoveDestinationUnknown);
					throw;
				}
			}
		}

		/// <summary>
		/// Determines whether has files to send, ie, all storage instances do not have a status of processing failure
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [has files to send]; otherwise, <c>false</c>.
		/// </returns>
		private bool HasFilesToSend()
		{
			foreach (StorageInstance storageInstance in _storageInstanceList)
			{
				if (storageInstance.SendStatus == null || storageInstance.SendStatus != DicomStatuses.ProcessingFailure)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Begins sending the files in <see cref="StorageInstanceList"/> in asynchronous mode.  See the example in the class comment
		/// for an example on how to use this.
		/// </summary>
		/// <param name="callback">The callback.</param>
		/// <param name="asyncState">State of the async.</param>
		/// <returns></returns>
		public IAsyncResult BeginSend(AsyncCallback callback, object asyncState)
		{
			SendDelegate sendDelegate = this.Send;
			return sendDelegate.BeginInvoke(callback, asyncState);
		}

		/// <summary>
		/// Ends the send (asynchronous mode).  See the example in the class comment
		/// for an example on how to use this.
		/// </summary>
		/// <param name="ar">The ar.</param>
		public void EndSend(IAsyncResult ar)
		{
			SendDelegate sendDelegate = ((System.Runtime.Remoting.Messaging.AsyncResult)ar).AsyncDelegate as SendDelegate;

			if (sendDelegate != null)
			{
				sendDelegate.EndInvoke(ar);
			}
			else
			{
				throw new InvalidOperationException("cannot end invoke, asynchresult is null");
			}
		}

		/// <summary>
		/// Adds the specified file to <see cref="StorageInstanceList"/>.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		public void AddFile(string fileName)
		{
			AddStorageInstance(new StorageInstance(fileName));
		}

		/// <summary>
		/// Adds the specified storage instanceto <see cref="StorageInstanceList"/>.
		/// </summary>
		/// <param name="storageInstance">The storage instance.</param>
		public void AddStorageInstance(StorageInstance storageInstance)
		{
			StorageInstanceList.Add(storageInstance);
		}

		/// <summary>
		/// Add a list of <see cref="StorageInstance"/>s to transfer with the class.
		/// </summary>
		/// <param name="list">The list of storage instances to transfer.</param>
		public void AddStorageInstanceList(IList<StorageInstance> list)
		{
			if (list != null)
				_storageInstanceList.AddRange(list);
		}
		#endregion

		#region Protected Virtual Methods...
		/// <summary>
		/// Is called when an image store is completed.
		/// </summary>
		/// <remarks>If there are any callers hooked up to the <see cref="ImageStoreCompleted"/> event, it will
		/// forward the <paramref name="storageInstance"/> to the callers.</remarks>
		/// <param name="storageInstance">The storage instance.</param>
		protected virtual void OnImageStoreCompleted(StorageInstance storageInstance)
		{
			EventHandler<StorageInstance> tempHandler = ImageStoreCompleted;
			if (tempHandler != null)
				tempHandler(this, storageInstance);
		}

		/// <summary>
		/// Is called when an image store is started.
		/// </summary>
		/// <param name="storageInstance">The storage instance.</param>
		protected virtual void OnImageStoreStarted(StorageInstance storageInstance)
		{
			EventHandler<StorageInstance> tempHandler = ImageStoreStarted;
			if (tempHandler != null)
				tempHandler(this, storageInstance);
		}

		#endregion

		#region Private Methods...
		/// <summary>
		/// Fail the remaining SOP Instances for sending.
		/// </summary>
		private void FailRemaining(DicomStatus status)
		{

			while (_fileListIndex < _storageInstanceList.Count)
			{
				StorageInstance fileToSend = _storageInstanceList[_fileListIndex];

				OnImageStoreStarted(fileToSend);

				fileToSend.SendStatus = status;

				_failureSubOperations++;
				_remainingSubOperations--;

				fileToSend.ExtendedFailureDescription = "The association was aborted.";

				OnImageStoreCompleted(fileToSend);

				_fileListIndex++;
			}
		}

		/// <summary>
		/// Load details about the list of files to transfer into memory.
		/// </summary>
		private void LoadStorageInstanceInfo()
		{
			foreach (StorageInstance instance in _storageInstanceList)
			{
				try
				{
					instance.LoadInfo();
					// Need to set pending, because checking for sendStatus == null in HasFilesToSend() does not work !!!
					instance.SendStatus = DicomStatuses.Pending;
				}
				catch (Exception ex)
				{
					instance.SendStatus = DicomStatuses.ProcessingFailure;
					instance.ExtendedFailureDescription = ex.GetType().Name + " " + ex.Message;
				}
			}
		}

		/// <summary>
		/// Generic routine to continue attempting to send C-STORE-RQ messages in the <see cref="StorageInstanceList"/> until one is successful.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This routine will continue attempting to send a C-STORE until one has successfully been sent or all
		/// SOP instances in the <see cref="StorageInstanceList"/> have been sent.  Possible failures are that 
		/// a SOP Class was not negotiated, or a failure happened reading the SOP Instance from disk.
		/// </para>
		/// </remarks>
		/// <param name="client">DICOM Client class</param>
		/// <param name="association">Association Parameters</param>
		private void SendCStoreUntilSuccess(DicomClient client, ClientAssociationParameters association)
		{
			bool ok = SendCStore(client, association);
			while (ok == false)
			{
				_fileListIndex++;
				if (_fileListIndex >= _storageInstanceList.Count)
				{
					Platform.Log(LogLevel.Info, "Completed sending C-STORE-RQ messages, releasing association.");
					client.SendReleaseRequest();
					StopRunningOperation();
					return;
				}
				ok = SendCStore(client, association);
			}
		}

		/// <summary>
		/// Generic routine to send the next C-STORE-RQ message in the <see cref="StorageInstanceList"/>.
		/// </summary>
		/// <param name="client">DICOM Client class</param>
		/// <param name="association">Association Parameters</param>
		private bool SendCStore(DicomClient client, ClientAssociationParameters association)
		{
			StorageInstance fileToSend = _storageInstanceList[_fileListIndex];

			OnImageStoreStarted(fileToSend);

			DicomFile dicomFile;

			try
			{
				// Check to see if image does not exist or is corrupted
				if (fileToSend.SendStatus == DicomStatuses.ProcessingFailure)
				{
					_failureSubOperations++;
					_remainingSubOperations--;
					OnImageStoreCompleted(fileToSend);
					return false;
				}

				dicomFile = fileToSend.LoadFile();
			}
			catch (DicomException e)
			{
				Platform.Log(LogLevel.Error, e, "Unexpected exception when loading DICOM file {0}", fileToSend.Filename);

				fileToSend.ExtendedFailureDescription = e.GetType().Name + " " + e.Message;
				_failureSubOperations++;
				_remainingSubOperations--;
				OnImageStoreCompleted(fileToSend);
				return false;
			}

			DicomMessage msg = new DicomMessage(dicomFile);

			byte pcid = 0;

			if (fileToSend.TransferSyntax.Encapsulated)
			{
				pcid = association.FindAbstractSyntaxWithTransferSyntax(fileToSend.SopClass, fileToSend.TransferSyntax);

				if (DicomCodecRegistry.GetCodec(fileToSend.TransferSyntax) != null)
				{
					if (pcid == 0)
						pcid = association.FindAbstractSyntaxWithTransferSyntax(fileToSend.SopClass,
						                                                        TransferSyntax.ExplicitVrLittleEndian);
					if (pcid == 0)
						pcid = association.FindAbstractSyntaxWithTransferSyntax(fileToSend.SopClass,
						                                                        TransferSyntax.ImplicitVrLittleEndian);
				}
			}
			else
			{
				if (pcid == 0)
					pcid = association.FindAbstractSyntaxWithTransferSyntax(fileToSend.SopClass,
					                                                        TransferSyntax.ExplicitVrLittleEndian);
				if (pcid == 0)
					pcid = association.FindAbstractSyntaxWithTransferSyntax(fileToSend.SopClass,
					                                                        TransferSyntax.ImplicitVrLittleEndian);
			}

			if (pcid == 0)
			{
				fileToSend.SendStatus = DicomStatuses.SOPClassNotSupported;
				fileToSend.ExtendedFailureDescription = "No valid presentation contexts for file.";
				OnImageStoreCompleted(fileToSend);
				_failureSubOperations++;
				_remainingSubOperations--;
				return false;
			}

			try
			{
				if (_moveOriginatorAe == null)
					client.SendCStoreRequest(pcid, client.NextMessageID(), DicomPriority.Medium, msg);
				else
					client.SendCStoreRequest(pcid, client.NextMessageID(), DicomPriority.Medium, _moveOriginatorAe,
					                         _moveOriginatorMessageId, msg);
			}
			catch(DicomNetworkException)
			{
				throw; //This is a DicomException-derived class that we want to throw.
			}
			catch(DicomCodecException e)
			{
				Platform.Log(LogLevel.Error, e, "Unexpected exception when compressing or decompressing file before send {0}", fileToSend.Filename);

				fileToSend.SendStatus = DicomStatuses.ProcessingFailure;
				fileToSend.ExtendedFailureDescription = "Error decompressing or compressing file before send.";
				OnImageStoreCompleted(fileToSend);
				_failureSubOperations++;
				_remainingSubOperations--;
				return false;

			}
			catch(DicomException e)
			{
				Platform.Log(LogLevel.Error, e, "Unexpected exception while sending file {0}", fileToSend.Filename);

				fileToSend.SendStatus = DicomStatuses.ProcessingFailure;
				fileToSend.ExtendedFailureDescription = "Unexpected exception while sending file.";
				OnImageStoreCompleted(fileToSend);
				_failureSubOperations++;
				_remainingSubOperations--;
				return false;
			}

			return true;
		}

		#endregion

		#region Protected Overridden Methods...
		/// <summary>
		/// Scan the files to send, and create presentation contexts for each abstract syntax to send.
		/// </summary>
		protected override void SetPresentationContexts()
		{
			foreach (StorageInstance sendStruct in _storageInstanceList)
			{
				// skip if failed in LoadStorageInstanceInfo, ie file not found
				if (sendStruct.SendStatus == DicomStatuses.ProcessingFailure)
					continue;

				if (sendStruct.TransferSyntax.Encapsulated)
				{
					// If the image is encapsulated, make sure there's an exact match of the transfer
					// syntax, if not, just make sure there's an unencapsulated transfer syntax.
					byte pcid = AssociationParameters.FindAbstractSyntaxWithTransferSyntax(sendStruct.SopClass,
					                                                                            sendStruct.TransferSyntax);

					if (pcid == 0)
					{
						pcid = AssociationParameters.AddPresentationContext(sendStruct.SopClass);

						AssociationParameters.AddTransferSyntax(pcid, sendStruct.TransferSyntax);
					}

					// Check for a codec, and if it exists, also register an uncompressed context.
					IDicomCodec codec = DicomCodecRegistry.GetCodec(sendStruct.TransferSyntax);
					if (codec != null)
					{
						pcid = AssociationParameters.FindAbstractSyntaxWithTransferSyntax(sendStruct.SopClass,
						                                                                       TransferSyntax.ExplicitVrLittleEndian);
						if (pcid == 0)
						{
							pcid = AssociationParameters.AddPresentationContext(sendStruct.SopClass);

							AssociationParameters.AddTransferSyntax(pcid, TransferSyntax.ExplicitVrLittleEndian);
							AssociationParameters.AddTransferSyntax(pcid, TransferSyntax.ImplicitVrLittleEndian);
						}
					}
				}
				else
				{
					byte pcid = AssociationParameters.FindAbstractSyntaxWithTransferSyntax(sendStruct.SopClass,
					                                                                            TransferSyntax.ExplicitVrLittleEndian);

					if (pcid == 0)
					{
						pcid = AssociationParameters.AddPresentationContext(sendStruct.SopClass);

						AssociationParameters.AddTransferSyntax(pcid, TransferSyntax.ExplicitVrLittleEndian);
						AssociationParameters.AddTransferSyntax(pcid, TransferSyntax.ImplicitVrLittleEndian);
					}
				}

				// Now add the preferred syntaxes, if its been set.
				if (_preferredSyntaxes != null)
				{
					TransferSyntax syntax;
					if (_preferredSyntaxes.TryGetValue(sendStruct.SopClass.Uid, out syntax))
					{
						byte pcid = AssociationParameters.FindAbstractSyntaxWithTransferSyntax(sendStruct.SopClass,
						                                                                            syntax);

						// If we have more than 1 transfer syntax associated with the preferred, we want to 
						// have a dedicated presentation context for the preferred, so that we ensure it
						// gets accepted if the SCP supports it.  This is only really going to happen if
						// the preferred is Explicit VR Little Endian or Implicit VR Little Endian.
						if ((pcid == 0) || (AssociationParameters.GetPresentationContextTransferSyntaxes(pcid).Count > 1))
						{
							pcid = AssociationParameters.AddPresentationContext(sendStruct.SopClass);

							AssociationParameters.AddTransferSyntax(pcid, syntax);
						}
					}
				}
			}
		}

		/// <summary>
		/// Called when received associate accept.  For StorageScu, we then attempt to send the first file.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="association">The association.</param>
		public override void OnReceiveAssociateAccept(DicomClient client, ClientAssociationParameters association)
		{
			base.OnReceiveAssociateAccept(client, association);

			Platform.Log(LogLevel.Info, "Association Accepted:\r\n{0}", association.ToString());

			_fileListIndex = 0;

			SendCStoreUntilSuccess(client,association);
		}

		/// <summary>
		/// Called when received response message.  If there are more files to send, will send them here.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="association">The association.</param>
		/// <param name="presentationID">The presentation ID.</param>
		/// <param name="message">The message.</param>
		public override void OnReceiveResponseMessage(DicomClient client, ClientAssociationParameters association, byte presentationID, DicomMessage message)
		{
			_storageInstanceList[_fileListIndex].SendStatus = message.Status;

			if (message.Status.Status != DicomState.Success)
			{
				if (message.Status.Status == DicomState.Warning)
				{
					_warningSubOperations++;
					Platform.Log(LogLevel.Warn, "Warning status received in sending C-STORE to {0}: {1}",
								 association.CalledAE, message.Status.Description);
				}
				else if (message.Status.Status == DicomState.Failure)
				{
					_failureSubOperations++;
					Platform.Log(LogLevel.Error, "Failure status received in sending C-STORE to {0}: {1}",
						association.CalledAE, message.Status.Description);
				}
			}
			else
				_successSubOperations++;

			_remainingSubOperations--;

			OnImageStoreCompleted(_storageInstanceList[_fileListIndex]);

			if (Status == ScuOperationStatus.Canceled || message.Status.Status == DicomState.Cancel)
			{
				FailRemaining(DicomStatuses.Cancel);
				Platform.Log(LogLevel.Info, "Cancel requested by {0}, releasing association from {1} to {2}",
					(message.Status.Status == DicomState.Cancel) ? "remote host" : "client", 
					association.CallingAE, association.CalledAE);

				Status = ScuOperationStatus.Canceled;
				client.SendReleaseRequest();
				StopRunningOperation();
				return;
			}

			_fileListIndex++;
			if (_fileListIndex >= _storageInstanceList.Count)
			{
				Platform.Log(LogLevel.Info, "Completed sending {0} C-STORE-RQ messages, releasing association.", _storageInstanceList.Count);
				client.SendReleaseRequest();
				StopRunningOperation();
				return;
			}
			else
				SendCStoreUntilSuccess(client, association);
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
