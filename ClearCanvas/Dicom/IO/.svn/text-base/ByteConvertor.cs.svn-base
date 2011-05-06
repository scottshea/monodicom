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

namespace ClearCanvas.Dicom.IO
{
    internal static class ByteConverter
    {
        /// <summary>
        /// Determines if this machine has the same byte
        /// order as endian.
        /// </summary>
        /// <param name="endian">endianness</param>
        /// <returns>true - byte swapping is required</returns>
        public static bool NeedToSwapBytes(Endian endian)
        {
            if (BitConverter.IsLittleEndian)
            {
                if (Endian.Little == endian)
                    return false;
                return true;
            }
            else
            {
                if (Endian.Big == endian)
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Converts an array of ushorts to an array of bytes.
        /// </summary>
        /// <param name="words">Array of ushorts</param>
        /// <returns>Newly allocated byte array</returns>
        public static byte[] ToByteArray(ushort[] words)
        {
            int count = words.Length;
            byte[] bytes = new byte[words.Length * 2];
            for (int i = 0; i < count; i++)
            {
                // slow as shit, should be able to use Buffer.BlockCopy for this
                Array.Copy(BitConverter.GetBytes(words[i]), 0, bytes, i * 2, 2);
            }
            return bytes;
        }

        /// <summary>
        /// Converts an array of bytes to an array of ushorts.
        /// </summary>
        /// <param name="bytes">Array of bytes</param>
        /// <returns>Newly allocated ushort array</returns>
        public static ushort[] ToUInt16Array(byte[] bytes)
        {
            int count = bytes.Length / 2;
            ushort[] words = new ushort[count];
            for (int i = 0, p = 0; i < count; i++, p += 2)
            {
                words[i] = BitConverter.ToUInt16(bytes, p);
            }
            return words;
        }

        /// <summary>
        /// Converts an array of bytes to an array of shorts.
        /// </summary>
        /// <param name="bytes">Array of bytes</param>
        /// <returns>Newly allocated short array</returns>
        public static short[] ToInt16Array(byte[] bytes)
        {
            int count = bytes.Length / 2;
            short[] words = new short[count];
            for (int i = 0, p = 0; i < count; i++, p += 2)
            {
                words[i] = BitConverter.ToInt16(bytes, p);
            }
            return words;
        }

        /// <summary>
        /// Converts an array of bytes to an array of uints.
        /// </summary>
        /// <param name="bytes">Array of bytes</param>
        /// <returns>Newly allocated uint array</returns>
        public static uint[] ToUInt32Array(byte[] bytes)
        {
            int count = bytes.Length / 4;
            uint[] dwords = new uint[count];
            for (int i = 0, p = 0; i < count; i++, p += 4)
            {
                dwords[i] = BitConverter.ToUInt32(bytes, p);
            }
            return dwords;
        }
        /// <summary>
        /// Converts an array of bytes to an array of uints.
        /// </summary>
        /// <param name="bytes">Array of bytes</param>
        /// <returns>Newly allocated int array</returns>
        public static int[] ToInt32Array(byte[] bytes)
        {
            int count = bytes.Length / 4;
            int[] dwords = new int[count];
            for (int i = 0, p = 0; i < count; i++, p += 4)
            {
                dwords[i] = (int)BitConverter.ToUInt32(bytes, p);
            }
            return dwords;
        }

        /// <summary>
        /// Converts an array of bytes to an array of floats.
        /// </summary>
        /// <param name="bytes">Array of bytes</param>
        /// <returns>Newly allocated float array</returns>
        public static float[] ToFloatArray(byte[] bytes)
        {
            int count = bytes.Length / 4;
            float[] floats = new float[count];
            for (int i = 0, p = 0; i < count; i++, p += 4)
            {
                floats[i] = BitConverter.ToSingle(bytes, p);
            }
            return floats;
        }

        /// <summary>
        /// Converts an array of bytes to an array of doubles.
        /// </summary>
        /// <param name="bytes">Array of bytes</param>
        /// <returns>Newly allocated double array</returns>
        public static double[] ToDoubleArray(byte[] bytes)
        {
            int count = bytes.Length / 8;
            double[] doubles = new double[count];
            for (int i = 0, p = 0; i < count; i++, p += 8)
            {
                doubles[i] = BitConverter.ToDouble(bytes, p);
            }
            return doubles;
        }

        /// <summary>
        /// Swaps the bytes of an array of unsigned words.
        /// </summary>
        /// <param name="words">Array of ushorts</param>
        public static void SwapBytes(ushort[] words)
        {
            int count = words.Length;
            for (int i = 0; i < count; i++)
            {
                ushort u = words[i];
                words[i] = unchecked((ushort)((u >> 8) | (u << 8)));
            }
        }

        /// <summary>
        /// Swaps the bytes of an array of signed words.
        /// </summary>
        /// <param name="words">Array of shorts</param>
        public static void SwapBytes(short[] words)
        {
            int count = words.Length;
            for (int i = 0; i < count; i++)
            {
                short u = words[i];
                words[i] = unchecked((short)((u >> 8) | (u << 8)));
            }
        }
    }
}
