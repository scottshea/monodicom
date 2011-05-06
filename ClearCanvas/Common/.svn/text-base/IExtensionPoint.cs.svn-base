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
    /// Extension point interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interface provides a means for a client of an extension point to reference
    /// the extension point and call methods on it without knowing the type of the extension point class.
	/// </para>
	/// <para>
    /// Extension point classes should never implement this interface directly.
    /// Instead, subclass <see cref="ExtensionPoint" />.
	/// </para>
    /// </remarks>
    public interface IExtensionPoint
    {
        /// <summary>
        /// Lists the available extensions.
        /// </summary>
        /// <returns>An array of <see cref="ExtensionInfo" /> objects describing the available extensions.</returns>
        ExtensionInfo[] ListExtensions();

        /// <summary>
        /// Lists the available extensions, that also match the specified <see cref="ExtensionFilter"/>.
        /// </summary>
        /// <returns>An array of <see cref="ExtensionInfo" /> objects describing the available extensions.</returns>
        ExtensionInfo[] ListExtensions(ExtensionFilter filter);

        /// <summary>
        /// Lists the available extensions that match the specified filter.
        /// </summary>
        ExtensionInfo[] ListExtensions(Predicate<ExtensionInfo> filter);
 
        /// <summary>
        /// Instantiates one extension.
        /// </summary>
        /// <remarks>
        /// If more than one extension exists, then the type of the extension that is
        /// returned is non-deterministic.  If no extensions exist that can be successfully
        /// instantiated, an exception is thrown.
        /// </remarks>
        /// <returns>A reference to the extension.</returns>
        /// <exception cref="NotSupportedException">Failed to instantiate an extension.</exception>
        object CreateExtension();

        /// <summary>
        /// Instantiates an extension that also matches the specified <see cref="ExtensionFilter" />.
        /// </summary>
        /// <remarks>
        /// If more than one extension exists, then the type of the extension that is
        /// returned is non-deterministic.  If no extensions exist that can be successfully
        /// instantiated, an exception is thrown.
        /// </remarks>
        /// <returns>A reference to the extension.</returns>
        /// <exception cref="NotSupportedException">Failed to instantiate an extension.</exception>
        object CreateExtension(ExtensionFilter filter);

        /// <summary>
        /// Instantiates an extension that matches the specified filter.
        /// </summary>
        object CreateExtension(Predicate<ExtensionInfo> filter);
        
        /// <summary>
        /// Instantiates each available extension.
        /// </summary>
        /// <remarks>
        /// Attempts to instantiate each available extension.  If an extension fails to instantiate
        /// for any reason, the failure is logged and it is ignored.
        /// </remarks>
        /// <returns>An array of references to the created extensions.  If no extensions were created
        /// the array will be empty.</returns>
        object[] CreateExtensions();

        /// <summary>
        /// Instantiates each available extension that also matches the specified <see cref="ExtensionFilter" />.
        /// </summary>
        /// <remarks>
        /// Attempts to instantiate each matching extension.  If an extension fails to instantiate
        /// for any reason, the failure is logged and it is ignored.
        /// </remarks>
        /// <returns>An array of references to the created extensions.  If no extensions were created
        /// the array will be empty.</returns>
        object[] CreateExtensions(ExtensionFilter filter);

        /// <summary>
        /// Instantiates each available extension that matches the specified filter.
        /// </summary>
        object[] CreateExtensions(Predicate<ExtensionInfo> filter);
    }
}
