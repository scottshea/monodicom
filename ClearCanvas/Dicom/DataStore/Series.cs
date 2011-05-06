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
using ClearCanvas.Dicom.Iod;
using ClearCanvas.Dicom.Utilities.Xml;

namespace ClearCanvas.Dicom.DataStore
{
	internal class Series : ISeries
    {
		#region Private Fields

		private readonly Study _parentStudy;
		private readonly SeriesXml _seriesXml;
		private List<ISopInstance> _sopInstances;

		#endregion

		internal Series(Study parentStudy, SeriesXml seriesXml)
        {
			_parentStudy = parentStudy;
			_seriesXml = seriesXml;
		}

		#region Private Members

		private List<ISopInstance> SopInstances
		{
			get
			{
				if (_sopInstances == null)
				{
					_sopInstances = new List<ISopInstance>();
					foreach (InstanceXml instanceXml in _seriesXml)
						_sopInstances.Add(new SopInstance(this, instanceXml));
				}

				return _sopInstances;
			}	
		}

		private InstanceXml GetFirstSopInstanceXml()
		{
			using (IEnumerator<InstanceXml> iterator = _seriesXml.GetEnumerator())
			{
				if (!iterator.MoveNext())
				{
					string message = String.Format("There are no instances in this series ({0}).", SeriesInstanceUid);
					throw new DataStoreException(message);
				}

				return iterator.Current;
			}
		}

		#endregion

		#region ISeries Members

		public IStudy GetParentStudy()
		{
			return _parentStudy;
		}

		public string SpecificCharacterSet
		{
			get { return GetFirstSopInstanceXml()[DicomTags.SpecificCharacterSet].ToString(); }
		}

		[QueryableProperty(DicomTags.StudyInstanceUid, IsHigherLevelUnique = true)]
		public string StudyInstanceUid
		{
			get { return _parentStudy.StudyInstanceUid; }
		}

		[QueryableProperty(DicomTags.SeriesInstanceUid, IsUnique = true, PostFilterOnly = true)]
		public string SeriesInstanceUid
		{
			get { return _seriesXml.SeriesInstanceUid; }
		}

		[QueryableProperty(DicomTags.Modality, PostFilterOnly = true)]
		public string Modality
		{
			get
			{
				return GetFirstSopInstanceXml()[DicomTags.Modality].GetString(0, "");
			}
		}

		[QueryableProperty(DicomTags.SeriesDescription, PostFilterOnly = true)]
		public string SeriesDescription
		{
			get
			{
				return GetFirstSopInstanceXml()[DicomTags.SeriesDescription].GetString(0, "");
			}
		}

		[QueryableProperty(DicomTags.SeriesNumber, IsRequired = true, PostFilterOnly = true)]
		public int SeriesNumber
		{
			get
			{
				return GetFirstSopInstanceXml()[DicomTags.SeriesNumber].GetInt32(0, 0);
			}
		}

		[QueryableProperty(DicomTags.NumberOfSeriesRelatedInstances, PostFilterOnly = true)]
		public int NumberOfSeriesRelatedInstances
		{
			get { return SopInstances.Count; }
		}

		int? ISeriesData.NumberOfSeriesRelatedInstances
		{
			get { return NumberOfSeriesRelatedInstances; }
		}

    	public IEnumerable<ISopInstance> GetSopInstances()
        {
    		return SopInstances;
        }

        #endregion
	}
}
