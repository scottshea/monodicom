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
using System.Reflection;

namespace ClearCanvas.Common
{
    /// <summary>
    /// Describes a plugin, and provides properties for querying the extension points and extensions defined
    /// in the plugin.
    /// </summary>
    public class PluginInfo : IBrowsable
    {
        /// <summary>
        /// Internal method used by the framework to discover the extensions declared in a plugin.
        /// </summary>
        /// <param name="asm">The plugin assembly to inspect.</param>
        /// <returns>An array of <see cref="ExtensionInfo" /> objects describing the extensions.</returns>
        internal static List<ExtensionInfo> DiscoverExtensions(Assembly asm)
        {
            List<ExtensionInfo> extensionList = new List<ExtensionInfo>();
            foreach (Type type in asm.GetTypes())
            {
                object[] attrs = type.GetCustomAttributes(typeof(ExtensionOfAttribute), false);
                foreach (ExtensionOfAttribute a in attrs)
                {
                    extensionList.Add(
                        new ExtensionInfo(
                            type,
                            a.ExtensionPointClass,
                            a.Name,
                            a.Description,
                            ExtensionSettings.Default.IsEnabled(type, a.Enabled)
                        )
                    );
                }
            }
            return extensionList;
        }

        /// <summary>
        /// Internal method used by the framework to discover the extension points declared in a plugin.
        /// </summary>
        /// <param name="asm">The plugin assembly to inspect.</param>
        /// <returns>An array of <see cref="ExtensionPointInfo" />objects describing the extension points.</returns>
        internal static List<ExtensionPointInfo> DiscoverExtensionPoints(Assembly asm)
        {
            List<ExtensionPointInfo> extensionPointList = new List<ExtensionPointInfo>();
            foreach (Type type in asm.GetTypes())
            {
                try
                {
                    object[] attrs = type.GetCustomAttributes(typeof(ExtensionPointAttribute), false);
                    if (attrs.Length > 0)
                    {
                        ValidateExtensionPointClass(type);

                        ExtensionPointAttribute a = (ExtensionPointAttribute)attrs[0];
                        Type extensionInterface = type.BaseType.GetGenericArguments()[0];

                        extensionPointList.Add(new ExtensionPointInfo(type, extensionInterface, a.Name, a.Description));
                    }
                }
                catch (ExtensionPointException e)
                {
                    // log and continue discovering extension points
                    Platform.Log(LogLevel.Error, e.Message);
                }
            }
            return extensionPointList;
        }

        private static void ValidateExtensionPointClass(Type extensionPointClass)
        {
            Type baseType = extensionPointClass.BaseType;
            if (!baseType.IsGenericType || !baseType.GetGenericTypeDefinition().Equals(typeof(ExtensionPoint<>)))
                throw new ExtensionPointException(string.Format(
                    SR.ExceptionExtensionPointMustSubclassExtensionPoint, extensionPointClass.FullName));
        }

        
        private string _name;
        private string _description;
		private string _icon;
		private Assembly _assembly;

        private List<ExtensionPointInfo> _extensionPoints;
        private List<ExtensionInfo> _extensions;

        /// <summary>
        /// Internal constructor.
        /// </summary>
        internal PluginInfo(Assembly assembly, string name, string description, string icon)
        {
            _name = name;
            _description = description;
            _assembly = assembly;
        	_icon = icon;

            _extensionPoints = DiscoverExtensionPoints(assembly);
            _extensions = DiscoverExtensions(assembly);
        }

        /// <summary>
        /// Gets the set of extensions defined in this plugin, including disabled extensions.
        /// </summary>
        public IList<ExtensionInfo> Extensions
        {
            get { return _extensions.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the set of extension points defined in this plugin.
        /// </summary>
        public IList<ExtensionPointInfo> ExtensionPoints
        {
            get { return _extensionPoints.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the assembly that implements this plugin.
        /// </summary>
        public Assembly Assembly
        {
            get { return _assembly; }
        }

        /// <summary>
        /// The name of an icon resource to associate with the plugin.
        /// </summary>
        public string Icon
        {
            get { return _icon; }
        }

        #region IBrowsable Members

    	/// <summary>
    	/// Formal name of this object, typically the type name or assembly name.  Cannot be null.
    	/// </summary>
    	public string FormalName
        {
            get { return Assembly.FullName; }
        }

    	/// <summary>
    	/// Friendly name of the object, if one exists, otherwise null.
    	/// </summary>
    	public string Name
        {
            get { return _name; }
        }

    	/// <summary>
    	/// A friendly description of this object, if one exists, otherwise null.
    	/// </summary>
    	public string Description
        {
            get { return _description; }
        }

        #endregion
    }
}
