namespace DicomBulkAnonymizer
{
    partial class FieldEditorForm
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
            this.lbDicomFields = new System.Windows.Forms.ListBox();
            this.lbSelectedDicomFields = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnAddAll = new System.Windows.Forms.Button();
            this.bntRemove = new System.Windows.Forms.Button();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbDicomFields
            // 
            this.lbDicomFields.FormattingEnabled = true;
            this.lbDicomFields.Location = new System.Drawing.Point(13, 13);
            this.lbDicomFields.Name = "lbDicomFields";
            this.lbDicomFields.ScrollAlwaysVisible = true;
            this.lbDicomFields.Size = new System.Drawing.Size(423, 199);
            this.lbDicomFields.TabIndex = 0;
            // 
            // lbSelectedDicomFields
            // 
            this.lbSelectedDicomFields.FormattingEnabled = true;
            this.lbSelectedDicomFields.Location = new System.Drawing.Point(487, 12);
            this.lbSelectedDicomFields.Name = "lbSelectedDicomFields";
            this.lbSelectedDicomFields.ScrollAlwaysVisible = true;
            this.lbSelectedDicomFields.Size = new System.Drawing.Size(423, 199);
            this.lbSelectedDicomFields.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(442, 42);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(39, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = ">";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnAddAll
            // 
            this.btnAddAll.Location = new System.Drawing.Point(442, 71);
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Size = new System.Drawing.Size(39, 23);
            this.btnAddAll.TabIndex = 1;
            this.btnAddAll.Text = ">>";
            this.btnAddAll.UseVisualStyleBackColor = true;
            this.btnAddAll.Click += new System.EventHandler(this.btnAddAll_Click);
            // 
            // bntRemove
            // 
            this.bntRemove.Location = new System.Drawing.Point(442, 131);
            this.bntRemove.Name = "bntRemove";
            this.bntRemove.Size = new System.Drawing.Size(39, 23);
            this.bntRemove.TabIndex = 1;
            this.bntRemove.Text = "<";
            this.bntRemove.UseVisualStyleBackColor = true;
            this.bntRemove.Click += new System.EventHandler(this.bntRemove_Click);
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Location = new System.Drawing.Point(442, 160);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(39, 23);
            this.btnRemoveAll.TabIndex = 1;
            this.btnRemoveAll.Text = "<<";
            this.btnRemoveAll.UseVisualStyleBackColor = true;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // FieldEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 262);
            this.Controls.Add(this.btnRemoveAll);
            this.Controls.Add(this.bntRemove);
            this.Controls.Add(this.btnAddAll);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lbSelectedDicomFields);
            this.Controls.Add(this.lbDicomFields);
            this.Name = "FieldEditorForm";
            this.Text = "Field Editor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbDicomFields;
        private System.Windows.Forms.ListBox lbSelectedDicomFields;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnAddAll;
        private System.Windows.Forms.Button bntRemove;
        private System.Windows.Forms.Button btnRemoveAll;
    }
}