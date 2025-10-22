using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq.Expressions;
using System.Reflection;


namespace WebApplication1.App_Code
{
    public class TreeNode<T>
    {
        public int Level { get; set; }

        public PointF Point { get; set; }
        public float NodeWidth { get; set; }
        public float NodeHeight { get; set; }
        public float NodeVerticalSpacing { get; set; }
        public float NodeHorizontalSpacing { get; set; }
        public RectangleF NodeRectangleF { get; set; }
        public Graphics NodeGraphics { get; set; }
        public Pen NodePen { get; set; }
        public Color NodeColor { get; set; }

        public RectangleF DrawRectangle(Graphics g, string text)
        {
            return this.DrawRectangle(this.Point.X, this.Point.Y, this.NodeWidth, this.NodeHeight, text, g);
        }

        public RectangleF DrawRectangle(Graphics g)
        {
            return this.DrawRectangle(this.Point.X, this.Point.Y, this.NodeWidth, this.NodeHeight, this.Data.ToString(), g);
        }

        public virtual RectangleF DrawRectangle(float x, float y, float width, float height, string text, Graphics g)
        {
            RectangleF rectF1 = new RectangleF(x, y, width, height);
            this.NodeRectangleF = rectF1;
            return rectF1;
        }

        public T Data { get; set; }

        public List<TreeNode<T>> TreeNodeItem { get; set; }

        public TreeNode<T> Parent { get; set; }

        public TreeNode(T Data)
        {           
            this.Level = 0;
            this.Point = new PointF(0, 0);
            this.NodeWidth = 20F;
            this.NodeHeight = 20F;
            this.NodeVerticalSpacing = 20F;
            this.NodeHorizontalSpacing = 10F;
            this.NodePen = new Pen(Color.LightGray, 3);
            this.NodeColor = Color.White;

            this.Data = Data;
            this.TreeNodeItem = new List<TreeNode<T>>() { };
        }
    }
}
