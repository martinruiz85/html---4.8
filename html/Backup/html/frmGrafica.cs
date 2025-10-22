using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Drawing2D;

namespace UtilETWeb
{
    public partial class frmGrafica : Form
    {
        public frmGrafica()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmGrafica_Load);
        }

        void frmGrafica_Load(object sender, EventArgs e)
        {

            Dictionary<string, int> tags = new Dictionary<string, int>() { 
            { "test", 10 },
            { "my", 3 },
            { "code", 8 }
        };

            // Data arrays
            string[] seriesArray = { "Cat", "Dog", "Bird", "Monkey" };
            double[] pointsArray = { 2, 1, 5, 5 };

            //chart1.Series[0].Points.Clear();
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "0.0";
            chart1.ChartAreas[0].AxisX.LabelStyle.Font = new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Regular);
            chart1.ChartAreas[0].AxisX.Minimum = 0.0;
            chart1.ChartAreas[0].AxisX.Interval = 0.5;
            chart1.ChartAreas[0].AxisX.Maximum = 5.0;
            chart1.ChartAreas[0].AxisX.Title = "Potencial";
            chart1.ChartAreas[0].AxisX.TitleFont = new Font(FontFamily.GenericSansSerif, 16f, FontStyle.Regular);


            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "0.0";
            chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Regular);
            chart1.ChartAreas[0].AxisY.Minimum = 0.0;
            chart1.ChartAreas[0].AxisY.Interval = 0.5;
            chart1.ChartAreas[0].AxisY.Maximum = 5.0;
            chart1.ChartAreas[0].AxisY.Title = "Desempeño";
            chart1.ChartAreas[0].AxisY.TitleFont = new Font(FontFamily.GenericSansSerif, 16f, FontStyle.Regular);


            // Set title
            //this.chart1.Titles.Add("Animals");
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            this.chart1.Series[0].IsValueShownAsLabel = false;
            this.chart1.Series[0].IsVisibleInLegend = false;

            //puntos del grafico
            chart1.Series[0].MarkerStyle = MarkerStyle.Circle;
            chart1.Series[0].MarkerSize = 5;
            chart1.Series[0].MarkerColor = Color.Red;

            //grid
            //chart1.ChartAreas[0].AxisX.MajorTickMark.Interval = 1;
            chart1.ChartAreas[0].Name = "ChartArea1";
            chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 2.5;
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chart1.PrePaint += new EventHandler<ChartPaintEventArgs>(chart1_PrePaint);

            chart1.Series[0].Points.AddXY(1, 3);

            // Add series.
            //for (int i = 0; i < seriesArray.Length; i++)
            //{
            //    Series series = this.chart1.Series.Add(seriesArray[i]);
            //    series.Points.Add(pointsArray[i]);
            //}


            //chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            //foreach (string tagname in tags.Keys)
            //{
            //    chart1.Series[0].Points.AddXY(tagname, tags[tagname]);
            //    //chart1.Series[0].IsValueShownAsLabel = true;
            //}

        }

        // Draw a rotated string at a particular position.
        private void DrawRotatedTextAt(Graphics gr, float angle,
            string txt, int x, int y, Font the_font, Brush the_brush)
        {
            // Save the graphics state.
            GraphicsState state = gr.Save();
            gr.ResetTransform();

            // Rotate.
            gr.RotateTransform(angle);

            // Translate to desired position. Be sure to append
            // the rotation so it occurs after the rotation.
            gr.TranslateTransform(x, y, MatrixOrder.Append);

            // Draw the text at the origin.
            gr.DrawString(txt, the_font, the_brush, 0, 0);

            // Restore the graphics state.
            gr.Restore(state);
        }

        void chart1_PrePaint(object sender, ChartPaintEventArgs e)
        {
            ChartGraphics cg = e.ChartGraphics;

            double xMax = e.ChartGraphics.GetPositionFromAxis("ChartArea1", AxisName.X, chart1.ChartAreas[0].AxisX.Maximum);
            double xMin = e.ChartGraphics.GetPositionFromAxis("ChartArea1", AxisName.X, chart1.ChartAreas[0].AxisX.Minimum);
            double yMax = e.ChartGraphics.GetPositionFromAxis("ChartArea1", AxisName.Y, chart1.ChartAreas[0].AxisY.Minimum);
            double yMin = e.ChartGraphics.GetPositionFromAxis("ChartArea1", AxisName.Y, chart1.ChartAreas[0].AxisY.Maximum);

            double width = xMax - xMin;
            double heigth = yMax - yMin;

            /*
            RectangleF myRect = new RectangleF((float)xMin, (float)yMin, (float)width, (float)heigth);
            myRect = e.ChartGraphics.GetAbsoluteRectangle(myRect);
            cg.Graphics.DrawRectangle(Pens.Black, Rectangle.Round(myRect));
            */

            Font fontSquared = new Font(FontFamily.GenericSansSerif, 20f, FontStyle.Regular);

            Pen pBlack = new Pen(Brushes.Black, .5f);
            SolidBrush bLightGreen = new SolidBrush(Color.FromArgb(230, 255, 153));
            SolidBrush bSimpleGreen = new SolidBrush(Color.FromArgb(155, 255, 155));
            SolidBrush bStrongGreen = new SolidBrush(Color.FromArgb(127, 211, 127));

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            //row1
            double rec1_width = width * (2.5 / 5.0);
            double rec1_height = heigth * (1.5 / 5.0);
            RectangleF myRect1 = new RectangleF((float)xMin, (float)yMin, (float)rec1_width, (float)rec1_height);
            myRect1 = e.ChartGraphics.GetAbsoluteRectangle(myRect1);
            cg.Graphics.FillRectangle(bLightGreen, myRect1);
            cg.Graphics.DrawRectangle(pBlack, myRect1.X, myRect1.Y, myRect1.Width, myRect1.Height);
            cg.Graphics.DrawString("6", fontSquared, Brushes.Black, myRect1, sf);

            SizeF sizetext1 = cg.Graphics.MeasureString("Sobresaliente", this.Font);
            DrawRotatedTextAt(cg.Graphics, -90f, "Sobresaliente", (int)(myRect1.X - 40), (int)(myRect1.Y + ((myRect1.Height - sizetext1.Width) / 2) + sizetext1.Width), this.Font, Brushes.Black);


            double rec2_width = width * (1.0 / 5.0);
            double rec2_height = heigth * (1.5 / 5.0);
            RectangleF myRect2 = new RectangleF((float)(xMin + rec1_width), (float)yMin, (float)rec2_width, (float)rec2_height);
            myRect2 = e.ChartGraphics.GetAbsoluteRectangle(myRect2);
            cg.Graphics.FillRectangle(bStrongGreen, myRect2);
            cg.Graphics.DrawRectangle(pBlack, myRect2.X, myRect2.Y, myRect2.Width, myRect2.Height);
            cg.Graphics.DrawString("3", fontSquared, Brushes.Black, myRect2, sf);

            double rec3_width = width * (1.5 / 5.0);
            double rec3_height = heigth * (1.5 / 5.0);
            RectangleF myRect3 = new RectangleF((float)(xMin + rec1_width + rec2_width), (float)yMin, (float)rec3_width, (float)rec3_height);
            myRect3 = e.ChartGraphics.GetAbsoluteRectangle(myRect3);
            cg.Graphics.FillRectangle(bStrongGreen, myRect3);
            cg.Graphics.DrawRectangle(pBlack, myRect3.X, myRect3.Y, myRect3.Width, myRect3.Height);
            cg.Graphics.DrawString("1", fontSquared, Brushes.Black, myRect3, sf);

            //row2
            double rec4_width = width * (2.5 / 5.0);
            double rec4_height = heigth * (1.5 / 5.0);
            RectangleF myRect4 = new RectangleF((float)xMin, (float)(yMin + rec1_height), (float)rec4_width, (float)rec4_height);
            myRect4 = e.ChartGraphics.GetAbsoluteRectangle(myRect4);
            cg.Graphics.DrawRectangle(pBlack, myRect4.X, myRect4.Y, myRect4.Width, myRect4.Height);
            cg.Graphics.DrawString("8", fontSquared, Brushes.Black, myRect4, sf);

            SizeF sizetext2 = cg.Graphics.MeasureString("Alto", this.Font);
            DrawRotatedTextAt(cg.Graphics, -90f, "Alto", (int)(myRect4.X - 40), (int)(myRect4.Y + ((myRect4.Height - sizetext2.Width) / 2) + sizetext2.Width), this.Font, Brushes.Black);


            double rec5_width = width * (1.0 / 5.0);
            double rec5_height = heigth * (1.5 / 5.0);
            RectangleF myRect5 = new RectangleF((float)(xMin + rec4_width), (float)(yMin + rec1_height), (float)rec5_width, (float)rec5_height);
            myRect5 = e.ChartGraphics.GetAbsoluteRectangle(myRect5);
            cg.Graphics.FillRectangle(bLightGreen, myRect5);
            cg.Graphics.DrawRectangle(pBlack, myRect5.X, myRect5.Y, myRect5.Width, myRect5.Height);
            cg.Graphics.DrawString("5", fontSquared, Brushes.Black, myRect5, sf);

            double rec6_width = width * (1.5 / 5.0);
            double rec6_height = heigth * (1.5 / 5.0);
            RectangleF myRect6 = new RectangleF((float)(xMin + rec4_width + rec5_width), (float)(yMin + rec1_height), (float)rec6_width, (float)rec6_height);
            myRect6 = e.ChartGraphics.GetAbsoluteRectangle(myRect6);
            cg.Graphics.FillRectangle(bStrongGreen, myRect6);
            cg.Graphics.DrawRectangle(pBlack, myRect6.X, myRect6.Y, myRect6.Width, myRect6.Height);
            cg.Graphics.DrawString("2", fontSquared, Brushes.Black, myRect6, sf);

            //row3
            double rec7_width = width * (2.5 / 5.0);
            double rec7_height = heigth * (2.0 / 5.0);
            RectangleF myRect7 = new RectangleF((float)xMin, (float)(yMin + rec4_height + rec1_height), (float)rec7_width, (float)rec7_height);
            myRect7 = e.ChartGraphics.GetAbsoluteRectangle(myRect7);
            cg.Graphics.DrawRectangle(pBlack, myRect7.X, myRect7.Y, myRect7.Width, myRect7.Height);
            cg.Graphics.DrawString("9", fontSquared, Brushes.Black, myRect7, sf);

            SizeF sizetext3 = cg.Graphics.MeasureString("Promedio", this.Font);
            DrawRotatedTextAt(cg.Graphics, -90f, "Promedio", (int)(myRect7.X - 40), (int)(myRect7.Y + ((myRect7.Height - sizetext3.Width) / 2) + sizetext3.Width), this.Font, Brushes.Black);

            SizeF sizetext4 = cg.Graphics.MeasureString("Promedio", this.Font);
            DrawRotatedTextAt(cg.Graphics, 0f, "Promedio", (int)(myRect7.X + ((myRect7.Width - sizetext4.Width) / 2)), (int)(myRect7.Y + 20 + myRect7.Height), this.Font, Brushes.Black);

            double rec8_width = width * (1.0 / 5.0);
            double rec8_height = heigth * (2.0 / 5.0);
            RectangleF myRect8 = new RectangleF((float)(xMin + rec1_width), (float)(yMin + rec4_height + rec1_height), (float)rec8_width, (float)rec8_height);
            myRect8 = e.ChartGraphics.GetAbsoluteRectangle(myRect8);
            cg.Graphics.FillRectangle(bLightGreen, myRect8);
            cg.Graphics.DrawRectangle(pBlack, myRect8.X, myRect8.Y, myRect8.Width, myRect8.Height);
            cg.Graphics.DrawString("7", fontSquared, Brushes.Black, myRect8, sf);

            SizeF sizetext5 = cg.Graphics.MeasureString("Alto", this.Font);
            DrawRotatedTextAt(cg.Graphics, 0f, "Alto", (int)(myRect8.X + ((myRect8.Width - sizetext5.Width) / 2)), (int)(myRect8.Y + 20 + myRect8.Height), this.Font, Brushes.Black);

            double rec9_width = width * (1.5 / 5.0);
            double rec9_height = heigth * (2.0 / 5.0);
            RectangleF myRect9 = new RectangleF((float)(xMin + rec1_width + rec2_width), (float)(yMin + rec4_height + rec1_height), (float)rec9_width, (float)rec9_height);
            myRect9 = e.ChartGraphics.GetAbsoluteRectangle(myRect9);
            cg.Graphics.FillRectangle(bSimpleGreen, myRect9);
            cg.Graphics.DrawRectangle(pBlack, myRect9.X, myRect9.Y, myRect9.Width, myRect9.Height);
            cg.Graphics.DrawString("4", fontSquared, Brushes.Black, myRect9, sf);

            SizeF sizetext6 = cg.Graphics.MeasureString("Muy Alto", this.Font);
            DrawRotatedTextAt(cg.Graphics, 0f, "Muy Alto", (int)(myRect9.X + ((myRect9.Width - sizetext6.Width) / 2)), (int)(myRect9.Y + 20 + myRect9.Height), this.Font, Brushes.Black);


        }

        private void chart1_Click(object sender, EventArgs e)
        {


        }
    }
}
