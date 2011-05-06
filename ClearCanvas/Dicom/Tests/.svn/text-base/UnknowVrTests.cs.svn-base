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
using System.Collections.Generic;
using ClearCanvas.Dicom.IO;
using NUnit.Framework;

namespace ClearCanvas.Dicom.Tests
{
	[TestFixture]
	public class UnknowVrTests : AbstractTest
	{
		[Test]
		public void PrivateAttributeTest()
		{

			DicomFile file = new DicomFile("LittleEndianPrivateReadFileTest.dcm");

			DicomAttributeCollection dataSet = file.DataSet;

			SetupMR(dataSet);

			DicomTag privateCreatorTag = new DicomTag(0x00090010, "PrivateCreator", "PrivateCreator", DicomVr.LOvr, false, 1, 1,
													  false);
			dataSet[privateCreatorTag].SetStringValue("ClearCanvasGroup9");

			DicomTag privateTagFL = new DicomTag(0x00091020, "PrivateFL", "PrivateFL", DicomVr.FLvr, true, 1, 10, false);
			DicomTag privateTagLO = new DicomTag(0x00091021, "PrivateLO", "PrivateLO", DicomVr.LOvr, true, 1, 10, false);
			DicomTag privateTagAE = new DicomTag(0x00091022, "PrivateAE", "PrivateAE", DicomVr.AEvr, true, 1, 10, false);
			DicomTag privateTagAS = new DicomTag(0x00091023, "PrivateAS", "PrivateAS", DicomVr.ASvr, true, 1, 10, false);
			DicomTag privateTagAT = new DicomTag(0x00091024, "PrivateAT", "PrivateAT", DicomVr.ATvr, true, 1, 10, false);
			DicomTag privateTagCS = new DicomTag(0x00091025, "PrivateCS", "PrivateCS", DicomVr.CSvr, true, 1, 10, false);
			DicomTag privateTagDA = new DicomTag(0x00091026, "PrivateDA", "PrivateDA", DicomVr.DAvr, true, 1, 10, false);
			DicomTag privateTagDS = new DicomTag(0x00091027, "PrivateDS", "PrivateDS", DicomVr.DSvr, true, 1, 10, false);
			DicomTag privateTagDT = new DicomTag(0x00091028, "PrivateDT", "PrivateDT", DicomVr.DTvr, true, 1, 10, false);
			DicomTag privateTagFD = new DicomTag(0x00091029, "PrivateFD", "PrivateFD", DicomVr.FDvr, true, 1, 10, false);
			DicomTag privateTagIS = new DicomTag(0x00091030, "PrivateIS", "PrivateIS", DicomVr.ISvr, true, 1, 10, false);
			DicomTag privateTagLT = new DicomTag(0x00091031, "PrivateLT", "PrivateLT", DicomVr.LTvr, true, 1, 1, false);
			DicomTag privateTagOB = new DicomTag(0x00091032, "PrivateOB", "PrivateOB", DicomVr.OBvr, true, 1, 1, false);
			DicomTag privateTagOF = new DicomTag(0x00091033, "PrivateOF", "PrivateOF", DicomVr.OFvr, true, 1, 1, false);
			DicomTag privateTagOW = new DicomTag(0x00091034, "PrivateOW", "PrivateOW", DicomVr.OWvr, true, 1, 1, false);
			DicomTag privateTagPN = new DicomTag(0x00091035, "PrivatePN", "PrivatePN", DicomVr.PNvr, true, 1, 10, false);
			DicomTag privateTagSH = new DicomTag(0x00091036, "PrivateSH", "PrivateSH", DicomVr.SHvr, true, 1, 10, false);
			DicomTag privateTagSL = new DicomTag(0x00091037, "PrivateSL", "PrivateSL", DicomVr.SLvr, true, 1, 10, false);
			DicomTag privateTagSQ = new DicomTag(0x00091038, "PrivateSQ", "PrivateSQ", DicomVr.SQvr, true, 1, 10, false);
			DicomTag privateTagSS = new DicomTag(0x00091039, "PrivateSS", "PrivateSS", DicomVr.SSvr, true, 1, 10, false);
			DicomTag privateTagST = new DicomTag(0x00091040, "PrivateST", "PrivateST", DicomVr.STvr, true, 1, 10, false);
			DicomTag privateTagTM = new DicomTag(0x00091041, "PrivateTM", "PrivateTM", DicomVr.TMvr, true, 1, 10, false);
			DicomTag privateTagUI = new DicomTag(0x00091042, "PrivateUI", "PrivateUI", DicomVr.UIvr, true, 1, 10, false);
			DicomTag privateTagUL = new DicomTag(0x00091043, "PrivateUI", "PrivateUI", DicomVr.ULvr, true, 1, 10, false);
			DicomTag privateTagUS = new DicomTag(0x00091044, "PrivateUS", "PrivateUS", DicomVr.USvr, true, 1, 10, false);
			DicomTag privateTagUT = new DicomTag(0x00091045, "PrivateUT", "PrivateUT", DicomVr.UTvr, true, 1, 1, false);

			List<DicomTag> tagList = new List<DicomTag>();

			
			dataSet[privateTagFL].AppendFloat32(1.1f);
			dataSet[privateTagFL].AppendFloat32(1.1123132f);
			tagList.Add(privateTagFL);
			dataSet[privateTagLO].AppendString("Test");
			dataSet[privateTagLO].AppendString("Test Me 2");
			tagList.Add(privateTagLO);
			dataSet[privateTagAE].AppendString("TESTAE1");
			dataSet[privateTagAE].AppendString("TESTAE2");
			tagList.Add(privateTagAE);
			dataSet[privateTagAS].AppendString("003Y");
			dataSet[privateTagAS].AppendString("003D");
			tagList.Add(privateTagAS);
			dataSet[privateTagAT].AppendUInt32(DicomTags.ZoomFactor);
			dataSet[privateTagAT].AppendUInt32(DicomTags.PhotometricInterpretation);
			tagList.Add(privateTagAT);
			dataSet[privateTagCS].AppendString("CODE1");
			dataSet[privateTagCS].AppendString("CODE2");
			tagList.Add(privateTagCS);
			dataSet[privateTagDA].AppendDateTime(DateTime.Now);
			dataSet[privateTagDA].AppendDateTime(DateTime.Now);
			tagList.Add(privateTagDA);
			dataSet[privateTagDS].AppendFloat64(1.12351234124124f);
			dataSet[privateTagDS].AppendFloat64(-12312312312.1231f);
			tagList.Add(privateTagDS);
			dataSet[privateTagDT].AppendDateTime(DateTime.Now);
			dataSet[privateTagDT].AppendDateTime(DateTime.Now);
			tagList.Add(privateTagDT);
			dataSet[privateTagFD].AppendFloat64(1.112312d);
			dataSet[privateTagFD].AppendFloat64(-11123.13211d);
			tagList.Add(privateTagFD);
			dataSet[privateTagIS].AppendString("123456789");
			dataSet[privateTagIS].AppendString("123456789876");
			tagList.Add(privateTagIS);
			dataSet[privateTagLT].SetStringValue("Now is the time for all good men to come to the aide of their country.");
			tagList.Add(privateTagLT);
			dataSet[privateTagPN].AppendString("Last^First^Middle^Post");
			dataSet[privateTagPN].AppendString("W^Steven^R^Test");
			tagList.Add(privateTagPN);
			dataSet[privateTagST].SetStringValue("Now is the time for all good men to come to the aide of their country.");
			tagList.Add(privateTagST);
			dataSet[privateTagSH].AppendString("Short text 1");
			dataSet[privateTagSH].AppendString("Short text 2");
			dataSet[privateTagSH].AppendString("Short text 3");
			dataSet[privateTagSH].AppendString("Short text 4");
			tagList.Add(privateTagSH);
			dataSet[privateTagSL].AppendInt32(1024);
			dataSet[privateTagSL].AppendInt32(2048);
			dataSet[privateTagSL].AppendInt32(4096);
			dataSet[privateTagSL].AppendInt32(8192);
			dataSet[privateTagSL].AppendInt32(-1024);
			dataSet[privateTagSL].AppendInt32(-2048);
			dataSet[privateTagSL].AppendInt32(-4096);
			dataSet[privateTagSL].AppendInt32(-8192);
			tagList.Add(privateTagSL);
			dataSet[privateTagSS].AppendInt16(-1024);
			dataSet[privateTagSS].AppendInt16(512);
			dataSet[privateTagSS].AppendInt16(-256);
			dataSet[privateTagSS].AppendInt16(128);
			tagList.Add(privateTagSS);
			dataSet[privateTagTM].AppendDateTime(DateTime.Now);
			dataSet[privateTagTM].AppendDateTime(DateTime.Now);
			tagList.Add(privateTagTM);
			dataSet[privateTagUI].AppendUid(DicomUid.GenerateUid());
			dataSet[privateTagUI].AppendUid(DicomUid.GenerateUid());
			dataSet[privateTagUI].AppendUid(DicomUid.GenerateUid());
			dataSet[privateTagUI].AppendUid(DicomUid.GenerateUid());
			dataSet[privateTagUI].AppendUid(DicomUid.GenerateUid());
			dataSet[privateTagUI].AppendUid(DicomUid.GenerateUid());
			tagList.Add(privateTagUI);
			dataSet[privateTagUL].AppendUInt32(128);
			dataSet[privateTagUL].AppendUInt32(1024);
			dataSet[privateTagUL].AppendUInt32(16384);
			dataSet[privateTagUL].AppendUInt32(123123123);
			tagList.Add(privateTagUL);
			dataSet[privateTagUS].AppendUInt16(128);
			dataSet[privateTagUS].AppendUInt16(64);
			dataSet[privateTagUS].AppendUInt16(256);
			tagList.Add(privateTagUS);
			dataSet[privateTagUT].SetStringValue("A man, a plan, a canal, panama.");
			tagList.Add(privateTagUT);

			dataSet[privateTagOB].Values = new byte[] {0x01, 0x02, 0x03, 0x04, 0x05, 0x06};
			tagList.Add(privateTagOB);

			dataSet[privateTagOW].Values = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };
			tagList.Add(privateTagOW);

			dataSet[privateTagOF].Values = new[] { 1.1111f, 2.222f, 3.3333f, 44444.444f, 5555.555f, 66666.66666f, 123123.123123f };
			tagList.Add(privateTagOF);


			DicomSequenceItem item = new DicomSequenceItem();
			item[DicomTags.PhotometricInterpretation].AppendString("MONOCHROME1");
			item[DicomTags.Rows].AppendUInt16(256);
			item[DicomTags.Columns].AppendUInt16(256);
			item[DicomTags.BitsAllocated].AppendUInt16(8);
			item[DicomTags.BitsStored].AppendUInt16(8);
			item[DicomTags.HighBit].AppendUInt16(7);
			dataSet[privateTagSQ].AddSequenceItem(item);
			// SQ Attribute UN parsing does not work right now, don't add to the list.
			//tagList.Add(privateTagSQ);

			SetupMetaInfo(file);

			dataSet[DicomTags.StudyDescription].SetNullValue();

			// Little Endian Tests
			file.TransferSyntax = TransferSyntax.ImplicitVrLittleEndian;

			DicomReadOptions readOptions = DicomReadOptions.Default;

			// Use ExplicitLengthSequence to force SQ attributes to UN VR when they're ready back in
			bool result = file.Save(DicomWriteOptions.ExplicitLengthSequence);

			Assert.AreEqual(result, true);

			DicomFile newFile = new DicomFile(file.Filename);

			newFile.Load(readOptions);

			Assert.IsTrue(newFile.DataSet[DicomTags.StudyDescription].IsNull);

			Assert.AreNotEqual(file.DataSet.Equals(newFile.DataSet), true);

			foreach (DicomTag tag in tagList)
			{
				DicomAttributeUN unAttrib = newFile.DataSet[tag] as DicomAttributeUN;
				Assert.IsNotNull(unAttrib, String.Format("UN VR Attribute is not null for tag {0}",tag));

				ByteBuffer bb = unAttrib.GetByteBuffer(TransferSyntax.ImplicitVrLittleEndian,
														 newFile.DataSet[DicomTags.SpecificCharacterSet].ToString());
				Assert.IsNotNull(bb, String.Format("ByteBuffer not null for tag: {0}", tag));

				DicomAttribute validAttrib = tag.VR.CreateDicomAttribute(tag, bb);
				Assert.IsNotNull(validAttrib);

				Assert.IsTrue(validAttrib.Equals(file.DataSet[tag]), String.Format("Attributes equal for tag {0}", tag));
			}
		}
	}
}

#endif