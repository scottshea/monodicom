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
	public class EqualitySpecificationTests : TestsBase
	{
		[Test]
		public void Test_Equal_ValueType()
		{
			EqualSpecification s = new EqualSpecification();
			s.RefValueExpression = new ConstantExpression(1);
			Assert.IsTrue(s.Test(1).Success);
			Assert.IsFalse(s.Test(0).Success);
			Assert.IsFalse(s.Test(null).Success);
		}

		[Test]
		public void Test_Equal_ReferenceType()
		{
			object x = new object();
			object y = new object();

			EqualSpecification s = new EqualSpecification();
			s.RefValueExpression = new ConstantExpression(x);

			Assert.IsTrue(s.Test(x).Success);
			Assert.IsFalse(s.Test(y).Success);
			Assert.IsFalse(s.Test(null).Success);
		}

		[Test]
		public void Test_Equal_Null()
		{
			EqualSpecification s = new EqualSpecification();
			s.RefValueExpression = new ConstantExpression(null);

			Assert.IsTrue(s.Test(null).Success);
			Assert.IsFalse(s.Test(new object()).Success);
			Assert.IsFalse(s.Test(1).Success);
		}

		[Test]
		// This test is currently failing because coercion code hasn't been merged to trunk yet
		public void Test_Equal_CoerceTypes()
		{
			EqualSpecification s = new EqualSpecification();
			s.RefValueExpression = new ConstantExpression("1");

			Assert.IsTrue(s.Test(1).Success);
			Assert.IsTrue(s.Test(1.0).Success);
			Assert.IsTrue(s.Test("1").Success);
			Assert.IsFalse(s.Test(null).Success);
			Assert.IsFalse(s.Test(0).Success);
			Assert.IsFalse(s.Test(0.0).Success);
			Assert.IsFalse(s.Test("0").Success);
		}

		[Test]
		public void Test_Equal_Strict()
		{
			EqualSpecification s = new EqualSpecification();
			s.RefValueExpression = new ConstantExpression(1.0);
			s.Strict = true;

			// this should fail because in strict mode we don't do type coercion,
			// and Object.Equals(x, y) returns false when comparing different types
			Assert.IsFalse(s.Test(1).Success);
		}


		[Test]
		public void Test_NotEqual_ValueType()
		{
			NotEqualSpecification s = new NotEqualSpecification();
			s.RefValueExpression = new ConstantExpression(1);
			Assert.IsFalse(s.Test(1).Success);
			Assert.IsTrue(s.Test(0).Success);
			Assert.IsTrue(s.Test(null).Success);
		}

		[Test]
		public void Test_NotEqual_ReferenceType()
		{
			object x = new object();
			object y = new object();

			NotEqualSpecification s = new NotEqualSpecification();
			s.RefValueExpression = new ConstantExpression(x);

			Assert.IsFalse(s.Test(x).Success);
			Assert.IsTrue(s.Test(y).Success);
			Assert.IsTrue(s.Test(null).Success);
		}

		[Test]
		public void Test_NotEqual_Null()
		{
			NotEqualSpecification s = new NotEqualSpecification();
			s.RefValueExpression = new ConstantExpression(null);

			Assert.IsFalse(s.Test(null).Success);
			Assert.IsTrue(s.Test(new object()).Success);
			Assert.IsTrue(s.Test(1).Success);
		}

		[Test]
		// This test is currently failing because coercion code hasn't been merged to trunk yet
		public void Test_NotEqual_CoerceTypes()
		{
			NotEqualSpecification s = new NotEqualSpecification();
			s.RefValueExpression = new ConstantExpression("1");

			Assert.IsFalse(s.Test(1).Success);
			Assert.IsFalse(s.Test(1.0).Success);
			Assert.IsFalse(s.Test("1").Success);

			Assert.IsTrue(s.Test(null).Success);
			Assert.IsTrue(s.Test(0).Success);
			Assert.IsTrue(s.Test(0.0).Success);
			Assert.IsTrue(s.Test("0").Success);
		}

		[Test]
		public void Test_NotEqual_Strict()
		{
			NotEqualSpecification s = new NotEqualSpecification();
			s.RefValueExpression = new ConstantExpression(1.0);
			s.Strict = true;

			// this should pass because in strict mode we don't do type coercion,
			// and Object.Equals(x, y) returns false when comparing different types
			Assert.IsTrue(s.Test(1).Success);
		}

	}
}

#endif
