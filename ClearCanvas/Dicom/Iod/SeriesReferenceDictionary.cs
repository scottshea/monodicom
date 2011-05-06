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

using System.Collections.Generic;
using ClearCanvas.Dicom.Iod.Macros;
using ClearCanvas.Dicom.Iod.Macros.PresentationStateRelationship;

namespace ClearCanvas.Dicom.Iod
{
	public class SeriesReferenceDictionary
	{
		private readonly Dictionary<string, ImageSopInstanceReferenceDictionary> _dictionary = new Dictionary<string, ImageSopInstanceReferenceDictionary>();

		public SeriesReferenceDictionary(IEnumerable<IReferencedSeriesSequence> seriesReferences)
		{
			foreach (IReferencedSeriesSequence seriesReference in seriesReferences)
			{
				ImageSopInstanceReferenceDictionary imageSopDictionary = null;
				ImageSopInstanceReferenceMacro[] imageSopReferences = seriesReference.ReferencedImageSequence;

				if (imageSopReferences != null && imageSopReferences.Length > 0)
				{
					imageSopDictionary = new ImageSopInstanceReferenceDictionary(imageSopReferences);
				}

				_dictionary.Add(seriesReference.SeriesInstanceUid, imageSopDictionary);
			}
		}

		public bool ReferencesSeries(string seriesInstanceUid)
		{
			if (_dictionary.ContainsKey(seriesInstanceUid))
				return true;
			return false;
		}

		public bool ReferencesSop(string seriesInstanceUid, string sopInstanceUid)
		{
			if (_dictionary.ContainsKey(seriesInstanceUid))
			{
				ImageSopInstanceReferenceDictionary sopDictionary = _dictionary[seriesInstanceUid];
				if (sopDictionary == null || sopDictionary.ReferencesSop(sopInstanceUid))
					return true;
			}
			return false;
		}

		public bool ReferencesAllFrames(string seriesInstanceUid, string sopInstanceUid)
		{
			if (_dictionary.ContainsKey(seriesInstanceUid))
			{
				ImageSopInstanceReferenceDictionary sopDictionary = _dictionary[seriesInstanceUid];
				if (sopDictionary == null || sopDictionary.ReferencesAllFrames(sopInstanceUid))
					return true;
			}
			return false;
		}

		public bool ReferencesAllSegments(string seriesInstanceUid, string sopInstanceUid)
		{
			if (_dictionary.ContainsKey(seriesInstanceUid))
			{
				ImageSopInstanceReferenceDictionary sopDictionary = _dictionary[seriesInstanceUid];
				if (sopDictionary == null || sopDictionary.ReferencesAllSegments(sopInstanceUid))
					return true;
			}
			return false;
		}

		public bool ReferencesFrame(string seriesInstanceUid, string sopInstanceUid, int frameNumber)
		{
			if (_dictionary.ContainsKey(seriesInstanceUid))
			{
				ImageSopInstanceReferenceDictionary sopDictionary = _dictionary[seriesInstanceUid];
				if (sopDictionary == null || sopDictionary.ReferencesFrame(sopInstanceUid, frameNumber))
					return true;
			}
			return false;
		}

		public bool ReferencesSegment(string seriesInstanceUid, string sopInstanceUid, uint segmentNumber)
		{
			if (_dictionary.ContainsKey(seriesInstanceUid))
			{
				ImageSopInstanceReferenceDictionary sopDictionary = _dictionary[seriesInstanceUid];
				if (sopDictionary == null || sopDictionary.ReferencesSegment(sopInstanceUid, segmentNumber))
					return true;
			}
			return false;
		}
	}
}