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
using System.IO;

namespace ClearCanvas.Common.Utilities
{
	/// <summary>
	/// A helper class providing methods for processing files.
	/// </summary>
	public class FileProcessor
	{
		/// <summary>
		/// Delegate for use by the <see cref="FileProcessor.Process(string,string,ProcessFile,bool)"/> method.
		/// </summary>
		/// <param name="filePath">The path to the file to be processed.</param>
		public delegate void ProcessFile(string filePath);

		/// <summary>
		/// Delegate for use by the <see cref="FileProcessor.Process(string,string,ProcessFileCancellable,bool)"/> method.
		/// </summary>
		/// <param name="filePath">The path to the file to be processed.</param>
		/// <param name="cancel">Gets whether or not the entire processing operation should be cancelled.</param>
		public delegate void ProcessFileCancellable(string filePath, out bool cancel);

		/// <summary>
		/// Processes all files on the given <paramref name="path"/> matching the specified <paramref name="searchPattern"/>.
		/// </summary>
		/// <remarks>
		/// The input <paramref name="path"/> can be a file or a directory.
		/// </remarks>
		/// <param name="path">The root path to the file(s) to be processed.</param>
		/// <param name="searchPattern">The search pattern to be used.  A value of <b>null</b> or <b>""</b> indicates that all files are a match.</param>
		/// <param name="proc">The method to call for each matching file.</param>
		/// <param name="recursive">Whether or not the <paramref name="path"/> should be searched recursively.</param>
		public static void Process(string path, string searchPattern, FileProcessor.ProcessFile proc, bool recursive)
		{
			Process(path, searchPattern,
				delegate(string filePath, out bool cancel)
				{
					cancel = false;
					proc(filePath);
				},
				recursive);
		}

		/// <summary>
		/// Processes all files on the given <paramref name="path"/> matching the specified <paramref name="searchPattern"/>.
		/// </summary>
		/// <remarks>
		/// The input <paramref name="path"/> can be a file or a directory.
		/// </remarks>
		/// <param name="path">The root path to the file(s) to be processed.</param>
		/// <param name="searchPattern">The search pattern to be used.  A value of <b>null</b> or <b>""</b> indicates that all files are a match.</param>
		/// <param name="proc">The method to call for each matching file.</param>
		/// <param name="recursive">Whether or not the <paramref name="path"/> should be searched recursively.</param>
		public static bool Process(string path, string searchPattern, FileProcessor.ProcessFileCancellable proc, bool recursive)
		{
			Platform.CheckForNullReference(path, "path");
			Platform.CheckForEmptyString(path, "path");
			Platform.CheckForNullReference(proc, "proc");

			bool cancel;

			// If the path is a directory, process its contents
			if (Directory.Exists(path))
			{
				ProcessDirectory(path, searchPattern, proc, recursive, out cancel);
			}
			// If the path is a file, just process the file
			else if (File.Exists(path))
			{
				proc(path, out cancel);
			}
			else
			{
				throw new FileNotFoundException(String.Format(SR.ExceptionPathDoesNotExist, path));
			}

			return cancel;
		}

		private static void ProcessDirectory(string path, string searchPattern, FileProcessor.ProcessFileCancellable proc, bool recursive, out bool cancel)
		{
			cancel = false;

			// Process files in this directory
			string[] fileList;
			GetFiles(path, searchPattern, out fileList);

			if (fileList != null)
			{
				foreach (string file in fileList)
				{
					proc(file, out cancel);
					if (cancel)
						return;
				}
			}

			// If recursive, then descend into lower directories and process those as well
			string[] dirList;
			GetDirectories(path, searchPattern, proc, recursive, out dirList);
		}

		private static void GetFiles(string path, string searchPattern, out string[] fileList)
		{
			fileList = null;

			try
			{
				if (searchPattern == null || searchPattern == String.Empty)
					fileList = Directory.GetFiles(path);
				else
					fileList = Directory.GetFiles(path, searchPattern);
			}
			catch (Exception e)
			{
				Platform.Log(LogLevel.Warn, e);
				throw e;
			}
		}

		private static void GetDirectories(string path, string searchPattern, FileProcessor.ProcessFileCancellable proc, bool recursive, out string[] dirList)
		{
			dirList = null;

			try
			{
				dirList = Directory.GetDirectories(path);
			}
			catch (Exception e)
			{
				Platform.Log(LogLevel.Warn, e);
				throw;
			}

			if (recursive)
			{
				foreach (string dir in dirList)
				{
					bool cancel;
					ProcessDirectory(dir, searchPattern, proc, recursive, out cancel);
					if (cancel)
						break;
				}
			}
		}
	}
}
