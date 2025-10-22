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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Paint += new PaintEventHandler(Form1_Paint);
            this.Load += new EventHandler(Form1_Load);
        }

        void Form1_Load(object sender, EventArgs e)
        {
            this.Width = 350;
            this.Height = 350;
        }

        public int Width { get; set; }
        public int Height { get; set; }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            g.Clear(Color.White);

            float cx = Height / 2f;
            float cy = Width / 2f;
            float r = Height / 3f;
            int ticks = 205;
            int no = 44;
            float porcent = no / (float)ticks;
            float borderSize = 20 / 2f;

            for (int i = 0; i < ticks - 1; i++)
            {
                double theta = (360.00 / ticks) * i;

                float x1 = cx + (r - borderSize) * (float)Math.Cos(ConvertToRadians(theta));
                float y1 = cy - (r - borderSize) * (float)Math.Sin(ConvertToRadians(theta));    //note 2.

                //g.DrawLine(Pens.Black, cx, cy, x, y);
                int w = 10;
                int h = 10;

                //g.FillEllipse(Brushes.Black, x - (w / 2f), y - (h / 2f), w, h);

                float x2 = cx + (r + borderSize) * (float)Math.Cos(ConvertToRadians(theta));
                float y2 = cy - (r + borderSize) * (float)Math.Sin(ConvertToRadians(theta));    //note 2.

                //g.FillEllipse(Brushes.Black, x - (w / 2f), y - (h / 2f), w, h);

                g.DrawLine(Pens.DarkGray, x1, y1, x2, y2);

                Pen pCenter = new Pen(Brushes.Black, borderSize);

                g.DrawArc(pCenter, CenterRectangle(cx, cy, r), 0, porcent * 360);

                Font f = new Font(FontFamily.GenericSansSerif.Name, Height / 6, FontStyle.Regular, GraphicsUnit.Point);

                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;

                RectangleF recttext = CenterRectangle((Width + 1F) / 2F, (Height + 1F) / 2F, Height / 3);
                //g.DrawString(string.Format("{0:0%}", porcent), f, Brushes.Black, recttext, sf);

                f = new Font(FontFamily.GenericSansSerif.Name, Height / 12, FontStyle.Regular, GraphicsUnit.Pixel);

                g.DrawString(string.Format("{2:0%}\n{0} de {1}", no, ticks, porcent), f, Brushes.Black, recttext, sf);



            }
        }

        public RectangleF CenterRectangle(float xCenter, float yCenter, float radius)
        {
            return new RectangleF()
            {
                X = xCenter - radius,
                Y = yCenter - radius,
                Width = radius * 2F,
                Height = radius * 2F
            };
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
