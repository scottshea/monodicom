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
using NUnit.Framework;
using ClearCanvas.Dicom.ServiceModel.Query;

namespace ClearCanvas.Dicom.ServiceModel.Query.Tests
{
	[TestFixture]
	public class SortStudyTests
	{
		public SortStudyTests()
		{
		}

		[Test]
		public void Test()
		{
			List<StudyRootStudyIdentifier> identifiers = new List<StudyRootStudyIdentifier>();

			identifiers.Add(CreateStudyIdentifier("3", "20080101", "112300"));
			identifiers.Add(CreateStudyIdentifier("4", "20080101", ""));
			identifiers.Add(CreateStudyIdentifier("2", "20080104", "184400"));
			identifiers.Add(CreateStudyIdentifier("1", "20080104", "184500"));
			identifiers.Add(CreateStudyIdentifier("5", "", ""));
			identifiers.Add(CreateStudyIdentifier("6", "", ""));

			identifiers.Sort(new StudyDateTimeComparer());

			int i = 1;
			foreach (StudyRootStudyIdentifier identifier in identifiers)
			{
				Assert.AreEqual(i.ToString(), identifier.StudyInstanceUid);
				++i;
			}
		}

		private static StudyRootStudyIdentifier CreateStudyIdentifier(string uid, string date, string time)
		{
			StudyRootStudyIdentifier id = new StudyRootStudyIdentifier();
			id.StudyInstanceUid = uid;
			id.StudyDate = date;
			id.StudyTime = time;
			return id;
		}
	}
}

#endif