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

using System.Reflection;
using NHibernate.Cfg;
using NUnit.Framework;

namespace ClearCanvas.Dicom.DataStore.Tests
{
	[TestFixture]
	public class ValidationTests
	{
		private PersistentObjectValidator _validator;

		public ValidationTests()
		{
		}

		[TestFixtureSetUp]
		public void Initialize()
		{
			Configuration configuration = new Configuration();
			string assemblyName = MethodBase.GetCurrentMethod().DeclaringType.Assembly.GetName().Name;
			configuration.Configure(@"..\" + assemblyName + ".cfg.xml");
			configuration.AddAssembly(assemblyName);

			_validator = new PersistentObjectValidator(configuration);
		}

		private static Study NewStudy()
		{
			Study study = new Study();
			study.StudyInstanceUid = "123";
			study.AccessionNumber = "abc";
			return study;
		}

		#region Study Tests

		[Test]
		public void TestValidStudy()
		{
			_validator.ValidatePersistentObject(NewStudy());
		}

		[Test]
		[ExpectedException(typeof(DataValidationException))]
		public void TestNullStudyInstanceUid()
		{
			Study study = NewStudy();
			study.StudyInstanceUid = null;
			_validator.ValidatePersistentObject(study);
		}

		[Test]
		[ExpectedException(typeof(DataValidationException))]
		public void TestEmptyStudyInstanceUid()
		{
			Study study = NewStudy();
			study.StudyInstanceUid = "";
			_validator.ValidatePersistentObject(study);
		}

		[Test]
		[ExpectedException(typeof(DataValidationException))]
		public void TestStudyFieldTooLong()
		{
			Study study = NewStudy(); 
			study.StudyTimeRaw = "GreaterThanSixteenCharacters";
			_validator.ValidatePersistentObject(study);
		}

		[Test]
		[ExpectedException(typeof(DataValidationException))]
		public void TestComponentFieldTooLong()
		{
			Study study = NewStudy(); 
			study.PatientId = "A string that is more than 64 characters in length should throw an exception";
			_validator.ValidatePersistentObject(study);
		}

		#endregion
	}
}

#endif