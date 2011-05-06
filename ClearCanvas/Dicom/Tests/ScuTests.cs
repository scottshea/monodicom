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

#if UNIT_TESTS

using System.Collections.Generic;
using System.Net;
using ClearCanvas.Dicom.Network;
using ClearCanvas.Dicom.Network.Scu;
using NUnit.Framework;

namespace ClearCanvas.Dicom.Tests
{
	[TestFixture]
	public class ScuTests : AbstractTest
	{
		[TestFixtureSetUp]
		public void Init()
		{
			_serverType = TestTypes.Receive;
		}

		[TestFixtureTearDown]
		public void Cleanup()
		{
		}

		TestTypes _serverType;

		public IDicomServerHandler ServerHandlerCreator(DicomServer server, ServerAssociationParameters assoc)
		{
			return new ServerHandler(this, _serverType);
		}

		private StorageScu SetupScu()
		{
			StorageScu scu = new StorageScu("TestAe", "AssocTestServer", "localhost", 2112);

			IList<DicomAttributeCollection> list = SetupMRSeries(4, 2, DicomUid.GenerateUid().UID);

			foreach (DicomAttributeCollection collection in list)
			{
				DicomFile file = new DicomFile("test", new DicomAttributeCollection(), collection);
				file.TransferSyntax = TransferSyntax.ExplicitVrLittleEndian;
				file.MediaStorageSopClassUid = SopClass.MrImageStorage.Uid;
				file.MediaStorageSopInstanceUid = collection[DicomTags.SopInstanceUid].ToString();

				scu.AddStorageInstance(new StorageInstance(file));
			}

			return scu;
		}

		[Test]
		public void ScuAbortTest()
		{
			int port = 2112;

			/* Setup the Server */
			ServerAssociationParameters serverParameters = new ServerAssociationParameters("AssocTestServer", new IPEndPoint(IPAddress.Any, port));
			byte pcid = serverParameters.AddPresentationContext(SopClass.MrImageStorage);
			serverParameters.AddTransferSyntax(pcid, TransferSyntax.ExplicitVrLittleEndian);
			serverParameters.AddTransferSyntax(pcid, TransferSyntax.ExplicitVrBigEndian);
			serverParameters.AddTransferSyntax(pcid, TransferSyntax.ImplicitVrLittleEndian);

			_serverType = TestTypes.Receive;
			DicomServer.StartListening(serverParameters, ServerHandlerCreator);

			StorageScu scu = SetupScu();

			IList<DicomAttributeCollection> list = SetupMRSeries(4, 2, DicomUid.GenerateUid().UID);

			foreach (DicomAttributeCollection collection in list)
			{
				DicomFile file = new DicomFile("test",new DicomAttributeCollection(),collection );
				file.TransferSyntax = TransferSyntax.ExplicitVrLittleEndian;
				file.MediaStorageSopClassUid = SopClass.MrImageStorage.Uid;
				file.MediaStorageSopInstanceUid = collection[DicomTags.SopInstanceUid].ToString();

				scu.AddStorageInstance(new StorageInstance(file));
			}

			scu.ImageStoreCompleted += delegate(object o, StorageInstance instance)
			                           	{
											// Test abort
			                           		scu.Abort();
			                           	};

			scu.Send();
			scu.Join();

			Assert.AreEqual(scu.Status, ScuOperationStatus.NetworkError);

			// StopListening
			DicomServer.StopListening(serverParameters);
		}
	}
}

#endif
