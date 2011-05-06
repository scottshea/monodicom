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

namespace ClearCanvas.Common
{
    /// <summary>
    /// Enum flags describing the buttons/options that should be available to the user in a dialog box.
    /// </summary>
    /// <remarks>
    /// These flags can be combined together using the | operator to specify multiple buttons.
    /// </remarks>
	[Flags]
    public enum DialogBoxAction
    {
		/// <summary>
		/// An Ok button should be shown.
		/// </summary>
        Ok      = 0x0001,

		/// <summary>
		/// A Cancel button should be shown.
		/// </summary>
        Cancel  = 0x0002,

		/// <summary>
		/// A Yes button should be shown.
		/// </summary>
        Yes     = 0x0004,

		/// <summary>
		/// A No button should be shown.
		/// </summary>
        No      = 0x0008,
    }

	/// <summary>
	/// Enum flags specific to message boxes, which are just 
	/// commonly used combinations of <see cref="DialogBoxAction"/>s.
	/// </summary>
    public enum MessageBoxActions
    {
        /// <summary>
        /// An Ok button should be shown.
        /// </summary>
		Ok = DialogBoxAction.Ok,

		/// <summary>
		/// Both an Ok and a Cancel button should be shown.
		/// </summary>
        OkCancel = DialogBoxAction.Ok | DialogBoxAction.Cancel,

		/// <summary>
		/// Both a Yes and No button should be shown.
		/// </summary>
        YesNo = DialogBoxAction.Yes | DialogBoxAction.No,

		/// <summary>
		/// Yes, No and Cancel buttons should be shown.
		/// </summary>
        YesNoCancel = DialogBoxAction.Yes | DialogBoxAction.No | DialogBoxAction.Cancel
    }

	/// <summary>
	/// An interface for a message box.
	/// </summary>
	public interface IMessageBox
	{
		/// <summary>
		/// Shows a message box displaying the input <paramref name="message"/>, 
		/// usually with an Ok button.
		/// </summary>
		void Show(string message);

		/// <summary>
		/// Shows a message box displaying the input <paramref name="message"/>
		/// and the specified <paramref name="buttons"/>.
		/// </summary>
		DialogBoxAction Show(string message, MessageBoxActions buttons);
	}
}
