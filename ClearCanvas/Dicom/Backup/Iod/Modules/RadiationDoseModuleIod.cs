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
using ClearCanvas.Dicom.Iod.Sequences;

namespace ClearCanvas.Dicom.Iod.Modules
{
    /// <summary>
    /// As per Dicom DOC 3 Table C.4-16
    /// </summary>
    public class RadiationDoseModuleIod : IodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RadiationDoseModuleIod"/> class.
        /// </summary>
        public RadiationDoseModuleIod()
            :base()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="RadiationDoseModuleIod"/> class.
        /// </summary>
		public RadiationDoseModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider)
        {
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Anatomic structure, space or region that has been exposed to ionizing radiation. 
        /// The sequence may have zero or one Items.
        /// </summary>
        /// <value>The anatomic structure space or region sequence list.</value>
        public SequenceIodList<CodeSequenceMacro> AnatomicStructureSpaceOrRegionSequenceList
        {
            get
            {
                return new SequenceIodList<CodeSequenceMacro>(base.DicomAttributeProvider[DicomTags.AnatomicStructureSpaceOrRegionSequence] as DicomAttributeSQ);
            }
        }

        /// <summary>
        /// Total duration of X-Ray exposure during fluoroscopy in seconds (pedal time) during this Performed Procedure Step.
        /// </summary>
        /// <value>The total time of fluoroscopy.</value>
        public ushort TotalTimeOfFluoroscopy
        {
            get { return base.DicomAttributeProvider[DicomTags.TotalTimeOfFluoroscopy].GetUInt16(0, 0); }
            set { base.DicomAttributeProvider[DicomTags.TotalTimeOfFluoroscopy].SetUInt16(0, value); }
        }

        /// <summary>
        /// Total number of exposures made during this Performed Procedure Step. 
        /// The number includes non-digital and digital exposures.
        /// </summary>
        /// <value>The total number of exposures.</value>
        public ushort TotalNumberOfExposures
        {
            get { return base.DicomAttributeProvider[DicomTags.TotalNumberOfExposures].GetUInt16(0, 0); }
            set { base.DicomAttributeProvider[DicomTags.TotalNumberOfExposures].SetUInt16(0, value); }
        }

        /// <summary>
        /// Distance in mm from the source to detector center. 
        /// <para>Note: This value is traditionally referred to as Source Image Receptor Distance (SID).</para>
        /// </summary>
        /// <value>The distance source to detector.</value>
        public float DistanceSourceToDetector
        {
            get { return base.DicomAttributeProvider[DicomTags.DistanceSourceToDetector].GetFloat32(0, 0.0F); }
            set { base.DicomAttributeProvider[DicomTags.DistanceSourceToDetector].SetFloat32(0, value); }
        }

        /// <summary>
        /// Distance in mm from the source to the surface of the patient closest to the source during this Performed Procedure Step.
        /// Note: This may be an estimated value based on assumptions about the patient�s body size and habitus.
        /// </summary>
        /// <value>The distance source to entrance.</value>
        public float DistanceSourceToEntrance
        {
            get { return base.DicomAttributeProvider[DicomTags.DistanceSourceToEntrance].GetFloat32(0, 0.0F); }
            set { base.DicomAttributeProvider[DicomTags.DistanceSourceToEntrance].SetFloat32(0, value); }
        }

        /// <summary>
        /// Average entrance dose value measured in dGy at the surface of the patient during this Performed Procedure Step.
        /// Note: This may be an estimated value based on assumptions about the patient�s body size and habitus.
        /// </summary>
        /// <value>The entrance dose.</value>
        public ushort EntranceDose
        {
            get { return base.DicomAttributeProvider[DicomTags.EntranceDose].GetUInt16(0, 0); }
            set { base.DicomAttributeProvider[DicomTags.EntranceDose].SetUInt16(0, value); }
        }

        /// <summary>
        /// Average entrance dose value measured in mGy at the surface of the patient during this Performed Procedure Step.
        /// Note: This may be an estimated value based on assumptions about the patient�s body size and habitus.
        /// </summary>
        /// <value>The entrance dose in mgy.</value>
        public float EntranceDoseInMgy
        {
            get { return base.DicomAttributeProvider[DicomTags.EntranceDoseInMgy].GetFloat32(0, 0.0F); }
            set { base.DicomAttributeProvider[DicomTags.EntranceDoseInMgy].SetFloat32(0, value); }
        }

        /// <summary>
        /// Typical dimension of the exposed area at the detector plane. If Rectangular: ExposeArea1 is row dimension followed by column (ExposeArea2); if Round: ExposeArea1 is diameter. Measured in mm.
        /// </summary>
        /// <value>The exposed area1.</value>
        public float ExposedArea1
        {
            get { return base.DicomAttributeProvider[DicomTags.ExposedArea].GetFloat32(0, 0.0F); }
            set { base.DicomAttributeProvider[DicomTags.ExposedArea].SetFloat32(0, value); }
        }

        /// <summary>
        /// Typical dimension of the exposed area at the detector plane. If Rectangular: ExposeArea2 is column dimension (ExposeArea1 is column); if Round: ExposeArea2 is Null...
        /// </summary>
        /// <value>The exposed area2.</value>
        public float ExposedArea2
        {
            get { return base.DicomAttributeProvider[DicomTags.ExposedArea].GetFloat32(1, 0.0F); }
            set { base.DicomAttributeProvider[DicomTags.ExposedArea].SetFloat32(1, value); }
        }

        /// <summary>
        /// Total area-dose-product to which the patient was exposed, accumulated over the complete Performed
        /// Procedure Step and measured in dGy*cm*cm, including fluoroscopy.
        /// <para>Notes: 1. The sum of the area dose product of all images of a Series or a Study may not result in
        /// the total area dose product to which the patient was exposed.</para>
        /// <para>2. This may be an estimated value based on assumptions about the patient�s body size and habitus.</para>
        /// </summary>
        /// <value>The image and fluoroscopy area dose product.</value>
        public float ImageAndFluoroscopyAreaDoseProduct
        {
            get { return base.DicomAttributeProvider[DicomTags.ImageAndFluoroscopyAreaDoseProduct].GetFloat32(0, 0.0F); }
            set { base.DicomAttributeProvider[DicomTags.ImageAndFluoroscopyAreaDoseProduct].SetFloat32(0, value); }
        }

        /// <summary>
        /// User-defined comments on any special conditions related to radiation dose encountered during this Performed Procedure Step.
        /// </summary>
        /// <value>The comments on radiation dose.</value>
        public string CommentsOnRadiationDose
        {
            get { return base.DicomAttributeProvider[DicomTags.CommentsOnRadiationDose].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.CommentsOnRadiationDose].SetString(0, value); }
        }

        /// <summary>
        /// Exposure Dose Sequence will contain Total Number of Exposures (0040,0301) items plus an item for
        /// each fluoroscopy episode not already counted as an exposure.
        /// </summary>
        /// <value>The exposure dose sequence list.</value>
        public SequenceIodList<ExposureDoseSequenceIod> ExposureDoseSequenceList
        {
            get
            {
                return new SequenceIodList<ExposureDoseSequenceIod>(base.DicomAttributeProvider[DicomTags.ExposureDoseSequence] as DicomAttributeSQ);
            }
        }
        
        
        #endregion

    }
}
