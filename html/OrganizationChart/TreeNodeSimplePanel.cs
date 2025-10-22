using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Drawing;

namespace OrganizationChart
{
    public class TreeNodeSimplePanel : Control
    {
        private const int NODE_WIDTH = 175;
        private const int NODE_HEIGHT = 20;
        private const int NODE_VERTICAL_SPACING = 20;
        private const int NODE_HORIZONTAL_SPACING = 20;
        private const int NODE_ROWS_BY_COLUMN = 2;
        private const int NODE_COLUMN_BY_ROWS = 4; // solo numeros pares

        public int PosID { get; set; }

        private TreeNodePosition TreeNode;


        public TreeNodeSimplePanel()
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.Clear(Color.WhiteSmoke);

            // draw tree
            if (TreeNode != null)
            {
                float x = 10;
                float y = 10;
                int level = 0;
                PostOrdenByColumn<TreePositionData>(TreeNode, ref x, ref y, ref level);
                TreePaintLines<TreePositionData>(TreeNode, g);
                TreePaint<TreePositionData>(TreeNode, g);
            }
        }

        [Obsolete("PostOrdenByRow is deprecated, please use PostOrdenByColumn instead.")]
        public void PostOrdenByRow<T>(TreeNode<T> t, ref float x, ref float y, ref int level)
        {
            t.Level = level;
            if (t.TreeNodeItem.Count > 0)
            {
                level += 1;
            }

            foreach (TreeNode<T> Item in t.TreeNodeItem)
            {
                // Recorre en postorden el hijo i-esimo
                PostOrdenByRow(Item, ref x, ref y, ref level);
            }

            // Imprime la llave i-esima
            Console.WriteLine(string.Format("{0}", t.Data));

            if (t.TreeNodeItem.Count > 0)
            {
                level -= 1;
            }

            float new_y = y + level * (t.NodeHeight + t.NodeVerticalSpacing);

            if (t.Parent != null && t.Parent.TreeNodeItem.IndexOf(t) % NODE_ROWS_BY_COLUMN > 0)
                new_y = y + level * (t.NodeHeight + t.NodeVerticalSpacing) + (t.Parent.TreeNodeItem.IndexOf(t) % NODE_ROWS_BY_COLUMN) * (t.NodeHeight + t.NodeVerticalSpacing);

            if (t.TreeNodeItem.Count > 0)
            {
                float new_x = t.TreeNodeItem.FirstOrDefault().Point.X + ((t.TreeNodeItem.LastOrDefault().Point.X - t.TreeNodeItem.FirstOrDefault().Point.X) / 2F);
                t.Point = new PointF(new_x, new_y);
            }
            else
                t.Point = new PointF(x, new_y);

            // 2 rows
            if (t.Parent != null && (t.Parent.TreeNodeItem.IndexOf(t) + 1) % NODE_ROWS_BY_COLUMN == 0)
                x += t.NodeWidth + t.NodeHorizontalSpacing;
        }

        public void PostOrdenByColumn<T>(TreeNode<T> t, ref float x, ref float y, ref int level)
        {
            t.Level = level;
            if (t.TreeNodeItem.Count > 0)
            {
                level += 1;
            }

            foreach (TreeNode<T> Item in t.TreeNodeItem)
            {
                // Recorre en postorden el hijo i-esimo
                PostOrdenByColumn(Item, ref x, ref y, ref level);
            }

            // Imprime la llave i-esima
            Console.WriteLine(string.Format("{0}", t.Data));

            if (t.TreeNodeItem.Count > 0)
            {
                level -= 1;
            }

            float new_y = y + level * (t.NodeHeight + t.NodeVerticalSpacing);

            if (t.Parent != null && t.Parent.TreeNodeItem.IndexOf(t) % Math.Ceiling(t.Parent.TreeNodeItem.Count / (float)NODE_COLUMN_BY_ROWS) > 0)
                new_y = y + level * (t.NodeHeight + t.NodeVerticalSpacing) + (t.Parent.TreeNodeItem.IndexOf(t) % (int)Math.Ceiling(t.Parent.TreeNodeItem.Count / (float)NODE_COLUMN_BY_ROWS)) * (t.NodeHeight + t.NodeVerticalSpacing);

            if (t.TreeNodeItem.Count > 0)
            {
                float new_x = t.TreeNodeItem.FirstOrDefault().Point.X + ((t.TreeNodeItem.LastOrDefault().Point.X - t.TreeNodeItem.FirstOrDefault().Point.X) / 2F);
                t.Point = new PointF(new_x, new_y);
            }
            else
                t.Point = new PointF(x, new_y);

            // 2 cols
            if (t.Parent != null && (t.Parent.TreeNodeItem.IndexOf(t) + 1) % Math.Ceiling(t.Parent.TreeNodeItem.Count / (float)NODE_COLUMN_BY_ROWS) == 0)
                x += t.NodeWidth + t.NodeHorizontalSpacing;
        }

        void TreePaint<T>(TreeNode<T> tree, Graphics g)
        {
            tree.DrawRectangle(g);
            foreach (TreeNode<T> Item in tree.TreeNodeItem)
            {
                TreePaint<T>(Item, g);
            }
        }

        void TreePaintLines<T>(TreeNode<T> tree, Graphics g)
        {
            foreach (TreeNode<T> Item in tree.TreeNodeItem)
            {
                //g.DrawLine(Pens.Black,
                //    //parent
                //    new PointF(tree.Point.X + (tree.NodeWidth / 2), tree.Point.Y),
                //    //parent
                //    new PointF(Item.Point.X + (Item.NodeWidth / 2), Item.Point.Y));

                // Draw line to screen.
                g.DrawLine(Pens.Black,
                    //parent
                    new PointF(tree.Point.X + (tree.NodeWidth / 2), tree.Point.Y + tree.NodeHeight),
                    //parent
                    new PointF(tree.Point.X + (tree.NodeWidth / 2), tree.Point.Y + (tree.NodeHeight + tree.NodeVerticalSpacing / 2)));

                g.DrawLine(Pens.Black,
                    //parent 
                    new PointF(Item.Point.X + (Item.NodeWidth / 2), Item.Point.Y),
                    //child
                    new PointF(Item.Point.X + (Item.NodeWidth / 2), Item.Point.Y - Item.NodeVerticalSpacing / 2));

                g.DrawLine(Pens.Black,
                    //parent 
                    new PointF(Item.Point.X + (Item.NodeWidth / 2), Item.Point.Y - Item.NodeVerticalSpacing / 2),
                    //child
                    new PointF(tree.Point.X + (tree.NodeWidth / 2), Item.Point.Y - Item.NodeVerticalSpacing / 2));

                g.DrawLine(Pens.Black,
                    //parent 
                new PointF(tree.Point.X + (tree.NodeWidth / 2), Item.Point.Y - Item.NodeVerticalSpacing / 2),
                    //child
                new PointF(tree.Point.X + (tree.NodeWidth / 2), tree.Point.Y + (tree.NodeHeight + tree.NodeVerticalSpacing / 2)));

                TreePaintLines<T>(Item, g);
            }
        }

        void TreePaintLinesNotPair<T>(TreeNode<T> tree, Graphics g)
        {
            foreach (TreeNode<T> Item in tree.TreeNodeItem)
            {
                //g.DrawLine(Pens.Black,
                //    //parent
                //    new PointF(tree.Point.X + (tree.NodeWidth / 2), tree.Point.Y),
                //    //parent
                //    new PointF(Item.Point.X + (Item.NodeWidth / 2), Item.Point.Y));

                // Draw line to screen.
                g.DrawLine(Pens.Black,
                    //parent
                    new PointF(tree.Point.X + (tree.NodeWidth / 2), tree.Point.Y + tree.NodeHeight),
                    //parent
                    new PointF(tree.Point.X + (tree.NodeWidth / 2), tree.Point.Y + (tree.NodeHeight + tree.NodeVerticalSpacing / 2)));

                g.DrawLine(Pens.Black,
                    //parent 
                    new PointF(Item.Point.X + Item.NodeWidth, Item.Point.Y + (Item.NodeHeight / 2)),
                    //child
                    new PointF(Item.Point.X + (Item.NodeWidth + Item.NodeHorizontalSpacing / 2), Item.Point.Y + (Item.NodeHeight / 2)));

                g.DrawLine(Pens.Black,
                    //parent 
                   new PointF(Item.Point.X + (Item.NodeWidth + Item.NodeHorizontalSpacing / 2), Item.Point.Y + (Item.NodeHeight / 2)),
                    //child
                   new PointF(Item.Point.X + (Item.NodeWidth + Item.NodeHorizontalSpacing / 2), tree.Point.Y + (tree.NodeHeight + tree.NodeVerticalSpacing / 2)));

                g.DrawLine(Pens.Black,
                    //parent 
                  new PointF(Item.Point.X + (Item.NodeWidth + Item.NodeHorizontalSpacing / 2), tree.Point.Y + (tree.NodeHeight + tree.NodeVerticalSpacing / 2)),
                    //child
                                      new PointF(tree.Point.X + (tree.NodeWidth / 2), tree.Point.Y + (tree.NodeHeight + tree.NodeVerticalSpacing / 2)));




                TreePaintLinesNotPair<T>(Item, g);
            }
        }



        public DataSet Generate(int PosID)
        {
            DataSet ds = GetData(PosID);
            if (0 < ds.Tables.Count && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] rows = ds.Tables[0].Select("level = 0");
                if (rows.Length > 0)
                {
                    TreePositionData Data = new TreePositionData();
                    Data.Title = rows[0].Field<string>("Title");
                    TreeNode = new TreeNodePosition(Data);
                    TreeNode.Level = rows[0].Field<int>("Level");
                    TreeNode.NodeWidth = NODE_WIDTH;
                    TreeNode.NodeHeight = NODE_HEIGHT;
                    TreeNode.NodeVerticalSpacing = NODE_VERTICAL_SPACING;
                    TreeNode.NodeHorizontalSpacing = NODE_HORIZONTAL_SPACING;

                    GenerateTreeNodes(ds.Tables[0], TreeNode, rows[0].Field<int?>("SlotID"));
                }
            }
            return ds;
        }

        public void GenerateTreeNodes(DataTable dt, TreeNodePosition tree, int? SlotID)
        {
            DataRow[] rows = dt.Select(string.Format("ParentSlotID = {0}", SlotID ?? 0));
            foreach (DataRow item in rows)
            {
                TreePositionData Data = new TreePositionData();
                Data.Title = item.Field<string>("Title");
                TreeNodePosition newTreeNode = new TreeNodePosition(Data);
                newTreeNode.Level = item.Field<int>("Level");
                newTreeNode.NodeWidth = NODE_WIDTH;
                newTreeNode.NodeHeight = NODE_HEIGHT;
                newTreeNode.NodeVerticalSpacing = NODE_VERTICAL_SPACING;
                newTreeNode.NodeHorizontalSpacing = NODE_HORIZONTAL_SPACING;
                newTreeNode.Parent = tree;

                if (newTreeNode.Level == 1)
                    newTreeNode.NodeColor = Color.FromArgb(255, 255, 255, 178);

                GenerateTreeNodes(dt, newTreeNode, item.Field<int?>("SlotID"));
                tree.TreeNodeItem.Add(newTreeNode);
            }
        }

        public DataSet GetData(int PosID)
        {
            List<SqlParameter> l = new List<SqlParameter>();
            l.Add(new SqlParameter("@vintUserID", -1));
            l.Add(new SqlParameter("@vintSysLID", 3082));
            l.Add(new SqlParameter("@vintPosID", PosID));

            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection c = new SqlConnection(ConfigurationManager.ConnectionStrings["ETWebDev"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("dbo.pfrmXigPosRelationTree_List", c) { CommandType = CommandType.StoredProcedure })
                    {
                        c.Open();
                        cmd.Parameters.AddRange(l.ToArray());
                        cmd.CommandTimeout = 600;
                        using (SqlDataAdapter da = new SqlDataAdapter() { SelectCommand = cmd })
                        {
                            da.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
