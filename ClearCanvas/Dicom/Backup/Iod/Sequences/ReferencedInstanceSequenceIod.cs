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
    /// Referenced Sop Class and Instance Sequence, consisting of Referenced SOP Class UID (0008,1150)
    /// and Referenced SOP Instance UID (0008,1155), and optionally .
    /// <para>This is mainly for the different sequences in the the Basic Film Box Relationship
    /// Module (Part 3, Table C 13.4, pg 867) such as Referenced Film Session Sequence, Referenced Image Box Sequence, Referenced Basic Annotation Box Sequence,
    /// etc., but there may be other uses for it.</para>
    /// </summary>
    /// <remarks>As per Part 3, Table C 13.4, pg 867</remarks>
    public class ReferencedInstanceSequenceIod : SequenceIodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferencedInstanceSequenceIod"/> class.
        /// </summary>
        public ReferencedInstanceSequenceIod()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferencedInstanceSequenceIod"/> class.
        /// </summary>
        /// <param name="dicomSequenceItem">The dicom sequence item.</param>
        public ReferencedInstanceSequenceIod(DicomSequenceItem dicomSequenceItem)
            : base(dicomSequenceItem)
        {
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Uniquely identifies the referenced SOP Class. (0008,1150)
        /// </summary>
        /// <value>The referenced sop class uid.</value>
        public string ReferencedSopClassUid
        {
            get { return base.DicomAttributeProvider[DicomTags.ReferencedSopClassUid].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.ReferencedSopClassUid].SetString(0, value); }
        }

        /// <summary>
        /// Uniquely identifies the referenced SOP Instance. (0008,1155)
        /// </summary>
        /// <value>The referenced sop instance uid.</value>
        public string ReferencedSopInstanceUid
        {
            get { return base.DicomAttributeProvider[DicomTags.ReferencedSopInstanceUid].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.ReferencedSopInstanceUid].SetString(0, value); }
        }
        
       #endregion
    }

}
