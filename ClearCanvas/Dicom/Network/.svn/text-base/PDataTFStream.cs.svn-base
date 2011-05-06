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

namespace ClearCanvas.Dicom.Network
{
    internal class PDataTFStream : Stream
    {
        public delegate void TickDelegate();

        #region Private Members
        private bool _command;
        private readonly uint _max;
        private readonly byte _pcid;
        private PDataTF _pdu;
        private byte[] _bytes;
        private MemoryStream _buffer;
        private readonly NetworkBase _networkBase;
    	private bool _combineCommandData;
        #endregion

        #region Public Constructors
        public PDataTFStream(NetworkBase networkBase, byte pcid, uint max, uint total, bool combineCommandData)
        {
            _command = true;
            _pcid = pcid;
            _max = max;
            _pdu = new PDataTF();
            _buffer = new MemoryStream((int)total + 1024);
            _networkBase = networkBase;
        	_combineCommandData = combineCommandData;
        }
        #endregion

        #region Public Properties
        public TickDelegate OnTick;

        public bool IsCommand
        {
            get { return _command; }
            set
            {
                CreatePDV(true);
                _command = value;
				if (!_combineCommandData)
					WritePDU(true);
            }
        }
        #endregion

        #region Public Members
        public void Flush(bool last)
        {
            WritePDU(last);
            //_network.Flush();
        }
        #endregion

        #region Private Members
        private uint CurrentPduSize()
        {
            return _pdu.GetLengthOfPDVs();
        }

        private bool CreatePDV(bool isLast)
        {
            uint len = Math.Min(GetBufferLength(), _max - (CurrentPduSize() + 6));

            //Platform.Log(LogLevel.Info, "Created PDV of length: {0}",len);
            if (_bytes == null || _bytes.Length != len || _pdu.PDVs.Count > 0)
            {
                _bytes = new byte[len];
            }
            _buffer.Read(_bytes, 0, (int)len);

            PDV pdv = new PDV(_pcid, _bytes, _command, isLast);
            _pdu.PDVs.Add(pdv);

            return pdv.IsLastFragment;
        }

        private void WritePDU(bool last)
        {
            if (_pdu.PDVs.Count == 0 || ((CurrentPduSize() + 6) < _max && GetBufferLength() > 0))
            {
                CreatePDV(last);
            }
            if (_pdu.PDVs.Count > 0)
            {
                if (last)
                {
                    _pdu.PDVs[_pdu.PDVs.Count - 1].IsLastFragment = true;
                }
                RawPDU raw = _pdu.Write();

                _networkBase.EnqueuePdu(raw);
                if (OnTick != null)
                    OnTick();
                _pdu = new PDataTF();
            }
        }

        private void AppendBuffer(byte[] buffer, int offset, int count)
        {
            long pos = _buffer.Position;
            _buffer.Seek(0, SeekOrigin.End);
            _buffer.Write(buffer, offset, count);
            _buffer.Position = pos;
        }

        private uint GetBufferLength()
        {
            return (uint)(_buffer.Length - _buffer.Position);
        }
        #endregion

        #region Stream Members
        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            //_network.Flush();
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            AppendBuffer(buffer, offset, count);
            while ((CurrentPduSize() + 6 + GetBufferLength()) > _max)
            {
                WritePDU(false);
            }
        }
        #endregion
    }
}
