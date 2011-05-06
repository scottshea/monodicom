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

using System.Collections.Generic;
using System.IO;
using System.Text;
using ClearCanvas.Dicom.Tests;
using NUnit.Framework;

namespace ClearCanvas.Dicom.Codec.Tests
{
	[TestFixture]
	public class CodecTest : AbstractCodecTest
	{
		[Test]
		public void Test()
		{
			DicomFile file = CreateFile(256, 256, "MONOCHROME1", 12, 16, true, 1);
			file.Filename = "Monochrome1TestPattern.dcm";
			file.TransferSyntax = TransferSyntax.ExplicitVrLittleEndian;
            file.Save();

			file = CreateFile(256, 256, "MONOCHROME2", 14, 16, true, 1);
			file.Filename = "Monochrome2TestPattern.dcm";
            file.TransferSyntax = TransferSyntax.ExplicitVrLittleEndian;
            file.Save();

			file = CreateFile(256, 256, "RGB", 8, 8, false, 1);
            file.TransferSyntax = TransferSyntax.ExplicitVrLittleEndian;
			file.Filename = "RgbColorTestPattern.dcm";
			file.Save();

			file = CreateFile(256, 256, "YBR_FULL", 8, 8, false, 1);
			file.TransferSyntax = TransferSyntax.ExplicitVrLittleEndian;
			file.Filename = "YbrColorTestPattern.dcm";
			file.Save();

		}
	}
	public class AbstractCodecTest : AbstractTest
	{

		public static void LosslessImageTest(TransferSyntax syntax, DicomFile theFile)
		{
			if (File.Exists(theFile.Filename))
				File.Delete(theFile.Filename);

			DicomFile saveCopy = new DicomFile(theFile.Filename, theFile.MetaInfo.Copy(), theFile.DataSet.Copy());

			theFile.ChangeTransferSyntax(syntax);

			theFile.Save(DicomWriteOptions.ExplicitLengthSequence);

			DicomFile newFile = new DicomFile(theFile.Filename);

			newFile.Load(DicomReadOptions.Default);

			newFile.ChangeTransferSyntax(saveCopy.TransferSyntax);

			List<DicomAttributeComparisonResult> list = new List<DicomAttributeComparisonResult>();
			bool result = newFile.DataSet.Equals(saveCopy.DataSet, ref list);

			StringBuilder sb = new StringBuilder();
			foreach (DicomAttributeComparisonResult compareResult in list)
				sb.AppendFormat("Comparison Failure: {0}, ", compareResult.Details);

			Assert.IsTrue(result,sb.ToString());
		}

		public static void LosslessImageTestWithConversion(TransferSyntax syntax, DicomFile theFile)
		{
			if (File.Exists(theFile.Filename))
				File.Delete(theFile.Filename);

			DicomFile saveCopy = new DicomFile(theFile.Filename, theFile.MetaInfo.Copy(), theFile.DataSet.Copy());

			theFile.ChangeTransferSyntax(syntax);

			theFile.Save(DicomWriteOptions.ExplicitLengthSequence);

			DicomFile newFile = new DicomFile(theFile.Filename);

			newFile.Load(DicomReadOptions.Default);

			newFile.ChangeTransferSyntax(saveCopy.TransferSyntax);

			Assert.IsFalse(newFile.DataSet.Equals(saveCopy.DataSet));
		}

		public static void LossyImageTest(TransferSyntax syntax, DicomFile theFile)
		{
			if (File.Exists(theFile.Filename))
				File.Delete(theFile.Filename);

			DicomFile saveCopy = new DicomFile(theFile.Filename, theFile.MetaInfo.Copy(), theFile.DataSet.Copy());

			theFile.ChangeTransferSyntax(syntax);

			theFile.Save(DicomWriteOptions.ExplicitLengthSequence);

			DicomFile newFile = new DicomFile(theFile.Filename);

			newFile.Load(DicomReadOptions.Default);

			newFile.ChangeTransferSyntax(saveCopy.TransferSyntax);

			Assert.IsFalse(newFile.DataSet.Equals(saveCopy.DataSet));

			Assert.IsTrue(newFile.DataSet.Contains(DicomTags.DerivationDescription));
			Assert.IsTrue(newFile.DataSet.Contains(DicomTags.LossyImageCompression));
			Assert.IsTrue(newFile.DataSet.Contains(DicomTags.LossyImageCompressionMethod));
			Assert.IsTrue(newFile.DataSet.Contains(DicomTags.LossyImageCompressionRatio));

			Assert.IsFalse(newFile.DataSet[DicomTags.DerivationDescription].IsEmpty);
			Assert.IsFalse(newFile.DataSet[DicomTags.LossyImageCompression].IsEmpty);
			Assert.IsFalse(newFile.DataSet[DicomTags.LossyImageCompressionMethod].IsEmpty);
			Assert.IsFalse(newFile.DataSet[DicomTags.LossyImageCompressionRatio].IsEmpty);

			Assert.IsFalse(newFile.DataSet[DicomTags.DerivationDescription].IsNull);
			Assert.IsFalse(newFile.DataSet[DicomTags.LossyImageCompression].IsNull);
			Assert.IsFalse(newFile.DataSet[DicomTags.LossyImageCompressionMethod].IsNull);
			Assert.IsFalse(newFile.DataSet[DicomTags.LossyImageCompressionRatio].IsNull);

		}

		public static void ExpectedFailureTest(TransferSyntax syntax, DicomFile theFile)
		{
			try
			{
				theFile.ChangeTransferSyntax(syntax);
			}
			catch (DicomCodecUnsupportedSopException)
			{
				return;
			}

			Assert.IsTrue(false, "Unexpected successfull compression of object.");
		}
	}
}
