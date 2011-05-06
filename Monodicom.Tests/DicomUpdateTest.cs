using Monodicom.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Monodicom.Tests
{
    
    
    /// <summary>
    ///This is a test class for DicomUpdateTest and is intended
    ///to contain all DicomUpdateTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DicomUpdateTest
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
        ///A test for DicomUpdate Constructor
        ///</summary>
        [TestMethod()]
        public void DicomUpdateConstructorTest()
        {
            DicomUpdate target = new DicomUpdate();
            string dicomUpdateObject = target.ToString();
            Assert.AreNotEqual("", dicomUpdateObject);
        }

        /// <summary>
        ///A test for UpdateDicomFile
        ///</summary>
        [TestMethod()]
        public void UpdateDicomFileTest()
        {
            DicomUpdate target = new DicomUpdate();
            UpdateData updateData = new UpdateData();
            string[] files;
            string path = ".\\Monodicom.Tests\\TestStudies\\RFStudy";
            updateData.DicomFilePath = path;
            files = Directory.GetFiles(path, ".dcm");
            target.UpdateDicomFile(updateData);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UpdateDicomFile
        ///</summary>
        [TestMethod()]
        public void UpdateDicomFileTest1()
        {
            DicomUpdate target = new DicomUpdate(); // TODO: Initialize to an appropriate value
            string updateFile = string.Empty; // TODO: Initialize to an appropriate value
            string dicomTag = string.Empty; // TODO: Initialize to an appropriate value
            string newValue = string.Empty; // TODO: Initialize to an appropriate value
            target.UpdateDicomFile(updateFile, dicomTag, newValue);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
