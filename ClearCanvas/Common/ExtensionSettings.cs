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
using System.Configuration;
using System.Xml;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Common
{
	// these settings must be stored in a config file, not in the configuration store
	[SettingsProvider(typeof(LocalFileSettingsProvider))]
    internal sealed partial class ExtensionSettings
	{
        /// <summary>
        /// Orders the extensions according to the order specified by the XML document.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="ordered"></param>
        /// <param name="remainder"></param>
        public void OrderExtensions(IEnumerable<ExtensionInfo> input,
            out List<ExtensionInfo> ordered, out List<ExtensionInfo> remainder)
        {
            ordered = new List<ExtensionInfo>();
            remainder = new List<ExtensionInfo>(input);

            // order the items based on the order in the XML
            foreach (XmlElement element in ListExtensionNodes())
            {
                string className = element.GetAttribute("class");

                // ignore entries that don't specify the "class" attribute
                // (these shouldn't exist in a well-formed doc)
                if (string.IsNullOrEmpty(className))
                    continue;

                // find the extensions corresponding to this class
                // (yes, it is possible that that are multiple extensions implemented by the same class)
                List<ExtensionInfo> items = CollectionUtils.Select(remainder,
                    delegate(ExtensionInfo x) { return Equals(Type.GetType(className), x.ExtensionClass); });

                // add these to the ordered list and remove them from the remainder
                foreach (ExtensionInfo ext in items)
                {
                    ordered.Add(ext);
                    remainder.Remove(ext);
                    if (remainder.Count == 0)
                        break;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified extension class is enabled.
        /// </summary>
        /// <param name="extensionClass"></param>
        /// <param name="defaultEnablement"></param>
        /// <returns></returns>
        public bool IsEnabled(Type extensionClass, bool defaultEnablement)
        {
            // find an XML entry for this class
            XmlElement extensionNode = (XmlElement)CollectionUtils.SelectFirst(ListExtensionNodes(),
                delegate(object node)
                {
                    string className = ((XmlElement)node).GetAttribute("class");
                    return !string.IsNullOrEmpty(className) ?
                        Equals(Type.GetType(className), extensionClass) : false;
                });

            // if an entry exists, check if it specifies an enablement override
            if(extensionNode != null)
            {
                string enabledString = extensionNode.GetAttribute("enabled");
                if(!string.IsNullOrEmpty(enabledString))
                    return Convert.ToBoolean(enabledString);
            }

            // return default
            return defaultEnablement;
        }


        /// <summary>
        /// List the stored extensions in the XML doc
        /// </summary>
        /// <returns>A list of "extension" element</returns>
        private XmlNodeList ListExtensionNodes()
        {
            return this.ExtensionConfigurationXml.SelectNodes(String.Format("/extensions/extension"));
        }

    }
}
