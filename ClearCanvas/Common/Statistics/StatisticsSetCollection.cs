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
using System.Xml;

namespace ClearCanvas.Common.Statistics
{
    /// <summary>
    /// Base collection of <see cref="StatisticsSet"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class StatisticsSetCollection<T>
        where T : StatisticsSet, new()
    {
        #region Private members

        private List<T> _list = new List<T>();

        #endregion Private members

        #region public properties

        public List<T> Items
        {
            get { return _list; }
            set { _list = value; }
        }

        public int Count
        {
            get { return Items.Count; }
        }

        #endregion public properties

        #region public methods

        ///// <summary>
        ///// Returns a new instance of the underlying statistics set.
        ///// </summary>
        ///// <param name="name">Name to be assigned to the statistics set.</param>
        ///// <returns></returns>
        //public T NewStatistics(string name)
        //{
        //    T newStat = new T();
        //    newStat.Name = name;
        //    _list.Add(newStat);
        //    return newStat;
        //}

        /// <summary>
        /// Returns the statistics collection as a list of XML elements.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public virtual List<XmlElement> ToXmlElements(XmlDocument doc, bool recursive)
        {
            List<XmlElement> list = new List<XmlElement>();

            foreach (StatisticsSet item in Items)
            {
                XmlElement xml = item.GetXmlElement(doc, recursive);
                list.Add(xml);
            }

            return list;
        }

        #endregion public methods
    }
}