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

namespace ClearCanvas.Dicom.Iod.Modules
{
	/// <summary>
	/// C.9.3 Multi-frame Overlay Module, PS 3.3 - 2008
	/// </summary>
	public class MultiframeOverlayModule : IodBase
	{
		#region Constructors
        /// <summary>
		/// Initializes a new instance of the <see cref="MultiframeOverlayModule"/> class.
        /// </summary>
        public MultiframeOverlayModule()
        {
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="MultiframeOverlayModule"/> class.
        /// </summary>
		public MultiframeOverlayModule(IDicomAttributeProvider dicomAttributeProvider)
			: base(dicomAttributeProvider)
        {
        }
        #endregion

		/// <summary>
		/// Number of Frames in Overlay. Required if Overlay data contains multiple frames.
		/// </summary>
		/// <remarks>
		/// <para>
		/// A Multi-frame Overlay is defined as an Overlay whose overlay data consists of a sequential set of
		/// individual Overlay frames. A Multi-frame Overlay is transmitted as a single contiguous stream of
		/// overlay data. Frame delimiters are not contained within the data stream.
		/// </para>
		/// <para>
		///Each individual frame shall be defined (and thus can be identified) by the Attributes in the Overlay
		///Plane Module (see C.9.2).
		/// </para>
		/// <para>
		///The total number of frames contained within a Multi-frame Overlay is conveyed in the Number of
		///Frames in Overlay (60xx,0015).
		/// </para>
		/// <para>
		///The frames within a Multi-frame Overlay shall be conveyed as a logical sequence. If Multi-frame
		///Overlays are related to a Multi-frame Image, the order of the Overlay Frames are one to one with
		///the order of the Image frames. Otherwise, no attribute is used to indicate the sequencing of the
		///Overlay Frames. If Image Frame Origin (60xx,0051) is present, the Overlay frames are applied
		///one to one to the Image frames, beginning at the indicated frame number. Otherwise, no attribute
		///is used to indicated the sequencing of the Overlay Frames.
		/// </para>
		/// <para>
		///The Number of Frames in Overlay (60xx,0015) plus the Image Frame Origin (60xx,0051) minus 1
		///shall be less than or equal to the total number of frames in the Multi-frame Image.
		/// </para>
		/// <para>
		///If the Overlay data are embedded in the pixel data, then the Image Frame Origin (60xx,0051)
		///must be 1 and the Number of Frames in Overlay (60xx,0015) must equal the number of frames in
		///the Multi-frame Image.
		/// </para>
		/// </remarks>
		public ushort NumberOfFramesInOverlay
		{
			get { return DicomAttributeProvider[DicomTags.NumberOfFramesInOverlay].GetUInt16(0, 0); }
			set { DicomAttributeProvider[DicomTags.NumberOfFramesInOverlay].SetUInt16(0, value); }
		}

		/// <summary>
		/// Frame number of Multi-frame Image to which this overlay applies; frames are numbered from 1.
		/// </summary>
		public ushort ImageFrameOrigin
		{
			get { return DicomAttributeProvider[DicomTags.ImageFrameOrigin].GetUInt16(0, 0); }
			set { DicomAttributeProvider[DicomTags.ImageFrameOrigin].SetUInt16(0, value); }
		}
	}
}
