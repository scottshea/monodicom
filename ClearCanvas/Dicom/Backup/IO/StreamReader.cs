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
using ClearCanvas.Common;

namespace ClearCanvas.Dicom.IO
{
    internal enum DicomReadStatus
    {
        Success,
        UnknownError,
        NeedMoreData
    }

    internal class DicomStreamReader
    {
        #region Private Classes
        /// <summary>
        /// Class used to keep track of recursion within sequences
        /// </summary>
        private struct SequenceRecord
        {
            public long _pos;
            public long _len;
            public DicomAttributeCollection _parent;
            public DicomTag _tag;
            public DicomAttributeCollection _current;
            public long _curpos;
            public long _curlen;
        
        }
        #endregion

        #region Private Members
        private const uint UndefinedLength = 0xFFFFFFFF;

        private readonly Stream _stream;
        private BinaryReader _reader = null;
        private TransferSyntax _syntax = null;
        private Endian _endian;

        private DicomAttributeCollection _dataset;

        private DicomTag _tag = null;
        private DicomVr _vr = null;
        private uint _len = UndefinedLength;
        private long _pos = 0;

        private long _bytes = 0;
        private long _read = 0;
        private uint _need = 0;
        private long _remain = 0;

        private long _endGroup2 = 0;
        private bool _inGroup2 = false;

        private string _filename;

        private readonly Stack<SequenceRecord> _sqrs = new Stack<SequenceRecord>();

        private DicomFragmentSequence _fragment = null;
        #endregion

        #region Public Constructors
        public DicomStreamReader(Stream stream)
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
                _endian = _syntax.Endian;
                _reader = EndianBinaryReader.Create(_stream, _endian);
            }
        }

        public DicomAttributeCollection Dataset
        {
            get { return _dataset; }
            set
            {
                _dataset = value;
            }
        }

        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        public long BytesEstimated
        {
            get { return _bytes; }
        }

        public long BytesRead
        {
            get { return _read; }
        }

        public uint BytesNeeded
        {
            get { return _need; }
        }

    	public DicomTag LastTagRead
    	{
    		get { return _tag; }
    	}
        #endregion

        private DicomReadStatus NeedMoreData(long count)
        {
            _need = (uint)count;
            return DicomReadStatus.NeedMoreData;
        }

        public DicomReadStatus Read(DicomTag stopAtTag, DicomReadOptions options)
        {
            if (stopAtTag == null)
                stopAtTag = new DicomTag(0xFFFFFFFF, "Bogus Tag", "BogusTag", DicomVr.UNvr, false, 1, 1, false);

            // Counters:
            //  _remain - bytes remaining in stream
            //  _bytes - estimates bytes to end of dataset
            //  _read - number of bytes read from stream
            try
            {
                _need = 0;
                _remain = _stream.Length - _stream.Position;

                while (_remain > 0)
                {
                    if (_inGroup2 && _read >= _endGroup2)
                    {
                        _inGroup2 = false;
                        // Only change if we're still reading the meta info
                        if (_dataset.StartTagValue < DicomTags.TransferSyntaxUid)
                        {
                            TransferSyntax group2syntax =
                                TransferSyntax.GetTransferSyntax(
                                    _dataset[DicomTags.TransferSyntaxUid].GetString(0, String.Empty));
                            if (group2syntax == null)
                                throw new DicomException("Unsupported transfer syntax in group 2 elements");
                            TransferSyntax = group2syntax;
                        }
                    }
                    uint tagValue;
					if (_tag == null)
					{
						if (_remain < 4)
							return NeedMoreData(4);

						_pos = _stream.Position;
						ushort g = _reader.ReadUInt16();
						ushort e = _reader.ReadUInt16();
						tagValue = DicomTag.GetTagValue(g, e);
						if (DicomTag.IsPrivateGroup(g) && e > 0x00ff)
						{
							_tag = DicomTagDictionary.GetDicomTag(g, e);
							if (_tag == null)
								_tag =
									new DicomTag((uint) g << 16 | e, "Private Tag", "PrivateTag", DicomVr.UNvr, false, 1, uint.MaxValue, false);
						}
						else
						{
							if (e == 0x0000)
								_tag = new DicomTag((uint) g << 16 | e, "Group Length", "GroupLength", DicomVr.ULvr, false, 1, 1, false);
							else
							{
								_tag = DicomTagDictionary.GetDicomTag(g, e);

								if (_tag == null)
									_tag =
										new DicomTag((uint) g << 16 | e, "Private Tag", "PrivateTag", DicomVr.UNvr, false, 1, uint.MaxValue, false);
							}
						}
						_remain -= 4;
						_bytes += 4;
						_read += 4;
					}
					else
						tagValue = _tag.TagValue;

                    if ((tagValue >= stopAtTag.TagValue) 
						&& (_sqrs.Count == 0)) // only exit in root message when after stop tag
                        return DicomReadStatus.Success;

                    if (_vr == null)
                    {
						if (_syntax.ExplicitVr)
						{
							if (_tag == DicomTag.Item ||
								_tag == DicomTag.ItemDelimitationItem ||
								_tag == DicomTag.SequenceDelimitationItem)
							{
								_vr = DicomVr.NONE;
							}
							else
							{
								if (_remain < 2)
									return NeedMoreData(2);

								string vr = new string(_reader.ReadChars(2));
								_vr = DicomVr.GetVR(vr);
								_remain -= 2;
								_bytes += 2;
								_read += 2;
								if (_tag.VR.Equals(DicomVr.UNvr))
									_tag = new DicomTag(_tag.TagValue, "Private Tag", "PrivateTag", _vr, false, 1, uint.MaxValue, false);
								else if (!_tag.VR.Equals(_vr))
								{
									if (!vr.Equals("  "))
									{
										DicomTag tag =
											new DicomTag(_tag.TagValue, _tag.Name, _tag.VariableName, _vr, _tag.MultiVR,
											             _tag.VMLow, _tag.VMHigh,
											             _tag.Retired);
										_tag = tag;

										; // TODO, log something
									}
								}
							}
						}
						else
						{
							_vr = _tag.VR;
						}

                        if (_vr == DicomVr.UNvr)
                        {
                            if (_tag.IsPrivate)
                            {
								if (_tag.Element <= 0x00ff && _tag.Element >= 0x0010)
                                {
                                    // Reset the tag with the right VR and a more descriptive name.
                                    _tag = new DicomTag(_tag.TagValue, "Private Creator Code", "PrivateCreatorCode", DicomVr.LOvr, false, 1, uint.MaxValue, false);

                                    // private creator id
                                    // Only set the VR to LO for Implicit VR, if we do it for
                                    // Explicit VR syntaxes, we would incorrectly read the tag 
                                    // length below.
                                    if (!_syntax.ExplicitVr)
                                        _vr = DicomVr.LOvr;
                                    
                                }
                                else if (_stream.CanSeek && Flags.IsSet(options, DicomReadOptions.AllowSeekingForContext))
                                {
                                    // attempt to identify private sequence by checking if the tag has
									// an undefined length
                                    long pos = _stream.Position;

									int bytesToCheck = _syntax.ExplicitVr ? 6 : 4;

									if (_remain >= bytesToCheck)
									{
										if (_syntax.ExplicitVr)
											_reader.ReadUInt16();

										uint l;
										l = _reader.ReadUInt32();
										if (l == UndefinedLength)
											_vr = DicomVr.SQvr;
									}
                                	_stream.Position = pos;
                                }
                            }
                            else if (!_syntax.ExplicitVr || Flags.IsSet(options, DicomReadOptions.UseDictionaryForExplicitUN))
                                _vr = _tag.VR;
                        }
                    }

                    // Read the value length
					if (_len == UndefinedLength)
					{
						if (_syntax.ExplicitVr)
						{
							if (_tag == DicomTag.Item ||
							    _tag == DicomTag.ItemDelimitationItem ||
							    _tag == DicomTag.SequenceDelimitationItem)
							{
								if (_remain < 4)
									return NeedMoreData(4);

								_len = _reader.ReadUInt32();
								_remain -= 4;
								_bytes += 4;
								_read += 4;
							}
							else
							{
								if (_vr.Is16BitLengthField)
								{
									if (_remain < 2)
										return NeedMoreData(2);

									_len = _reader.ReadUInt16();
									_remain -= 2;
									_bytes += 2;
									_read += 2;
								}
								else
								{
									if (_remain < 6)
										return NeedMoreData(6);

									_reader.ReadByte();
									_reader.ReadByte();
									_len = _reader.ReadUInt32();
									_remain -= 6;
									_bytes += 6;
									_read += 6;
								}
							}
						}
						else
						{
							if (_remain < 4)
								return NeedMoreData(4);

							_len = _reader.ReadUInt32();
							_remain -= 4;
							_bytes += 4;
							_read += 4;
						}

						if ((_len != UndefinedLength)
						    && !_vr.Equals(DicomVr.SQvr)
						    && !(_tag.Equals(DicomTag.Item)
						         && _fragment == null))
							_bytes += _len;
					}

                	// If we have a private creator code, set the VR to LO, because
                    // that is what it is.  We must do this after we read the length
                    // so that the 32 bit length is read properly.
                    if ((_tag.IsPrivate)
					  && (_vr.Equals(DicomVr.UNvr))
                      && (_tag.Element <= 0x00ff))
                        _vr = DicomVr.LOvr;                    

                    if (_fragment != null)
                    {
                        // In the middle of parsing pixels
						if (_tag == DicomTag.Item)
						{
							if (_remain < _len)
								return NeedMoreData(_remain - _len);

							if (Flags.IsSet(options, DicomReadOptions.StorePixelDataReferences)
							    && _fragment.HasOffsetTable)
							{
								FileReference reference = new FileReference(Filename, _stream.Position, _len, _endian, DicomVr.OBvr);
								DicomFragment fragment =
									new DicomFragment(reference);
								_fragment.AddFragment(fragment);
								_stream.Seek(_len, SeekOrigin.Current);
							}
							else
							{
								ByteBuffer data = new ByteBuffer(_endian);
								data.CopyFrom(_stream, (int) _len);

								if (!_fragment.HasOffsetTable)
									_fragment.SetOffsetTable(data);
								else
								{
									DicomFragment fragment = new DicomFragment(data);
									_fragment.AddFragment(fragment);
								}
							}

							_remain -= _len;
							_read += _len;
						}
						else if (_tag == DicomTag.SequenceDelimitationItem)
						{
							_dataset[_fragment.Tag] = _fragment;
							_fragment = null;
						}
						else
						{
							Platform.Log(LogLevel.Error, "Encountered unexpected tag in stream: {0}", _tag.ToString());
							// unexpected tag
							return DicomReadStatus.UnknownError;
						}

                    }
                    else if (_sqrs.Count > 0 &&
                                (_tag == DicomTag.Item ||
                                _tag == DicomTag.ItemDelimitationItem ||
                                _tag == DicomTag.SequenceDelimitationItem))
                    {
                        SequenceRecord rec = _sqrs.Peek();

                        if (_tag.Equals(DicomTag.Item))
                        {
                            if (_len != UndefinedLength)
                            {
                                if (_len > _remain)
                                    return NeedMoreData(_remain - _len);
                            }

                        	DicomSequenceItem ds;

							if (rec._tag.TagValue.Equals(DicomTags.DirectoryRecordSequence))
							{
								DirectoryRecordSequenceItem dr = new DirectoryRecordSequenceItem();
								dr.Offset = (uint)_pos;

								ds = dr;
							}
							else 
								ds = new DicomSequenceItem();

                            rec._current = ds;
							if (rec._tag.VR.Equals(DicomVr.UNvr))
							{
								DicomTag tag = new DicomTag(rec._tag.TagValue, rec._tag.Name,
								                            rec._tag.VariableName, DicomVr.SQvr, rec._tag.MultiVR, rec._tag.VMLow,
								                            rec._tag.VMHigh, rec._tag.Retired);
								rec._parent[tag].AddSequenceItem(ds);
							}
							else
                        		rec._parent[rec._tag].AddSequenceItem(ds);

                            // Specific character set is inherited, save it.  It will be overwritten
                            // if a new value of the tag is encountered in the sequence.
                            rec._current.SpecificCharacterSet = rec._parent.SpecificCharacterSet;

                            // save the sequence length
                            rec._curpos = _pos + 8;
                            rec._curlen = _len;

                            _sqrs.Pop();
                            _sqrs.Push(rec);

                            if (_len != UndefinedLength)
                            {
                                ByteBuffer data = new ByteBuffer(_endian);
                                data.CopyFrom(_stream, (int)_len);
                                _remain -= _len;
                                _read += _len;

                                DicomStreamReader idsr = new DicomStreamReader(data.Stream);
                                idsr.Dataset = ds;
								if (rec._tag.VR.Equals(DicomVr.UNvr))
									idsr.TransferSyntax = TransferSyntax.ImplicitVrLittleEndian;
								else
									idsr.TransferSyntax = _syntax;
                                idsr.Filename = Filename;
                                DicomReadStatus stat = idsr.Read(null, options);
                                if (stat != DicomReadStatus.Success)
                                {
									Platform.Log(LogLevel.Error, "Unexpected parsing error ({0}) when reading sequence attribute: {1}.", stat, rec._tag.ToString());
                                    return stat;
                                }
                            }
                        }
                        else if (_tag == DicomTag.ItemDelimitationItem)
                        {
                        }
                        else if (_tag == DicomTag.SequenceDelimitationItem)
                        {
							SequenceRecord rec2 = _sqrs.Pop();
							if (rec2._current==null)
								rec2._parent[rec._tag].SetNullValue();                      
                        }

                        if (rec._len != UndefinedLength)
                        {
                            long end = rec._pos + 8 + rec._len;
                            if (_syntax.ExplicitVr)
                                end += 2 + 2;
                            if (_stream.Position >= end)
                            {
                                _sqrs.Pop();
                            }
                        }
                    }
                    else
                    {
                        if (_len == UndefinedLength)
                        {
							if (_vr.Equals(DicomVr.UNvr))
							{
								if (!_syntax.ExplicitVr)
								{
									_vr = DicomVr.SQvr;
									if (_tag.IsPrivate)
										_tag = new DicomTag(_tag.TagValue, "Private Tag", "PrivateTag", DicomVr.SQvr, false, 1, uint.MaxValue, false);
									else
										_tag = new DicomTag(_tag.TagValue, "Unknown Tag", "UnknownTag", DicomVr.SQvr, false, 1, uint.MaxValue, false);
								}
								else
								{
									// To handle this case, we'd have to add a new mechanism to transition the parser to implicit VR parsing,
									// and then return back to implicit once the parsing of the SQ is complete.
									Platform.Log(LogLevel.Error,
									             "Encountered unknown tag {0}, encoded as undefined length in an Explicit VR transfer syntax at offset {1}.  Unable to parse.",
									             _tag, _stream.Position);
									return DicomReadStatus.UnknownError;
								}
							}

                        	if (_vr.Equals(DicomVr.SQvr))
                            {
                                SequenceRecord rec = new SequenceRecord();
                                if (_sqrs.Count > 0)
                                    rec._parent = _sqrs.Peek()._current;
                                else
                                    rec._parent = _dataset;
                                rec._current = null;
                                rec._tag = _tag;
                                rec._len = UndefinedLength;
                                
                                _sqrs.Push(rec);
                            }
                            else
                            {
                                _fragment = new DicomFragmentSequence(_tag);

                                _dataset.LoadDicomFields(_fragment);
                            }
                        }
                        else
                        {
							if (_vr.Equals(DicomVr.SQvr))
							{
								if (_len == 0)
								{
									DicomAttributeCollection ds;
									if (_sqrs.Count > 0)
									{
										SequenceRecord rec = _sqrs.Peek();
										ds = rec._current;
									}
									else
										ds = _dataset;

									ds[_tag].SetNullValue();
								}
								else
								{
									SequenceRecord rec = new SequenceRecord();
									rec._len = _len;
									rec._pos = _pos;
									rec._tag = _tag;
									if (_sqrs.Count > 0)
										rec._parent = _sqrs.Peek()._current;
									else
										rec._parent = _dataset;

									_sqrs.Push(rec);
								}
							}
							else
							{
								if (_remain < _len)
									return NeedMoreData(_len - _remain);

								if ((_tag.TagValue == DicomTags.PixelData)
								    && Flags.IsSet(options, DicomReadOptions.DoNotStorePixelDataInDataSet))
								{
									// Skip PixelData !!
									_stream.Seek((int) _len, SeekOrigin.Current);
									_remain -= _len;
									_read += _len;
								}
								else if ((_tag.TagValue == DicomTags.PixelData) &&
								         Flags.IsSet(options, DicomReadOptions.StorePixelDataReferences))
								{
									FileReference reference =
										new FileReference(Filename, _stream.Position, _len, _endian,
										                  _tag.VR);
									_stream.Seek((int) _len, SeekOrigin.Current);

									if (_tag.VR.Equals(DicomVr.OWvr))
									{
										DicomAttributeOW elem = new DicomAttributeOW(_tag, reference);
										_dataset[_tag] = elem;
									}
									else if (_tag.VR.Equals(DicomVr.OBvr))
									{
										DicomAttributeOB elem = new DicomAttributeOB(_tag, reference);
										_dataset[_tag] = elem;
									}
									else
									{
										DicomAttributeOF elem = new DicomAttributeOF(_tag, reference);
										_dataset[_tag] = elem;
									}
									_remain -= _len;
									_read += _len;
								}
								else
								{
									ByteBuffer bb = new ByteBuffer();
									// If the tag is impacted by specific character set, 
									// set the encoding properly.
									if (_tag.VR.SpecificCharacterSet)
									{
										if (_sqrs.Count > 0)
										{
											SequenceRecord rec = _sqrs.Peek();
											bb.SpecificCharacterSet = rec._current.SpecificCharacterSet;
										}
										else
										{
											bb.SpecificCharacterSet = _dataset.SpecificCharacterSet;
										}
									}

									bb.Endian = _endian;
									bb.CopyFrom(_stream, (int) _len);

									DicomAttribute elem = _tag.CreateDicomAttribute(bb);

									_remain -= _len;
									_read += _len;


									if (_sqrs.Count > 0)
									{
										SequenceRecord rec = _sqrs.Peek();
										DicomAttributeCollection ds = rec._current;

										if (elem.Tag.TagValue == DicomTags.SpecificCharacterSet)
										{
											ds.SpecificCharacterSet = elem.ToString();
										}

										if (_tag.Element == 0x0000)
										{
											if (Flags.IsSet(options, DicomReadOptions.KeepGroupLengths))
												ds[_tag] = elem;
										}
										else
											ds[_tag] = elem;

										if (rec._curlen != UndefinedLength)
										{
											long end = rec._curpos + rec._curlen;
											if (_stream.Position >= end)
											{
												rec._current = null;
											}
										}
									}
									else
									{
										if (_tag.TagValue == DicomTags.FileMetaInformationGroupLength)
										{
											// Save the end of the group 2 elements, so that we can automatically 
											// check and change our transfer syntax when needed.
											_inGroup2 = true;
											uint group2len;
											elem.TryGetUInt32(0, out group2len);
											_endGroup2 = _read + group2len;
										}
										else if (_tag.TagValue == DicomTags.SpecificCharacterSet)
										{
											_dataset.SpecificCharacterSet = elem.ToString();
										}

										if (_tag.Element == 0x0000)
										{
											if (Flags.IsSet(options, DicomReadOptions.KeepGroupLengths))
												_dataset[_tag] = elem;
										}
										else
											_dataset[_tag] = elem;
									}
								}
							}
                        }
                    }

                    _tag = null;
                    _vr = null;
                    _len = UndefinedLength;
                }
                return DicomReadStatus.Success;
            }
            catch (EndOfStreamException e)
            {
                // should never happen
				Platform.Log(LogLevel.Error, "Unexpected exception when reading file: {0}", e.ToString());
                return DicomReadStatus.UnknownError;
            }
        }
    }
}
