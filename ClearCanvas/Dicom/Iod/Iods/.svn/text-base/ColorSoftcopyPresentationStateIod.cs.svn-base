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

using ClearCanvas.Dicom.Iod.Modules;

namespace ClearCanvas.Dicom.Iod.Iods {
	public class ColorSoftcopyPresentationStateIod {
		private readonly IDicomAttributeProvider _dicomAttributeProvider;

		public ColorSoftcopyPresentationStateIod() : this(new DicomAttributeCollection()) { }

		public ColorSoftcopyPresentationStateIod(IDicomAttributeProvider provider) {
			_dicomAttributeProvider = provider;

			this.Patient = new PatientModuleIod(_dicomAttributeProvider);
			this.ClinicalTrialSubject = new ClinicalTrialSubjectModuleIod(_dicomAttributeProvider);

			this.GeneralStudy = new GeneralStudyModuleIod(_dicomAttributeProvider);
			this.PatientStudy = new PatientStudyModuleIod(_dicomAttributeProvider);
			this.ClinicalTrialStudy = new ClinicalTrialStudyModuleIod(_dicomAttributeProvider);

			this.GeneralSeries = new GeneralSeriesModuleIod(_dicomAttributeProvider);
			this.ClinicalTrialSeries = new ClinicalTrialSeriesModuleIod(_dicomAttributeProvider);
			this.PresentationSeries = new PresentationSeriesModuleIod(_dicomAttributeProvider);

			this.GeneralEquipment = new GeneralEquipmentModuleIod(_dicomAttributeProvider);

			this.PresentationStateIdentification = new PresentationStateIdentificationModuleIod(_dicomAttributeProvider);
			this.PresentationStateRelationship = new PresentationStateRelationshipModuleIod(_dicomAttributeProvider);
			this.PresentationStateShutter = new PresentationStateShutterModuleIod(_dicomAttributeProvider);
			this.DisplayShutter = new DisplayShutterModuleIod(_dicomAttributeProvider);
			this.BitmapDisplayShutter = new BitmapDisplayShutterModuleIod(_dicomAttributeProvider);
			this.OverlayPlane = new OverlayPlaneModuleIod(_dicomAttributeProvider);
			this.OverlayActivation = new OverlayActivationModuleIod(_dicomAttributeProvider);
			this.DisplayedArea = new DisplayedAreaModuleIod(_dicomAttributeProvider);
			this.GraphicAnnotation = new GraphicAnnotationModuleIod(_dicomAttributeProvider);
			this.SpatialTransform = new SpatialTransformModuleIod(_dicomAttributeProvider);
			this.GraphicLayer = new GraphicLayerModuleIod(_dicomAttributeProvider);
			this.IccProfile = new IccProfileModuleIod(_dicomAttributeProvider);
			this.SopCommon = new SopCommonModuleIod(_dicomAttributeProvider);
		}

		public IDicomAttributeProvider DicomAttributeProvider {
			get { return _dicomAttributeProvider; }
		}

		#region Patient IE

		/// <summary>
		/// Gets the Patient module (required usage).
		/// </summary>
		public readonly PatientModuleIod Patient;

		/// <summary>
		/// Gets the Clinical Trial Subject module (optional usage).
		/// </summary>
		public readonly ClinicalTrialSubjectModuleIod ClinicalTrialSubject;

		#endregion

		#region Study IE

		/// <summary>
		/// Gets the General Study module (required usage).
		/// </summary>
		public readonly GeneralStudyModuleIod GeneralStudy;

		/// <summary>
		/// Gets the Patient Study module (optional usage).
		/// </summary>
		public readonly PatientStudyModuleIod PatientStudy;

		/// <summary>
		/// Gets the Clinical Trial Study module (optional usage).
		/// </summary>
		public readonly ClinicalTrialStudyModuleIod ClinicalTrialStudy;

		#endregion

		#region Series IE

		/// <summary>
		/// Gets the General Series module (required usage).
		/// </summary>
		public readonly GeneralSeriesModuleIod GeneralSeries;

		/// <summary>
		/// Gets the Clinical Trial Series module (optional usage).
		/// </summary>
		public readonly ClinicalTrialSeriesModuleIod ClinicalTrialSeries;

		/// <summary>
		/// Gets the Presentation Series module (required usage).
		/// </summary>
		public readonly PresentationSeriesModuleIod PresentationSeries;

		#endregion

		#region Equipment IE

		/// <summary>
		/// Gets the General Equipment module (required usage).
		/// </summary>
		public readonly GeneralEquipmentModuleIod GeneralEquipment;

		#endregion

		#region Presentation State IE

		/// <summary>
		/// Gets the Presentation State Identification module (required usage).
		/// </summary>
		public readonly PresentationStateIdentificationModuleIod PresentationStateIdentification;

		/// <summary>
		/// Gets the Presentation State Relationship module (required usage).
		/// </summary>
		public readonly PresentationStateRelationshipModuleIod PresentationStateRelationship;

		/// <summary>
		/// Gets the Presentation State Shutter module (required usage).
		/// </summary>
		public readonly PresentationStateShutterModuleIod PresentationStateShutter;

		/// <summary>
		/// Gets the Display Shutter module (conditionally required usage).
		/// </summary>
		public readonly DisplayShutterModuleIod DisplayShutter;

		/// <summary>
		/// Gets the Bitmap Display Shutter module (conditionally required usage).
		/// </summary>
		public readonly BitmapDisplayShutterModuleIod BitmapDisplayShutter;

		/// <summary>
		/// Gets the Overlay Plane module (conditionally required usage).
		/// </summary>
		public readonly OverlayPlaneModuleIod OverlayPlane;

		/// <summary>
		/// Gets the Overlay Activation module (conditionally required usage).
		/// </summary>
		public readonly OverlayActivationModuleIod OverlayActivation;

		/// <summary>
		/// Gets the Displayed Area module (required usage).
		/// </summary>
		public readonly DisplayedAreaModuleIod DisplayedArea;

		/// <summary>
		/// Gets the Graphic Annotation module (conditionally required usage).
		/// </summary>
		public readonly GraphicAnnotationModuleIod GraphicAnnotation;

		/// <summary>
		/// Gets the Spatial Transform module (conditionally required usage).
		/// </summary>
		public readonly SpatialTransformModuleIod SpatialTransform;

		/// <summary>
		/// Gets the Graphic Layer module (conditionally required usage).
		/// </summary>
		public readonly GraphicLayerModuleIod GraphicLayer;

		/// <summary>
		/// Gets the ICC Profile module (required usage).
		/// </summary>
		public readonly IccProfileModuleIod IccProfile;

		/// <summary>
		/// Gets the SOP Common module (required usage).
		/// </summary>
		public readonly SopCommonModuleIod SopCommon;

		#endregion
	}
}