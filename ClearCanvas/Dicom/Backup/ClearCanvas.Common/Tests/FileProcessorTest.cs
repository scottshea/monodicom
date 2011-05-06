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

#if	UNIT_TESTS

#pragma warning disable 1591

using System;
using System.IO;
using ClearCanvas.Common.Utilities;
using NUnit.Framework;

namespace ClearCanvas.Common.Tests
{
	[TestFixture]
	public class FileProcessorTest
	{
		// The delegate
		private FileProcessor.ProcessFile _del;

		// Root test directory
		private string _testDir = Directory.GetCurrentDirectory() + @"..\..\..\..\..\UnitTestFiles\ClearCanvas.Common.Tests.FileProcessorTest";

		// The delgate function
		static void PrintPath(string path)
		{
			System.Console.WriteLine(path);
		}

		public FileProcessorTest()
		{
		}

		public string[] CreateFiles(string path, string extension, int numFiles)
		{
			string file;
			string[] fileList = new string[numFiles];
			FileStream stream;
			
			for (int i = 0; i < numFiles; i++)
			{
				file = String.Format("{0}\\File{1}{2}", path, i, extension);
				fileList[i] = file;
				stream = File.Create(file);
				// Close the file so we don't have a problem deleting the directory later
				stream.Close();
			}

			return fileList;
		}

		public string[] CreateDirectories(string path, int numDirs)
		{
			string dir;
			string[] dirList= new string[numDirs];
			
			for (int i = 0; i < numDirs; i++)
			{
				dir = String.Format("{0}\\Dir{1}", path, i);
				dirList[i] = dir;
				Directory.CreateDirectory(dir);
			}

			return dirList;
		}

		[TestFixtureSetUp]
		public void Init()
		{
			// Assign the delegate
			_del = new FileProcessor.ProcessFile(PrintPath);

			// Delete the old test directory, if it somehow didn't get deleted on teardown
			if (Directory.Exists(_testDir))
				Directory.Delete(_testDir, true);

			// Create the new test directory
			Directory.CreateDirectory(_testDir);
		}

		[TestFixtureTearDown]
		public void Cleanup()
		{
			// Get rid of the test directory
			Directory.Delete(_testDir, true);
		}

		[Test]
		public void ProcessEmptyDirectory()
		{
			FileProcessor.Process(_testDir, "", _del, true);
		}

		[Test]
		public void ProcessDirectoryWithFilesOnly()
		{
			CreateFiles(_testDir, "", 10);
			FileProcessor.Process(_testDir, "", _del, true);
		}

		[Test]
		public void ProcessDirectoryWithSubdirectoriesOnly()
		{
			CreateDirectories(_testDir, 3);
			FileProcessor.Process(_testDir, "", _del, true);
		}

		[Test]
		public void ProcessDirectoryWithFileAndSubdirectories()
		{
			string[] dirList = CreateDirectories(_testDir, 3);
			CreateFiles(_testDir, "", 5);
			CreateFiles(dirList[0], "",  6);

			FileProcessor.Process(_testDir, "", _del, true);
		}

		[Test]
		public void ProcessFileOnly()
		{
			string[] fileList = CreateFiles(_testDir, "", 1);

			FileProcessor.Process(fileList[0], "", _del, true);
		}

		[Test]
		public void ProcessWildcards()
		{
			CreateFiles(_testDir, ".txt", 5);
			CreateFiles(_testDir, ".abc", 5);
			
			FileProcessor.Process(_testDir, "*.abc", _del, true);
		}
		
		[Test]
		[ExpectedException(typeof(FileNotFoundException))]
		public void ProcessPathDoesNotExist()
		{
			FileProcessor.Process("c:\\NoSuchPath", "", _del, true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ProcessPathEmpty()
		{
			FileProcessor.Process("", "", _del, true);
		}
	}
}

#endif