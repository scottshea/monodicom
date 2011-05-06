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
using ClearCanvas.Dicom.Iod.Macros;

namespace ClearCanvas.Dicom.Iod.Sequences
{
    /// <summary>
    /// Performed Series Sequence.  
    /// </summary>
    /// <remarks>As per Part 3, Table C4.15, pg 256</remarks>
    public class PerformedSeriesSequenceIod : SequenceIodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PerformedSeriesSequenceIod"/> class.
        /// </summary>
        public PerformedSeriesSequenceIod()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformedSeriesSequenceIod"/> class.
        /// </summary>
        /// <param name="dicomSequenceItem">The dicom sequence item.</param>
        public PerformedSeriesSequenceIod(DicomSequenceItem dicomSequenceItem)
            : base(dicomSequenceItem)
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Name of the physician(s) administering this Series.
        /// </summary>
        /// <value>The name of the performing physicians.</value>
        public PersonName PerformingPhysiciansName
        {
            get { return new PersonName(base.DicomAttributeProvider[DicomTags.PerformingPhysiciansName].GetString(0, String.Empty)); }
            set { base.DicomAttributeProvider[DicomTags.PerformingPhysiciansName].SetString(0, value.ToString()); }
        }

        /// <summary>
        /// Identification of the physician(s) administering the Series. One or more items 
        /// shall be included in this sequence. If more than one Item, the number and
        /// order shall correspond to the value of Performing Physician�s Name (0008,1050), if present.
        /// </summary>
        /// <value>The performing physician identification sequence list.</value>
        public SequenceIodList<PersonIdentificationMacro> PerformingPhysicianIdentificationSequenceList
        {
            get
            {
                return new SequenceIodList<PersonIdentificationMacro>(base.DicomAttributeProvider[DicomTags.PerformingPhysicianIdentificationSequence] as DicomAttributeSQ);
            }
        }

        /// <summary>
        /// Gets or sets the name of the operators.
        /// </summary>
        /// <value>The name of the operators.</value>
        public PersonName OperatorsName
        {
            get { return new PersonName(base.DicomAttributeProvider[DicomTags.OperatorsName].GetString(0, String.Empty)); }
            set { base.DicomAttributeProvider[DicomTags.OperatorsName].SetString(0, value.ToString()); }
        }

        /// <summary>
        /// Identification of the operator(s) supporting the Series. One or more items shall be 
        /// included in this sequence. If more than one Item, the number and
        /// order shall correspond to the value of Operators� Name (0008,1070), if present.
        /// </summary>
        /// <value>The operator identification sequence list.</value>
        public SequenceIodList<PersonIdentificationMacro> OperatorIdentificationSequenceList
        {
            get
            {
                return new SequenceIodList<PersonIdentificationMacro>(base.DicomAttributeProvider[DicomTags.OperatorIdentificationSequence] as DicomAttributeSQ);
            }
        }

        /// <summary>
        /// User-defined description of the conditions under which the Series was performed. 
        /// <para>Note: This attribute conveys series-specific protocol identification and may or may not be identical 
        /// to the one presented in the Performed Protocol Code Sequence (0040,0260).</para>
        /// </summary>
        /// <value>The name of the protocol.</value>
        public string ProtocolName
        {
            get { return base.DicomAttributeProvider[DicomTags.ProtocolName].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.ProtocolName].SetString(0, value); }
        }

        public string SeriesInstanceUid
        {
            get { return base.DicomAttributeProvider[DicomTags.SeriesInstanceUid].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.SeriesInstanceUid].SetString(0, value); }
        }

        public string SeriesDescription
        {
            get { return base.DicomAttributeProvider[DicomTags.SeriesDescription].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.SeriesDescription].SetString(0, value); }
        }

        /// <summary>
        /// Title of the DICOM Application Entity where the Images and other Composite SOP 
        /// Instances in this Series may be retrieved on the network.
        /// <para>Note: The duration for which this location remains valid is unspecified.</para>
        /// </summary>
        /// <value>The retrieve ae title.</value>
        public string RetrieveAeTitle
        {
            get { return base.DicomAttributeProvider[DicomTags.RetrieveAeTitle].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.RetrieveAeTitle].SetString(0, value); }
        }

        /// <summary>
        /// A Sequence that provides reference to one or more sets of Image SOP Class/SOP 
        /// Instance pairs created during the acquisition of the procedure step.
        /// The sequence may have zero or more Items.
        /// </summary>
        /// <value>The referenced image sequence list.</value>
        public SequenceIodList<ReferencedInstanceSequenceIod> ReferencedImageSequenceList
        {
            get
            {
                return new SequenceIodList<ReferencedInstanceSequenceIod>(base.DicomAttributeProvider[DicomTags.ReferencedImageSequence] as DicomAttributeSQ);
            }
        }

        /// <summary>
        /// Uniquely identifies instances, other than images, of any SOP Class that conforms to the DICOM 
        /// Composite IOD Information Model, such as Waveforms, Presentation States or Structured 
        /// Reports, created during the acquisition of the procedure step. The sequence may have zero or
        /// more Items.
        /// </summary>
        /// <value>The referenced non image composite sop instance sequence list.</value>
        public SequenceIodList<ReferencedInstanceSequenceIod> ReferencedNonImageCompositeSopInstanceSequenceList
        {
            get
            {
                return new SequenceIodList<ReferencedInstanceSequenceIod>(base.DicomAttributeProvider[DicomTags.ReferencedNonImageCompositeSopInstanceSequence] as DicomAttributeSQ);
            }
        }
        
       #endregion
    }

}
