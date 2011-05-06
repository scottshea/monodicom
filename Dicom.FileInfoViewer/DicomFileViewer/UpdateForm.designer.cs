namespace Monodicom.DicomInfoViewer
{
    partial class UpdateForm
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
            this.lblDicomTag = new System.Windows.Forms.Label();
            this.lblDicomTagName = new System.Windows.Forms.Label();
            this.lblDicomValueOriginal = new System.Windows.Forms.Label();
            this.txtboxNewValue = new System.Windows.Forms.TextBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblStatusMsg = new System.Windows.Forms.Label();
            this.lblDicomNameLabel = new System.Windows.Forms.Label();
            this.lblDicomElementTagLabel = new System.Windows.Forms.Label();
            this.lblDicomElementValueLabel = new System.Windows.Forms.Label();
            this.chkboxApplyToAllFiles = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblDicomTag
            // 
            this.lblDicomTag.AutoSize = true;
            this.lblDicomTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDicomTag.Location = new System.Drawing.Point(141, 9);
            this.lblDicomTag.Name = "lblDicomTag";
            this.lblDicomTag.Size = new System.Drawing.Size(0, 20);
            this.lblDicomTag.TabIndex = 0;
            // 
            // lblDicomTagName
            // 
            this.lblDicomTagName.AutoSize = true;
            this.lblDicomTagName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDicomTagName.Location = new System.Drawing.Point(141, 36);
            this.lblDicomTagName.Name = "lblDicomTagName";
            this.lblDicomTagName.Size = new System.Drawing.Size(0, 20);
            this.lblDicomTagName.TabIndex = 1;
            // 
            // lblDicomValueOriginal
            // 
            this.lblDicomValueOriginal.AutoSize = true;
            this.lblDicomValueOriginal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDicomValueOriginal.Location = new System.Drawing.Point(141, 68);
            this.lblDicomValueOriginal.Name = "lblDicomValueOriginal";
            this.lblDicomValueOriginal.Size = new System.Drawing.Size(0, 20);
            this.lblDicomValueOriginal.TabIndex = 2;
            // 
            // txtboxNewValue
            // 
            this.txtboxNewValue.Location = new System.Drawing.Point(12, 121);
            this.txtboxNewValue.MaxLength = 256;
            this.txtboxNewValue.Name = "txtboxNewValue";
            this.txtboxNewValue.Size = new System.Drawing.Size(157, 20);
            this.txtboxNewValue.TabIndex = 3;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(13, 147);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(94, 147);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblStatusMsg
            // 
            this.lblStatusMsg.AutoSize = true;
            this.lblStatusMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusMsg.Location = new System.Drawing.Point(16, 237);
            this.lblStatusMsg.Name = "lblStatusMsg";
            this.lblStatusMsg.Size = new System.Drawing.Size(0, 17);
            this.lblStatusMsg.TabIndex = 6;
            // 
            // lblDicomNameLabel
            // 
            this.lblDicomNameLabel.AutoSize = true;
            this.lblDicomNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDicomNameLabel.Location = new System.Drawing.Point(12, 36);
            this.lblDicomNameLabel.Name = "lblDicomNameLabel";
            this.lblDicomNameLabel.Size = new System.Drawing.Size(118, 20);
            this.lblDicomNameLabel.TabIndex = 7;
            this.lblDicomNameLabel.Text = "Element Name:";
            // 
            // lblDicomElementTagLabel
            // 
            this.lblDicomElementTagLabel.AutoSize = true;
            this.lblDicomElementTagLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDicomElementTagLabel.Location = new System.Drawing.Point(26, 9);
            this.lblDicomElementTagLabel.Name = "lblDicomElementTagLabel";
            this.lblDicomElementTagLabel.Size = new System.Drawing.Size(103, 20);
            this.lblDicomElementTagLabel.TabIndex = 8;
            this.lblDicomElementTagLabel.Text = "Element Tag:";
            // 
            // lblDicomElementValueLabel
            // 
            this.lblDicomElementValueLabel.AutoSize = true;
            this.lblDicomElementValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDicomElementValueLabel.Location = new System.Drawing.Point(12, 68);
            this.lblDicomElementValueLabel.Name = "lblDicomElementValueLabel";
            this.lblDicomElementValueLabel.Size = new System.Drawing.Size(117, 20);
            this.lblDicomElementValueLabel.TabIndex = 9;
            this.lblDicomElementValueLabel.Text = "Element Value:";
            // 
            // chkboxApplyToAllFiles
            // 
            this.chkboxApplyToAllFiles.AutoSize = true;
            this.chkboxApplyToAllFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkboxApplyToAllFiles.Location = new System.Drawing.Point(189, 120);
            this.chkboxApplyToAllFiles.Name = "chkboxApplyToAllFiles";
            this.chkboxApplyToAllFiles.Size = new System.Drawing.Size(186, 21);
            this.chkboxApplyToAllFiles.TabIndex = 10;
            this.chkboxApplyToAllFiles.Text = "Apply to all files in study?";
            this.chkboxApplyToAllFiles.UseVisualStyleBackColor = true;
            // 
            // UpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 262);
            this.Controls.Add(this.chkboxApplyToAllFiles);
            this.Controls.Add(this.lblDicomElementValueLabel);
            this.Controls.Add(this.lblDicomElementTagLabel);
            this.Controls.Add(this.lblDicomNameLabel);
            this.Controls.Add(this.lblStatusMsg);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.txtboxNewValue);
            this.Controls.Add(this.lblDicomValueOriginal);
            this.Controls.Add(this.lblDicomTagName);
            this.Controls.Add(this.lblDicomTag);
            this.Name = "UpdateForm";
            this.Text = "Update Dicom Data";
            this.Load += new System.EventHandler(this.UpdateForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDicomTag;
        private System.Windows.Forms.Label lblDicomTagName;
        private System.Windows.Forms.Label lblDicomValueOriginal;
        private System.Windows.Forms.TextBox txtboxNewValue;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblStatusMsg;
        private System.Windows.Forms.Label lblDicomNameLabel;
        private System.Windows.Forms.Label lblDicomElementTagLabel;
        private System.Windows.Forms.Label lblDicomElementValueLabel;
        private System.Windows.Forms.CheckBox chkboxApplyToAllFiles;
    }
}