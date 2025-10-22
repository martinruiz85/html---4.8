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
    public partial class frmBlow : Form
    {
        public frmBlow()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmBlow_Load);
            this.Paint += new PaintEventHandler(frmBlow_Paint);
        }

        void frmBlow_Paint(object sender, PaintEventArgs e)
        {
            // Create pen.
            Pen blackPen = new Pen(Color.Black, 3);

            // Create points for curve.
            Point start = new Point(100, 100);
            Point control1 = new Point(200, 10);
            Point control2 = new Point(350, 50);
            Point end1 = new Point(500, 100);
            Point control3 = new Point(600, 150);
            Point control4 = new Point(650, 250);
            Point end2 = new Point(500, 300);
            Point[] bezierPoints =
             {
                 start, control1, control2, end1,
                 control3, control4, end2
             };

            // Draw arc to screen.
            //e.Graphics.DrawBeziers(blackPen, bezierPoints);
            TextureBrush tbrush = new TextureBrush(Properties.Resources.descarga);


            e.Graphics.DrawPolygon(blackPen, bezierPoints);
            e.Graphics.FillPolygon(tbrush, bezierPoints);

        }


        void frmBlow_Paint2(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            

            // Create pens.
            Pen redPen = new Pen(Color.Red, 3);
            Pen greenPen = new Pen(Color.Green, 3);

            // Create points that define curve.
            PointF point1 = new PointF(50.0F, 50.0F);
            PointF point2 = new PointF(100.0F, 25.0F);
            PointF point3 = new PointF(200.0F, 5.0F);
            PointF point4 = new PointF(250.0F, 50.0F);
            PointF point5 = new PointF(300.0F, 100.0F);
            PointF point6 = new PointF(350.0F, 200.0F);
            PointF point7 = new PointF(250.0F, 250.0F);
            PointF[] curvePoints = { point1, point2, point3, point4, point5, point6, point7 };

            // Draw lines between original points to screen.
            e.Graphics.DrawLines(redPen, curvePoints);

            // Draw closed curve to screen.
            e.Graphics.DrawClosedCurve(greenPen, curvePoints);


            e.Graphics.FillClosedCurve(Brushes.Bisque, curvePoints);



        }

        void frmBlow_Load(object sender, EventArgs e)
        {

        }
    }
}
