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
    public partial class frmGifMM : Form
    {
        public frmGifMM()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmGifMM_Load);
            this.Paint += new PaintEventHandler(frmGifMM_Paint);
        }

        void frmGifMM_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            /*
            g.DrawImage(Properties.Resources.bow_images_clipart32px, new RectangleF((60f - 54f) / 2f, (60f - 32f) / 2f + 2, 54, 32));
            SolidBrush p = new SolidBrush(Color.Black);
            Pen pBlack = new Pen(p, 3);
            g.DrawArc(pBlack, new RectangleF(2, 2, 54, 54), 45, 90);
             * */

            generateUpdate(g);

        }

        void frmGifMM_Load(object sender, EventArgs e)
        {
            //generateUpdate(sender, e);

            int total = 24;
            for (int i = 0; i < total; i++)
            {
                Bitmap bmp = new Bitmap(60, 60);
                Graphics g = Graphics.FromImage(bmp);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                SolidBrush sbBlack = new SolidBrush(Color.Black);
                Pen pBlack = new Pen(sbBlack, 4);
                pBlack.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                pBlack.EndCap = System.Drawing.Drawing2D.LineCap.DiamondAnchor;// .Round;

                SolidBrush sbWhiteSmoke = new SolidBrush(Color.WhiteSmoke);
                Pen pWhiteSmoke = new Pen(sbWhiteSmoke, 4);

                SolidBrush sbWhite = new SolidBrush(Color.White);
                Pen pWhite = new Pen(sbWhiteSmoke, 8);

                g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
                g.DrawArc(pWhiteSmoke, new RectangleF(2, 2, 54, 54), 0, 360);
                g.DrawArc(pWhiteSmoke, new RectangleF(2, 2, 54, 54), 0, 360);
                g.DrawArc(pBlack, new RectangleF(2, 2, 54, 54),
                   (360f * (((float)i) / (float)total)),
                   (360f * (((float)(i + 6)) / (float)total)) - (360f * (((float)i) / (float)total))
                   );

                g.DrawImage(Properties.Resources.bow_images_clipart32px, new RectangleF((60f - 54f) / 2f, (60f - 32f) / 2f + 2, 54, 32));

                bmp.Save(string.Format("MM{0}.png", i));

            }

        }

        void generateUpdate(Graphics gx)
        {
            int total = 101;
            for (int i = 0; i < total; i++)
            {
                //Bitmap bmp = new Bitmap(20, 20);
                Bitmap bitmap = Properties.Resources.available_updates_;

                Graphics g = Graphics.FromImage(bitmap);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                float alpha = i / 100f;

                // Initialize the color matrix.
                // Note the value 0.8 in row 4, column 4.
                float[][] matrixItems ={
                   new float[] {1, 0, 0, 0, 0},
                   new float[] {0, 1, 0, 0, 0},
                   new float[] {0, 0, 1, 0, 0},
                   new float[] {0, 0, 0, 1, 0},
                   new float[] {0, 0, 0, 0, 1}};
                ColorMatrix colorMatrix = new ColorMatrix(matrixItems);

                // Create an ImageAttributes object and set its color matrix.
                ImageAttributes imageAtt = new ImageAttributes();
                imageAtt.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                //g.DrawImage(Properties.Resources.bow_images_clipart32px, new RectangleF(0, 0, 20, 20));

                // Now draw the semitransparent bitmap image.
                /*
                int iWidth = bitmap.Width;
                int iHeight = bitmap.Height;
                g.DrawImage(
                   bitmap,
                   new Rectangle(30, 0, iWidth, iHeight),  // destination rectangle
                   0.0f,                          // source rectangle x
                   0.0f,                          // source rectangle y
                   iWidth,                        // source rectangle width
                   iHeight,                       // source rectangle height
                   GraphicsUnit.Pixel,
                   imageAtt);*/

                Bitmap b1 = new Bitmap(20, 20, PixelFormat.Format32bppPArgb);
                int iWidth = b1.Width;
                int iHeight = b1.Height;
                Graphics gy = Graphics.FromImage(b1);
                //gy.Clear(Color.White);
                //gy.FillRectangle(Brushes.Red, 0, 0, b1.Width, b1.Height);
                //gy.DrawImage(bitmap, new RectangleF(0, 0, 20, 20));

                SolidBrush sb1 = new SolidBrush(Color.FromArgb(255, 238, 238, 238));
                gy.FillRectangle(sb1, 0, 0, b1.Width, b1.Height);

                gy.DrawImage(bitmap, new RectangleF(0, 0, 20, 20));

                SolidBrush sb2 = new SolidBrush(Color.FromArgb((int)(alpha * 255), 238, 238, 238));
                gy.FillRectangle(sb2, 0, 0, b1.Width, b1.Height);

                /*
                gx.DrawImage(
                   bitmap,
                   new Rectangle(30, 0, iWidth, iHeight),  // destination rectangle
                   0.0f,                          // source rectangle x
                   0.0f,                          // source rectangle y
                   iWidth,                        // source rectangle width
                   iHeight,                       // source rectangle height
                   GraphicsUnit.Pixel,
                   imageAtt);
                 */

                b1.Save(string.Format("available_updates_{0}.png", i));




            }
        }

    }
}
