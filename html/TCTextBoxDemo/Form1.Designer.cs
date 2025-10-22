namespace Trestan
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tcTextBox1 = new Trestan.TCTextBox();
            this.textBox1 = new Trestan.TestTextBox();
            this.SuspendLayout();
            // 
            // tcTextBox1
            // 
            this.tcTextBox1.BackColor = System.Drawing.Color.Transparent;
            this.tcTextBox1.BackgroundImage = global::Trestan.Properties.Resources.butterfly;
            this.tcTextBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.tcTextBox1.CharIndex = 0;
            this.tcTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tcTextBox1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.tcTextBox1.ImeMode = System.Windows.Forms.ImeMode.On;
            this.tcTextBox1.Location = new System.Drawing.Point(22, 37);
            this.tcTextBox1.Name = "tcTextBox1";
            this.tcTextBox1.Selecting = false;
            this.tcTextBox1.SelectionLength = 0;
            this.tcTextBox1.SelectionStart = -1;
            this.tcTextBox1.SelectText = "";
            this.tcTextBox1.Size = new System.Drawing.Size(199, 198);
            this.tcTextBox1.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(290, 37);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(176, 198);
            this.textBox1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(574, 330);
            this.Controls.Add(this.tcTextBox1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "TCTextBoxDemo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TestTextBox textBox1;
        private Trestan.TCTextBox tcTextBox1;
    }
}

