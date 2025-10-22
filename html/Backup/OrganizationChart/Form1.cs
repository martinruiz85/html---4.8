using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;


namespace OrganizationChart
{
    public partial class Form1 : Form
    {
        private Timer tmr = new Timer();

        private const int NODE_WIDTH = 175;
        private const int NODE_HEIGHT = 20;
        private const int NODE_VERTICAL_SPACING = 10;
        private const int NODE_HORIZONTAL_SPACING = 40;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;

            this.Load += new EventHandler(Form1_Load);
            this.Paint += new PaintEventHandler(Form1_Paint);

            this.tmr.Interval = 100;
            this.tmr.Tick += new EventHandler(tmr_Tick);
            //this.tmr.Start();
        }

        void tmr_Tick(object sender, EventArgs e)
        {
            this.Refresh();

        }

        void Form1_Load(object sender, EventArgs e)
        {
            // get parameter PosID
            int _PosID;
            if (!int.TryParse(this.textBox1.Text, out _PosID))
                _PosID = -1;

            // generate tree
            DataSet ds = Generate(_PosID);

        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            GraphicsUnit gu = GraphicsUnit.Pixel;
            g.Clear(Color.WhiteSmoke);

            // draw tree
            if (TreeNode != null)
            {
                float x = 10;
                float y = 10;
                int level = 0;
                PostOrden<TreePositionData>(TreeNode, ref x, ref y, ref level);
                TreePaint<TreePositionData>(TreeNode, g);
            }

        }

        private TreeNodePosition TreeNode;


        public void PostOrden<T>(TreeNode<T> t, ref float x, ref float y, ref int level)
        {
            t.Level = level;
            if (t.TreeNodeItem.Count > 0)
            {
                level += 1;
            }

            foreach (TreeNode<T> Item in t.TreeNodeItem)
            {
                // Recorre en postorden el hijo i-esimo
                PostOrden(Item, ref x, ref y, ref level);
            }

            // Imprime la llave i-esima
            Console.WriteLine(string.Format("{0}", t.Data));

            if (t.TreeNodeItem.Count > 0)
            {
                level -= 1;
            }

            float newx = x + level * (t.NodeWidth + t.NodeHorizontalSpacing);

            if (t.TreeNodeItem.Count > 0)
            {
                float newy = t.TreeNodeItem.FirstOrDefault().Point.Y + ((t.TreeNodeItem.LastOrDefault().Point.Y - t.TreeNodeItem.FirstOrDefault().Point.Y) / 2F);
                t.Point = new PointF(newx, newy);
            }
            else
                t.Point = new PointF(newx, y);

            y += t.NodeHeight + t.NodeVerticalSpacing;
        }

        void TreePaint<T>(TreeNode<T> tree, Graphics g)
        {
            tree.DrawRectangle(g);
            foreach (TreeNode<T> Item in tree.TreeNodeItem)
            {
                // Draw line to screen.
                g.DrawLine(Pens.Black,
                    //parent
                    new PointF(tree.Point.X + tree.NodeWidth, tree.Point.Y + (tree.NodeHeight / 2)),
                    //parent
                    new PointF(tree.Point.X + (tree.NodeWidth + tree.NodeHorizontalSpacing / 2), tree.Point.Y + (tree.NodeHeight / 2)));

                g.DrawLine(Pens.Black,
                    //parent 
                    new PointF(tree.Point.X + (tree.NodeWidth + tree.NodeHorizontalSpacing / 2), tree.Point.Y + (tree.NodeHeight / 2)),
                    //child
                    new PointF(Item.Point.X - tree.NodeHorizontalSpacing / 2, Item.Point.Y + tree.NodeHeight / 2));

                g.DrawLine(Pens.Black,
                    //child
                    new PointF(Item.Point.X, Item.Point.Y + tree.NodeHeight / 2),
                    //child
                    new PointF(Item.Point.X - tree.NodeHorizontalSpacing / 2, Item.Point.Y + tree.NodeHeight / 2));

                TreePaint<T>(Item, g);
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
                using (SqlConnection c = new SqlConnection(ConfigurationManager.ConnectionStrings["ETWeb"].ConnectionString))
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // get parameter PosID
            int _PosID;
            if (!int.TryParse(this.textBox1.Text, out _PosID))
                _PosID = -1;

            // generate tree
            DataSet ds = Generate(_PosID);

            this.Refresh();
        }

    }
}
