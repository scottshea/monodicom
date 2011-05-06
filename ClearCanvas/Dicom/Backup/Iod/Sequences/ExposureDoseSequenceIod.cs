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
    /// Exposure Dose Sequence.  
    /// </summary>
    /// <remarks>As per Part 3, Table C4.16, pg 259</remarks>
    public class ExposureDoseSequenceIod : SequenceIodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ExposureDoseSequenceIod"/> class.
        /// </summary>
        public ExposureDoseSequenceIod()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExposureDoseSequenceIod"/> class.
        /// </summary>
        /// <param name="dicomSequenceItem">The dicom sequence item.</param>
        public ExposureDoseSequenceIod(DicomSequenceItem dicomSequenceItem)
            : base(dicomSequenceItem)
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the x-ray radiation mode.
        /// </summary>
        /// <value>The radiation mode.</value>
        public RadiationMode RadiationMode
        {
            get { return IodBase.ParseEnum<RadiationMode>(base.DicomAttributeProvider[DicomTags.RadiationMode].GetString(0, String.Empty), RadiationMode.None); }
            set { IodBase.SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.RadiationMode], value); }
        }

        /// <summary>
        /// Peak kilo voltage output of the x-ray generator used. An average in the case of fluoroscopy (continuous radiation mode).
        /// </summary>
        /// <value>The KVP.</value>
        public float Kvp
        {
            get { return base.DicomAttributeProvider[DicomTags.Kvp].GetFloat32(0, 0.0F); }
            set { base.DicomAttributeProvider[DicomTags.Kvp].SetFloat32(0, value); }
        }

        /// <summary>
        /// X-ray Tube Current in uA. An average in the case of fluoroscopy (continuous radiation mode).
        /// </summary>
        /// <value>The X ray tube current in A.</value>
        public float XRayTubeCurrentInA
        {
            get { return base.DicomAttributeProvider[DicomTags.XRayTubeCurrentInA].GetFloat32(0, 0.0F); }
            set { base.DicomAttributeProvider[DicomTags.XRayTubeCurrentInA].SetFloat32(0, value); }
        }

        /// <summary>
        /// Time of x-ray exposure or fluoroscopy in msec.
        /// </summary>
        /// <value>The exposure time.</value>
        public DateTime? ExposureTime
        {
        	get { return base.DicomAttributeProvider[DicomTags.ExposureTime].GetDateTime(0);  }
            set { base.DicomAttributeProvider[DicomTags.ExposureTime].SetDateTime(0, value); }
        }

        /// <summary>
        /// Type of filter(s) inserted into the X-Ray beam (e.g. wedges). See C.8.7.10 and C.8.15.3.9 (for enhanced CT) for Defined Terms.
        /// </summary>
        /// <value>The type of the filter.</value>
        public string FilterType
        {
            get { return base.DicomAttributeProvider[DicomTags.FilterType].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.FilterType].SetString(0, value); }
        }

        /// <summary>
        /// The X-Ray absorbing material used in the filter. May be multi-valued. See C.8.7.10 and C.8.15.3.9 (for enhanced CT) for Defined Terms.
        /// </summary>
        /// <value>The filter material.</value>
        public string FilterMaterial
        {
            get { return base.DicomAttributeProvider[DicomTags.FilterMaterial].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.FilterMaterial].SetString(0, value); }
        }

        /// <summary>
        /// User-defined comments on any special conditions related to radiation dose encountered during during
        /// the episode described by this Exposure Dose Sequence Item.
        /// </summary>
        /// <value>The comments on radiation dose.</value>
        public string CommentsOnRadiationDose
        {
            get { return base.DicomAttributeProvider[DicomTags.CommentsOnRadiationDose].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.CommentsOnRadiationDose].SetString(0, value); }
        }
        
       #endregion
    }

    #region RadiationMode Enum
    /// <summary>
    /// Specifies X-Ray radiation mode.
    /// </summary>
    public enum RadiationMode
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 
        /// </summary>
        Continuous,
        /// <summary>
        /// 
        /// </summary>
        Pulsed
    }
    #endregion
    
}
