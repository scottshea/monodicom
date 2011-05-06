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
using NUnit.Framework;

#if UNIT_TESTS

namespace ClearCanvas.Dicom.DataStore.Tests
{
	internal class PathTest : DicomTagPath
	{
		public PathTest()
			: base()
		{ 
		}

		public void SetPath(string path)
		{
			base.Path = path;
		}

		public void SetPath(IList<DicomTag> tags)
		{
			base.TagsInPath = tags;
		}
	}

    [TestFixture]
    public class TypeTests
    {

        [TestFixtureSetUp]
        public void Init()
        {
        }

        [TestFixtureTearDown]
        public void Dispose()
        {
        }

		[Test]
		public void DicomTagPathTests()
		{
			DicomTagPath path = new PathTest();
			((PathTest)path).SetPath("(0010,0010)");

			Assert.AreEqual(path, "(0010,0010)"); 
			Assert.AreEqual(path, (uint)0x00100010);
			Assert.AreEqual(path, NewDicomTag(0x00100010));
			Assert.IsTrue(path.Equals("(0010,0010)"));
			Assert.IsTrue(path.Equals((uint)0x00100010));
			Assert.IsTrue(path.Equals(NewDicomTag(0x00100010)));

			((PathTest)path).SetPath("(0054,0220)\\(0054,0222)");
			Assert.AreEqual(path, "(0054,0220)\\(0054,0222)"); 
			Assert.IsFalse(path.Equals("(0054,0220)"));
			Assert.IsFalse(path.Equals((uint)0x00540220));
			Assert.IsFalse(path.Equals(NewDicomTag(0x00540220)));

			((PathTest)path).SetPath("(0054,0220)\\(0054,0222)\\(0010,0022)");
			Assert.AreEqual(path.ToString(), "(0054,0220)\\(0054,0222)\\(0010,0022)");

			path = new PathTest();
			((PathTest)path).SetPath(new List<DicomTag>(new DicomTag[] { NewDicomTag(0x00540220) }));
			Assert.AreEqual(path.ToString(), "(0054,0220)");

			((PathTest)path).SetPath(new List<DicomTag>(new DicomTag[] { NewDicomTag(0x00540220), NewDicomTag(0x00540222) }));
			Assert.AreEqual(path.ToString(), "(0054,0220)\\(0054,0222)");

			((PathTest)path).SetPath(new List<DicomTag>(new DicomTag[] { NewDicomTag(0x00540220), NewDicomTag(0x00540222), NewDicomTag(0x00100022) }));
			Assert.AreEqual(path.ToString(), "(0054,0220)\\(0054,0222)\\(0010,0022)");

			path = new DicomTagPath(new uint[] { 0x00540220, 0x00540222 });
			Assert.AreEqual(path.ToString(), "(0054,0220)\\(0054,0222)");

			path = new DicomTagPath(new uint[] { 0x00540220, 0x00540222, 0x00100010 });
			Assert.AreEqual(path.ToString(), "(0054,0220)\\(0054,0222)\\(0010,0010)");
		}

		private DicomTag NewDicomTag(ushort group, ushort element)
		{
			return new DicomTag(DicomTag.GetTagValue(group, element), "Throwaway Tag", "ThrowawayTag", DicomVr.UNvr, false, 1, uint.MaxValue, false);
		}

		private DicomTag NewDicomTag(uint tag)
		{
			return new DicomTag(tag, "Throwaway Tag", "ThrowawayTag", DicomVr.UNvr, false, 1, uint.MaxValue, false);
		}
    }
}

#endif