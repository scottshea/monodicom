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
using System.Collections;
using System.Collections.Generic;

namespace ClearCanvas.Common.Utilities
{
    /// <summary>
    /// Provides a set of methods for performing functional-style operations on collections.
    /// </summary>
    public static class CollectionUtils
    {
        /// <summary>
        /// Delegate for use with <see cref="CollectionUtils.Reduce{TItem,TMemo}"/>.
        /// </summary>
		public delegate M ReduceDelegate<T, M>(T item, M memo);

        /// <summary>
        /// Selects all items in the target collection that match the specified predicate, returning
        /// them as a new collection of the specified type.
        /// </summary>
        /// <typeparam name="TItem">The type of items in the target collection.</typeparam>
        /// <typeparam name="TResultCollection">The type of collection to return.</typeparam>
        /// <param name="target">The collection to operate on.</param>
        /// <param name="predicate">The predicate to test.</param>
        /// <returns>A collection containing the subset of matching items from the target collection.</returns>
        public static TResultCollection Select<TItem, TResultCollection>(IEnumerable target, Predicate<TItem> predicate)
            where TResultCollection : ICollection<TItem>, new()
        {
            TResultCollection result = new TResultCollection();
            foreach (TItem item in target)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Selects all items in the target collection that match the specified predicate.
        /// </summary>
        /// <typeparam name="TItem">The type of items in the target collection.</typeparam>
        /// <param name="target">The collection to operate on.</param>
        /// <param name="predicate">The predicate to test.</param>
        /// <returns>A collection containing the subset of matching items from the target collection.</returns>
        public static List<TItem> Select<TItem>(IEnumerable target, Predicate<TItem> predicate)
        {
            List<TItem> result = new List<TItem>();
            foreach (TItem item in target)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Selects all items in the target collection that match the specified predicate.
        /// </summary>
        /// <typeparam name="TItem">The type of items in the target collection.</typeparam>
        /// <param name="target">The collection to operate on.</param>
        /// <param name="predicate">The predicate to test.</param>
        /// <returns>A collection containing the subset of matching items from the target collection.</returns>
        public static List<TItem> Select<TItem>(IEnumerable<TItem> target, Predicate<TItem> predicate)
        {
            return Select((IEnumerable)target, predicate);
        }

        /// <summary>
        /// Selects all items in the target collection that match the specified predicate.
        /// </summary>
        /// <remarks>
		/// This overload accepts an untyped collection, and returns an untyped collection.
		/// </remarks>
        /// <param name="target">The collection to operate on.</param>
        /// <param name="predicate">The predicate to test.</param>
        /// <returns>A collection containing the subset of matching items from the target collection.</returns>
        public static ArrayList Select(IEnumerable target, Predicate<object> predicate)
        {
            ArrayList result = new ArrayList();
            foreach (object item in target)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Excludes all items in the target collection that match the specified predicate, returning
        /// the rest of the items as a new collection of the specified type.
        /// </summary>
        /// <typeparam name="TItem">The type of items in the target collection.</typeparam>
        /// <typeparam name="TResultCollection">The type of collection to return.</typeparam>
        /// <param name="target">The collection to operate on.</param>
        /// <param name="predicate">The predicate to test.</param>
        /// <returns>A collection containing the subset of matching items from the target collection.</returns>
        public static TResultCollection Reject<TItem, TResultCollection>(IEnumerable target, Predicate<TItem> predicate)
            where TResultCollection : ICollection<TItem>, new()
        {
            return Select<TItem, TResultCollection>(target, delegate(TItem item) { return !predicate(item); });
        }

        /// <summary>
        /// Excludes all items in the target collection that match the specified predicate, returning
        /// the rest of the items as a new collection.
        /// </summary>
        /// <typeparam name="TItem">The type of items in the target collection.</typeparam>
        /// <param name="target">The collection to operate on.</param>
        /// <param name="predicate">The predicate to test.</param>
        /// <returns>A collection containing the subset of matching items from the target collection.</returns>
        public static List<TItem> Reject<TItem>(IEnumerable target, Predicate<TItem> predicate)
        {
            return Select<TItem>(target, delegate(TItem item) { return !predicate(item); });
        }

        /// <summary>
        /// Excludes all items in the target collection that match the specified predicate, returning
        /// the rest of the items as a new collection.
        /// </summary>
        /// <typeparam name="TItem">The type of items in the target collection.</typeparam>
        /// <param name="target">The collection to operate on.</param>
        /// <param name="predicate">The predicate to test.</param>
        /// <returns>A collection containing the subset of matching items from the target collection.</returns>
        public static List<TItem> Reject<TItem>(IEnumerable<TItem> target, Predicate<TItem> predicate)
        {
            return Reject((IEnumerable)target, predicate);
        }

        /// <summary>
        /// Excludes all items in the target collection that match the specified predicate, returning
        /// the rest of the items as a new collection.
        /// </summary>
        /// <remarks>
		/// This overload accepts an untyped collection and returns an untyped collection.
		/// </remarks>
        /// <param name="target">The collection to operate on</param>
        /// <param name="predicate">The predicate to test</param>
        /// <returns>A collection containing the subset of matching items from the target collection</returns>
        public static ArrayList Reject(IEnumerable target, Predicate<object> predicate)
        {
            return Select(target, delegate(object item) { return !predicate(item); });
        }

        /// <summary>
        /// Returns the first item in the target collection that matches the specified predicate, or
        /// null if no match is found.
        /// </summary>
        /// <remarks>
		/// <typeparamref name="TItem"/> must be a reference type, not a value type.
		/// </remarks>
        /// <typeparam name="TItem">The type of items in the target collection.</typeparam>
        /// <param name="target">The collection to operate on.</param>
        /// <param name="predicate">The predicate to test.</param>
        /// <returns>The first matching item, or null if no match are found.</returns>
        public static TItem SelectFirst<TItem>(IEnumerable target, Predicate<TItem> predicate)
            where TItem : class
        {
            foreach (TItem item in target)
            {
                if (predicate(item))
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the first item in the target collection that matches the specified predicate, or
        /// null if no match is found.
        /// </summary>
		/// <remarks>
		/// <typeparamref name="TItem"/> must be a reference type, not a value type.
		/// </remarks>
        /// <typeparam name="TItem">The type of items in the target collection.</typeparam>
        /// <param name="target">The collection to operate on.</param>
        /// <param name="predicate">The predicate to test.</param>
        /// <returns>The first matching item, or null if no match are found.</returns>
        public static TItem SelectFirst<TItem>(IEnumerable<TItem> target, Predicate<TItem> predicate)
            where TItem : class
        {
            return SelectFirst((IEnumerable)target, predicate);
        }

        /// <summary>
        /// Returns the first item in the target collection that matches the specified predicate, or
        /// null if no match is found.
        /// </summary>
        /// <remarks>
		/// This overload accepts an untyped collection.
		/// </remarks>
        /// <param name="target">The collection to operate on.</param>
        /// <param name="predicate">The predicate to test.</param>
        /// <returns>The first matching item, or null if no matches are found.</returns>
        public static object SelectFirst(IEnumerable target, Predicate<object> predicate)
        {
            foreach (object item in target)
            {
                if (predicate(item))
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Maps the specified collection onto a new collection according to the specified map function.
        /// </summary>
        /// <remarks>
		/// Allows the type of the return collection to be specified.
		/// </remarks>
        /// <typeparam name="TItem">The type of items in the target collection.</typeparam>
        /// <typeparam name="TResultItem">The type of item returned by the map function.</typeparam>
        /// <typeparam name="TResultCollection">The type of collection to return.</typeparam>
        /// <param name="target">The collection to operate on.</param>
        /// <param name="mapFunction">A delegate that performs the mapping.</param>
        /// <returns>A new collection of the specified type, containing a mapped entry for each entry in the target collection.</returns>
        public static TResultCollection Map<TItem, TResultItem, TResultCollection>(IEnumerable target, Converter<TItem, TResultItem> mapFunction)
            where TResultCollection : ICollection<TResultItem>, new()
        {
            TResultCollection result = new TResultCollection();
            foreach (TItem item in target)
            {
                result.Add(mapFunction(item));
            }
            return result;
        }

        /// <summary>
        /// Maps the specified collection onto a new collection according to the specified map function.
        /// </summary>
        /// <typeparam name="TItem">The type of items in the target collection.</typeparam>
        /// <typeparam name="TResultItem">The type of item returned by the map function.</typeparam>
        /// <param name="target">The collection to operate on.</param>
        /// <param name="mapFunction">A delegate that performs the mapping.</param>
        /// <returns>A new collection containing a mapped entry for each entry in the target collection.</returns>
        public static List<TResultItem> Map<TItem, TResultItem>(IEnumerable target, Converter<TItem, TResultItem> mapFunction)
        {
            List<TResultItem> result = new List<TResultItem>();
            foreach (TItem item in target)
            {
                result.Add(mapFunction(item));
            }
            return result;
        }

        /// <summary>
        /// Maps the specified collection onto a new collection according to the specified map function.
        /// </summary>
        /// <remarks>
		/// This overload operates on an untyped collection and returns an untyped collection.
		/// </remarks>
		/// <param name="target">The collection to operate on.</param>
        /// <param name="mapFunction">A delegate that performs the mapping.</param>
        /// <returns>A new collection containing a mapped entry for each entry in the target collection.</returns>
        public static ArrayList Map(IEnumerable target, Converter<object, object> mapFunction)
        {
            ArrayList result = new ArrayList();
            foreach (object item in target)
            {
                result.Add(mapFunction(item));
            }
            return result;
        }

        /// <summary>
        /// Reduces the specified collection to a singular value according to the specified reduce function.
        /// </summary>
        /// <typeparam name="TItem">The type of items in the target collection.</typeparam>
        /// <typeparam name="TMemo">The type of the singular value to reduce the collection to.</typeparam>
        /// <param name="target">The collection to operate on.</param>
        /// <param name="initial">The initial value for the reduce operation.</param>
        /// <param name="reduceFunction">A delegate that performs the reduce operation.</param>
        /// <returns>The value of the reduce operation.</returns>
        public static TMemo Reduce<TItem, TMemo>(IEnumerable target, TMemo initial, ReduceDelegate<TItem, TMemo> reduceFunction)
        {
            TMemo memo = initial;
            foreach (TItem item in target)
            {
                memo = reduceFunction(item, memo);
            }
            return memo;
        }

        /// <summary>
        /// Performs the specified action for each item in the target collection.
        /// </summary>
        /// <typeparam name="TItem">The type of items in the target collection.</typeparam>
        /// <param name="target">The collection to operate on.</param>
        /// <param name="action">The action to perform.</param>
        public static void ForEach<TItem>(IEnumerable target, Action<TItem> action)
        {
            foreach (TItem item in target)
            {
                action(item);
            }
        }

        /// <summary>
        /// Performs the specified action for each item in the target collection.
        /// </summary>
        /// <typeparam name="TItem">The type of items in the target collection.</typeparam>
        /// <param name="target">The collection to operate on.</param>
        /// <param name="action">The action to perform.</param>
        public static void ForEach<TItem>(IEnumerable<TItem> target, Action<TItem> action)
        {
            ForEach((IEnumerable)target, action);
        }

        /// <summary>
        /// Performs the specified action for each item in the target collection.
        /// </summary>
		/// <remarks>
		/// This overload operates on an untyped collection.
		/// </remarks>
		/// <param name="target">The collection to operate on.</param>
        /// <param name="action">The action to perform.</param>
        public static void ForEach(IEnumerable target, Action<object> action)
        {
            foreach (object item in target)
            {
                action(item);
            }
        }

        /// <summary>
        /// Returns true if any item in the target collection satisfies the specified predicate.
        /// </summary>
        public static bool Contains<TItem>(IEnumerable target, Predicate<TItem> predicate)
        {
            foreach (TItem item in target)
            {
                if (predicate(item))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if any item in the target collection satisfies the specified predicate.
        /// </summary>
        public static bool Contains<TItem>(IEnumerable<TItem> target, Predicate<TItem> predicate)
        {
            return Contains((IEnumerable)target, predicate);
        }

        /// <summary>
        /// Returns true if any item in the target collection satisfies the specified predicate.
        /// </summary>
        public static bool Contains(IEnumerable target, Predicate<object> predicate)
        {
            foreach (object item in target)
            {
                if (predicate(item))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if all items in the target collection satisfy the specified predicate.
        /// </summary>
        public static bool TrueForAll<TItem>(IEnumerable target, Predicate<TItem> predicate)
        {
            foreach (TItem item in target)
            {
                if (!predicate(item))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if all items in the target collection satisfy the specified predicate.
        /// </summary>
        public static bool TrueForAll<TItem>(IEnumerable<TItem> target, Predicate<TItem> predicate)
        {
            return TrueForAll((IEnumerable)target, predicate);
        }

        /// <summary>
        /// Returns true if all items in the target collection satisfy the specified predicate.
        /// </summary>
        public static bool TrueForAll(IEnumerable target, Predicate<object> predicate)
        {
            foreach (object item in target)
            {
                if (!predicate(item))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the first element in the target collection, or null if the collection is empty.
        /// </summary>
        public static object FirstElement(IEnumerable target)
        {
            if(target is IList)
            {
                IList list = (IList)target;
                return list.Count > 0 ? list[0] : null;
            }
            else
            {
                IEnumerator e = target.GetEnumerator();
                return e.MoveNext() ? e.Current : null;
            }
        }

        /// <summary>
		/// Returns the first element in the target collection, or the specified <paramref name="defaultValue"/> if the collection is empty.
        /// </summary>
        public static TItem FirstElement<TItem>(IEnumerable target, TItem defaultValue)
        {
            object value = FirstElement(target);
            return value != null ? (TItem)value : defaultValue;
        }

        /// <summary>
        /// Returns the first element in the target collection, or null if the collection is empty.
        /// </summary>
		/// <remarks>
		/// TItem must be a reference type, not a value type.
		/// </remarks>
        public static TItem FirstElement<TItem>(IEnumerable target)
            where TItem: class
        {
            return FirstElement<TItem>(target, null);
        }

		/// <summary>
		/// Returns the first element in the target collection, or null if the collection is empty.
		/// </summary>
		/// <remarks>
		/// TItem must be a reference type, not a value type.
		/// </remarks>
		public static TItem FirstElement<TItem>(IEnumerable<TItem> target)
            where TItem : class
        {
            return FirstElement<TItem>(target, null);
        }

        /// <summary>
        /// Returns the last element in the target collection, or null if the collection is empty.
        /// </summary>
        public static object LastElement(IEnumerable target)
        {
            if (target is IList)
            {
                IList list = (IList)target;
                return list.Count > 0 ? list[list.Count - 1] : null;
            }
            else
            {
                object element = null;
                IEnumerator e = target.GetEnumerator();
                while (e.MoveNext())
                    element = e.Current;
                return element;
            }
        }

        /// <summary>
		/// Returns the last element in the target collection, or the specified <paramref name="defaultValue "/> if the collection is empty.
        /// </summary>
        public static TItem LastElement<TItem>(IEnumerable target, TItem defaultValue)
        {
            object value = LastElement(target);
            return value != null ? (TItem)value : defaultValue;
        }

        /// <summary>
        /// Returns the last element in the target collection, or null if the collection is empty.
        /// </summary>
		///<remarks>
		/// TItem must be a reference type, not a value type.
		/// </remarks>
		public static TItem LastElement<TItem>(IEnumerable target)
            where TItem : class
        {
            return LastElement<TItem>(target, null);
        }

        /// <summary>
        /// Returns the last element in the target collection, or null if the collection is empty.
		/// </summary>
		///<remarks>
		/// TItem must be a reference type, not a value type.
		/// </remarks>
		public static TItem LastElement<TItem>(IEnumerable<TItem> target)
            where TItem : class
        {
            return LastElement<TItem>(target, null);
        }

        /// <summary>
        /// Removes all items in the target collection that match the specified predicate.
        /// </summary>
        /// <remarks>
		/// Unlike <see cref="Reject"/>, this method modifies the target collection itself.
		/// </remarks>
        public static void Remove<TItem>(ICollection<TItem> target, Predicate<TItem> predicate)
        {
            List<TItem> removes = new List<TItem>();
            foreach (TItem item in target)
            {
                if (predicate(item))
                    removes.Add(item);
            }
            foreach (TItem item in removes)
            {
                target.Remove(item);
            }
        }

		/// <summary>
		/// Removes all items in the target collection that match the specified predicate.
		/// </summary>
		/// <remarks>
		/// Unlike <see cref="Reject"/>, this method modifies the target collection itself.
		/// </remarks>
		public static void Remove(IList target, Predicate<object> predicate)
        {
            List<object> removes = new List<object>();
            foreach (object item in target)
            {
                if (predicate(item))
                    removes.Add(item);
            }
            foreach (object item in removes)
            {
                target.Remove(item);
            }
        }

        /// <summary>
        /// Returns a list of the items in the target collection, sorted according to the specified comparison.
        /// </summary>
		/// <remarks>
		/// Does not modify the target collection, since it may not even be a sortable collection.
        /// If the collection may contain nulls, the comparison must handle nulls.
		/// </remarks>
        public static List<TItem> Sort<TItem>(IEnumerable target, Comparison<TItem> comparison)
        {
            List<TItem> list = new List<TItem>(new TypeSafeEnumerableWrapper<TItem>(target));
            list.Sort(comparison);
            return list;
        }

		/// <summary>
		/// Returns a list of the items in the target collection, sorted using the default comparer.
		/// </summary>
		/// <remarks>
		/// Does not modify the target collection, since it may not even be a sortable collection.
		/// If the collection may contain nulls, the comparison must handle nulls.
		/// </remarks>
		public static List<TItem> Sort<TItem>(IEnumerable target)
		{
			List<TItem> list = new List<TItem>(new TypeSafeEnumerableWrapper<TItem>(target));
			list.Sort();
			return list;
		}

		/// <summary>
		/// Returns a list of the items in the target collection, sorted according to the specified comparison.
		/// </summary>
		/// <remarks>
		/// Does not modify the target collection, since it may not even be a sortable collection.
		/// If the collection may contain nulls, the comparison must handle nulls.
		/// </remarks>
		public static List<TItem> Sort<TItem>(IEnumerable<TItem> target, Comparison<TItem> comparison)
        {
            return Sort((IEnumerable)target, comparison);
        }

		/// <summary>
		/// Returns a list of the items in the target collection, sorted using the default comparer.
		/// </summary>
		/// <remarks>
		/// Does not modify the target collection, since it may not even be a sortable collection.
		/// If the collection may contain nulls, the comparison must handle nulls.
		/// </remarks>
		public static List<TItem> Sort<TItem>(IEnumerable<TItem> target)
		{
			List<TItem> list = new List<TItem>(target);
			list.Sort();
			return list;
		}

        /// <summary>
        /// Converts the target enumerable to an array of the specified type.
        /// </summary>
        public static TItem[] ToArray<TItem>(IEnumerable target)
        {
            // optimize if collection
            if (target is ICollection<TItem>)
            {
                ICollection<TItem> c = (ICollection<TItem>)target;
                TItem[] arr = new TItem[c.Count];
                c.CopyTo(arr, 0);
                return arr;
            }
            else
            {
                List<TItem> list = new List<TItem>(new TypeSafeEnumerableWrapper<TItem>(target));
                return list.ToArray();
            }
        }

        /// <summary>
        /// Converts the target enumerable to an array of the specified type.
        /// </summary>
        public static TItem[] ToArray<TItem>(IEnumerable<TItem> target)
        {
            // optimize if collection
            if (target is ICollection<TItem>)
            {
                ICollection<TItem> c = (ICollection<TItem>)target;
                TItem[] arr = new TItem[c.Count];
                c.CopyTo(arr, 0);
                return arr;
            }
            else
            {
                List<TItem> list = new List<TItem>(target);
                return list.ToArray();
            }
        }

        /// <summary>
		/// Returns the minimum value in the target collection, or the specified <paramref name="nullValue "/> if the target is empty.
        /// </summary>
        /// <remarks>
		/// If the collection may contain nulls, the comparison must handle nulls.
		/// </remarks>
        public static TItem Min<TItem>(IEnumerable target, TItem nullValue, Comparison<TItem> comparison)
        {
            return FindExtremeValue(target, nullValue, comparison, -1);
        }

		/// <summary>
		/// Returns the minimum value in the target collection, or the specified <paramref name="nullValue "/> if the target is empty.
		/// </summary>
		/// <remarks>
		/// If the collection may contain nulls, the comparison must handle nulls.
		/// </remarks>
		public static TItem Min<TItem>(IEnumerable<TItem> target, TItem nullValue, Comparison<TItem> comparison)
        {
            return FindExtremeValue(target, nullValue, comparison, -1);
        }

		/// <summary>
		/// Returns the minimum value in the target collection, or the specified <paramref name="nullValue "/> if the target is empty.
		/// </summary>
		/// <remarks>
        /// <para>If the collection contains nulls, they are treated as less than any other value.</para>
        /// </remarks>
		public static TItem Min<TItem>(IEnumerable target, TItem nullValue)
        {
            return Min(target, nullValue, Comparer<TItem>.Default.Compare);
        }

		/// <summary>
		/// Returns the minimum value in the target collection, or the specified <paramref name="nullValue "/> if the target is empty.
		/// </summary>
		/// <remarks>
        /// <para>If the collection contains nulls, they are treated as less than any other value.</para>
        /// </remarks>
		public static TItem Min<TItem>(IEnumerable<TItem> target, TItem nullValue)
        {
            return Min(target, nullValue, Comparer<TItem>.Default.Compare);
        }

        /// <summary>
        /// Returns the minimum value in the target collection, or null if the collection is empty.
        /// </summary>
        /// <remarks>
		/// <para>The collection must contain object references, not value types.</para>
		/// <para>If the collection contains nulls, they are treated as less than any other value.</para>
		/// </remarks>
        public static TItem Min<TItem>(IEnumerable target)
            where TItem : class, IComparable<TItem>
        {
            return Min<TItem>(target, null);
        }

		/// <summary>
		/// Returns the minimum value in the target collection, or null if the collection is empty.
		/// </summary>
		/// <remarks>
		/// <para>The collection must contain object references, not value types.</para>
		/// <para>If the collection contains nulls, they are treated as less than any other value.</para>
		/// </remarks>
		public static TItem Min<TItem>(IEnumerable<TItem> target)
            where TItem : class, IComparable<TItem>
        {
            return Min(target, null);
        }

        /// <summary>
		/// Returns the maximum value in the target collection, or the specified <paramref name="nullValue"/> if the collection is empty.
		/// </summary>
		/// <remarks>
		/// If the collection may contain nulls, the comparison must handle nulls.
		/// </remarks>
        public static TItem Max<TItem>(IEnumerable target, TItem nullValue, Comparison<TItem> comparison)
        {
            return FindExtremeValue(target, nullValue, comparison, 1);
        }

		/// <summary>
		/// Returns the maximum value in the target collection, or the specified <paramref name="nullValue"/> if the collection is empty.
		/// </summary>
		/// <remarks>
		/// If the collection may contain nulls, the comparison must handle nulls.
		/// </remarks>
		public static TItem Max<TItem>(IEnumerable<TItem> target, TItem nullValue, Comparison<TItem> comparison)
        {
            return FindExtremeValue(target, nullValue, comparison, 1);
        }

		/// <summary>
		/// Returns the maximum value in the target collection, or the specified <paramref name="nullValue"/> if the collection is empty.
		/// </summary>
		/// <remarks>
        /// <para>If the collection contains nulls, they are treated as less than any other value.</para>
        /// </remarks>
		public static TItem Max<TItem>(IEnumerable target, TItem nullValue)
        {
            return Max(target, nullValue, Comparer<TItem>.Default.Compare);
        }

		/// <summary>
		/// Returns the maximum value in the target collection, or the specified <paramref name="nullValue"/> if the collection is empty.
		/// </summary>
		/// <remarks>
        /// <para>If the collection contains nulls, they are treated as less than any other value.</para>
        /// </remarks>
		public static TItem Max<TItem>(IEnumerable<TItem> target, TItem nullValue)
        {
            return Max(target, nullValue, Comparer<TItem>.Default.Compare);
        }

        /// <summary>
        /// Returns the maximum value in the target collection, or the null if the collection is empty.
		/// </summary>
		/// <remarks>
		/// <para>The collection must contain object references, not value types.</para>
		/// <para>If the collection contains nulls, they are treated as less than any other value.</para>
		/// </remarks>
        public static TItem Max<TItem>(IEnumerable target)
            where TItem : class, IComparable<TItem>
        {
            return Max<TItem>(target, null);
        }

		/// <summary>
		/// Returns the maximum value in the target collection, or the null if the collection is empty.
		/// </summary>
		/// <remarks>
		/// <para>The collection must contain object references, not value types.</para>
		/// <para>If the collection contains nulls, they are treated as less than any other value.</para>
		/// </remarks>
		public static TItem Max<TItem>(IEnumerable<TItem> target)
            where TItem : class, IComparable<TItem>
        {
            return Max(target, null);
        }

        /// <summary>
        /// Helper method to provide implementation of <b>Min</b> and <b>Max</b>.
        /// </summary>
        private static T FindExtremeValue<T>(IEnumerable items, T nullValue, Comparison<T> comparison, int sign)
        {
            IEnumerator enumerator = items.GetEnumerator();

            // empty collection - return nullValue
            if (!enumerator.MoveNext())
                return nullValue;

            // enumerate all items to find extreme value
            T memo = (T)enumerator.Current;
            while (enumerator.MoveNext())
            {
                T item = (T)enumerator.Current;
                if (comparison(item, memo)*sign > 0)
                    memo = item;
            }
            return memo;
        }

        /// <summary>
        /// Compares two collections to determine if they are equal, optionally considering the order of elements.
        /// </summary>
        /// <remarks>
        /// Two collections are considered equal if they contain the same number of elements and every element
        /// contained in one collection is contained in the other. If <paramref name="orderSensitive"/> is true,
        /// the elements must also enumerate in the same order.  Equality of individual elements is determined
        /// by their implementation of <see cref="object.Equals(object)"/>.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="orderSensitive"></param>
        /// <returns></returns>
        //TODO: write unit test
        public static bool Equal<T>(ICollection<T> x, ICollection<T> y, bool orderSensitive)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x == null || y == null)
                return false;

            if (x.Count != y.Count)
                return false;

            // if order matters, compare each item one by one
            if(orderSensitive)
            {
                IEnumerator<T> enumY = y.GetEnumerator();
                foreach (T item in x)
                {
                    if(!enumY.MoveNext())
                        return false;
                    if(!Equals(enumY.Current, item))
                        return false;
                }
                return true;
            }

            // order does not matter, so need to do an O(N2) comparison
            return TrueForAll(x, delegate(T obj) { return y.Contains(obj); });
        }

        /// <summary>
        /// Compares two collections to determine if they are equal, optionally considering the order of elements.
        /// </summary>
        /// <remarks>
        /// Two collections are considered equal if they contain the same number of elements and every element
        /// contained in one collection is contained in the other. If <paramref name="orderSensitive"/> is true,
        /// the elements must also enumerate in the same order.  Equality of individual elements is determined
        /// by their implementation of <see cref="object.Equals(object)"/>.
        /// </remarks>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="orderSensitive"></param>
        /// <returns></returns>
        //TODO: write unit test
        public static bool Equal(ICollection x, ICollection y, bool orderSensitive)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x == null || y == null)
                return false;

            if (x.Count != y.Count)
                return false;

            // if order matters, compare each item one by one
            if (orderSensitive)
            {
                IEnumerator enumY = y.GetEnumerator();
                foreach (object itemX in x)
                {
                    if (!enumY.MoveNext())
                        return false;
                    if (!Equals(enumY.Current, itemX))
                        return false;
                }
                return true;
            }

            // order does not matter, so need to do an O(N2) comparison
            return TrueForAll(x,
                delegate(object itemX)
                {
                    return Contains(y, delegate(object itemY) { return Equals(itemY, itemX); });
                });
        }

        /// <summary>
        /// Returns a new list containing only the unique elements of the target collection, preserving the order.
        /// Relies on <see cref="object.Equals(object)"/> and <see cref="object.GetHashCode"/>, since a dictionary
        /// is used internally to create the unique set of results.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static List<T> Unique<T>(IEnumerable<T> target)
        {
            return Unique(target, null);
        }

        /// <summary>
        /// Returns a new list containing only the unique elements of the target collection, preserving the order.
        /// The specified <see cref="IEqualityComparer{T}"/> is used to determine uniqueness.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static List<T> Unique<T>(IEnumerable<T> target, IEqualityComparer<T> comparer)
        {
            Dictionary<T, T> set = comparer == null ? new Dictionary<T, T>() : new Dictionary<T, T>(comparer);
            List<T> result = new List<T>();
            bool resultContainsNull = false;
            foreach(T item in target)
            {
                // handle null item as a special case, because cannot insert it as a key into the hash table
                if(item == null)
                {
                    if(!resultContainsNull) result.Add(item);
                    resultContainsNull = true;
                }
                else
                if (!set.ContainsKey(item))
                {
                    set.Add(item, item);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a new list containing only the unique elements of the target collection, preserving the order.
        /// Relies on <see cref="object.Equals(object)"/> and <see cref="object.GetHashCode"/>, since a dictionary
        /// is used internally to create the unique set of results.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static ArrayList Unique(IEnumerable target)
        {
            return Unique(target, null);
        }


        /// <summary>
        /// Returns a new list containing only the unique elements of the target collection, preserving the order.
        /// The specified <see cref="IEqualityComparer"/> is used to determine uniqueness.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static ArrayList Unique(IEnumerable target, IEqualityComparer comparer)
        {
            Hashtable set = comparer == null ? new Hashtable() : new Hashtable(comparer);
            ArrayList result = new ArrayList();
            bool resultContainsNull = false;
            foreach (object item in target)
            {
                // handle null item as a special case, because cannot insert it as a key into the hash table
                if (item == null)
                {
                    if (!resultContainsNull) result.Add(item);
                    resultContainsNull = true;
                }
                else if (!set.ContainsKey(item))
                {
                    set.Add(item, item);
                    result.Add(item);
                }
            }
            return result;
        }

		/// <summary>
		/// Casts each item in the target collection to the specified type, and returns the results
		/// in a new list.
		/// </summary>
		/// <typeparam name="TOutput"></typeparam>
		/// <param name="target"></param>
		/// <returns></returns>
		public static List<TOutput> Cast<TOutput>(IEnumerable target)
		{
			return Map<object, TOutput>(target, delegate(object input) { return (TOutput) input; });
		}

		/// <summary>
		/// Concatenates all target collections into a single collection.  The items are added
		/// in order.
		/// </summary>
		/// <typeparam name="TItem"></typeparam>
		/// <param name="targets"></param>
		/// <returns></returns>
		public static List<TItem> Concat<TItem>(params IEnumerable<TItem>[] targets)
		{
			List<TItem> result = new List<TItem>();
			foreach (IEnumerable<TItem> target in targets)
			{
				result.AddRange(target);
			}
			return result;
		}

		/// <summary>
		/// Concatenates all target collections into a single collection.  The items are added
		/// in order.
		/// </summary>
		/// <param name="targets"></param>
		/// <returns></returns>
		public static ArrayList Concat(params ICollection[] targets)
		{
			ArrayList result = new ArrayList();
			foreach (ICollection target in targets)
			{
				result.AddRange(target);
			}
			return result;
		}

		/// <summary>
		/// Concatenates all target collections into a single collection.  The items are added
		/// in order.
		/// </summary>
		/// <typeparam name="TItem"></typeparam>
		/// <param name="targets"></param>
		/// <returns></returns>
		public static List<TItem> Concat<TItem>(List<List<TItem>> targets)
		{
			List<TItem> result = new List<TItem>();
			foreach (List<TItem> target in targets)
			{
				result.AddRange(target);
			}
			return result;
		}

		/// <summary>
		/// Partitions elements of the target collection into sub-groups based on the specified key generating function,
		/// and returns a dictionary of the generated keys, where each value is a list of the items that produced that key.
		/// Items appear in the sub-lists in the order in which they were enumerated from the target.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="K"></typeparam>
		/// <param name="target"></param>
		/// <param name="keyFunc"></param>
		/// <returns></returns>
		public static Dictionary<K, List<T>> GroupBy<T, K>(IEnumerable<T> target, Converter<T, K> keyFunc)
		{
			Dictionary<K, List<T>> results = new Dictionary<K, List<T>>();
			foreach (T item in target)
			{
				K key = keyFunc(item);
				List<T> group;
				if (!results.TryGetValue(key, out group))
				{
					results[key] = group = new List<T>();
				}
				group.Add(item);
			}
			return results;
		}
	}
}
