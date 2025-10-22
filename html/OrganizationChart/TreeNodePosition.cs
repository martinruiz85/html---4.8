using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace OrganizationChart
{
    public class TreeNodePosition : TreeNode<TreePositionData>
    {

        

        public TreeNodePosition(TreePositionData Data)
            : base(Data)
        {

        }

        public override System.Drawing.RectangleF DrawRectangle(float x, float y, float width, float height, string text, System.Drawing.Graphics g)
        {
            RectangleF rectF1 = new RectangleF(x, y, width, height);
            this.NodeRectangleF = rectF1;

            SolidBrush sbDarkGray = new SolidBrush(Color.DarkGray);
            GraphicsPath pathshadow = RoundedRectangle.Create((int)x + 4, (int)y + 4, (int)width, (int)height);
            g.FillPath(sbDarkGray, pathshadow);
            sbDarkGray.Dispose();

            SolidBrush sbColor = new SolidBrush(this.NodeColor);
            GraphicsPath path = RoundedRectangle.Create((int)x, (int)y, (int)width, (int)height);
            g.FillPath(sbColor, path);
            sbColor.Dispose();

            if (string.IsNullOrEmpty(Data.Title))
            {
                Pen p = new Pen(Brushes.Black, 2);
                p.DashStyle = DashStyle.Dash;
                g.DrawPath(p, path);
            }

            g.DrawPath(Pens.LightGray, path);

            using (Font font1 = new Font("Arial", 6, FontStyle.Regular, GraphicsUnit.Point))
            {
                StringFormat format = new StringFormat();
                format.Trimming = StringTrimming.EllipsisCharacter;
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;

                g.DrawString(Data.Title, font1, Brushes.Black, rectF1, format);
            }

            return rectF1;
        }
    }
}
