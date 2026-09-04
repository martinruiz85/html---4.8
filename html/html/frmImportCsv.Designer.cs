namespace UtilETWeb
{
    partial class frmImportCsv
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblFile = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.cmbDatabase = new System.Windows.Forms.ComboBox();
            this.lblTable = new System.Windows.Forms.Label();
            this.txtTableName = new System.Windows.Forms.TextBox();
            this.lblDelimiter = new System.Windows.Forms.Label();
            this.cmbDelimiter = new System.Windows.Forms.ComboBox();
            this.lblEncoding = new System.Windows.Forms.Label();
            this.cmbEncoding = new System.Windows.Forms.ComboBox();
            this.chkHasHeaders = new System.Windows.Forms.CheckBox();
            this.chkCreateTable = new System.Windows.Forms.CheckBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            //
            // lblFile
            //
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(12, 15);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(63, 13);
            this.lblFile.TabIndex = 0;
            this.lblFile.Text = "Archivo CSV:";
            //
            // txtFilePath
            //
            this.txtFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilePath.Location = new System.Drawing.Point(12, 31);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(390, 20);
            this.txtFilePath.TabIndex = 1;
            //
            // btnBrowse
            //
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(408, 29);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Examinar...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            //
            // lblDatabase
            //
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Location = new System.Drawing.Point(12, 62);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(85, 13);
            this.lblDatabase.TabIndex = 3;
            this.lblDatabase.Text = "Base de datos:";
            //
            // cmbDatabase
            //
            this.cmbDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDatabase.FormattingEnabled = true;
            this.cmbDatabase.Location = new System.Drawing.Point(12, 78);
            this.cmbDatabase.Name = "cmbDatabase";
            this.cmbDatabase.Size = new System.Drawing.Size(471, 21);
            this.cmbDatabase.TabIndex = 4;
            //
            // lblTable
            //
            this.lblTable.AutoSize = true;
            this.lblTable.Location = new System.Drawing.Point(12, 110);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(90, 13);
            this.lblTable.TabIndex = 5;
            this.lblTable.Text = "Tabla destino:";
            //
            // txtTableName
            //
            this.txtTableName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTableName.Location = new System.Drawing.Point(12, 126);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Size = new System.Drawing.Size(471, 20);
            this.txtTableName.TabIndex = 6;
            //
            // lblDelimiter
            //
            this.lblDelimiter.AutoSize = true;
            this.lblDelimiter.Location = new System.Drawing.Point(12, 158);
            this.lblDelimiter.Name = "lblDelimiter";
            this.lblDelimiter.Size = new System.Drawing.Size(59, 13);
            this.lblDelimiter.TabIndex = 7;
            this.lblDelimiter.Text = "Separador:";
            //
            // cmbDelimiter
            //
            this.cmbDelimiter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDelimiter.FormattingEnabled = true;
            this.cmbDelimiter.Items.AddRange(new object[] { "Coma  ( , )", "Punto y coma  ( ; )", "Tabulador  ( \\t )", "Pipe  ( | )" });
            this.cmbDelimiter.Location = new System.Drawing.Point(12, 174);
            this.cmbDelimiter.Name = "cmbDelimiter";
            this.cmbDelimiter.Size = new System.Drawing.Size(160, 21);
            this.cmbDelimiter.TabIndex = 8;
            this.cmbDelimiter.SelectedIndex = 0;
            //
            // lblEncoding
            //
            this.lblEncoding.AutoSize = true;
            this.lblEncoding.Location = new System.Drawing.Point(185, 158);
            this.lblEncoding.Name = "lblEncoding";
            this.lblEncoding.Size = new System.Drawing.Size(64, 13);
            this.lblEncoding.TabIndex = 9;
            this.lblEncoding.Text = "Codificación:";
            //
            // cmbEncoding
            //
            this.cmbEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEncoding.FormattingEnabled = true;
            this.cmbEncoding.Items.AddRange(new object[] { "UTF-8", "Windows-1252", "Latin-1" });
            this.cmbEncoding.Location = new System.Drawing.Point(185, 174);
            this.cmbEncoding.Name = "cmbEncoding";
            this.cmbEncoding.Size = new System.Drawing.Size(150, 21);
            this.cmbEncoding.TabIndex = 10;
            this.cmbEncoding.SelectedIndex = 0;
            //
            // chkHasHeaders
            //
            this.chkHasHeaders.AutoSize = true;
            this.chkHasHeaders.Checked = true;
            this.chkHasHeaders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHasHeaders.Location = new System.Drawing.Point(12, 207);
            this.chkHasHeaders.Name = "chkHasHeaders";
            this.chkHasHeaders.Size = new System.Drawing.Size(200, 17);
            this.chkHasHeaders.TabIndex = 11;
            this.chkHasHeaders.Text = "Primera fila contiene encabezados";
            this.chkHasHeaders.UseVisualStyleBackColor = true;
            //
            // chkCreateTable
            //
            this.chkCreateTable.AutoSize = true;
            this.chkCreateTable.Location = new System.Drawing.Point(12, 230);
            this.chkCreateTable.Name = "chkCreateTable";
            this.chkCreateTable.Size = new System.Drawing.Size(190, 17);
            this.chkCreateTable.TabIndex = 12;
            this.chkCreateTable.Text = "Crear tabla si no existe";
            this.chkCreateTable.UseVisualStyleBackColor = true;
            //
            // btnImport
            //
            this.btnImport.Image = global::UtilETWeb.Properties.Resources.scripts;
            this.btnImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImport.Location = new System.Drawing.Point(12, 257);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(120, 25);
            this.btnImport.TabIndex = 13;
            this.btnImport.Text = "Importar";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            //
            // progressBar1
            //
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 290);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(471, 23);
            this.progressBar1.TabIndex = 14;
            //
            // lblStatus
            //
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.AutoSize = false;
            this.lblStatus.Location = new System.Drawing.Point(12, 318);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(471, 17);
            this.lblStatus.TabIndex = 15;
            this.lblStatus.Text = "";
            //
            // errorProvider1
            //
            this.errorProvider1.ContainerControl = this;
            //
            // frmImportCsv
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 348);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.chkCreateTable);
            this.Controls.Add(this.chkHasHeaders);
            this.Controls.Add(this.cmbEncoding);
            this.Controls.Add(this.lblEncoding);
            this.Controls.Add(this.cmbDelimiter);
            this.Controls.Add(this.lblDelimiter);
            this.Controls.Add(this.txtTableName);
            this.Controls.Add(this.lblTable);
            this.Controls.Add(this.cmbDatabase);
            this.Controls.Add(this.lblDatabase);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.lblFile);
            this.Name = "frmImportCsv";
            this.Text = "Importar CSV a SQL";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.ComboBox cmbDatabase;
        private System.Windows.Forms.Label lblTable;
        private System.Windows.Forms.TextBox txtTableName;
        private System.Windows.Forms.Label lblDelimiter;
        private System.Windows.Forms.ComboBox cmbDelimiter;
        private System.Windows.Forms.Label lblEncoding;
        private System.Windows.Forms.ComboBox cmbEncoding;
        private System.Windows.Forms.CheckBox chkHasHeaders;
        private System.Windows.Forms.CheckBox chkCreateTable;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}
