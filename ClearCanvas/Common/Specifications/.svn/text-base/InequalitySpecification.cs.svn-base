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
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// From MSDN IComparable.CompareTo docs: 
    /// "By definition, any object compares greater than a null reference, and two null references compare equal to each other."
    /// </remarks>
    public abstract class InequalitySpecification : ComparisonSpecification
    {
        private bool _inclusive;
        private readonly int _multiplier;

        internal InequalitySpecification(int multiplier)
        {
            _multiplier = multiplier;
        }

        public bool Inclusive
        {
            get { return _inclusive; }
            set { _inclusive = value; }
        }

        protected override bool CompareValues(object testValue, object refValue)
        {
            // two nulls compare equal
            if (testValue == null && refValue == null)
                return true;

            // if testValue is null, refValue is greater by definition
            if (testValue == null)
                return (_multiplier == -1);

            // if refValue is null, testValue is greater by definition
            if (refValue == null)
                return (_multiplier == 1);

            // need to perform a comparison - ensure IComparable is implemented
            if (!(testValue is IComparable))
                throw new SpecificationException("Test expression does not evaluate to an IComparable object");

            int x = (testValue as IComparable).CompareTo(refValue) * _multiplier;
            return x > 0 || x == 0 && _inclusive;
        }
    }
}
