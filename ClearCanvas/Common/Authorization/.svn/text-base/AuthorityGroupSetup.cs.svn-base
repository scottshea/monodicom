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

namespace ClearCanvas.Common.Authorization
{
    /// <summary>
    /// Extension point for defining default authority groups to be imported at deployment time.
    /// </summary>
    [ExtensionPoint]
    public sealed class DefineAuthorityGroupsExtensionPoint : ExtensionPoint<IDefineAuthorityGroups>
    {
    }

    /// <summary>
    /// Helper class for setting up authority groups.
    /// </summary>
	public static class AuthorityGroupSetup
    {
		/// <summary>
		/// Returns the set of authority tokens defined by all plugins.
		/// </summary>
		/// <returns></returns>
		public static AuthorityTokenDefinition[] GetAuthorityTokens()
		{
			List<AuthorityTokenDefinition> tokens = new List<AuthorityTokenDefinition>();
			// scan all plugins for token definitions
			foreach (PluginInfo plugin in Platform.PluginManager.Plugins)
			{
				IResourceResolver resolver = new ResourceResolver(plugin.Assembly);
				foreach (Type type in plugin.Assembly.GetTypes())
				{
					// look at public fields
					foreach (FieldInfo field in type.GetFields())
					{
						AuthorityTokenAttribute attr = AttributeUtils.GetAttribute<AuthorityTokenAttribute>(field, false);
						if (attr != null)
						{
							string token = (string)field.GetValue(null);
							string description = resolver.LocalizeString(attr.Description);

							tokens.Add(new AuthorityTokenDefinition(token, description));

						}
					}
				}
			}
			return tokens.ToArray();
		}


        /// <summary>
        /// Returns the set of default authority groups defined by all plugins.
        /// </summary>
        /// <remarks>
        /// The default authority groups should only be used at deployment time to initialize the authorization system.
        /// They do not reflect the actual set of authority groups that exist for a given deployment.
        /// </remarks>
        /// <returns></returns>
        public static AuthorityGroupDefinition[] GetDefaultAuthorityGroups()
        {
            List<AuthorityGroupDefinition> groupDefs = new List<AuthorityGroupDefinition>();
            foreach (IDefineAuthorityGroups groupDefiner in new DefineAuthorityGroupsExtensionPoint().CreateExtensions())
            {
                groupDefs.AddRange(groupDefiner.GetAuthorityGroups());
            }
            return groupDefs.ToArray();
        }
    }
}
