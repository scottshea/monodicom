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
using ClearCanvas.Dicom.IO;

namespace ClearCanvas.Dicom
{
    #region DicomAttributeSingleValueText
    /// <summary>
    /// <see cref="DicomAttribute"/> derived class for storing single value text value representation attributes.
    /// </summary>
    public abstract class DicomAttributeSingleValueText : DicomAttribute
    {
        private String _value = null;

        #region Constructors
        internal DicomAttributeSingleValueText(uint tag)
            : base(tag)
        {

        }

        internal DicomAttributeSingleValueText(DicomTag tag)
            : base(tag)
        {

        }

        internal DicomAttributeSingleValueText(DicomTag tag, ByteBuffer item)
            : base(tag)
        {
            _value = item.GetString();

            // Saw some Osirix images that had padding on SH attributes with a null character, just
            // pull them out here.
            _value = _value.Trim(new char[] { tag.VR.PadChar, '\0' });

            Count = 1;
            StreamLength = (uint)_value.Length;
        }

        internal DicomAttributeSingleValueText(DicomAttributeSingleValueText attrib)
            : base(attrib)
        {
			string value = attrib.Values as string;
			if (value != null)
				_value = String.Copy(value);
        }
        #endregion

        #region Abstract Method Implementation
        public override void SetNullValue()
        {
            _value = "";
            base.StreamLength = 0;
            base.Count = 1;
        }

		public override void SetEmptyValue()
		{
			_value = null;
			base.StreamLength = 0;
			base.Count = 0;
		}

        /// <summary>
        /// The StreamLength of the attribute.
        /// </summary>
        public override uint StreamLength
        {
            get
            {
                if (IsNull || IsEmpty)
                {
                    return 0;
                }


                if (ParentCollection!=null && ParentCollection.SpecificCharacterSet != null)
                {
                    return (uint)GetByteBuffer(TransferSyntax.ExplicitVrBigEndian, ParentCollection.SpecificCharacterSet).Length;
                }
                return base.StreamLength;
            }
        }

        public override bool TryGetString(int i, out String value)
        {
            if (i == 0)
            {
                value = _value;
                return true;
            }
            value = "";
            return false;
        }

        public override string ToString()
        {
            if (_value == null)
                return "";

            return _value;
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if (obj == null || GetType() != obj.GetType()) return false;

            DicomAttribute a = (DicomAttribute)obj;
        	return Object.Equals(a.Values, _value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override Type GetValueType()
        {
            return typeof(string);
        }

        public override bool IsNull
        {
            get
            {
                if ((_value != null) && (_value.Length == 0))
                    return true;
                return false;
            }
        }
        public override bool IsEmpty
        {
            get
            {
                if ((Count == 0) && (_value == null))
                    return true;
                return false;
            }
        }

        public override Object Values
        {
            get { return _value; }
            set
            {
                if (value is String)
                {
                    SetStringValue((string)value);
                }
                else
                {
                    throw new DicomException(SR.InvalidType);
                }
            }
        }

        public override void SetStringValue(String stringValue)
        {
            if (stringValue == null || stringValue.Length == 0)
            {
                Count = 1;
                StreamLength = 0;
                _value = "";
                return;
            }

            _value = stringValue;

            Count = 1;
            StreamLength = (uint)_value.Length;
        }

        public abstract override DicomAttribute Copy();
        internal abstract override DicomAttribute Copy(bool copyBinary);

        internal override ByteBuffer GetByteBuffer(TransferSyntax syntax, String specificCharacterSet)
        {
            ByteBuffer bb = new ByteBuffer(syntax.Endian);
           if (Tag.VR.SpecificCharacterSet)
                bb.SpecificCharacterSet = specificCharacterSet;
            
            //if (_value == null)
            //{
            //    return bb; // return empty buffer if the value is not set
            //}
            
            bb.SetString(_value, (byte)' ');
            return bb;
        }

        #endregion
    }
    #endregion

    #region DicomAttributeLT
    /// <summary>
    /// <see cref="DicomAttributeSingleValueText"/> derived class for storing LT value representation attributes.
    /// </summary>
    public class DicomAttributeLT : DicomAttributeSingleValueText
    {
        #region Constructors

        public DicomAttributeLT(uint tag)
            : base(tag)
        {

        }

        public DicomAttributeLT(DicomTag tag)
            : base(tag)
        {
            if (!tag.VR.Equals(DicomVr.LTvr)
             && !tag.MultiVR)
                throw new DicomException(SR.InvalidVR);

        }

        internal DicomAttributeLT(DicomTag tag, ByteBuffer item)
            : base(tag, item)
        {
        }

        internal DicomAttributeLT(DicomAttributeLT attrib)
            : base(attrib)
        {
        }

        #endregion

        public override DicomAttribute Copy()
        {
            return new DicomAttributeLT(this);
        }

        internal override DicomAttribute Copy(bool copyBinary)
        {
            return new DicomAttributeLT(this);
        }

    }
    #endregion

    #region DicomAttributeST
    /// <summary>
    /// <see cref="DicomAttributeSingleValueText"/> derived class for storing ST value representation attributes.
    /// </summary>
    public class DicomAttributeST : DicomAttributeSingleValueText
    {
        #region Constructors

        public DicomAttributeST(uint tag)
            : base(tag)
        {

        }

        public DicomAttributeST(DicomTag tag)
            : base(tag)
        {
            if (!tag.VR.Equals(DicomVr.STvr)
             && !tag.MultiVR)
                throw new DicomException(SR.InvalidVR);

        }

        internal DicomAttributeST(DicomTag tag, ByteBuffer item)
            : base(tag, item)
        {
        }


        internal DicomAttributeST(DicomAttributeST attrib)
            : base(attrib)
        {
        }

        #endregion

        public override DicomAttribute Copy()
        {
            return new DicomAttributeST(this);
        }

        internal override DicomAttribute Copy(bool copyBinary)
        {
            return new DicomAttributeST(this);
        }

    }
    #endregion

    #region DicomAttributeUT
    /// <summary>
    /// <see cref="DicomAttributeSingleValueText"/> derived class for storing UT value representation attributes.
    /// </summary>
    public class DicomAttributeUT : DicomAttributeSingleValueText
    {
        #region Constructors

        public DicomAttributeUT(uint tag)
            : base(tag)
        {

        }

        public DicomAttributeUT(DicomTag tag)
            : base(tag)
        {
            if (!tag.VR.Equals(DicomVr.UTvr)
             && !tag.MultiVR)
                throw new DicomException(SR.InvalidVR);

        }

        internal DicomAttributeUT(DicomTag tag, ByteBuffer item)
            : base(tag, item)
        {
        }

        internal DicomAttributeUT(DicomAttributeUT attrib)
            : base(attrib)
        {

        }

        #endregion


        public override DicomAttribute Copy()
        {
            return new DicomAttributeUT(this);
        }

        internal override DicomAttribute Copy(bool copyBinary)
        {
            return new DicomAttributeUT(this);
        }

    }
    #endregion
}
