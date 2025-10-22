using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public class TextBoxClean : TextBox
    {
        public TextBoxClean()
        {
            this.AllowPaint = false;

        }

        public bool AllowPaint
        {
            get;
            private set;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            this.Refresh();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.AllowPaint = true;
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            this.AllowPaint = false;
        }


        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.AllowPaint = false;
            this.Cursor = Cursors.Default;
        }

        private ToolTip Tip = new ToolTip();

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            this.AllowPaint = true;
            if (rectangle != null && rectangle.Contains(e.Location))
            {
                this.Cursor = Cursors.Hand;
                //Tip.Show("Clean text", this);
            }
            else
            {
                this.Cursor = Cursors.IBeam;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (rectangle != null && rectangle.Contains(e.Location))
                this.Text = "";
        }

        Rectangle rectangle;

        private static int WM_NCPAINT = 0x0085;
        private static int WM_ERASEBKGND = 0x0014;
        private static int WM_PAINT = 0x000F;

        [DllImport("user32.dll")]
        static extern IntPtr GetDCEx(IntPtr hwnd, IntPtr hrgnclip, uint fdwOptions);

        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hwnd, IntPtr hDC);

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCPAINT || m.Msg == WM_ERASEBKGND || m.Msg == WM_PAINT)
            {
                rectangle = new Rectangle(this.Bounds.Width - 16, this.Margin.Top, 0, 0);
                if (this.AllowPaint)
                {
                    Graphics graphics = this.CreateGraphics();// Graphics.FromHdc(hdc);
                    Color borderColor = Color.Blue;
                    rectangle = new Rectangle(this.Bounds.Width - 18, this.Margin.Top-1, 12, 12);

                    TextureBrush texture = new TextureBrush(global::UtilETWeb.Properties.Resources.cross_small);
                    texture.TranslateTransform(this.Bounds.Width - 19, this.Margin.Top-4, MatrixOrder.Prepend);

                    graphics.FillRectangle(Brushes.WhiteSmoke, rectangle);
                    graphics.FillRectangle(texture, rectangle);
                    graphics.DrawRectangle(Pens.LightGray, rectangle);
                    //ControlPaint.DrawBorder(graphics, rectangle, borderColor, ButtonBorderStyle.Solid);

                }

                m.Result = (IntPtr)1;
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        //private const int EM_SETRECT = 0xB3;

        //[DllImport(@"User32.dll", EntryPoint = @"SendMessage", CharSet = CharSet.Auto)]
        //private static extern int SendMessageRefRect(IntPtr hWnd, uint msg, int wParam, ref RECT rect);

        //[StructLayout(LayoutKind.Sequential)]
        //private struct RECT
        //{
        //    public readonly int Left;
        //    public readonly int Top;
        //    public readonly int Right;
        //    public readonly int Bottom;

        //    private RECT(int left, int top, int right, int bottom)
        //    {
        //        Left = left;
        //        Top = top;
        //        Right = right;
        //        Bottom = bottom;
        //    }

        //    public RECT(Rectangle r)
        //        : this(r.Left, r.Top, r.Right, r.Bottom)
        //    {
        //    }
        //}

        //public void SetPadding(TextBox textBox, Padding padding)
        //{
        //    var rect = new Rectangle(padding.Left, padding.Top, textBox.ClientSize.Width - padding.Left - padding.Right, textBox.ClientSize.Height - padding.Top - padding.Bottom);
        //    RECT rc = new RECT(rect);
        //    SendMessageRefRect(Handle, EM_SETRECT, 0, ref rc);
        //}

    }
}
