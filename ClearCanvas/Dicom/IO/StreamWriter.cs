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

using System.IO;

namespace ClearCanvas.Dicom.IO
{
    internal enum DicomWriteStatus
    {
        Success,
        UnknownError
    }

    internal class DicomStreamWriter
    {
        #region Private Members
        private const uint UndefinedLength = 0xFFFFFFFF;

        private Stream _stream = null;
        private BinaryWriter _writer = null;
        private TransferSyntax _syntax = null;
        private Endian _endian;

        private ushort _group = 0xffff;
        #endregion

        #region Public Constructors
        public DicomStreamWriter(Stream stream)
        {
            _stream = stream;
            TransferSyntax = TransferSyntax.ExplicitVrLittleEndian;
        }
        #endregion

        #region Public Properties
        public TransferSyntax TransferSyntax
        {
            get { return _syntax; }
            set
            {
                _syntax = value;
                if (_endian != _syntax.Endian || _writer == null)
                {
                    _endian = _syntax.Endian;
                    _writer = EndianBinaryWriter.Create(_stream, _endian);
                }
            }
        }

        #endregion

        public DicomWriteStatus Write(TransferSyntax syntax, DicomAttributeCollection dataset, DicomWriteOptions options)
        {
            TransferSyntax = syntax;

            foreach (DicomAttribute item in dataset)
            {
                if (item.Tag.Element == 0x0000)
                    continue;

                if (item.IsEmpty)
                    continue;

                if (Flags.IsSet(options, DicomWriteOptions.CalculateGroupLengths)
                    && item.Tag.Group != _group && item.Tag.Group <= 0x7fe0)
                {
                    _group = item.Tag.Group;
                    _writer.Write((ushort)_group);
                    _writer.Write((ushort)0x0000);
                    if (_syntax.ExplicitVr)
                    {
                        _writer.Write((byte)'U');
                        _writer.Write((byte)'L');
                        _writer.Write((ushort)4);
                    }
                    else
                    {
                        _writer.Write((uint)4);
                    }
                    _writer.Write((uint)dataset.CalculateGroupWriteLength(_group, _syntax, options));
                }

                _writer.Write((ushort)item.Tag.Group);
                _writer.Write((ushort)item.Tag.Element);

                if (_syntax.ExplicitVr)
                {
                    _writer.Write((byte)item.Tag.VR.Name[0]);
                    _writer.Write((byte)item.Tag.VR.Name[1]);
                }

                if (item is DicomAttributeSQ)
                {
                    DicomAttributeSQ sq = item as DicomAttributeSQ;

                    if (_syntax.ExplicitVr)
                        _writer.Write((ushort)0x0000);

                    if (Flags.IsSet(options, DicomWriteOptions.ExplicitLengthSequence))
                    {
                        int hl = _syntax.ExplicitVr ? 12 : 8;
                        _writer.Write((uint)sq.CalculateWriteLength(_syntax, options & ~DicomWriteOptions.CalculateGroupLengths) - (uint)hl);
                    }
                    else
                    {
                        _writer.Write((uint)UndefinedLength);
                    }

                    foreach (DicomSequenceItem ids in item.Values as DicomSequenceItem[])
                    {
                        _writer.Write((ushort)DicomTag.Item.Group);
                        _writer.Write((ushort)DicomTag.Item.Element);

                        if (Flags.IsSet(options, DicomWriteOptions.ExplicitLengthSequenceItem))
                        {
                            _writer.Write((uint)ids.CalculateWriteLength(_syntax, options & ~DicomWriteOptions.CalculateGroupLengths));
                        }
                        else
                        {
                            _writer.Write((uint)UndefinedLength);
                        }

                        Write(this.TransferSyntax, ids, options & ~DicomWriteOptions.CalculateGroupLengths);

                        if (!Flags.IsSet(options, DicomWriteOptions.ExplicitLengthSequenceItem))
                        {
                            _writer.Write((ushort)DicomTag.ItemDelimitationItem.Group);
                            _writer.Write((ushort)DicomTag.ItemDelimitationItem.Element);
                            _writer.Write((uint)0x00000000);
                        }
                    }

                    if (!Flags.IsSet(options, DicomWriteOptions.ExplicitLengthSequence))
                    {
                        _writer.Write((ushort)DicomTag.SequenceDelimitationItem.Group);
                        _writer.Write((ushort)DicomTag.SequenceDelimitationItem.Element);
                        _writer.Write((uint)0x00000000);
                    }
                }

                else if (item is DicomFragmentSequence)
                {
                    DicomFragmentSequence fs = item as DicomFragmentSequence;

                    if (_syntax.ExplicitVr)
                        _writer.Write((ushort)0x0000);
                    _writer.Write((uint)UndefinedLength);

                    _writer.Write((ushort)DicomTag.Item.Group);
                    _writer.Write((ushort)DicomTag.Item.Element);

                    if (Flags.IsSet(options, DicomWriteOptions.WriteFragmentOffsetTable) && fs.HasOffsetTable)
                    {
                        _writer.Write((uint)fs.OffsetTableBuffer.Length);
                        fs.OffsetTableBuffer.CopyTo(_writer);
                    }
                    else
                    {
                        _writer.Write((uint)0x00000000);
                    }

                    foreach (DicomFragment bb in fs.Fragments)
                    {
                        _writer.Write((ushort)DicomTag.Item.Group);
                        _writer.Write((ushort)DicomTag.Item.Element);
                        _writer.Write((uint)bb.Length);
                        bb.GetByteBuffer(_syntax).CopyTo(_writer);
                    }

                    _writer.Write((ushort)DicomTag.SequenceDelimitationItem.Group);
                    _writer.Write((ushort)DicomTag.SequenceDelimitationItem.Element);
                    _writer.Write((uint)0x00000000);
                }
                else
                {
                    DicomAttribute de = item;
                	ByteBuffer theData = de.GetByteBuffer(_syntax, dataset.SpecificCharacterSet);
                    if (_syntax.ExplicitVr)
                    {
                        if (de.Tag.VR.Is16BitLengthField)
                        {
							_writer.Write((ushort)theData.Length);
                        }
                        else
                        {
                            _writer.Write((ushort)0x0000);
                            _writer.Write((uint)theData.Length);
                        }
                    }
                    else
                    {
						_writer.Write((uint)theData.Length);
                    }

					if (theData.Length > 0)
						theData.CopyTo(_writer);
                }
            }

            return DicomWriteStatus.Success;
        }
    }
}
