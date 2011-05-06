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
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Dicom.DataStore
{
	public class DicomTagPath : IEquatable<DicomTagPath>, IEquatable<DicomTag>, IEquatable<string>, IEquatable<uint>
	{
		private static readonly string _exceptionFormatInvalidTagPath = "The specified Dicom Tag Path is invalid: {0}.";

		private static readonly char[] _pathSeparator = new char[] { '\\' };
		private static readonly char[] _tagSeparator = new char[] { ',' };

		private List<DicomTag> _tags;
		private string _path;

		protected DicomTagPath()
			: this(new DicomTag[] {})
		{
		}

		public DicomTagPath(string path)
			: this(GetTags(path))
		{
		}

		public DicomTagPath(uint tag)
			: this(new uint[] { tag })
		{
		}

		public DicomTagPath(IEnumerable<uint> tags)
			: this(GetTags(tags))
		{
		}

		public DicomTagPath(DicomTag tag)
			: this(new DicomTag[] { tag })
		{
		}

		public DicomTagPath(IEnumerable<DicomTag> tags)
		{
			BuildPath(tags);
		}

		public virtual string Path
		{
			get { return _path; }
			protected set 
			{
				BuildPath(GetTags(value));
			}
		}

		public IList<DicomTag> TagsInPath
		{
			get { return _tags.AsReadOnly(); }
			protected set
			{
				BuildPath(value);
			}
		}

		public DicomVr ValueRepresentation
		{
			get { return _tags[_tags.Count - 1].VR; }	
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
				return true;

			if (obj is DicomTagPath)
				return Equals(obj as DicomTagPath);
			if (obj is DicomTag)
				return Equals(obj as DicomTag);
			if (obj is string)
				return Equals(obj as string);
			if (obj is uint)
				return Equals((uint)obj);

			return false;
		}

		#region IEquatable<DicomTagPath> Members

		public bool Equals(DicomTagPath other)
		{
			if (other == null)
				return false;

			return other.Path.Equals(Path);
		}

		#endregion	

		#region IEquatable<DicomTag> Members

		public bool Equals(DicomTag other)
		{
			if (other == null)
				return false;

			if (_tags.Count != 1)
				return false;

			return _tags[0].Equals(other);
		}

		#endregion

		#region IEquatable<string> Members

		public bool Equals(string other)
		{
			return Path.Equals(other);
		}

		#endregion

		#region IEquatable<uint> Members

		public bool Equals(uint other)
		{
			if (_tags.Count != 1)
				return false;

			return _tags[0].TagValue.Equals(other);
		}

		#endregion

		public override int GetHashCode()
		{
			return _path.GetHashCode();
		}

		public override string ToString()
		{
			return _path;
		}

		public static DicomTagPath operator +(DicomTagPath left, DicomTagPath right)
		{
			List<DicomTag> tags = new List<DicomTag>(left.TagsInPath);
			tags.AddRange(right.TagsInPath);
			return new DicomTagPath(tags);
		}

		public static DicomTagPath operator +(DicomTagPath left, DicomTag right)
		{
			List<DicomTag> tags = new List<DicomTag>(left.TagsInPath);
			tags.Add(right);
			return new DicomTagPath(tags);
		}

		public static DicomTagPath operator +(DicomTagPath left, uint right)
		{
			List<DicomTag> tags = new List<DicomTag>(left.TagsInPath);
			tags.Add(DicomTagDictionary.GetDicomTag(right));
			return new DicomTagPath(tags);
		}
		
		public static implicit operator DicomTagPath(DicomTag tag)
		{
			return new DicomTagPath(tag);
		}

		public static implicit operator DicomTagPath(uint tag)
		{
			return new DicomTagPath(tag);
		}

		/// <summary>
		/// Implicit cast to a String object, for ease of use.
		/// </summary>
		public static implicit operator string(DicomTagPath path)
		{
			return path.ToString();
		}

		private void BuildPath(IEnumerable<DicomTag> dicomTags)
		{
			Platform.CheckForNullReference(dicomTags, "dicomTags");
			_tags = new List<DicomTag>(dicomTags);
			_path = StringUtilities.Combine(dicomTags, "\\", delegate(DicomTag tag) { return String.Format("({0:x4},{1:x4})", tag.Group, tag.Element); });
		}

		private static IEnumerable<DicomTag> GetTags(string path)
		{
			Platform.CheckForEmptyString(path, "path");

			List<DicomTag> dicomTags = new List<DicomTag>();

			string[] groupElementValues = path.Split(_pathSeparator);

			foreach (string groupElement in groupElementValues)
			{
				string[] values = groupElement.Split(_tagSeparator);
				if (values.Length != 2)
					throw new ArgumentException(String.Format(_exceptionFormatInvalidTagPath, path));

				string group = values[0];
				if (!group.StartsWith("(") || group.Length != 5)
					throw new ArgumentException(String.Format(_exceptionFormatInvalidTagPath, path));

				string element = values[1];
				if (!element.EndsWith(")") || element.Length != 5)
					throw new ArgumentException(String.Format(_exceptionFormatInvalidTagPath, path));

				try
				{
					ushort groupValue = System.Convert.ToUInt16(group.TrimStart('('), 16);
					ushort elementValue = System.Convert.ToUInt16(element.TrimEnd(')'), 16);

					dicomTags.Add(NewTag(DicomTag.GetTagValue(groupValue, elementValue)));

				}
				catch
				{
					throw new ArgumentException(String.Format(_exceptionFormatInvalidTagPath, path));
				}
			}

			ValidatePath(dicomTags);

			return dicomTags;
		}

		private static void ValidatePath(IList<DicomTag> dicomTags)
		{
			for (int i = 0; i < dicomTags.Count - 1; ++i)
			{
				if (dicomTags[i].VR != DicomVr.SQvr)
					throw new ArgumentException("All but the last item in the path must have VR = SQ.");
			}
		}

		private static IEnumerable<DicomTag> GetTags(IEnumerable<uint> tags)
		{
			foreach (uint tag in tags)
				yield return NewTag(tag);
		}

		private static DicomTag NewTag(uint tag)
		{
			DicomTag returnTag = DicomTagDictionary.GetDicomTag(tag);
			if (returnTag == null)
				returnTag = new DicomTag(tag, "Unknown Tag", "UnknownTag", DicomVr.UNvr, false, 1, uint.MaxValue, false);

			return returnTag;
		}
	}
}
