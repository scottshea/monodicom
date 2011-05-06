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
using ClearCanvas.Dicom.Iod.Sequences;

namespace ClearCanvas.Dicom.Iod.Modules
{
    /// <summary>
    /// Basic Film Session Presentation and Relationship Module as per Part 3, C.13.1 (pg 863) and C.13.2 (pg 863)
    /// </summary>
    public class BasicFilmSessionModuleIod : IodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FilmSessionModuleIod"/> class.
        /// </summary>
        public BasicFilmSessionModuleIod()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilmSessionModuleIod"/> class.
        /// </summary>
		public BasicFilmSessionModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider)
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Number of copies to be printed for each film of the film session.
        /// </summary>
        /// <value>The number of copies.</value>
        public int NumberOfCopies
        {
            get { return DicomAttributeProvider[DicomTags.NumberOfCopies].GetInt32(0, 0); }
            set { DicomAttributeProvider[DicomTags.NumberOfCopies].SetInt32(0, value); }
        }


        /// <summary>
        /// Gets or sets the print priority.
        /// </summary>
        /// <value>The print priority.</value>
        public PrintPriority PrintPriority
        {
            get { return IodBase.ParseEnum<PrintPriority>(DicomAttributeProvider[DicomTags.PrintPriority].GetString(0, String.Empty), PrintPriority.None); }
            set { IodBase.SetAttributeFromEnum(DicomAttributeProvider[DicomTags.PrintPriority], value, false); }
        }

        /// <summary>
        /// Type of medium on which the print job will be printed.
        /// </summary>
        /// <value>The type of the medium.</value>
        public MediumType MediumType
        {
            get { return ParseEnum<MediumType>(DicomAttributeProvider[DicomTags.MediumType].GetString(0, String.Empty), MediumType.None); }
            set { SetAttributeFromEnum(DicomAttributeProvider[DicomTags.MediumType], value, true); }
        }

        /// <summary>
        /// Gets or sets the film destination.
        /// </summary>
        /// <value>The film destination.</value>
        public FilmDestination FilmDestination
        {
            get { return ParseEnum<FilmDestination>(base.DicomAttributeProvider[DicomTags.FilmDestination].GetString(0, String.Empty), FilmDestination.None); }
            set { SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.FilmDestination], value, false); }
        }

        /// <summary>
        /// Gets or sets the film destination string (in case you want to set it to BIN_i 
        /// </summary>
        /// <value>The film destination string.</value>
        public string FilmDestinationString
        {
            get { return base.DicomAttributeProvider[DicomTags.FilmDestination].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.FilmDestination].SetString(0, value); }
        }

        /// <summary>
        /// Human readable label that identifies the film session
        /// </summary>
        /// <value>The film session label.</value>
        public string FilmSessionLabel
        {
            get { return base.DicomAttributeProvider[DicomTags.FilmSessionLabel].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.FilmSessionLabel].SetString(0, value); }
        }

        /// <summary>
        /// Amount of memory allocated for the film session. Value is expressed in KB.
        /// </summary>
        /// <value>The memory allocation.</value>
        public int MemoryAllocation
        {
            get { return base.DicomAttributeProvider[DicomTags.MemoryAllocation].GetInt32(0, 0); }
            set { base.DicomAttributeProvider[DicomTags.MemoryAllocation].SetInt32(0, value); }
        }

        /// <summary>
        /// Identification of the owner of the film session
        /// </summary>
        /// <value>The owner id.</value>
        public string OwnerId
        {
            get { return base.DicomAttributeProvider[DicomTags.OwnerId].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.OwnerId].SetString(0, value); }
        }

        /// <summary>
        /// A Sequence which provides references to a set of Film Box SOP Class/Instance pairs. Zero or more Items may be included in this Sequence.
        /// </summary>
        /// <value>The referenced film box sequence list.</value>
        public SequenceIodList<ReferencedInstanceSequenceIod> ReferencedFilmBoxSequenceList
        {
            get
            {
                return new SequenceIodList<ReferencedInstanceSequenceIod>(base.DicomAttributeProvider[DicomTags.ReferencedFilmBoxSequence] as DicomAttributeSQ);
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

            dicomAttributeProvider[DicomTags.NumberOfCopies].SetNullValue();
            dicomAttributeProvider[DicomTags.PrintPriority].SetNullValue();
            dicomAttributeProvider[DicomTags.MediumType].SetNullValue();
            dicomAttributeProvider[DicomTags.FilmDestination].SetNullValue();
            dicomAttributeProvider[DicomTags.FilmSessionLabel].SetNullValue();
            dicomAttributeProvider[DicomTags.MemoryAllocation].SetNullValue();
            dicomAttributeProvider[DicomTags.OwnerId].SetNullValue();
        }
        #endregion
    }

    #region Print Priority Enum
    /// <summary>
    /// enumeration for Print Priority
    /// </summary>
    public enum PrintPriority
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 
        /// </summary>
        High,
        /// <summary>
        /// 
        /// </summary>
        Med,
        /// <summary>
        /// 
        /// </summary>
        Low
    }
    #endregion

    #region MediumType Enum
    /// <summary>
    /// Enumeration for Medium Type (Print Film Session Module) as per Part 3, C.13.1 (2000,0030)
    /// </summary>
    public enum MediumType
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 
        /// </summary>
        Paper,
        /// <summary>
        /// 
        /// </summary>
        ClearFilm,
        /// <summary>
        /// 
        /// </summary>
        BlueFilm,
        /// <summary>
        /// 
        /// </summary>
        MammoClearFilm,
        /// <summary>
        /// 
        /// </summary>
        MammoBlueFilm
    }
    #endregion

    #region FilmDestination Enum
    /// <summary>
    /// Enumeration for Film Destination
    /// </summary>
    public enum FilmDestination
    {
        /// <summary>
        /// None - note, it could also be BIN_i
        /// </summary>
        None,
        /// <summary>
        /// the exposed film is stored in film magazine
        /// </summary>
        Magazine,
        /// <summary>
        /// the exposed film is developed in film processor
        /// </summary>
        Processor

    }
    #endregion
}
