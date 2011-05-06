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

namespace ClearCanvas.Dicom.Iod.Sequences
{
    /// <summary>
    /// Basic Color Image Sequence
    /// </summary>
    /// <remarks>As per Dicom Doc 3, Table C.13-5 (pg 871)</remarks>
    public class BasicColorImageSequenceIod : SequenceIodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicColorImageSequenceIod"/> class.
        /// </summary>
        public BasicColorImageSequenceIod()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicColorImageSequenceIod"/> class.
        /// </summary>
        /// <param name="dicomSequenceItem">The dicom sequence item.</param>
        public BasicColorImageSequenceIod(DicomSequenceItem dicomSequenceItem)
            : base(dicomSequenceItem)
        {
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the samples per pixel.  Number of samples (planes) in this image.
        /// <para>Possible values for Basic Color Sequence Iod is 3.</para>
        /// </summary>
        /// <value>The samples per pixel.</value>
        /// <remarks>See Part 3, C.7.6.3.1.1 for more info.</remarks>
        public ushort SamplesPerPixel
        {
            get { return base.DicomAttributeProvider[DicomTags.SamplesPerPixel].GetUInt16(0, 0); }
            set { base.DicomAttributeProvider[DicomTags.SamplesPerPixel].SetUInt16(0, value); }
        }

        /// <summary>
        /// Gets or sets the photometric interpretation.
        /// <para>Possible values for Basic Grayscale SequenceIod are RGB.</para>
        /// </summary>
        /// <value>The photometric interpretation.</value>
        public PhotometricInterpretation PhotometricInterpretation
        {
            get { return PhotometricInterpretation.FromCodeString(base.DicomAttributeProvider[DicomTags.PhotometricInterpretation].GetString(0, String.Empty)); }
            set
            {
				if (value == null)
					base.DicomAttributeProvider[DicomTags.PhotometricInterpretation] = null;
				else
					base.DicomAttributeProvider[DicomTags.PhotometricInterpretation].SetStringValue(value.Code);
            }
        }

        /// <summary>
        /// Gets or sets the planar configuration.
        /// <para>Possible value for Basic Grayscale SequenceIod is 1 (frame interleave).</para>
        /// </summary>
        /// <value>The planar configuration.</value>
        public ushort PlanarConfiguration
        {
            get { return base.DicomAttributeProvider[DicomTags.PlanarConfiguration].GetUInt16(0, 0); }
            set { base.DicomAttributeProvider[DicomTags.PlanarConfiguration].SetUInt16(0, value); }
        }

        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>The rows.</value>
        public ushort Rows
        {
            get { return base.DicomAttributeProvider[DicomTags.Rows].GetUInt16(0, 0); }
            set { base.DicomAttributeProvider[DicomTags.Rows].SetUInt16(0, value); }
        }

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>The columns.</value>
        public ushort Columns
        {
            get { return base.DicomAttributeProvider[DicomTags.Columns].GetUInt16(0, 0); }
            set { base.DicomAttributeProvider[DicomTags.Columns].SetUInt16(0, value); }
        }

        /// <summary>
        /// Gets or sets the pixel aspect ratio.
        /// </summary>
        /// <value>The pixel aspect ratio.</value>
        public int PixelAspectRatio
        {
            get { return base.DicomAttributeProvider[DicomTags.PixelAspectRatio].GetInt32(0, 0); }
            set { base.DicomAttributeProvider[DicomTags.PixelAspectRatio].SetInt32(0, value); }
        }

        /// <summary>
        /// Gets or sets the bits allocated.
        /// <para>Possible values for Basic Color Sequence Iod is 8.</para>
        /// </summary>
        /// <value>The bits allocated.</value>
        public ushort BitsAllocated
        {
            get { return base.DicomAttributeProvider[DicomTags.BitsAllocated].GetUInt16(0, 0); }
            set { base.DicomAttributeProvider[DicomTags.BitsAllocated].SetUInt16(0, value); }
        }

        /// <summary>
        /// Gets or sets the bits stored.
        /// <para>Possible values for Basic Color Sequence Iod is 8.</para>
        /// </summary>
        /// <value>The bits stored.</value>
        public ushort BitsStored
        {
            get { return base.DicomAttributeProvider[DicomTags.BitsStored].GetUInt16(0, 0); }
            set { base.DicomAttributeProvider[DicomTags.BitsStored].SetUInt16(0, value); }
        }

        /// <summary>
        /// Gets or sets the high bit.
        /// <para>Possible values for Basic Color Sequence Iod is 7.</para>
        /// </summary>
        /// <value>The high bit.</value>
        public ushort HighBit
        {
            get { return base.DicomAttributeProvider[DicomTags.HighBit].GetUInt16(0, 0); }
            set { base.DicomAttributeProvider[DicomTags.HighBit].SetUInt16(0, value); }
        }

        /// <summary>
        /// Gets or sets the pixel representation.Data representation of the pixel samples. 
        /// Each sample shall have the same pixel representation. 
        /// <para>Possible values for Basic Color Sequence Iod is 0 (000H).</para>
        /// </summary>
        /// <value>The pixel representation.</value>
        public string PixelRepresentation
        {
            get { return base.DicomAttributeProvider[DicomTags.PixelRepresentation].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PixelRepresentation].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the pixel data.
        /// </summary>
        /// <value>The pixel data.</value>
        public byte[] PixelData
        {
            get
            {
            	DicomAttribute attribute = base.DicomAttributeProvider[DicomTags.PixelData];
				if (!attribute.IsNull && !attribute.IsEmpty)
                    return (byte[])attribute.Values;
                else
                    return null;
            }
            set { base.DicomAttributeProvider[DicomTags.PixelData].Values = value; }
        }

        #endregion

    }


}
