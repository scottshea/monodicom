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
using ClearCanvas.Common;
using ClearCanvas.Dicom;

namespace ClearCanvas.Dicom.Iod.Modules
{
	/// <summary>
	/// OverlayActivation Module
	/// </summary>
	/// <seealso cref="OverlayActivationModuleIod.this"/>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.11.7 (Table C.11.7-1)</remarks>
	public class OverlayActivationModuleIod : IodBase, IEnumerable<OverlayActivation>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OverlayActivationModuleIod"/> class.
		/// </summary>	
		public OverlayActivationModuleIod() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="OverlayActivationModuleIod"/> class.
		/// </summary>
		public OverlayActivationModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider) {}

		/// <summary>
		/// Gets the Overlay Activation groups in the underlying collection. The index must be between 0 and 15 inclusive.
		/// </summary>
		/// <remarks>
		/// The implementation of the Overlay Activation module involving repeating groups is a holdover
		/// from previous versions of the DICOM Standard. For each of the 16 allowed overlays, there
		/// exists a separate set of tags bearing the same element numbers but with a group number
		/// of the form 60xx, where xx is an even number from 00 to 1E inclusive. In order to make
		/// these IOD classes easier to use, each of these 16 sets of tags are represented as
		/// separate items of a collection, and may be addressed by an index between 0 and 15
		/// inclusive (mapping to the even groups between 6000 and 601E).
		/// </remarks>
		public OverlayActivation this[int index]
		{
			get
			{
				Platform.CheckArgumentRange(index, 0, 15, "index");
				return new OverlayActivation(index, this.DicomAttributeProvider);
			}
		}

		public void Delete(int index)
		{
			Platform.CheckArgumentRange(index, 0, 15, "index");
			foreach (uint tag in this[index].DefinedTags)
				base.DicomAttributeProvider[tag] = null;
		}

		public bool HasOverlayActivationLayer(int index)
		{
			if (index < 0 || index >= 16)
				return false;
			DicomAttribute attrib;
			if (!base.DicomAttributeProvider.TryGetAttribute(OverlayPlaneModuleIod.ComputeTagOffset(index) + DicomTags.OverlayActivationLayer, out attrib))
				return false;
			else
				return true;
		}

		/// <summary>
		/// Gets an enumeration of <see cref="DicomTag"/>s used by this module.
		/// </summary>
		public static IEnumerable<uint> DefinedTags
		{
			get
			{
				for (int n = 0; n < 16; n++)
				{
					uint tagOffset = OverlayPlaneModuleIod.ComputeTagOffset(n);
					yield return tagOffset + DicomTags.OverlayActivationLayer;
				}
			}
		}

		#region IEnumerable<OverlayActivation> Members

		public IEnumerator<OverlayActivation> GetEnumerator()
		{
			for (int n = 0; n < 16; n++)
			{
				if (this.HasOverlayActivationLayer(n))
					yield return this[n];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}

	/// <summary>
	/// OverlayActivation Group
	/// </summary>
	/// <seealso cref="OverlayActivationModuleIod.this"/>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.11.7 (Table C.11.7-1)</remarks>
	public class OverlayActivation : IodBase
	{
		private readonly int _index;
		private readonly uint _tagOffset;

		/// <summary>
		/// Initializes a new instance of the <see cref="OverlayActivation"/> class.
		/// </summary>
		/// <param name="index">The zero-based index of the overlay to which this module refers.</param>
		/// <param name="dicomAttributeProvider">The underlying collection.</param>
		internal OverlayActivation(int index, IDicomAttributeProvider dicomAttributeProvider)
			: base(dicomAttributeProvider)
		{
			_index = index;
			_tagOffset = OverlayPlaneModuleIod.ComputeTagOffset(_index);
		}

		/// <summary>
		/// Gets the zero-based index of the overlay to which this group refers (0-15).
		/// </summary>
		public int Index
		{
			get { return _index; }
		}

		/// <summary>
		/// Gets the DICOM tag group number.
		/// </summary>
		public ushort Group
		{
			get { return (ushort) (_index*2 + 0x6000); }
		}

		/// <summary>
		/// Gets the DICOM tag value offset from the defined base tags (such as <see cref="DicomTags.OverlayActivationLayer"/>).
		/// </summary>
		public uint TagOffset
		{
			get { return _tagOffset; }
		}

		/// <summary>
		/// Gets or sets the value of OverlayActivationLayer in the underlying collection. Type 2C.
		/// </summary>
		public string OverlayActivationLayer
		{
			get { return base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayActivationLayer].GetString(0, string.Empty); }
			set { base.DicomAttributeProvider[_tagOffset + DicomTags.OverlayActivationLayer].SetString(0, value ?? string.Empty); }
		}

		/// <summary>
		/// Gets an enumeration of <see cref="DicomTag"/>s used by this group.
		/// </summary>
		public IEnumerable<uint> DefinedTags
		{
			get { yield return _tagOffset + DicomTags.OverlayActivationLayer; }
		}
	}
}