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

using System.Net;
using ClearCanvas.Dicom.Network;
using ClearCanvas.Dicom.Network.Scu;
using ClearCanvas.Dicom.Tests;
using NUnit.Framework;

namespace ClearCanvas.Dicom.Audit.Test
{

    [TestFixture]
    public class AuditTest : AbstractTest
    {
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			ClearCanvas.Common.Platform.SetExtensionFactory(new ClearCanvas.Common.Utilities.NullExtensionFactory());
		}

		[Test]
		public void ApplicationActivityAuditTest()
		{
			ApplicationActivityAuditHelper helper =
				new ApplicationActivityAuditHelper(
					new DicomAuditSource("testApp", "Site", AuditSourceTypeCodeEnum.ApplicationServerProcessTierInMultiTierSystem),
					EventIdentificationTypeEventOutcomeIndicator.Success, ApplicationActivityType.ApplicationStarted,
					new AuditProcessActiveParticipant("testApp"));

			helper.AddUserParticipant(new AuditPersonActiveParticipant("testUser", "test@test", "Test Name"));

			string output = helper.Serialize(true);

			Assert.IsNotEmpty(output);

			string failure;
			bool result = helper.Verify(out failure);

			Assert.IsTrue(result, failure);
		}

		[Test]
		public void AuditLogUsedAuditTest()
		{
			AuditLogUsedAuditHelper helper =
				new AuditLogUsedAuditHelper(
					new DicomAuditSource("testApp", "Site", AuditSourceTypeCodeEnum.ApplicationServerProcessTierInMultiTierSystem),
					EventIdentificationTypeEventOutcomeIndicator.Success, 
					"http://www.clearcanvas.ca");

			helper.AddActiveParticipant(new AuditPersonActiveParticipant("testUser", "test@test", "Test Name"));

			string output = helper.Serialize(true);

			Assert.IsNotEmpty(output);

			string failure;
			bool result = helper.Verify(out failure);

			Assert.IsTrue(result, failure);
		}

		[Test]
		public void BeginTransferringDicomInstancesAuditTest()
		{
			AssociationParameters parms = new ClientAssociationParameters("CLIENT", "SERVER",
																		  new IPEndPoint(new IPAddress(new byte[] { 2, 2, 2, 2 }),
																						 2));
			parms.LocalEndPoint = new IPEndPoint(new IPAddress(new byte[] { 1, 1, 1, 1 }),
												 1);


			BeginTransferringDicomInstancesAuditHelper helper =
				new BeginTransferringDicomInstancesAuditHelper(
					new DicomAuditSource("testApp", "Site", AuditSourceTypeCodeEnum.ApplicationServerProcessTierInMultiTierSystem),
					EventIdentificationTypeEventOutcomeIndicator.Success,
					parms, new AuditPatientParticipantObject("id1234", "Test Patient"));

			DicomAttributeCollection collection = new DicomAttributeCollection();
			SetupMR(collection);
			helper.AddStorageInstance(new StorageInstance(new DicomMessage(new DicomAttributeCollection(), collection)));
			
			string output = helper.Serialize(true);

			Assert.IsNotEmpty(output);

			string failure;
			bool result = helper.Verify(out failure);

			Assert.IsTrue(result, failure);
		}

		[Test]
		public void DataExportAuditTest()
		{
			DataExportAuditHelper helper =
				new DataExportAuditHelper(
					new DicomAuditSource("testApp", "Site", AuditSourceTypeCodeEnum.ApplicationServerProcessTierInMultiTierSystem),
					EventIdentificationTypeEventOutcomeIndicator.Success,
					"MEDIA123");

			helper.AddExporter(new AuditPersonActiveParticipant("testUser", "test@test", "Test Name"));

			DicomAttributeCollection collection = new DicomAttributeCollection();
			SetupMR(collection);
			helper.AddPatientParticipantObject(new AuditPatientParticipantObject(collection));
			helper.AddStorageInstance(new StorageInstance(new DicomMessage(new DicomAttributeCollection(), collection)));

			string output = helper.Serialize(true);

			Assert.IsNotEmpty(output);

			string failure;
			bool result = helper.Verify(out failure);

			Assert.IsTrue(result, failure);
		}

		[Test]
		public void DataImportAuditTest()
		{
			DataImportAuditHelper helper =
				new DataImportAuditHelper(
					new DicomAuditSource("testApp", "Site", AuditSourceTypeCodeEnum.ApplicationServerProcessTierInMultiTierSystem),
					EventIdentificationTypeEventOutcomeIndicator.Success,
					"MEDIA123");

			helper.AddImporter(new AuditPersonActiveParticipant("testUser", "test@test", "Test Name"));

			DicomAttributeCollection collection = new DicomAttributeCollection();
			SetupMR(collection);
			helper.AddPatientParticipantObject(new AuditPatientParticipantObject(collection));
			helper.AddStorageInstance(new StorageInstance(new DicomMessage(new DicomAttributeCollection(), collection)));

			string output = helper.Serialize(true);

			Assert.IsNotEmpty(output);

			string failure;
			bool result = helper.Verify(out failure);

			Assert.IsTrue(result, failure);
		}


		[Test]
		public void DicomInstancesAccessedAuditTest()
		{
			DicomInstancesAccessedAuditHelper helper =
				new DicomInstancesAccessedAuditHelper(
					new DicomAuditSource("testApp", "Site", AuditSourceTypeCodeEnum.ApplicationServerProcessTierInMultiTierSystem),
					EventIdentificationTypeEventOutcomeIndicator.Success,
					EventIdentificationTypeEventActionCode.R);

			helper.AddUser(new AuditPersonActiveParticipant("testUser", "test@test", "Test Name"));

			DicomAttributeCollection collection = new DicomAttributeCollection();
			SetupMR(collection);
			helper.AddPatientParticipantObject(new AuditPatientParticipantObject(collection));
			helper.AddStorageInstance(new StorageInstance(new DicomMessage(new DicomAttributeCollection(), collection)));

			string output = helper.Serialize(true);

			Assert.IsNotEmpty(output);

			string failure;
			bool result = helper.Verify(out failure);

			Assert.IsTrue(result, failure);
		}


		[Test]
		public void DicomInstancesTransferredAuditTest()
		{
			AssociationParameters parms = new ClientAssociationParameters("CLIENT", "SERVER",
			                                                              new IPEndPoint(new IPAddress(new byte[] {2, 2, 2, 2}),
			                                                                             2));
			parms.LocalEndPoint = new IPEndPoint(new IPAddress(new byte[] {1, 1, 1, 1}),
			                                     1);


			DicomInstancesTransferredAuditHelper helper =
				new DicomInstancesTransferredAuditHelper(
					new DicomAuditSource("testApp", "Site", AuditSourceTypeCodeEnum.ApplicationServerProcessTierInMultiTierSystem),
					EventIdentificationTypeEventOutcomeIndicator.Success,
					EventIdentificationTypeEventActionCode.R,
					parms);

			DicomAttributeCollection collection = new DicomAttributeCollection();
			SetupMultiframeXA(collection,128,128,2);
			helper.AddPatientParticipantObject(new AuditPatientParticipantObject(collection));
			helper.AddStorageInstance(new StorageInstance(new DicomMessage(new DicomAttributeCollection(), collection)));

			string output = helper.Serialize(true);

			Assert.IsNotEmpty(output);

			string failure;
			bool result = helper.Verify(out failure);

			Assert.IsTrue(result, failure);
		}

		[Test]
		public void DicomStudyDeletedAuditTest()
		{
			DicomStudyDeletedAuditHelper helper =
				new DicomStudyDeletedAuditHelper(
					new DicomAuditSource("testApp", "Site", AuditSourceTypeCodeEnum.ApplicationServerProcessTierInMultiTierSystem),
					EventIdentificationTypeEventOutcomeIndicator.Success);

			helper.AddUserParticipant(new AuditPersonActiveParticipant("testUser", "test@test", "Test Name"));

			DicomAttributeCollection collection = new DicomAttributeCollection();
			SetupMultiframeXA(collection, 128, 128, 2);
			helper.AddPatientParticipantObject(new AuditPatientParticipantObject(collection));
			helper.AddStorageInstance(new StorageInstance(new DicomMessage(new DicomAttributeCollection(), collection)));

			string output = helper.Serialize(true);

			Assert.IsNotEmpty(output);

			string failure;
			bool result = helper.Verify(out failure);

			Assert.IsTrue(result, failure);
		}

		[Test]
		public void NetworkEntryAuditTest()
		{
			NetworkEntryAuditHelper helper =
				new NetworkEntryAuditHelper(
					new DicomAuditSource("testApp", "Site", AuditSourceTypeCodeEnum.ApplicationServerProcessTierInMultiTierSystem),
					EventIdentificationTypeEventOutcomeIndicator.Success,NetworkEntryType.Attach,
					new AuditProcessActiveParticipant("testAe"));

			string output = helper.Serialize(true);

			Assert.IsNotEmpty(output);

			string failure;
			bool result = helper.Verify(out failure);

			Assert.IsTrue(result, failure);
		}

		[Test]
		public void OrderRecordAuditTest()
		{
			OrderRecordAuditHelper helper =
				new OrderRecordAuditHelper(
					new DicomAuditSource("testApp", "Site", AuditSourceTypeCodeEnum.ApplicationServerProcessTierInMultiTierSystem),
					EventIdentificationTypeEventOutcomeIndicator.Success,EventIdentificationTypeEventActionCode.C);

			helper.AddUserParticipant(new AuditPersonActiveParticipant("testUser", "test@test", "Test Name"));
			helper.AddPatientParticipantObject(new AuditPatientParticipantObject("id1234", "Test Patient"));

			string output = helper.Serialize(true);

			Assert.IsNotEmpty(output);

			string failure;
			bool result = helper.Verify(out failure);

			Assert.IsTrue(result, failure);
		}

		[Test]
		public void PatientRecordAuditTest()
		{
			PatientRecordAuditHelper helper =
				new PatientRecordAuditHelper(
					new DicomAuditSource("testApp", "Site", AuditSourceTypeCodeEnum.ApplicationServerProcessTierInMultiTierSystem),
					EventIdentificationTypeEventOutcomeIndicator.Success, EventIdentificationTypeEventActionCode.C);

			helper.AddUserParticipant(new AuditPersonActiveParticipant("testUser", "test@test", "Test Name"));
			helper.AddPatientParticipantObject(new AuditPatientParticipantObject("id1234", "Test Patient"));

			string output = helper.Serialize(true);

			Assert.IsNotEmpty(output);

			string failure;
			bool result = helper.Verify(out failure);

			Assert.IsTrue(result, failure);
		}

		[Test]
		public void ProcedureRecordAuditTest()
		{
			ProcedureRecordAuditHelper helper =
				new ProcedureRecordAuditHelper(
					new DicomAuditSource("testApp", "Site", AuditSourceTypeCodeEnum.ApplicationServerProcessTierInMultiTierSystem),
					EventIdentificationTypeEventOutcomeIndicator.Success, EventIdentificationTypeEventActionCode.C);

			helper.AddUserParticipant(new AuditPersonActiveParticipant("testUser", "test@test", "Test Name"));
			DicomAttributeCollection collection = new DicomAttributeCollection();
			SetupMR(collection);
			helper.AddPatientParticipantObject(new AuditPatientParticipantObject(collection));
			helper.AddStorageInstance(new StorageInstance(new DicomMessage(new DicomAttributeCollection(), collection)));

			string output = helper.Serialize(true);

			Assert.IsNotEmpty(output);

			string failure;
			bool result = helper.Verify(out failure);

			Assert.IsTrue(result, failure);
		}

    	[Test]
        public void QueryAuditTest()
        {
        	AssociationParameters parms = new ClientAssociationParameters("CLIENT", "SERVER",
        	                                                         new IPEndPoint(new IPAddress(new byte[] {2, 2, 2, 2}),
        	                                                                       2));
			parms.LocalEndPoint = new IPEndPoint(new IPAddress(new byte[] {1, 1, 1, 1}),
        	                                                                       1);

        	QueryAuditHelper helper =
        		new QueryAuditHelper(new DicomAuditSource("testApplication"), EventIdentificationTypeEventOutcomeIndicator.Success, parms);

			helper.AddOtherParticipant(new AuditPersonActiveParticipant("testUser","test@test","Test Name"));
        	helper.AddPatientParticipantObject(new AuditPatientParticipantObject("id1234", "Test Patient"));

			AuditStudyParticipantObject study = new AuditStudyParticipantObject("1.2.3.4.5", "A1234", "1.2.3");
        	study.AddSopClass("1.2.3", 5);
			helper.AddStudyParticipantObject(study);

        	string output = helper.Serialize(true);

        	Assert.IsNotEmpty(output);

        	string failure;
        	bool result = helper.Verify(out failure);

        	Assert.IsTrue(result, failure);

			helper = new QueryAuditHelper(new DicomAuditSource("testApplication2","enterpriseId", AuditSourceTypeCodeEnum.EndUserInterface),EventIdentificationTypeEventOutcomeIndicator.Success, parms);
			helper.AddStudyParticipantObject(new AuditStudyParticipantObject("1.2.3.4.5"));

			output = helper.Serialize(true);

			Assert.IsNotEmpty(output);

			result = helper.Verify(out failure);

			Assert.IsTrue(result, failure);              
        }

		[Test]
		public void SecurityAlertAuditTest()
		{
			SecurityAlertAuditHelper helper =
				new SecurityAlertAuditHelper(
					new DicomAuditSource("testApp", "Site", AuditSourceTypeCodeEnum.ApplicationServerProcessTierInMultiTierSystem),
					EventIdentificationTypeEventOutcomeIndicator.Success,SecurityAlertEventTypeCodeEnum.NodeAuthentication);
			helper.AddReportingUser(new AuditProcessActiveParticipant("serverAe"));
			helper.AddActiveParticipant(new AuditPersonActiveParticipant("testUser", "test@test", "Test Name"));

			string output = helper.Serialize(true);

			Assert.IsNotEmpty(output);

			string failure;
			bool result = helper.Verify(out failure);

			Assert.IsTrue(result, failure);
		}

		[Test]
		public void UserAuthenticationAuditTest()
		{
			UserAuthenticationAuditHelper helper =
				new UserAuthenticationAuditHelper(
					new DicomAuditSource("testApp", "Site", AuditSourceTypeCodeEnum.ApplicationServerProcessTierInMultiTierSystem),
					EventIdentificationTypeEventOutcomeIndicator.Success,UserAuthenticationEventType.Login);
			helper.AddNode(new AuditProcessActiveParticipant("serverAe"));
			helper.AddUserParticipant(new AuditPersonActiveParticipant("testUser", "test@test", "Test Name"));

			string output = helper.Serialize(true);

			Assert.IsNotEmpty(output);

			string failure;
			bool result = helper.Verify(out failure);

			Assert.IsTrue(result, failure);
		}
	}
}

#endif