using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Text;
using System.IO;

namespace UtilETWeb
{
    public partial class frmFont : Form
    {
        public frmFont()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.Paint += new PaintEventHandler(frmFont_Paint);
        }

        void frmFont_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            float h = 50;

            PrivateFontCollection collection = new PrivateFontCollection();

            //string result = System.Text.Encoding.UTF8.GetString(Properties.Resources.fontawesome_webfont);

            collection.AddFontFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\fontawesome-webfont.ttf"));
            

            FontFamily fontFamily = new FontFamily("FontAwesome", collection);
            Font font = new Font(fontFamily, h);

            //http://astronautweb.co/snippet/font-awesome/
            g.DrawString("\uf0d6", font, Brushes.Black, 0, 0 * h);
            g.DrawString("\uf15b", font, Brushes.Black, 0, 1.2f * h);

        }

        private void frmFont_Load(object sender, EventArgs e)
        {

        }
    }
}
