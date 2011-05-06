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
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using NUnit.Framework;

namespace ClearCanvas.Dicom.DataStore
{
	internal class QueryablePropertyInfo
	{
		private readonly QueryablePropertyAttribute _attribute;

		public QueryablePropertyInfo(QueryablePropertyAttribute attribute, PropertyInfo property)
		{
			_attribute = attribute;
			Property = property;

			ColumnName = String.Format("{0}_", property.Name);

			ReturnProperty = property;
			if (attribute.ReturnProperty != null)
				ReturnProperty = property.DeclaringType.GetProperty(attribute.ReturnProperty) ?? Property;

			Type convertType = ReturnProperty.PropertyType;
			if (convertType.IsArray)
				convertType = convertType.GetElementType();

			ReturnPropertyConverter = TypeDescriptor.GetConverter(convertType);
			Debug.Assert(ReturnPropertyConverter.CanConvertFrom(typeof (string)) && ReturnPropertyConverter.CanConvertTo(typeof (string)),
				"The property type must be convertible to/from a string.");

			AllowListMatching = Path.ValueRepresentation == DicomVr.UIvr || Path.Equals(DicomTags.ModalitiesInStudy);
		}

		public readonly PropertyInfo Property;
		public readonly string ColumnName;

		public readonly TypeConverter ReturnPropertyConverter;

		public DicomTagPath Path
		{
			get { return _attribute.Path; }
		}

		public bool IsHigherLevelUnique
		{
			get { return _attribute.IsHigherLevelUnique; }	
		}

		public bool IsUnique
		{
			get { return _attribute.IsUnique; }
		}

		public bool IsRequired
		{
			get { return _attribute.IsRequired; }
		}

		public bool PostFilterOnly
		{
			get { return _attribute.PostFilterOnly; }
		}

		public readonly bool AllowListMatching;

		public readonly PropertyInfo ReturnProperty;

		public override string ToString()
		{
			return Path.ToString();
		}
	}

	internal class QueryablePropertyAttribute : Attribute
	{
		public QueryablePropertyAttribute(params DicomTag[] tags)
		{
			Path = new DicomTagPath(tags);
		}

		public QueryablePropertyAttribute(params uint[] tags)
		{
			Path = new DicomTagPath(tags);
		}

		public DicomTagPath Path;
		public bool IsHigherLevelUnique = false;
		public bool IsUnique = false;
		public bool IsRequired = false;
		public bool PostFilterOnly = false;

		public string ReturnProperty = null;
	}

	internal static class QueryableProperties<T>
	{
		private static readonly Dictionary<DicomTagPath, QueryablePropertyInfo> _dictionary;

		static QueryableProperties()
		{
			_dictionary = new Dictionary<DicomTagPath, QueryablePropertyInfo>();

			foreach (PropertyInfo property in typeof(T).GetProperties())
			{
				foreach (QueryablePropertyAttribute attribute in property.GetCustomAttributes(typeof(QueryablePropertyAttribute), false))
				{
					_dictionary[attribute.Path] = new QueryablePropertyInfo(attribute, property);
				}
			}
		}

		public static IEnumerable<QueryablePropertyInfo> GetProperties()
		{
			return _dictionary.Values;
		}

		public static QueryablePropertyInfo GetProperty(DicomTagPath path)
		{
				if (!_dictionary.ContainsKey(path))
					return null;

				return _dictionary[path];
		}
	}
}