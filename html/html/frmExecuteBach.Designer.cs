namespace UtilETWeb
{
    partial class frmExecuteBach
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
			this.components = new System.ComponentModel.Container();
			this.txtSearchPatern = new System.Windows.Forms.TextBox();
			this.cmbSearchOption = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnOpenFolder = new System.Windows.Forms.Button();
			this.txtPath = new System.Windows.Forms.TextBox();
			this.btnGenerate = new System.Windows.Forms.Button();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.cmbDatabase = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.lblPorcent = new System.Windows.Forms.Label();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
			this.chkDesc = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
			this.SuspendLayout();
			// 
			// txtSearchPatern
			// 
			this.txtSearchPatern.Location = new System.Drawing.Point(93, 59);
			this.txtSearchPatern.Name = "txtSearchPatern";
			this.txtSearchPatern.ReadOnly = true;
			this.txtSearchPatern.Size = new System.Drawing.Size(162, 20);
			this.txtSearchPatern.TabIndex = 15;
			// 
			// cmbSearchOption
			// 
			this.cmbSearchOption.FormattingEnabled = true;
			this.cmbSearchOption.Location = new System.Drawing.Point(92, 83);
			this.cmbSearchOption.Name = "cmbSearchOption";
			this.cmbSearchOption.Size = new System.Drawing.Size(163, 21);
			this.cmbSearchOption.TabIndex = 14;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 83);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(78, 13);
			this.label3.TabIndex = 13;
			this.label3.Text = "Search Option:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 59);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(81, 13);
			this.label2.TabIndex = 12;
			this.label2.Text = "Search Pattern:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 36);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(52, 13);
			this.label1.TabIndex = 11;
			this.label1.Text = "Directory:";
			// 
			// btnOpenFolder
			// 
			this.btnOpenFolder.Image = global::UtilETWeb.Properties.Resources.folder_horizontal_open;
			this.btnOpenFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnOpenFolder.Location = new System.Drawing.Point(261, 36);
			this.btnOpenFolder.Name = "btnOpenFolder";
			this.btnOpenFolder.Size = new System.Drawing.Size(87, 23);
			this.btnOpenFolder.TabIndex = 10;
			this.btnOpenFolder.Text = "...";
			this.btnOpenFolder.UseVisualStyleBackColor = true;
			this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
			// 
			// txtPath
			// 
			this.txtPath.Location = new System.Drawing.Point(92, 36);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(163, 20);
			this.txtPath.TabIndex = 9;
			this.txtPath.Validating += new System.ComponentModel.CancelEventHandler(this.txtPath_Validating);
			// 
			// btnGenerate
			// 
			this.btnGenerate.Image = global::UtilETWeb.Properties.Resources.work;
			this.btnGenerate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnGenerate.Location = new System.Drawing.Point(261, 83);
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(86, 21);
			this.btnGenerate.TabIndex = 16;
			this.btnGenerate.Text = "Generate";
			this.btnGenerate.UseVisualStyleBackColor = true;
			this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(11, 110);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(337, 23);
			this.progressBar1.Step = 1;
			this.progressBar1.TabIndex = 17;
			// 
			// cmbDatabase
			// 
			this.cmbDatabase.FormattingEnabled = true;
			this.cmbDatabase.Location = new System.Drawing.Point(92, 9);
			this.cmbDatabase.Name = "cmbDatabase";
			this.cmbDatabase.Size = new System.Drawing.Size(163, 21);
			this.cmbDatabase.TabIndex = 18;
			this.cmbDatabase.Validating += new System.ComponentModel.CancelEventHandler(this.cmbDatabase_Validating);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 12);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(60, 13);
			this.label4.TabIndex = 19;
			this.label4.Text = "Data Base:";
			// 
			// lblPorcent
			// 
			this.lblPorcent.AutoSize = true;
			this.lblPorcent.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPorcent.Location = new System.Drawing.Point(261, 8);
			this.lblPorcent.Name = "lblPorcent";
			this.lblPorcent.Size = new System.Drawing.Size(17, 25);
			this.lblPorcent.TabIndex = 20;
			this.lblPorcent.Text = " ";
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// chkDesc
			// 
			this.chkDesc.AutoSize = true;
			this.chkDesc.Location = new System.Drawing.Point(261, 61);
			this.chkDesc.Name = "chkDesc";
			this.chkDesc.Size = new System.Drawing.Size(73, 17);
			this.chkDesc.TabIndex = 21;
			this.chkDesc.Text = "Sort Desc";
			this.chkDesc.UseVisualStyleBackColor = true;
			this.chkDesc.CheckedChanged += new System.EventHandler(this.chkDesc_CheckedChanged);
			// 
			// frmExecuteBach
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(362, 136);
			this.Controls.Add(this.chkDesc);
			this.Controls.Add(this.lblPorcent);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cmbDatabase);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.btnGenerate);
			this.Controls.Add(this.txtSearchPatern);
			this.Controls.Add(this.cmbSearchOption);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnOpenFolder);
			this.Controls.Add(this.txtPath);
			this.Name = "frmExecuteBach";
			this.Text = "frmExecuteBach";
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSearchPatern;
        private System.Windows.Forms.ComboBox cmbSearchOption;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnGenerate;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ComboBox cmbDatabase;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblPorcent;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.CheckBox chkDesc;
    }
}