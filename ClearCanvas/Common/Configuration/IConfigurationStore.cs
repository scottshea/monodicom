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
using System.IO;

namespace ClearCanvas.Common.Configuration
{
    /// <summary>
    /// Defines the interface to a mechanism for the storage of configuration data.
    /// </summary>
    /// <remarks>
    /// This interface is more general purpose than <see cref="ISettingsStore"/>, in that it allows storage
    /// of arbitrary configuration "documents" that need not conform to any particular structure.
    /// </remarks>
    public interface IConfigurationStore
    {
        /// <summary>
        /// Obtains the specified document for the specified user and instance key.  If user is null,
        /// the shared document is obtained.
        /// </summary>
        /// <remarks>
        /// Implementors should throw a <see cref="ConfigurationDocumentNotFoundException"/> if the requested document does not exist.
        /// </remarks>
        /// <exception cref="ConfigurationDocumentNotFoundException">The requested document does not exist.</exception>
        TextReader GetDocument(string name, Version version, string user, string instanceKey);

        /// <summary>
        /// Stores the specified document for the current user and instance key.  If user is null,
        /// the document is stored as a shared document.
        /// </summary>
        void PutDocument(string name, Version version, string user, string instanceKey, TextReader content);
    }
}
