using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Monodicom.DicomInfoViewer
{
    public partial class mainForm : Form
    {
        private int sortColumn = -1;
        private string _selectedFile;

        DicomInfo gdi = new DicomInfo();

        #region Public Area
        public mainForm()
        {
            InitializeComponent();
            InitializeListView();
            InitializeDicomDataListView();
        }

        public void InitializeListView()
        {
            this.colFiles.Width = 30 * Convert.ToInt32(lstvwFiles.Font.SizeInPoints);
            this.lstvwFiles.ColumnClick += new ColumnClickEventHandler(lstvwFiles_ColumnClick);
        }

        public void InitializeDicomDataListView()
        {
            this.colElementName.Width = 30 * Convert.ToInt32(lstvwFiles.Font.SizeInPoints);
            this.colElementTagId.Width = -2;
            this.colElementValue.Width = 30 * Convert.ToInt32(lstvwFiles.Font.SizeInPoints);
            this.lstViewDicomData.ColumnClick += new ColumnClickEventHandler(lstViewDicomData_ColumnClick);
            this.lstViewDicomData.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstViewDicomData_MouseDoubleClick);
        }
        #endregion

        #region Private Area
        private void populateListView(string dir)
        {
            lstvwFiles.Items.Clear();
            DirectoryInfo nodeDirInfo = new DirectoryInfo(dir);
            ListViewItem item = null;

            foreach (FileInfo file in nodeDirInfo.GetFiles())
            {
                if(file.Name.ToString().EndsWith(".dcm"))
                {
                    item = new ListViewItem(file.Name,1);
                    lstvwFiles.Items.Add(item);
                }
            }
            this.colFiles.Width = -1;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowDialog();
            txtFolder.Text = folderBrowserDialog.SelectedPath;
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            populateListView(txtFolder.Text);
        }

        private void lstvwFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selectedDicomFile = this.lstvwFiles.SelectedItems;
            lstViewDicomData.Items.Clear();
            foreach (ListViewItem item in selectedDicomFile)
            {
                List<Dictionary<string, string>> elements = new List<Dictionary<string, string>>();
                ListViewItem lvItem = null;
                ListViewItem.ListViewSubItem[] subitem;
                _selectedFile = txtFolder.Text + "\\" + item.Text;
                elements = gdi.getDicomElementInfo(item.Text, txtFolder.Text);
                foreach (Dictionary<string, string> baseData in elements)
                {
                    lvItem = new ListViewItem(baseData["Name"], 1);
                    subitem = new ListViewItem.ListViewSubItem[] { new ListViewItem.ListViewSubItem(item, baseData["Tag"]), new ListViewItem.ListViewSubItem(item, baseData["Value"]) };
                    lvItem.SubItems.AddRange(subitem);
                    lstViewDicomData.Items.Add(lvItem);
                }
            }
            this.colElementName.Width = -1;
            this.colElementValue.Width = -1;
        }

        private void lstvwFiles_ColumnClick(object o, ColumnClickEventArgs e)
        {
            if (e.Column != sortColumn)
            {
                sortColumn = e.Column;
                lstvwFiles.Sorting = SortOrder.Ascending;
            }
            else
            {
                if (lstvwFiles.Sorting == SortOrder.Ascending)
                    lstvwFiles.Sorting = SortOrder.Descending;
                else
                    lstvwFiles.Sorting = SortOrder.Ascending;
            }
            lstvwFiles.Sort();
            this.lstvwFiles.ListViewItemSorter = new ListViewItemComparer(e.Column,lstvwFiles.Sorting);
        }

        private void lstViewDicomData_ColumnClick(object o, ColumnClickEventArgs e)
        {
            if (e.Column != sortColumn)
            {
                sortColumn = e.Column;
                lstViewDicomData.Sorting = SortOrder.Ascending;
            }
            else
            {
                if (lstViewDicomData.Sorting == SortOrder.Ascending)
                    lstViewDicomData.Sorting = SortOrder.Descending;
                else
                    lstViewDicomData.Sorting = SortOrder.Ascending;
            }
            lstViewDicomData.Sort();
            this.lstViewDicomData.ListViewItemSorter = new ListViewItemComparer(e.Column, lstViewDicomData.Sorting);
        }

        private void lstViewDicomData_MouseDoubleClick(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selectedElement = this.lstViewDicomData.SelectedItems;

            UpdateForm updateForm = new UpdateForm();
            updateForm.DicomElementName = selectedElement[0].Text;
            updateForm.UpdateFile = _selectedFile;
            updateForm.Show();
        }
        
        #endregion
    }

    class ListViewItemComparer : IComparer
    {
        private int col;
        private SortOrder order;
        public ListViewItemComparer()
        {
            col = 0;
            order = SortOrder.Ascending;
        }
        public ListViewItemComparer(int column, SortOrder order)
        {
            col = column;
            this.order = order;
        }
        public int Compare(object x, object y) 
        {
            int returnVal= -1;
            returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text,((ListViewItem)y).SubItems[col].Text);
            if (order == SortOrder.Descending)
                returnVal *= -1;
            return returnVal;
        }
    }
}
