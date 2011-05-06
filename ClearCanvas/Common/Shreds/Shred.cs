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

namespace ClearCanvas.Common.Shreds
{
	/// <summary>
	/// Abstract base implementation of <see cref="IShred"/>.  Shred implementations should inherit
	/// from this class rather than implement <see cref="IShred"/> directly.
	/// </summary>
    public abstract class Shred : MarshalByRefObject, IShred
    {
		///<summary>
		///Obtains a lifetime service object to control the lifetime policy for this instance.
		///</summary>
		public override object InitializeLifetimeService()
        {
            // cause lifetime lease to never expire
            return null;
        }

        #region IShred Members

		/// <summary>
		/// Called to start the shred.
		/// </summary>
		/// <remarks>
		/// This method should perform any initialization of the shred, and then return immediately.
		/// </remarks>
        public abstract void Start();

		/// <summary>
		/// Called to stop the shred.
		/// </summary>
		/// <remarks>
		/// This method should perform any necessary clean-up, and then return immediately.
		/// </remarks>
        public abstract void Stop();

		/// <summary>
		/// Gets the display name of the shred.
		/// </summary>
		/// <returns></returns>
        public abstract string GetDisplayName();

		/// <summary>
		/// Gets a description of the shred.
		/// </summary>
		/// <returns></returns>
        public abstract string GetDescription();

        #endregion       
    }
}
