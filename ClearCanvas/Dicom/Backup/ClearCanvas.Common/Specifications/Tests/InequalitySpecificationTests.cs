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
	public class InequalitySpecificationTests : TestsBase
	{
		[Test]
		public void Test_GreaterThan_Exclusive()
		{
			GreaterThanSpecification s = new GreaterThanSpecification();
			s.RefValueExpression = new ConstantExpression(1);
			Assert.IsFalse(s.Test(0).Success);
			Assert.IsFalse(s.Test(1).Success);
			Assert.IsTrue(s.Test(2).Success);

			// null is less than any other value
			Assert.IsFalse(s.Test(null).Success);
		}

		[Test]
		public void Test_GreaterThan_Inclusive()
		{
			GreaterThanSpecification s = new GreaterThanSpecification();
			s.RefValueExpression = new ConstantExpression(1);
			s.Inclusive = true;

			Assert.IsFalse(s.Test(0).Success);
			Assert.IsTrue(s.Test(1).Success);
			Assert.IsTrue(s.Test(2).Success);

			// null is less than any other value
			Assert.IsFalse(s.Test(null).Success);
		}

		[Test]
		// This test is currently failing because coercion code hasn't been merged to trunk yet
		public void Test_GreaterThan_CoerceTypes()
		{
			GreaterThanSpecification s = new GreaterThanSpecification();
			s.RefValueExpression = new ConstantExpression("1");

			Assert.IsFalse(s.Test(0).Success);
			Assert.IsFalse(s.Test(1).Success);
			Assert.IsTrue(s.Test(2).Success);
			//Assert.IsTrue(s.Test(null).Success);
			//Assert.IsTrue(s.Test(1).Success);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Test_GreaterThan_Strict()
		{
			GreaterThanSpecification s = new GreaterThanSpecification();
			s.RefValueExpression = new ConstantExpression("1");
			s.Strict = true;

			// this should fail because in strict mode we don't do type coercion,
			// and IComparable throws an ArgumentException in this situation
			s.Test(0);
		}

		[Test]
		public void Test_LessThan_Exclusive()
		{
			LessThanSpecification s = new LessThanSpecification();
			s.RefValueExpression = new ConstantExpression(1);
			Assert.IsTrue(s.Test(0).Success);
			Assert.IsFalse(s.Test(1).Success);
			Assert.IsFalse(s.Test(2).Success);

			// null is less than any other value
			Assert.IsTrue(s.Test(null).Success);
		}

		[Test]
		public void Test_LessThan_Inclusive()
		{
			LessThanSpecification s = new LessThanSpecification();
			s.RefValueExpression = new ConstantExpression(1);
			s.Inclusive = true;

			Assert.IsTrue(s.Test(0).Success);
			Assert.IsTrue(s.Test(1).Success);
			Assert.IsFalse(s.Test(2).Success);

			// null is less than any other value
			Assert.IsTrue(s.Test(null).Success);
		}

		[Test]
		// This test is currently failing because coercion code hasn't been merged to trunk yet
		public void Test_LessThan_CoerceTypes()
		{
			LessThanSpecification s = new LessThanSpecification();
			s.RefValueExpression = new ConstantExpression("1");

			Assert.IsTrue(s.Test(0).Success);
			//Assert.IsTrue(s.Test(1).Success);
			Assert.IsFalse(s.Test(2).Success);
			//Assert.IsFalse(s.Test(null).Success);
			Assert.IsFalse(s.Test(1).Success);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Test_LessThan_Strict()
		{
			LessThanSpecification s = new LessThanSpecification();
			s.RefValueExpression = new ConstantExpression("1");
			s.Strict = true;

			// this should fail because in strict mode we don't do type coercion,
			// and IComparable throws an ArgumentException in this situation
			s.Test(0);
		}
	}
}

#endif
