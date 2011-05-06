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
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace ClearCanvas.Dicom.Utilities.Xml
{
    /// <summary>
    /// Class that can represent a series as XML.
    /// </summary>
    public class SeriesXml : IEnumerable<InstanceXml>
    {
        #region Private members

        private Dictionary<string, InstanceXml> _sourceImageList;

        private readonly Dictionary<string, InstanceXml> _instanceList = new Dictionary<string, InstanceXml>();
        private String _seriesInstanceUid = null;
        private BaseInstanceXml _seriesTagsStream = null;
        private bool _dirty = true; 

        #endregion

        #region Public Properties

        public String SeriesInstanceUid
        {
            get
            {
                if (_seriesInstanceUid == null)
                    return "";
                return _seriesInstanceUid;
            }
        }

    	public int NumberOfSeriesRelatedInstances
    	{
			get { return _instanceList.Count; }
    	}

        #endregion

        #region Constructors

        public SeriesXml(String seriesInstanceUid)
        {
            _seriesInstanceUid = seriesInstanceUid;
        }

        #endregion

        #region Public Methods

        public InstanceXml this[String sopInstanceUid]
        {
            get
            {
            	InstanceXml instance;
				if (!_instanceList.TryGetValue(sopInstanceUid, out instance))
					return null;

            	return instance;
            }
            set
            {
                if (value == null)
                    _instanceList.Remove(sopInstanceUid);
                else
                {
                    _instanceList[sopInstanceUid] = value;
                }

                _dirty = true;
            }
        }

        #endregion

        #region Internal Methods

        internal void CalculateBaseCollectionForSeries()
        {
            if (_instanceList.Count < 2)
                return;

            // Optimization:  a base collection has already been created, just return.
            if (_seriesTagsStream != null)
                return;

            IEnumerator<InstanceXml> iterator = GetEnumerator();

            if (false == iterator.MoveNext())
                return;

            DicomAttributeCollection collect1 = iterator.Current.Collection;

            if (false == iterator.MoveNext())
                return;

            DicomAttributeCollection collect2 = iterator.Current.Collection;

            _seriesTagsStream = new BaseInstanceXml(collect1, collect2);
        }

        internal XmlElement GetMemento(XmlDocument theDocument, StudyXmlOutputSettings settings)
        {
            // Calc the base attributes
            CalculateBaseCollectionForSeries();

            XmlElement series = theDocument.CreateElement("Series");

            XmlAttribute seriesInstanceUid = theDocument.CreateAttribute("UID");
            seriesInstanceUid.Value = _seriesInstanceUid;
            series.Attributes.Append(seriesInstanceUid);

            XmlElement baseElement = theDocument.CreateElement("BaseInstance");

            // If there's only 1 total image in the series, leave an empty base instance
            // and just have the entire image be stored.
            if (_instanceList.Count > 1)
            {
                XmlElement baseInstance = _seriesTagsStream.GetMemento(theDocument, settings);
                baseElement.AppendChild(baseInstance);
            }
            series.AppendChild(baseElement);

            foreach (InstanceXml instance in _instanceList.Values)
            {
                instance.SetBaseInstance(_seriesTagsStream);
                XmlElement instanceElement = instance.GetMemento(theDocument, settings);

                series.AppendChild(instanceElement);
            }

            return series;
        }

        internal void SetMemento(XmlNode theSeriesNode)
        {
            _dirty = true;
            _seriesInstanceUid = theSeriesNode.Attributes["UID"].Value;

            if (!theSeriesNode.HasChildNodes)
                return;

            XmlNode childNode = theSeriesNode.FirstChild;

            while (childNode != null)
            {
                // Just search for the first study node, parse it, then break
                if (childNode.Name.Equals("BaseInstance"))
                {
                    if (childNode.HasChildNodes)
                    {
                        XmlNode instanceNode = childNode.FirstChild;
                        if (instanceNode.Name.Equals("Instance"))
                        {
                            _seriesTagsStream = new BaseInstanceXml(instanceNode);
                        }
                    }
                }
                else if (childNode.Name.Equals("Instance"))
                {
                    // This assumes the BaseInstance is in the xml ahead of the actual instances, note, however,
                    // that if there is only 1 instance in the series, there will be no base instance value

                    InstanceXml instanceStream;

                    if (_seriesTagsStream == null)
                        instanceStream = new InstanceXml(childNode, null);
                    else
						instanceStream = new InstanceXml(childNode, _seriesTagsStream.Collection);

                    _instanceList.Add(instanceStream.SopInstanceUid, instanceStream);
                }

                childNode = childNode.NextSibling;
            }
        }

        #endregion

        #region IEnumerator Implementation

        public IEnumerator<InstanceXml> GetEnumerator()
        {
            return _instanceList.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public InstanceXml FindSourceImageInstanceXml(string instanceUid)
        {
            BuildSourceImageMap();
            InstanceXml instanceXml;
            if (_sourceImageList.TryGetValue(instanceUid, out instanceXml))
            {
                return instanceXml;
            }
            else
            {
                return null;
            }
        }

        private void BuildSourceImageMap()
        {
            if (_dirty || _sourceImageList==null)
            {
                _sourceImageList = new Dictionary<string, InstanceXml>();
                foreach(InstanceXml instanceXml in _instanceList.Values)
                {
                    if (instanceXml.SourceImageInfoList!=null)
                    {
                       foreach(SourceImageInfo sourceInfo in instanceXml.SourceImageInfoList)
                       {
                           _sourceImageList.Add(sourceInfo.SopInstanceUid, instanceXml);
                       }
                    }
                }
            }
        }
    }
}