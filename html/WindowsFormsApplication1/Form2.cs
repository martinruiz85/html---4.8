using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Paint += new PaintEventHandler(Form1_Paint);
            this.Load += new EventHandler(Form1_Load);
        }

        void Form1_Load(object sender, EventArgs e)
        {
            this.Width = 250;
            this.Height = 250;
        }

        public int Width { get; set; }
        public int Height { get; set; }

        //https://www.mathopenref.com/coordcirclealgorithm.html
        //https://www.mathopenref.com/coordparamcircle.html

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float h = Height / 2f;
            float k = Width / 2f;
            float r = Height / 2f;

            for (int i = 0; i < 10; i++)
            {
                double theta = (360.00 / 10) * i;

                float x = h + r * (float)Math.Cos(ConvertToRadians(theta));
                float y = k - r * (float)Math.Sin(ConvertToRadians(theta));    //note 2.

                g.DrawLine(Pens.Black, h, k, x, y);

            }
        }

        public double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        public static double ConvertRadiansToDegrees(double radians)
        {
            double degrees = (180 / Math.PI) * radians;
            return (degrees);
        }
    }
}
