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
using System.Resources;
using System.Text.RegularExpressions;

namespace ClearCanvas.Common.Utilities
{
    /// <summary>
    /// Default implementation of <see cref="IResourceResolver"/>; finds resources embedded in assemblies.
    /// </summary>
    /// <remarks>
    /// Resolves resources by searching the set of assemblies (specified in the constructor)
    /// in order for a matching resource.  Instances of this class are immutable and thread-safe.
    /// </remarks>
    public class ResourceResolver : IResourceResolver
    {
        /// <summary>
        /// Cache of string resource managers for each assembly.
        /// </summary>
        /// <remarks>
		/// This field is accessed in only one method, GetStringResourceManagers().  This 
		/// is important from a thread-sync point of view.
		/// </remarks>
        private static readonly Dictionary<Assembly, List<ResourceManager>> _mapStringResourceManagers = new Dictionary<Assembly, List<ResourceManager>>();

        /// <summary>
        /// Assemblies to search for resources.
        /// </summary>
        private readonly Assembly[] _assemblies;

        /// <summary>
        /// A fallback resolver, used when a resource cannot be resolved by this resolver.
        /// </summary>
        private readonly IResourceResolver _fallbackResovler;

        /// <summary>
        /// Constructs a resource resolver that will look in the specified set of assemblies for resources.
        /// </summary>
        /// <param name="assemblies">The set of assemblies to search.</param>
        public ResourceResolver(Assembly[] assemblies)
            :this(assemblies, null)
        {
        }

        /// <summary>
        /// Constructs an object that will search the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to search.</param>
        public ResourceResolver(Assembly assembly)
            :this(new Assembly[] { assembly })
        {
        }

        /// <summary>
        /// Constructs a resource resolver that will look in the specified assembly for resources.
        /// </summary>
        /// <param name="assembly">The assembly to search.</param>
        /// <param name="fallback">The fallback <see cref="IResourceResolver"/> to use when an object cannot be resolved by this resolver.</param>
        public ResourceResolver(Assembly assembly, IResourceResolver fallback)
            :this(new Assembly[] {assembly}, fallback)
        {

        }

        /// <summary>
        /// Constructs a resource resolver that will look in the specified set of assemblies for resources.
        /// </summary>
        /// <param name="assemblies">Assemblies covered by this resolver.</param>
        /// <param name="fallback">A fallback resolver, that will be invoked if resources are not found in the specified assemblies.</param>
        public ResourceResolver(Assembly[] assemblies, IResourceResolver fallback)
        {
            _assemblies = assemblies;
            _fallbackResovler = fallback;
        }

		/// <summary>
		/// Constructs a resource resolver that will find resources in the assembly containing the specified type,
		/// and optionally those assemblies containing its base types.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="walkInheritanceChain">If true, assemblies containing base types of the specified type will also be included.</param>
		public ResourceResolver(Type type, bool walkInheritanceChain)
			: this(GetAssembliesForType(type, walkInheritanceChain))
		{

		}

		/// <summary>
		/// Constructs a resource resolver that will find resources in the assembly containing the specified type,
		/// and optionally those assemblies containing its base types.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="walkInheritanceChain">If true, assemblies containing base types of the specified type will also be included.</param>
		/// <param name="fallback"></param>
		public ResourceResolver(Type type, bool walkInheritanceChain, IResourceResolver fallback)
			: this(GetAssembliesForType(type, walkInheritanceChain), fallback)
		{

		}
		
		/// <summary>
        /// Attempts to localize the specified unqualified string resource key
        /// by searching the set of assemblies associated with this <see cref="ResourceResolver"/> in order.
        /// </summary>
        /// <remarks>
        /// Searches the assemblies for resources ending in "SR.resources", and searches those resources
        /// for a string matching the specified key.
        /// </remarks>
        /// <param name="unqualifiedStringKey">The string resource key to search for.  Must not be qualified.</param>
        /// <returns>The localized string, or the argument unchanged if the key could not be found.</returns>
        public string LocalizeString(string unqualifiedStringKey)
        {
            // search the assemblies in order
            foreach(Assembly asm in _assemblies)
            {
                try
                {
                    string localized = LocalizeString(unqualifiedStringKey, asm);
                    if (localized != null)
                    {
                        return localized;
                    }
                }
                catch (Exception)
                {
                    // failed to resolve in the specified assembly
                }
            }

            // try the fallback
            if (_fallbackResovler != null)
                return _fallbackResovler.LocalizeString(unqualifiedStringKey);

            // return the unresolved string if not resolved
            return unqualifiedStringKey;     
        }

        /// <summary>
        /// Attempts to return a fully qualified resource name from the specified name, which may be partially
        /// qualified or entirely unqualified, by searching the assemblies associated with this <see cref="ResourceResolver"/> in order.
        /// </summary>
        /// <param name="resourceName">A partially qualified or unqualified resource name.</param>
        /// <returns>A qualified resource name, if found, otherwise an exception is thrown.</returns>
        /// <exception cref="MissingManifestResourceException">if the resource name could not be resolved.</exception>
        public string ResolveResource(string resourceName)
        { 
			//resources are qualified internally in the manifest with '.' characters, so let's first try
			//to find a resource that matches 'exactly' by preceding the name with a '.'
			string exactMatch = String.Format(".{0}", resourceName);

			foreach (Assembly asm in _assemblies)
            {
				foreach (string match in GetResourcesEndingWith(asm, exactMatch))
				{
					return match;
				}

				//next we'll just try to find the first match ending with the resource name.
				foreach (string match in GetResourcesEndingWith(asm, resourceName))
                {
                    return match;
                }
            }

            // try the fallback
            if (_fallbackResovler != null)
                return _fallbackResovler.ResolveResource(resourceName);

            // not found - throw exception
            throw new MissingManifestResourceException(string.Format(SR.ExceptionResourceNotFound, resourceName));
        }

        /// <summary>
        /// Attempts to resolve and open a resource from the specified name, which may be partially
        /// qualified or entirely unqualified, by searching the assemblies associated with this <see cref="ResourceResolver"/> in order.
        /// </summary>
        /// <param name="resourceName">A partially qualified or unqualified resource name.</param>
        /// <returns>A qualified resource name, if found, otherwise an exception is thrown.</returns>
        /// <exception cref="MissingManifestResourceException">if the resource name could not be resolved.</exception>
        public Stream OpenResource(string resourceName)
        {
			//resources are qualified internally in the manifest with '.' characters, so let's first try
			//to find a resource that matches 'exactly' by preceding the name with a '.'
			string exactMatch = String.Format(".{0}", resourceName);
			
			foreach (Assembly asm in _assemblies)
			{
				foreach (string match in GetResourcesEndingWith(asm, exactMatch))
				{
                    // Assembly type is thread-safe, so this call is ok
					return asm.GetManifestResourceStream(match);
				}

				//next we'll just try to find the first match ending with the resource name.
				foreach (string match in GetResourcesEndingWith(asm, resourceName))
				{
                    // Assembly type is thread-safe, so this call is ok
                    return asm.GetManifestResourceStream(match);
				}
			}

            // try the fallback
            if (_fallbackResovler != null)
                return _fallbackResovler.OpenResource(resourceName);

            // not found - throw exception
            throw new MissingManifestResourceException(string.Format(SR.ExceptionResourceNotFound, resourceName));
        }

        /// <summary>
        /// Returns the set of resources whose name matches the specified regular expression.
        /// </summary>
        /// <param name="regex"></param>
        /// <returns></returns>
        public string[] FindResources(Regex regex)
        {
            List<string> matches = new List<string>();
            foreach (Assembly asm in _assemblies)
            {
                matches.AddRange(
                    CollectionUtils.Select(asm.GetManifestResourceNames(),
                        delegate(string res) { return regex.Match(res).Success; }));
            }

            // include the fallback
            if (_fallbackResovler != null)
                matches.AddRange(_fallbackResovler.FindResources(regex));

            return CollectionUtils.Unique(matches).ToArray();
        }

        /// <summary>
        /// Attempts to localize the specified string table key from the specified assembly, checking all
        /// string resource files in arbitrary order.
        /// </summary>
        /// <remarks>
		/// The first match is returned, or null if no matches are found.
		/// </remarks>
        /// <param name="stringTableKey">The string table key to localize.</param>
        /// <param name="asm">The assembly to look in.</param>
        /// <returns>The first string table entry that matches the specified key, or null if no matches are found.</returns>
        private string LocalizeString(string stringTableKey, Assembly asm)
        {
            foreach (ResourceManager resourceManager in GetStringResourceManagers(asm))
            {
                // resource managers are thread-safe (according to MSDN)
                string resolved = resourceManager.GetString(stringTableKey);
                if (resolved != null)
                    return resolved;
            }
            return null;
        }

        /// <summary>
        /// Returns a list of <see cref="ResourceManager"/>s, one for each string resource file that is present
        /// in the specified assembly.
        /// </summary>
        /// <remarks>
		/// The returned <see cref="ResourceManager"/>s can be used to localize strings.
		/// </remarks>
        private static List<ResourceManager> GetStringResourceManagers(Assembly asm)
        {
            List<ResourceManager> resourceManagers;

            // look for a cached copy
            lock (_mapStringResourceManagers)
            {
                //List<ResourceManager> resourceManagers;
                if (_mapStringResourceManagers.TryGetValue(asm, out resourceManagers))
                    return resourceManagers;
            }

            // no cached copy, so create
            resourceManagers = new List<ResourceManager>();
            foreach (string stringResource in GetResourcesEndingWith(asm, "SR.resources"))
            {
                resourceManagers.Add(new ResourceManager(stringResource.Replace(".resources", ""), asm));
            }

            // update the cache
            lock (_mapStringResourceManagers)
            {
                // note: another thread may have written to the cache in the interim, but it really doesn't matter
                // we can just overwrite it
                _mapStringResourceManagers[asm] = resourceManagers;
                return resourceManagers;
            }
        }

        /// <summary>
        /// Searches the specified assembly for resource files whose names end with the specified string.
        /// </summary>
        /// <param name="asm">The assembly to search.</param>
        /// <param name="endingWith">The string to match the end of the resource name with.</param>
        private static string[] GetResourcesEndingWith(Assembly asm, string endingWith)
        {
            List<string> stringResources = new List<string>();

            // Assembly type is thread-safe, so this call is ok
            foreach (string resName in asm.GetManifestResourceNames())
            {
                if (resName.EndsWith(endingWith))
                    stringResources.Add(resName);
            }
            return stringResources.ToArray();
        }

		/// <summary>
		/// Returns the set of assemblies containing the specified type and all of its base types.
		/// </summary>
		private static Assembly[] GetAssembliesForType(Type type, bool walkInheritanceChain)
		{
			List<Assembly> assemblies = new List<Assembly>();
			while (type != typeof(object))
			{
				assemblies.Add(type.Assembly);
				if(!walkInheritanceChain)
					break;
				type = type.BaseType;
			}
			return CollectionUtils.Unique(assemblies).ToArray();
		}

   }
}
