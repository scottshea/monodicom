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

using System.Runtime.Serialization;
using ClearCanvas.Dicom.Iod;

namespace ClearCanvas.Dicom.ServiceModel.Query
{
	public interface ISeriesIdentifier : ISeriesData, IIdentifier
	{
		[DicomField(DicomTags.SeriesNumber)]
		new int? SeriesNumber { get; }
	}

	/// <summary>
	/// Query identifier for a series.
	/// </summary>
	[DataContract(Namespace = QueryNamespace.Value)]
	public class SeriesIdentifier : Identifier, ISeriesIdentifier
	{
		#region Private Fields

		private string _studyInstanceUid;
		private string _seriesInstanceUid;
		private string _modality;
		private string _seriesDescription;
		private int? _seriesNumber;
		private int? _numberOfSeriesRelatedInstances;

		#endregion

		#region Public Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public SeriesIdentifier()
		{
		}

		public SeriesIdentifier(ISeriesIdentifier other)
			: base(other)
		{
			CopyFrom(other);
			SeriesNumber = other.SeriesNumber;
		}

		public SeriesIdentifier(ISeriesData other, IIdentifier identifier)
			: base(identifier)
		{
			CopyFrom(other);
		}

		public SeriesIdentifier(ISeriesData other)
		{
			CopyFrom(other);
		}

		private void CopyFrom(ISeriesData other)
		{
			StudyInstanceUid = other.StudyInstanceUid;
			SeriesInstanceUid = other.SeriesInstanceUid;
			Modality = other.Modality;
			SeriesDescription = other.SeriesDescription;
			SeriesNumber = other.SeriesNumber;
			NumberOfSeriesRelatedInstances = other.NumberOfSeriesRelatedInstances;
		}

		/// <summary>
		/// Creates an instance of <see cref="SeriesIdentifier"/> from a <see cref="DicomAttributeCollection"/>.
		/// </summary>
		public SeriesIdentifier(DicomAttributeCollection attributes)
			: base(attributes)
		{
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the level of the query - SERIES.
		/// </summary>
		public override string QueryRetrieveLevel
		{
			get { return "SERIES"; }
		}

		/// <summary>
		/// Gets or sets the Study Instance Uid of the identified series.
		/// </summary>
		[DicomField(DicomTags.StudyInstanceUid, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = true)]
		public string StudyInstanceUid
		{
			get { return _studyInstanceUid; }
			set { _studyInstanceUid = value; }
		}

		/// <summary>
		/// Gets or sets the Series Instance Uid of the identified series.
		/// </summary>
		[DicomField(DicomTags.SeriesInstanceUid, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = true)]
		public string SeriesInstanceUid
		{
			get { return _seriesInstanceUid; }
			set { _seriesInstanceUid = value; }
		}

		/// <summary>
		/// Gets or sets the modality of the identified series.
		/// </summary>
		[DicomField(DicomTags.Modality, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string Modality
		{
			get { return _modality; }
			set { _modality = value; }
		}

		/// <summary>
		/// Gets or sets the series description of the identified series.
		/// </summary>
		[DicomField(DicomTags.SeriesDescription, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public string SeriesDescription
		{
			get { return _seriesDescription; }
			set { _seriesDescription = value; }
		}

		/// <summary>
		/// Gets or sets the series number of the identified series.
		/// </summary>
		[DicomField(DicomTags.SeriesNumber, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public int? SeriesNumber
		{
			get { return _seriesNumber; }
			set { _seriesNumber = value; }
		}

		int ISeriesData.SeriesNumber
		{
			get { return _seriesNumber ?? 0; }
		}

		/// <summary>
		/// Gets or sets the number of composite object instances belonging to the identified series.
		/// </summary>
		[DicomField(DicomTags.NumberOfSeriesRelatedInstances, CreateEmptyElement = true, SetNullValueIfEmpty = true)]
		[DataMember(IsRequired = false)]
		public int? NumberOfSeriesRelatedInstances
		{
			get { return _numberOfSeriesRelatedInstances; }
			set { _numberOfSeriesRelatedInstances = value; }
		}

		#endregion
	}
}
