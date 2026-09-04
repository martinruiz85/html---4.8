namespace UtilETWeb
{
    partial class frmMigration
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.btnMigrateTables = new System.Windows.Forms.Button();
			this.chkCreateTable = new System.Windows.Forms.CheckBox();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.lblStatus = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbDatabaseTarget = new UtilETWeb.ComboBoxIcon();
			this.cmbDatabaseSorce = new UtilETWeb.ComboBoxIcon();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(0, 2);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.richTextBox1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.btnMigrateTables);
			this.splitContainer1.Panel2.Controls.Add(this.chkCreateTable);
			this.splitContainer1.Panel2.Controls.Add(this.progressBar1);
			this.splitContainer1.Panel2.Controls.Add(this.lblStatus);
			this.splitContainer1.Panel2.Controls.Add(this.label3);
			this.splitContainer1.Panel2.Controls.Add(this.label1);
			this.splitContainer1.Panel2.Controls.Add(this.cmbDatabaseTarget);
			this.splitContainer1.Panel2.Controls.Add(this.cmbDatabaseSorce);
			this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
			this.splitContainer1.Size = new System.Drawing.Size(785, 697);
			this.splitContainer1.SplitterDistance = 382;
			this.splitContainer1.TabIndex = 25;
			// 
			// richTextBox1
			// 
			this.richTextBox1.BackColor = System.Drawing.Color.White;
			this.richTextBox1.BackgroundImage = global::UtilETWeb.Properties.Resources.unnamed;
			this.richTextBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.richTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox1.ImeMode = System.Windows.Forms.ImeMode.On;
			this.richTextBox1.Location = new System.Drawing.Point(0, 0);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(382, 697);
			this.richTextBox1.TabIndex = 8;
			this.richTextBox1.Text = "";
			// 
			// btnMigrateTables
			// 
			this.btnMigrateTables.AccessibleRole = System.Windows.Forms.AccessibleRole.OutlineButton;
			this.btnMigrateTables.Image = global::UtilETWeb.Properties.Resources.scripts;
			this.btnMigrateTables.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnMigrateTables.Location = new System.Drawing.Point(3, 118);
			this.btnMigrateTables.Name = "btnMigrateTables";
			this.btnMigrateTables.Size = new System.Drawing.Size(162, 23);
			this.btnMigrateTables.TabIndex = 28;
			this.btnMigrateTables.Text = "Migrate Tables";
			this.btnMigrateTables.UseVisualStyleBackColor = true;
			this.btnMigrateTables.Click += new System.EventHandler(this.btnMigrateTables_Click);
			// 
			// chkCreateTable
			// 
			this.chkCreateTable.AutoSize = true;
			this.chkCreateTable.Location = new System.Drawing.Point(3, 96);
			this.chkCreateTable.Name = "chkCreateTable";
			this.chkCreateTable.Size = new System.Drawing.Size(132, 17);
			this.chkCreateTable.TabIndex = 30;
			this.chkCreateTable.Text = "Crear tabla si no existe";
			this.chkCreateTable.UseVisualStyleBackColor = true;
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(3, 147);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(370, 23);
			this.progressBar1.TabIndex = 25;
			// 
			// lblStatus
			// 
			this.lblStatus.Location = new System.Drawing.Point(3, 175);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(370, 17);
			this.lblStatus.TabIndex = 29;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 5);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(93, 13);
			this.label3.TabIndex = 17;
			this.label3.Text = "Database Source:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 52);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(90, 13);
			this.label1.TabIndex = 23;
			this.label1.Text = "Database Target:";
			// 
			// cmbDatabaseTarget
			// 
			this.cmbDatabaseTarget.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.cmbDatabaseTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDatabaseTarget.FilterRule = null;
			this.cmbDatabaseTarget.FormattingEnabled = true;
			this.cmbDatabaseTarget.Img = global::UtilETWeb.Properties.Resources.database_network;
			this.cmbDatabaseTarget.Location = new System.Drawing.Point(3, 68);
			this.cmbDatabaseTarget.Name = "cmbDatabaseTarget";
			this.cmbDatabaseTarget.PropertySelector = null;
			this.cmbDatabaseTarget.Size = new System.Drawing.Size(370, 21);
			this.cmbDatabaseTarget.SuggestBoxHeight = 96;
			this.cmbDatabaseTarget.SuggestListOrderRule = null;
			this.cmbDatabaseTarget.TabIndex = 1;
			this.cmbDatabaseTarget.Validating += new System.ComponentModel.CancelEventHandler(this.cmbDatabaseTarget_Validating);
			// 
			// cmbDatabaseSorce
			// 
			this.cmbDatabaseSorce.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.cmbDatabaseSorce.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDatabaseSorce.FilterRule = null;
			this.cmbDatabaseSorce.FormattingEnabled = true;
			this.cmbDatabaseSorce.Img = global::UtilETWeb.Properties.Resources.database_network;
			this.cmbDatabaseSorce.ItemHeight = 15;
			this.cmbDatabaseSorce.Location = new System.Drawing.Point(3, 21);
			this.cmbDatabaseSorce.Name = "cmbDatabaseSorce";
			this.cmbDatabaseSorce.PropertySelector = null;
			this.cmbDatabaseSorce.Size = new System.Drawing.Size(370, 21);
			this.cmbDatabaseSorce.SuggestBoxHeight = 96;
			this.cmbDatabaseSorce.SuggestListOrderRule = null;
			this.cmbDatabaseSorce.TabIndex = 0;
			this.cmbDatabaseSorce.Validating += new System.ComponentModel.CancelEventHandler(this.cmbDatabaseSorce_Validating);
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// frmMigration
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(783, 701);
			this.Controls.Add(this.splitContainer1);
			this.Name = "frmMigration";
			this.Text = "Migration de Tablas";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnMigrateTables;
        private System.Windows.Forms.CheckBox chkCreateTable;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private ComboBoxIcon cmbDatabaseTarget;
        private ComboBoxIcon cmbDatabaseSorce;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
