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
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Dicom.Iod;
using ClearCanvas.Dicom.Utilities;
using System.Threading;
using ClearCanvas.Dicom.Utilities.Xml;
using System.Text;

namespace ClearCanvas.Dicom.DataStore
{
	public class Study : IStudy, IEquatable<Study>
    {
		private event EventHandler _changed;
		
		#region Private Fields

		private Guid _studyOid;
    	private string _studyInstanceUid;
    	private string _patientId;
    	private PersonName _patientsName;
    	private string _patientsNameRaw;
    	private string _patientsSex;
    	private string _patientsBirthDateRaw;
    	private string _studyId;
    	private string _accessionNumber;
    	private string _studyDescription;
		private PersonName _referringPhysiciansName;
		private string _referringPhysiciansNameRaw;
		private DateTime? _studyDate;
		private string _studyDateRaw;
    	private string _studyTimeRaw;
		private ReadOnlyCollection<string> _modalitiesInStudy;
		private int _numberOfStudyRelatedInstances;
		private int _numberOfStudyRelatedSeries;
		private string _specificCharacterSet;
    	private string _procedureCodeSequenceCodeValue;
    	private string _procedureCodeSequenceCodingSchemeDesignator;
        private DateTime? _storeTime;
		private DicomUri _studyXmlUri;

		private List<ISeries> _series;
		private StudyXml _studyXml;

		#endregion //Private Fields

		protected internal Study()
        {
		}

		internal virtual event EventHandler Changed
		{
			add { _changed += value; }
			remove { _changed -= value; }
		}

		#region Public Properties

		#region NHibernate Persistent Properties

		protected virtual Guid StudyOid
        {
            get { return _studyOid; }
			set { _studyOid = value; }
		}

		public virtual string SpecificCharacterSet
		{
			get { return _specificCharacterSet; }
			set { SetClassMember(ref _specificCharacterSet, value); }
		}

		[QueryableProperty(DicomTags.StudyInstanceUid, IsUnique = true)]
    	public virtual string StudyInstanceUid
    	{
    		get { return _studyInstanceUid; }
			set { SetClassMember(ref _studyInstanceUid, value); }
		}

		[QueryableProperty(DicomTags.PatientId, IsRequired = true)]
		public virtual string PatientId
    	{
    		get { return _patientId; }
			set { SetClassMember(ref _patientId, value); }
    	}

		[QueryableProperty(DicomTags.PatientsName, IsRequired = true)]
		public virtual PersonName PatientsName
    	{
    		get { return _patientsName; }
			set { SetClassMember(ref _patientsName, value); }
    	}

		string IPatientData.PatientsName
		{
			get { return _patientsName; }	
		}

    	public virtual string PatientsNameRaw
    	{
    		get { return _patientsNameRaw; }
			set { SetClassMember(ref _patientsNameRaw, value); }
    	}

		[QueryableProperty(DicomTags.PatientsSex)]
		public virtual string PatientsSex
    	{
    		get { return _patientsSex; }
			set { SetClassMember(ref _patientsSex, value); }
    	}

		#region IPatientData Members

		string IPatientData.PatientsBirthDate
		{
			get { return _patientsBirthDateRaw; }
		}

		public string PatientsBirthTime
		{
			get
			{
				//TODO: add this to the database!!!
				foreach (ISeries series in Series)
				{
					foreach (ISopInstance instance in series.GetSopInstances())
					{
						return instance[DicomTags.PatientsBirthTime].ToString();
					}
				}

				return "";
			}
		}

		#endregion
		[QueryableProperty(DicomTags.PatientsBirthDate, PostFilterOnly = true)]
		public virtual string PatientsBirthDateRaw
    	{
    		get { return _patientsBirthDateRaw; }
			set { SetClassMember(ref _patientsBirthDateRaw, value); }
    	}

		[QueryableProperty(DicomTags.StudyId, IsRequired = true)]
		public virtual string StudyId
        {
            get { return _studyId; }
			set { SetClassMember(ref _studyId, value); }
        }

		[QueryableProperty(DicomTags.AccessionNumber, IsRequired = true)]
		public virtual string AccessionNumber
    	{
    		get { return _accessionNumber; }
			set { SetClassMember(ref _accessionNumber, value); }
    	}

		[QueryableProperty(DicomTags.StudyDescription)]
		public virtual string StudyDescription
    	{
    		get { return _studyDescription; }
			set { SetClassMember(ref _studyDescription, value); }
    	}

		[QueryableProperty(DicomTags.ReferringPhysiciansName, IsRequired = true)]
		public virtual PersonName ReferringPhysiciansName
		{
			get { return _referringPhysiciansName; }
			set { SetClassMember(ref _referringPhysiciansName, value); }
		}

		string IStudyData.ReferringPhysiciansName
		{
			get { return _referringPhysiciansName; }
		}

		public virtual string ReferringPhysiciansNameRaw
		{
			get { return _referringPhysiciansNameRaw; }
			set { SetClassMember(ref _referringPhysiciansNameRaw, value); }
		}

		[QueryableProperty(DicomTags.StudyDate, IsRequired = true, ReturnProperty = "StudyDateRaw")]
		public virtual DateTime? StudyDate
    	{
    		get { return _studyDate; }
			set { SetNullableTypeMember(ref _studyDate, value); }
    	}

		public virtual string StudyDateRaw
    	{
    		get { return _studyDateRaw; }
			set { SetClassMember(ref _studyDateRaw, value); }
    	}

		[QueryableProperty(DicomTags.StudyTime, IsRequired = true, PostFilterOnly = true)]
		public virtual string StudyTimeRaw
        {
            get { return _studyTimeRaw; }
			set { SetClassMember(ref _studyTimeRaw, value); }
		}

		[QueryableProperty(DicomTags.ModalitiesInStudy)]
		public virtual string ModalitiesInStudy
		{
			get { return DicomStringHelper.GetDicomStringArray((this as IStudy).ModalitiesInStudy); }
			set
			{
				if (ModalitiesInStudy != value)
				{
					_modalitiesInStudy = new ReadOnlyCollection<string>(DicomStringHelper.GetStringArray(value ?? ""));
					OnChanged();
				}
			}
		}

		[QueryableProperty(DicomTags.NumberOfStudyRelatedSeries)]
		public virtual int NumberOfStudyRelatedSeries
		{
			get { return _numberOfStudyRelatedSeries; }
			set { SetValueTypeMember(ref _numberOfStudyRelatedSeries, value); }
		}

		int? IStudyData.NumberOfStudyRelatedSeries
		{
			get { return NumberOfStudyRelatedSeries; }	
		}

		[QueryableProperty(DicomTags.NumberOfStudyRelatedInstances)]
		public virtual int NumberOfStudyRelatedInstances
		{
			get { return _numberOfStudyRelatedInstances; }
			set { SetValueTypeMember(ref _numberOfStudyRelatedInstances, value);}
		}

		int? IStudyData.NumberOfStudyRelatedInstances
		{
			get { return NumberOfStudyRelatedInstances; }
		}

		[QueryableProperty(DicomTags.ProcedureCodeSequence, DicomTags.CodeValue)]
		public virtual string ProcedureCodeSequenceCodeValue
		{
			get { return _procedureCodeSequenceCodeValue; }
			set { SetClassMember(ref _procedureCodeSequenceCodeValue, value); }
		}

		[QueryableProperty(DicomTags.ProcedureCodeSequence, DicomTags.CodingSchemeDesignator)]
		public virtual string ProcedureCodeSequenceCodingSchemeDesignator
		{
			get { return _procedureCodeSequenceCodingSchemeDesignator; }
			set { SetClassMember(ref _procedureCodeSequenceCodingSchemeDesignator, value); }
		}

		public virtual DateTime? StoreTime
        {
            get { return _storeTime; }
			set { SetNullableTypeMember(ref _storeTime, value); }
        }

		public virtual DicomUri StudyXmlUri
	    {
			get { return _studyXmlUri; }
			set { SetClassMember(ref _studyXmlUri, value); }
		}

		#endregion
		#endregion

		#region Private Properties

		private StudyXml StudyXml
		{
			get
			{
				LoadStudyXml(true);
			    return _studyXml;
			}	
		}

		private List<ISeries> Series
        {
            get
            {
				if (_series == null)
				{
					_series = new List<ISeries>();
					foreach (SeriesXml seriesXml in StudyXml)
						_series.Add(new Series(this, seriesXml));
				}

				return _series;
            }
		}

		#endregion

		#region IEquatable<Study> Members

		public bool Equals(Study other)
    	{
			if (other == null)
				return false;

			return (StudyInstanceUid == other.StudyInstanceUid);
    	}

    	#endregion

    	public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

			if (obj is Study)
				return Equals((Study) obj);

			return false;
        }
		
        public override int GetHashCode()
        {
            int accumulator = 0;
            foreach (char character in StudyInstanceUid)
            {
                if ('.' != character)
					accumulator += System.Convert.ToInt32(character);
                else
                    accumulator -= 23;
            }
            return accumulator;
        }

		#region IStudy Members

		string IStudyData.StudyDate
		{
			get { return _studyDateRaw; }
		}

		string IStudyData.StudyTime
		{
			get { return _studyTimeRaw; }
		}

		string[] IStudyData.ModalitiesInStudy
		{
			get
			{
				if (_modalitiesInStudy != null)
					return CollectionUtils.ToArray(_modalitiesInStudy);
				else
					return new string[0];
			}	
		}

		public DateTime? GetStoreTime()
		{
			return _storeTime;
		}

		public IEnumerable<ISeries> GetSeries()
		{
			foreach (ISeries series in Series)
				yield return series;
		}

		public IEnumerable<ISopInstance> GetSopInstances()
        {
            foreach (ISeries series in Series)
            {
                foreach (ISopInstance sopInstance in series.GetSopInstances())
                {
                	yield return sopInstance;
                }
            }
        }

		#endregion

		#region Helper Methods

		internal void Initialize(DicomFile file)
		{
			DicomAttributeCollection sopInstanceDataset = file.DataSet;
			
			DicomAttribute attribute = sopInstanceDataset[DicomTags.StudyInstanceUid];
			string datasetStudyUid = attribute.ToString();
			if (!String.IsNullOrEmpty(StudyInstanceUid) && StudyInstanceUid != datasetStudyUid)
			{
				string message = String.Format("The study uid in the data set does not match this study's uid ({0} != {1}).", 
				                               datasetStudyUid, StudyInstanceUid);

				throw new InvalidOperationException(message);
			}
			else
			{
				StudyInstanceUid = attribute.ToString();
			}

			Platform.CheckForEmptyString(StudyInstanceUid, "StudyInstanceUid");

			attribute = sopInstanceDataset[DicomTags.PatientId];
			PatientId = attribute.ToString();

			attribute = sopInstanceDataset[DicomTags.PatientsName];
			PatientsName = new PersonName(attribute.ToString());
			PatientsNameRaw = DicomImplementation.CharacterParser.EncodeAsIsomorphicString(PatientsName, sopInstanceDataset.SpecificCharacterSet);

			attribute = sopInstanceDataset[DicomTags.ReferringPhysiciansName];
			ReferringPhysiciansName = new PersonName(attribute.ToString());
			ReferringPhysiciansNameRaw = DicomImplementation.CharacterParser.EncodeAsIsomorphicString(ReferringPhysiciansName, sopInstanceDataset.SpecificCharacterSet);

			attribute = sopInstanceDataset[DicomTags.PatientsSex];
			PatientsSex = attribute.ToString();

			attribute = sopInstanceDataset[DicomTags.PatientsBirthDate];
			PatientsBirthDateRaw = attribute.ToString();

			attribute = sopInstanceDataset[DicomTags.StudyId];
			StudyId = attribute.ToString();

			attribute = sopInstanceDataset[DicomTags.AccessionNumber];
			AccessionNumber = attribute.ToString();

			attribute = sopInstanceDataset[DicomTags.StudyDescription];
			StudyDescription = attribute.ToString();

			attribute = sopInstanceDataset[DicomTags.StudyDate];
			StudyDateRaw = attribute.ToString();
			StudyDate = DateParser.Parse(StudyDateRaw);

			attribute = sopInstanceDataset[DicomTags.StudyTime];
			StudyTimeRaw = attribute.ToString();

			if (sopInstanceDataset.Contains(DicomTags.ProcedureCodeSequence))
			{
				attribute = sopInstanceDataset[DicomTags.ProcedureCodeSequence];
				if (!attribute.IsEmpty && !attribute.IsNull)
				{
					DicomSequenceItem sequence = ((DicomSequenceItem[]) attribute.Values)[0];
					ProcedureCodeSequenceCodeValue = sequence[DicomTags.CodeValue].ToString();
					ProcedureCodeSequenceCodingSchemeDesignator = sequence[DicomTags.CodingSchemeDesignator].ToString();
				}
			}	

			attribute = sopInstanceDataset[DicomTags.SpecificCharacterSet];
			SpecificCharacterSet = attribute.ToString();

			string[] modalitiesInStudy = DicomStringHelper.GetStringArray(ModalitiesInStudy ?? "");
			ModalitiesInStudy = DicomStringHelper.GetDicomStringArray(
				ComputeModalitiesInStudy(modalitiesInStudy, sopInstanceDataset[DicomTags.Modality].GetString(0, "")));
		}

		internal void Update(DicomFile file)
		{
			Initialize(file);

			LoadStudyXml(false);
			_studyXml.AddFile(file);

			//these have to be here, rather than in Initialize b/c they are 
			// computed from the series, which are parsed from the xml.
			NumberOfStudyRelatedSeries = _studyXml.NumberOfStudyRelatedSeries;
			NumberOfStudyRelatedInstances = _studyXml.NumberOfStudyRelatedInstances;
		}

		internal void Flush()
		{
			StudyXmlOutputSettings settings = new StudyXmlOutputSettings();
			settings.IncludePrivateValues = StudyXmlTagInclusion.IgnoreTag;
			settings.IncludeUnknownTags = StudyXmlTagInclusion.IgnoreTag;
			settings.IncludeLargeTags = StudyXmlTagInclusion.IncludeTagExclusion;
			settings.MaxTagLength = 2048;
			
			settings.IncludeSourceFileName = true;

			//Ensure the existing stuff is loaded.
			LoadStudyXml(false);

			XmlDocument doc = _studyXml.GetMemento(settings);

			using (FileStream stream = GetFileStream(FileMode.Create, FileAccess.Write))
			{
				StudyXmlIo.Write(doc, stream);
				stream.Close();
			}
		}

		private void LoadStudyXml(bool throwIfNotExists)
		{
			if (_studyXml == null)
			{
				if (StudyXmlUri == null)
					throw new DataStoreException("The study xml location must be set.");

				XmlDocument doc = new XmlDocument();
				_studyXml = new StudyXml(StudyInstanceUid);

				if (File.Exists(StudyXmlUri.LocalDiskPath))
				{
					using (FileStream stream = GetFileStream(FileMode.Open, FileAccess.Read))
					{
						StudyXmlIo.Read(doc, stream);
						_studyXml.SetMemento(doc);
					}
				}
				else if (throwIfNotExists)
				{
					throw new FileNotFoundException("The study xml file could not be found", StudyXmlUri.LocalDiskPath);
				}
			}
		}

		// This is a bit hacky, but it seems silly to use a named mutex for the rare case when the service
		// and client processes are both accessing the exact same file at the same time.
		private FileStream GetFileStream(FileMode fileMode, FileAccess fileAccess)
		{
			bool alreadyLogged = false;
			TimeSpan start = new TimeSpan(Platform.Time.Ticks);
			
			while(true)
			{
				try
				{
					return new FileStream(StudyXmlUri.LocalDiskPath, fileMode, fileAccess, FileShare.Delete);
				}
				catch(IOException) //thrown when another process has the file open.
				{
					if (!alreadyLogged)
					{
						alreadyLogged = true;
						Platform.Log(LogLevel.Info, "Failed to open the study xml file because another process currently has it open.  Retry will continue for up to 5 seconds.");
					}

					TimeSpan diff = new TimeSpan(Platform.Time.Ticks) - start;
					if (diff.TotalSeconds >= 5)
					{
						string message = String.Format("Another process has had the study xml (uid = {0}) file open for more than 5 seconds.  Aborting attempt to open file.", StudyInstanceUid);
						throw new TimeoutException(message);
					}

					Thread.Sleep(20);
				}
			}
		}

		private static IEnumerable<string> ComputeModalitiesInStudy(IEnumerable<string> existingModalities, string candidate)
		{
			foreach(string existingModality in existingModalities)
			{
				if (existingModality == candidate)
					candidate = null;

				yield return existingModality;
			}

			if (candidate != null)
				yield return candidate;
		}

		private void OnChanged()
		{
			EventsHelper.Fire(_changed, this, EventArgs.Empty);
		}

		private void SetValueTypeMember<T>(ref T member, T newValue)
			where T : struct
		{
			if (member.Equals(newValue))
				return;

			member = newValue;
			OnChanged();
		}

		private void SetClassMember<T>(ref T member, T newValue)
			where T : class
		{
			if (Equals(member, newValue))
				return;

			member = newValue;
			OnChanged();
		}

		private void SetNullableTypeMember<T>(ref T? member, T? newValue)
			where T : struct
		{
			if (Nullable.Equals(member, newValue))
				return;

			member = newValue;
			OnChanged();
		}

		#endregion

		public override string ToString()
		{
			string storeTime = StoreTime == null ? "" : StoreTime.ToString();

			StringBuilder builder = new StringBuilder();
			builder.AppendFormat("StoreTime: {0}, Patient: {1} {2}, A# {3}, Study Date: {4}",
								 storeTime, PatientId ?? "", PatientsName ?? "", AccessionNumber ?? "", StudyDateRaw ?? "");

			return builder.ToString();
		}
	}
}
