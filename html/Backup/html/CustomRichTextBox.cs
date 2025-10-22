using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace UtilETWeb
{
    public class CustomRichTextBox : RichTextBox
    {
        private int _SelectionEnd;
        public int SelectionEnd
        {
            get
            {
                return _SelectionEnd;
            }
            set
            {
                _SelectionEnd = value;
            }

        }

        private int _alpha;

        public int Alpha
        {
            get
            {
                return _alpha;
            }
            set
            {
                _alpha = value;
            }

        }

        private double _porcent;

        public double Porcent
        {
            get
            {
                return _porcent;
            }
            set
            {
                _porcent = value;
            }

        }


        public CustomRichTextBox()
        {
        }

        private const int WM_PAINT = 15;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PAINT)
            {
                if (!this.Enabled)
                {
                    System.Drawing.Point p1 = new System.Drawing.Point();

                    p1.X = this.GetPositionFromCharIndex(this.SelectionStart).X;
                    p1.Y = this.GetPositionFromCharIndex(this.SelectionStart).Y;

                    System.Drawing.Point p2 = new System.Drawing.Point();

                    p2.X = this.GetPositionFromCharIndex(this.SelectionStart + this.SelectedText.Length).X;
                    p2.Y = this.GetPositionFromCharIndex(this.SelectionStart + this.SelectedText.Length).Y;

                    string text = this.SelectedText;

                    this.Invalidate();
                    base.WndProc(ref m);
                    using (Graphics g = Graphics.FromHwnd(this.Handle))
                    {

                        //SizeF size = g.MeasureString(text, this.SelectionFont, p2.X - p1.X);
                        SizeF size = g.MeasureString(text, this.SelectionFont);

                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                        //SolidBrush sb = new SolidBrush(Color.FromArgb(this.Alpha, Color.Yellow));
                        SolidBrush sb = new SolidBrush(Color.FromArgb(255, Color.Yellow));

                        g.FillRectangle(Brushes.White, p1.X, p1.Y, p2.X - p1.X, size.Height);
                        g.FillRectangle(sb, p1.X, p1.Y, (float)(this.Porcent * (p2.X - p1.X)), size.Height);

                        StringFormat format = new StringFormat();
                        format.Trimming = StringTrimming.EllipsisCharacter;
                        format.LineAlignment = StringAlignment.Near;
                        format.Alignment = StringAlignment.Near;

                        if (this.SelectionLength > 0)
                        {
                            text = string.Format("{0:p0}" + text, Porcent, this.Porcent);
                            g.DrawString(text, this.SelectionFont, Brushes.Black, new RectangleF(p1.X, p1.Y, p2.X - p1.X, size.Height), format);
                        }
                    }
                }
                else 
                {
                    this.Invalidate();
                    base.WndProc(ref m);
                }


            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}
