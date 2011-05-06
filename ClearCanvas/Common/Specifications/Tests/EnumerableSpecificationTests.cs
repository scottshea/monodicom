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
	public class EnumerableSpecificationTests : TestsBase
	{
		[Test]
		public void Test_Each_Empty()
		{
			EachSpecification s1 = new EachSpecification(AlwaysFalse);
			Assert.IsTrue(s1.Test(new object[0]).Success);

			EachSpecification s2 = new EachSpecification(AlwaysTrue);
			Assert.IsTrue(s2.Test(new object[0]).Success);
		}

		[Test]
		public void Test_Each_Normal()
		{
			EachSpecification s = new EachSpecification(new PredicateSpecification<int>(delegate(int i) { return i > 0; }));
			Assert.IsFalse(s.Test(new int[] { 0, 1, 2 }).Success);
			Assert.IsTrue(s.Test(new int[] { 1, 2, 3 }).Success);
		}

		[Test]
		[ExpectedException(typeof(SpecificationException))]
		public void Test_Each_InvalidType()
		{
			// cannot test a non-enumerable object
			EachSpecification s = new EachSpecification(AlwaysTrue);
			s.Test(new object());
		}

		[Test]
		public void Test_Any_Empty()
		{
			AnySpecification s1 = new AnySpecification(AlwaysFalse);
			Assert.IsFalse(s1.Test(new object[0]).Success);

			AnySpecification s2 = new AnySpecification(AlwaysTrue);
			Assert.IsFalse(s2.Test(new object[0]).Success);
		}

		[Test]
		public void Test_Any_Normal()
		{
			AnySpecification s = new AnySpecification(new PredicateSpecification<int>(delegate(int i) { return i > 0; }));
			Assert.IsFalse(s.Test(new int[] { 0, 0, 0 }).Success);
			Assert.IsTrue(s.Test(new int[] { 0, 0, 1 }).Success);
			Assert.IsTrue(s.Test(new int[] { 1, 1, 1 }).Success);
		}

		[Test]
		[ExpectedException(typeof(SpecificationException))]
		public void Test_Any_InvalidType()
		{
			// cannot test a non-enumerable object
			AnySpecification s = new AnySpecification(AlwaysTrue);
			s.Test(new object());
		}

	}
}

#endif
