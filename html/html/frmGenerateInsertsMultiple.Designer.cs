namespace UtilETWeb
{
    partial class frmGenerateInsertsMultiple
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
			this.btnGenerate = new System.Windows.Forms.Button();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.cmbDatabase = new System.Windows.Forms.ComboBox();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.txtOwner = new System.Windows.Forms.TextBox();
			this.richTextBox1 = new UtilETWeb.CustomRichTextBox();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnGenerate
			// 
			this.btnGenerate.Image = global::UtilETWeb.Properties.Resources.work;
			this.btnGenerate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnGenerate.Location = new System.Drawing.Point(333, 242);
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(75, 23);
			this.btnGenerate.TabIndex = 4;
			this.btnGenerate.Text = "Generate";
			this.btnGenerate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnGenerate.UseVisualStyleBackColor = true;
			this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
			// 
			// cmbDatabase
			// 
			this.cmbDatabase.FormattingEnabled = true;
			this.cmbDatabase.Location = new System.Drawing.Point(117, 242);
			this.cmbDatabase.Name = "cmbDatabase";
			this.cmbDatabase.Size = new System.Drawing.Size(204, 21);
			this.cmbDatabase.TabIndex = 3;
			this.cmbDatabase.Validating += new System.ComponentModel.CancelEventHandler(this.cmbDatabase_Validating);
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(13, 271);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(396, 23);
			this.progressBar1.TabIndex = 5;
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(13, 219);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(395, 17);
			this.checkBox1.TabIndex = 1;
			this.checkBox1.Text = "Apply Regex";
			this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// txtOwner
			// 
			this.txtOwner.Location = new System.Drawing.Point(13, 242);
			this.txtOwner.Name = "txtOwner";
			this.txtOwner.Size = new System.Drawing.Size(100, 20);
			this.txtOwner.TabIndex = 2;
			this.txtOwner.Text = "dbo";
			this.txtOwner.TextChanged += new System.EventHandler(this.txtOwner_TextChanged);
			// 
			// richTextBox1
			// 
			this.richTextBox1.Alpha = 0;
			this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.richTextBox1.Location = new System.Drawing.Point(13, 13);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Porcent = 0D;
			this.richTextBox1.SelectionEnd = 0;
			this.richTextBox1.Size = new System.Drawing.Size(395, 201);
			this.richTextBox1.TabIndex = 0;
			this.richTextBox1.Text = "";
			// 
			// frmGenerateInsertsMultiple
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(417, 303);
			this.Controls.Add(this.txtOwner);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.cmbDatabase);
			this.Controls.Add(this.btnGenerate);
			this.Controls.Add(this.richTextBox1);
			this.Name = "frmGenerateInsertsMultiple";
			this.Text = "frmGenerateInsertsMultiple";
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private CustomRichTextBox richTextBox1;
        private System.Windows.Forms.Button btnGenerate;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ComboBox cmbDatabase;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.TextBox txtOwner;
	}
}