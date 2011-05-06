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
	public abstract class TestsBase
	{
		private class ConstantSpecification : Specification
		{
			private readonly TestResult _result;

			public ConstantSpecification(TestResult result)
			{
				_result = result;
			}

			protected override TestResult InnerTest(object exp, object root)
			{
				return _result;
			}
		}

		public ISpecification AlwaysTrue = new ConstantSpecification(new TestResult(true, new TestResultReason("Always true")));
		public ISpecification AlwaysFalse = new ConstantSpecification(new TestResult(false, new TestResultReason("Always false")));

		protected class PredicateSpecification<T> : Specification
		{
			private readonly Predicate<T> _predicate;

			public PredicateSpecification(Predicate<T> predicate)
			{
				_predicate = predicate;
			}

			protected override TestResult InnerTest(object exp, object root)
			{
				return new TestResult(_predicate((T)exp));
			}
		}

		protected class ConstantExpression : Expression
		{
			private readonly object _value;

			public ConstantExpression(object constantValue)
				:base("")
			{
				_value = constantValue;
			}

			public ConstantExpression(string text, object constantValue)
				:base(text)
			{
				_value = constantValue;
			}

			public object Value
			{
				get { return _value; }
			}

			public override object Evaluate(object arg)
			{
				return _value;
			}
		}

		protected class ConstantExpressionFactory : IExpressionFactory
		{
			public Expression CreateExpression(string text)
			{
				return new ConstantExpression(text, null);
			}
		}
	}
}

#endif
