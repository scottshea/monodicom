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

namespace ClearCanvas.Common.Utilities
{
	/// <summary>
	/// Event used to notify observers of a change in a dictionary.
	/// </summary>
	/// <remarks>
	/// This class is used internally by the <see cref="ObservableDictionary{TKey,TItem}"/>, but can be used
	/// for any dictionary-related event.
	/// </remarks>
	/// <typeparam name="TKey">The type of key in the dictionary.</typeparam>
	/// <typeparam name="TItem">The type of item in the dictionary.</typeparam>
	public class DictionaryEventArgs<TKey, TItem> : EventArgs
	{
		private TKey _key;
		private TItem _item;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="key">The key for the <paramref name="item"/> that has changed.</param>
		/// <param name="item">The item that has changed.</param>
		public DictionaryEventArgs(TKey key, TItem item)
		{
			Platform.CheckForNullReference(key, "key");
			_key = key;
			_item = item;
		}

		/// <summary>
		/// Gets the key for the <see cref="Item"/> that has changed.
		/// </summary>
		public TKey Key
		{
			get { return _key; }
		}

		/// <summary>
		/// Gets the item that has changed.
		/// </summary>
		public TItem Item
		{
			get { return _item; }
		}
	}
}
