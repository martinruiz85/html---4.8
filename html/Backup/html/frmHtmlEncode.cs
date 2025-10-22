using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;
using Microsoft.Office.Interop.Excel;
using System.Web.UI.WebControls;

namespace UtilETWeb
{
    public partial class frmHtmlEncode : Form
    {
        public frmHtmlEncode()
        {
            InitializeComponent();
        }

        private void btnEncode_Click(object sender, EventArgs e)
        {
            this.txtResult.Text = HttpUtility.HtmlEncode(this.txtCode.Text);
            this.txtResult.SelectAll();
            this.txtResult.Focus(); //you need to call this to show selection if it doesn't has focus
            this.txtResult.LinkClicked += new LinkClickedEventHandler(txtResult_LinkClicked);
        }

        private void btnDecode_Click(object sender, EventArgs e)
        {
            this.txtResult.Text = HttpUtility.HtmlDecode(this.txtCode.Text);
            this.txtResult.SelectAll();
            this.txtResult.Focus(); //you need to call this to show selection if it doesn't has focus
            this.txtResult.LinkClicked += new LinkClickedEventHandler(txtResult_LinkClicked);
        }

        private void btnEncodeUrl_Click(object sender, EventArgs e)
        {
            this.txtResult.Text = HttpUtility.UrlEncode(this.txtCode.Text);
            this.txtResult.SelectAll();
            this.txtResult.Focus(); //you need to call this to show selection if it doesn't has focus
            this.txtResult.LinkClicked += new LinkClickedEventHandler(txtResult_LinkClicked);
        }

        private void btnDecodeUrl_Click(object sender, EventArgs e)
        {
            this.txtResult.Text = HttpUtility.UrlDecode(this.txtCode.Text);
            this.txtResult.SelectAll();
            this.txtResult.Focus(); //you need to call this to show selection if it doesn't has focus
            this.txtResult.LinkClicked += new LinkClickedEventHandler(txtResult_LinkClicked);            
        }

        void txtResult_LinkClicked(object sender, LinkClickedEventArgs e)
        { 
           
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            this.txtCode.Text = "";
            this.txtResult.Text = "";
            this.txtCode.Focus();
        }

        private void txtResult_Enter(object sender, EventArgs e)
        {
            txtResult.SelectAll();
        }


    }
}
