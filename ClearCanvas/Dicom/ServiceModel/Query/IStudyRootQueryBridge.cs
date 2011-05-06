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
using ClearCanvas.Dicom;

namespace ClearCanvas.Dicom.ServiceModel.Query
{
	/// <summary>
	/// Bridge interface for <see cref="IStudyRootQuery"/>.
	/// </summary>
	/// <remarks>
	/// The bridge design pattern allows the public interface (<see cref="IStudyRootQueryBridge"/>) and it's
	/// underlying implementation <see cref="IStudyRootQuery"/> to vary independently.
	/// </remarks>
	public interface IStudyRootQueryBridge : IStudyRootQuery, IDisposable
	{
		/// <summary>
		/// Comparer used to sort the results returned from <see cref="IStudyRootQuery.StudyQuery"/>.
		/// </summary>
		IComparer<StudyRootStudyIdentifier> StudyComparer { get; set; }
		/// <summary>
		/// Comparer used to sort the results returned from <see cref="IStudyRootQuery.SeriesQuery"/>.
		/// </summary>
		IComparer<SeriesIdentifier> SeriesComparer { get; set; }
		/// <summary>
		/// Comparer used to sort the results returned from <see cref="IStudyRootQuery.ImageQuery"/>.
		/// </summary>
		IComparer<ImageIdentifier> ImageComparer { get; set; }

		/// <summary>
		/// Performs a STUDY query for the given <b>exact</b> Accession Number.
		/// </summary>
		IList<StudyRootStudyIdentifier> QueryByAccessionNumber(string accessionNumber);

		/// <summary>
		/// Performs a STUDY query for the given <b>exact</b> Patient Id.
		/// </summary>
		IList<StudyRootStudyIdentifier> QueryByPatientId(string patientId);

		/// <summary>
		/// Performs a STUDY query for the given Study Instance Uid.
		/// </summary>
		IList<StudyRootStudyIdentifier> QueryByStudyInstanceUid(string studyInstanceUid);

		/// <summary>
		/// Performs a STUDY query for the given Study Instance Uids.
		/// </summary>
		IList<StudyRootStudyIdentifier> QueryByStudyInstanceUid(IEnumerable<string> studyInstanceUids);

		/// <summary>
		/// Performs a SERIES query for the given Study Instance Uid.
		/// </summary>
		IList<SeriesIdentifier> SeriesQuery(string studyInstanceUid);

		/// <summary>
		/// Performs an IMAGE query for the given Study and Series Instance Uid.
		/// </summary>
		IList<ImageIdentifier> ImageQuery(string studyInstanceUid, string seriesInstanceUid);

		/// <summary>
		/// Performs the appropriate query given the input <see cref="DicomAttributeCollection"/>, based
		/// on the value of the QueryRetrieveLevel attribute.
		/// </summary>
		IList<DicomAttributeCollection> Query(DicomAttributeCollection queryCriteria);
	}
}
