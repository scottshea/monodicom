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
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ClearCanvas.Dicom.ServiceModel.Query
{
	/// <summary>
	/// WCF client proxy for <see cref="IStudyRootQuery"/> services.
	/// </summary>
	public class StudyRootQueryServiceClient : ClientBase<IStudyRootQuery>, IStudyRootQuery
	{
		/// <summary>
		/// Constructor - uses default configuration name to configure endpoint and bindings.
		/// </summary>
		public StudyRootQueryServiceClient()
		{
		}

		/// <summary>
		/// Constructor - uses input configuration name to configure endpoint and bindings.
		/// </summary>
		public StudyRootQueryServiceClient(string endpointConfigurationName)
			: base(endpointConfigurationName)
		{
		}

		/// <summary>
		/// Constructor - uses input endpoint and binding.
		/// </summary>
		public StudyRootQueryServiceClient(Binding binding, EndpointAddress remoteAddress)
			: base(binding, remoteAddress)
		{
		}

		/// <summary>
		/// Constructor - uses input endpoint, loads bindings from given configuration name.
		/// </summary>
		public StudyRootQueryServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
			: base(endpointConfigurationName, remoteAddress)
		{
		}

		#region IStudyRootQuery Members

		/// <summary>
		/// Performs a STUDY level query.
		/// </summary>
		/// <exception cref="FaultException{DataValidationFault}">Thrown when some part of the data in the request is poorly formatted.</exception>
		/// <exception cref="FaultException{QueryFailedFault}">Thrown when the query fails.</exception>
		public IList<StudyRootStudyIdentifier> StudyQuery(StudyRootStudyIdentifier queryCriteria)
		{
			return base.Channel.StudyQuery(queryCriteria);
		}

		/// <summary>
		/// Performs a SERIES level query.
		/// </summary>
		/// <exception cref="FaultException{DataValidationFault}">Thrown when some part of the data in the request is poorly formatted.</exception>
		/// <exception cref="FaultException{QueryFailedFault}">Thrown when the query fails.</exception>
		public IList<SeriesIdentifier> SeriesQuery(SeriesIdentifier queryCriteria)
		{
			return base.Channel.SeriesQuery(queryCriteria);
		}

		/// <summary>
		/// Performs an IMAGE level query.
		/// </summary>
		/// <exception cref="FaultException{DataValidationFault}">Thrown when some part of the data in the request is poorly formatted.</exception>
		/// <exception cref="FaultException{QueryFailedFault}">Thrown when the query fails.</exception>
		public IList<ImageIdentifier> ImageQuery(ImageIdentifier queryCriteria)
		{
			return base.Channel.ImageQuery(queryCriteria);
		}

		#endregion
	}
}
