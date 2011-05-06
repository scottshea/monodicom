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

using System.Collections;
using System.Collections.Generic;

namespace ClearCanvas.Dicom
{
	/// <summary>
	/// A collection of DICOMDIR Directory Records at a given level.
	/// </summary>
	/// <remarks>
	/// The collection is used so that within a Directory Entity within a DICOMDIR,
	/// the records at a given level can be enumerated over.  It supports the
	/// <see cref="IEnumerable"/> interface for the collection.
	/// </remarks>
	public class DirectoryRecordCollection : IEnumerable<DirectoryRecordSequenceItem>
	{
		#region Classes
		internal class DirectoryRecordEnumerator : IEnumerator<DirectoryRecordSequenceItem>
		{
			#region Private Members

			private readonly DirectoryRecordSequenceItem _head;
			private DirectoryRecordSequenceItem _current;
			private bool _atEnd;

			#endregion

			#region Constructors

			internal DirectoryRecordEnumerator(DirectoryRecordSequenceItem head)
			{
				_head = head;
			}

			#endregion

			#region Implementation of IDisposable

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			/// <filterpriority>2</filterpriority>
			public void Dispose()
			{
				_current = null;
			}

			#endregion

			#region Implementation of IEnumerator

			/// <summary>
			/// Advances the enumerator to the next element of the collection.
			/// </summary>
			/// <returns>
			/// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
			/// </returns>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. 
			///                 </exception><filterpriority>2</filterpriority>
			public bool MoveNext()
			{
				if (_head == null)
					return false;

				if (_atEnd)
					return false;

				if (_current == null)
				{
					_current = _head;
					return true;
				}

				if (_current.NextDirectoryRecord == null)
				{
					_atEnd = true;
					return false;
				}

				_current = _current.NextDirectoryRecord;
				return true;
			}

			/// <summary>
			/// Sets the enumerator to its initial position, which is before the first element in the collection.
			/// </summary>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. 
			///                 </exception><filterpriority>2</filterpriority>
			public void Reset()
			{
				_current = null;
				_atEnd = false;
			}

			/// <summary>
			/// Gets the element in the collection at the current position of the enumerator.
			/// </summary>
			/// <returns>
			/// The element in the collection at the current position of the enumerator.
			/// </returns>
			public DirectoryRecordSequenceItem Current
			{
				get
				{
					return _current;
				}
			}

			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			/// <returns>
			/// The current element in the collection.
			/// </returns>
			/// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.
			///                 </exception><filterpriority>2</filterpriority>
			object IEnumerator.Current
			{
				get { return Current; }
			}

			#endregion
		}

		#endregion

		#region Private Members

		private readonly DirectoryRecordSequenceItem _firstRecord;

		#endregion

		#region Constructors

		internal DirectoryRecordCollection(DirectoryRecordSequenceItem firstRecord)
		{
			_firstRecord = firstRecord;
		}

		#endregion

		#region Implementation of IEnumerable

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public IEnumerator<DirectoryRecordSequenceItem> GetEnumerator()
		{
			return new DirectoryRecordEnumerator(_firstRecord);
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
