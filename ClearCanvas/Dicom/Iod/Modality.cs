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

namespace ClearCanvas.Dicom.Iod
{
	/// <summary>
	/// Modality Enum.  as per Part 3, C.7.3.1.1.1
	/// </summary>
	/// <remarks>The modality listed in the Modality Data Element (0008,0060) may not match the name of the 
	/// IOD in which it appears. For example, a SOP instance from XA IOD may list the RF modality 
	/// when an RF implementation produces an XA object.
	/// </remarks>
	public enum Modality
	{
		/// <summary>
		/// None or blank value
		/// </summary>
		None,

		/// <summary>
		/// Computed Radiography
		/// </summary>
		CR,

		/// <summary>
		/// Computed Tomography
		/// </summary>
		CT,

		/// <summary>
		/// Magnetic Resonance
		/// </summary>
		/// <remarks>
		/// The MR modality incorporates the retired modalities MA and MS.
		/// </remarks>
		MR,

		/// <summary>
		/// Nuclear Medicine
		/// </summary>
		NM,

		/// <summary>
		/// Ultrasound
		/// </summary>
		US,

		/// <summary>
		/// Other
		/// </summary>
		OT,

		/// <summary>
		/// Biomagnetic Imaging 
		/// </summary>
		BI,

		/// <summary>
		/// Color Flow Doppler
		/// </summary>
		CD,

		/// <summary>
		/// Duplex Doppler 
		/// </summary>
		DD,

		/// <summary>
		/// Diaphanography
		/// </summary>
		DG,

		/// <summary>
		/// Endoscopy 
		/// </summary>
		ES,

		/// <summary>
		/// Laser Surface Scan
		/// </summary>
		LS,

		/// <summary>
		/// Positron Emission Tomography (PET) 
		/// </summary>
		PT,

		/// <summary>
		/// Radiographic Imaging (Conventional Film/Screen)
		/// </summary>
		RG,

		/// <summary>
		/// Single-Photon Emission Computed Tomography (SPECT)
		/// </summary>
		ST,

		/// <summary>
		/// Thermography
		/// </summary>
		TG,

		/// <summary>
		/// X-Ray Angiography
		/// </summary>
		/// <remarks>
		/// The XA modality incorporates the retired modality DS.
		/// </remarks>
		XA,

		/// <summary>
		/// Radio Fluoroscopy (The RF modality incorporates the retired modalities CF, DF, VF.)
		/// </summary>
		RF,
		/// <summary>
		/// Radiotherapy Image
		/// </summary>
		RTImage,
		/// <summary>
		/// Radiotherapy Dose
		/// </summary>
		RTDose,
		/// <summary>
		/// Radiotherapy Structure Set 
		/// </summary>
		RTStruct,
		/// <summary>
		/// Radiotherapy Plan
		/// </summary>
		RTPlan,
		/// <summary>
		/// RT Treatment Record 
		/// </summary>
		RTRecord,
		/// <summary>
		/// Hard Copy
		/// </summary>
		HC,
		/// <summary>
		/// Digital Radiography 
		/// </summary>
		DX,
		/// <summary>
		/// Mammography
		/// </summary>
		MG,
		/// <summary>
		/// Intra-oral Radiography 
		/// </summary>
		IO,
		/// <summary>
		/// Panoramic X-Ray
		/// </summary>
		PX,
		/// <summary>
		/// General Microscopy
		/// </summary>
		GM,
		/// <summary>
		/// Slide Microscopy
		/// </summary>
		SM,
		/// <summary>
		/// External-camera Photography 
		/// </summary>
		XC,
		/// <summary>
		/// Presentation State
		/// </summary>
		PR,
		/// <summary>
		/// Audio
		/// </summary>
		AU,
		/// <summary>
		/// Electrocardiography
		/// </summary>
		Ecg,
		/// <summary>
		/// Cardiac Electrophysiology 
		/// </summary>
		Eps,
		/// <summary>
		/// Hemodynamic Waveform
		/// </summary>
		HD,
		/// <summary>
		/// SR Document 
		/// </summary>
		SR,
		/// <summary>
		/// Intravascular Ultrasound
		/// </summary>
		Ivus,
		/// <summary>
		/// Ophthalmic Photography 
		/// </summary>
		OP,
		/// <summary>
		/// Stereometric Relationship
		/// </summary>
		Smr,
		/// <summary>
		/// Optical Coherence Tomography 
		/// </summary>
		Oct,
		/// <summary>
		/// Ophthalmic Refraction
		/// </summary>
		Opr,
		/// <summary>
		/// Ophthalmic Visual Field
		/// </summary>
		Opv,
		/// <summary>
		/// Ophthalmic Mapping
		/// </summary>
		Opm,
		/// <summary>
		/// Key Object Selection
		/// </summary>
		KO,
		/// <summary>
		/// Segmentation
		/// </summary>
		Seg,
		/// <summary>
		/// Registration
		/// </summary>
		Reg,
		/// <summary>
		/// Digital Subtraction Angiography (retired)  The XA modality incorporates the retired modality DS.
		/// </summary>
		DS,
		/// <summary>
		/// Cinefluorography (retired) The RF modality incorporates the retired modalities CF, DF, VF.
		/// </summary>
		CF,
		/// <summary>
		/// Digital fluoroscopy (retired) The RF modality incorporates the retired modalities CF, DF, VF.
		/// </summary>
		DF,
		/// <summary>
		///  = Videofluorography (retired) The RF modality incorporates the retired modalities CF, DF, VF.
		/// </summary>
		VF,
		/// <summary>
		/// Angioscopy (retired)
		/// </summary>
		AS,
		/// <summary>
		/// Cystoscopy (retired)
		/// </summary>
		CS,
		/// <summary>
		/// Echocardiography (retired)
		/// </summary>
		EC,
		/// <summary>
		/// Laparoscopy (retired)
		/// </summary>
		LP,
		/// <summary>
		/// Fluorescein angiography (retired)
		/// </summary>
		FA,
		/// <summary>
		/// Culposcopy (retired)
		/// </summary>
		CP,
		/// <summary>
		/// Digital microscopy (retired)
		/// </summary>
		DM,
		/// <summary>
		/// Fundoscopy (retired)
		/// </summary>
		FS,
		/// <summary>
		/// Magnetic resonance angiography (retired) The MR modality incorporates the retired modalities MA and MS.
		/// </summary>
		MA,
		/// <summary>
		/// Magnetic resonance spectroscopy (retired) The MR modality incorporates the retired modalities MA and MS.
		/// </summary>
		MS
	}
}