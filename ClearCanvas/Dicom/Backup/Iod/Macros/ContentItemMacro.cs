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

namespace ClearCanvas.Dicom.Iod.Macros
{
	/// <summary>
	/// Content Item Macro
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section 10.2 (Table 10-2)</remarks>
	public class ContentItemMacro : SequenceIodBase
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ContentItemMacro"/> class.
		/// </summary>
		public ContentItemMacro() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="ContentItemMacro"/> class.
		/// </summary>
		/// <param name="dicomSequenceItem">The dicom sequence item.</param>
		public ContentItemMacro(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the type of the value.
		/// </summary>
		/// <value>The type of the value.</value>
		public ContentItemValueType ValueType
		{
			get { return ParseEnum<ContentItemValueType>(base.DicomAttributeProvider[DicomTags.ValueType].GetString(0, String.Empty), ContentItemValueType.None); }
			set { SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.ValueType], value); }
		}

		public SequenceIodList<CodeSequenceMacro> ConceptNameCodeSequenceList
		{
			get { return new SequenceIodList<CodeSequenceMacro>(base.DicomAttributeProvider[DicomTags.ConceptNameCodeSequence] as DicomAttributeSQ); }
		}

		/// <summary>
		/// Datetime value for this name-value Item. Required if Value Type (0040,A040) is DATETIME.
		/// </summary>
		/// <value>The datetime.</value>
		public DateTime? Datetime
		{
			get { return base.DicomAttributeProvider[DicomTags.Datetime].GetDateTime(0); }

			set
			{
				if (value.HasValue)
					base.DicomAttributeProvider[DicomTags.Datetime].SetDateTime(0, value.Value);
				else
					base.DicomAttributeProvider[DicomTags.Datetime].SetNullValue();
			}
		}

		/// <summary>
		/// Date value for this name-value Item. Required if Value Type (0040,A040) is DATE.
		/// </summary>
		/// <value>The date.</value>
		public DateTime? Date
		{
			get { return base.DicomAttributeProvider[DicomTags.Date].GetDateTime(0); }

			set { base.DicomAttributeProvider[DicomTags.Date].SetDateTime(0, value); }
		}

		/// <summary>
		/// Time value for this name-value Item.  Required if Value Type (0040,A040) is TIME.
		/// </summary>
		/// <value>The time.</value>
		public DateTime? Time
		{
			get { return base.DicomAttributeProvider[DicomTags.Time].GetDateTime(0); }

			set { base.DicomAttributeProvider[DicomTags.Time].SetDateTime(0, value); }
		}

		/// <summary>
		/// Person name value for this name-value Item.  Required if Value Type (0040,A040) is PNAME.
		/// </summary>
		/// <value>The name of the person.</value>
		public PersonName PersonName
		{
			get { return new PersonName(base.DicomAttributeProvider[DicomTags.PersonName].GetString(0, String.Empty)); }
			set { base.DicomAttributeProvider[DicomTags.PersonName].SetString(0, value.ToString()); }
		}

		/// <summary>
		/// UID value for this name-value Item.  Required if Value Type (0040,A040) is UIDREF.
		/// </summary>
		/// <value>The uid.</value>
		public string Uid
		{
			get { return base.DicomAttributeProvider[DicomTags.Uid].GetString(0, String.Empty); }
			set { base.DicomAttributeProvider[DicomTags.Uid].SetString(0, value); }
		}

		/// <summary>
		/// Text value for this name-value Item.  Required if Value Type (0040,A040) is TEXT.
		/// </summary>
		/// <value>The text value.</value>
		public string TextValue
		{
			get { return base.DicomAttributeProvider[DicomTags.TextValue].GetString(0, String.Empty); }
			set { base.DicomAttributeProvider[DicomTags.TextValue].SetString(0, value); }
		}

		/// <summary>
		/// Coded concept value of this name-value Item.  Required if Value Type (0040,A040) is CODE.
		/// </summary>
		/// <value>The concept code sequence list.</value>
		public SequenceIodList<CodeSequenceMacro> ConceptCodeSequenceList
		{
			get { return new SequenceIodList<CodeSequenceMacro>(base.DicomAttributeProvider[DicomTags.ConceptCodeSequence] as DicomAttributeSQ); }
		}

		/// <summary>
		/// Numeric value for this name-value Item. Required if Value Type (0040,A040) is NUMERIC.
		/// </summary>
		/// <value>The numeric value.</value>
		public float NumericValue
		{
			get { return base.DicomAttributeProvider[DicomTags.NumericValue].GetFloat32(0, 0.0F); }
			set { base.DicomAttributeProvider[DicomTags.NumericValue].SetFloat32(0, value); }
		}

		/// <summary>
		/// Units of measurement for a numeric value in this namevalue Item.  Required if Value Type (0040,A040) is NUMERIC.
		/// </summary>
		/// <value>The measurement units code sequence list.</value>
		public SequenceIodList<CodeSequenceMacro> MeasurementUnitsCodeSequenceList
		{
			get { return new SequenceIodList<CodeSequenceMacro>(base.DicomAttributeProvider[DicomTags.MeasurementUnitsCodeSequence] as DicomAttributeSQ); }
		}

		#endregion
	}

	#region ContentItemValueType Enum

	/// <summary>
	/// 
	/// </summary>
	public enum ContentItemValueType
	{
		/// <summary>
		/// 
		/// </summary>
		None,
		/// <summary>
		/// 
		/// </summary>
		DateTime,
		/// <summary>
		/// 
		/// </summary>
		Date,
		/// <summary>
		/// 
		/// </summary>
		Time,
		/// <summary>
		/// 
		/// </summary>
		PName,
		/// <summary>
		/// 
		/// </summary>
		UidRef,
		/// <summary>
		/// 
		/// </summary>
		Text,
		/// <summary>
		/// 
		/// </summary>
		Code,
		/// <summary>
		/// 
		/// </summary>
		Numeric
	}

	#endregion
}