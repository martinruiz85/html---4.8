namespace UtilETWeb
{
    partial class frmTimeLine
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
            this.timeLine1 = new UtilETWeb.TimeLine();
            this.SuspendLayout();
            // 
            // timeLine1
            // 
            this.timeLine1.AutoScroll = true;
            this.timeLine1.AutoScrollMinSize = new System.Drawing.Size(133, 1281);
            this.timeLine1.BackColor = System.Drawing.Color.White;
            this.timeLine1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timeLine1.Location = new System.Drawing.Point(0, 0);
            this.timeLine1.Name = "timeLine1";
            this.timeLine1.Size = new System.Drawing.Size(284, 262);
            this.timeLine1.TabIndex = 0;
            // 
            // frmTimeLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.timeLine1);
            this.Name = "frmTimeLine";
            this.Text = "frmTimeLine";
            this.ResumeLayout(false);

        }

        #endregion

        private TimeLine timeLine1;
    }
}