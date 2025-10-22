using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Linq.Expressions;
using System.Drawing;
using System.Drawing.Drawing2D;
using UtilETWeb;

namespace html
{
    public class RadialTreePanel<T> : Panel
         where T : class, new()
    {
        public static List<Node<TNode>> Find<TNode>(Node<TNode> Node, Expression<Func<Node<TNode>, bool>> Lambda)
             where TNode : class, new()
        {
            List<Node<TNode>> l = new List<Node<TNode>>() { };
            Func<Node<TNode>, bool> Result = Lambda.Compile();
            if (Result(Node)) l.Add(Node);
            foreach (Node<TNode> Item in Node.Nodes)
            {
                l.AddRange(Find<TNode>(Item, Lambda));
            }
            return l;
        }

        public static int zIndex = 0;

        public Node<T> TreeNode { get; set; }

        public RadialTreePanel()
        {
            this.DoubleBuffered = true;
            this.MouseDown += new MouseEventHandler(TreePanelBase_MouseDown);
            this.MouseMove += new MouseEventHandler(TreePanelBase_MouseMove);
            this.DragDrop += new DragEventHandler(RadialTreePanel_DragDrop);
            this.Paint += new PaintEventHandler(RadialTreePanel_Paint);
        }

        void RadialTreePanel_DragDrop(object sender, DragEventArgs e)
        {
        }

        public event DelegateTreeNode_MouseDown MyTreeNode_MouseDown;
        public delegate void DelegateTreeNode_MouseDown(Node<T> Node, object sender, MouseEventArgs e);
        public Node<T> SelectNode { get; set; }

        void TreePanelBase_MouseDown(object sender, MouseEventArgs e)
        {
            this.MouseDown -= new MouseEventHandler(TreePanelBase_MouseDown);
            this.TreeNode_MouseDown(this.TreeNode, sender, e);
            this.Invalidate();
            this.Refresh();
            this.MouseDown += new MouseEventHandler(TreePanelBase_MouseDown);
        }

        void TreeNode_MouseDown(Node<T> Node, object sender, MouseEventArgs e)
        {
            Point NewLocation = new Point(e.Location.X + this.HorizontalScroll.Value, e.Location.Y + this.VerticalScroll.Value);
            if (Node.NodeRectangleF.Contains(NewLocation))
            {
                this.SelectNode = Node;
                this.SelectNode.NodePen = new Pen(Color.OrangeRed, 3);
                if (MyTreeNode_MouseDown != null)
                    MyTreeNode_MouseDown(Node, sender, e);
            }
            else
            {
                if (this.SelectNode == Node)
                {
                    this.SelectNode = null;
                }
                Node.NodePen = new Pen(Color.LightGray, 3);
            }
            foreach (Node<T> Item in Node.Nodes)
            {
                this.TreeNode_MouseDown(Item, sender, e);
            }
        }

        public event DelegateTreeNode_MouseMove MyTreeNode_MouseMove;
        public event DelegateTreeNode_MouseHover MyTreeNode_MouseHover;
        public event DelegateTreeNode_MouseOut MyTreeNode_MouseOut;

        public delegate void DelegateTreeNode_MouseMove(Node<T> Node, object sender, MouseEventArgs e);
        public delegate void DelegateTreeNode_MouseHover(Node<T> Node, object sender, MouseEventArgs e);
        public delegate void DelegateTreeNode_MouseOut(Node<T> Node, object sender, MouseEventArgs e);
        private Node<T> HoverNode;

        void TreePanelBase_MouseMove(object sender, MouseEventArgs e)
        {
            this.TreePanelBase_MouseMove(this.TreeNode, sender, e);
            this.Invalidate();
            this.Refresh();

        }

        void TreePanelBase_MouseMove(Node<T> Node, object sender, MouseEventArgs e)
        {

            Point NewLocation = new Point(e.Location.X + this.HorizontalScroll.Value, e.Location.Y + this.VerticalScroll.Value);
            if (Node.NodeRectangleF.Contains(NewLocation))
            {
                //if (this.SelectNode != Node) Node.NodePen = new Pen(Color.DarkTurquoise, 3);
                if (this.SelectNode != Node) Node.NodePen = new Pen(Color.FromArgb(255, 191, 51), 3);
                if (this.HoverNode != Node)
                {
                    Cursor.Current = Cursors.Hand;
                    this.Cursor = Cursors.Hand;
                    this.HoverNode = Node;
                    if (this.MyTreeNode_MouseHover != null)
                        this.MyTreeNode_MouseHover(Node, sender, e);
                }

                if (MyTreeNode_MouseMove != null)
                    MyTreeNode_MouseMove(Node, sender, e);
            }
            else
            {
                if (this.SelectNode != Node) Node.NodePen = new Pen(Color.LightGray, 3);
                if (this.HoverNode == Node)
                {
                    this.HoverNode = null;
                    Cursor.Current = Cursors.Arrow;
                    this.Cursor = Cursors.Arrow;
                    if (this.MyTreeNode_MouseOut != null)
                        this.MyTreeNode_MouseOut(Node, sender, e);
                }
            }

            foreach (Node<T> Item in Node.Nodes)
            {
                this.TreePanelBase_MouseMove(Item, sender, e);
            }

        }


        void RadialTreePanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(this.BackColor);
            g.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            if (this.TreeNode != null)
            {
                for (int i = 0; i < this.TreeNode.Depth; i++)
                {
                    Pen myPen = new Pen(Color.LightGray, 1 * (this.TreeNode.Depth - i));


                    Pen myPen2 = new Pen(Color.Black, 1 * (this.TreeNode.Depth - i));
                    float[] dashValues = { i + 1, i + 1 };
                    myPen2.DashPattern = dashValues;
                    myPen2.Alignment = PenAlignment.Outset;



                    //g.DrawEllipse(myPen, Rectangle.Round(circle2Rect(new PointF(this.ROOTX, this.ROOTX), this.RADIUSY * i + 1)));
                    if (this.SelectNode != null && this.SelectNode.Level == i)
                    {
                        LinearGradientBrush linGrBrush = new LinearGradientBrush(
                            Rectangle.Round(circle2Rect(new PointF(this.ROOTX, this.ROOTX), this.RADIUSY * i + 1)),
                            //this.SelectNode.NodePen.Color,
                            Color.LightGray,
                            Color.Gray,
                            RadiansToDegrees(this.SelectNode.angle),
                        false
                            );

                        Pen pen = new Pen(linGrBrush, 1 * (this.TreeNode.Depth - i));

                        g.DrawEllipse(pen, Rectangle.Round(circle2Rect(new PointF(this.ROOTX, this.ROOTX), this.RADIUSY * i + 1)));
                    }
                    else
                        g.DrawEllipse(myPen, Rectangle.Round(circle2Rect(new PointF(this.ROOTX, this.ROOTX), this.RADIUSY * i + 1)));


                }
                this.PaintNodesLines(this.TreeNode, g);
                this.PaintNodes(this.TreeNode, g);
            }
        }

        RectangleF circle2Rect(PointF midPoint, float radius)
        {
            return new RectangleF(midPoint.X - radius,
                                 midPoint.Y - radius,
                                 radius * 2,
                                 radius * 2);
        }

        private float RadiansToDegrees(float radians)
        {
            return (float)(radians * (180 / Math.PI));
        }


        private float DegreesToRadians(float degrees)
        {
            return (float)(degrees / (180 / Math.PI));
        }


        private PointF PolarToRectangular(
        double radius,
        double theta)
        {
            try
            {
                double sin = Math.Sin(theta);

                // This is faster then:
                // double cos = Math.Cos(theta);
                double cos = -Math.Sqrt(1 - (sin * sin));

                float x = this.ROOTX + (float)Math.Round(radius * cos);
                float y = this.ROOTY + (float)Math.Round(radius * sin);

                return new PointF(x, y);
            }
            catch (OverflowException ex)
            {
                ex.Data.Add("Screen polar Radius", radius);
                ex.Data.Add("Screen polar Theta", theta);
                throw;
            }
        }

        public void PaintNodesLines(Node<T> Node, Graphics g)
        {
            float radius = ((float)(this.Depth - Node.Level) / (this.Depth)) * (this.RADIUSX / this.Depth);
            foreach (Node<T> Item in Node.Nodes)
            {

                float correctionFactor = 1f;
                float red = (255 - Node.NodePen.Color.R) * correctionFactor + Node.NodePen.Color.R;
                float green = (255 - Node.NodePen.Color.G) * correctionFactor + Node.NodePen.Color.G;
                float blue = (255 - Node.NodePen.Color.B) * correctionFactor + Node.NodePen.Color.B;
                Color lighterColor = Color.FromArgb(Node.NodePen.Color.A, (int)red, (int)green, (int)blue);

                LinearGradientBrush linGrBrush = new LinearGradientBrush(
                    new PointF(Node.x, Node.y),
                    new PointF(Item.x, Item.y),
                    Node.NodePen.Color,
                    lighterColor
                    );

                //Pen pen = new Pen(linGrBrush, Item.Depth + 1);
                Pen pen = new Pen(linGrBrush, 1);
                pen.EndCap = LineCap.Triangle;


                Pen AuxPen = new Pen(Node.NodePen.Brush, Item.Depth + 1);
                float[] dashValues = { 2, 2 };
                AuxPen.DashPattern = dashValues;
                g.DrawLine(new Pen(AuxPen.Color), new PointF(Node.x, Node.y), new PointF(Item.x, Item.y));

                //PointF NewPointFParent = (new PolarToCartesian(Node.x, Node.y, (radius * 3), RadiansToDegrees(Node.angle))).Point;
                //PointF NewPointFChild = (new PolarToCartesian(Item.x, Item.y, -(radius * 3), RadiansToDegrees(Item.angle))).Point;
                //g.DrawBezier(pen, new PointF(Node.x, Node.y),
                //    NewPointFParent,
                //    NewPointFChild,
                //    new PointF(Item.x, Item.y));

                //PointF NewPointFParent = (new PolarToCartesian(Node.x, Node.y, (radius * 5), RadiansToDegrees(Node.angle))).Point;
                //PointF NewPointFChild = (new PolarToCartesian(Item.x, Item.y, -(radius * 1), RadiansToDegrees(Item.angle))).Point;
                //g.DrawBezier(AuxPen, new PointF(Node.x, Node.y),
                //    NewPointFParent,
                //    NewPointFChild,
                //    new PointF(Item.x, Item.y));

                //PointF NewPointFParent = (new PolarToCartesian(Node.x, Node.y, -(radius * 3), RadiansToDegrees(Node.angle))).Point;
                //PointF NewPointFChild = (new PolarToCartesian(Item.x, Item.y, (radius * 3), RadiansToDegrees(Item.angle))).Point;                
                //g.DrawBezier(AuxPen, new PointF(Node.x, Node.y),
                //    NewPointFParent,
                //    NewPointFChild,                                        
                //    new PointF(Item.x, Item.y));

                //PointF NewPointFParent = (new PolarToCartesian(Node.x, Node.y, -(radius * 1), RadiansToDegrees(Node.angle))).Point;
                //PointF NewPointFChild = (new PolarToCartesian(Item.x, Item.y, -(radius * 3), RadiansToDegrees(Item.angle))).Point;
                //g.DrawBezier(AuxPen, new PointF(Node.x, Node.y),
                //    NewPointFChild,
                //    NewPointFParent,
                //    new PointF(Item.x, Item.y));

                PaintNodesLines(Item, g);
            }
        }

        protected virtual string GetText(Node<T> Node)
        {
            return string.Format("{0}:{1}:{2}", Node.Item, Node.Ancestors.Count(), Node.zIndex);
        }

        public IEnumerable<Color> DistinctColors(int n)
        {
            int m = (int)Math.Ceiling(Math.Pow(n, 1 / 3.0));

            for (int green = 0; green <= m; ++green)
            {
                for (int blue = 0; blue <= m; ++blue)
                {
                    for (int red = 0; red <= m; ++red)
                    {
                        if (n-- == 0)
                            yield break;

                        int r = (int)(0.5 + red * 255.0 / m);
                        int g = (int)(0.5 + green * 255.0 / m);
                        int b = (int)(0.5 + blue * 255.0 / m);

                        yield return Color.FromArgb(r, g, b);
                    }
                }
            }
        }

        public void PaintNodes(Node<T> Node, Graphics g)
        {
            ++zIndex;
            Node.zIndex = zIndex;
            float radius = ((float)(this.Depth - Node.Level) / (this.Depth)) * (this.RADIUSX / this.Depth);
            //List<Color> l = WheelOfColor.Instance.Generate((int)this.Depth); //DistinctColors((int)this.Depth).ToList();
            //float radius = 9;

            //float x = Node.x - radius;
            //float y = Node.y - radius;
            //float width = 2 * radius;
            //float height = 2 * radius;

            foreach (Node<T> Item in Node.Nodes)
            {
                //g.DrawLine(Pens.LightGray, new PointF(Node.x, Node.y), new PointF(Item.x, Item.y));
                PaintNodes(Item, g);
            }

            //g.DrawEllipse(Pens.Black, x, y, width, height);
            Font drawFont = new Font("Trebuchet MS", 8);
            // Create a StringFormat object with the each line of text, and the block
            // of text centered on the page.
            StringFormat stringFormat = new StringFormat(StringFormatFlags.NoClip);
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            // Create a new pen.
            Pen skyBluePen = new Pen(Brushes.LightGray);

            // Set the pen's width.
            skyBluePen.Width = 6.0F;

            // Set the LineJoin property.
            skyBluePen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;

            TextureBrush texture = new TextureBrush(global::UtilETWeb.Properties.Resources.node);
            texture.TranslateTransform(Node.x + 8, Node.y + 8, MatrixOrder.Prepend);

            Node.NodeRectangleF = circle2Rect(new PointF(Node.x, Node.y), radius);
            g.DrawEllipse(Node.NodePen, Rectangle.Round(circle2Rect(new PointF(Node.x, Node.y), radius + 3)));
            if (Node.Nodes.Count > 0)
            {
                //g.FillEllipse(Brushes.LightGray, Rectangle.Round(circle2Rect(new PointF(Node.x, Node.y), radius)));
                //g.DrawString(Node.Item.ToString(), drawFont, Brushes.White, circle2Rect(new PointF(Node.x, Node.y), radius), stringFormat);

                SolidBrush newbrush;
                //SolidBrush newbrush = new System.Drawing.SolidBrush(l[(int)(Node.Level % this.Depth)]);
                if (Node.IsRoot)
                {
                    newbrush = new SolidBrush(Color.Gray);
                }
                else if (Node.Level == 1)
                {
                    List<Color> l = WheelOfColor.Instance.Generate(Node.Parent.Nodes.Count);
                    newbrush = new SolidBrush(l[Node.Parent.Nodes.IndexOf(Node)]);
                    Node.NodeBrush = newbrush;
                }
                else if (Node.Level > 1)
                {
                    newbrush = Node.Parent.NodeBrush;
                    Node.NodeBrush = newbrush;
                }
                else
                {
                    newbrush = new SolidBrush(Color.White);
                }

                g.FillEllipse(newbrush, Rectangle.Round(circle2Rect(new PointF(Node.x, Node.y), radius + 2)));

                //descomentar
                //g.FillEllipse(Brushes.WhiteSmoke, Rectangle.Round(circle2Rect(new PointF(Node.x, Node.y), radius + 2)));
                //g.FillEllipse(texture, Rectangle.Round(circle2Rect(new PointF(Node.x, Node.y), radius)));

                //DrawText(false, g, Node.Item.ToString(), drawFont, Brushes.Black, stringFormat, Node.x, Node.y, radius*2, radius*2,Node.angle);
                //g.DrawString(Node.Item.ToString(), drawFont, Brushes.Black, circle2Rect(new PointF(Node.x, Node.y + 20), radius * 2), stringFormat);

                GraphicsPath myPath = new GraphicsPath();


                // Set up all the string parameters.
                //string stringText = (Node.angle * (180 / Math.PI)).ToString(); //Node.Item.ToString();
                string stringText = GetText(Node);
                FontFamily family = new FontFamily("Trebuchet MS");
                int fontStyle = (int)FontStyle.Regular;
                int emSize = 9;
                PointF origin = new PointF(Node.x, Node.y);
                StringFormat format = new StringFormat(StringFormatFlags.DirectionRightToLeft); //new StringFormat(StringFormatFlags.LineLimit);
                //format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;





                // Add the string to the path.
                //myPath.AddRectangle(Rectangle.Round(new RectangleF(new PointF(Node.x, Node.y), new SizeF(radius * 2, radius * 2))));

                PointF NewPointF = (new PolarToCartesian(Node.x, Node.y, -(radius + 2), RadiansToDegrees(Node.angle))).Point;

                myPath.AddString(stringText,
                    family,
                    fontStyle,
                    emSize,
                    //new PointF(Node.x, Node.y),
                    NewPointF,
                    //Rectangle.Round(circle2Rect(new PointF(Node.x, Node.y), radius)),
                    //Rectangle.Round(new RectangleF(new PointF(Node.x, Node.y), new SizeF(radius, radius))),
                    format);


                // Move the ellipse 100 points to the right.
                Matrix translateMatrix = new Matrix();
                //translateMatrix.Rotate(Node.angle, MatrixOrder.Prepend);
                //translateMatrix.Translate(9, 9);
                translateMatrix.RotateAt(
                    RadiansToDegrees(Node.angle),
                    //new PointF(Node.x, Node.y), 
                    NewPointF,
                    MatrixOrder.Append);
                myPath.Transform(translateMatrix);

                g.DrawPath(new Pen(Color.Black, 0.5F), myPath);

            }
            else
            {
                //g.FillEllipse(Brushes.LightGray, Rectangle.Round(circle2Rect(new PointF(Node.x, Node.y), radius)));
                //g.DrawString(Node.Item.ToString(), drawFont, Brushes.White, circle2Rect(new PointF(Node.x, Node.y), radius), stringFormat);

                SolidBrush newbrush;
                //SolidBrush newbrush = new System.Drawing.SolidBrush(l[(int)(Node.Level % this.Depth)]);
                if (Node.IsRoot)
                {
                    newbrush = new SolidBrush(Color.Gray);
                }
                else if (Node.Level == 1)
                {
                    List<Color> l = WheelOfColor.Instance.Generate(Node.Parent.Nodes.Count);
                    newbrush = new SolidBrush(l[Node.Parent.Nodes.IndexOf(Node)]);
                    Node.NodeBrush = newbrush;
                }
                else if (Node.Level > 1)
                {
                    newbrush = Node.Parent.NodeBrush;
                    Node.NodeBrush = newbrush;
                }
                else 
                {
                    newbrush = new SolidBrush(Color.White);
                }

                g.FillEllipse(newbrush, Rectangle.Round(circle2Rect(new PointF(Node.x, Node.y), radius + 2)));
                //descomentar
                //g.FillEllipse(Brushes.White, Rectangle.Round(circle2Rect(new PointF(Node.x, Node.y), radius + 2)));
                //g.FillEllipse(texture, Rectangle.Round(circle2Rect(new PointF(Node.x, Node.y), radius)));

                //g.DrawString(Node.Item.ToString(), drawFont, Brushes.Gray, circle2Rect(new PointF(Node.x, Node.y + 20), radius * 2), stringFormat);

                GraphicsPath myPath = new GraphicsPath();

                // Set up all the string parameters.
                //string stringText = (Node.angle * (180 / Math.PI)).ToString();
                string stringText = this.GetText(Node);
                FontFamily family = new FontFamily("Trebuchet MS");
                int fontStyle = (int)FontStyle.Regular;
                int emSize = 9;
                PointF origin = new PointF(Node.x, Node.y);
                StringFormat format = new StringFormat(StringFormatFlags.NoClip); //new StringFormat(StringFormatFlags.LineLimit);
                //format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;





                // Add the string to the path.
                //myPath.AddRectangle(Rectangle.Round(new RectangleF(new PointF(Node.x, Node.y), new SizeF(radius * 2, radius * 2))));
                PointF NewPointF = (new PolarToCartesian(Node.x, Node.y, radius + 5, RadiansToDegrees(Node.angle))).Point;

                myPath.AddString(stringText,
                    family,
                    fontStyle,
                    emSize,
                    //new PointF(Node.x, Node.y),
                    NewPointF,
                    //Rectangle.Round(circle2Rect(new PointF(Node.x, Node.y), radius)),
                    //Rectangle.Round(new RectangleF(new PointF(Node.x, Node.y), new SizeF(radius, radius))),
                    format);


                // Move the ellipse 100 points to the right.
                Matrix translateMatrix = new Matrix();
                //translateMatrix.Rotate(Node.angle, MatrixOrder.Prepend);
                //translateMatrix.Translate(9, 9);
                translateMatrix.RotateAt((float)(Node.angle * (180 / Math.PI)),
                    //new PointF(Node.x, Node.y),
                    NewPointF,
                    MatrixOrder.Append);
                myPath.Transform(translateMatrix);

                g.DrawPath(new Pen(Color.Gray, 1F), myPath);



            }



            //g.DrawRectangle(Pens.Blue, Rectangle.Round(circle2Rect(new PointF(Node.x, Node.y), radius)));
            //g.FillRectangle(Brushes.White, Rectangle.Round(circle2Rect(new PointF(Node.x, Node.y), radius)));

        }

        public float _zomm = 1.0F;


        public void ItemBind()
        {
            this.ROOTX = (this.Width * _zomm) / 2;
            this.ROOTY = (this.Height * _zomm) / 2;

            if (this.TreeNode != null)
            {
                this.Depth = this.TreeNode.Depth;
                this.RADIUSX = ROOTX / Depth;
                this.RADIUSY = ROOTY / Depth;

                this.TreeNode.x = this.ROOTX;
                this.TreeNode.y = this.ROOTY;

                List<Node<T>> l = new List<Node<T>>() { this.TreeNode };
                TreeLayoutN(l, 1);
                this.AutoScrollMinSize = new Size((int)(this.Width * _zomm), (int)(this.Height * _zomm));
                this.Refresh();
            }
        }

        private static float TWO_PI = (float)Math.PI * (float)2.0;
        private static float PI = (float)Math.PI * (float)1.0;
        private float d = 25;

        private float ROOTX = 0;
        private float ROOTY = 0;
        private float RADIUSX = 0;
        private float RADIUSY = 0;
        private float Depth = 0;

        public void TreeLayoutN(List<Node<T>> lNode, int level)
        {
            float prevAngle = 0;
            Node<T> parent, node, firstParent = null, prevParent = null;
            List<Node<T>> parentNodes = new List<Node<T>>();

            System.Collections.IEnumerator nitr = lNode.GetEnumerator();
            while (nitr.MoveNext())
            {

                parent = (Node<T>)nitr.Current;
                List<Node<T>> children = parent.Nodes;
                //List<Node<T>> children = Find(parent, x => x.Level == level);
                float rightLimit = parent.rightLimit();
                float angleSpace = (parent.leftLimit() - rightLimit) / children.Count;

                System.Collections.IEnumerator itr = children.GetEnumerator();
                for (float i = .5F; itr.MoveNext(); i++)
                {
                    node = (Node<T>)itr.Current;

                    //float centerAdjust = 0;
                    //if (parent.Parent != null)
                    //{
                    //    centerAdjust = (-parent.angleRange + parent.angleRange / children.Count) / 2;
                    //}
                    //node.angle = parent.angle + parent.angleRange / children.Count * i + centerAdjust;
                    //node.angleRange = parent.angleRange / children.Count;

                    node.angle = rightLimit + (i * angleSpace);

                    node.x = (float)(ROOTX + ((level * RADIUSX) * Math.Cos(node.angle)));
                    node.y = (float)(ROOTY + ((level * RADIUSY) * Math.Sin(node.angle)));

                    // Is it a parent node?
                    if (node.Nodes.Count > 0)
                    {
                        parentNodes.Add(node);

                        if (null == firstParent)
                        {
                            firstParent = node;
                        }

                        // right bisector limit
                        float prevGap = node.angle - prevAngle;
                        node.rightBisector = node.angle - (prevGap / 2.0F);
                        if (null != prevParent)
                        {
                            prevParent.leftBisector = node.rightBisector;
                        }

                        float arcAngle = level / (level + 1.0F);
                        float arc = 1.0F * (float)Math.Asin(arcAngle);

                        node.leftTangent = node.angle + arc;
                        node.rightTangent = node.angle - arc;

                        prevAngle = node.angle;
                        prevParent = node;
                    }

                }
            }


            if (null != firstParent)
            {
                float remaningAngle = TWO_PI - prevParent.angle;
                firstParent.rightBisector = (firstParent.angle - remaningAngle) / 2.0F;
                if (firstParent.rightBisector < 0)
                {
                    prevParent.leftBisector = firstParent.rightBisector + TWO_PI + TWO_PI;
                }
                else
                {
                    prevParent.leftBisector = firstParent.rightBisector + TWO_PI;
                }
            }

            if (parentNodes.Count > 0)
            {
                TreeLayoutN(parentNodes, level + 1);
            }

        }

        public void TreeLayout(Node<T> Node)
        {
            float RightBisLimit = 0;
            Node<T> NodeRoot = null;
            Node<T> FirstDirectory = null;
            List<Node<T>> parentNodes = new List<Node<T>>() { };

            for (int level = 0; level < Node.Depth; level++)
            {
                if (level == 0)
                {
                    List<Node<T>> l = Find(Node, x => x.Level == 0);
                    NodeRoot = l.FirstOrDefault();
                    NodeRoot.x = this.Width / 2;
                    NodeRoot.y = this.Height / 2;
                }
                else if (level == 1)
                {
                    List<Node<T>> l = Find(Node, x => x.Level == 1);
                    Node<T> CurrentNode = l.FirstOrDefault();
                    Node<T> FirstNode = l.FirstOrDefault();
                    Node<T> LastNode = l.LastOrDefault();
                    int NumNodes = l.Count;

                    float AngleSpace = (float)(TWO_PI / NumNodes);


                    bool FirstDirectoryFound = false; // if a directory has been found yet
                    float LastDirectoryAngle = 0; // angle of the last directory
                    float HadPreviousDirectory = 0; // if have had a previous directory
                    float DegsToPrevDirectory = 0; // initialize degrees to previous for the first directory
                    float DegsToNextDirectory = 0; // initialize degrees to next


                    // loop through all nodes at this level
                    foreach (var MyCurrentNode in l)
                    {
                        // compute x,y positions
                        MyCurrentNode.x = (float)(NodeRoot.x + ((d * level) * Math.Cos(AngleSpace * l.IndexOf(MyCurrentNode))));
                        MyCurrentNode.y = (float)(NodeRoot.y + ((d * level) * Math.Sin(AngleSpace * l.IndexOf(MyCurrentNode))));
                        MyCurrentNode.angle = AngleSpace * l.IndexOf(MyCurrentNode);

                        // Is it a parent node?
                        if (MyCurrentNode.Nodes.Count > 0)
                        {
                            parentNodes.Add(MyCurrentNode);
                            // if no first directory has been found yet, then one has been found, so set it to the first directory
                            if (!FirstDirectoryFound)
                            {
                                FirstDirectory = MyCurrentNode;
                                FirstDirectoryFound = true;
                            }

                            // get degrees to previous directory
                            DegsToPrevDirectory = MyCurrentNode.angle - LastDirectoryAngle;
                            // calculate right bisector limit
                            RightBisLimit = MyCurrentNode.angle - DegsToPrevDirectory / 2;
                            MyCurrentNode.rightBisector = RightBisLimit;

                        }
                    }




                }
                else if (level > 1)
                {
                    List<Node<T>> l = Find(Node, x => x.Level > 1);

                }
            }
        }
    }

    public class RadialTreePanelMySqlObjects : RadialTreePanel<UtilETWeb.frmDependsCustom.MySqlObjects>
    {
        protected override string GetText(Node<UtilETWeb.frmDependsCustom.MySqlObjects> Node)
        {
            return string.Format("{0}", Node.Level);
        }
    }
}
