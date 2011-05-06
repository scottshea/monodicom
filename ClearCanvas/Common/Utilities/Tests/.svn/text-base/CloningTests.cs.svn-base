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

#if	UNIT_TESTS

#pragma warning disable 1591,0419,1574,1587

using NUnit.Framework;

namespace ClearCanvas.Common.Utilities.Tests
{
	[Cloneable]
	internal class TestDerivedClass : TestCallOrder
	{
		public static bool Cloning = false;

		[CloneIgnore]
		private object _ignoredValue;
		
		private TestDerivedClass(TestDerivedClass source, ICloningContext context)
		{
			context.CloneFields(source, this);
		}

		public TestDerivedClass()
		{
			if (Cloning)
				Assert.Fail("Cloning constructor should be called.");
		}

		public object IgnoredValue
		{
			get { return _ignoredValue; }
			set { _ignoredValue = value; }
		}

		[OnCloneComplete]
		private void OnCloneComplete()
		{
			Assert.AreEqual(CallOrder++, 2);
		}
	}

	[Cloneable(true)]
	internal class TestCallOrder : TestDefaultConstructor
	{
		private int _testField;

		public TestCallOrder()
		{
		}

		public int TestField
		{
			get { return _testField; }
			set { _testField = value; }
		}

		[OnCloneComplete]
		private void OnCloneComplete()
		{
			Assert.AreEqual(CallOrder++, 1);
		}
	}

	[Cloneable(true)]
	internal class TestDefaultConstructor
	{
		private object _cloneableObject;
		[CloneCopyReference]
		private object _copyReferenceObject;
		private int _value;

		[CloneIgnore] 
		public int CallOrder = 0;
		[CloneIgnore]
		public bool CloneInitializeCalled = false;
		[CloneIgnore]
		public bool CloneCompleteCalled = false;

		public TestDefaultConstructor()
		{
		}

		public object CloneableObject
		{
			get { return _cloneableObject; }
			set { _cloneableObject = value; }
		}

		public object CopyReferenceObject
		{
			get { return _copyReferenceObject; }
			set { _copyReferenceObject = value; }
		}

		public int Value
		{
			get { return _value; }
			set { _value = value; }
		}
		
		[CloneInitialize]
		private void Initialize(TestDefaultConstructor source, ICloningContext context)
		{
			context.CloneFields(source, this);
			CloneInitializeCalled = true;
		}

		[OnCloneComplete]
		private void OnCloneComplete()
		{
			CloneCompleteCalled = true;
			Assert.AreEqual(CallOrder++, 0);
		}
	}

	[Cloneable(true)]
	public class SimpleCloneableObject
	{
		public SimpleCloneableObject()
		{
		}
	}

	[TestFixture]
	public class CloningTests
	{
		public CloningTests()
		{
		}

		[Test]
		public void Test()
		{
			try
			{
				SimpleCloneableObject simple = new SimpleCloneableObject();

				TestDerivedClass.Cloning = false;

				TestDerivedClass test = new TestDerivedClass();

				TestDerivedClass.Cloning = true;

				test.IgnoredValue = simple;
				test.Value = 4;
				test.TestField = 5;
				test.CloneableObject = simple;
				test.CopyReferenceObject = simple;

				TestDerivedClass clone = (TestDerivedClass) CloneBuilder.Clone(test);

				Assert.AreEqual(clone.IgnoredValue, null);
				Assert.AreEqual(test.Value, clone.Value);
				Assert.AreEqual(test.TestField, clone.TestField);
				Assert.AreEqual(clone.CloneInitializeCalled, true);
				Assert.AreEqual(clone.CloneCompleteCalled, true);
				Assert.AreSame(clone.CopyReferenceObject, simple);
				Assert.AreNotSame(clone.CloneableObject, simple);
			}
			finally
			{
				TestDerivedClass.Cloning = false;
			}
		}
	}
}

#endif