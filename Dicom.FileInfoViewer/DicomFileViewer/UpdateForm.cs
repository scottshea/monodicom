using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Monodicom.Utilities;

namespace Monodicom.DicomInfoViewer
{
    public partial class UpdateForm : Form
    {
        private string _dicomElementName;
        private string _updateTargetFile;
        private string _dicomTag;

        public string DicomElementName
        {
            get { return _dicomElementName; }
            set { _dicomElementName = value; }
        }

        public string UpdateFile
        {
            get { return _updateTargetFile; }
            set { _updateTargetFile = value; }
        }

        public UpdateForm()
        {
            InitializeComponent();
        }
        
        private void UpdateForm_Load(object sender, EventArgs e)
        {
            DicomInfo targetFileElements = new DicomInfo();
            List<Dictionary<string, string>> lookupElement = new List<Dictionary<string, string>>();

            this.lblDicomTagName.Text = _dicomElementName;
            lookupElement = targetFileElements.getDicomElementInfo(_updateTargetFile);
            foreach (Dictionary<string, string> individualElements in lookupElement)
            {
                if (individualElements["Name"].Equals(_dicomElementName))
                {
                    this.lblDicomTag.Text = individualElements["Tag"];
                    this.lblDicomValueOriginal.Text = individualElements["Value"];
                    _dicomTag = individualElements["Tag"];
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DicomUpdate updateTargetFile = new DicomUpdate();
            if (chkboxApplyToAllFiles.Checked == true)
            {
                string[] studyFiles;
                string parentDir = Directory.GetParent(_updateTargetFile).FullName;
                studyFiles = Directory.GetFiles(parentDir);
                foreach (string file in studyFiles)
                {
                    if (file.EndsWith(".dcm"))
                    {
                        updateTargetFile.UpdateDicomFile(file, _dicomTag, txtboxNewValue.Text);
                    }
                }
                lblStatusMsg.Text = "Dicom element " + _dicomTag + "," + _dicomElementName + " updated for all files";
                txtboxNewValue.Text = "";
            }

            else
            {
                updateTargetFile.UpdateDicomFile(_updateTargetFile, _dicomTag, txtboxNewValue.Text);
                lblStatusMsg.Text = "Dicom element " + _dicomTag + "," + _dicomElementName + " updated";
                lblDicomValueOriginal.Text = txtboxNewValue.Text;
                txtboxNewValue.Text = "";
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
