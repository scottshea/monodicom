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
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.Iod.Macros;

namespace ClearCanvas.Dicom.Iod.Modules
{
	/// <summary>
	/// PatientStudy Module
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.7.2.2 (Table C.7.4-a)</remarks>
	public class PatientStudyModuleIod : IodBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PatientStudyModuleIod"/> class.
		/// </summary>	
		public PatientStudyModuleIod() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="PatientStudyModuleIod"/> class.
		/// </summary>
		public PatientStudyModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider) { }

		/// <summary>
		/// Initializes the underlying collection to implement the module or sequence using default values.
		/// </summary>
		public void InitializeAttributes()
		{
			this.AdditionalPatientHistory = null;
			this.AdmissionId = null;
			this.AdmittingDiagnosesCodeSequence = null;
			this.AdmittingDiagnosesDescription = null;
			this.IssuerOfAdmissionId = null;
			this.IssuerOfServiceEpisodeId = null;
			this.Occupation = null;
			this.PatientsAge = null;
			this.PatientsSexNeutered = null;
			this.PatientsSize = null;
			this.PatientsWeight = null;
			this.ServiceEpisodeDescription = null;
			this.ServiceEpisodeId = null;
		}

		/// <summary>
		/// Checks if this module appears to be non-empty.
		/// </summary>
		/// <returns>True if the module appears to be non-empty; False otherwise.</returns>
		public bool HasValues()
		{
			if (string.IsNullOrEmpty(this.AdditionalPatientHistory)
			    && string.IsNullOrEmpty(this.AdmissionId)
			    && this.AdmittingDiagnosesCodeSequence == null
			    && string.IsNullOrEmpty(this.AdmittingDiagnosesDescription)
			    && string.IsNullOrEmpty(this.IssuerOfAdmissionId)
			    && string.IsNullOrEmpty(this.IssuerOfServiceEpisodeId)
			    && string.IsNullOrEmpty(this.Occupation)
			    && string.IsNullOrEmpty(this.PatientsAge)
			    && (!this.PatientsSexNeutered.HasValue || this.PatientsSexNeutered == Iod.PatientsSexNeutered.Unknown)
			    && !this.PatientsSize.HasValue
			    && !this.PatientsWeight.HasValue
			    && string.IsNullOrEmpty(this.ServiceEpisodeDescription)
			    && string.IsNullOrEmpty(this.ServiceEpisodeId))
				return false;
			return true;
		}

		/// <summary>
		/// Gets or sets the value of AdmittingDiagnosesDescription in the underlying collection. Type 3.
		/// </summary>
		public string AdmittingDiagnosesDescription
		{
			get { return base.DicomAttributeProvider[DicomTags.AdmittingDiagnosesDescription].ToString(); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.AdmittingDiagnosesDescription] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.AdmittingDiagnosesDescription].SetStringValue(value);
			}
		}

		/// <summary>
		/// Gets or sets the value of AdmittingDiagnosesCodeSequence in the underlying collection. Type 3.
		/// </summary>
		public CodeSequenceMacro[] AdmittingDiagnosesCodeSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.AdmittingDiagnosesCodeSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
				{
					return null;
				}

				CodeSequenceMacro[] result = new CodeSequenceMacro[dicomAttribute.Count];
				DicomSequenceItem[] items = (DicomSequenceItem[]) dicomAttribute.Values;
				for (int n = 0; n < items.Length; n++)
					result[n] = new CodeSequenceMacro(items[n]);

				return result;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					base.DicomAttributeProvider[DicomTags.AdmittingDiagnosesCodeSequence] = null;
					return;
				}

				DicomSequenceItem[] result = new DicomSequenceItem[value.Length];
				for (int n = 0; n < value.Length; n++)
					result[n] = value[n].DicomSequenceItem;

				base.DicomAttributeProvider[DicomTags.AdmittingDiagnosesCodeSequence].Values = result;
			}
		}

		/// <summary>
		/// Gets or sets the value of PatientsAge in the underlying collection. Type 3.
		/// </summary>
		public string PatientsAge
		{
			get { return base.DicomAttributeProvider[DicomTags.PatientsAge].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.PatientsAge] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.PatientsAge].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of PatientsSize in the underlying collection. Type 3.
		/// </summary>
		public double? PatientsSize
		{
			get
			{
				double result;
				if (base.DicomAttributeProvider[DicomTags.PatientsSize].TryGetFloat64(0, out result))
					return result;
				return null;
			}
			set
			{
				if (!value.HasValue)
				{
					base.DicomAttributeProvider[DicomTags.PatientsSize] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.PatientsSize].SetFloat64(0, value.Value);
			}
		}

		/// <summary>
		/// Gets or sets the value of PatientsWeight in the underlying collection. Type 3.
		/// </summary>
		public double? PatientsWeight
		{
			get
			{
				double result;
				if (base.DicomAttributeProvider[DicomTags.PatientsWeight].TryGetFloat64(0, out result))
					return result;
				return null;
			}
			set
			{
				if (!value.HasValue)
				{
					base.DicomAttributeProvider[DicomTags.PatientsWeight] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.PatientsWeight].SetFloat64(0, value.Value);
			}
		}

		/// <summary>
		/// Gets or sets the value of Occupation in the underlying collection. Type 3.
		/// </summary>
		public string Occupation
		{
			get { return base.DicomAttributeProvider[DicomTags.Occupation].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.Occupation] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.Occupation].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of AdditionalPatientHistory in the underlying collection. Type 3.
		/// </summary>
		public string AdditionalPatientHistory
		{
			get { return base.DicomAttributeProvider[DicomTags.AdditionalPatientHistory].ToString(); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.AdditionalPatientHistory] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.AdditionalPatientHistory].SetStringValue(value);
			}
		}

		/// <summary>
		/// Gets or sets the value of AdmissionId in the underlying collection. Type 3.
		/// </summary>
		public string AdmissionId
		{
			get { return base.DicomAttributeProvider[DicomTags.AdmissionId].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.AdmissionId] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.AdmissionId].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of IssuerOfAdmissionId in the underlying collection. Type 3.
		/// </summary>
		public string IssuerOfAdmissionId
		{
			get { return base.DicomAttributeProvider[DicomTags.IssuerOfAdmissionId].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.IssuerOfAdmissionId] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.IssuerOfAdmissionId].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of ServiceEpisodeId in the underlying collection. Type 3.
		/// </summary>
		public string ServiceEpisodeId
		{
			get { return base.DicomAttributeProvider[DicomTags.ServiceEpisodeId].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.ServiceEpisodeId] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.ServiceEpisodeId].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of IssuerOfServiceEpisodeId in the underlying collection. Type 3.
		/// </summary>
		public string IssuerOfServiceEpisodeId
		{
			get { return base.DicomAttributeProvider[DicomTags.IssuerOfServiceEpisodeId].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.IssuerOfServiceEpisodeId] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.IssuerOfServiceEpisodeId].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of ServiceEpisodeDescription in the underlying collection. Type 3.
		/// </summary>
		public string ServiceEpisodeDescription
		{
			get { return base.DicomAttributeProvider[DicomTags.ServiceEpisodeDescription].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.ServiceEpisodeDescription] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.ServiceEpisodeDescription].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of PatientsSexNeutered in the underlying collection. Type 2C.
		/// </summary>
		public PatientsSexNeutered? PatientsSexNeutered
		{
			get
			{
				if (base.DicomAttributeProvider[DicomTags.PatientsSexNeutered].IsEmpty)
					return null;
				return ParseEnum(base.DicomAttributeProvider[DicomTags.PatientsSexNeutered].GetString(0, string.Empty), Iod.PatientsSexNeutered.Unknown);
			}
			set
			{
				if (!value.HasValue)
				{
					base.DicomAttributeProvider[DicomTags.PatientsSexNeutered] = null;
					return;
				}
				if (value == Iod.PatientsSexNeutered.Unknown)
				{
					base.DicomAttributeProvider[DicomTags.PatientsSexNeutered].SetNullValue();
					return;
				}
				SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.PatientsSexNeutered], value);
			}
		}

		/// <summary>
		/// Gets an enumeration of <see cref="DicomTag"/>s used by this module.
		/// </summary>
		public static IEnumerable<uint> DefinedTags {
			get {
				yield return DicomTags.AdditionalPatientHistory;
				yield return DicomTags.AdmissionId;
				yield return DicomTags.AdmittingDiagnosesCodeSequence;
				yield return DicomTags.AdmittingDiagnosesDescription;
				yield return DicomTags.IssuerOfAdmissionId;
				yield return DicomTags.IssuerOfServiceEpisodeId;
				yield return DicomTags.Occupation;
				yield return DicomTags.PatientsAge;
				yield return DicomTags.PatientsSexNeutered;
				yield return DicomTags.PatientsSize;
				yield return DicomTags.PatientsWeight;
				yield return DicomTags.ServiceEpisodeDescription;
				yield return DicomTags.ServiceEpisodeId;
			}
		}
	}
}