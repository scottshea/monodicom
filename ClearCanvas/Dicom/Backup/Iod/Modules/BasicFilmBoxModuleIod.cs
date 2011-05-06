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
using System.Globalization;
using ClearCanvas.Dicom.Iod.Sequences;

namespace ClearCanvas.Dicom.Iod.Modules
{
    /// <summary>
    /// Basic Film Box Presentation and Relationship Module as per Part 3, C.13-3 (pg 862) and C.13.4 (pg 869)
    /// </summary>
    public class BasicFilmBoxModuleIod : IodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FilmBoxModuleIod"/> class.
        /// </summary>
        public BasicFilmBoxModuleIod()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilmBoxModuleIod"/> class.
        /// </summary>
		public BasicFilmBoxModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider)
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Type of image display format. Enumerated Values:
        /// <para>
        /// STANDARD\C,R : film contains equal size rectangular image boxes with R rows of image boxes and C columns of image boxes; C and R are integers.
        /// </para>
        /// <para>
        /// ROW\R1,R2,R3, etc. : film contains rows with equal size rectangular image boxes with R1 image boxes in the first row, R2 image boxes in second row, 
        /// R3 image boxes in third row, etc.; R1, R2, R3, etc. are integers.
        /// </para>
        /// <para>
        /// COL\C1,C2,C3, etc.: film contains columns with equal size rectangular image boxes with C1 image boxes in the first column, C2 image boxes in second
        ///  column, C3 image boxes in third column, etc.; C1, C2, C3, etc. are integers.
        /// </para>
        /// <para>
        /// SLIDE : film contains 35mm slides; the number of slides for a particular film size is configuration dependent.
        /// </para>
        /// <para>
        /// SUPERSLIDE : film contains 40mm slides; the number of slides for a particular film size is configuration dependent.
        /// </para>
        /// <para>
        /// CUSTOM\i : film contains a customized ordering of rectangular image boxes; i identifies the image display format; the definition of the image display
        /// formats is defined in the Conformance Statement; i is an integer.
        /// </para>
        /// </summary>
        /// <value></value>
        public string ImageDisplayFormat
        {
            get { return base.DicomAttributeProvider[DicomTags.ImageDisplayFormat].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.ImageDisplayFormat].SetStringValue(value); }
        }

        /// <summary>
        /// Identification of annotation display format. The definition of the annotation display formats and the
        /// annotation box position sequence are defined in the Conformance Statement.
        /// </summary>
        /// <value>The annotation display format id.</value>
        public string AnnotationDisplayFormatId
        {
            get { return base.DicomAttributeProvider[DicomTags.AnnotationDisplayFormatId].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.AnnotationDisplayFormatId].SetString(0, value); }
        }


        /// <summary>
        /// Gets or sets the film orientation.
        /// </summary>
        /// <value>The film orientation.</value>
        public FilmOrientation FilmOrientation
        {
            get { return IodBase.ParseEnum<FilmOrientation>(base.DicomAttributeProvider[DicomTags.FilmOrientation].GetString(0, String.Empty), FilmOrientation.None); }
            set { IodBase.SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.FilmOrientation], value, false); }
        }

        /// <summary>
        /// Gets or sets the film size id.
        /// </summary>
        /// <value>The film size id.</value>
        public FilmSize FilmSizeId
        {
			get { return GetFilmSizeEnumFromString(base.DicomAttributeProvider[DicomTags.FilmSizeId].GetString(0, String.Empty)); }
			set { IodBase.SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.FilmSizeId], GetFilmSizeStringFromEnum(value), false); }
        }

        /// <summary>
        /// Gets or sets the type of the magnification.Interpolation type by which the printer magnifies or decimates the image in order to fit the image in the
        /// image box on film.
        /// </summary>
        /// <value>The type of the magnification.</value>
        public MagnificationType MagnificationType
        {
            get { return IodBase.ParseEnum<MagnificationType>(base.DicomAttributeProvider[DicomTags.MagnificationType].GetString(0, String.Empty), MagnificationType.None); }
            set { IodBase.SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.MagnificationType], value, false); }
        }

        /// <summary>
        /// Gets or sets the type of the smoothing.  Further specifies the type of the interpolation function. Values are defined in Conformance Statement.
        /// </summary>
        /// <value>The type of the smoothing.</value>
        public SmoothingType SmoothingType
        {
            get { return IodBase.ParseEnum<SmoothingType>(base.DicomAttributeProvider[DicomTags.SmoothingType].GetString(0, String.Empty), SmoothingType.None); }
            set { IodBase.SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.SmoothingType], value, false); }
        }

        /// <summary>
        /// Gets or sets the border density.  Density of the film areas surrounding and between images on the film. Defined Terms: 
        /// <para>BLACK 
        /// </para>
        /// <para>
        /// WHITE 
        /// </para>
        /// <para>
        /// i where i represents the desired density in hundreds of OD
        /// </para>
        /// </summary>
        /// <value>The border density.</value>
        public string BorderDensity
        {
            get { return base.DicomAttributeProvider[DicomTags.BorderDensity].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.BorderDensity].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the empty image density.  Density of the image box area on the film that contains no image. Defined Terms: 
        /// <para>BLACK 
        /// </para>
        /// <para>
        /// WHITE 
        /// </para>
        /// <para>
        /// i where i represents the desired density in hundreds of OD
        /// </para>
        /// </summary>
        /// <value>The empty image density.</value>
        public string EmptyImageDensity
        {
            get { return base.DicomAttributeProvider[DicomTags.EmptyImageDensity].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.EmptyImageDensity].SetString(0, value); }
        }


        /// <summary>
        /// Gets or sets the min density.  Minimum density of the images on the film, expressed in hundredths of OD. If Min Density is lower than minimum printer density than Min Density 
        /// is set to minimum printer density.
        /// </summary>
        /// <value>The min density.</value>
        public ushort MinDensity
        {
            get { return base.DicomAttributeProvider[DicomTags.MinDensity].GetUInt16(0, 0); }
            set { base.DicomAttributeProvider[DicomTags.MinDensity].SetUInt16(0, value); }
        }

        /// <summary>
        /// Gets or sets the min density.  Maximum density of the images on the film, expressed in hundredths of OD. If Max Density higher than maximum printer density than Max 
        /// Density is set to maximum printer density.
        /// </summary>
        /// <value>The min density.</value>
        public ushort MaxDensity
        {
            get { return base.DicomAttributeProvider[DicomTags.MaxDensity].GetUInt16(0, 0); }
            set { base.DicomAttributeProvider[DicomTags.MaxDensity].SetUInt16(0, value); }
        }

        /// <summary>
        /// Gets or sets the trim, YES OR NO.
        /// </summary>
        /// <value>The trim.</value>
        public DicomBoolean Trim
        {
            get { return IodBase.ParseEnum<DicomBoolean>(base.DicomAttributeProvider[DicomTags.Trim].GetString(0, String.Empty), DicomBoolean.None); }
            set { IodBase.SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.Trim], value, false); }
        }

        /// <summary>
        /// Gets or sets the configuration information.
        /// </summary>
        /// <value>The configuration information.</value>
        public string ConfigurationInformation
        {
            get { return base.DicomAttributeProvider[DicomTags.ConfigurationInformation].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.ConfigurationInformation].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the illumination.  Luminance of lightbox illuminating a piece of transmissive film, or for the case of reflective media, luminance obtainable from diffuse reflection of the illumination present. Expressed as L0, in candelas per square meter (cd/m2).
        /// </summary>
        /// <value>The illumination.</value>
        public ushort Illumination
        {
            get { return base.DicomAttributeProvider[DicomTags.Illumination].GetUInt16(0, 0); }
            set { base.DicomAttributeProvider[DicomTags.Illumination].SetUInt16(0, value); }
        }

        /// <summary>
        /// Gets or sets the reflected ambient light.  For transmissive film, luminance contribution due to reflected ambient light. Expressed as La, in candelas per square meter (cd/m2).
        /// </summary>
        /// <value>The reflected ambient light.</value>
        public ushort ReflectedAmbientLight
        {
            get { return base.DicomAttributeProvider[DicomTags.ReflectedAmbientLight].GetUInt16(0, 0); }
            set { base.DicomAttributeProvider[DicomTags.ReflectedAmbientLight].SetUInt16(0, value); }
        }

        /// <summary>
        /// Gets or sets the requested resolution id.  Specifies the resolution at which images in this Film Box are to be printed.
        /// </summary>
        /// <value>The requested resolution id.</value>
        public RequestedResolution RequestedResolutionId
        {
            get { return IodBase.ParseEnum<RequestedResolution>(base.DicomAttributeProvider[DicomTags.RequestedResolutionId].GetString(0, String.Empty), RequestedResolution.None); }
            set { IodBase.SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.RequestedResolutionId], value, false); }
        }

        public SequenceIodList<ReferencedInstanceSequenceIod> ReferencedFilmSessionSequenceList
        {
            get
            {
                return new SequenceIodList<ReferencedInstanceSequenceIod>(base.DicomAttributeProvider[DicomTags.ReferencedFilmSessionSequence] as DicomAttributeSQ);
            }
        }

        public SequenceIodList<ReferencedInstanceSequenceIod> ReferencedImageBoxSequenceList
        {
            get
            {
                return new SequenceIodList<ReferencedInstanceSequenceIod>(base.DicomAttributeProvider[DicomTags.ReferencedImageBoxSequence] as DicomAttributeSQ);
            }
        }

        public SequenceIodList<ReferencedInstanceSequenceIod> ReferencedBasicAnnotationBoxSequenceList
        {
            get
            {
                return new SequenceIodList<ReferencedInstanceSequenceIod>(base.DicomAttributeProvider[DicomTags.ReferencedBasicAnnotationBoxSequence] as DicomAttributeSQ);
            }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the commonly used tags in the base dicom attribute collection.
        /// </summary>
        public void SetCommonTags()
        {
            SetCommonTags(base.DicomAttributeProvider);
        }
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Sets the commonly used tags in the specified dicom attribute collection.
        /// </summary>
        public static void SetCommonTags(IDicomAttributeProvider dicomAttributeProvider)
        {
            if (dicomAttributeProvider == null)
				throw new ArgumentNullException("dicomAttributeProvider");

            //dicomAttributeProvider[DicomTags.NumberOfCopies].SetNullValue();
            //dicomAttributeProvider[DicomTags.PrintPriority].SetNullValue();
            //dicomAttributeProvider[DicomTags.MediumType].SetNullValue();
            //dicomAttributeProvider[DicomTags.FilmDestination].SetNullValue();
            //dicomAttributeProvider[DicomTags.FilmSessionLabel].SetNullValue();
            //dicomAttributeProvider[DicomTags.MemoryAllocation].SetNullValue();
            //dicomAttributeProvider[DicomTags.OwnerId].SetNullValue();
        }

        /// <summary>
        /// Gets the film size string from enum.  Note, have to do this because can't make enum values that start with a number...
        /// </summary>
        /// <param name="filmSizeId">The film size id.</param>
        /// <returns></returns>
        public static string GetFilmSizeStringFromEnum(FilmSize filmSizeId)
        {
            string result = String.Empty;
            if (filmSizeId != FilmSize.None)
            {
                if (filmSizeId == FilmSize.A3 || filmSizeId == FilmSize.A4)
                    result = filmSizeId.ToString();
                else if (filmSizeId.ToString().Length > 2)
                {
                    string filmSizeString = filmSizeId.ToString();
                    // Format is INYxZ or CMYxZ - where Y is first dimension and Z is second dimension - ie, IN10x14 - need to turn it into 10INX14IN
                    string inOrCm = filmSizeString.Substring(0, 2);
                    string[] numbers = filmSizeString.Substring(2).Split("x".ToCharArray());
                    if (numbers.Length == 2)
                        result = String.Format(CultureInfo.InvariantCulture, "{0}{1}X{2}{1}", numbers[0], inOrCm, numbers[1]);
                    else
                    {
                        // TODO: put warning somewhere...
                        result = String.Empty;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the film size enum from string.  Note, have to do this because can't make enum values that start with a number...
        /// </summary>
        /// <param name="filmSizeString">The film size string.</param>
        /// <returns></returns>
        public static FilmSize GetFilmSizeEnumFromString(string filmSizeString)
        {
            FilmSize result = FilmSize.None;
            if (!String.IsNullOrEmpty(filmSizeString) && String.Compare(filmSizeString, FilmSize.None.ToString(), StringComparison.OrdinalIgnoreCase) != 0)
            {
                string stringToParse = String.Empty;
                if (String.Compare(filmSizeString, FilmSize.A3.ToString(), StringComparison.OrdinalIgnoreCase) == 0 || String.Compare(filmSizeString, FilmSize.A4.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
                {
                    stringToParse = filmSizeString;
                }
                else if (filmSizeString.ToString().Length > 3)
                {
                    // Format is YINXZIN or YCMXZCM - where Y is first dimension and Z is second dimension - ie, 10INX14IN - need to turn it into IN10x14 (enum format)
                    string inOrCm = filmSizeString.Substring(filmSizeString.Length - 2, 2);
                    string[] numbers = filmSizeString.Split("X".ToCharArray());
                    if (numbers.Length == 2)
                    {
                        if (numbers[0].Length > 2)
                        {
                            // remove CM or IN from end of each number
                            for (int i = 0; i < 2; i++)
                            {
                                numbers[i] = numbers[i].Substring(0, numbers[i].Length - 2).Trim();
                            }
                            stringToParse = String.Format(CultureInfo.InvariantCulture, "{0}{1}x{2}", inOrCm, numbers[0], numbers[1]);
                        }
                    }
                }
                if (!String.IsNullOrEmpty(stringToParse))
                {
                    try
                    {
                        result = (FilmSize) Enum.Parse(typeof(FilmSize), stringToParse);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return result;
        }

        #endregion
    }

    #region FilmOrientation Enum
    /// <summary>
    /// enumeration for the Film Orientation
    /// </summary>
    public enum FilmOrientation
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// vertical film position
        /// </summary>
        Portrait,
        /// <summary>
        /// horizontal film position
        /// </summary>
        Landscape
    }
    #endregion

    #region FilmSizeId Enum
    /// <summary>
    /// Enumeration for Film size identification.
    /// </summary>
    public enum FilmSize
    {
        None,
        /// <summary>
        /// 8INX10IN
        /// </summary>
        IN8x10,
        /// <summary>
        /// 8_5INX11IN
        /// </summary>
        IN8_5x11,

        /// <summary>
        /// 10INX12IN
        /// </summary>
        IN10x12,
        /// <summary>
        /// 10INX14IN, corresponds with 25.7CMX36.4CM
        /// </summary>
        IN10x14,
        /// <summary>
        /// 11INX14IN
        /// </summary>
        IN11x14,
        /// <summary>
        /// 11INX17IN
        /// </summary>
        IN11x17,
        /// <summary>
        /// 14INX14IN
        /// </summary>
        IN14x14,
        /// <summary>
        /// 14INX17IN
        /// </summary>
        IN14x17,
        /// <summary>
        /// 24CMX24CM
        /// </summary>
        CM24x24,
        /// <summary>
        /// 24CMX30CM
        /// </summary>
        CM24x30,
        /// <summary>
        /// A4 corresponds with 210 x 297 millimeters
        /// </summary>
        A4,
        /// <summary>
        /// A3 corresponds with 297 x 420 millimeters
        /// </summary>
        A3
    }
    #endregion

    #region MagnificationType Enum
    /// <summary>
    /// Magnification type enum.  Interpolation type by which the printer magnifies or decimates the image in order to fit the image in the
    /// image box on film.
    /// </summary>
    public enum MagnificationType
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 
        /// </summary>
        Replicate,
        /// <summary>
        /// 
        /// </summary>
        Bilinear,
        /// <summary>
        /// 
        /// </summary>
        Cubic
    }
    #endregion

    #region SmoothingType Enum
    /// <summary>
    /// Further specifies the type of the interpolation function. Values are defined in Conformance Statement.
    /// </summary>
    public enum SmoothingType
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// Only valid for Magnification Type
        /// </summary>
        Cubic
    }
    #endregion

    #region DicomBoolean Enum
    public enum DicomBoolean
    {
        None,
        Yes,
        No
    }
    #endregion

    #region RequestedResolution Enum
    /// <summary>
    /// Specifies the resolution at which images in this Film Box are to be printed.
    /// </summary>
    public enum RequestedResolution
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// approximately 4k x 5k printable pixels on a 14 x 17 inch film
        /// </summary>
        Standard,
        /// <summary>
        /// Approximately twice the resolution of STANDARD.
        /// </summary>
        High
    }
    #endregion
    
}
