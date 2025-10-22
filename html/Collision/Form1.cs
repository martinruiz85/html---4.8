using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Collision
{
    public partial class Form1 : Form
    {
        public class RecF
        {
            public int X { get; set; }
            public int Y { get; set; }
            public double Width { get; set; }
            public double Heigth { get; set; }
            public Bitmap Bmp { get; set; }

            public Point P
            {
                get
                {
                    return new Point(this.X, this.Y);
                }
            }

            public RecF(int x, int y, double width, double height, Bitmap bmp)
            {
                this.X = x;
                this.Y = y;
                this.Width = width;
                this.Heigth = height;
                this.Bmp = bmp;
            }

            public void Draw(Graphics g)
            {
                g.DrawImage(this.Bmp, this.P);
            }

        }

        public List<RecF> rects = new List<RecF>();

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Paint += new PaintEventHandler(Form1_Paint);
            this.Load += new EventHandler(Form1_Load);
        }

        void Form1_Load(object sender, EventArgs e)
        {
            RecF blue = new RecF(0, 0, 10, 10, Properties.Resources.empty);
            RecF red = new RecF(5, 5, 10, 10, Properties.Resources.red);

            rects.Add(blue);
            rects.Add(red);

            //Bitmap bmp = new Bitmap(10, 10);


            //Graphics myGraphics = Graphics.FromImage(bmp);

            ////myGraphics.Clear(Color.White);
            //myGraphics.Clear(Color.Blue);

            ////myGraphics.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);

            //// some code with draw on myGraphics
            //myGraphics.Dispose();

            // bmp.Save("empty.png",System.Drawing.Imaging.ImageFormat.Png);

            detectCollsion(blue);
            detectCollsion(red);

        }

        //https://medium.com/@euryperez/android-pearls-pixel-perfect-collision-detection-with-no-framework-53a5137baca2
        bool detectCollsion(RecF blue)
        {
            for (int i = 0; i < blue.Width; i++)
            {
                for (int j = 0; j < blue.Heigth; j++)
                {
                    //have color
                    if (blue.Bmp.GetPixel(i, j).A != 0)
                        return true;

                }
            }
            return false;
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            foreach (RecF item in this.rects)
            {
                item.Draw(g);
            }

        }
    }
}
