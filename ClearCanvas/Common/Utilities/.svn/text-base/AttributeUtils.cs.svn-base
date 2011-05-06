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

namespace ClearCanvas.Common.Utilities
{
    /// <summary>
    /// Utilities class that provides a set of convenience methods for working with attributes.
    /// </summary>
    public static class AttributeUtils
    {
        /// <summary>
        /// Searches a type/method/property/field for attributes of the specified type, matching the
        /// specified filter.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute (may also be a base class).</typeparam>
        /// <param name="member">The type/method/property/field to find attributes on.</param>
        /// <param name="inherit">True to include inherited attributes in the search.</param>
        /// <param name="filter">A filter that restricts the results of the search.</param>
        /// <returns>A list of matching attributes.</returns>
        public static List<TAttribute> GetAttributes<TAttribute>(MemberInfo member, bool inherit, Predicate<TAttribute> filter)
            where TAttribute : Attribute
        {
            return CollectionUtils.Map<TAttribute, TAttribute, List<TAttribute>>(
                CollectionUtils.Select<TAttribute>(member.GetCustomAttributes(typeof(TAttribute), inherit), filter),
                delegate(TAttribute obj) { return obj; });
        }

        /// <summary>
        /// Searches a type/method/property/field for attributes of the specified type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute (may also be a base class).</typeparam>
        /// <param name="member">The type/method/property/field to find attributes on.</param>
        /// <param name="inherit">True to include inherited attributes in the search.</param>
        /// <returns>A list of matching attributes.</returns>
        public static List<TAttribute> GetAttributes<TAttribute>(MemberInfo member, bool inherit)
            where TAttribute : Attribute
        {
            return GetAttributes<TAttribute>(member, inherit, NullFilter);
        }

        /// <summary>
        /// Searches a type/method/property/field for attributes of the specified type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute (may also be a base class).</typeparam>
        /// <param name="member">The type/method/property/field to find attributes on.</param>
        /// <returns>A list of matching attributes.</returns>
        public static List<TAttribute> GetAttributes<TAttribute>(MemberInfo member)
            where TAttribute : Attribute
        {
            return GetAttributes<TAttribute>(member, false);
        }

        /// <summary>
        /// Searches a type/method/property/field for attributes of the specified type, returning the first match.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute (may also be a base class).</typeparam>
        /// <param name="member">The type/method/property/field to find attributes on.</param>
        /// <param name="inherit">True to include inherited attributes in the search.</param>
        /// <param name="filter">A filter that restricts the results of the search.</param>
        /// <returns>The first matching attribute instance, or null if no matches are found.</returns>
        public static TAttribute GetAttribute<TAttribute>(MemberInfo member, bool inherit, Predicate<TAttribute> filter)
            where TAttribute : Attribute
        {
            return CollectionUtils.SelectFirst<TAttribute>(member.GetCustomAttributes(typeof(TAttribute), inherit), filter);
        }

        /// <summary>
        /// Searches a type/method/property/field for attributes of the specified type, returning the first match.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute (may also be a base class).</typeparam>
        /// <param name="member">The type/method/property/field to find attributes on.</param>
        /// <param name="inherit">True to include inherited attributes in the search.</param>
        /// <returns>The first matching attribute instance, or null if no matches are found.</returns>
        public static TAttribute GetAttribute<TAttribute>(MemberInfo member, bool inherit)
            where TAttribute : Attribute
        {
            return GetAttribute<TAttribute>(member, inherit, NullFilter);
        }

        /// <summary>
        /// Searches a type/method/property/field for attributes of the specified type, returning the first match.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute (may also be a base class).</typeparam>
        /// <param name="member">The type/method/property/field to find attributes on.</param>
        /// <returns>The first matching attribute instance, or null if no matches are found.</returns>
        public static TAttribute GetAttribute<TAttribute>(MemberInfo member)
            where TAttribute : Attribute
        {
            return GetAttribute<TAttribute>(member, false);
        }

        /// <summary>
        /// Tests a type/method/property/field for the presence of an attribute of the specified type, and matching
        /// the specified filter.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute (may also be a base class).</typeparam>
        /// <param name="member">The type/method/property/field to find attributes on.</param>
        /// <param name="inherit">True to include inherited attributes in the search.</param>
        /// <param name="filter">A filter that restricts the results of the search.</param>
        /// <returns>True if a match is found, otherwise false.</returns>
        public static bool HasAttribute<TAttribute>(MemberInfo member, bool inherit, Predicate<TAttribute> filter)
            where TAttribute : Attribute
        {
            return CollectionUtils.Contains<TAttribute>(
                member.GetCustomAttributes(typeof(TAttribute), inherit),
                filter);
        }

        /// <summary>
        /// Tests a type/method/property/field for the presence of an attribute of the specified type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute (may also be a base class).</typeparam>
        /// <param name="member">The type/method/property/field to find attributes on.</param>
        /// <param name="inherit">True to include inherited attributes in the search.</param>
        /// <returns>True if a match is found, otherwise false.</returns>
        public static bool HasAttribute<TAttribute>(MemberInfo member, bool inherit)
            where TAttribute : Attribute
        {
            return HasAttribute<TAttribute>(member, inherit, NullFilter);
        }

        /// <summary>
        /// Tests a type/method/property/field for the presence of an attribute of the specified type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute (may also be a base class).</typeparam>
        /// <param name="member">The type/method/property/field to find attributes on.</param>
        /// <returns>True if a match is found, otherwise false.</returns>
        public static bool HasAttribute<TAttribute>(MemberInfo member)
           where TAttribute : Attribute
        {
            return HasAttribute<TAttribute>(member, false);
        }

        private static bool NullFilter(object obj)
        {
            return true;
        }
    }
}
