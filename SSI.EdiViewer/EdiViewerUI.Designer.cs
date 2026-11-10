namespace SSI.EdiViewer
{
    partial class EdiViewerUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnSelectFile = new Button();
            dgvClaims = new DataGridView();
            lblTotalCharges = new Label();
            lblFileName = new Label();
            btnViewHistory = new Button();
            dgvImportedFiles = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvClaims).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvImportedFiles).BeginInit();
            SuspendLayout();
            // 
            // btnSelectFile
            // 
            btnSelectFile.Location = new Point(670, 139);
            btnSelectFile.Name = "btnSelectFile";
            btnSelectFile.Size = new Size(94, 29);
            btnSelectFile.TabIndex = 0;
            btnSelectFile.Text = "Upload File";
            btnSelectFile.UseVisualStyleBackColor = true;
            btnSelectFile.Click += btnSelectFile_Click;
            // 
            // dgvClaims
            // 
            dgvClaims.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvClaims.Location = new Point(-1, 3);
            dgvClaims.Name = "dgvClaims";
            dgvClaims.RowHeadersWidth = 51;
            dgvClaims.Size = new Size(1431, 130);
            dgvClaims.TabIndex = 1;
            // 
            // lblTotalCharges
            // 
            lblTotalCharges.AutoSize = true;
            lblTotalCharges.Location = new Point(477, 143);
            lblTotalCharges.Name = "lblTotalCharges";
            lblTotalCharges.Size = new Size(138, 20);
            lblTotalCharges.TabIndex = 2;
            lblTotalCharges.Text = "Total Claim Charge:";
            // 
            // lblFileName
            // 
            lblFileName.AutoSize = true;
            lblFileName.Location = new Point(770, 143);
            lblFileName.Name = "lblFileName";
            lblFileName.Size = new Size(120, 20);
            lblFileName.TabIndex = 3;
            lblFileName.Text = "No File Selected.";
            // 
            // btnViewHistory
            // 
            btnViewHistory.Location = new Point(155, 521);
            btnViewHistory.Name = "btnViewHistory";
            btnViewHistory.Size = new Size(141, 29);
            btnViewHistory.TabIndex = 4;
            btnViewHistory.Text = "View History";
            btnViewHistory.UseVisualStyleBackColor = true;
            // 
            // dgvImportedFiles
            // 
            dgvImportedFiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvImportedFiles.Location = new Point(-1, 143);
            dgvImportedFiles.Name = "dgvImportedFiles";
            dgvImportedFiles.RowHeadersWidth = 51;
            dgvImportedFiles.Size = new Size(456, 372);
            dgvImportedFiles.TabIndex = 5;
            // 
            // EdiViewerUI
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1430, 603);
            Controls.Add(dgvImportedFiles);
            Controls.Add(btnViewHistory);
            Controls.Add(lblFileName);
            Controls.Add(lblTotalCharges);
            Controls.Add(dgvClaims);
            Controls.Add(btnSelectFile);
            Name = "EdiViewerUI";
            Text = "EDI 837 Viewer";
            ((System.ComponentModel.ISupportInitialize)dgvClaims).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvImportedFiles).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSelectFile;
        private DataGridView dgvClaims;
        private Label lblTotalCharges;
        private Label lblFileName;
        private Button btnViewHistory;
        private DataGridView dgvImportedFiles;
    }
}
