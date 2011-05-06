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

namespace ClearCanvas.Common.Utilities
{
    /// <summary>
    /// An implementation of <see cref="IExtensionFactory"/> that returns no extensions.
    /// </summary>
    /// <remarks>
    /// This implementation simply returns zero extensions for any extension point.  This is useful
    /// for unit-testing scenarios to prevent any extensions from being inadvertantly created.  This class
    /// may also be used as a base class for a more specialized extension factory that may respond to requests
    /// for certain extension points but not for others.
    /// </remarks>
    public class NullExtensionFactory : IExtensionFactory
    {
        #region IExtensionFactory Members

        /// <summary>
        /// Return an empty array.
        /// </summary>
        public virtual object[] CreateExtensions(ExtensionPoint extensionPoint, ExtensionFilter filter, bool justOne)
        {
            return new object[] { };
        }

        /// <summary>
        /// Returns an empty array.
        /// </summary>
        public virtual ExtensionInfo[] ListExtensions(ExtensionPoint extensionPoint, ExtensionFilter filter)
        {
            return new ExtensionInfo[] { };
        }

        #endregion
    }
}
