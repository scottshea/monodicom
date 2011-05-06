namespace DicomBulkAnonymizer
{
    partial class QuickAnonymizationForm
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
            this.lblPatientId = new System.Windows.Forms.Label();
            this.txtPatientIdInput = new System.Windows.Forms.TextBox();
            this.lblPatientName = new System.Windows.Forms.Label();
            this.txtPatientNameInput = new System.Windows.Forms.TextBox();
            this.lblPatientDOB = new System.Windows.Forms.Label();
            this.txtPatientDOBInput = new System.Windows.Forms.TextBox();
            this.lblStudyDescription = new System.Windows.Forms.Label();
            this.txtStudyDescrptionInput = new System.Windows.Forms.TextBox();
            this.lblAccessionNumber = new System.Windows.Forms.Label();
            this.txtAccessionNumberInput = new System.Windows.Forms.TextBox();
            this.lblStudyDate = new System.Windows.Forms.Label();
            this.txtStudyDateInput = new System.Windows.Forms.TextBox();
            this.lblReferringPhysicianName = new System.Windows.Forms.Label();
            this.txtReferringPhysicianNameInput = new System.Windows.Forms.TextBox();
            this.lblSourceDirectory = new System.Windows.Forms.Label();
            this.txtSourceDirInput = new System.Windows.Forms.TextBox();
            this.btnChooseSourceDir = new System.Windows.Forms.Button();
            this.lblTargetDirectory = new System.Windows.Forms.Label();
            this.txtTargetDirInput = new System.Windows.Forms.TextBox();
            this.btnChooseTargetDir = new System.Windows.Forms.Button();
            this.dlgFolderChoose = new System.Windows.Forms.FolderBrowserDialog();
            this.btnAnonymize = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkbxRandomPatientId = new System.Windows.Forms.CheckBox();
            this.chkbxRandomAccessionNumber = new System.Windows.Forms.CheckBox();
            this.stsstripQuickAnonForm = new System.Windows.Forms.StatusStrip();
            this.toolstripStatuslbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.stsstripQuickAnonForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPatientId
            // 
            this.lblPatientId.AutoSize = true;
            this.lblPatientId.Location = new System.Drawing.Point(13, 13);
            this.lblPatientId.Name = "lblPatientId";
            this.lblPatientId.Size = new System.Drawing.Size(54, 13);
            this.lblPatientId.TabIndex = 0;
            this.lblPatientId.Text = "Patient ID";
            // 
            // txtPatientIdInput
            // 
            this.txtPatientIdInput.Location = new System.Drawing.Point(13, 30);
            this.txtPatientIdInput.Name = "txtPatientIdInput";
            this.txtPatientIdInput.Size = new System.Drawing.Size(200, 20);
            this.txtPatientIdInput.TabIndex = 1;
            // 
            // lblPatientName
            // 
            this.lblPatientName.AutoSize = true;
            this.lblPatientName.Location = new System.Drawing.Point(14, 53);
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Size = new System.Drawing.Size(199, 13);
            this.lblPatientName.TabIndex = 0;
            this.lblPatientName.Text = "Patient Name (LastName^FirstName^MI)";
            // 
            // txtPatientNameInput
            // 
            this.txtPatientNameInput.Location = new System.Drawing.Point(15, 69);
            this.txtPatientNameInput.Name = "txtPatientNameInput";
            this.txtPatientNameInput.Size = new System.Drawing.Size(200, 20);
            this.txtPatientNameInput.TabIndex = 1;
            // 
            // lblPatientDOB
            // 
            this.lblPatientDOB.AutoSize = true;
            this.lblPatientDOB.Location = new System.Drawing.Point(13, 92);
            this.lblPatientDOB.Name = "lblPatientDOB";
            this.lblPatientDOB.Size = new System.Drawing.Size(137, 13);
            this.lblPatientDOB.TabIndex = 0;
            this.lblPatientDOB.Text = "Patient DOB (YYYYMMDD)";
            // 
            // txtPatientDOBInput
            // 
            this.txtPatientDOBInput.Location = new System.Drawing.Point(13, 114);
            this.txtPatientDOBInput.Name = "txtPatientDOBInput";
            this.txtPatientDOBInput.Size = new System.Drawing.Size(100, 20);
            this.txtPatientDOBInput.TabIndex = 1;
            // 
            // lblStudyDescription
            // 
            this.lblStudyDescription.AutoSize = true;
            this.lblStudyDescription.Location = new System.Drawing.Point(14, 137);
            this.lblStudyDescription.Name = "lblStudyDescription";
            this.lblStudyDescription.Size = new System.Drawing.Size(90, 13);
            this.lblStudyDescription.TabIndex = 0;
            this.lblStudyDescription.Text = "Study Description";
            // 
            // txtStudyDescrptionInput
            // 
            this.txtStudyDescrptionInput.Location = new System.Drawing.Point(13, 154);
            this.txtStudyDescrptionInput.Name = "txtStudyDescrptionInput";
            this.txtStudyDescrptionInput.Size = new System.Drawing.Size(200, 20);
            this.txtStudyDescrptionInput.TabIndex = 1;
            // 
            // lblAccessionNumber
            // 
            this.lblAccessionNumber.AutoSize = true;
            this.lblAccessionNumber.Location = new System.Drawing.Point(13, 172);
            this.lblAccessionNumber.Name = "lblAccessionNumber";
            this.lblAccessionNumber.Size = new System.Drawing.Size(96, 13);
            this.lblAccessionNumber.TabIndex = 0;
            this.lblAccessionNumber.Text = "Accession Number";
            // 
            // txtAccessionNumberInput
            // 
            this.txtAccessionNumberInput.Location = new System.Drawing.Point(13, 189);
            this.txtAccessionNumberInput.Name = "txtAccessionNumberInput";
            this.txtAccessionNumberInput.Size = new System.Drawing.Size(150, 20);
            this.txtAccessionNumberInput.TabIndex = 1;
            // 
            // lblStudyDate
            // 
            this.lblStudyDate.AutoSize = true;
            this.lblStudyDate.Location = new System.Drawing.Point(12, 212);
            this.lblStudyDate.Name = "lblStudyDate";
            this.lblStudyDate.Size = new System.Drawing.Size(131, 13);
            this.lblStudyDate.TabIndex = 0;
            this.lblStudyDate.Text = "Study Date (YYYYMMDD)";
            // 
            // txtStudyDateInput
            // 
            this.txtStudyDateInput.Location = new System.Drawing.Point(12, 228);
            this.txtStudyDateInput.Name = "txtStudyDateInput";
            this.txtStudyDateInput.Size = new System.Drawing.Size(100, 20);
            this.txtStudyDateInput.TabIndex = 1;
            // 
            // lblReferringPhysicianName
            // 
            this.lblReferringPhysicianName.AutoSize = true;
            this.lblReferringPhysicianName.Location = new System.Drawing.Point(12, 251);
            this.lblReferringPhysicianName.Name = "lblReferringPhysicianName";
            this.lblReferringPhysicianName.Size = new System.Drawing.Size(257, 13);
            this.lblReferringPhysicianName.TabIndex = 0;
            this.lblReferringPhysicianName.Text = "Referring Physician Name (LastName^FirstName^MI)";
            // 
            // txtReferringPhysicianNameInput
            // 
            this.txtReferringPhysicianNameInput.Location = new System.Drawing.Point(12, 267);
            this.txtReferringPhysicianNameInput.Name = "txtReferringPhysicianNameInput";
            this.txtReferringPhysicianNameInput.Size = new System.Drawing.Size(200, 20);
            this.txtReferringPhysicianNameInput.TabIndex = 1;
            // 
            // lblSourceDirectory
            // 
            this.lblSourceDirectory.AutoSize = true;
            this.lblSourceDirectory.Location = new System.Drawing.Point(12, 318);
            this.lblSourceDirectory.Name = "lblSourceDirectory";
            this.lblSourceDirectory.Size = new System.Drawing.Size(133, 13);
            this.lblSourceDirectory.TabIndex = 0;
            this.lblSourceDirectory.Text = "Source Directory (required)";
            // 
            // txtSourceDirInput
            // 
            this.txtSourceDirInput.Location = new System.Drawing.Point(12, 334);
            this.txtSourceDirInput.Name = "txtSourceDirInput";
            this.txtSourceDirInput.Size = new System.Drawing.Size(200, 20);
            this.txtSourceDirInput.TabIndex = 1;
            // 
            // btnChooseSourceDir
            // 
            this.btnChooseSourceDir.Location = new System.Drawing.Point(242, 331);
            this.btnChooseSourceDir.Name = "btnChooseSourceDir";
            this.btnChooseSourceDir.Size = new System.Drawing.Size(75, 23);
            this.btnChooseSourceDir.TabIndex = 2;
            this.btnChooseSourceDir.Text = "Browse";
            this.btnChooseSourceDir.UseVisualStyleBackColor = true;
            this.btnChooseSourceDir.Click += new System.EventHandler(this.btnChooseSourceDir_Click);
            // 
            // lblTargetDirectory
            // 
            this.lblTargetDirectory.AutoSize = true;
            this.lblTargetDirectory.Location = new System.Drawing.Point(12, 362);
            this.lblTargetDirectory.Name = "lblTargetDirectory";
            this.lblTargetDirectory.Size = new System.Drawing.Size(129, 13);
            this.lblTargetDirectory.TabIndex = 0;
            this.lblTargetDirectory.Text = "Target Directory (optional)";
            // 
            // txtTargetDirInput
            // 
            this.txtTargetDirInput.Location = new System.Drawing.Point(12, 378);
            this.txtTargetDirInput.Name = "txtTargetDirInput";
            this.txtTargetDirInput.Size = new System.Drawing.Size(200, 20);
            this.txtTargetDirInput.TabIndex = 1;
            // 
            // btnChooseTargetDir
            // 
            this.btnChooseTargetDir.Location = new System.Drawing.Point(242, 375);
            this.btnChooseTargetDir.Name = "btnChooseTargetDir";
            this.btnChooseTargetDir.Size = new System.Drawing.Size(75, 23);
            this.btnChooseTargetDir.TabIndex = 2;
            this.btnChooseTargetDir.Text = "Browse";
            this.btnChooseTargetDir.UseVisualStyleBackColor = true;
            this.btnChooseTargetDir.Click += new System.EventHandler(this.btnChooseTargetDir_Click);
            // 
            // btnAnonymize
            // 
            this.btnAnonymize.Enabled = false;
            this.btnAnonymize.Location = new System.Drawing.Point(12, 416);
            this.btnAnonymize.Name = "btnAnonymize";
            this.btnAnonymize.Size = new System.Drawing.Size(75, 23);
            this.btnAnonymize.TabIndex = 3;
            this.btnAnonymize.Text = "Anonymize";
            this.btnAnonymize.UseVisualStyleBackColor = true;
            this.btnAnonymize.Click += new System.EventHandler(this.btnAnonymize_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(94, 416);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkbxRandomPatientId
            // 
            this.chkbxRandomPatientId.AutoSize = true;
            this.chkbxRandomPatientId.Location = new System.Drawing.Point(220, 32);
            this.chkbxRandomPatientId.Name = "chkbxRandomPatientId";
            this.chkbxRandomPatientId.Size = new System.Drawing.Size(113, 17);
            this.chkbxRandomPatientId.TabIndex = 5;
            this.chkbxRandomPatientId.Text = "Generate Random";
            this.chkbxRandomPatientId.UseVisualStyleBackColor = true;
            this.chkbxRandomPatientId.CheckedChanged += new System.EventHandler(this.chkbxRandomPatientId_CheckedChanged);
            // 
            // chkbxRandomAccessionNumber
            // 
            this.chkbxRandomAccessionNumber.AutoSize = true;
            this.chkbxRandomAccessionNumber.Location = new System.Drawing.Point(220, 192);
            this.chkbxRandomAccessionNumber.Name = "chkbxRandomAccessionNumber";
            this.chkbxRandomAccessionNumber.Size = new System.Drawing.Size(113, 17);
            this.chkbxRandomAccessionNumber.TabIndex = 5;
            this.chkbxRandomAccessionNumber.Text = "Generate Random";
            this.chkbxRandomAccessionNumber.UseVisualStyleBackColor = true;
            this.chkbxRandomAccessionNumber.CheckedChanged += new System.EventHandler(this.chkbxRandomAccessionNumber_CheckedChanged);
            // 
            // stsstripQuickAnonForm
            // 
            this.stsstripQuickAnonForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolstripStatuslbl});
            this.stsstripQuickAnonForm.Location = new System.Drawing.Point(0, 439);
            this.stsstripQuickAnonForm.Name = "stsstripQuickAnonForm";
            this.stsstripQuickAnonForm.Size = new System.Drawing.Size(368, 22);
            this.stsstripQuickAnonForm.TabIndex = 6;
            // 
            // toolstripStatuslbl
            // 
            this.toolstripStatuslbl.Name = "toolstripStatuslbl";
            this.toolstripStatuslbl.Size = new System.Drawing.Size(0, 17);
            // 
            // QuickAnonymizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 461);
            this.Controls.Add(this.stsstripQuickAnonForm);
            this.Controls.Add(this.chkbxRandomAccessionNumber);
            this.Controls.Add(this.chkbxRandomPatientId);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAnonymize);
            this.Controls.Add(this.btnChooseTargetDir);
            this.Controls.Add(this.btnChooseSourceDir);
            this.Controls.Add(this.txtStudyDateInput);
            this.Controls.Add(this.lblStudyDate);
            this.Controls.Add(this.txtAccessionNumberInput);
            this.Controls.Add(this.lblAccessionNumber);
            this.Controls.Add(this.txtStudyDescrptionInput);
            this.Controls.Add(this.lblStudyDescription);
            this.Controls.Add(this.txtPatientDOBInput);
            this.Controls.Add(this.lblPatientDOB);
            this.Controls.Add(this.txtTargetDirInput);
            this.Controls.Add(this.txtSourceDirInput);
            this.Controls.Add(this.lblTargetDirectory);
            this.Controls.Add(this.txtReferringPhysicianNameInput);
            this.Controls.Add(this.lblSourceDirectory);
            this.Controls.Add(this.txtPatientNameInput);
            this.Controls.Add(this.lblReferringPhysicianName);
            this.Controls.Add(this.lblPatientName);
            this.Controls.Add(this.txtPatientIdInput);
            this.Controls.Add(this.lblPatientId);
            this.Name = "QuickAnonymizationForm";
            this.Text = "Quick Anonymization";
            this.stsstripQuickAnonForm.ResumeLayout(false);
            this.stsstripQuickAnonForm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPatientId;
        private System.Windows.Forms.TextBox txtPatientIdInput;
        private System.Windows.Forms.Label lblPatientName;
        private System.Windows.Forms.TextBox txtPatientNameInput;
        private System.Windows.Forms.Label lblPatientDOB;
        private System.Windows.Forms.TextBox txtPatientDOBInput;
        private System.Windows.Forms.Label lblStudyDescription;
        private System.Windows.Forms.TextBox txtStudyDescrptionInput;
        private System.Windows.Forms.Label lblAccessionNumber;
        private System.Windows.Forms.TextBox txtAccessionNumberInput;
        private System.Windows.Forms.Label lblStudyDate;
        private System.Windows.Forms.TextBox txtStudyDateInput;
        private System.Windows.Forms.Label lblReferringPhysicianName;
        private System.Windows.Forms.TextBox txtReferringPhysicianNameInput;
        private System.Windows.Forms.Label lblSourceDirectory;
        private System.Windows.Forms.TextBox txtSourceDirInput;
        private System.Windows.Forms.Button btnChooseSourceDir;
        private System.Windows.Forms.Label lblTargetDirectory;
        private System.Windows.Forms.TextBox txtTargetDirInput;
        private System.Windows.Forms.Button btnChooseTargetDir;
        private System.Windows.Forms.FolderBrowserDialog dlgFolderChoose;
        private System.Windows.Forms.Button btnAnonymize;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkbxRandomPatientId;
        private System.Windows.Forms.CheckBox chkbxRandomAccessionNumber;
        private System.Windows.Forms.StatusStrip stsstripQuickAnonForm;
        private System.Windows.Forms.ToolStripStatusLabel toolstripStatuslbl;
    }
}