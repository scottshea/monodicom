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
using System.ServiceModel;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Dicom.Utilities;

namespace ClearCanvas.Dicom.ServiceModel.Query
{
	/// <summary>
	/// Default implementation of <see cref="IStudyRootQueryBridge"/>.
	/// </summary>
	public class StudyRootQueryBridge : IStudyRootQueryBridge
	{
		private IStudyRootQuery _client;

		private IComparer<StudyRootStudyIdentifier> _studyComparer;
		private IComparer<SeriesIdentifier> _seriesComparer;
		private IComparer<ImageIdentifier> _imageComparer;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="client">The implementation of <see cref="IStudyRootQuery"/> this bridge will use.</param>
		public StudyRootQueryBridge(IStudyRootQuery client)
		{
			Platform.CheckForNullReference(client, "client");
			_client = client;
			_studyComparer = new StudyDateTimeComparer();
		}

		/// <summary>
		/// Comparer used to sort the results returned from <see cref="IStudyRootQuery.StudyQuery"/>.
		/// </summary>
		public IComparer<StudyRootStudyIdentifier> StudyComparer
		{
			get { return _studyComparer; }
			set { _studyComparer = value; }
		}

		/// <summary>
		/// Comparer used to sort the results returned from <see cref="IStudyRootQuery.SeriesQuery"/>.
		/// </summary>
		public IComparer<SeriesIdentifier> SeriesComparer
		{
			get { return _seriesComparer; }
			set { _seriesComparer = value; }
		}

		/// <summary>
		/// Comparer used to sort the results returned from <see cref="IStudyRootQuery.ImageQuery"/>.
		/// </summary>
		public IComparer<ImageIdentifier> ImageComparer
		{
			get { return _imageComparer; }
			set { _imageComparer = value; }
		}

		/// <summary>
		/// Performs a STUDY query for the given <b>exact</b> Accession Number.
		/// </summary>
		public IList<StudyRootStudyIdentifier> QueryByAccessionNumber(string accessionNumber)
		{
			Platform.CheckForEmptyString(accessionNumber, "accessionNumber");
			if (accessionNumber.Contains("*") || accessionNumber.Contains("?"))
				throw new ArgumentException("Accession Number cannot contain wildcard characters.");

			StudyRootStudyIdentifier criteria = new StudyRootStudyIdentifier();
			criteria.AccessionNumber = accessionNumber;
			return StudyQuery(criteria);
		}

		/// <summary>
		/// Performs a STUDY query for the given <b>exact</b> Patient Id.
		/// </summary>
		public IList<StudyRootStudyIdentifier> QueryByPatientId(string patientId)
		{
			Platform.CheckForEmptyString(patientId, "patientId");
			if (patientId.Contains("*") || patientId.Contains("?"))
				throw new ArgumentException("Patient Id cannot contain wildcard characters.");

			StudyRootStudyIdentifier criteria = new StudyRootStudyIdentifier();
			criteria.PatientId = patientId;
			return StudyQuery(criteria);
		}

		/// <summary>
		/// Performs a STUDY query for the given Study Instance Uid.
		/// </summary>
		public IList<StudyRootStudyIdentifier> QueryByStudyInstanceUid(string studyInstanceUid)
		{
			return QueryByStudyInstanceUid(new string[] {studyInstanceUid});
		}

		/// <summary>
		/// Performs a STUDY query for the given Study Instance Uids.
		/// </summary>
		public IList<StudyRootStudyIdentifier> QueryByStudyInstanceUid(IEnumerable<string> studyInstanceUids)
		{
			foreach (string studyInstanceUid in studyInstanceUids)
			{
				Platform.CheckForEmptyString(studyInstanceUid, "studyInstanceUid");

				if (studyInstanceUid.Contains("*") || studyInstanceUid.Contains("?"))
					throw new ArgumentException("Study Instance Uid cannot contain wildcard characters.");
			}

			StudyRootStudyIdentifier criteria = new StudyRootStudyIdentifier();
			criteria.StudyInstanceUid = DicomStringHelper.GetDicomStringArray(studyInstanceUids);
			return StudyQuery(criteria);
		}

		/// <summary>
		/// Performs a SERIES query for the given Study Instance Uid.
		/// </summary>
		public IList<SeriesIdentifier> SeriesQuery(string studyInstanceUid)
		{
			Platform.CheckForEmptyString(studyInstanceUid, "studyInstanceUid");

			SeriesIdentifier criteria = new SeriesIdentifier();
			criteria.StudyInstanceUid = studyInstanceUid;
			return SeriesQuery(criteria);
		}

		/// <summary>
		/// Performs an IMAGE query for the given Study and Series Instance Uid.
		/// </summary>
		public IList<ImageIdentifier> ImageQuery(string studyInstanceUid, string seriesInstanceUid)
		{
			Platform.CheckForEmptyString(studyInstanceUid, "studyInstanceUid");
			Platform.CheckForEmptyString(seriesInstanceUid, "seriesInstanceUid");

			ImageIdentifier criteria = new ImageIdentifier();
			criteria.StudyInstanceUid = studyInstanceUid;
			criteria.SeriesInstanceUid = seriesInstanceUid;
			return ImageQuery(criteria);
		}

		#region IStudyRootQuery Members

		/// <summary>
		/// Performs a STUDY level query.
		/// </summary>
		/// <exception cref="FaultException{TDetail}">Thrown when some part of the data in the request is poorly formatted.</exception>
		/// <exception cref="FaultException{QueryFailedFault}">Thrown when the query fails.</exception>
		public IList<StudyRootStudyIdentifier> StudyQuery(StudyRootStudyIdentifier queryCriteria)
		{
			IList<StudyRootStudyIdentifier> results = _client.StudyQuery(queryCriteria);
			if (_studyComparer != null)
				results = CollectionUtils.Sort(results, _studyComparer.Compare);

			return results;
		}

		/// <summary>
		/// Performs a SERIES level query.
		/// </summary>
		/// <exception cref="FaultException{DataValidationFault}">Thrown when some part of the data in the request is poorly formatted.</exception>
		/// <exception cref="FaultException{QueryFailedFault}">Thrown when the query fails.</exception>
		public IList<SeriesIdentifier> SeriesQuery(SeriesIdentifier queryCriteria)
		{
			IList<SeriesIdentifier> results = _client.SeriesQuery(queryCriteria);
			if (_seriesComparer != null)
				results = CollectionUtils.Sort(results, _seriesComparer.Compare);

			return results;
		}

		/// <summary>
		/// Performs an IMAGE level query.
		/// </summary>
		/// <exception cref="FaultException{DataValidationFault}">Thrown when some part of the data in the request is poorly formatted.</exception>
		/// <exception cref="FaultException{QueryFailedFault}">Thrown when the query fails.</exception>
		public IList<ImageIdentifier> ImageQuery(ImageIdentifier queryCriteria)
		{
			IList<ImageIdentifier> results = _client.ImageQuery(queryCriteria);
			if (_imageComparer != null)
				results = CollectionUtils.Sort(results, _imageComparer.Compare);

			return results;
		}

		#endregion

		/// <summary>
		/// Performs the appropriate query given the input <see cref="DicomAttributeCollection"/>, based
		/// on the value of the QueryRetrieveLevel attribute.
		/// </summary>
		public IList<DicomAttributeCollection> Query(DicomAttributeCollection queryCriteria)
		{
			Platform.CheckForNullReference(queryCriteria, "queryCriteria");

			string level = queryCriteria[DicomTags.QueryRetrieveLevel];
			Platform.CheckForEmptyString(level, "level");

			if (level == "STUDY")
				return Convert(_client.StudyQuery(new StudyRootStudyIdentifier(queryCriteria)));
			else if (level == "SERIES")
				return Convert(_client.SeriesQuery(new SeriesIdentifier(queryCriteria)));
			else if (level == "IMAGE")
				return Convert(_client.ImageQuery(new ImageIdentifier(queryCriteria)));

			throw new ArgumentException(String.Format("Query/Retrieve level '{0}' is not supported.", level));
		}

		private static IList<DicomAttributeCollection> Convert<T>(IList<T> identifiers) where T : Identifier, new()
		{
			return CollectionUtils.Map<T, DicomAttributeCollection>(identifiers,
				delegate(T id) { return id.ToDicomAttributeCollection(); });
		}

		/// <summary>
		/// Implementation of the Dispose pattern.
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_client != null && _client is IDisposable)
				{
					((IDisposable)_client).Dispose();
					_client = null;
				}
			}
		}

		#region IDisposable Members

		/// <summary>
		/// Disposes this instance.
		/// </summary>
		public void Dispose()
		{
			try
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}
			catch(CommunicationException)
			{
				//connection already closed.
			}
			catch (Exception e)
			{
				Platform.Log(LogLevel.Error, e);
			}
		}

		#endregion
	}
}
