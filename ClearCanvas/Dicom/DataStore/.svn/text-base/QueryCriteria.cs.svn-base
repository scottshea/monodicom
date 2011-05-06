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

using System.Collections;
using System.Collections.Generic;

namespace ClearCanvas.Dicom.DataStore
{
	internal class QueryCriteria : IEnumerable<KeyValuePair<DicomTagPath, string>>
	{
		private readonly IDictionary<DicomTagPath, string> _internalDictionary;

		public QueryCriteria(DicomAttributeCollection query)
		{
			_internalDictionary = new Dictionary<DicomTagPath, string>();
			BuildQueryDictionary(query, null, _internalDictionary);
		}

		public string this[DicomTagPath path]
		{
			get
			{
				if (!_internalDictionary.ContainsKey(path))
					return null;

				return _internalDictionary[path];
			}
		}

		private static void BuildQueryDictionary(IEnumerable<DicomAttribute> collection, DicomTagPath currentPath, IDictionary<DicomTagPath, string> queryDictionary)
		{
			foreach (DicomAttribute attribute in collection)
			{
				DicomTagPath path;
				if (currentPath == null)
					path = new DicomTagPath(attribute.Tag);
				else
					path = currentPath + attribute.Tag;

				if (attribute is DicomAttributeSQ && attribute.Values != null)
				{
					foreach (DicomAttributeCollection sequenceItem in (object[])attribute.Values)
						BuildQueryDictionary(sequenceItem, path, queryDictionary);
				}

				if (!attribute.IsEmpty)
					queryDictionary[path] = attribute.ToString() ?? "";
			}
		}

		#region IEnumerable<KeyValuePair<DicomTagPath,string>> Members

		public IEnumerator<KeyValuePair<DicomTagPath, string>> GetEnumerator()
		{
			return _internalDictionary.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _internalDictionary.GetEnumerator();
		}

		#endregion
	}
}
