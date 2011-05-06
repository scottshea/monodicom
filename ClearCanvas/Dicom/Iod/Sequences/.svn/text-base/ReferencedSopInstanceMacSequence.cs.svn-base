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

using System;

namespace ClearCanvas.Dicom.Iod.Sequences
{
	/// <summary>
	/// ReferencedSopInstanceMac Sequence
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.17.2.1 (Table C.17-3a)</remarks>
	public class ReferencedSopInstanceMacSequence : SequenceIodBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ReferencedSopInstanceMacSequence"/> class.
		/// </summary>
		public ReferencedSopInstanceMacSequence() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReferencedSopInstanceMacSequence"/> class.
		/// </summary>
		/// <param name="dicomSequenceItem">The dicom sequence item.</param>
		public ReferencedSopInstanceMacSequence(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

		/// <summary>
		/// Gets or sets the value of MacCalculationTransferSyntaxUid in the underlying collection. Type 1.
		/// </summary>
		public string MacCalculationTransferSyntaxUid
		{
			get { return base.DicomAttributeProvider[DicomTags.MacCalculationTransferSyntaxUid].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException("value", "MacCalculationTransferSyntaxUid is Type 1 Required.");
				base.DicomAttributeProvider[DicomTags.MacCalculationTransferSyntaxUid].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of MacAlgorithm in the underlying collection. Type 1.
		/// </summary>
		public MacAlgorithm MacAlgorithm
		{
			get { return ParseEnum(base.DicomAttributeProvider[DicomTags.MacAlgorithm].GetString(0, string.Empty), MacAlgorithm.Unknown); }
			set
			{
				if (value == MacAlgorithm.Unknown)
					throw new ArgumentOutOfRangeException("value", "MacAlgorithm is Type 1 Required.");
				SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.MacAlgorithm], value);
			}
		}

		/// <summary>
		/// Gets or sets the value of DataElementsSigned in the underlying collection. Type 1.
		/// </summary>
		public uint[] DataElementsSigned
		{
			get { return (uint[]) base.DicomAttributeProvider[DicomTags.DataElementsSigned].Values; }
			set
			{
				if (value == null || value.Length == 0)
					throw new ArgumentNullException("value", "DataElementsSigned is Type 1 Required.");
				base.DicomAttributeProvider[DicomTags.DataElementsSigned].Values = value;
			}
		}

		/// <summary>
		/// Gets or sets the value of Mac in the underlying collection. Type 1.
		/// </summary>
		public byte[] Mac
		{
			get { return (byte[]) base.DicomAttributeProvider[DicomTags.Mac].Values; }
			set
			{
				if (value == null || value.Length == 0)
					throw new ArgumentNullException("value", "Mac is Type 1 Required.");
				base.DicomAttributeProvider[DicomTags.Mac].Values = value;
			}
		}
	}
}