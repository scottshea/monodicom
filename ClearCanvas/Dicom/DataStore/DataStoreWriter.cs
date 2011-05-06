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
using System.IO;
using NHibernate;

namespace ClearCanvas.Dicom.DataStore
{
	public sealed partial class DataAccessLayer
	{
		internal interface IDataStoreWriter : IDisposable
		{
			void StoreStudies(IEnumerable<Study> studies);
		}

		private class DataStoreWriter : SessionConsumer, IDataStoreWriter, IDataStoreStudyRemover
		{
			public DataStoreWriter(ISessionManager sessionManager)
				: base(sessionManager)
			{
			}

			#region IDataStoreWriter Members

			public void StoreStudies(IEnumerable<Study> studies)
			{
				try
				{
					SessionManager.BeginWriteTransaction();
					foreach (Study study in studies)
						Session.SaveOrUpdate(study);
				}
				catch (Exception e)
				{
					SessionManager.Rollback();
					throw new DataStoreException("Failed to commit studies to the data store.", e);
				}
			}

			#endregion

			#region IDataStoreStudyRemover Members

			public void ClearAllStudies()
			{
				try
				{
					using (IDataStoreReader reader = GetIDataStoreReader())
					{
						foreach (Study study in reader.GetStudies())
							File.Delete(study.StudyXmlUri.LocalDiskPath);
					}

					SessionManager.BeginWriteTransaction();
					Session.Delete("from Study");
					SessionManager.Commit();
				}
				catch (Exception e)
				{
					SessionManager.Rollback();
					throw new DataStoreException("Failed to clear all studies from the data store.", e);
				}
			}

			public void RemoveStudy(string studyInstanceUid)
			{
				RemoveStudies(new string[] { studyInstanceUid });
			}

			public void RemoveStudies(IEnumerable<string> studyInstanceUids)
			{
				try
				{
					using (IDataStoreReader reader = GetIDataStoreReader())
					{
						foreach (string studyUid in studyInstanceUids)
						{
							Study study = (Study)reader.GetStudy(studyUid);
							File.Delete(study.StudyXmlUri.LocalDiskPath);
						}
					}

					SessionManager.BeginWriteTransaction();
					foreach (string uid in studyInstanceUids)
					{
						Session.Delete("from Study where StudyInstanceUid_ = ?", uid, NHibernateUtil.String);
					}

					SessionManager.Commit();
				}
				catch (Exception e)
				{
					SessionManager.Rollback();
					throw new DataStoreException("Failed to clear specified studies from the data store.", e);
				}
			}

			#endregion

			protected override void Dispose(bool disposing)
			{
				SessionManager.Commit();
				base.Dispose(disposing);
			}
		}
	}
}