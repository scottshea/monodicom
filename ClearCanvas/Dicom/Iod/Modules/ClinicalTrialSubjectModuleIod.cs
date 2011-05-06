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
using System.Collections.Generic;

namespace ClearCanvas.Dicom.Iod.Modules
{
	/// <summary>
	/// ClinicalTrialSubject Module
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.7.1.3 (Table C.7-2b)</remarks>
	public class ClinicalTrialSubjectModuleIod : IodBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ClinicalTrialSubjectModuleIod"/> class.
		/// </summary>	
		public ClinicalTrialSubjectModuleIod() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="ClinicalTrialSubjectModuleIod"/> class.
		/// </summary>
		public ClinicalTrialSubjectModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider) {}

		/// <summary>
		/// Initializes the underlying collection to implement the module or sequence using default values.
		/// </summary>
		public void InitializeAttributes()
		{
			this.ClinicalTrialProtocolId = " ";
			this.ClinicalTrialProtocolName = null;
			this.ClinicalTrialSiteId = null;
			this.ClinicalTrialSiteName = null;
			this.ClinicalTrialSponsorName = " ";
			this.ClinicalTrialSubjectId = null;
			this.ClinicalTrialSubjectReadingId = null;
		}

		/// <summary>
		/// Checks if this module appears to be non-empty.
		/// </summary>
		/// <returns>True if the module appears to be non-empty; False otherwise.</returns>
		public bool HasValues()
		{
			if (string.IsNullOrEmpty(this.ClinicalTrialProtocolId)
				&& string.IsNullOrEmpty(this.ClinicalTrialProtocolName)
				&& string.IsNullOrEmpty(this.ClinicalTrialSiteId)
				&& string.IsNullOrEmpty(this.ClinicalTrialSiteName)
				&& string.IsNullOrEmpty(this.ClinicalTrialSponsorName)
				&& string.IsNullOrEmpty(this.ClinicalTrialSubjectId)
				&& string.IsNullOrEmpty(this.ClinicalTrialSubjectReadingId))
				return false;
			return true;
		}

		/// <summary>
		/// Gets or sets the value of ClinicalTrialSponsorName in the underlying collection. Type 1.
		/// </summary>
		public string ClinicalTrialSponsorName
		{
			get { return base.DicomAttributeProvider[DicomTags.ClinicalTrialSponsorName].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException("value", "ClinicalTrialSponsorName is Type 1 Required.");
				base.DicomAttributeProvider[DicomTags.ClinicalTrialSponsorName].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of ClinicalTrialProtocolId in the underlying collection. Type 1.
		/// </summary>
		public string ClinicalTrialProtocolId
		{
			get { return base.DicomAttributeProvider[DicomTags.ClinicalTrialProtocolId].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException("value", "ClinicalTrialProtocolId is Type 1 Required.");
				base.DicomAttributeProvider[DicomTags.ClinicalTrialProtocolId].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of ClinicalTrialProtocolName in the underlying collection. Type 2.
		/// </summary>
		public string ClinicalTrialProtocolName
		{
			get { return base.DicomAttributeProvider[DicomTags.ClinicalTrialProtocolName].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.ClinicalTrialProtocolName].SetNullValue();
					return;
				}
				base.DicomAttributeProvider[DicomTags.ClinicalTrialProtocolName].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of ClinicalTrialSiteId in the underlying collection. Type 2.
		/// </summary>
		public string ClinicalTrialSiteId
		{
			get { return base.DicomAttributeProvider[DicomTags.ClinicalTrialSiteId].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.ClinicalTrialSiteId].SetNullValue();
					return;
				}
				base.DicomAttributeProvider[DicomTags.ClinicalTrialSiteId].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of ClinicalTrialSiteName in the underlying collection. Type 2.
		/// </summary>
		public string ClinicalTrialSiteName
		{
			get { return base.DicomAttributeProvider[DicomTags.ClinicalTrialSiteName].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.ClinicalTrialSiteName].SetNullValue();
					return;
				}
				base.DicomAttributeProvider[DicomTags.ClinicalTrialSiteName].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of ClinicalTrialSubjectId in the underlying collection. Type 1C.
		/// </summary>
		public string ClinicalTrialSubjectId
		{
			get { return base.DicomAttributeProvider[DicomTags.ClinicalTrialSubjectId].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.ClinicalTrialSubjectId] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.ClinicalTrialSubjectId].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of ClinicalTrialSubjectReadingId in the underlying collection. Type 1C.
		/// </summary>
		public string ClinicalTrialSubjectReadingId
		{
			get { return base.DicomAttributeProvider[DicomTags.ClinicalTrialSubjectReadingId].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.ClinicalTrialSubjectReadingId] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.ClinicalTrialSubjectReadingId].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets an enumeration of <see cref="DicomTag"/>s used by this module.
		/// </summary>
		public static IEnumerable<uint> DefinedTags {
			get {
				yield return DicomTags.ClinicalTrialProtocolId;
				yield return DicomTags.ClinicalTrialProtocolName;
				yield return DicomTags.ClinicalTrialSiteId;
				yield return DicomTags.ClinicalTrialSiteName;
				yield return DicomTags.ClinicalTrialSponsorName;
				yield return DicomTags.ClinicalTrialSubjectId;
				yield return DicomTags.ClinicalTrialSubjectReadingId;
			}
		}
	}
}