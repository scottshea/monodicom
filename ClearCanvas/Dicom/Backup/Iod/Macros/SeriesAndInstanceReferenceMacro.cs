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

using ClearCanvas.Dicom.Iod.Sequences;

namespace ClearCanvas.Dicom.Iod.Macros
{
    /// <summary>
    /// Series and Instance Reference Macro Attributes
    /// </summary>
    /// <remarks>As per Dicom Doc 3, Table 10.4 (pg 78)</remarks>
    public class SeriesAndInstanceReferenceMacro : SequenceIodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesAndInstanceReferenceMacro"/> class.
        /// </summary>
        public SeriesAndInstanceReferenceMacro()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesAndInstanceReferenceMacro"/> class.
        /// </summary>
        /// <param name="dicomSequenceItem">The dicom sequence item.</param>
        public SeriesAndInstanceReferenceMacro(DicomSequenceItem dicomSequenceItem)
            : base(dicomSequenceItem)
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Sequence of Items each of which includes the Attributes of one Series. 
        /// One or more Items shall be present. (0008,1115)
        /// </summary>
        /// <value>The referenced series sequence list.</value>
        public SequenceIodList<ReferencedSeriesSequenceIod> ReferencedSeriesSequenceList
        {
            get
            {
                return new SequenceIodList<ReferencedSeriesSequenceIod>(base.DicomAttributeProvider[DicomTags.ReferencedSeriesSequence] as DicomAttributeSQ);
            }
        } 
        #endregion

    }
}
