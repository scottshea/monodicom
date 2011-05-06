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

namespace ClearCanvas.Common
{
    /// <summary>
    /// Implements an extension filter that performs matching on attributes.
    /// </summary>
    /// <remarks>
    /// For each attribute that is supplied to the constructor of this filter, the filter
    /// will check if the extension is marked with at least one matching attribute.  A matching attribute is an
    /// attribute for which the <see cref="Attribute.Match"/> method returns true.  This allows
    /// for quite sophisticated matching capabilities, as the attribute itself decides what constitutes
    /// a match.
    /// </remarks>
    public class AttributeExtensionFilter : ExtensionFilter
    {
        private Attribute[] _attributes;

        /// <summary>
        /// Creates a filter to match on multiple attributes.
        /// </summary>
        /// <remarks>
		/// The extension must test true on each attribute.
		/// </remarks>
        /// <param name="attributes">The attributes to be used as test criteria.</param>
        public AttributeExtensionFilter(Attribute[] attributes)
        {
            _attributes = attributes;
        }

        /// <summary>
        /// Creates a filter to match on a single attribute.
        /// </summary>
        /// <param name="attribute">The attribute to be used as test criteria.</param>
        public AttributeExtensionFilter(Attribute attribute)
            :this(new Attribute[] { attribute })
        {
        }

        /// <summary>
        /// Checks whether the specified extension is marked with attributes that 
        /// match every test attribute supplied as criteria to this filter.
        /// </summary>
        /// <param name="extension">The information about the extension to test.</param>
        /// <returns>true if the test succeeds.</returns>
        public override bool Test(ExtensionInfo extension)
        {
            foreach (Attribute a in _attributes)
            {
                object[] candidates = extension.ExtensionClass.GetCustomAttributes(a.GetType(), true);
                if (!AnyMatch(a, candidates))
                {
                    return false;
                }
            }
            return true;
        }

        private bool AnyMatch(Attribute a, object[] candidates)
        {
            foreach (Attribute c in candidates)
            {
                if (c.Match(a))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
