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
using System.Configuration;

namespace ClearCanvas.Common.Configuration
{
    /// <summary>
    /// An extension point for <see cref="ISettingsStore"/>s.
    /// </summary>
	[ExtensionPoint]
    public sealed class SettingsStoreExtensionPoint : ExtensionPoint<ISettingsStore>
    {
    }

    /// <summary>
    /// This class is the standard settings provider that should be used by all settings classes that operate
    /// within the ClearCanvas framework.
    /// </summary>
    /// <remarks>
	/// Internally, this class will delegate the storage of settings between
	/// the local file system and an implemetation of <see cref="SettingsStoreExtensionPoint"/>,
	/// if an extension is found.  All methods on this class are thread-safe, as per MSDN guidelines.
	/// </remarks>
    public class StandardSettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {
        private string _appName;
        private SettingsProvider _sourceProvider;
        private readonly object _syncLock = new object();

		/// <summary>
		/// Constructor.
		/// </summary>
        public StandardSettingsProvider()
        {
            // according to MSDN recommendation, use the name of the executing assembly here
            _appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        }

        #region SettingsProvider overrides

		/// <summary>
		/// Gets the Application Name used to initialize the settings subsystem.
		/// </summary>
        public override string ApplicationName
        {
            get
            {
                return _appName;
            }
            set
            {
                _appName = value;
            }
        }

    	///<summary>
    	///Initializes the provider.
    	///</summary>
    	///
    	///<param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
    	///<param name="name">The friendly name of the provider.</param>
    	public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            lock (_syncLock)
            {
                // obtain a source provider
                try
                {
                    ISettingsStore ecs = (ISettingsStore)(new SettingsStoreExtensionPoint()).CreateExtension();
                    _sourceProvider = new SettingsStoreSettingsProvider(ecs);
                }
                catch (NotSupportedException)
                {
                    Platform.Log(LogLevel.Warn, SR.LogConfigurationStoreNotFound);

                    // default to LocalFileSettingsProvider as a last resort
                    _sourceProvider = new LocalFileSettingsProvider();
                }

                // init source provider
                // according to sample implementations, use the application name here
                _sourceProvider.Initialize(this.ApplicationName, config);
                base.Initialize(this.ApplicationName, config);
            }
        }

    	///<summary>
    	///Returns the collection of settings property values for the specified application instance and settings property group.
    	///</summary>
    	///
    	///<returns>
    	///A <see cref="T:System.Configuration.SettingsPropertyValueCollection"></see> containing the values for the specified settings property group.
    	///</returns>
    	///
    	///<param name="context">A <see cref="T:System.Configuration.SettingsContext"></see> describing the current application use.</param>
    	///<param name="props">A <see cref="T:System.Configuration.SettingsPropertyCollection"></see> containing the settings property group whose values are to be retrieved.</param><filterpriority>2</filterpriority>
    	public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection props)
        {
            lock (_syncLock)
            {
                Type settingsClass = (Type)context["SettingsClassType"];

                SettingsPropertyValueCollection values = _sourceProvider.GetPropertyValues(context, props);
                foreach (SettingsPropertyValue value in values)
                {
					if (value.SerializedValue == null || (value.SerializedValue is string) && ((string)value.SerializedValue) == ((string)value.Property.DefaultValue))
					{
						value.SerializedValue = SettingsClassMetaDataReader.TranslateDefaultValue(settingsClass,
							(string)value.Property.DefaultValue);
					}
                }
                return values;
            }
        }

    	///<summary>
    	///Sets the values of the specified group of property settings.
    	///</summary>
    	///
    	///<param name="context">A <see cref="T:System.Configuration.SettingsContext"></see> describing the current application usage.</param>
		///<param name="settings">A <see cref="T:System.Configuration.SettingsPropertyValueCollection"></see> representing the group of property settings to set.</param><filterpriority>2</filterpriority>
    	public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection settings)
        {
            lock (_syncLock)
            {
                _sourceProvider.SetPropertyValues(context, settings);
            }
        }

        #endregion


        #region IApplicationSettingsProvider Members

    	///<summary>
    	///Returns the value of the specified settings property for the previous version of the same application.
    	///</summary>
    	///
    	///<returns>
    	///A <see cref="T:System.Configuration.SettingsPropertyValue"></see> containing the value of the specified property setting as it was last set in the previous version of the application; or null if the setting cannot be found.
    	///</returns>
    	///
    	///<param name="context">A <see cref="T:System.Configuration.SettingsContext"></see> describing the current application usage.</param>
    	///<param name="property">The <see cref="T:System.Configuration.SettingsProperty"></see> whose value is to be returned.</param><filterpriority>2</filterpriority>
    	public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
        {
            lock (_syncLock)
            {
                if (_sourceProvider is IApplicationSettingsProvider)
                {
                    return (_sourceProvider as IApplicationSettingsProvider).GetPreviousVersion(context, property);
                }
                else
                {
                    // fail silently as per MSDN 
                    return new SettingsPropertyValue(property);
                }
            }
        }

    	///<summary>
    	///Resets the application settings associated with the specified application to their default values.
    	///</summary>
    	///
    	///<param name="context">A <see cref="T:System.Configuration.SettingsContext"></see> describing the current application usage.</param><filterpriority>2</filterpriority>
    	public void Reset(SettingsContext context)
        {
            lock (_syncLock)
            {
                if (_sourceProvider is IApplicationSettingsProvider)
                {
                    (_sourceProvider as IApplicationSettingsProvider).Reset(context);
                }
                else
                {
                    // fail silently as per MSDN 
                }
            }
        }

    	///<summary>
    	///Indicates to the provider that the application has been upgraded. This offers the provider an opportunity to upgrade its stored settings as appropriate.
    	///</summary>
    	///
    	///<param name="properties">A <see cref="T:System.Configuration.SettingsPropertyCollection"></see> containing the settings property group whose values are to be retrieved.</param>
    	///<param name="context">A <see cref="T:System.Configuration.SettingsContext"></see> describing the current application usage.</param><filterpriority>2</filterpriority>
    	public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
        {
            lock (_syncLock)
            {
                if (_sourceProvider is IApplicationSettingsProvider)
                {
                    (_sourceProvider as IApplicationSettingsProvider).Upgrade(context, properties);
                }
                else
                {
                    // fail silently as per MSDN 
                }
            }
        }

        #endregion


    }
}
