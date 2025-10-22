using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace MM.Forms
{
    public partial class frmGif : Form
    {
        public frmGif()
        {
            InitializeComponent();
        }

        private void frmGif_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            this.Paint += new PaintEventHandler(frmGif_Paint);
        }

        //https://www.freepng.es/
        void frmGif_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            FontFamily fontFamily = new FontFamily("Arial");
            Font f = new Font(fontFamily, 9, GraphicsUnit.Point);

            ImageAttributes imageAttributes = new ImageAttributes();
            int width = Properties.Resources.bacteria2.Width;
            int height = Properties.Resources.bacteria2.Height;

            float[][] colorMatrixElements = { 
           new float[] {2,  0,  0,  0, 0},        // red scaling factor of 2
           new float[] {0,  1,  0,  0, 0},        // green scaling factor of 1
           new float[] {0,  0,  1,  0, 0},        // blue scaling factor of 1
           new float[] {0,  0,  0,  1, 0},        // alpha scaling factor of 1
           new float[] {.2f, .2f, .2f, 0, 1}};    // three translations of 0.2

            colorMatrixElements = new float[][] { 
           new float[] {255,  0,  0,  0, 0},        // red scaling factor of 2
           new float[] {0,  0,  0,  0, 0},        // green scaling factor of 1
           new float[] {0,  0,  0,  0, 0},        // blue scaling factor of 1
           new float[] {0,  0,  0,  1, 0},        // alpha scaling factor of 1
           new float[] {.2f, .2f, .2f, 0, 1}};    // three translations of 0.2

            //blanco y negro
            /*
            colorMatrixElements = new float[][] { 
                new float[] { 0.299f, 0.299f, 0.299f, 0, 0 }, 
                new float[] { 0.587f, 0.587f, 0.587f, 0, 0 }, 
                new float[] { 0.114f, 0.114f, 0.114f, 0, 0 }, 
                new float[] { 0,      0,      0,      1, 0 }, 
                new float[] { 0,      0,      0,      0, 1 } 
            };
             * */

            //g.DrawImage(Properties.Resources.bacteria, 0, 0);

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);

            g.DrawImage(
   Properties.Resources.bacteria2,
   new Rectangle(0, 0, width, height),  // destination rectangle 
   0, 0,        // upper-left corner of source rectangle 
   width,       // width of source rectangle
   height,      // height of source rectangle
   GraphicsUnit.Pixel,
   imageAttributes);


        }
    }
}
