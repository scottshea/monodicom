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

namespace ClearCanvas.Common.Utilities
{
	/// <summary>
	/// Helper class for raising events.
	/// </summary>
	public class EventsHelper
	{
		/// <summary>
		/// Helper method for raising events safely.
		/// </summary>
		/// <param name="del">Delegate to invoke.</param>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/>.</param>
		/// <remarks>
		/// Use this method to invoke user code via delegates.
		/// This method will log any exceptions thrown in user code and immediately rethrow it.
		/// The typical usage is shown below.
		/// </remarks>
		/// <example>
		/// <code>
		/// [C#]
		/// public class PresentationImage
		/// {
		///    private event EventHandler _imageDrawingEvent;
		///    
		///    public void Draw()
		///    {
		///       EventsHelper.Fire(_imageDrawingEvent, this, new DrawImageEventArgs());
		///    }
		/// }
		/// </code>
		/// </example>
		public static void Fire(Delegate del, object sender, EventArgs e)
		{
			if (del == null)
				return;

			Delegate[] delegates = del.GetInvocationList();

			foreach(Delegate sink in delegates)
			{
				try
				{
					sink.DynamicInvoke(sender, e);
				}
				catch (Exception ex)
				{
                    Platform.Log(LogLevel.Error, ex);
					throw;
				}
			}
		}
	}
}
