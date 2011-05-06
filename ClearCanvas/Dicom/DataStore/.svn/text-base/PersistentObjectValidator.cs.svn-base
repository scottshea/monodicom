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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Mapping;
using NHibernate.Type;

namespace ClearCanvas.Dicom.DataStore
{
	internal class PersistentObjectValidator
	{
		private readonly Configuration _configuration;
		private Dictionary<Type, Dictionary<string, PropertyInfo>> _persistentTypesPropertyInfo = null;

		public PersistentObjectValidator(Configuration configuration)
		{
			_configuration = configuration;
		}

		private IDictionary<string, PropertyInfo> GetPropertyInfo(Type type)
		{
			if (_persistentTypesPropertyInfo == null)
				_persistentTypesPropertyInfo = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

			if (_persistentTypesPropertyInfo.ContainsKey(type))
			{
				return _persistentTypesPropertyInfo[type];
			}
			else
			{
				BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
				PropertyInfo[] propertyInfo = type.GetProperties(flags);
				Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>();
				foreach (PropertyInfo info in propertyInfo)
					properties[info.Name] = info;

				_persistentTypesPropertyInfo[type] = properties;
				return properties;
			}
		}

		private static void ValidateStringValue(Column column, object sourceObject, PropertyInfo info)
		{
			if (info.PropertyType == typeof(string))
			{
				string value = info.GetValue(sourceObject, null) as string;
				if (value == null && !column.IsNullable)
					throw new DataValidationException(String.Format("The specified column is not nullable ({0}).", column.Name));
				else if (String.IsNullOrEmpty(value) && column.IsUnique)
					throw new DataValidationException(String.Format("The specified column value must be unique ({0}).", column.Name));
				else if (value != null && value.Length > column.Length)
					throw new DataValidationException(String.Format("The specified value exceeds the maximum length (column={0}:{1}, value='{2}').", column.Name, column.Length, value));
			}
		}
		public void ValidatePersistentObject(object obj)
		{
			PersistentClass pclass = _configuration.GetClassMapping(obj.GetType());
			IDictionary<string, PropertyInfo> pclassProperties = GetPropertyInfo(obj.GetType());

			foreach (Property property in pclass.PropertyClosureIterator)
			{
				IEnumerable columns = property.ColumnIterator;

				if (property.Type.IsComponentType)
				{
					IDictionary<string, PropertyInfo> properties = GetPropertyInfo(property.Type.ReturnedClass);
					object sourceObject = pclassProperties[property.Name].GetValue(obj, null);
					if (sourceObject == null)
						continue;

					int i = 0;
					ComponentType componentType = (ComponentType)property.Type;

					foreach (Column column in columns)
						ValidateStringValue(column, sourceObject, properties[componentType.PropertyNames[i++]]);
				}
				else
				{
					//There is only one column, but we have to iterate.
					foreach (Column column in columns)
						ValidateStringValue(column, obj, pclassProperties[property.Name]);
				}
			}
		}
	}
}
