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
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Dicom.Utilities.Xml
{
	//TODO: this is not ideal, but is the most straightforward given the current study xml design.  Later,
	//we should refactor for a cleaner API.
	public interface IInstanceXmlDicomAttributeCollection : IDicomAttributeProvider, IEnumerable<DicomAttribute>
	{
		IList<DicomTag> ExcludedTags { get; }

		bool IsTagExcluded(uint tag);
		bool HasExcludedTags(bool recursive);
	}

	internal interface IPrivateInstanceXmlDicomAttributeCollection : IInstanceXmlDicomAttributeCollection
	{
		ExcludedTagsHelper ExcludedTagsHelper { get; }
	}

	internal class ExcludedTagsHelper : IEquatable<ExcludedTagsHelper>
	{
		private readonly IInstanceXmlDicomAttributeCollection _parent;
		private readonly SortedList<DicomTag, DicomTag> _excludedTags;

		public ExcludedTagsHelper(IInstanceXmlDicomAttributeCollection parent)
		{
			_parent = parent;
			_excludedTags = new SortedList<DicomTag, DicomTag>();
		}

		public IList<DicomTag> ExcludedTags
		{
			get
			{
				Cleanup();
				return _excludedTags.Keys;
			}
		}

		internal void Cleanup()
		{
			List<DicomTag> tagsToRemove = new List<DicomTag>();
			foreach (DicomTag tag in _excludedTags.Keys)
			{
				DicomAttribute attribute;
				if (_parent.TryGetAttribute(tag, out attribute) && !attribute.IsEmpty)
					tagsToRemove.Add(tag);
			}

			foreach (DicomTag tag in tagsToRemove)
				Remove(tag);
		}

		public void Remove(DicomTag tag)
		{
			DicomTag existingTag;
			if (_excludedTags.TryGetValue(tag, out existingTag))
				_excludedTags.Remove(existingTag);
		}

		public void Add(DicomTag tag)
		{
			DicomTag existingTag;
			if (!_excludedTags.TryGetValue(tag, out existingTag))
				_excludedTags.Add(tag, tag);
		}

		public void Add(IEnumerable<DicomTag> tagList)
		{
			foreach (DicomTag tag in tagList)
				Add(tag);
		}

		public bool IsTagExcluded(uint tag)
		{
			return CollectionUtils.Contains(ExcludedTags,
				delegate(DicomTag dicomTag) { return dicomTag.TagValue == tag; });
		}

		public bool HasExcludedTags(bool recursive)
		{
			if (ExcludedTags.Count > 0)
				return true;

			if (recursive)
			{
				foreach (DicomAttribute attribute in _parent)
				{
					if (attribute.Tag.VR == DicomVr.SQvr)
					{
						DicomSequenceItem[] items = attribute.Values as DicomSequenceItem[];
						if (items != null)
						{
							foreach (DicomSequenceItem item in items)
							{
								if (item is InstanceXmlDicomSequenceItem)
								{
									if (((InstanceXmlDicomSequenceItem)item).HasExcludedTags(recursive))
										return true;
								}
							}
						}
					}
				}
			}

			return false;
		}

		#region IEquatable<ExcludedTagsHelper> Members

		public bool Equals(ExcludedTagsHelper other)
		{
			if (other == null)
				return false;

			if (other.ExcludedTags.Count != ExcludedTags.Count)
				return false;

			foreach (DicomTag tag in ExcludedTags)
			{
				if (!other.ExcludedTags.Contains(tag))
					return false;
			}

			return true;
		}

		#endregion

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj is ExcludedTagsHelper)
				return Equals((ExcludedTagsHelper)obj);

			return false;
		}
	}

	public class InstanceXmlDicomAttributeCollection : DicomAttributeCollection, IPrivateInstanceXmlDicomAttributeCollection
	{
		private readonly ExcludedTagsHelper _excludedTagsHelper;

		internal InstanceXmlDicomAttributeCollection(DicomAttributeCollection source, bool copyBinary, bool copyPrivate, bool copyUnknown, uint stopTag)
			: base(source, copyBinary, copyPrivate, copyUnknown, stopTag)
		{
			_excludedTagsHelper = new ExcludedTagsHelper(this);
			if (source is IInstanceXmlDicomAttributeCollection)
				_excludedTagsHelper.Add(((IInstanceXmlDicomAttributeCollection)source).ExcludedTags);
		}

		internal InstanceXmlDicomAttributeCollection()
		{
			_excludedTagsHelper = new ExcludedTagsHelper(this);
		}

		public override DicomAttributeCollection Copy(bool copyBinary, bool copyPrivate, bool copyUnknown, uint stopTag)
		{
			return new InstanceXmlDicomAttributeCollection(this, copyBinary, copyPrivate, copyUnknown, stopTag);
		}

		#region IInstanceXmlDicomAttributeCollection Members

		public IList<DicomTag> ExcludedTags
		{
			get { return _excludedTagsHelper.ExcludedTags; }
		}

		public bool IsTagExcluded(uint tag)
		{
			return _excludedTagsHelper.IsTagExcluded(tag);
		}

		public bool HasExcludedTags(bool recursive)
		{
			return _excludedTagsHelper.HasExcludedTags(recursive);
		}

		#endregion

		#region IInternalInstanceXmlDicomAttributeCollection Members

		ExcludedTagsHelper IPrivateInstanceXmlDicomAttributeCollection.ExcludedTagsHelper
		{
			get { return _excludedTagsHelper; }
		}

		#endregion

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj is InstanceXmlDicomAttributeCollection)
			{
				if (!_excludedTagsHelper.Equals(((InstanceXmlDicomAttributeCollection)obj)._excludedTagsHelper))
					return false;
			}

			return base.Equals(obj);
		}
	}

	public class InstanceXmlDicomSequenceItem : DicomSequenceItem, IPrivateInstanceXmlDicomAttributeCollection
	{
		private readonly ExcludedTagsHelper _excludedTagsHelper;

		internal InstanceXmlDicomSequenceItem(DicomSequenceItem source, bool copyBinary, bool copyPrivate, bool copyUnknown)
			: base(source, copyBinary, copyPrivate, copyUnknown)
		{
			_excludedTagsHelper = new ExcludedTagsHelper(this);
			if (source is IInstanceXmlDicomAttributeCollection)
				_excludedTagsHelper.Add(((IInstanceXmlDicomAttributeCollection)source).ExcludedTags);
		}

		internal InstanceXmlDicomSequenceItem()
		{
			_excludedTagsHelper = new ExcludedTagsHelper(this);
		}

		public override DicomAttributeCollection Copy(bool copyBinary, bool copyPrivate, bool copyUnknown)
		{
			return new InstanceXmlDicomSequenceItem(this, copyBinary, copyPrivate, copyUnknown);
		}

		#region IInstanceXmlDicomAttributeCollection Members

		public IList<DicomTag> ExcludedTags
		{
			get { return _excludedTagsHelper.ExcludedTags; }
		}

		public bool IsTagExcluded(uint tag)
		{
			return _excludedTagsHelper.IsTagExcluded(tag);
		}

		public bool HasExcludedTags(bool recursive)
		{
			return _excludedTagsHelper.HasExcludedTags(recursive);
		}

		#endregion

		#region IInternalInstanceXmlDicomAttributeCollection Members

		ExcludedTagsHelper IPrivateInstanceXmlDicomAttributeCollection.ExcludedTagsHelper
		{
			get { return _excludedTagsHelper; }
		}

		#endregion

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj is InstanceXmlDicomSequenceItem)
			{
				if (!_excludedTagsHelper.Equals(((InstanceXmlDicomSequenceItem)obj)._excludedTagsHelper))
					return false;
			}

			return base.Equals(obj);
		}
	}
}