namespace UtilETWeb
{
	partial class frmGenerateTableCode
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.btnGenerar = new System.Windows.Forms.Button();
			this.txtAliasTableName = new System.Windows.Forms.TextBox();
			this.lblTableCodeName = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnGenerar
			// 
			this.btnGenerar.Location = new System.Drawing.Point(197, 39);
			this.btnGenerar.Name = "btnGenerar";
			this.btnGenerar.Size = new System.Drawing.Size(75, 23);
			this.btnGenerar.TabIndex = 0;
			this.btnGenerar.Text = "Generar";
			this.btnGenerar.UseVisualStyleBackColor = true;
			this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
			// 
			// txtAliasTableName
			// 
			this.txtAliasTableName.Location = new System.Drawing.Point(116, 13);
			this.txtAliasTableName.Name = "txtAliasTableName";
			this.txtAliasTableName.Size = new System.Drawing.Size(156, 20);
			this.txtAliasTableName.TabIndex = 1;
			this.txtAliasTableName.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// lblTableCodeName
			// 
			this.lblTableCodeName.AutoSize = true;
			this.lblTableCodeName.Location = new System.Drawing.Point(13, 19);
			this.lblTableCodeName.Name = "lblTableCodeName";
			this.lblTableCodeName.Size = new System.Drawing.Size(97, 13);
			this.lblTableCodeName.TabIndex = 2;
			this.lblTableCodeName.Text = "Alias Table Name:*";
			// 
			// frmGenerateTableCode
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.lblTableCodeName);
			this.Controls.Add(this.txtAliasTableName);
			this.Controls.Add(this.btnGenerar);
			this.Name = "frmGenerateTableCode";
			this.Text = "frmGenerateTableCode";
			this.Load += new System.EventHandler(this.frmGenerateTableCode_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnGenerar;
		private System.Windows.Forms.TextBox txtAliasTableName;
		private System.Windows.Forms.Label lblTableCodeName;
	}
}