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

namespace ClearCanvas.Common.Scripting
{
	/// <summary>
	/// Used to specify that a class (for example, an <see cref="IScriptEngine"/>) 
	/// supports a certain scripting language.
	/// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=false)]
    public class LanguageSupportAttribute : Attribute
    {
        private string _language;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="language">A string describing the language.</param>
		public LanguageSupportAttribute(string language)
        {
            _language = language;
        }

		/// <summary>
		/// Gets a string describing the language.
		/// </summary>
        public string Language
        {
            get { return _language; }
        }

		/// <summary>
		/// Determines whether or not this instance is the same as <paramref name="obj"/>, which is itself an <see cref="Attribute"/>.
		/// </summary>
		public override bool Match(object obj)
        {
            LanguageSupportAttribute that = obj as LanguageSupportAttribute;
            return that != null && that.Language.ToLower().Equals(this.Language.ToLower());
        }
    }
}
