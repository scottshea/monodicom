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
using ClearCanvas.Common.Utilities;
using System.Runtime.Serialization;

namespace ClearCanvas.Common.Configuration
{
    /// <summary>
    /// Describes a settings property.
    /// </summary>
    /// <remarks>
	/// A settings property is a single property belonging to a settings group.
	/// </remarks>
	[DataContract]
    public class SettingsPropertyDescriptor
    {
		/// <summary>
		/// Returns a list of <see cref="SettingsPropertyDescriptor"/> objects describing each property belonging
		/// to a settings group.
		/// </summary>
		public static List<SettingsPropertyDescriptor> ListSettingsProperties(SettingsGroupDescriptor group)
        {
            Type settingsClass = Type.GetType(group.AssemblyQualifiedTypeName);

            return CollectionUtils.Map<PropertyInfo, SettingsPropertyDescriptor, List<SettingsPropertyDescriptor>>(
                SettingsClassMetaDataReader.GetSettingsProperties(settingsClass),
                delegate(PropertyInfo p)
                {
                    SettingsPropertyDescriptor info = new SettingsPropertyDescriptor(
                        SettingsClassMetaDataReader.GetName(p),
                        SettingsClassMetaDataReader.GetType(p).FullName,
                        SettingsClassMetaDataReader.GetDescription(p),
                        SettingsClassMetaDataReader.GetScope(p),
                        SettingsClassMetaDataReader.GetDefaultValue(p));
                    return info;
                });
        }


        private string _name;
        private string _typeName;
        private string _description;
        private SettingScope _scope;
        private string _defaultValue;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SettingsPropertyDescriptor(string name, string typeName, string description, SettingScope scope, string defaultValue)
        {
            _name = name;
            _typeName = typeName;
            _description = description;
            _scope = scope;
            _defaultValue = defaultValue;
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return _name; }
			private set { _name = value; }
        }

        /// <summary>
        /// Gets the name of the type of the property.
        /// </summary>
		[DataMember]
		public string TypeName
        {
            get { return _typeName; }
			private set { _typeName = value; }
		}

        /// <summary>
        /// Gets the description of the property.
        /// </summary>
		[DataMember]
		public string Description
        {
            get { return _description; }
			private set { _description = value; }
		}

        /// <summary>
        /// Gets the scope of the property.
        /// </summary>
		[DataMember]
		public SettingScope Scope
        {
            get { return _scope; }
			private set { _scope = value; }
		}

        /// <summary>
        /// Gets the serialized default value of the property.
        /// </summary>
		[DataMember]
		public string DefaultValue
        {
            get { return _defaultValue; }
			private set { _defaultValue = value; }
		}
    }
}
