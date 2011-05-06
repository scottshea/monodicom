using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Dicom;
using Dicom.Data;

namespace DicomBulkAnonymizer
{
    public partial class FieldEditorForm : Form
    {
        public FieldEditorForm()
        {
            InitializeComponent();
            populateDicomFieldsListBox();
        }

        private void populateDicomFieldsListBox()
        {
            lbDicomFields.SuspendLayout();
            lbDicomFields.Items.Clear();
            foreach (DcmDictionaryEntry entry in DcmDictionary.Entries)
            {
                lbDicomFields.Items.Add(entry.Name);
            }
            if (lbDicomFields.Items.Count > 0)
                lbDicomFields.SelectedIndex = 0;
            lbDicomFields.ResumeLayout();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbDicomFields.Items.Count > 0)
                {
                    lbSelectedDicomFields.Items.Add(lbDicomFields.SelectedItem.ToString());
                    lbDicomFields.Items.Remove(lbDicomFields.SelectedItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (lbDicomFields.Items.Count > 0)
            {
                lbDicomFields.SelectedIndex = 0;
            }
            lbSelectedDicomFields.SelectedIndex = lbSelectedDicomFields.Items.Count - 1;
        }

        private void bntRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbSelectedDicomFields.Items.Count > 0)
                {
                    lbDicomFields.Items.Add(lbDicomFields.SelectedItem.ToString());
                    lbSelectedDicomFields.Items.Remove(lbSelectedDicomFields.SelectedItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (lbSelectedDicomFields.Items.Count > 0)
            {
                lbSelectedDicomFields.SelectedIndex = 0;
            }
            lbDicomFields.SelectedIndex = 0;
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            try
            {
                lbSelectedDicomFields.Items.AddRange(lbDicomFields.Items);
                lbDicomFields.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            lbSelectedDicomFields.SelectedIndex = 0;
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            try
            {
                lbDicomFields.Items.AddRange(lbSelectedDicomFields.Items);
                lbSelectedDicomFields.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            lbDicomFields.SelectedIndex = 0;
        }

    }
}
