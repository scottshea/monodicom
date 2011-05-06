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

namespace ClearCanvas.Dicom.Iod.Macros
{
    /// <summary>
    /// Person Identification Macro
    /// </summary>
    /// <remarks>As per Dicom Doc 3, Table 10-1 (pg 75)</remarks>
    public class PersonIdentificationMacro : SequenceIodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonIdentificationMacro"/> class.
        /// </summary>
        public PersonIdentificationMacro()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonIdentificationMacro"/> class.
        /// </summary>
        /// <param name="dicomSequenceItem">The dicom sequence item.</param>
        public PersonIdentificationMacro(DicomSequenceItem dicomSequenceItem)
            : base(dicomSequenceItem)
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// A coded entry which identifies a person.
        /// <para>The Code Meaning attribute, though it will be encoded with a VR of LO, 
        /// may be encoded according to the rules of the PN VR (e.g. caret �^� delimiters 
        /// shall separate name components), except that a single component (i.e. the
        /// whole name unseparated by caret delimiters) is not permitted. Name component 
        /// groups for use with multi-byte character sets are permitted, as long as they fit 
        /// within the 64 characters (the length of the LO VR).</para>
        /// <para>One or more Items may be permitted in this Sequence.</para>
        /// </summary>
        /// <value>The person identification code sequence list.</value>
        public SequenceIodList<CodeSequenceMacro> PersonIdentificationCodeSequenceList
        {
            get
            {
                return new SequenceIodList<CodeSequenceMacro>(base.DicomAttributeProvider[DicomTags.PersonIdentificationCodeSequence] as DicomAttributeSQ);
            }
        }

        /// <summary>
        /// Gets or sets the persons address.
        /// </summary>
        /// <value>The persons address.</value>
        public string PersonsAddress
        {
            get { return base.DicomAttributeProvider[DicomTags.PersonsAddress].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PersonsAddress].SetString(0, value); }
        }

        /// <summary>
        /// Person's telephone number(s).  TODO: be able to specify list...
        /// </summary>
        /// <value>The persons telephone numbers.</value>
        public string PersonsTelephoneNumbers
        {
            get { return base.DicomAttributeProvider[DicomTags.PersonsTelephoneNumbers].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PersonsTelephoneNumbers].SetString(0, value); }
        }

        /// <summary>
        /// Institution or organization to which the identified individual is
        /// responsible or accountable. Shall not be present if Institution Code Sequence (0008,0082) is present.
        /// </summary>
        /// <value>The name of the institution.</value>
        public string InstitutionName
        {
            get { return base.DicomAttributeProvider[DicomTags.InstitutionName].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.InstitutionName].SetString(0, value); }
        }

        /// <summary>
        /// Mailing address of the institution or organization to which 
        /// the identified individual is responsible or accountable.
        /// </summary>
        /// <value>The institution address.</value>
        public string InstitutionAddress
        {
            get { return base.DicomAttributeProvider[DicomTags.InstitutionAddress].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.InstitutionAddress].SetString(0, value); }
        }

        /// <summary>
        /// Institution or organization to which the identified individual is responsible or 
        /// accountable. Shall not be present if Institution Name (0008,0080) is present.
        /// <para>Only a single Item shall be permitted in this Sequence.</para>
        /// </summary>
        /// <value>The institution code sequence list.</value>
        public SequenceIodList<CodeSequenceMacro> InstitutionCodeSequenceList
        {
            get
            {
                return new SequenceIodList<CodeSequenceMacro>(base.DicomAttributeProvider[DicomTags.InstitutionCodeSequence] as DicomAttributeSQ);
            }
        }

        
        #endregion

    }

}
