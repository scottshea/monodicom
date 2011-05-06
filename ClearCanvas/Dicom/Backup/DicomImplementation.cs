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

namespace ClearCanvas.Dicom
{
    /// <summary>
    /// Stores DICOM implementation specific information.
    /// </summary>
    public static class DicomImplementation
    {
        #region Private Static Members
        private static DicomUid _classUid = new DicomUid("1.3.6.1.4.1.25403.1.1.1", "Implementation Class UID", UidType.Unknown);
        private static string _version = "Dicom 0.1";
        private static IDicomCharacterSetParser _characterParser = new SpecificCharacterSetParser();
        private static bool _unitTest = false;
        #endregion

        #region Public Static Properties
        /// <summary>
        /// Unit tests are currently being run.
        /// </summary>
        public static bool UnitTest
        {
            get { return _unitTest; }
            set { _unitTest = value; }
        }
        /// <summary>
        /// The DICOM Implementation Class UID.
        /// </summary>
        public static DicomUid ClassUID
        {
            get { return _classUid; }
            set { _classUid = value; }        
        }

        /// <summary>
        /// The DICOM Implementation Version.
        /// </summary>
        public static string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>
        /// The Specific Character Set Parser used by the implementation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property sets the parser to be used to translate between raw bytes encoded in a
        /// DICOM stream for text attributes and the unicode characters stored by the assembly 
        /// for text attributes.  A default implementation is included, which can be overridden.
        /// </para>
        /// <para>
        /// See the <see cref="IDicomCharacterSetParser"/> interface for the methods required 
        /// to be implemented for a character set parser.
        /// </para>
        /// </remarks>
        public static IDicomCharacterSetParser CharacterParser
        {
            get { return _characterParser; }
            set { _characterParser = value; }
        }
        #endregion
    }

}
