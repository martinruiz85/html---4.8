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
            this.btnEncrpytFile = new UtilETWeb.ButtonFont();
            this.btnDecryptFile = new UtilETWeb.ButtonFont();
            this.SuspendLayout();
            // 
            // txtDecryptFile
            // 
            this.txtDecryptFile.Enabled = false;
            this.txtDecryptFile.Location = new System.Drawing.Point(441, 12);
            this.txtDecryptFile.Name = "txtDecryptFile";
            this.txtDecryptFile.Size = new System.Drawing.Size(347, 20);
            this.txtDecryptFile.TabIndex = 0;
            // 
            // btnDecryptGenerate
            // 
            this.btnDecryptGenerate.Location = new System.Drawing.Point(440, 64);
            this.btnDecryptGenerate.Name = "btnDecryptGenerate";
            this.btnDecryptGenerate.Size = new System.Drawing.Size(100, 23);
            this.btnDecryptGenerate.TabIndex = 3;
            this.btnDecryptGenerate.Text = "Decrypt";
            this.btnDecryptGenerate.UseVisualStyleBackColor = true;
            this.btnDecryptGenerate.Click += new System.EventHandler(this.btnDecryptGenerate_Click);
            // 
            // prgDecrypt
            // 
            this.prgDecrypt.Location = new System.Drawing.Point(440, 93);
            this.prgDecrypt.Name = "prgDecrypt";
            this.prgDecrypt.Size = new System.Drawing.Size(100, 23);
            this.prgDecrypt.TabIndex = 4;
            // 
            // dlgDecrypt
            // 
            this.dlgDecrypt.FileName = "openFileDialog1";
            // 
            // txtDecryptFileResult
            // 
            this.txtDecryptFileResult.Location = new System.Drawing.Point(440, 38);
            this.txtDecryptFileResult.Name = "txtDecryptFileResult";
            this.txtDecryptFileResult.Size = new System.Drawing.Size(348, 20);
            this.txtDecryptFileResult.TabIndex = 2;
            this.txtDecryptFileResult.Enter += new System.EventHandler(this.txtDecryptFileResult_Enter);
            // 
            // txtEncryptFileResult
            // 
            this.txtEncryptFileResult.Location = new System.Drawing.Point(11, 41);
            this.txtEncryptFileResult.Name = "txtEncryptFileResult";
            this.txtEncryptFileResult.Size = new System.Drawing.Size(348, 20);
            this.txtEncryptFileResult.TabIndex = 7;
            // 
            // prgEncrypt
            // 
            this.prgEncrypt.Location = new System.Drawing.Point(11, 96);
            this.prgEncrypt.Name = "prgEncrypt";
            this.prgEncrypt.Size = new System.Drawing.Size(100, 23);
            this.prgEncrypt.TabIndex = 9;
            // 
            // btnEncryptGenerate
            // 
            this.btnEncryptGenerate.Location = new System.Drawing.Point(11, 67);
            this.btnEncryptGenerate.Name = "btnEncryptGenerate";
            this.btnEncryptGenerate.Size = new System.Drawing.Size(100, 23);
            this.btnEncryptGenerate.TabIndex = 8;
            this.btnEncryptGenerate.Text = "Encrypt";
            this.btnEncryptGenerate.UseVisualStyleBackColor = true;
            this.btnEncryptGenerate.Click += new System.EventHandler(this.btnEncryptGenerate_Click);
            // 
            // txtEncryptFile
            // 
            this.txtEncryptFile.Enabled = false;
            this.txtEncryptFile.Location = new System.Drawing.Point(12, 15);
            this.txtEncryptFile.Name = "txtEncryptFile";
            this.txtEncryptFile.Size = new System.Drawing.Size(347, 20);
            this.txtEncryptFile.TabIndex = 5;
            // 
            // dlgEncrypt
            // 
            this.dlgEncrypt.FileName = "openFileDialog1";
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
            this.btnDecryptFile.Location = new System.Drawing.Point(794, 12);
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
            this.ClientSize = new System.Drawing.Size(847, 130);
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
    }
}

