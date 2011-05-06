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
using ClearCanvas.Common;
using ClearCanvas.Dicom.Iod.Macros;

namespace ClearCanvas.Dicom.Iod
{
	public class ImageSopInstanceReferenceDictionary
	{
		private readonly Dictionary<string, IList<int>> _frameDictionary = new Dictionary<string, IList<int>>();
		private readonly Dictionary<string, IList<uint>> _segmentDictionary = new Dictionary<string, IList<uint>>();
		private readonly bool _emptyDictionaryMatchesAll;

		public ImageSopInstanceReferenceDictionary(IEnumerable<ImageSopInstanceReferenceMacro> imageSopReferences) : this(imageSopReferences ?? new ImageSopInstanceReferenceMacro[0], false) {}

		public ImageSopInstanceReferenceDictionary(IEnumerable<ImageSopInstanceReferenceMacro> imageSopReferences, bool emptyDictionaryMatchesAll)
		{
			Platform.CheckForNullReference(imageSopReferences, "imageSopReferences");

			_emptyDictionaryMatchesAll = emptyDictionaryMatchesAll;

			foreach (ImageSopInstanceReferenceMacro imageSopReference in imageSopReferences)
			{
				DicomAttributeIS frames = imageSopReference.ReferencedFrameNumber;
				List<int> frameList = null;
				if (!frames.IsNull && !frames.IsEmpty && frames.Count > 0)
				{
					frameList = new List<int>();
					for (int n = 0; n < frames.Count; n++)
						frameList.Add(frames.GetInt32(n, -1));
				}
				_frameDictionary.Add(imageSopReference.ReferencedSopInstanceUid, frameList);

				DicomAttributeUS segments = imageSopReference.ReferencedSegmentNumber;
				List<uint> segmentList = null;
				if (!segments.IsNull && !segments.IsEmpty && segments.Count > 0)
				{
					segmentList = new List<uint>();
					for (int n = 0; n < segments.Count; n++)
						segmentList.Add(segments.GetUInt32(n, 0));
				}
				_segmentDictionary.Add(imageSopReference.ReferencedSopInstanceUid, segmentList);
			}
		}

		public bool IsEmpty
		{
			get { return _frameDictionary.Count == 0; }
		}

		public bool ReferencesSop(string imageSopInstanceUid)
		{
			return ReferencesAny(imageSopInstanceUid);
		}

		public bool ReferencesAny(string imageSopInstanceUid)
		{
			if (_emptyDictionaryMatchesAll && this.IsEmpty)
				return true; // return true if dictionary is empty and empty matches all

			if (_frameDictionary.ContainsKey(imageSopInstanceUid))
				return true;
			return false;
		}

		public bool ReferencesAllFrames(string imageSopInstanceUid)
		{
			if (_emptyDictionaryMatchesAll && this.IsEmpty)
				return true; // return true if dictionary is empty and empty matches all

			if (_frameDictionary.ContainsKey(imageSopInstanceUid))
			{
				IList<int> frames = _frameDictionary[imageSopInstanceUid];
				if (frames == null)
					return true;
			}
			return false;
		}

		public bool ReferencesAllSegments(string imageSopInstanceUid) 
		{
			if (_emptyDictionaryMatchesAll && this.IsEmpty)
				return true; // return true if dictionary is empty and empty matches all

			if (_segmentDictionary.ContainsKey(imageSopInstanceUid))
			{
				IList<uint> segments = _segmentDictionary[imageSopInstanceUid];
				if (segments == null)
					return true;
			}
			return false;
		}

		public bool ReferencesFrame(string imageSopInstanceUid, int frameNumber) 
		{
			if (_emptyDictionaryMatchesAll && this.IsEmpty)
				return true; // return true if dictionary is empty and empty matches all

			if (_frameDictionary.ContainsKey(imageSopInstanceUid))
			{
				IList<int> frames = _frameDictionary[imageSopInstanceUid];
				if (frames == null || frames.Contains(frameNumber))
					return true;
			}
			return false;
		}

		public bool ReferencesSegment(string imageSopInstanceUid, uint segmentNumber) 
		{
			if (_emptyDictionaryMatchesAll && this.IsEmpty)
				return true; // return true if dictionary is empty and empty matches all

			if (_segmentDictionary.ContainsKey(imageSopInstanceUid))
			{
				IList<uint> segments = _segmentDictionary[imageSopInstanceUid];
				if (segments == null || segments.Contains(segmentNumber))
					return true;
			}
			return false;
		}
	}
}