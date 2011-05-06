using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Dicom;
using Dicom.Data;

namespace DicomBulkAnonymizer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            if (File.Exists("dicom.dic"))
                DcmDictionary.ImportDictionary("dicom.dic");
            else
                DcmDictionary.LoadInternalDictionary();
        }

        private void btnFieldEditor_Click(object sender, EventArgs e)
        {
            FieldEditorForm fieldForm = new FieldEditorForm();
            fieldForm.Show();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            QuickAnonymizationForm quick = new QuickAnonymizationForm();
            quick.Show();
        }

    }
}
