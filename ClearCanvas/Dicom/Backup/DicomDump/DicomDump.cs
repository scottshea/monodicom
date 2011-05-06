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
using System.Collections.Generic;
using System.Text;

using ClearCanvas.Dicom;
using ClearCanvas.Dicom.IO;

namespace ClearCanvas.Dicom.DicomDump
{
    class DicomDump
    {

        static DicomDumpOptions _options = DicomDumpOptions.Default;

        static void PrintCommandLine()
        {
            Console.WriteLine("DicomDump Parameters:");
            Console.WriteLine("\t-h\tDisplay this help information");
            Console.WriteLine("\t-g\tInclude group lengths");
            Console.WriteLine("\t-c\tAllow more than 80 characters per line");
            Console.WriteLine("\t-l\tDisplay long values");
            Console.WriteLine("All other parameters are considered filenames to list.");
        }

        static bool ParseArgs(string[] args)
        {
            foreach (String arg in args)
            {
                if (arg.ToLower().Equals("-h"))
                {
                    PrintCommandLine();
                    return false;
                }
                else if (arg.ToLower().Equals("-g"))
                {
                    _options &= ~DicomDumpOptions.KeepGroupLengthElements;
                }
                else if (arg.ToLower().Equals("-c"))
                {
                    _options &= ~DicomDumpOptions.Restrict80CharactersPerLine;
                }
                else if (arg.ToLower().Equals("-l"))
                {
                    _options &= ~DicomDumpOptions.ShortenLongValues;
                }
            }
            return true;
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintCommandLine();
                return;
            }

            if (false == ParseArgs(args))
                return;

            foreach (String filename in args)
            {
                if (filename.StartsWith("-"))
                    continue;

                DicomFile file = new DicomFile(filename);

                DicomReadOptions readOptions = DicomReadOptions.Default;

				try
				{
					file.Load(readOptions);
				}
				catch (Exception e)
				{
					Console.WriteLine("Unexpected exception when loading file: {0}", e.Message);
				}

                StringBuilder sb = new StringBuilder();

                file.Dump(sb, "", _options);

                Console.WriteLine(sb.ToString());
            }
        }
    }
}
