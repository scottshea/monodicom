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

using System.Collections.Generic;
using System.IO;
using System.Xml;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Common.Specifications
{
    public class SpecificationFactory : ISpecificationProvider
    {
        class SingleDocumentSource : ISpecificationXmlSource
        {
            private XmlDocument _xmlDoc;

            public SingleDocumentSource(Stream xml)
            {
                _xmlDoc = new XmlDocument();
                _xmlDoc.Load(xml);
            }

            public SingleDocumentSource(TextReader xml)
            {
                _xmlDoc = new XmlDocument();
                _xmlDoc.Load(xml);
            }



            #region ISpecificationXmlSource Members

            public string DefaultExpressionLanguage
            {
                get
                {
                    string exprLang = _xmlDoc.DocumentElement.GetAttribute("expressionLanguage");

                    // if not specified, assume jscript
                    return string.IsNullOrEmpty(exprLang) ? "jscript" : exprLang;
                }
            }

            public XmlElement GetSpecificationXml(string id)
            {
                XmlElement specNode = (XmlElement)CollectionUtils.SelectFirst(_xmlDoc.GetElementsByTagName("spec"),
                    delegate(object node) { return ((XmlElement)node).GetAttribute("id") == id; });

                if (specNode == null)
                    throw new UndefinedSpecificationException(id);

                return specNode;
            }

            public IDictionary<string, XmlElement> GetAllSpecificationsXml()
            {
                Dictionary<string, XmlElement> specs = new Dictionary<string, XmlElement>();
                foreach (XmlElement specNode in _xmlDoc.GetElementsByTagName("spec"))
                {
                    specs.Add(specNode.GetAttribute("id"), specNode);
                }
                return specs;
            }

            #endregion
        }



        private XmlSpecificationCompiler _builder;
        private ISpecificationXmlSource _xmlSource;

        private Dictionary<string, ISpecification> _cache;

        public SpecificationFactory(Stream xml)
            :this(new SingleDocumentSource(xml))
        {
        }

        public SpecificationFactory(TextReader xml)
            : this(new SingleDocumentSource(xml))
        {
        }


        public SpecificationFactory(ISpecificationXmlSource xmlSource)
        {
            _builder = new XmlSpecificationCompiler(this, xmlSource.DefaultExpressionLanguage);
            _cache = new Dictionary<string, ISpecification>();
            _xmlSource = xmlSource;
        }

        public ISpecification GetSpecification(string id)
        {
            if (_cache.ContainsKey(id))
            {
                return _cache[id];
            }
            else
            {
                XmlElement specNode = _xmlSource.GetSpecificationXml(id);
                return _cache[id] = _builder.Compile(specNode, false);
            }
        }

        public IDictionary<string, ISpecification> GetAllSpecifications()
        {
            Dictionary<string, ISpecification> specs = new Dictionary<string, ISpecification>();
            foreach (KeyValuePair<string, XmlElement> kvp in _xmlSource.GetAllSpecificationsXml())
            {
                specs.Add(kvp.Key, _builder.Compile(kvp.Value, false));
            }
            return specs;
        }
    }
}
