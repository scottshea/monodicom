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

using ClearCanvas.Dicom.Iod.Macros;
using ClearCanvas.Dicom.Iod.Sequences;

namespace ClearCanvas.Dicom.Iod.Modules
{
    /// <summary>
    /// As per Dicom DOC 3 Table C.4-17
    /// </summary>
    public class BillingAndMaterialManagementCodesModuleIod : IodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BillingAndMaterialManagementCodesModuleIod"/> class.
        /// </summary>
        public BillingAndMaterialManagementCodesModuleIod()
            :base()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="BillingAndMaterialManagementCodesModuleIod"/> class.
        /// </summary>
		public BillingAndMaterialManagementCodesModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider)
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Contains billing codes for the Procedure Type performed within the Procedure Step. The sequence may have zero or more Items.
        /// </summary>
        /// <value>The billing procedure step sequence list.</value>
        public SequenceIodList<CodeSequenceMacro> BillingProcedureStepSequenceList
        {
            get
            {
                return new SequenceIodList<CodeSequenceMacro>(base.DicomAttributeProvider[DicomTags.BillingProcedureStepSequence] as DicomAttributeSQ);
            }
        }

        /// <summary>
        /// Information about the film consumption for this Performed Procedure Step. The sequence may have zero or more Items.
        /// </summary>
        /// <value>The film consumption sequence list.</value>
        public SequenceIodList<FilmConsumptionSequenceIod> FilmConsumptionSequenceList
        {
            get
            {
                return new SequenceIodList<FilmConsumptionSequenceIod>(base.DicomAttributeProvider[DicomTags.FilmConsumptionSequence] as DicomAttributeSQ);
            }
        }

        /// <summary>
        /// Chemicals, supplies and devices for billing used in the Performed Procedure Step. The sequence may have one or more Items.
        /// </summary>
        /// <value>The billing supplies and devices sequence list.</value>
        public SequenceIodList<BillingSuppliesAndDevicesSequenceIod> BillingSuppliesAndDevicesSequenceList
        {
            get
            {
                return new SequenceIodList<BillingSuppliesAndDevicesSequenceIod>(base.DicomAttributeProvider[DicomTags.BillingSuppliesAndDevicesSequence] as DicomAttributeSQ);
            }
        }
        
        

        #endregion

    }
}
