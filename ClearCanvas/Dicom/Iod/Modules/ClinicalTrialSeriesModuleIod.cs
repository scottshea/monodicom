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

using System.Collections.Generic;

namespace ClearCanvas.Dicom.Iod.Modules
{
	/// <summary>
	/// ClinicalTrialSeries Module
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.7.3.2 (Table C.7-5b)</remarks>
	public class ClinicalTrialSeriesModuleIod : IodBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ClinicalTrialSeriesModuleIod"/> class.
		/// </summary>	
		public ClinicalTrialSeriesModuleIod() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="ClinicalTrialSeriesModuleIod"/> class.
		/// </summary>
		public ClinicalTrialSeriesModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider) { }

		/// <summary>
		/// Initializes the underlying collection to implement the module or sequence using default values.
		/// </summary>
		public void InitializeAttributes()
		{
			this.ClinicalTrialCoordinatingCenterName = null;
			this.ClinicalTrialSeriesId = null;
			this.ClinicalTrialSeriesDescription = null;
		}

		/// <summary>
		/// Checks if this module appears to be non-empty.
		/// </summary>
		/// <returns>True if the module appears to be non-empty; False otherwise.</returns>
		public bool HasValues()
		{
			if (string.IsNullOrEmpty(this.ClinicalTrialCoordinatingCenterName)
			    && string.IsNullOrEmpty(this.ClinicalTrialSeriesId)
			    && string.IsNullOrEmpty(this.ClinicalTrialSeriesDescription))
				return false;
			return true;
		}

		/// <summary>
		/// Gets or sets the value of ClinicalTrialCoordinatingCenterName in the underlying collection. Type 2.
		/// </summary>
		public string ClinicalTrialCoordinatingCenterName
		{
			get { return base.DicomAttributeProvider[DicomTags.ClinicalTrialCoordinatingCenterName].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.ClinicalTrialCoordinatingCenterName].SetNullValue();
					return;
				}
				base.DicomAttributeProvider[DicomTags.ClinicalTrialCoordinatingCenterName].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of ClinicalTrialSeriesId in the underlying collection. Type 3.
		/// </summary>
		public string ClinicalTrialSeriesId
		{
			get { return base.DicomAttributeProvider[DicomTags.ClinicalTrialSeriesId].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.ClinicalTrialSeriesId] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.ClinicalTrialSeriesId].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of ClinicalTrialSeriesDescription in the underlying collection. Type 3.
		/// </summary>
		public string ClinicalTrialSeriesDescription
		{
			get { return base.DicomAttributeProvider[DicomTags.ClinicalTrialSeriesDescription].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.ClinicalTrialSeriesDescription] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.ClinicalTrialSeriesDescription].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets an enumeration of <see cref="DicomTag"/>s used by this module.
		/// </summary>
		public static IEnumerable<uint> DefinedTags {
			get {
				yield return DicomTags.ClinicalTrialCoordinatingCenterName;
				yield return DicomTags.ClinicalTrialSeriesDescription;
				yield return DicomTags.ClinicalTrialSeriesId;
			}
		}
	}
}