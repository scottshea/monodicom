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
using ClearCanvas.Common;

namespace ClearCanvas.Dicom.Network.Scu
{
	/// <summary>
	/// Used by <see cref="StorageScu"/> to specify the <see cref="DicomFile"/>s to transfer over the association.
	/// </summary>
	public class StorageInstance : EventArgs
	{
		#region Private Variables...
		private string _filename;
		private DicomStatus _sendStatus;
		private SopClass _sopClass;
		private string _sopInstanceUid;
		private TransferSyntax _syntax;
		private bool _infoLoaded = false;
		private string _extendedFailureDescription;
		private string _patientId = string.Empty;
		private string _patientsName = string.Empty;
		private string _studyInstanceUid = string.Empty;
		private DicomFile _dicomFile = null;
		#endregion

		#region Public Properties
		/// <summary>
		/// The filename of the storage instance.
		/// </summary>
		public string Filename
		{
			get { return _filename; }
			set { _filename = value; }
		}

		/// <summary>
		/// The <see cref="SopClass"/> of the storage instance.
		/// </summary>
		public SopClass SopClass
		{
			get { return _sopClass; }
			set
			{
				_sopClass = value;
				if (_sopClass != null && _syntax != null)
					_infoLoaded = true;
			}
		}

		/// <summary>
		/// The SOP Instance Uid of the storage instance.
		/// </summary>
		public string SopInstanceUid
		{
			get { return _sopInstanceUid; }
			set { _sopInstanceUid = value; }
		}

		/// <summary>
		/// The Study Instance Uid of the storage instance.
		/// </summary>
		public string StudyInstanceUid
		{
			get { return _studyInstanceUid; }
			set { _studyInstanceUid = value; }
		}

		/// <summary>
		/// The Patient's Name of the storage instance.
		/// </summary>
		public string PatientsName
		{
			get { return _patientsName; }
			set { _patientsName = value; }
		}

		/// <summary>
		/// The Patient Id of the storage instance.
		/// </summary>
		public string PatientId
		{
			get { return _patientId; }
			set { _patientId = value; }
		}

		/// <summary>
		/// The <see cref="TransferSyntax"/> of the storage instance.
		/// </summary>
		public TransferSyntax TransferSyntax
		{
			get { return _syntax; }
			set
			{
				_syntax = value;
				if (_sopClass != null && _syntax != null)
					_infoLoaded = true;
			}
		}

		/// <summary>
		/// The <see cref="DicomStatus"/> returned from the remote SCP when the storage instance was trasferred.
		/// </summary>
		public DicomStatus SendStatus
		{
			get { return _sendStatus; }
			set { _sendStatus = value; }
		}

		/// <summary>
		/// An extended failure description if <see cref="SendStatus"/> is a failure status.
		/// </summary>
		public string ExtendedFailureDescription
		{
			get { return _extendedFailureDescription; }
			set { _extendedFailureDescription = value; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="dicomFile"></param>
		public StorageInstance(DicomFile dicomFile)
		{
			_dicomFile = dicomFile;
			
			string sopClassInFile = _dicomFile.DataSet[DicomTags.SopClassUid].ToString();
			if (!sopClassInFile.Equals(_dicomFile.SopClass.Uid))
			{
				Platform.Log(LogLevel.Warn, "SOP Class in Meta Info ({0}) does not match SOP Class in DataSet ({1})",
							 _dicomFile.SopClass.Uid, sopClassInFile);
				_sopClass = SopClass.GetSopClass(sopClassInFile);
				if (_sopClass == null)
				{
					Platform.Log(LogLevel.Warn, "Unknown SOP Class in dataset, reverting to meta info:  {0}", sopClassInFile);
					_sopClass = _dicomFile.SopClass;
				}
			}
			else
				_sopClass = _dicomFile.SopClass;

			_syntax = _dicomFile.TransferSyntax;
			_sopInstanceUid = _dicomFile.MediaStorageSopInstanceUid;
			_filename = dicomFile.Filename;

			_studyInstanceUid = _dicomFile.DataSet[DicomTags.StudyInstanceUid].GetString(0, string.Empty);
			_patientsName = _dicomFile.DataSet[DicomTags.PatientsName].GetString(0, string.Empty);
			_patientId = _dicomFile.DataSet[DicomTags.PatientId].GetString(0, string.Empty);
			_infoLoaded = true;
		}

		public StorageInstance(DicomMessage msg)
		{
			_sopClass = msg.SopClass;

			_syntax = msg.TransferSyntax;
			_sopInstanceUid = msg.DataSet[DicomTags.SopInstanceUid].GetString(0, string.Empty);

			_studyInstanceUid = msg.DataSet[DicomTags.StudyInstanceUid].GetString(0, string.Empty);
			_patientsName = msg.DataSet[DicomTags.PatientsName].GetString(0, string.Empty);
			_patientId = msg.DataSet[DicomTags.PatientId].GetString(0, string.Empty);
			_infoLoaded = true;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="filename"></param>
		public StorageInstance(string filename)
		{
			_filename = filename;
		}

		/// <summary>
		/// Constructor for primary usage with the <see cref="StorageCommitScu"/> class.
		/// </summary>
		/// <param name="sopClass">The SOP Class for a DICOM instance</param>
		/// <param name="sopInstanceUid">The SOP Instance UID of a DICOM instance</param>
		public StorageInstance(SopClass sopClass, string sopInstanceUid)
		{
			_sopClass = sopClass;
			_sopInstanceUid = sopInstanceUid;
			_filename = String.Empty;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Load a <see cref="DicomFile"/> for the storage instance.
		/// </summary>
		/// <remarks>
		/// If the constructor that supplies a <see cref="DicomFile"/> is used, that file is returned.
		/// Otherwise, the file is loaded and returned.  Note that a reference is not kept for the file
		/// in this case.
		/// </remarks>
		/// <returns></returns>
		public DicomFile LoadFile()
		{
			if (_dicomFile != null)
				return _dicomFile;

			DicomFile theFile = new DicomFile(_filename);

			theFile.Load(DicomReadOptions.StorePixelDataReferences);

			_studyInstanceUid = theFile.DataSet[DicomTags.StudyInstanceUid].GetString(0, string.Empty);
			_patientsName = theFile.DataSet[DicomTags.PatientsName].GetString(0, string.Empty);
			_patientId = theFile.DataSet[DicomTags.PatientId].GetString(0, string.Empty);

			return theFile;
		}

		/// <summary>
		/// Load enough information from the file to allow negotiation of the association.
		/// </summary>
		public void LoadInfo()
		{
			if (_infoLoaded)
				return;

			DicomFile theFile = new DicomFile(_filename);

			theFile.Load(DicomTags.RelatedGeneralSopClassUid, DicomReadOptions.Default);
			string sopClassInFile = theFile.DataSet[DicomTags.SopClassUid].ToString();
			if (!sopClassInFile.Equals(theFile.SopClass.Uid))
			{
				Platform.Log(LogLevel.Warn, "SOP Class in Meta Info ({0}) does not match SOP Class in DataSet ({1})",
				             theFile.SopClass.Uid, sopClassInFile);
				_sopClass = SopClass.GetSopClass(sopClassInFile);
				if (_sopClass == null)
				{
					Platform.Log(LogLevel.Warn,"Unknown SOP Class in dataset, reverting to meta info:  {0}", sopClassInFile);
					_sopClass = theFile.SopClass;
				}
			}
			else
				_sopClass = theFile.SopClass;

			_syntax = theFile.TransferSyntax;
			_sopInstanceUid = theFile.MediaStorageSopInstanceUid;

			_infoLoaded = true;
		}
		#endregion
	}
}