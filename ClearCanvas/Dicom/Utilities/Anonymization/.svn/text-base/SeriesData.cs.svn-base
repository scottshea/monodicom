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

using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Dicom.Utilities.Anonymization
{
	/// <summary>
	/// A class containing commonly anonymized dicom series attributes.
	/// </summary>
	[Cloneable(true)]
	public class SeriesData
	{
		private string _seriesInstanceUid = "";
		private string _seriesDescription = "";
		private string _seriesNumber = "";
		private string _protocolName = "";

		/// <summary>
		/// Constructor.
		/// </summary>
		public SeriesData()
		{
		}

		/// <summary>
		/// Gets or sets the series description.
		/// </summary>
		[DicomField(DicomTags.SeriesDescription)] 
		public string SeriesDescription
		{
			get { return _seriesDescription; }
			set { _seriesDescription = value ?? ""; }
		}

		/// <summary>
		/// Gets or sets the series number.
		/// </summary>
		[DicomField(DicomTags.SeriesNumber)] 
		public string SeriesNumber
		{
			get { return _seriesNumber; }
			set { _seriesNumber = value ?? ""; }
		}

		/// <summary>
		/// Gets or sets the protocol name.
		/// </summary>
		[DicomField(DicomTags.ProtocolName)]
		public string ProtocolName
		{
			get { return _protocolName; }
			set { _protocolName = value ?? ""; }
		}

		internal string SeriesInstanceUid
		{
			get { return _seriesInstanceUid; }
			set { _seriesInstanceUid = value ?? ""; }
		}

		internal void LoadFrom(DicomFile file)
		{
			file.DataSet.LoadDicomFields(this);
			this.SeriesInstanceUid = file.DataSet[DicomTags.SeriesInstanceUid];
		}

		internal void SaveTo(DicomFile file)
		{
			file.DataSet.SaveDicomFields(this);
			file.DataSet[DicomTags.SeriesInstanceUid].SetStringValue(this.SeriesInstanceUid);
		}
		
		/// <summary>
		/// Creates a deep clone of this instance.
		/// </summary>
		public SeriesData Clone()
		{
			return CloneBuilder.Clone(this) as SeriesData;
		}
	}
}