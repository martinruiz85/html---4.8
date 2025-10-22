namespace UtilETWeb
{
    partial class frmRadialTree
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
            this.radialTreePanelMySqlObjects1 = new html.RadialTreePanelMySqlObjects();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // radialTreePanelMySqlObjects1
            // 
            this.radialTreePanelMySqlObjects1.Location = new System.Drawing.Point(3, 2);
            this.radialTreePanelMySqlObjects1.Name = "radialTreePanelMySqlObjects1";
            this.radialTreePanelMySqlObjects1.SelectNode = null;
            this.radialTreePanelMySqlObjects1.Size = new System.Drawing.Size(529, 529);
            this.radialTreePanelMySqlObjects1.TabIndex = 0;
            this.radialTreePanelMySqlObjects1.TreeNode = null;
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(539, 13);
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(104, 45);
            this.trackBar1.TabIndex = 1;
            this.trackBar1.Value = 1;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // frmRadialTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 543);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.radialTreePanelMySqlObjects1);
            this.Name = "frmRadialTree";
            this.Text = "frmRadialTree";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public html.RadialTreePanelMySqlObjects radialTreePanelMySqlObjects1;
        private System.Windows.Forms.TrackBar trackBar1;
    }
}