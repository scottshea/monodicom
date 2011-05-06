namespace Monodicom.DicomInfoViewer
{
    partial class mainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstvwFiles = new System.Windows.Forms.ListView();
            this.colFiles = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.btnActivate = new System.Windows.Forms.Button();
            this.lstViewDicomData = new System.Windows.Forms.ListView();
            this.colElementName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colElementTagId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colElementValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lstvwFiles
            // 
            this.lstvwFiles.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lstvwFiles.AllowColumnReorder = true;
            this.lstvwFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFiles});
            this.lstvwFiles.HoverSelection = true;
            this.lstvwFiles.Location = new System.Drawing.Point(13, 13);
            this.lstvwFiles.Name = "lstvwFiles";
            this.lstvwFiles.Size = new System.Drawing.Size(402, 354);
            this.lstvwFiles.TabIndex = 0;
            this.lstvwFiles.UseCompatibleStateImageBehavior = false;
            this.lstvwFiles.View = System.Windows.Forms.View.Details;
            this.lstvwFiles.SelectedIndexChanged += new System.EventHandler(this.lstvwFiles_SelectedIndexChanged);
            // 
            // colFiles
            // 
            this.colFiles.Tag = "";
            this.colFiles.Text = "Files";
            this.colFiles.Width = 25;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(12, 373);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.Location = new System.Drawing.Point(93, 375);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(196, 20);
            this.txtFolder.TabIndex = 2;
            // 
            // btnActivate
            // 
            this.btnActivate.Location = new System.Drawing.Point(12, 402);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(75, 23);
            this.btnActivate.TabIndex = 6;
            this.btnActivate.Text = "Activate";
            this.btnActivate.UseVisualStyleBackColor = true;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // lstViewDicomData
            // 
            this.lstViewDicomData.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lstViewDicomData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colElementName,
            this.colElementTagId,
            this.colElementValue});
            this.lstViewDicomData.Location = new System.Drawing.Point(431, 13);
            this.lstViewDicomData.Name = "lstViewDicomData";
            this.lstViewDicomData.Size = new System.Drawing.Size(428, 354);
            this.lstViewDicomData.TabIndex = 7;
            this.lstViewDicomData.UseCompatibleStateImageBehavior = false;
            this.lstViewDicomData.View = System.Windows.Forms.View.Details;
            // 
            // colElementName
            // 
            this.colElementName.Text = "Element Name";
            this.colElementName.Width = 25;
            // 
            // colElementTagId
            // 
            this.colElementTagId.Text = "Element Tag Id";
            this.colElementTagId.Width = 84;
            // 
            // colElementValue
            // 
            this.colElementValue.Text = "Element Value";
            this.colElementValue.Width = 25;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 431);
            this.Controls.Add(this.lstViewDicomData);
            this.Controls.Add(this.btnActivate);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.lstvwFiles);
            this.Name = "mainForm";
            this.Text = "Protobuff Viewer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lstvwFiles;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button btnActivate;
        private System.Windows.Forms.ColumnHeader colFiles;
        private System.Windows.Forms.ListView lstViewDicomData;
        private System.Windows.Forms.ColumnHeader colElementName;
        private System.Windows.Forms.ColumnHeader colElementTagId;
        private System.Windows.Forms.ColumnHeader colElementValue;
    }
}

