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
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Policy;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Common
{
	/// <summary>
	/// Loads plugin assemblies dynamically from disk and exposes meta-data about the set of installed
	/// plugins, extension points, and extensions to the application.
	/// </summary>
	public class PluginManager 
	{
        private volatile List<PluginInfo> _plugins;
        private volatile List<ExtensionInfo> _extensions;
        private volatile List<ExtensionPointInfo> _extensionPoints;
		private readonly string _pluginDir;
		private event EventHandler<PluginLoadedEventArgs> _pluginProgressEvent;

        private readonly object _syncLock = new object();

		internal PluginManager(string pluginDir)
		{
			Platform.CheckForNullReference(pluginDir, "pluginDir");
			Platform.CheckForEmptyString(pluginDir, "pluginDir");

			_pluginDir = pluginDir;
		}

        /// <summary>
		/// Gets information about the set of all installed plugins.
        /// </summary>
        /// <remarks>
		/// If plugins have not yet been loaded into memory,
		/// querying this property will cause them to be loaded.
		/// </remarks>
        public IList<PluginInfo> Plugins
        {
            get 
            {
                EnsurePluginsLoaded();
                return _plugins.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets information about the set of extensions defined across all installed plugins,
        /// including disabled extensions.
        /// </summary>
        /// <remarks>
		/// If plugins have not yet been loaded
		/// into memory, querying this property will cause them to be loaded.
		/// </remarks>
        public IList<ExtensionInfo> Extensions
        {
            get
            {
                EnsurePluginsLoaded();
                return _extensions.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets information about the set of extension points defined across all installed plugins.  
        /// </summary>
        /// <remarks>
		/// If plugins have not yet been loaded into memory, querying this property will cause them to be loaded.
		/// </remarks>
        public IList<ExtensionPointInfo> ExtensionPoints
        {
            get
            {
                EnsurePluginsLoaded();
                return _extensionPoints.AsReadOnly();
            }
        }

		/// <summary>
		/// Occurs when a plugin is loaded.
		/// </summary>
		public event EventHandler<PluginLoadedEventArgs> PluginLoaded
		{
			add
			{
				lock(_syncLock)
				{
					_pluginProgressEvent += value;
				}
			}
			remove
			{
				lock(_syncLock)
				{
					_pluginProgressEvent -= value;
				}
			}
		}

		/// <summary>
		/// Ensures plugins are loaded exactly once.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// <exception cref="PluginException">Specified plugin directory does not exist or 
		/// a problem has occurred while loading a plugin.</exception>
		private void EnsurePluginsLoaded()
		{
            if(_plugins == null)
            {
                lock(_syncLock)
                {
                    if(_plugins == null)
                    {
                        LoadPlugins();
                    }
                }
            }
        }

        /// <summary>
        /// Loads all plugins in the current plugin directory.
        /// </summary>
        /// <remarks>
        /// This method will traverse the plugin directory and all its subdirectories loading
        /// all valid plugin assemblies.  A valid plugin is an assembly that is marked with an assembly
        /// attribute of type <see cref="ClearCanvas.Common.PluginAttribute"/>.
        /// </remarks>
        /// <exception cref="PluginException">Specified plugin directory does not exist or 
        /// a problem has occurred while loading a plugin.</exception>
        private void LoadPlugins()
        {
            if (!RootPluginDirectoryExists())
                throw new PluginException(SR.ExceptionPluginDirectoryNotFound);

            string[] pluginFiles = FindPlugins(_pluginDir);

            Assembly[] assemblies = LoadFoundPlugins(pluginFiles);
            _plugins = ProcessAssemblies(assemblies);

            // If no plugins could be loaded, just setup empty lists
            if (_plugins.Count == 0)
			{
				_extensions = new List<ExtensionInfo>();
				_extensionPoints = new List<ExtensionPointInfo>();
				return;
			}

            // scan plugins for extensions
            List<ExtensionInfo> extList = new List<ExtensionInfo>();
            foreach (PluginInfo plugin in _plugins)
            {
                extList.AddRange(plugin.Extensions);
            }

            // hack: add extensions from ClearCanvas.Common, which isn't technically a plugin
            extList.AddRange(PluginInfo.DiscoverExtensions(GetType().Assembly));

            // #742: order the extensions according to the XML configuration
            List<ExtensionInfo> ordered, remainder;
            ExtensionSettings.Default.OrderExtensions(extList, out ordered, out remainder);

            // create global extension list, with the ordered set appearing first
            _extensions = CollectionUtils.Concat<ExtensionInfo>(ordered, remainder);

            // scan plugins for extension points
            List<ExtensionPointInfo> epList = new List<ExtensionPointInfo>();
            foreach (PluginInfo plugin in _plugins)
            {
                epList.AddRange(plugin.ExtensionPoints);
            }
            // hack: add extension points from ClearCanvas.Common, which isn't technically a plugin
            epList.AddRange(PluginInfo.DiscoverExtensionPoints(GetType().Assembly));
            _extensionPoints = epList;
        }

        private static List<PluginInfo> ProcessAssemblies(Assembly[] assemblies)
        {
            List<PluginInfo> plugins = new List<PluginInfo>();
            for(int i = 0; i < assemblies.Length; i++)
            {
                try
                {
                    object[] attrs = assemblies[i].GetCustomAttributes(typeof(PluginAttribute), false);
                    if (attrs.Length > 0)
                    {
                        PluginAttribute a = (PluginAttribute)attrs[0];
                        plugins.Add(new PluginInfo(assemblies[i], a.Name, a.Description, a.Icon));
                    }

                }
                catch (ReflectionTypeLoadException e)
                {
                    // this exception usually means one of the dependencies is missing
                    Platform.Log(LogLevel.Error, SR.LogFailedToProcessPluginAssembly, assemblies[i].FullName);
                    
                    // log a detail message for each missing dependency
                    foreach (Exception loaderException in e.LoaderExceptions)
                    {
                        // just log the message, don't need the full stack trace
                        Platform.Log(LogLevel.Error, loaderException.Message);
                    }
                }
                catch (Exception e)
                {
                    // there was a problem processing this assembly
                    Platform.Log(LogLevel.Error, e, SR.LogFailedToProcessPluginAssembly, assemblies[i].FullName);
                }
            }
            return plugins;
        }

        private bool RootPluginDirectoryExists()
		{
			return Directory.Exists(_pluginDir);
		}

        private string[] FindPlugins(string path)
		{
			Platform.CheckForNullReference(path, "path");
			Platform.CheckForEmptyString(path, "path");

			AppDomain domain = null;
            string[] pluginFiles = null;

			try
			{
				EventsHelper.Fire(_pluginProgressEvent, this, new PluginLoadedEventArgs(SR.MessageFindingPlugins, null));

				// Create a secondary AppDomain where we can load all the DLLs in the plugin directory
#if MONO
				domain = AppDomain.CreateDomain("Secondary");
#else		
				Evidence evidence = new Evidence(
					new object[] { new Zone(SecurityZone.MyComputer) },
					new object[] { });

				PermissionSet permissions =
					SecurityManager.ResolvePolicy(evidence);

				AppDomainSetup setup = new AppDomainSetup(); 
				setup.ApplicationBase =	AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

                #region fix plugin loading problem in ASP.NET
                // Apparently we need to use the same lookup paths from the original app domain
                // if the application is launched by ASP.NET. The original app domain 
                // has reference to the "bin" folder where all reference dll's in the webpage are kept (auto copied by
                // VS.NET when you add references). If this is not set, calling
                //
                //  	domain.CreateInstanceAndUnwrap(asm.FullName, "ClearCanvas.Common.PluginFinder")
                //
                // will cause file-not-found exception 
                //
                // To be safe, it's best to copy the original PrivateBinPath instead of hardcoding "Bin"
                //
                
                setup.PrivateBinPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
			    setup.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
			 
                #endregion 

                domain = AppDomain.CreateDomain(
					"Secondary", evidence, setup,
					permissions, new StrongName[] { }); 
#endif			
				Assembly asm = Assembly.GetExecutingAssembly();

				// Instantiate the finder in the secondary domain
                PluginFinder finder = domain.CreateInstanceAndUnwrap(asm.FullName, "ClearCanvas.Common.PluginFinder") as PluginFinder;

				// Assign the FileProcessor's delegate to the finder
				FileProcessor.ProcessFile del = new FileProcessor.ProcessFile(finder.FindPlugin);

				// Process the plugin directory
				FileProcessor.Process(path, "*.dll", del, true);

				// Get the list of legitimate plugin DLLs
                pluginFiles = finder.PluginFiles;
			}
			catch (Exception e)
			{
				Platform.Log(LogLevel.Error,e);
				throw; // TH (Oct 5, 2007) replaced "throw e" with "throw".  "throw e" produces a new exception stack. We want to know where the original exception occurs instead
			}
			finally
			{
				// Unload the domain so that we free up memory used on loading non-plugin DLLs
				if (domain != null)
					AppDomain.Unload(domain);

                if (pluginFiles == null)
					throw new PluginException(SR.ExceptionNoPluginsFound);
			}
            return pluginFiles;
		}

		private Assembly[] LoadFoundPlugins(string[] pluginFileList)
		{
			Platform.CheckForNullReference(pluginFileList, "pluginFileList");

			PluginLoader loader = new PluginLoader();

			// Load the legitimate plugins into the primary AppDomain
			foreach (string pluginFile in pluginFileList)
			{
				Assembly pluginAssembly = loader.LoadPlugin(pluginFile);
				string pluginName = Path.GetFileName(pluginFile);
				EventsHelper.Fire(_pluginProgressEvent, this, 
					new PluginLoadedEventArgs(String.Format(SR.FormatLoadingPlugin, pluginName), pluginAssembly));
			}

			return loader.PluginAssemblies;
		}
	}
}