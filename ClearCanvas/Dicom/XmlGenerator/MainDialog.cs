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
using System.Windows.Forms;
using System.Xml;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.Utilities.Xml;

namespace ClearCanvas.Dicom.XmlGenerator
{
    public partial class MainDialog : Form
    {
        StudyXml _theStream = new StudyXml();

        public MainDialog()
        {
            InitializeComponent();
        }


        private void ButtonLoadFile_Click(object sender, EventArgs e)
        {
            openFileDialog.DefaultExt = "dcm";
            openFileDialog.ShowDialog();

            DicomFile dicomFile = new DicomFile(openFileDialog.FileName);

            DicomReadOptions options = new DicomReadOptions();

            dicomFile.Load(options);
            


            _theStream.AddFile(dicomFile);

        }

        private void _buttonLoadDirectory_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowDialog();

            String directory = folderBrowserDialog.SelectedPath;
			if (directory.Equals(String.Empty))
				return;

            DirectoryInfo dir = new DirectoryInfo(directory);

            LoadFiles(dir);
			

        }


        private void LoadFiles(DirectoryInfo dir)
        {
         
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {

                Dicom.DicomFile dicomFile = new Dicom.DicomFile(file.FullName);

                try
                {

                    DicomReadOptions options = new DicomReadOptions();


                    dicomFile.Load(options);
                    _theStream.AddFile(dicomFile);
                    /*
					if (true == dicomFile.Load())
					{
						_theStream.AddFile(dicomFile);
					}
                     * */
                }
                catch (DicomException) 
                {
                    // TODO:  Add some logging for failed files
                }

            }

            String[] subdirectories = Directory.GetDirectories(dir.FullName);
            foreach (String subPath in subdirectories)
            {
                DirectoryInfo subDir = new DirectoryInfo(subPath);
                LoadFiles(subDir);
                continue;
            }

        }

        private void _buttonGenerateXml_Click(object sender, EventArgs e)
        {
            saveFileDialog.DefaultExt = "xml";
            saveFileDialog.ShowDialog();

			StudyXmlOutputSettings settings = new StudyXmlOutputSettings();
        	settings.IncludeSourceFileName = false;
            XmlDocument doc = _theStream.GetMemento(settings);

            Stream fileStream = saveFileDialog.OpenFile();

            StudyXmlIo.Write(doc, fileStream);

            fileStream.Close();
        }

        private void _buttonLoadXml_Click(object sender, EventArgs e)
        {
            openFileDialog.DefaultExt = "xml";
            openFileDialog.ShowDialog();

            Stream fileStream = openFileDialog.OpenFile();

            XmlDocument theDoc = new XmlDocument();

            StudyXmlIo.Read(theDoc, fileStream);

            fileStream.Close();

            _theStream = new StudyXml();

            _theStream.SetMemento(theDoc);
        }

        private void _buttonGenerateGzipXml_Click(object sender, EventArgs e)
        {
            saveFileDialog.DefaultExt = "gzip";

            saveFileDialog.ShowDialog();

            String file = saveFileDialog.FileName;
            XmlDocument doc = _theStream.GetMemento(StudyXmlOutputSettings.None);

            Stream fileStream = saveFileDialog.OpenFile();

            StudyXmlIo.WriteGzip(doc, fileStream);

            fileStream.Close();
        }

        private void _buttonLoadGzipXml_Click(object sender, EventArgs e)
        {
            openFileDialog.DefaultExt = "gzip";
            openFileDialog.ShowDialog();

            Stream fileStream = openFileDialog.OpenFile();

            XmlDocument theDoc = new XmlDocument();

            StudyXmlIo.ReadGzip(theDoc, fileStream);

            fileStream.Close();

            _theStream = new StudyXml();

            _theStream.SetMemento(theDoc);
        }
    }
}