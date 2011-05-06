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
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Common.Configuration
{
	/// <summary>
	/// Enum defining the scope of a setting.
	/// </summary>
    [Serializable]
    public enum SettingScope
    {
        /// <summary>
        /// Indicates that a setting has application scope.
        /// </summary>
		Application,

		/// <summary>
		/// Indicates that a setting has user scope.
		/// </summary>
        User
    }

    /// <summary>
    /// Utility class for reading meta-data associated with a settings class
    /// (a subclass of <see cref="SettingsBase"/>).
    /// </summary>
    public static class SettingsClassMetaDataReader
    {
        /// <summary>
        /// Obtains the version of the settings class, which is always the version of the assembly
        /// in which the class is contained.
        /// </summary>
        public static Version GetVersion(Type settingsClass)
        {
            return settingsClass.Assembly.GetName().Version;
        }

        /// <summary>
        /// Obtains the name of the settings group, which is always the full name of the settings class.
        /// </summary>
        public static string GetGroupName(Type settingsClass)
        {
            return settingsClass.FullName;
        }

        /// <summary>
        /// Obtains the settings group description from the <see cref="SettingsGroupDescriptionAttribute"/>.
        /// </summary>
        public static string GetGroupDescription(Type settingsClass)
        {
            SettingsGroupDescriptionAttribute a = CollectionUtils.FirstElement<SettingsGroupDescriptionAttribute>(
                settingsClass.GetCustomAttributes(typeof(SettingsGroupDescriptionAttribute), false));

            return a == null ? "" : a.Description;
        }

        /// <summary>
        /// Obtains the collection of properties that represent settings.
        /// </summary>
        public static ICollection<PropertyInfo> GetSettingsProperties(Type settingsClass)
        {
            return CollectionUtils.Select(settingsClass.GetProperties(),
                delegate(PropertyInfo property) { return IsUserScoped(property) || IsAppScoped(property); });
        }

        /// <summary>
        /// Returns true if the specified settings class has any settings that are user-scoped.
        /// </summary>
        /// <param name="settingsClass"></param>
        /// <returns></returns>
        public static bool HasUserScopedSettings(Type settingsClass)
        {
            return CollectionUtils.Contains(GetSettingsProperties(settingsClass),
                delegate(PropertyInfo p) { return IsUserScoped(p); });
        }

        /// <summary>
        /// Returns true if the specified settings class has any settings that are application-scoped.
        /// </summary>
        /// <param name="settingsClass"></param>
        /// <returns></returns>
        public static bool HasAppScopedSettings(Type settingsClass)
        {
            return CollectionUtils.Contains(GetSettingsProperties(settingsClass),
                delegate(PropertyInfo p) { return IsAppScoped(p); });
        }

		/// <summary>
		/// Obtains the default value of a setting from the <see cref="DefaultSettingValueAttribute"/>.
		/// </summary>
		/// <remarks>
		/// If translate is true, and the value is the name of an embedded resource, it is automatically translated.
		/// </remarks>
		public static string GetDefaultValue(PropertyInfo property, bool translate)
		{
			DefaultSettingValueAttribute a = CollectionUtils.FirstElement<DefaultSettingValueAttribute>(
				property.GetCustomAttributes(typeof(DefaultSettingValueAttribute), false));

			if (a == null)
				return "";

			if (!translate)
				return a.Value;

			return TranslateDefaultValue(property.ReflectedType, a.Value);
		}
		
		/// <summary>
        /// Obtains the default value of a setting from the <see cref="DefaultSettingValueAttribute"/>.
        /// </summary>
        /// <remarks>
		/// If the value is the name of an embedded resource, it is automatically translated.
		/// </remarks>
        public static string GetDefaultValue(PropertyInfo property)
        {
			return GetDefaultValue(property, true);
        }

        /// <summary>
        /// Translates the default value for a settings class given the raw value.
        /// </summary>
        /// <remarks>
        /// If the specified raw value is the name of an embedded resource (embedded in the same
        /// assembly as the specified settings class), the contents of the resource are returned.
        /// Otherwise, the raw value is simply returned.
		/// </remarks>
        public static string TranslateDefaultValue(Type settingsClass, string rawValue)
        {
			// short circuit if nothing translatable
			if (string.IsNullOrEmpty(rawValue))
				return rawValue;

            // does the raw value look like it could be an embedded resource?
            if (Regex.IsMatch(rawValue, @"^([\w]+\.)+\w+$"))
            {
                try
                {
                    // try to open the resource
                    IResourceResolver resolver = new ResourceResolver(settingsClass.Assembly);
                    using (Stream resourceStream = resolver.OpenResource(rawValue))
                    {
                        StreamReader r = new StreamReader(resourceStream);
                        return r.ReadToEnd();
                    }
                }
                catch (MissingManifestResourceException)
                {
                    // guess it was not an embedded resource, so return the raw value
                    return rawValue;
                }
            }
            else
            {
                return rawValue;
            }
        }

        /// <summary>
        /// Obtains the setting description from the <see cref="SettingsDescriptionAttribute"/>.
        /// </summary>
        public static string GetDescription(PropertyInfo property)
        {
            SettingsDescriptionAttribute a = CollectionUtils.FirstElement<SettingsDescriptionAttribute>(
                property.GetCustomAttributes(typeof(SettingsDescriptionAttribute), false));

            return a == null ? "" : a.Description;
        }

        /// <summary>
		/// Returns a <see cref="SettingScope"/> enum describing the scope of the property.
        /// </summary>
        public static SettingScope GetScope(PropertyInfo property)
        {
            if(IsAppScoped(property))
                return SettingScope.Application;
            if(IsUserScoped(property))
                return SettingScope.User;

			throw new Exception(SR.MessageSettingsScopeNotDefined);
        }

        /// <summary>
        /// Returns the name of the settings property.
        /// </summary>
        public static string GetName(PropertyInfo property)
        {
            return property.Name;
        }

        /// <summary>
        /// Returns the <see cref="Type"/> of the settings property.
        /// </summary>
        public static Type GetType(PropertyInfo property)
        {
            return property.PropertyType;
        }

        /// <summary>
        /// Returns true if the property is decorated with a <see cref="UserScopedSettingAttribute"/>.
        /// </summary>
        public static bool IsUserScoped(PropertyInfo property)
        {
            UserScopedSettingAttribute a = CollectionUtils.FirstElement<UserScopedSettingAttribute>(
                property.GetCustomAttributes(typeof(UserScopedSettingAttribute), false));

            return a != null;
        }

        /// <summary>
        /// Returns true if the property is decorated with a <see cref="ApplicationScopedSettingAttribute"/>.
        /// </summary>
        public static bool IsAppScoped(PropertyInfo property)
        {
            ApplicationScopedSettingAttribute a = CollectionUtils.FirstElement<ApplicationScopedSettingAttribute>(
                property.GetCustomAttributes(typeof(ApplicationScopedSettingAttribute), false));

            return a != null;
        }
    }
}
