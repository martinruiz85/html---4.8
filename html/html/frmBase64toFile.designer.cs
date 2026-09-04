namespace Base64ToFile
{
    partial class frmBase64toFile
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
			this.txtDecryptFile = new System.Windows.Forms.TextBox();
			this.btnDecryptGenerate = new System.Windows.Forms.Button();
			this.bgwDecrypt = new System.ComponentModel.BackgroundWorker();
			this.prgDecrypt = new System.Windows.Forms.ProgressBar();
			this.dlgDecrypt = new System.Windows.Forms.OpenFileDialog();
			this.txtDecryptFileResult = new System.Windows.Forms.TextBox();
			this.txtEncryptFileResult = new System.Windows.Forms.TextBox();
			this.prgEncrypt = new System.Windows.Forms.ProgressBar();
			this.btnEncryptGenerate = new System.Windows.Forms.Button();
			this.txtEncryptFile = new System.Windows.Forms.TextBox();
			this.dlgEncrypt = new System.Windows.Forms.OpenFileDialog();
			this.bgwEncrypt = new System.ComponentModel.BackgroundWorker();
			this.lblPathEncryptFile = new System.Windows.Forms.Label();
			this.lblResultEncriptFile = new System.Windows.Forms.Label();
			this.lblDecryptFile = new System.Windows.Forms.Label();
			this.lblResultDencriptFile = new System.Windows.Forms.Label();
			this.btnEncrpytFile = new UtilETWeb.ButtonFont();
			this.btnDecryptFile = new UtilETWeb.ButtonFont();
			this.SuspendLayout();
			// 
			// txtDecryptFile
			// 
			this.txtDecryptFile.Enabled = false;
			this.txtDecryptFile.Location = new System.Drawing.Point(580, 12);
			this.txtDecryptFile.Name = "txtDecryptFile";
			this.txtDecryptFile.Size = new System.Drawing.Size(254, 20);
			this.txtDecryptFile.TabIndex = 0;
			// 
			// btnDecryptGenerate
			// 
			this.btnDecryptGenerate.Location = new System.Drawing.Point(440, 64);
			this.btnDecryptGenerate.Name = "btnDecryptGenerate";
			this.btnDecryptGenerate.Size = new System.Drawing.Size(147, 23);
			this.btnDecryptGenerate.TabIndex = 3;
			this.btnDecryptGenerate.Text = "Decrypt (Txt to Any file)";
			this.btnDecryptGenerate.UseVisualStyleBackColor = true;
			this.btnDecryptGenerate.Click += new System.EventHandler(this.btnDecryptGenerate_Click);
			// 
			// prgDecrypt
			// 
			this.prgDecrypt.Location = new System.Drawing.Point(440, 93);
			this.prgDecrypt.Name = "prgDecrypt";
			this.prgDecrypt.Size = new System.Drawing.Size(147, 23);
			this.prgDecrypt.TabIndex = 4;
			// 
			// dlgDecrypt
			// 
			this.dlgDecrypt.FileName = "openFileDialog1";
			// 
			// txtDecryptFileResult
			// 
			this.txtDecryptFileResult.Location = new System.Drawing.Point(580, 38);
			this.txtDecryptFileResult.Name = "txtDecryptFileResult";
			this.txtDecryptFileResult.Size = new System.Drawing.Size(254, 20);
			this.txtDecryptFileResult.TabIndex = 2;
			this.txtDecryptFileResult.Enter += new System.EventHandler(this.txtDecryptFileResult_Enter);
			// 
			// txtEncryptFileResult
			// 
			this.txtEncryptFileResult.Location = new System.Drawing.Point(129, 41);
			this.txtEncryptFileResult.Name = "txtEncryptFileResult";
			this.txtEncryptFileResult.Size = new System.Drawing.Size(230, 20);
			this.txtEncryptFileResult.TabIndex = 7;
			// 
			// prgEncrypt
			// 
			this.prgEncrypt.Location = new System.Drawing.Point(11, 96);
			this.prgEncrypt.Name = "prgEncrypt";
			this.prgEncrypt.Size = new System.Drawing.Size(172, 23);
			this.prgEncrypt.TabIndex = 9;
			// 
			// btnEncryptGenerate
			// 
			this.btnEncryptGenerate.Location = new System.Drawing.Point(11, 67);
			this.btnEncryptGenerate.Name = "btnEncryptGenerate";
			this.btnEncryptGenerate.Size = new System.Drawing.Size(172, 23);
			this.btnEncryptGenerate.TabIndex = 8;
			this.btnEncryptGenerate.Text = "Encrypt (Any file to Txt)";
			this.btnEncryptGenerate.UseVisualStyleBackColor = true;
			this.btnEncryptGenerate.Click += new System.EventHandler(this.btnEncryptGenerate_Click);
			// 
			// txtEncryptFile
			// 
			this.txtEncryptFile.Enabled = false;
			this.txtEncryptFile.Location = new System.Drawing.Point(129, 15);
			this.txtEncryptFile.Name = "txtEncryptFile";
			this.txtEncryptFile.Size = new System.Drawing.Size(230, 20);
			this.txtEncryptFile.TabIndex = 5;
			// 
			// dlgEncrypt
			// 
			this.dlgEncrypt.FileName = "openFileDialog1";
			// 
			// lblPathEncryptFile
			// 
			this.lblPathEncryptFile.AutoSize = true;
			this.lblPathEncryptFile.Location = new System.Drawing.Point(8, 22);
			this.lblPathEncryptFile.Name = "lblPathEncryptFile";
			this.lblPathEncryptFile.Size = new System.Drawing.Size(115, 13);
			this.lblPathEncryptFile.TabIndex = 10;
			this.lblPathEncryptFile.Text = "Path any file to encrypt";
			// 
			// lblResultEncriptFile
			// 
			this.lblResultEncriptFile.AutoSize = true;
			this.lblResultEncriptFile.Location = new System.Drawing.Point(8, 48);
			this.lblResultEncriptFile.Name = "lblResultEncriptFile";
			this.lblResultEncriptFile.Size = new System.Drawing.Size(115, 13);
			this.lblResultEncriptFile.TabIndex = 11;
			this.lblResultEncriptFile.Text = "Name encrypt txt result";
			// 
			// lblDecryptFile
			// 
			this.lblDecryptFile.AutoSize = true;
			this.lblDecryptFile.Location = new System.Drawing.Point(437, 19);
			this.lblDecryptFile.Name = "lblDecryptFile";
			this.lblDecryptFile.Size = new System.Drawing.Size(113, 13);
			this.lblDecryptFile.TabIndex = 12;
			this.lblDecryptFile.Text = "Path Txt file to decrypt";
			// 
			// lblResultDencriptFile
			// 
			this.lblResultDencriptFile.AutoSize = true;
			this.lblResultDencriptFile.Location = new System.Drawing.Point(437, 45);
			this.lblResultDencriptFile.Name = "lblResultDencriptFile";
			this.lblResultDencriptFile.Size = new System.Drawing.Size(137, 13);
			this.lblResultDencriptFile.TabIndex = 13;
			this.lblResultDencriptFile.Text = "Name decrypt any file result";
			// 
			// btnEncrpytFile
			// 
			this.btnEncrpytFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnEncrpytFile.Icon = "";
			this.btnEncrpytFile.Location = new System.Drawing.Point(365, 15);
			this.btnEncrpytFile.Name = "btnEncrpytFile";
			this.btnEncrpytFile.Size = new System.Drawing.Size(46, 46);
			this.btnEncrpytFile.TabIndex = 6;
			this.btnEncrpytFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnEncrpytFile.UseVisualStyleBackColor = true;
			this.btnEncrpytFile.Click += new System.EventHandler(this.btnEncrpytFile_Click);
			// 
			// btnDecryptFile
			// 
			this.btnDecryptFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDecryptFile.Icon = "";
			this.btnDecryptFile.Location = new System.Drawing.Point(840, 12);
			this.btnDecryptFile.Name = "btnDecryptFile";
			this.btnDecryptFile.Size = new System.Drawing.Size(46, 46);
			this.btnDecryptFile.TabIndex = 1;
			this.btnDecryptFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnDecryptFile.UseVisualStyleBackColor = true;
			this.btnDecryptFile.Click += new System.EventHandler(this.btnDecryptFile_Click);
			// 
			// frmBase64toFile
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(915, 128);
			this.Controls.Add(this.lblResultDencriptFile);
			this.Controls.Add(this.lblDecryptFile);
			this.Controls.Add(this.lblResultEncriptFile);
			this.Controls.Add(this.lblPathEncryptFile);
			this.Controls.Add(this.txtEncryptFileResult);
			this.Controls.Add(this.btnEncrpytFile);
			this.Controls.Add(this.prgEncrypt);
			this.Controls.Add(this.btnEncryptGenerate);
			this.Controls.Add(this.txtEncryptFile);
			this.Controls.Add(this.txtDecryptFileResult);
			this.Controls.Add(this.btnDecryptFile);
			this.Controls.Add(this.prgDecrypt);
			this.Controls.Add(this.btnDecryptGenerate);
			this.Controls.Add(this.txtDecryptFile);
			this.Name = "frmBase64toFile";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDecryptFile;
        private System.Windows.Forms.Button btnDecryptGenerate;
        private System.ComponentModel.BackgroundWorker bgwDecrypt;
        private System.Windows.Forms.ProgressBar prgDecrypt;
        private UtilETWeb.ButtonFont btnDecryptFile;
        private System.Windows.Forms.OpenFileDialog dlgDecrypt;
        private System.Windows.Forms.TextBox txtDecryptFileResult;
        private System.Windows.Forms.TextBox txtEncryptFileResult;
        private UtilETWeb.ButtonFont btnEncrpytFile;
        private System.Windows.Forms.ProgressBar prgEncrypt;
        private System.Windows.Forms.Button btnEncryptGenerate;
        private System.Windows.Forms.TextBox txtEncryptFile;
        private System.Windows.Forms.OpenFileDialog dlgEncrypt;
        private System.ComponentModel.BackgroundWorker bgwEncrypt;
        private System.Windows.Forms.Label lblPathEncryptFile;
        private System.Windows.Forms.Label lblResultEncriptFile;
        private System.Windows.Forms.Label lblDecryptFile;
        private System.Windows.Forms.Label lblResultDencriptFile;
    }
}

