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
using System.IO;
using System.Text;

namespace ClearCanvas.Dicom.IO
{
    public class ByteBuffer
    {
        #region Private Members
        private MemoryStream _ms;
        private byte[] _data;
        private BinaryReader _br;
        private BinaryWriter _bw;
        private Endian _endian;
        private Encoding _encoding;
        private String _specificCharacterSet;
        #endregion

        #region Public Constructors
        public ByteBuffer()
            : this(LocalMachineEndian)
        {
        }

        public ByteBuffer(Endian endian)
        {
            _endian = endian;
            _encoding = Encoding.ASCII;
            _specificCharacterSet = null;
        }

        public ByteBuffer(byte[] data)
            : this(data, LocalMachineEndian)
        {
        }

        public ByteBuffer(byte[] data, Endian endian)
        {
            _data = data;
            _endian = endian;
            _encoding = Encoding.ASCII;
            _specificCharacterSet = null;
        }
        #endregion

        #region Public Properties
        public static Endian LocalMachineEndian =
            BitConverter.IsLittleEndian ? Endian.Little : Endian.Big;

        public MemoryStream Stream
        {
            get
            {
                if (_ms == null)
                {
                    if (_data == null)
                    {
                        _ms = new MemoryStream();
                    }
                    else
                    {
                        _ms = new MemoryStream(_data);
                        _data = null;
                    }
                }
                return _ms;
            }
        }

        public BinaryReader Reader
        {
            get
            {
                if (_br == null)
                {
                    _br = EndianBinaryReader.Create(Stream, Endian);
                }
                return _br;
            }
        }

        public BinaryWriter Writer
        {
            get
            {
                if (_bw == null)
                {
                    _bw = EndianBinaryWriter.Create(Stream, Endian);
                }
                return _bw;
            }
        }

        public Endian Endian
        {
            get { return _endian; }
            set
            {
                _endian = value;
                _br = null;
                _bw = null;
            }
        }

        public Encoding Encoding
        {
            get { return _encoding; }
            set { _encoding = value; }
        }

        public String SpecificCharacterSet
        {
            get { return _specificCharacterSet; }
            set { _specificCharacterSet = value; }
        }

        public int Length
        {
            get
            {
                if (_ms != null)
                    return (int)_ms.Length;
                if (_data != null)
                    return _data.Length;
                return 0;
            }
        }
        #endregion

        #region Public Functions
        public void Clear()
        {
            _ms = null;
            _br = null;
            _bw = null;
            _data = null;
        }

        public void Chop(int count)
        {
            int len = (int)Stream.Length;
            if (len <= count)
            {
                Stream.SetLength(0);
                return;
            }
            byte[] bytes = GetChunk(count, len - count);
            Stream.SetLength(0);
            Stream.Position = 0;
            Stream.Write(bytes, 0, bytes.Length);
        }

        public void Append(byte[] buffer, int offset, int count)
        {
            long pos = Stream.Position;
            Stream.Seek(0, SeekOrigin.End);
            Stream.Write(buffer, offset, count);
            Stream.Position = pos;
        }

        public int CopyFrom(Stream s, int count)
        {
            _ms = null;
            _br = null;
            _bw = null;

            int read = 0;
            _data = new byte[count];

            while (read < count)
            {
                int rd = s.Read(_data, read, count - read);
                if (rd == 0)
                    return read;
                read += rd;
            }

            return read;
        }

        public void CopyTo(BinaryWriter s)
        {
			int maxSize = 1024 * 1024 * 4;
			
			if (_ms != null)
            {
				// Discovered a bug when saving to a network drive, where 
				// writing data in large chunks caused an IOException.  
				// Limiting single writes to a smaller size seems to fix the issue.
				if (_ms.Length > maxSize)
				{
					if (s.BaseStream is FileStream)
					{
						byte[] array = _ms.ToArray();
						int bytesLeft = array.Length;
						int offset = 0;
						while (bytesLeft > 0)
						{
							int bytesToWrite = bytesLeft > maxSize ? maxSize : bytesLeft;
							s.Write(array, offset, bytesToWrite);
							bytesLeft -= bytesToWrite;
							offset += bytesToWrite;
						}
					}
					else
						s.Write(_ms.ToArray());
				}
				else
				{
					s.Write(_ms.ToArray());
				}
            	return;
            }

            if (_data != null)
            {
				if (_data.Length > maxSize)
				{
					if (s.BaseStream is FileStream)
					{
						int bytesLeft = _data.Length;
						int offset = 0;
						while (bytesLeft > 0)
						{
							int bytesToWrite = bytesLeft > maxSize ? maxSize : bytesLeft;
							s.Write(_data, offset, bytesToWrite);
							bytesLeft -= bytesToWrite;
							offset += bytesToWrite;
						}
					}
					else
						s.Write(_data, 0, _data.Length);
				}
				else
					s.Write(_data, 0, _data.Length);
            }
        }

        public void CopyTo(Stream s, int offset, int count)
        {
            byte[] bytes = new byte[count];
            Stream.Position = offset;
            Stream.Read(bytes, 0, count);
            s.Write(bytes, 0, count);
        }

        public void CopyTo(byte[] buffer, int offset, int count)
        {
            if (_ms != null)
            {
                Stream.Position = offset;
                Stream.Read(buffer, 0, count);
                return;
            }
            if (_data != null)
            {
                Array.Copy(_data, offset, buffer, 0, count);
            }
        }

        public byte[] GetChunk(int offset, int count)
        {
            byte[] chunk = new byte[count];
            CopyTo(chunk, offset, count);
            return chunk;
        }

        public void FromBytes(byte[] bytes)
        {
            _data = bytes;
            _ms = null;
        }

        public byte[] ToBytes()
        {
            if (_ms != null)
            {
                _data = _ms.ToArray();
                _ms = null;
                _br = null;
                _bw = null;
            }
            if (_data == null)
            {
                return new byte[0];
            }
            return _data;
        }

        public byte[] ToBytes(int offset, int count)
        {
            byte[] data = new byte[count];
            Array.Copy(ToBytes(), offset, data, 0, count);
            return data;
        }

        public ushort[] ToUInt16s()
        {
            return ByteConverter.ToUInt16Array(ToBytes());
        }

        public short[] ToInt16s()
        {
            return ByteConverter.ToInt16Array(ToBytes());
        }

        public uint[] ToUInt32s()
        {
            return ByteConverter.ToUInt32Array(ToBytes());
        }
        public int[] ToInt32s()
        {
            return ByteConverter.ToInt32Array(ToBytes());
        }

        public float[] ToFloats()
        {
            return ByteConverter.ToFloatArray(ToBytes());
        }

        public double[] ToDoubles()
        {
            return ByteConverter.ToDoubleArray(ToBytes());
        }

        public string GetString()
        {
            if (_specificCharacterSet != null)
                return DicomImplementation.CharacterParser.Decode(ToBytes(), _specificCharacterSet);

            return _encoding.GetString(ToBytes());
        }

        public void SetString(string val)
        {
            if (val == null)
            {
                _data = null;
            }
            else
            {
                if (_specificCharacterSet != null)
                    _data = DicomImplementation.CharacterParser.Encode(val, _specificCharacterSet);
                else
                    _data = _encoding.GetBytes(val);
            }
            _ms = null;
        }

        public void SetString(string val, byte pad)
        {
            if (_specificCharacterSet != null)
            {
                _data = DicomImplementation.CharacterParser.Encode(val, _specificCharacterSet);
                if (_data != null && (_data.Length & 1) == 1)
                {
                    byte[] rawBytes = new byte[_data.Length + 1];
                    rawBytes[_data.Length] = pad;

                    _data.CopyTo(rawBytes, 0);
                    _data = rawBytes;
                }
            }
            else
            {
                if (val==null)
                {
                    _data = null;
                }
                else
                {
                    int count = _encoding.GetByteCount(val);
                    if ((count & 1) == 1)
                        count++;

                    byte[] bytes = new byte[count];
                    if (_encoding.GetBytes(val, 0, val.Length, bytes, 0) < count)
                        bytes[count - 1] = pad;

                    _data = bytes;
                }
                
            }
            _ms = null;
        }

        public void Swap(int bytesToSwap)
        {
            if (bytesToSwap == 1)
                return;
            if (bytesToSwap == 2) { Swap2(); return; }
            if (bytesToSwap == 4) { Swap4(); return; }
            //if (bytesToSwap == 8) { Swap8(); return; }
            ToBytes();
            int l = Length - (Length % bytesToSwap);
            for (int i = 0; i < l; i += bytesToSwap)
            {
                Array.Reverse(_data, i, bytesToSwap);
            }
        }

        public void Swap2()
        {
            ToBytes();
            int l = Length - (Length % 2);
            for (int i = 0; i < l; i += 2)
            {
                byte b = _data[i + 1];
                _data[i + 1] = _data[i];
                _data[i] = b;
            }
        }
        public void Swap4()
        {
            ToBytes();
            int l = Length - (Length % 4);
            for (int i = 0; i < l; i += 4)
            {
                byte b = _data[i + 3];
                _data[i + 3] = _data[i];
                _data[i] = b;
                b = _data[i + 2];
                _data[i + 2] = _data[i + 1];
                _data[i + 1] = b;
            }
        }
        public void Swap8()
        {
            Swap(8);
        }
        #endregion
    }
}
