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
using System.IO;

namespace ClearCanvas.Dicom.Samples
{
	/// <summary>
	/// Simple class for reading DICOMDIR files and sending the images they reference to a remote AE.
	/// </summary>
	public class DicomdirReader
	{
		private readonly string _aeTitle;
		private DicomDirectory _dir;
		private int _patientRecords = 0;
		private int _studyRecords = 0;
		private int _seriesRecords = 0;
		private int _instanceRecords = 0;

		public DicomdirReader(string aeTitle)
		{
			_aeTitle = aeTitle;
		}

		public DicomDirectory Dicomdir
		{
			get { return _dir;}
		}

		public int PatientRecords
		{
			get { return _patientRecords; }
			set { _patientRecords = value; }
		}

		public int StudyRecords
		{
			get { return _studyRecords; }
			set { _studyRecords = value; }
		}

		public int SeriesRecords
		{
			get { return _seriesRecords; }
			set { _seriesRecords = value; }
		}

		public int InstanceRecords
		{
			get { return _instanceRecords; }
			set { _instanceRecords = value; }
		}

		/// <summary>
		/// Load a DICOMDIR
		/// </summary>
		/// <param name="filename"></param>
		public void Load(string filename)
		{
			try
			{
				_dir = new DicomDirectory(_aeTitle);

				_dir.Load(filename);


				// Show a simple traversal
				foreach (DirectoryRecordSequenceItem patientRecord in _dir.RootDirectoryRecordCollection)
				{
					PatientRecords++;
					foreach (DirectoryRecordSequenceItem studyRecord in patientRecord.LowerLevelDirectoryRecordCollection)
					{
						StudyRecords++;
						foreach (DirectoryRecordSequenceItem seriesRecord in studyRecord.LowerLevelDirectoryRecordCollection)
						{
							SeriesRecords++;
							foreach (DirectoryRecordSequenceItem instanceRecord in seriesRecord.LowerLevelDirectoryRecordCollection)
							{
								InstanceRecords++;
							}
						}
					}
				}

				Logger.LogInfo("Loaded DICOMDIR with {0} Patient Records, {1} Study Records, {2} Series Records, and {3} Image Records",
					PatientRecords,StudyRecords,SeriesRecords,InstanceRecords);

			}
			catch (Exception e)
			{
				Logger.LogErrorException(e, "Unexpected exception reading DICOMDIR: {0}", filename);
			}
		}

		/// <summary>
		/// Send the images of a loaded DICOMDIR to a remote AE.
		/// </summary>
		/// <param name="rootPath"></param>
		/// <param name="aeTitle"></param>
		/// <param name="host"></param>
		/// <param name="port"></param>
		public void Send(string rootPath, string aeTitle, string host, int port)
		{
			if (_dir == null) return;

			StorageScu scu = new StorageScu();

			foreach (DirectoryRecordSequenceItem patientRecord in _dir.RootDirectoryRecordCollection)
			{
				foreach (DirectoryRecordSequenceItem studyRecord in patientRecord.LowerLevelDirectoryRecordCollection)
				{
					foreach (DirectoryRecordSequenceItem seriesRecord in studyRecord.LowerLevelDirectoryRecordCollection)
					{
						foreach (DirectoryRecordSequenceItem instanceRecord in seriesRecord.LowerLevelDirectoryRecordCollection)
						{
							string path = rootPath;

							foreach (string subpath in instanceRecord[DicomTags.ReferencedFileId].Values as string[])
								path = Path.Combine(path, subpath);

							scu.AddFileToSend(path);
						}
					}
				}
			}

			// Do the send
			scu.Send("DICOMDIR", aeTitle, host, port);

		}
	}
}
