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

using System.Text.RegularExpressions;

namespace ClearCanvas.Common.Specifications
{
    public class RegexSpecification : PrimitiveSpecification
    {
        private readonly string _pattern;
    	private readonly bool _ignoreCase = true;	// true by default
    	private readonly bool _nullMatches;

        public RegexSpecification(string pattern, bool ignoreCase, bool nullMatches)
        {
			Platform.CheckForNullReference(pattern, "pattern");
			Platform.CheckForEmptyString(pattern, "pattern");

            _pattern = pattern;
            _ignoreCase = ignoreCase;
        	_nullMatches = nullMatches;
        }

		public RegexSpecification(string pattern)
			:this(pattern, true, false)
		{
		}

    	public string Pattern
    	{
			get { return _pattern; }
    	}

    	public bool IgnoreCase
    	{
			get { return _ignoreCase; }
    	}

    	public bool NullMatches
    	{
			get { return _nullMatches; }
    	}

        protected override TestResult InnerTest(object exp, object root)
        {
            if (exp == null)
				return DefaultTestResult(_nullMatches);

            if (exp is string)
            {
                if (_ignoreCase)
                    return DefaultTestResult(Regex.Match(exp as string, _pattern, RegexOptions.IgnoreCase).Success);
                else
                    return DefaultTestResult(Regex.Match(exp as string, _pattern).Success);
            }
            else
            {
                throw new SpecificationException(SR.ExceptionCastExpressionString);
            }
        }
    }
}
