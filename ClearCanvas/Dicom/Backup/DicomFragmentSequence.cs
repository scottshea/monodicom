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
using System.Collections.Generic;
using System.IO;
using ClearCanvas.Dicom.IO;

namespace ClearCanvas.Dicom
{
    /// <summary>
    /// Compressed DICOM fragment.
    /// </summary>
    public class DicomFragment
    {
        #region Private members
        private readonly FileReference _reference = null;
        private readonly ByteBuffer _bb = null;
        #endregion

        #region Constructors
        internal DicomFragment(FileReference reference)
        {
            _reference = reference;
        }

        internal DicomFragment(DicomFragment source)
        {
            if (source._reference != null)
                _reference = source._reference;
            else
            {
                _bb = source._bb;
            }
        }

        public DicomFragment(ByteBuffer bb)
        {
            _bb = bb;
        }
        #endregion

        #region Properties
        public uint Length
        {
            get
            {
                if (_reference != null)
                    return _reference.Length;

                return (uint)_bb.Length;
            }
        }
        #endregion

        #region Private Methods
        private ByteBuffer Load()
        {
            if (_reference != null)
            {
                ByteBuffer bb;
				using (FileStream fs = File.OpenRead(_reference.Filename))
                {
                    fs.Seek(_reference.Offset, SeekOrigin.Begin);

                    bb = new ByteBuffer();
                    bb.Endian = _reference.Endian;
                    bb.CopyFrom(fs, (int)_reference.Length);
					fs.Close();
                }
                return bb;
            }

            return null;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get a byte array for the frame's data.
        /// </summary>
        /// <returns></returns>
        public byte[] GetByteArray()
        {
            if (_reference != null)
                return Load().ToBytes();

            return _bb.ToBytes();
        }

        /// <summary>
        /// Get a <see cref="ByteBuffer"/> for the frame's data.
        /// </summary>
        /// <returns></returns>
        public ByteBuffer GetByteBuffer(TransferSyntax syntax)
        {
            if (_reference != null)
                return Load(); // no need to swap, always OB

            return new ByteBuffer(GetByteArray(), ByteBuffer.LocalMachineEndian);
        }
        #endregion

        #region Overrides
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            DicomFragment frame = (DicomFragment)obj;

            byte[] source = this.GetByteArray();
            byte[] dest = frame.GetByteArray();

            for (int index = 0; index < source.Length; index++)
                if (!source[index].Equals(dest[index]))
                    return false;

            return true;
        }

        public override int GetHashCode()
        {
            byte[] source = this.GetByteArray();
            int hash = 0;
            for (int index = 0; index < source.Length; index++)
            {
                hash += (index + 1) * source[index].GetHashCode();
            }
            return hash;
        }
        #endregion

    }

    /// <summary>
    /// <see cref="DicomAttribute"/> representing compressed pixel data encoding rules.
    /// </summary>
    public class DicomFragmentSequence : DicomAttribute
    {
        #region Protected Members
        protected List<uint> _table;
        protected List<DicomFragment> _fragments = new List<DicomFragment>();
        private bool _isNull = false;
        #endregion

        #region Constructors
        public DicomFragmentSequence(uint tag) : base(tag)
        {
        }

        public DicomFragmentSequence(DicomTag tag) : base(tag)
        {
        }

        internal DicomFragmentSequence(DicomFragmentSequence attrib)
            : base(attrib)
        {
            _isNull = attrib._isNull;
            foreach (DicomFragment fragment in attrib._fragments)
            {
                _fragments.Add(new DicomFragment(fragment));
            }
        }

        #endregion

        #region Public Properties
        public bool HasOffsetTable
        {
            get { return _table != null; }
        }

        public ByteBuffer OffsetTableBuffer
        {
            get
            {
                ByteBuffer offsets = new ByteBuffer();
                if (_table != null)
                {
                    foreach (uint offset in _table)
                    {
                        offsets.Writer.Write(offset);
                    }
                }
                return offsets;
            }
        }

        public List<uint> OffsetTable
        {
            get
            {
                if (_table == null)
                    _table = new List<uint>();
                return _table;
            }
        }

        public IList<DicomFragment> Fragments
        {
            get { return _fragments; }
        }
        #endregion

        #region Public Methods
        public void SetOffsetTable(ByteBuffer table)
        {
            _table = new List<uint>();
            _table.AddRange(table.ToUInt32s());
        }
        public void SetOffsetTable(List<uint> table)
        {
            _table = new List<uint>(table);
        }

        public void AddFragment(DicomFragment fragment)
        {
            _fragments.Add(fragment);
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return Tag + " Copmressed with " + _fragments.Count + " fragments";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            DicomFragmentSequence sq = (DicomFragmentSequence)obj;

            if (sq.Count != this.Count)
                return false;

            for (int i = 0; i < Count; i++)
            {
                if (!_fragments[i].Equals(sq._fragments[i]))
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override bool IsNull
        {
            get { return _isNull; }
        }

        public override bool IsEmpty
        {
            get { return _fragments.Count == 0; }
        }

        public override object Values
        {
            get { return _fragments.ToArray(); }
            set
            {
                _fragments.Clear();
                _fragments.AddRange((DicomFragment[])value);
            }
        }

        public override DicomAttribute Copy()
        {
            return new DicomFragmentSequence(this);
        }

        public override Type GetValueType()
        {
            return typeof (byte);
        }

        public override void SetNullValue()
        {
            _fragments.Clear();
            _isNull = true;
        }

		public override void SetEmptyValue()
		{
			_fragments.Clear();
			_isNull = false;
		}

        internal override ByteBuffer GetByteBuffer(TransferSyntax syntax, string specificCharacterSet)
        {
            throw new NotImplementedException();
        }

        internal override DicomAttribute Copy(bool copyBinary)
        {
            return new DicomFragmentSequence(this);
        }

        internal override uint CalculateWriteLength(TransferSyntax syntax, DicomWriteOptions options)
        {
            uint length = 0;
            length += 4; // element tag
            if (syntax.ExplicitVr)
            {
                length += 2; // vr
                length += 6; // length
            }
            else
            {
                length += 4; // length
            }
            length += 4 + 4; // offset tag
            if (Flags.IsSet(options, DicomWriteOptions.WriteFragmentOffsetTable) && _table != null)
                length += (uint)(_table.Count * 4);
            foreach (DicomFragment fragment in this._fragments)
            {
                length += 4; // item tag
                length += 4; // fragment length
                length += fragment.Length;
            }    
            return length;
        }
        #endregion
    }    
}
