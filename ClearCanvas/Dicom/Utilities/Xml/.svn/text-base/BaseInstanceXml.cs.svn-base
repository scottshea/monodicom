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

using System.Xml;
using ClearCanvas.Common;

namespace ClearCanvas.Dicom.Utilities.Xml
{
	/// <summary>
	/// Class for representing a base instance of a series as XML.
	/// </summary>
	public class BaseInstanceXml : InstanceXml
	{
		/// <summary>
		/// Creates an empty instance of <see cref="BaseInstanceXml"/>.
		/// </summary>
		public BaseInstanceXml()
			: base(new InstanceXmlDicomAttributeCollection(), null, TransferSyntax.ExplicitVrLittleEndian)
		{
		}

		/// <summary>
		/// Creates an instance of <see cref="BaseInstanceXml"/> from a specified Xml node.
		/// </summary>
		/// <param name="node"></param>
		public BaseInstanceXml(XmlNode node)
			: base(node, null)
		{
		}

		/// <summary>
		/// Creates an instance of <see cref="BaseInstanceXml"/> based on the specified <see cref="DicomAttributeCollection"/>.
		/// </summary>
		/// <param name="collect1"></param>
		/// <param name="collect2"></param>
		public BaseInstanceXml(DicomAttributeCollection collect1, DicomAttributeCollection collect2)
			: this()
		{
			Platform.CheckForNullReference(collect1, "collect1");
			Platform.CheckForNullReference(collect2, "collect2");

			foreach (DicomAttribute attrib1 in collect1)
			{
				DicomAttribute attrib2;
				if ((attrib1 is DicomAttributeOB)
					|| (attrib1 is DicomAttributeOW)
					|| (attrib1 is DicomAttributeOF)
					|| (attrib1 is DicomFragmentSequence))
				{
					if (collect2.TryGetAttribute(attrib1.Tag, out attrib2))
						((IPrivateInstanceXmlDicomAttributeCollection)Collection).ExcludedTagsHelper.Add(attrib1.Tag);
					continue;
				}

				if (collect2.TryGetAttribute(attrib1.Tag, out attrib2))
				{
					if (!attrib1.IsEmpty && attrib1.Equals(attrib2)) //don't store empty tags in the base collection.
					{
						Collection[attrib1.Tag] = attrib1.Copy();
					}
				}
			}

			if (collect1 is IInstanceXmlDicomAttributeCollection && collect2 is IInstanceXmlDicomAttributeCollection)
			{
				IInstanceXmlDicomAttributeCollection collection2 = (IInstanceXmlDicomAttributeCollection) collect2;
				foreach (DicomTag tag in ((IInstanceXmlDicomAttributeCollection)collect1).ExcludedTags)
				{
					if (collection2.ExcludedTags.Contains(tag))
						PrivateCollection.ExcludedTagsHelper.Add(tag);
				}
			}
		}

		public new InstanceXmlDicomAttributeCollection Collection
		{
			get { return (InstanceXmlDicomAttributeCollection)base.Collection; }
		}

		internal IPrivateInstanceXmlDicomAttributeCollection PrivateCollection
		{
			get { return (IPrivateInstanceXmlDicomAttributeCollection)base.Collection; }
		}
	}
}