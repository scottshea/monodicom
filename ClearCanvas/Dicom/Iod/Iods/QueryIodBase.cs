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

namespace ClearCanvas.Dicom.Iod.Iods
{
	public abstract class QueryIodBase : IodBase
	{
		#region Constructors
        /// <summary>
		/// Initializes a new instance of the <see cref="QueryIodBase"/> class.
        /// </summary>
        public QueryIodBase() : base()
        {
            SetAttributeFromEnum(DicomAttributeProvider[DicomTags.QueryRetrieveLevel], QueryRetrieveLevel.Series);
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="QueryIodBase"/> class.
        /// </summary>
		public QueryIodBase(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider)
        {
            SetAttributeFromEnum(DicomAttributeProvider[DicomTags.QueryRetrieveLevel], QueryRetrieveLevel.Series);
        }
        #endregion


		/// <summary>
		/// Gets or sets the specific character set.
		/// </summary>
		/// <value>The specific character set.</value>
		public string SpecificCharacterSet
		{
			get { return DicomAttributeProvider[DicomTags.SpecificCharacterSet].GetString(0, String.Empty); }
			set { DicomAttributeProvider[DicomTags.SpecificCharacterSet].SetString(0, value); }
		}

		/// <summary>
		/// Gets or sets the Retrieve AE Title.
		/// </summary>
		/// <value>The Retrieve AE Title.</value>
		public string RetrieveAeTitle
		{
			get { return DicomAttributeProvider[DicomTags.RetrieveAeTitle].GetString(0, String.Empty); }
			set { DicomAttributeProvider[DicomTags.RetrieveAeTitle].SetString(0, value); }
		}

		/// <summary>
		/// Gets or sets the Storage Media Fileset Id.
		/// </summary>
		/// <value>The media Fileset Id.</value>
		public string StorageMediaFileSetId
		{
			get { return DicomAttributeProvider[DicomTags.StorageMediaFileSetId].GetString(0, String.Empty); }
			set { DicomAttributeProvider[DicomTags.StorageMediaFileSetId].SetString(0, value); }
		}

		/// <summary>
		/// Gets or sets the Storage Media Fileset Uid.
		/// </summary>
		/// <value>The media Fileset Uid.</value>
		public string StorageMediaFileSetUid
		{
			get { return DicomAttributeProvider[DicomTags.StorageMediaFileSetUid].GetString(0, String.Empty); }
			set { DicomAttributeProvider[DicomTags.StorageMediaFileSetUid].SetString(0, value); }
		}

		/// <summary>
		/// Gets or sets the query retrieve level.
		/// </summary>
		/// <value>The query retrieve level.</value>
		public QueryRetrieveLevel QueryRetrieveLevel
		{
			get
			{
				if (!DicomAttributeProvider[DicomTags.QueryRetrieveLevel].IsEmpty)
				{
					try
					{
						return (QueryRetrieveLevel)Enum.Parse(typeof(QueryRetrieveLevel), DicomAttributeProvider[DicomTags.QueryRetrieveLevel].GetString(0, QueryRetrieveLevel.None.ToString()), true);
					}
					catch (Exception)
					{
						return QueryRetrieveLevel.None;
					}
				}
				return QueryRetrieveLevel.None;

			}
			set
			{
				SetAttributeFromEnum(DicomAttributeProvider[DicomTags.QueryRetrieveLevel], value);
			}
		}

		/// <summary>
		/// Gets or sets the Instance Availability
		/// </summary>
		public InstanceAvailability InstanceAvailability
		{
			get
			{
				if (!DicomAttributeProvider[DicomTags.InstanceAvailability].IsEmpty)
				{
					try
					{
						return (InstanceAvailability)Enum.Parse(typeof(InstanceAvailability), DicomAttributeProvider[DicomTags.InstanceAvailability].GetString(0, InstanceAvailability.Unknown.ToString()), true);
					}
					catch (Exception)
					{
						return InstanceAvailability.Unknown;
					}
				}
				return InstanceAvailability.Unknown;
			}
			set { SetAttributeFromEnum(DicomAttributeProvider[DicomTags.InstanceAvailability], value); }
		}
	}

	#region InstanceAvailability Enum
	/// <summary>
	/// <see cref="DicomTags.InstanceAvailability"/>
	/// </summary>
	public enum InstanceAvailability
	{
		/// <summary>
		/// The instances are immediately available
		/// </summary>
		Online,
		/// <summary>
		/// The instances need to be retrieved from relatively slow media such as optical disk or tape
		/// </summary>
		Nearline,
		/// <summary>
		/// The instances need to be retrieved by manual intervention
		/// </summary>
		Offline,
		/// <summary>
		/// The instances cannot be retrieved. Note that SOP Instances that are unavailable may have an 
		/// alternate representation that is available (see section C.6.1.1.5.1).
		/// </summary>
		Unknown
	}

	#endregion

	#region QueryRetrieveLevel Enum
	/// <summary>
	/// 
	/// </summary>
	public enum QueryRetrieveLevel
	{
		/// <summary>
		/// 
		/// </summary>
		None,
		/// <summary>
		/// 
		/// </summary>
		Patient,
		/// <summary>
		/// 
		/// </summary>
		Study,
		/// <summary>
		/// 
		/// </summary>
		Series,
		/// <summary>
		/// 
		/// </summary>
		Image
	}

	#endregion
}
