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

namespace ClearCanvas.Dicom.ServiceModel.Query
{
	public interface IIdentifier
	{
		[DicomField(DicomTags.SpecificCharacterSet)]
		string SpecificCharacterSet { get; }

		[DicomField(DicomTags.RetrieveAeTitle)]
		string RetrieveAeTitle { get; }

		[DicomField(DicomTags.InstanceAvailability)]
		string InstanceAvailability { get; }
	}

	/// <summary>
	/// Base class for Dicom query Identifiers.
	/// </summary>
	[DataContract(Namespace = QueryNamespace.Value)]
	public abstract class Identifier : IIdentifier
	{
		#region Private Fields

		private string _specificCharacterSet = "";
		private string _retrieveAeTitle = "";
		private string _instanceAvailability = "";

		#endregion

		#region Internal Constructors

		internal Identifier()
		{
		}

		internal Identifier(IIdentifier other)
		{
			SpecificCharacterSet = other.SpecificCharacterSet;
			InstanceAvailability = other.InstanceAvailability;
			RetrieveAeTitle = other.RetrieveAeTitle;
		}

		internal Identifier(DicomAttributeCollection attributes)
		{
			Initialize(attributes);
		}

		#endregion

		internal void Initialize(DicomAttributeCollection attributes)
		{
			attributes.LoadDicomFields(this);
		}

		#region Public Properties

		/// <summary>
		/// Gets the level of the query.
		/// </summary>
		public abstract string QueryRetrieveLevel { get; }

		/// <summary>
		/// Gets or sets the Specific Character set of the identified instance.
		/// </summary>
		[DicomField(DicomTags.SpecificCharacterSet)] //only include in rq when set explicitly.
		[DataMember(IsRequired = false)]
		public string SpecificCharacterSet
		{
			get { return _specificCharacterSet; }
			set { _specificCharacterSet = value; }
		}

		/// <summary>
		/// Gets or sets the AE Title the identified instance can be retrieved from.
		/// </summary>
		[DicomField(DicomTags.RetrieveAeTitle)]
		[DataMember(IsRequired = false)]
		public string RetrieveAeTitle
		{
			get { return _retrieveAeTitle; }
			set { _retrieveAeTitle = value; }
		}

		/// <summary>
		/// Gets or sets the availability of the identified instance.
		/// </summary>
		[DicomField(DicomTags.InstanceAvailability)]
		[DataMember(IsRequired = false)]
		public string InstanceAvailability
		{
			get { return _instanceAvailability; }
			set { _instanceAvailability = value; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Converts this object into a <see cref="DicomAttributeCollection"/>.
		/// </summary>
		public DicomAttributeCollection ToDicomAttributeCollection()
		{
			DicomAttributeCollection attributes = new DicomAttributeCollection();
			if (!string.IsNullOrEmpty(_specificCharacterSet))
				attributes.SpecificCharacterSet = _specificCharacterSet;

			attributes[DicomTags.SpecificCharacterSet] = null;
			attributes[DicomTags.QueryRetrieveLevel].SetStringValue(QueryRetrieveLevel);

			attributes.SaveDicomFields(this);

			return attributes;
		}

		/// <summary>
		/// Factory method to create an <see cref="Identifier"/> of type <typeparamref name="T"/> from
		/// the given <see cref="DicomAttributeCollection"/>.
		/// </summary>
		public static T FromDicomAttributeCollection<T>(DicomAttributeCollection attributes) where T : Identifier, new()
		{
			T identifier = new T();
			identifier.Initialize(attributes);
			return identifier;
		}

		#endregion
	}
}
