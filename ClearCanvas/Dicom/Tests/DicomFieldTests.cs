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

using System;
using System.Globalization;
using NUnit.Framework;

namespace ClearCanvas.Dicom.Tests
{
    [TestFixture]
    public class DicomFieldTests : AbstractTest
    {
        public class TestFields
        {
            [DicomField(DicomTags.SopClassUid, DefaultValue = DicomFieldDefault.Default)]
            public DicomUid SopClassUid = null;

            [DicomField(DicomTags.SopInstanceUid, DefaultValue = DicomFieldDefault.Default)]
            public DicomUid SOPInstanceUID = null;

            [DicomField(DicomTags.StudyDate, DefaultValue = DicomFieldDefault.Default)]
            public DateTime StudyDate;

            [DicomField(DicomTags.AccessionNumber, DefaultValue = DicomFieldDefault.Default)]
            public string AccessionNumber = null;

            [DicomField(DicomTags.Modality, DefaultValue = DicomFieldDefault.Default)]
            public string Modality = null;

            [DicomField(DicomTags.StudyDescription, DefaultValue = DicomFieldDefault.Default)]
            public string StudyDescription = null;

            [DicomField(DicomTags.StudyInstanceUid, DefaultValue = DicomFieldDefault.Default)]
            public DicomUid StudyInstanceUID = null;

            [DicomField(DicomTags.SeriesInstanceUid, DefaultValue = DicomFieldDefault.Default)]
            public DicomUid SeriesInstanceUID = null;

            [DicomField(DicomTags.StudyId, DefaultValue = DicomFieldDefault.Default)]
            public string StudyID = null;

            [DicomField(DicomTags.PatientsName, DefaultValue = DicomFieldDefault.Default)]
            public string PatientsName = null;

            [DicomField(DicomTags.PatientId, DefaultValue = DicomFieldDefault.Default)]
            public string PatientID = null;

            [DicomField(DicomTags.PatientsBirthDate, DefaultValue = DicomFieldDefault.Default)]
            public DateTime PatientsBirthDate;

            [DicomField(DicomTags.PatientsSex, DefaultValue = DicomFieldDefault.Default)]
            public string PatientsSex = null;

            [DicomField(DicomTags.Rows, DefaultValue = DicomFieldDefault.Default)]
            public ushort Rows = 0;

            [DicomField(DicomTags.Columns, DefaultValue = DicomFieldDefault.Default)]
            public ushort Columns = 0;

            [DicomField(DicomTags.PixelSpacing, DefaultValue = DicomFieldDefault.Default)]
            public float PixelSpacing = 0.0f;

            [DicomField(DicomTags.InstanceNumber, DefaultValue = DicomFieldDefault.Default)]
            public int InstanceNumber = 0;

            [DicomField(DicomTags.ImageType, DefaultValue = DicomFieldDefault.Default)]
            public string[] ImageType = null;

            [DicomField(DicomTags.ImagePositionPatient, DefaultValue = DicomFieldDefault.Default)]
            public float[] ImagePositionPatient = null;
            
        }

        [Test]
        public void FieldTest()
        {
            DicomAttributeCollection theSet = new DicomAttributeCollection();
            TestFields theFields = new TestFields();

            SetupMR(theSet);

            theSet.LoadDicomFields(theFields);

            Assert.IsTrue(theFields.AccessionNumber.Equals(theSet[DicomTags.AccessionNumber].GetString(0,"")), "Accession Numbers did not match!");
            Assert.IsTrue(theFields.SopClassUid.UID.Equals(theSet[DicomTags.SopClassUid].GetString(0, "")), "SOP Class UIDs did not match!");
            Assert.IsTrue(theFields.SOPInstanceUID.UID.Equals(theSet[DicomTags.SopInstanceUid].GetString(0, "")), "SOP Class UIDs did not match!");
			Assert.IsTrue(theFields.StudyDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture).Equals(theSet[DicomTags.StudyDate].GetString(0, "")));
            Assert.IsTrue(theFields.Modality.Equals(theSet[DicomTags.Modality].GetString(0, "")), "Modality did not match!");
            Assert.IsTrue(theFields.StudyDescription.Equals(theSet[DicomTags.StudyDescription].GetString(0, "")), "Study Description did not match!");
            Assert.IsTrue(theFields.StudyInstanceUID.UID.Equals(theSet[DicomTags.StudyInstanceUid].GetString(0, "")), "Study Instance UIDs did not match!");
            Assert.IsTrue(theFields.SeriesInstanceUID.UID.Equals(theSet[DicomTags.SeriesInstanceUid].GetString(0, "")), "Series Instance UIDs did not match!");
            Assert.IsTrue(theFields.StudyID.Equals(theSet[DicomTags.StudyId].GetString(0, "")), "StudyID did not match!");
            Assert.IsTrue(theFields.PatientsName.Equals(theSet[DicomTags.PatientsName].GetString(0, "")), "PatientsName did not match!");
            Assert.IsTrue(theFields.PatientID.Equals(theSet[DicomTags.PatientId].GetString(0, "")), "PatientID did not match!");
			Assert.IsTrue(theFields.PatientsBirthDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture).Equals(theSet[DicomTags.PatientsBirthDate].GetString(0, "")));
            Assert.IsTrue(theFields.PatientsSex.Equals(theSet[DicomTags.PatientsSex].GetString(0, "")), "PatientsSex did not match!");
            Assert.IsTrue(theFields.Rows == theSet[DicomTags.Rows].GetUInt16(0,0));
            Assert.IsTrue(theFields.Columns == theSet[DicomTags.Columns].GetUInt16(0,0));
            float floatValue;
            theSet[DicomTags.PixelSpacing].TryGetFloat32(0, out floatValue);
            Assert.IsTrue(theFields.PixelSpacing == floatValue);
            int intValue;
            theSet[DicomTags.InstanceNumber].TryGetInt32(0, out intValue);
            Assert.IsTrue(theFields.InstanceNumber == intValue);
            //Assert.IsTrue(string.Join("\\", theFields.ImageType).Equals(theSet[DicomTags.ImageType].ToString()));
        }

    }
}

#endif