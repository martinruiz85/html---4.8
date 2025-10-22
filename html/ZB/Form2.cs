using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZB
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form2_Load);
        }

        void Form2_Load(object sender, EventArgs e)
        {
            this.webBrowser1.Navigate(@"C:\Users\Consultorin\Desktop\x.html");
        }
    }
}
