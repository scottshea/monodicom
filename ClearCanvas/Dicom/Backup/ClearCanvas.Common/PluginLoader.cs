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
	internal class PluginLoader 
	{
		// Private attributes
        private List<Assembly> _pluginList = new List<Assembly>();

		// Constructor
		public PluginLoader() {	}

		// Properties
        public Assembly[] PluginAssemblies
        {
            get { return _pluginList.ToArray(); }
        }

		// Public methods
		public Assembly LoadPlugin(string path)
		{
			Platform.CheckForNullReference(path, "path");
			Platform.CheckForEmptyString(path, "path");

			try
			{
				Assembly asm = Assembly.LoadFrom(path);
                _pluginList.Add(asm);

				Platform.Log(LogLevel.Debug, SR.LogPluginLoaded, path);

				return asm;
			}
			catch (FileNotFoundException e)
			{
				Platform.Log(LogLevel.Error, e);
				throw;
			}
			catch (Exception e)
			{
				Platform.Log(LogLevel.Error, e);
				throw;
			}
		}

		private Type GetPluginType(Assembly asm)
		{
			Platform.CheckForNullReference(asm, "asm");

			foreach (Type type in asm.GetExportedTypes())
			{
				if (typeof(PluginInfo).IsAssignableFrom(type))
					return type;
			}

			return null;
		}
	}
}
