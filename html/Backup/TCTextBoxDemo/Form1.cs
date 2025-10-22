using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Trestan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            TCResize resizableTextBox = new TCResize(this.textBox1);
            this.textBox1.Text = "This is a test text box, subclassed from the original textbox.\r\n\r\nThere is no easy way to set a background image without user-drawn text.";
            TCResize resizableTCBox = new TCResize(this.tcTextBox1);
            this.tcTextBox1.Text = "This is TCTextBox,with transparent background and a sample background image. Try to play with it.";

        }
    }
}
