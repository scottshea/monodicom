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
    /// <summary>
    /// A class representing a DICOM Sequence Item.
    /// </summary>
    public class DicomSequenceItem : DicomAttributeCollection
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public DicomSequenceItem() : base(0x00000000,0xFFFFFFFF)
        {
        }

        /// <summary>
        /// Internal constructor used when making a copy of a <see cref="DicomAttributeCollection"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="copyBinary"></param>
        /// <param name="copyPrivate"></param>
        /// <param name="copyUnknown"></param>
        internal DicomSequenceItem(DicomAttributeCollection source, bool copyBinary, bool copyPrivate, bool copyUnknown)
            : base(source, copyBinary, copyPrivate, copyUnknown)
        {
        }
        #endregion

        #region Public Overridden Methods
        /// <summary>
        /// Create a copy of this DicomSequenceItem.
        /// </summary>
        /// <returns>The copied DicomSequenceItem.</returns>
        public override DicomAttributeCollection Copy()
        {
        	return Copy(true, true, true);
        }

    	/// <summary>
    	/// Creates a copy of this DicomSequenceItem.
    	/// </summary>
    	/// <param name="copyBinary">When set to false, the copy will not include <see cref="DicomAttribute"/>
    	/// instances that are of type <see cref="DicomAttributeOB"/>, <see cref="DicomAttributeOW"/>,
    	/// or <see cref="DicomAttributeOF"/>.</param>
    	/// <param name="copyPrivate">When set to false, the copy will not include Private tags</param>
    	/// <param name="copyUnknown">When set to false, the copy will not include UN VR tags</param>
    	/// <returns>The copied DicomSequenceItem.</returns>
    	public override DicomAttributeCollection Copy(bool copyBinary, bool copyPrivate, bool copyUnknown)
        {
            return new DicomSequenceItem(this,copyBinary,copyPrivate,copyUnknown);
        }
        #endregion

    }
}
