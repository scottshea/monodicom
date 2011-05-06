using Monodicom.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Dicom.Data;
using System.Collections.Generic;

namespace Monodicom.Tests
{
    
    
    /// <summary>
    ///This is a test class for UpdateDataTest and is intended
    ///to contain all UpdateDataTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UpdateDataTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for UpdateData Constructor
        ///</summary>
        [TestMethod()]
        public void UpdateDataConstructorTest()
        {
            UpdateData target = new UpdateData();
            string testObjectCreation = "";
            testObjectCreation = target.ToString();
            Assert.AreNotSame("", testObjectCreation);
        }

        /// <summary>
        ///A test for DicomFileName
        ///</summary>
        [TestMethod()]
        public void DicomFileNameTest()
        {
            UpdateData target = new UpdateData();
            string expected = "DicomTestFileName.dcm";
            string actual;
            target.DicomFileName = expected;
            actual = target.DicomFileName;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DicomFilePath
        ///</summary>
        [TestMethod()]
        public void DicomFilePathTest()
        {
            UpdateData target = new UpdateData();
            string expected = "C:\\TestPath\\TestDicomFileLocation";
            string actual;
            target.DicomFilePath = expected;
            actual = target.DicomFilePath;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UpdateDataset
        ///</summary>
        [TestMethod()]
        public void UpdateDatasetTest()
        {
            UpdateData target = new UpdateData();
            Dictionary<DicomTag, string> expected = new Dictionary<DicomTag,string>();
            expected.Add(DicomTag.Parse("0008,0090"),"Random^Doctor");
            Dictionary<DicomTag, string> actual;
            target.UpdateDataset = expected;
            actual = target.UpdateDataset;
            Assert.AreEqual(expected, actual);  //TODO: See if this is how this should work...
        }

        /// <summary>
        ///A test for UpdateMetadata
        ///</summary>
        [TestMethod()]
        public void UpdateMetadataTest()
        {
            UpdateData target = new UpdateData(); 
            Dictionary<DicomTag, string> expected = new Dictionary<DicomTag,string>();
            expected.Add(DicomTag.Parse("0002,0016"), "TEST_AE");
            Dictionary<DicomTag, string> actual;
            target.UpdateMetadata = expected;
            actual = target.UpdateMetadata;
            Assert.AreEqual(expected, actual); //TODO: See if this is how this should work...
        }
    }
}
