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

using ClearCanvas.Dicom.Iod;
using NUnit.Framework;

namespace ClearCanvas.Dicom.Utilities.Tests
{
	[TestFixture]
	public class DicomStringHelperTests
	{
		public DicomStringHelperTests()
		{
		}

		[Test]
		public void TestStringArrayConverter()
		{
			string input = null;
			string[] output = DicomStringHelper.GetStringArray(input);
			Assert.AreEqual(output.Length, 0);

			input = "";
			output = DicomStringHelper.GetStringArray(input);
			Assert.AreEqual(output.Length, 0);

			input = @"the\lazy\\brown\dog";
			output = DicomStringHelper.GetStringArray(input);
			Assert.AreEqual(output[0], "the");
			Assert.AreEqual(output[1], "lazy");
			Assert.AreEqual(output[2], "brown");
			Assert.AreEqual(output[3], "dog");
		}

		[Test]
		public void TestDoubleArrayConverter()
		{
			string input = null;
			double[] output;
			DicomStringHelper.TryGetDoubleArray(input, out output);
			Assert.AreEqual(output.Length, 0);

			input = "";
			DicomStringHelper.TryGetDoubleArray(input, out output);
			Assert.AreEqual(output.Length, 0);

			input = @"0\1.2\2.3";
			DicomStringHelper.TryGetDoubleArray(input, out output);
			Assert.AreEqual(output[0], 0);
			Assert.AreEqual(output[1], 1.2);
			Assert.AreEqual(output[2], 2.3);
		}
		
		[Test]
		public void TestIntArrayConverter()
		{
			string input = null;
			int[] output;
			DicomStringHelper.TryGetIntArray(input, out output);
			Assert.AreEqual(output.Length, 0);

			input = "";
			DicomStringHelper.TryGetIntArray(input, out output);
			Assert.AreEqual(output.Length, 0);

			input = @"0\1\30";
			DicomStringHelper.TryGetIntArray(input, out output);
			Assert.AreEqual(output[0], 0);
			Assert.AreEqual(output[1], 1);
			Assert.AreEqual(output[2], 30);
		}

		[Test]
		public void TestPersonNameArrayConverter()
		{
			string input = null;
			PersonName[] output = DicomStringHelper.GetPersonNameArray(input);
			Assert.AreEqual(output.Length, 0);

			input = "";
			output = DicomStringHelper.GetPersonNameArray(input);
			Assert.AreEqual(output.Length, 0);

			input = @"Doe^John^^^\Doe^Jane^^^";
			output = DicomStringHelper.GetPersonNameArray(input);
			Assert.AreEqual(output[0].FirstName, "John");
			Assert.AreEqual(output[0].LastName, "Doe");

			Assert.AreEqual(output[1].FirstName, "Jane");
			Assert.AreEqual(output[1].LastName, "Doe");
		}
	}
}

#endif