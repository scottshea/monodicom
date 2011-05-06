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
using System.Reflection;

namespace ClearCanvas.Dicom
{

	/// <summary>
	/// An attribute that describes the name encoded in a Directory record.
	/// </summary>
	public class DirectoryRecordTypeAttribute : Attribute
	{
		private string _name;
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		public DirectoryRecordTypeAttribute(string name)
		{
			_name = name;
		}
	}

	/// <summary>
	/// A list of DICOM Directory record types.
	/// </summary>
	public enum DirectoryRecordType
	{
		[DirectoryRecordType("PATIENT")]
		Patient,
		[DirectoryRecordType("STUDY")]
		Study,
		[DirectoryRecordType("SERIES")]
		Series,
		[DirectoryRecordType("IMAGE")]
		Image,
		[DirectoryRecordType("RT DOSE")]
		RtDose,
		[DirectoryRecordType("RT STRUCTURE SET")]
		RtStructureSet,
		[DirectoryRecordType("RT PLAN")]
		RtPlan,
		[DirectoryRecordType("RT TREAT RECORD")]
		RtTreatRecord,
		[DirectoryRecordType("PRESENTATION")]
		Presentation,
		[DirectoryRecordType("WAVEFORM")]
		Waveform,
		[DirectoryRecordType("SR DOCUMENT")]
		SrDocument,
		[DirectoryRecordType("KEY OBJECT DOC")]
		KeyObjectDoc,
		[DirectoryRecordType("SPECTROSCOPY")]
		Spectroscopy,
		[DirectoryRecordType("RAW DATA")]
		RawData,
		[DirectoryRecordType("REGISTRATION")]
		Registration,
		[DirectoryRecordType("FIDUCIAL")]
		Fiducial,
		[DirectoryRecordType("HANGING PROTOCOL")]
		HangingProtocol,
		[DirectoryRecordType("ENCAP DOC")]
		EncapDoc,
		[DirectoryRecordType("HL7 STRUC DOC")]
		Hl7StrucDoc,
		[DirectoryRecordType("VALUE MAP")]
		ValueMap,
		[DirectoryRecordType("STEREOMETRIC")]
		Stereometric,
		[DirectoryRecordType("PRIVATE")]
		Private,
	}

	/// <summary>
	/// Dictionary for converting betwen <see cref="DirectoryRecordType"/> and string description for the type
	/// used in DICOM Directory Records.
	/// </summary>
	internal static class DirectoryRecordTypeDictionary
	{
		private static readonly Dictionary<string, DirectoryRecordType> _nameList = new Dictionary<string, DirectoryRecordType>();
		private static readonly Dictionary<DirectoryRecordType, string> _typeList = new Dictionary<DirectoryRecordType, string>();

		static DirectoryRecordTypeDictionary()
		{
			Type enumType = typeof(DirectoryRecordType);

			FieldInfo[] infos = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
			foreach (FieldInfo fi in infos)
			{
				DirectoryRecordTypeAttribute attrib =
					(DirectoryRecordTypeAttribute)fi.GetCustomAttributes(typeof(DirectoryRecordTypeAttribute), true)[0];

				_nameList.Add(attrib.Name, (DirectoryRecordType)Enum.Parse(typeof(DirectoryRecordType), fi.Name));
				_typeList.Add((DirectoryRecordType)Enum.Parse(typeof(DirectoryRecordType), fi.Name), attrib.Name);
			}
		}

		/// <summary>
		/// Get the <see cref="DirectoryRecordType"/> for a string.
		/// </summary>
		/// <param name="val"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		static public bool TryGetType(string val, out DirectoryRecordType type)
		{
			return _nameList.TryGetValue(val, out type);
		}

		/// <summary>
		/// Get the name for a <see cref="DirectoryRecordType"/>
		/// </summary>
		/// <param name="type"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		static public bool TryGetName(DirectoryRecordType type, out string val)
		{
			return _typeList.TryGetValue(type, out val);
		}
	}

	/// <summary>
	/// A class representing a DICOMDIR Directory Record
	/// </summary>
	public class DirectoryRecordSequenceItem : DicomSequenceItem
	{
		#region Private Members
		private DirectoryRecordSequenceItem _lowerLevelDirectoryDirectoryRecord;
		private DirectoryRecordSequenceItem _nextDirectoryRecord;
		private uint _offset;
		#endregion

		public DirectoryRecordCollection LowerLevelDirectoryRecordCollection
		{
			get { return new DirectoryRecordCollection(LowerLevelDirectoryRecord); }
		}
		/// <summary>
		/// The first directory record in the level below the current record.
		/// </summary>
		public DirectoryRecordSequenceItem LowerLevelDirectoryRecord
		{
			get { return _lowerLevelDirectoryDirectoryRecord; }
			set { _lowerLevelDirectoryDirectoryRecord = value; }
		}

		/// <summary>
		/// The next directory record at the current level.
		/// </summary>
		public DirectoryRecordSequenceItem NextDirectoryRecord
		{
			get { return _nextDirectoryRecord; }
			set { _nextDirectoryRecord = value; }
		}

		/// <summary>
		/// An offset to the directory record.  Used for reading and writing.
		/// </summary>
		internal uint Offset
		{
			get { return _offset; }
			set { _offset = value; }
		}

		/// <summary>
		/// The <see cref="DirectoryRecordType"/> associated withe the Directory record.
		/// </summary>
		/// <remarks>
		/// If the Directory Record Type is unknown to the code, it will return that the 
		/// record is a <see cref="DirectoryRecordType.PRIVATE"/> record.
		/// </remarks>
		public DirectoryRecordType DirectoryRecordType
		{
			get
			{
				string recordType = base[DicomTags.DirectoryRecordType].GetString(0, String.Empty);
				DirectoryRecordType type;
				if (DirectoryRecordTypeDictionary.TryGetType(recordType, out type))
					return type;

				return DirectoryRecordType.Private;
			}
		}

		/// <summary>
		/// Override.
		/// </summary>
		/// <returns>A string description of the Directory Record.</returns>
		public override string ToString()
		{
			string toString;
			if (DirectoryRecordType == DirectoryRecordType.Series)
				toString = base[DicomTags.SeriesInstanceUid].GetString(0, string.Empty);
			else if (DirectoryRecordType == DirectoryRecordType.Study)
				toString = base[DicomTags.StudyInstanceUid].GetString(0, string.Empty);
			else if (DirectoryRecordType == DirectoryRecordType.Patient)
				toString = base[DicomTags.PatientId] + " " + base[DicomTags.PatientsName];
			else
				toString = base[DicomTags.ReferencedSopInstanceUidInFile].GetString(0, string.Empty);

			string recordType;
			DirectoryRecordTypeDictionary.TryGetName(DirectoryRecordType, out recordType);

			return recordType + " " + toString;
		}
	}
}
