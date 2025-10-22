using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using UtilETWeb.Data;

namespace UtilETWeb
{
    public class TimeLine: UserControl
    {
   
        BackgroundWorker bw = new BackgroundWorker();

        public TimeLine()
        {

            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.BackColor = Color.White;
            this.Load += new EventHandler(frmTimeLine_Load);
            this.Paint += new PaintEventHandler(frmTimeLine_Paint);
            this.Resize += new EventHandler(frmTimeLine_Resize);

            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            this.Arrange(DataSource);
            this.AutoScrollMinSize = new Size(this.ClientSize.Width, Math.Max((int)Math.Round((61f * this.Rectangles.Count())), this.Height));
            this.Invalidate();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            List<SqlParameter> l = new List<SqlParameter>();
            //l.Add(new SqlParameter("@idSolicitud", 405));
            DataSource = Querys.ExecDatatable(
                "sp_GetHistoricoSolicitud",
                l,
                @"Data Source=vxmtymxintdev01\intsqldev01;Initial Catalog=Becas;Integrated Security=False;Uid=sa;Pwd=chopito;"
                //@"Data Source=vxmtymxsqltst\PSTST2005,1433;Initial Catalog=CheckupMedico;Persist Security Info=True;User ID=usr_checkup;Password=admcheckup"
                );
        }

        void frmTimeLine_Resize(object sender, EventArgs e)
        {
            if (DataSource != null)
                this.Arrange(this.DataSource);
        }

        void frmTimeLine_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            g.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y);

            SolidBrush drawWhite = new SolidBrush(Color.White);
            Font drawFontBold = new Font("Arial", 8, FontStyle.Bold);

            using (Pen pLightGray = new Pen(Color.FromArgb(250, Color.LightGray), 4))
            {
                using (Pen pOrange = new Pen(Color.FromArgb(250, Color.Orange), 4))
                {
                    if (Rectangles.Count > 1)
                        g.DrawLines(pOrange, Rectangles.Select<DesingRectangle, PointF>(square => new PointF(square.X + square.Width / 2f, square.Y + square.Height / 2f)).ToArray());


                    using (SolidBrush sbOrange = new SolidBrush(Color.FromArgb(250, Color.Orange)))
                    {
                        foreach (DesingRectangle item in this.Rectangles)
                        {
                            g.FillEllipse(Brushes.White, item.RectangleF);
                            this.FillEllipse(g, sbOrange, item.Center.X, item.Center.Y, 20, 20);
                            g.DrawArc(pOrange, item.RectangleF, 0, 360);


                            // Create font and brush.
                            Font drawFont = new Font("Arial", 9);

                            //SolidBrush drawWhite = new SolidBrush(Color.White);
                            SolidBrush drawBlack = new SolidBrush(Color.Black);
                            SolidBrush drawBrush = new SolidBrush(Color.DimGray);

                            // Create point for upper-left corner of drawing.
                            PointF drawPoint = new PointF(150.0F, 50.0F);

                            // Set format of string.
                            StringFormat drawFormat = new StringFormat();

                            drawFormat.FormatFlags = Rectangles.IndexOf(item) % 2 == 0 ? StringFormatFlags.NoFontFallback : StringFormatFlags.DirectionRightToLeft;
                            //drawFormat.Alignment = StringAlignment.Near;
                            //drawFormat.LineAlignment = StringAlignment.Near;


                            // Draw string to screen.
                            g.DrawString(item.Row.Field<string>("Descripcion"), drawFont, drawBlack, new PointF(item.Point.X + (Rectangles.IndexOf(item) % 2 == 0 ? item.Width + 5f : 0 - 5f), item.Y), drawFormat);
                            g.DrawString(item.Row.Field<DateTime>("FechaUltAct").ToString("dd/MM/yyyy HH:mm:ss"), drawFont, drawBrush, new PointF(item.Point.X + (Rectangles.IndexOf(item) % 2 == 0 ? item.Width + 5f : 0 - 5f), item.Y + 12f), drawFormat);
                            g.DrawString(item.Row.Field<string>("EmailUltAct"), drawFont, drawBrush, new PointF(item.Point.X + (Rectangles.IndexOf(item) % 2 == 0 ? item.Width + 5f : 0 - 5f), item.Y + 24f), drawFormat);


                            //Font drawFontBold = new Font("Arial", 8, FontStyle.Bold);

                            DrawstringFromCenter(Rectangles.IndexOf(item).ToString().PadLeft(2, '0'), drawFontBold, g, drawWhite, item.Center.X + this.AutoScrollPosition.X, item.Center.Y + this.AutoScrollPosition.Y, 20, 20);
                        }
                    }
                }
            }



        }

        public void FillEllipse(Graphics g, SolidBrush sb, float xCenter, float yCenter, float width, float height)
        {
            //Find the x-coordinate of the upper-left corner of the rectangle to draw.
            float x = xCenter - width / 2f;

            //Find y-coordinate of the upper-left corner of the rectangle to draw. 
            float y = yCenter - height / 2f;

            g.FillEllipse(sb, x, y, width, height);
        }

        public void DrawstringFromCenter(string text, Font font, Graphics g, SolidBrush sb, float xCenter, float yCenter, float width, float height)
        {

            //Find the x-coordinate of the upper-left corner of the rectangle to draw.
            float x = xCenter - width / 2f;

            //Find y-coordinate of the upper-left corner of the rectangle to draw. 
            float y = yCenter - height / 2f;

            // Create a TextFormatFlags with word wrapping, horizontal center and
            // vertical center specified.
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter |
                TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;

            // Draw the text and the surrounding rectangle.
            TextRenderer.DrawText(g, text, font, Rectangle.Round(new RectangleF(x + 1, y, width, height)), Color.White, flags);

        }

        DataTable DataSource
        {
            get;
            set;
        }

        List<DesingRectangle> Rectangles = new List<DesingRectangle>();

        void Arrange(DataTable dt)
        {
            Rectangles.Clear();

            float width = 30;
            float height = 30;
            float padding = 30;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DesingRectangle drect = new DesingRectangle()
                {
                    X = (this.ClientRectangle.Width - width) / 2f,
                    Y = i * height + padding,
                    Width = width,
                    Height = height,
                    Row = dt.Rows[i]
                };

                padding += 30;
                Rectangles.Add(drect);
            }

        }

        public void frmTimeLine_Load(object sender, EventArgs e)
        {
            bw.RunWorkerAsync();
        }
    }

    public class DesingRectangle : ICloneable
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Angule { get; set; }
        public PointF[] PointsF { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Padding Padding { get; set; }
        public Bitmap Bitmap { get; set; }
        public DataRow Row { get; set; }
        public RectangleF RectangleF
        {
            get
            {
                return new RectangleF(X, Y, Width, Height)
                {
                };
            }
        }

        public PointF Point
        {
            get
            {
                return new PointF(this.X, this.Y);
            }
        }

        public PointF Center
        {
            get
            {
                return new PointF(this.X + this.Width / 2f, this.Y + this.Height / 2f);
            }
        }

        public DesingRectangle()
        {
            this.Z = 0f;
            this.Angule = 0;
            this.PointsF = new PointF[3];
        }

        #region ICloneable Members

        public object Clone()
        {
            //
            // Instantiate and allocate the target array.
            //
            PointF[] target = new PointF[3];
            //
            // Copy the source to the target.
            //
            Array.Copy(this.PointsF, target, 3);

            DesingRectangle clone = new DesingRectangle()
            {

                X = this.X,
                Y = this.Y,
                Z = this.Z,
                Width = this.Width,
                Height = this.Height,
                Bitmap = this.Bitmap,
                //
                // Clone the target array.
                //
                PointsF = target,
                Angule = this.Angule
            };
            return clone;
        }

        #endregion
    }
}
