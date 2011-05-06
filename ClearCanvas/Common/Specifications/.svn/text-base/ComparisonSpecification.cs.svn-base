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
namespace ClearCanvas.Common.Specifications
{
    public abstract class ComparisonSpecification : PrimitiveSpecification
    {
        private Expression _refValueExpr;
    	private bool _strict;

        public Expression RefValueExpression
        {
            get { return _refValueExpr; }
            set { _refValueExpr = value; }
        }

    	public bool Strict
    	{
			get { return _strict; }
			set { _strict = value; }
    	}

        protected override TestResult InnerTest(object exp, object root)
        {
            if (_refValueExpr == null)
                throw new SpecificationException("Reference value required.");

            object refValue = _refValueExpr.Evaluate(root);

			// bug #3279:if the refValue is of a different type than the test expression, 
			// attempt to coerce the refValue to the same type
			// if the coercion fails, the comparison will be performed on the raw refValue
			if(!_strict && exp != null && refValue != null && exp.GetType() != refValue.GetType())
			{
				try
				{
					refValue = Convert.ChangeType(refValue, exp.GetType());
				}
				catch (InvalidCastException)
				{
					// unable to perform coercion - continue using raw refValue
				}
			}

            return DefaultTestResult(CompareValues(exp, refValue));
        }

        protected abstract bool CompareValues(object testValue, object refValue);
    }
}
