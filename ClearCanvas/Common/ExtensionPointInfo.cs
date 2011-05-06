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
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Common
{
    /// <summary>
    /// Describes an extension point.  
    /// </summary>
    /// <remarks>
    /// Instances of this class are constructed by the framework when it processes
    /// plugins looking for extension points.
    /// </remarks>
    public class ExtensionPointInfo : IBrowsable
    {
        private Type _extensionPointClass;
        private Type _extensionInterface;
        private string _name;
        private string _description;

        /// <summary>
        /// Internal constructor.
        /// </summary>
        internal ExtensionPointInfo(Type extensionPointClass, Type extensionInterface, string name, string description)
        {
            _extensionPointClass = extensionPointClass;
            _extensionInterface = extensionInterface;
            _name = name;
            _description = description;
        }

        /// <summary>
        /// Gets the class that defines the extension point.
        /// </summary>
        public Type ExtensionPointClass
        {
            get { return _extensionPointClass; }
        }

        /// <summary>
        /// Gets the interface that an extension must implement.
        /// </summary>
        public Type ExtensionInteface
        {
            get { return _extensionInterface; }
        }

        /// <summary>
        /// Computes and returns a list of the installed extensions to this point,
        /// including disabled extensions.
        /// </summary>
        /// <returns></returns>
        public IList<ExtensionInfo> ListExtensions()
        {
            return CollectionUtils.Select(Platform.PluginManager.Extensions,
                delegate(ExtensionInfo ext) { return ext.PointExtended == _extensionPointClass; });
        }

        #region IBrowsable Members

        /// <summary>
        /// Friendly name of the extension point, if one exists, otherwise null.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// A friendly description of the extension point, if one exists, otherwise null.
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Formal name of the extension, which is the fully qualified name of the extension point class.
        /// </summary>
        public string FormalName
        {
            get { return _extensionPointClass.FullName; }
        }

        #endregion
    }
}
