using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace UtilETWeb
{
    public class DesingRichTextBox: RichTextBox
    {
        public DesingRichTextBox() 
            :base()
        {
            //this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //this.SetStyle(ControlStyles.UserPaint, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //this.Cursor = System.Windows.Forms.Cursors.IBeam;
            //this.ImeMode = System.Windows.Forms.ImeMode.On;
            //this.BackgroundImage = Properties.Resources.arrow;
        }

        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            //g.DrawImage(Properties.Resources.arrow,new Point(0,0));
            e.Graphics.DrawString(Text, Font, new SolidBrush(this.ForeColor),
               new RectangleF(0, 0, this.Width, this.Height), StringFormat.GenericDefault);

        }
    }
}
