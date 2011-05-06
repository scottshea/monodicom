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
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ClearCanvas.Dicom.Codec;
using ClearCanvas.Dicom.Network.Scu;
using ClearCanvas.Dicom.Utilities.Xml;

namespace ClearCanvas.Dicom.Samples
{
    public partial class SamplesForm : Form
    {
        #region Private Constants
        private const string STR_Cancel = "Cancel";
        private const string STR_Verify = "Verify";
        #endregion

		private readonly StorageScu _storageScu = new StorageScu();
        private readonly VerificationScu _verificationScu = new VerificationScu();
		private DicomdirReader _reader = new DicomdirReader("DICOMDIR_READER");

        public SamplesForm()
        {
			InitializeComponent();
            _buttonStorageScuVerify.Text = STR_Verify;

            Logger.RegisterLogHandler(OutputTextBox);

            if (String.IsNullOrEmpty(Properties.Settings.Default.ScpStorageFolder))
            {
                Properties.Settings.Default.ScpStorageFolder = Path.Combine(Path.GetTempPath(), "DicomImages");
            }

			_destinationSyntaxCombo.Items.Clear();
			_destinationSyntaxCombo.Items.Add(TransferSyntax.ExplicitVrLittleEndian);
			foreach (TransferSyntax syntax in DicomCodecRegistry.GetCodecTransferSyntaxes())
				_destinationSyntaxCombo.Items.Add(syntax);

        	comboBoxQueryScuQueryType_SelectedIndexChanged(null, null);
        }

        #region Button Click Handlers
        private void buttonStorageScuSelectFiles_Click(object sender, EventArgs e)
        {
			openFileDialogStorageScu.Multiselect = true;

			openFileDialogStorageScu.Filter = "DICOM files|*.dcm|All files|*.*";
			if (DialogResult.OK == openFileDialogStorageScu.ShowDialog())
			{
				foreach (String file in openFileDialogStorageScu.FileNames)
				{
					if (file != null)
						try
						{
							_storageScu.AddFileToSend(file);
						}
						catch (FileNotFoundException ex)
						{
							Logger.LogErrorException(ex, "Unexpectedly cannot find file: {0}", file);
						}
				}
			}
        }

        private void buttonStorageScuConnect_Click(object sender, EventArgs e)
        {
            int port;
            if (!int.TryParse(_textBoxStorageScuRemotePort.Text,out port))
            {
                Logger.LogError("Unable to parse port number: {0}", _textBoxStorageScuRemotePort.Text);
                return;
            }


            _storageScu.Send(_textBoxStorageScuLocalAe.Text, _textBoxStorageScuRemoteAe.Text, _textBoxStorageScuRemoteHost.Text, port);
        }

        private void buttonStorageScpStartStop_Click(object sender, EventArgs e)
        {
            if (StorageScp.Started)
            {
                _buttonStorageScpStartStop.Text = "Start";
                StorageScp.StopListening(int.Parse(_textBoxStorageScpPort.Text));
            }
            else
            {
                _buttonStorageScpStartStop.Text = "Stop";
                StorageScp.StorageLocation = _textBoxStorageScpStorageLocation.Text;
                StorageScp.StartListening(_textBoxStorageScpAeTitle.Text,
                    int.Parse(_textBoxStorageScpPort.Text));

            }

        }

        private void buttonStorageScuSelectDirectory_Click(object sender, EventArgs e)
        {
			if (DialogResult.OK == folderBrowserDialogStorageScu.ShowDialog())
			{
				if (folderBrowserDialogStorageScu.SelectedPath == null)
					return;

				_storageScu.AddDirectoryToSend(folderBrowserDialogStorageScu.SelectedPath);
			}
        }

        private void _buttonStorageScuSelectStorageLocation_Click(object sender, EventArgs e)
        {
            folderBrowserDialogStorageScp.ShowDialog();

            _textBoxStorageScpStorageLocation.Text = folderBrowserDialogStorageScp.SelectedPath;
        }

        private void _buttonStorageScuVerify_Click(object sender, EventArgs e)
        {
            if (_buttonStorageScuVerify.Text == STR_Verify)
                StartVerify();
            else
                CancelVerify();
        }

        private void _buttonOutputClearLog_Click(object sender, EventArgs e)
        {
            OutputTextBox.Text = "";
        }

        private void _buttonStorageScuClearFiles_Click(object sender, EventArgs e)
        {
            _storageScu.ClearFiles();
        }
        #endregion

        private void StartVerify()
        {
            int port;
            if (!int.TryParse(_textBoxStorageScuRemotePort.Text, out port))
            {
                Logger.LogError("Unable to parse port number: {0}", _textBoxStorageScuRemotePort.Text);
                return;
            }
            _verificationScu.BeginVerify(_textBoxStorageScuLocalAe.Text, _textBoxStorageScuRemoteAe.Text, _textBoxStorageScuRemoteHost.Text, port, new AsyncCallback(VerifyComplete), null);
            SetVerifyButton(true);
        }

        private void VerifyComplete(IAsyncResult ar)
        {
            VerificationResult verificationResult = _verificationScu.EndVerify(ar);
            Logger.LogInfo("Verify result: " + verificationResult);
            SetVerifyButton(false);
        }


        private void SetVerifyButton(bool running)
        {
            if (!InvokeRequired)
            {
                _buttonStorageScuVerify.Text = running ? STR_Cancel : STR_Verify;
            }
            else
            {
                BeginInvoke(new Action<bool>(SetVerifyButton), new object[] { running });
            }
        }

        private void CancelVerify()
        {
            _verificationScu.Cancel();
        }

    	private Compression _compression;

		private void _openFileButton_Click(object sender, EventArgs e)
		{
			openFileDialogStorageScu.Multiselect = false;
			openFileDialogStorageScu.Filter = "DICOM files|*.dcm|All files|*.*";
			if (DialogResult.OK == openFileDialogStorageScu.ShowDialog())
			{
				_sourcePathTextBox.Text = openFileDialogStorageScu.FileName;
				_destinationPathTextBox.Text = string.Empty;

				_compression = new Compression(openFileDialogStorageScu.FileName);
				_compression.Load();

				_sourceTransferSyntaxCombo.Items.Clear();
				_sourceTransferSyntaxCombo.Items.Add(_compression.DicomFile.TransferSyntax);
				_sourceTransferSyntaxCombo.SelectedItem = _compression.DicomFile.TransferSyntax;
			}
		}

		private void _saveFileButton_Click(object sender, EventArgs e)
		{
			if (_compression != null)
			{
				TransferSyntax destinationSyntax = _destinationSyntaxCombo.SelectedItem as TransferSyntax;

				string dump = _compression.DicomFile.Dump();
				Logger.LogInfo(dump);

				_compression.ChangeSyntax(destinationSyntax);

				dump = _compression.DicomFile.Dump();
				Logger.LogInfo(dump);

				saveFileDialog.Filter = "DICOM|*.dcm";
				if (DialogResult.OK == saveFileDialog.ShowDialog())
				{
					_destinationPathTextBox.Text = saveFileDialog.FileName;
					_compression.Save(saveFileDialog.FileName);
				}
			}
		}

		private void _savePixelsButton_Click(object sender, EventArgs e)
		{
			if (_compression != null)
			{
				if (!_compression.DicomFile.TransferSyntax.Encapsulated)
					saveFileDialog.Filter = "RAW|*.raw";
				else if (_compression.DicomFile.TransferSyntax.Equals(TransferSyntax.Jpeg2000ImageCompression)
					|| _compression.DicomFile.TransferSyntax.Equals(TransferSyntax.Jpeg2000ImageCompressionLosslessOnly))
					saveFileDialog.Filter = "JPEG 2000|*.j2k";
				else if (_compression.DicomFile.TransferSyntax.Equals(TransferSyntax.RleLossless))
					saveFileDialog.Filter = "RLE|*.rle";
				else
					saveFileDialog.Filter = "JPEG|*.jpg";

				if (DialogResult.OK == saveFileDialog.ShowDialog())
					_compression.SavePixels(saveFileDialog.FileName);
			}
		}

		private void comboBoxQueryScuQueryType_SelectedIndexChanged(object sender, EventArgs e)
		{
			XmlDocument doc = new XmlDocument();

			if (comboBoxQueryScuQueryType.SelectedIndex == 0)
			{
				Stream stream = GetType().Assembly.GetManifestResourceStream(GetType(), "StudyRootStudy.xml");
				if (stream != null)
				{
					doc.Load(stream);
					stream.Close();
				}
				comboBoxQueryScuQueryLevel.Items.Clear();
				comboBoxQueryScuQueryLevel.Items.Add("STUDY");
				comboBoxQueryScuQueryLevel.Items.Add("SERIES");
				comboBoxQueryScuQueryLevel.Items.Add("IMAGE");
			}
			else
			{
				Stream stream = GetType().Assembly.GetManifestResourceStream(GetType(), "PatientRootPatient.xml");
				if (stream != null)
				{
					doc.Load(stream);
					stream.Close();
				}
				comboBoxQueryScuQueryLevel.Items.Clear();
				comboBoxQueryScuQueryLevel.Items.Add("PATIENT");
				comboBoxQueryScuQueryLevel.Items.Add("STUDY");
				comboBoxQueryScuQueryLevel.Items.Add("SERIES");
				comboBoxQueryScuQueryLevel.Items.Add("IMAGE");
			}

			StringWriter sw = new StringWriter();

			XmlWriterSettings xmlSettings = new XmlWriterSettings();

			xmlSettings.Encoding = Encoding.UTF8;
			xmlSettings.ConformanceLevel = ConformanceLevel.Fragment;
			xmlSettings.Indent = true;
			xmlSettings.NewLineOnAttributes = false;
			xmlSettings.CheckCharacters = true;
			xmlSettings.IndentChars = "  ";

			XmlWriter tw = XmlWriter.Create(sw, xmlSettings);
			if (tw != null)
			{
				doc.WriteTo(tw);
				tw.Close();
			}
			textBoxQueryMessage.Text = sw.ToString();
		}

		private void buttonQueryScuSearch_Click(object sender, EventArgs e)
		{
			XmlDocument theDoc = new XmlDocument();

			try
			{
				theDoc.LoadXml(textBoxQueryMessage.Text);
				InstanceXml instanceXml = new InstanceXml(theDoc.DocumentElement, null);
				DicomAttributeCollection queryMessage = instanceXml.Collection;

				if (queryMessage == null)
				{
					Logger.LogError("Unexpected error parsing query message");
				}

				int maxResults;
				if (!int.TryParse(textBoxQueryScuMaxResults.Text, out maxResults))
					maxResults = -1;

				IList<DicomAttributeCollection> resultsList;
				if (comboBoxQueryScuQueryType.SelectedIndex == 0)
				{
					StudyRootFindScu findScu = new StudyRootFindScu();
					findScu.MaxResults = maxResults;
					resultsList = findScu.Find(textBoxQueryScuLocalAe.Text,
								 textBoxQueryScuRemoteAe.Text,
								 textBoxQueryScuRemoteHost.Text,
								 int.Parse(textBoxQueryScuRemotePort.Text), queryMessage);
					findScu.Dispose();
				}
				else
				{
					PatientRootFindScu findScu = new PatientRootFindScu();
					findScu.MaxResults = maxResults;
					resultsList = findScu.Find(textBoxQueryScuLocalAe.Text,
								 textBoxQueryScuRemoteAe.Text,
								 textBoxQueryScuRemoteHost.Text,
								 int.Parse(textBoxQueryScuRemotePort.Text), queryMessage);
					findScu.Dispose();
				}

				foreach (DicomAttributeCollection msg in resultsList)
				{
					Logger.LogInfo(msg.DumpString);
				}
			}
			catch (Exception x)
			{
				Logger.LogErrorException(x, "Unable to perform query");
				return;
			}		
		}

		private void comboBoxQueryScuQueryLevel_SelectedIndexChanged(object sender, EventArgs e)
		{
			XmlDocument doc = new XmlDocument();

			string xmlFile;
			if (comboBoxQueryScuQueryType.SelectedIndex == 0)
			{
				if (comboBoxQueryScuQueryLevel.SelectedItem.Equals("STUDY"))
					xmlFile = "StudyRootStudy.xml";
				else if (comboBoxQueryScuQueryLevel.SelectedItem.Equals("SERIES"))
					xmlFile = "StudyRootSeries.xml";
				else 
					xmlFile = "StudyRootImage.xml";
			}
			else
			{
				if (comboBoxQueryScuQueryLevel.SelectedItem.Equals("PATIENT"))
					xmlFile = "PatientRootPatient.xml";
				else if (comboBoxQueryScuQueryLevel.SelectedItem.Equals("STUDY"))
					xmlFile = "PatientRootStudy.xml";
				else if (comboBoxQueryScuQueryLevel.SelectedItem.Equals("SERIES"))
					xmlFile = "PatientRootSeries.xml";
				else 
					xmlFile = "PatientRootImage.xml";
			}

			Stream stream = GetType().Assembly.GetManifestResourceStream(GetType(), xmlFile);
			if (stream != null)
			{
				doc.Load(stream);
				stream.Close();
			}

			StringWriter sw = new StringWriter();

			XmlWriterSettings xmlSettings = new XmlWriterSettings();

			xmlSettings.Encoding = Encoding.UTF8;
			xmlSettings.ConformanceLevel = ConformanceLevel.Fragment;
			xmlSettings.Indent = true;
			xmlSettings.NewLineOnAttributes = false;
			xmlSettings.CheckCharacters = true;
			xmlSettings.IndentChars = "  ";

			XmlWriter tw = XmlWriter.Create(sw, xmlSettings);

			if (tw != null)
			{
				doc.WriteTo(tw); 
				tw.Close();
			}

			textBoxQueryMessage.Text = sw.ToString();
		}

		private void _buttonOpenDicomdir_Click(object sender, EventArgs e)
		{
			openFileDialogStorageScu.Filter = "DICOMDIR|*.*|DICOM files|*.dcm";
			openFileDialogStorageScu.FileName = String.Empty;
			openFileDialogStorageScu.Multiselect = false;
			if (DialogResult.OK == openFileDialogStorageScu.ShowDialog())
			{
				_textBoxDicomdir.Text = openFileDialogStorageScu.FileName;

				_reader = new DicomdirReader("DICOMDIR_READER");
				_reader.Load(openFileDialogStorageScu.FileName);

				DicomdirDisplay display = new DicomdirDisplay();
				display.Add(_reader.Dicomdir);

				display.Show(this);
				
			}
		}

		private void buttonSendDicomdir_Click(object sender, EventArgs e)
		{
			string rootDirectory = Path.GetDirectoryName(_textBoxDicomdir.Text);

			_reader.Send(rootDirectory, _textBoxDicomdirRemoteAe.Text, _textBoxDicomdirRemoteHost.Text, int.Parse(_textBoxDicomdirRemotePort.Text));
		}
    }
}