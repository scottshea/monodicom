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

using System;
using System.Collections.Generic;
using System.Text;

namespace ClearCanvas.Common.Specifications
{
    /// <summary>
    /// Class for implementing Case/When/Else statements.
    /// </summary>
    /// <see cref="ISpecification"/>
    public class CaseSpecification : Specification
    {
        private readonly List<WhenThenPair> _whenThens;
        private readonly ISpecification _else;

        internal CaseSpecification(List<WhenThenPair> whenThens, ISpecification elseSpecification)
        {
			Platform.CheckForNullReference(whenThens, "whenThens");
			Platform.CheckForNullReference(elseSpecification, "elseSpecification");

            _whenThens = whenThens;
            _else = elseSpecification;
        }

        /// <summary>
        /// Perform the test.
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        protected override TestResult InnerTest(object exp, object root)
        {
            // test when-then pairs in order
            // the first "when" that succeeds will determine the result
            foreach (WhenThenPair pair in _whenThens)
            {
                if(pair.When.Test(exp).Success)
                {
                    return ResultOf(pair.Then.Test(exp));
                }
            }

            // otherwise execute the "else" clause
            return ResultOf(_else.Test(exp));
        }

        private TestResult ResultOf(TestResult innerResult)
        {
            return innerResult.Success ? new TestResult(true) : 
                new TestResult(false, new TestResultReason(this.FailureMessage, innerResult.Reasons)); 
        }
    }
}
