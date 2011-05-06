namespace DicomBulkAnonymizer
{
    partial class MainForm
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
            this.btnFieldEditor = new System.Windows.Forms.Button();
            this.btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnFieldEditor
            // 
            this.btnFieldEditor.Location = new System.Drawing.Point(13, 227);
            this.btnFieldEditor.Name = "btnFieldEditor";
            this.btnFieldEditor.Size = new System.Drawing.Size(75, 23);
            this.btnFieldEditor.TabIndex = 0;
            this.btnFieldEditor.Text = "Field Editor";
            this.btnFieldEditor.UseVisualStyleBackColor = true;
            this.btnFieldEditor.Click += new System.EventHandler(this.btnFieldEditor_Click);
            // 
            // btn
            // 
            this.btn.Location = new System.Drawing.Point(95, 227);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(75, 23);
            this.btn.TabIndex = 1;
            this.btn.Text = "Quick";
            this.btn.UseVisualStyleBackColor = true;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btn);
            this.Controls.Add(this.btnFieldEditor);
            this.Name = "MainForm";
            this.Text = "Monodicom Bulk Anonymizer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnFieldEditor;
        private System.Windows.Forms.Button btn;
    }
}

