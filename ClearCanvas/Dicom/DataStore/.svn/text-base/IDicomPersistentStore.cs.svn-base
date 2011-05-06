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

namespace ClearCanvas.Dicom.DataStore
{
    /// <summary>
    /// Persists dicom data to the Data Store.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Any failure during a call to <see cref="UpdateSopInstance"/> should be considered a failure for the entire
    /// transaction and should not be committed (via <see cref="Commit"/>).
	/// </para>
	/// <para>
    /// Although internally, <see cref="UpdateSopInstance"/> uses <see cref="IDicomPersistentStoreValidator"/>, it is
    /// only to ensure that bad data does not get into the Data Store.  You should use the <see cref="IDicomPersistentStoreValidator"/>
    /// ahead of time to rule out unacceptable sops before calling <see cref="UpdateSopInstance"/>.  This will increase
    /// the likelihood that batch updates will succeed, containing only valid data.
	/// </para>
    /// </remarks>
	public interface IDicomPersistentStore : IDisposable
    {
		/// <summary>
		/// Adds/Updates a Sop Instance in the data store.
		/// </summary>
    	void UpdateSopInstance(DicomFile dicomFile);
    	
		/// <summary>
		/// Commits a batch set of updates, from previous calls to <see cref="UpdateSopInstance"/>.
		/// </summary>
		void Commit();
    }
}
