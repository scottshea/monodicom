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

using System.Xml;
using System.Xml.Schema;

namespace ClearCanvas.Common.Actions
{
    /// <summary>
    /// Interface for extensions implementing <see cref="XmlActionCompilerOperatorExtensionPoint{TActionContext,TSchemaContext}"/>.
    /// </summary>
    public interface IXmlActionCompilerOperator<TActionContext, TSchemaContext>
    {
        /// <summary>
        /// The name of the action implemented.  This is typically the name of the <see cref="XmlElement"/> describing the action.
        /// </summary>
        string OperatorTag { get; }

        /// <summary>
        /// Method used to compile the action.  
        /// </summary>
        /// <param name="xmlNode">Input <see cref="XmlElement"/> describing the action to perform.</param>
        /// <returns>A class implementing the <see cref="IActionItem{T}"/> interface which can perform the action.</returns>
        IActionItem<TActionContext> Compile(XmlElement xmlNode);

        /// <summary>
        /// Get an <see cref="XmlSchemaElement"/> describing the ActionItem for validation purposes.
        /// </summary>
        /// <param name="context">A context in which the schema is being generated.</param>
        /// <returns></returns>
        XmlSchemaElement GetSchema(TSchemaContext context);
    }
}