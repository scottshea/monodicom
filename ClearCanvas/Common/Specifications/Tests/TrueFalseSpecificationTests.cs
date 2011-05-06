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

#pragma warning disable 1591

using System;
using NUnit.Framework;

namespace ClearCanvas.Common.Specifications.Tests
{
	[TestFixture]
	public class TrueFalseSpecificationTests: TestsBase
	{
		[Test]
		public void Test_True()
		{
			TrueSpecification s = new TrueSpecification();
			Assert.IsTrue(s.Test(true).Success);
			Assert.IsFalse(s.Test(false).Success);
		}

		[Test]
		[ExpectedException(typeof(SpecificationException))]
		public void Test_True_InvalidType()
		{
			// test something that is not a boolean value
			TrueSpecification s = new TrueSpecification();
			s.Test(1);
		}

		[Test]
		public void Test_False()
		{
			FalseSpecification s = new FalseSpecification();
			Assert.IsTrue(s.Test(false).Success);
			Assert.IsFalse(s.Test(true).Success);
		}

		[Test]
		[ExpectedException(typeof(SpecificationException))]
		public void Test_False_InvalidType()
		{
			// test something that is not a boolean value
			TrueSpecification s = new TrueSpecification();
			s.Test(1);
		}

		[Test]
		public void Test_IsNull()
		{
			IsNullSpecification s = new IsNullSpecification();
			Assert.IsTrue(s.Test(null).Success);
			Assert.IsTrue(s.Test("").Success);	// treat empty string as null
			Assert.IsFalse(s.Test(new object()).Success);
			Assert.IsFalse(s.Test(0).Success);
		}

		[Test]
		public void Test_NotNull()
		{
			NotNullSpecification s = new NotNullSpecification();
			Assert.IsTrue(s.Test(new object()).Success);
			Assert.IsFalse(s.Test(null).Success);
			Assert.IsFalse(s.Test("").Success); 	// treat empty string as null
			Assert.IsTrue(s.Test(0).Success);
		}
	}
}

#endif
