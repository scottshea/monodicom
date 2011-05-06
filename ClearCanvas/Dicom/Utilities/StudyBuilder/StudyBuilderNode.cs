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

using System.ComponentModel;

namespace ClearCanvas.Dicom.Utilities.StudyBuilder
{
	/// <summary>
	/// The abstract base class for nodes within the study builder tree represented by the <see cref="StudyBuilder"/> class.
	/// </summary>
	/// <remarks>
	/// This class should not be (and cannot be) inherited directly. To add a node to the study builder tree, instantiate a node at the desired tree level instead.
	/// </remarks>
	/// <see cref="PatientNode"/>
	/// <see cref="StudyNode"/>
	/// <see cref="SeriesNode"/>
	/// <see cref="SopInstanceNode"/>
	/// <see cref="StudyBuilder"/>
	public abstract class StudyBuilderNode
	{
		private event PropertyChangedEventHandler _propertyChanged;
		private readonly string _key;
		private StudyBuilderNode _parent = null;

		internal StudyBuilderNode()
		{
			_key = serialGener.GetNext().ToString().PadLeft(8, '0');
		}

		/// <summary>
		/// Fires the <see cref="PropertyChanged"/> event.
		/// </summary>
		/// <param name="propertyName">The name of the property on this node that changed.</param>
		protected virtual void FirePropertyChanged(string propertyName)
		{
			if (_propertyChanged != null)
				_propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Raised when a property on this node has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged
		{
			add { _propertyChanged += value; }
			remove { _propertyChanged -= value; }
		}

		/// <summary>
		/// Internally used key to uniquely identify the node.
		/// </summary>
		internal string Key
		{
			get { return _key; }
		}

		/// <summary>
		/// Gets the parent of this node, or null if the node is not in a study builder tree.
		/// </summary>
		public StudyBuilderNode Parent
		{
			get { return _parent; }
			internal set
			{
				if (_parent != value)
				{
					_parent = value;
					FirePropertyChanged("Parent");
				}
			}
		}

		private static readonly SerialGenerator serialGener = new SerialGenerator();

		/// <summary>
		/// A unique serial number generator for generating unique keys
		/// </summary>
		private class SerialGenerator
		{
			private volatile int _next;

			public int GetNext()
			{
				lock (this)
				{
					return _next++;
				}
			}
		}
	}
}