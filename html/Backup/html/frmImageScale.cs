using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace UtilETWeb
{
    public partial class frmImageScale : Form
    {
        public frmImageScale()
        {
            InitializeComponent();
        }

        private void frmImageScale_Load(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(@"C:\sprite_with_gabo_xantolo.png", "*.PNG");
            foreach (string name in files)
            {
                
                Image img = Image.FromFile(name);
                Graphics g = Graphics.FromImage(img);
                
                /*
                img.Width
                img.Height
                */



            }
        }
    }
}
