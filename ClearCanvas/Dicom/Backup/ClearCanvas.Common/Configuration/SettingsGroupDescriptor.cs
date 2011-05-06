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
using System.Configuration;
using ClearCanvas.Common.Utilities;
using System.Runtime.Serialization;

namespace ClearCanvas.Common.Configuration
{
    /// <summary>
    /// Describes a settings group.
    /// </summary>
    [DataContract]
    public class SettingsGroupDescriptor : IEquatable<SettingsGroupDescriptor>
    {
        /// <summary>
        /// Returns a list of <see cref="SettingsGroupDescriptor"/> objects describing each settings class
        /// that exists in the installed plugin base.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If <param name="excludeLocalSettingsGroups"/> is true, this method only returns settings classes 
        /// that use the <see cref="StandardSettingsProvider"/> for persistence.
		/// </para>
		/// <para>
		/// This method is thread-safe.
		/// </para>
        /// </remarks>
        public static List<SettingsGroupDescriptor> ListInstalledSettingsGroups(bool excludeLocalSettingsGroups)
        {
            List<SettingsGroupDescriptor> groups = new List<SettingsGroupDescriptor>();

            foreach (PluginInfo plugin in Platform.PluginManager.Plugins)
            {
                foreach (Type t in plugin.Assembly.GetTypes())
                {
                    if (t.IsSubclassOf(typeof(ApplicationSettingsBase)) && !t.IsAbstract)
                    {
                        if (excludeLocalSettingsGroups)
                        {
                            bool isStandard = AttributeUtils.HasAttribute<SettingsProviderAttribute>(t, false,
                                                   delegate(SettingsProviderAttribute a)
                                                   {
                                                       return a.ProviderTypeName == typeof(StandardSettingsProvider).AssemblyQualifiedName;
                                                   });

                            // exclude non-standard settings groups
                            if (!isStandard)
                                continue;
                        }
                        groups.Add(new SettingsGroupDescriptor(t));
                    }
                }
            }

            return groups;
        }

        private string _name;
        private Version _version;
        private string _description;
        private string _assemblyQualifiedTypeName;
        private bool _hasUserScopedSettings;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SettingsGroupDescriptor(string name, Version version, string description, string assemblyQualifiedTypeName,
            bool hasUserScopedSettings)
        {
            _name = name;
            _version = version;
            _description = description;
            _assemblyQualifiedTypeName = assemblyQualifiedTypeName;
            _hasUserScopedSettings = hasUserScopedSettings;
        }

		/// <summary>
		/// Constructor.
		/// </summary>
        public SettingsGroupDescriptor(Type settingsClass)
        {
            _name = SettingsClassMetaDataReader.GetGroupName(settingsClass);
            _version = SettingsClassMetaDataReader.GetVersion(settingsClass);
            _description = SettingsClassMetaDataReader.GetGroupDescription(settingsClass);
		    _hasUserScopedSettings = SettingsClassMetaDataReader.HasUserScopedSettings(settingsClass);
            _assemblyQualifiedTypeName = GetSafeClassName(settingsClass);
        }

        /// <summary>
        /// Gets the name of the settings group.
        /// </summary>
		[DataMember]
		public string Name
        {
            get { return _name; }
			private set { _name = value; }
        }

        /// <summary>
        /// Gets the version of the settings group.
        /// </summary>
		[DataMember]
		public Version Version
        {
            get { return _version; }
			private set { _version = value; }
		}

        /// <summary>
        /// Gets the description of the settings group.
        /// </summary>
		[DataMember]
		public string Description
        {
            get { return _description; }
			private set { _description = value; }
		}

        /// <summary>
        /// Gets a value indicating whether this settings class has user-scoped settings.
        /// </summary>
		[DataMember]
		public bool HasUserScopedSettings
        {
            get { return _hasUserScopedSettings; }
			private set { _hasUserScopedSettings = value; }
		}

        /// <summary>
        /// Gets the assembly-qualified type name of the class that implements the settings group.
        /// </summary>
		[DataMember]
		public string AssemblyQualifiedTypeName
        {
            get { return _assemblyQualifiedTypeName; }
			private set { _assemblyQualifiedTypeName = value; }
		}

        /// <summary>
        /// Settings groups are considered equal if they have the same name and version.
        /// </summary>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as SettingsGroupDescriptor);
        }

		/// <summary>
		/// Gets the hash code for this object.
		/// </summary>
        public override int GetHashCode()
        {
            return _name.GetHashCode() ^ _version.GetHashCode();
        }

        #region IEquatable<SettingsGroupDescriptor> Members

        /// <summary>
        /// Settings groups are considered equal if they have the same name and version.
        /// </summary>
        public bool Equals(SettingsGroupDescriptor other)
        {
            return other != null && this._name == other._name && this._version == other._version;
        }

        #endregion

        /// <summary>
        /// Gets the assembly qualified name of the type, but without all the version and culture info.
        /// </summary>
        /// <param name="settingsClass"></param>
        /// <returns></returns>
        private static string GetSafeClassName(Type settingsClass)
        {
            return string.Format("{0}, {1}", settingsClass.FullName, settingsClass.Assembly.GetName().Name);
        }

    }
}
