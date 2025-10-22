using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace UtilETWeb
{
    public static class MyExtensionGrapichs
    {
        public static void DrawRoundedRectangle(this Graphics g,
        Rectangle r, int d, Pen p)
        {

            System.Drawing.Drawing2D.GraphicsPath gp =
                    new System.Drawing.Drawing2D.GraphicsPath();

            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.X + r.Width - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Y + r.Height - d, d, d, 90, 90);
            gp.AddLine(r.X, r.Y + r.Height - d, r.X, r.Y + d / 2);

            g.DrawPath(p, gp);
        }
    }
}
