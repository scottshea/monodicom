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

#region mDCM License
// mDCM: A C# DICOM library
//
// Copyright (c) 2008  Colby Dillion
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Author:
//    Colby Dillion (colby.dillion@gmail.com)
#endregion

using System;

namespace ClearCanvas.Dicom
{
    public enum DicomFieldDefault
    {
        None,
        Null,
        Default,
        MinValue,
        MaxValue,
        DateTimeNow,
        StringEmpty,
        DBNull,
        EmptyArray
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class DicomFieldAttribute : Attribute
    {
        private DicomTag _tag;
        private DicomFieldDefault _default;
        private bool _defltOnZL;
        private bool _createEmpty;
		private bool _setNullValueIfEmpty;

        public DicomFieldAttribute(uint tag)
        {
            _tag = DicomTagDictionary.GetDicomTag(tag);
            if (_tag == null)
                _tag = new DicomTag(tag, "Unknown Tag", "UnknownTag", DicomVr.UNvr, false, 1, uint.MaxValue, false);

            _default = DicomFieldDefault.None;
            _defltOnZL = false;
            _createEmpty = false;
        }

        public DicomTag Tag
        {
            get { return _tag; }
        }

        public DicomFieldDefault DefaultValue
        {
            get { return _default; }
            set { _default = value; }
        }

        public bool UseDefaultForZeroLength
        {
            get { return _defltOnZL; }
            set { _defltOnZL = value; }
        }

        public bool CreateEmptyElement
        {
            get { return _createEmpty; }
            set { _createEmpty = value; }
        }
		
    	public bool SetNullValueIfEmpty
    	{
			get { return _setNullValueIfEmpty; }
			set { _setNullValueIfEmpty = value; }
    	}
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DicomClassAttribute : Attribute
    {
        private bool _defltOnZL;
        private bool _createEmpty;

        public DicomClassAttribute()
        {
        }

        public bool UseDefaultForZeroLength
        {
            get { return _defltOnZL; }
            set { _defltOnZL = value; }
        }

        public bool CreateEmptyElement
        {
            get { return _createEmpty; }
            set { _createEmpty = value; }
        }
    }
}
