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

namespace ClearCanvas.Common
{
	/// <summary>
	/// A helper class for finding ClearCanvas plugins.
	/// </summary>
	internal class PluginFinder : MarshalByRefObject
	{
		// Private attributes
        private List<string> _pluginFiles = new List<string>();

		// Constructor
		public PluginFinder()
        {
        }

		// Properties
        public string[] PluginFiles
        {
            get { return _pluginFiles.ToArray(); }
        }

		// Public methods
		public void FindPlugin(string path)
		{
			Platform.CheckForNullReference(path, "path");
			Platform.CheckForEmptyString(path, "path");

			try
			{
				Assembly asm = LoadAssembly(path);
                Attribute[] attrs = Attribute.GetCustomAttributes(asm);
                foreach (Attribute attr in attrs)
                {
                    if (attr is PluginAttribute)
                    {
                        _pluginFiles.Add(path);

                        break;
                    }
                }
            }
			catch (BadImageFormatException e)
			{
				// Encountered an unmanaged DLL in the plugin directory; this is okay
				// but we'll log it anyway
                Platform.Log(LogLevel.Debug, SR.LogFoundUnmanagedDLL, e.FileName);
			}
			catch (FileNotFoundException e)
			{
				Platform.Log(LogLevel.Error, e, "File not found while loading plugin: {0}", path);
				throw;
			}
			catch (Exception e)
			{
				Platform.Log(LogLevel.Error, e, "Error loading plugin: {0}", path);
				throw;
			}
		}

		// Private methods
		private Assembly LoadAssembly(string path)
		{
			Platform.CheckForNullReference(path, "path");
			Platform.CheckForEmptyString(path, "path");

            // The following block of code did not work under Mono, presumably
            // because Mono expects a fully qualified assembly name

            /*
            AppDomain domain = AppDomain.CurrentDomain;

            // Set the AppDomain's relative search path
            string baseDir = domain.BaseDirectory;
            string pathDir = Path.GetDirectoryName(path);
            string relDir = pathDir.Replace(baseDir, "");
            domain.AppendPrivatePath(relDir);

            // Assembly name = filename without extension
            string assemblyName = Path.GetFileNameWithoutExtension(path);

            return domain.Load(assemblyName);
            */

            // However, the same effect can be accomplished with this single line of code
            return Assembly.LoadFrom(path);
		}
	}
}
