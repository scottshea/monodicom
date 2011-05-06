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

#pragma warning disable 1591

using System;
using System.Collections;
using System.Collections.Generic;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Common.Specifications
{
    /// <summary>
    /// Counts the number of items in a <see cref="IEnumerable"/> that satisfy the inner specification. If no
    /// inner specification is supplied, all items are counted.
    /// </summary>
    public class CountSpecification : PrimitiveSpecification
    {
        class NullSpecification : ISpecification
        {
            public TestResult Test(object obj)
            {
                return new TestResult(true);
            }
        }

        private static readonly ISpecification NullFilter = new NullSpecification();

        private readonly int _min = 0;
        private readonly int _max = Int32.MaxValue;
        private readonly ISpecification _filterSpecification;

        public CountSpecification(int min, int max, ISpecification filterSpecification)
        {
			Platform.CheckArgumentRange(min, 0, int.MaxValue, "min");
			Platform.CheckArgumentRange(max, 0, int.MaxValue, "max");
			if(max < min)
				throw new ArgumentException("min cannot be larger than max");

            _max = max;
            _min = min;
            _filterSpecification = filterSpecification ?? NullFilter;
        }

    	public int Min
    	{
			get { return _min; }
    	}

    	public int Max
    	{
			get { return _max; }
    	}

    	public ISpecification FilterSpecification
    	{
			get { return _filterSpecification; }
    	}

        protected override TestResult InnerTest(object exp, object root)
        {
            // optimizations
            // if _innerSpecification is NullFilter, and exp is an Array or ICollection, we can just use Length/Count
            if (_filterSpecification == NullFilter)
            {
                if (exp is Array)
                {
                    return DefaultTestResult(InRange((exp as Array).Length));
                }

                if (exp is ICollection)
                {
                    return DefaultTestResult(InRange((exp as ICollection).Count));
                }
            }

            // otherwise, treat as IEnumerable and evaluate _innerSpecification
            if (exp is IEnumerable)
            {
                ICollection countableItems = CollectionUtils.Select(exp as IEnumerable,
                    delegate(object item) { return _filterSpecification.Test(item).Success; });

                return DefaultTestResult(InRange(countableItems.Count));
            }

			throw new SpecificationException(SR.ExceptionCastExpressionArrayCollectionEnumerable);
        }

        protected bool InRange(int n)
        {
            return n >= _min && n <= _max;
        }
    }
}
