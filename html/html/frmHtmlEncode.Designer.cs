namespace UtilETWeb
{
    partial class frmHtmlEncode
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
            this.txtCode = new System.Windows.Forms.RichTextBox();
            this.txtResult = new System.Windows.Forms.RichTextBox();
            this.btnEncode = new System.Windows.Forms.Button();
            this.btnDecode = new System.Windows.Forms.Button();
            this.btnEncodeUrl = new System.Windows.Forms.Button();
            this.btnDecodeUrl = new System.Windows.Forms.Button();
            this.btnClean = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtCode
            // 
            this.txtCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCode.Location = new System.Drawing.Point(13, 13);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(419, 96);
            this.txtCode.TabIndex = 0;
            this.txtCode.Text = "";
            // 
            // txtResult
            // 
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.Location = new System.Drawing.Point(13, 176);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(419, 96);
            this.txtResult.TabIndex = 6;
            this.txtResult.Text = "";
            this.txtResult.Enter += new System.EventHandler(this.txtResult_Enter);
            // 
            // btnEncode
            // 
            this.btnEncode.Location = new System.Drawing.Point(13, 116);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(124, 23);
            this.btnEncode.TabIndex = 1;
            this.btnEncode.Text = "Encode";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.btnEncode_Click);
            // 
            // btnDecode
            // 
            this.btnDecode.Location = new System.Drawing.Point(13, 145);
            this.btnDecode.Name = "btnDecode";
            this.btnDecode.Size = new System.Drawing.Size(124, 23);
            this.btnDecode.TabIndex = 3;
            this.btnDecode.Text = "Decode";
            this.btnDecode.UseVisualStyleBackColor = true;
            this.btnDecode.Click += new System.EventHandler(this.btnDecode_Click);
            // 
            // btnEncodeUrl
            // 
            this.btnEncodeUrl.Location = new System.Drawing.Point(143, 116);
            this.btnEncodeUrl.Name = "btnEncodeUrl";
            this.btnEncodeUrl.Size = new System.Drawing.Size(124, 23);
            this.btnEncodeUrl.TabIndex = 2;
            this.btnEncodeUrl.Text = "Encode Url";
            this.btnEncodeUrl.UseVisualStyleBackColor = true;
            this.btnEncodeUrl.Click += new System.EventHandler(this.btnEncodeUrl_Click);
            // 
            // btnDecodeUrl
            // 
            this.btnDecodeUrl.Location = new System.Drawing.Point(143, 145);
            this.btnDecodeUrl.Name = "btnDecodeUrl";
            this.btnDecodeUrl.Size = new System.Drawing.Size(124, 23);
            this.btnDecodeUrl.TabIndex = 4;
            this.btnDecodeUrl.Text = "Decode Url";
            this.btnDecodeUrl.UseVisualStyleBackColor = true;
            this.btnDecodeUrl.Click += new System.EventHandler(this.btnDecodeUrl_Click);
            // 
            // btnClean
            // 
            this.btnClean.Location = new System.Drawing.Point(273, 116);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(75, 23);
            this.btnClean.TabIndex = 5;
            this.btnClean.Text = "Clean";
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.btnClean_Click);
            // 
            // frmHtmlEncode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 284);
            this.Controls.Add(this.btnClean);
            this.Controls.Add(this.btnDecodeUrl);
            this.Controls.Add(this.btnEncodeUrl);
            this.Controls.Add(this.btnDecode);
            this.Controls.Add(this.btnEncode);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.txtCode);
            this.Name = "frmHtmlEncode";
            this.Text = "frmHtmlEncode";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtCode;
        private System.Windows.Forms.RichTextBox txtResult;
        private System.Windows.Forms.Button btnEncode;
        private System.Windows.Forms.Button btnDecode;
        private System.Windows.Forms.Button btnEncodeUrl;
        private System.Windows.Forms.Button btnDecodeUrl;
        private System.Windows.Forms.Button btnClean;
    }
}