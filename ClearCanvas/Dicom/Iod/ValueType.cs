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

namespace ClearCanvas.Dicom.Iod
{
	/// <summary>
	/// Enumerated values for the <see cref="DicomTags.ValueType"/> attribute describing the type of content encoded in a Content Item.
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.17.3 (Table C.17-5)</remarks>
	public enum ValueType
	{
		/// <summary>
		/// TEXT. Free text, narrative description of unlimited length. May also be used to provide a label or identifier value.
		/// </summary>
		Text,

		/// <summary>
		/// NUM. Numeric value fully qualified by coded representation of the measurement name and unit of measurement.
		/// </summary>
		Num,

		/// <summary>
		/// CODE. Categorical coded value. Representation of nominal or non-numeric ordinal values.
		/// </summary>
		Code,

		/// <summary>
		/// DATETIME. Date and time of occurrence of the type of event denoted by the Concept Name.
		/// </summary>
		DateTime,

		/// <summary>
		/// DATE. Date of occurrence of the type of event denoted by the Concept Name.
		/// </summary>
		Date,

		/// <summary>
		/// TIME. Time of occurrence of the type of event denoted by the Concept Name.
		/// </summary>
		Time,

		/// <summary>
		/// UIDREF. Unique Identifier (UID) of the entity identified by the Concept Name.
		/// </summary>
		UidRef,

		/// <summary>
		/// PNAME. Person name of the person whose role is described by the Concept Name.
		/// </summary>
		PName,

		/// <summary>
		/// COMPOSITE. A reference to one Composite SOP Instance which is not an Image or Waveform.
		/// </summary>
		Composite,

		/// <summary>
		/// IMAGE. A reference to one Image. IMAGE Content Item may convey a reference to a Softcopy Presentation State associated with the Image.
		/// </summary>
		Image,

		/// <summary>
		/// WAVEFORM. A reference to one Waveform.
		/// </summary>
		Waveform,

		/// <summary>
		/// SCOORD. Spatial coordinates of a geometric region of interest in the DICOM image coordinate system. The IMAGE Content Item from which spatial coordinates are selected is denoted by a SELECTED FROM relationship.
		/// </summary>
		SCoord,

		/// <summary>
		/// TCOORD. Temporal Coordinates (i.e. time or eventbased coordinates) of a region of interest in the DICOM waveform coordinate system. The WAVEFORM or IMAGE or SCOORD Content Item from which Temporal Coordinates are selected is denoted by a SELECTED FROM relationship.
		/// </summary>
		TCoord,

		/// <summary>
		/// CONTAINER. Groups Content Items and defines the heading or category of observation that applies to that content. The heading describes the content of the CONTAINER Content Item and may map to a document section heading in a printed or displayed document.
		/// </summary>
		Container,

		/// <summary>
		/// Represents the null value.
		/// </summary>
		None
	}
}