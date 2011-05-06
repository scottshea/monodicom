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
using System.Text;

namespace ClearCanvas.Common.Utilities
{
    /// <summary>
    /// When placed on a field/property of a class derived from <see cref="CommandLine"/>, instructs
    /// the base class to attempt to set the field/property according to the parsed command line arguments.
    /// </summary>
    /// <remarks>
    /// If the field/property is of type string, int, or enum, it is treated as a named parameter, unless
    /// the <see cref="Position"/> property of the attribute is set, in which case it is treated as a positional
    /// parameter.  If the field/property is of type boolean, it is treated as a switch.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class CommandLineParameterAttribute : Attribute
    {
        private readonly int _position = -1;
        private bool _required;
        private readonly string _key;
        private readonly string _keyShortForm;
        private readonly string _usage;
        private readonly string _displayName;

        /// <summary>
        /// Constructor for declaring a positional parameter.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="displayName"></param>
        public CommandLineParameterAttribute(int position, string displayName)
        {
            _position = position;
            _displayName = displayName;
        }

        /// <summary>
        /// Constructor for declaring a named parameter or boolean switch.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="usage"></param>
        public CommandLineParameterAttribute(string key, string usage)
        {
            _key = key;
            _usage = usage;
        }

        /// <summary>
        /// Constructor for declaring a named parameter or boolean switch.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyShortForm"></param>
        /// <param name="usage"></param>
        public CommandLineParameterAttribute(string key, string keyShortForm, string usage)
        {
            _key = key;
            _keyShortForm = keyShortForm;
            _usage = usage;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this parameter is a required parameter.
        /// </summary>
        public bool Required
        {
            get { return _required; }
            set { _required = value; }
        }

        /// <summary>
        /// Gets the position of a positional parameter.
        /// </summary>
        internal int Position
        {
            get { return _position; }
        }

        /// <summary>
        /// Gets the display name for a positional parameter.
        /// </summary>
        internal string DisplayName
        {
            get { return _displayName; }
        }

        /// <summary>
        /// Gets the key (parameter name) for a named parameter.
        /// </summary>
        internal string Key
        {
            get { return _key; }
        }

        /// <summary>
        /// Gets the key short-form for a named parameter.
        /// </summary>
        internal string KeyShortForm
        {
            get { return _keyShortForm; }
        }

        /// <summary>
        /// Gets a message describing the usage of this parameter.
        /// </summary>
        internal string Usage
        {
            get { return _usage; }
        }

    }
}
