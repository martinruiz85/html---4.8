<%@ WebHandler Language="C#" Class="OrgColumnChartImgExporter" %>

using System;
using System.Web;
using System.Linq;
using System.Drawing;
using System.IO;
using WebApplication1.App_Code;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;


/// <summary>
/// Exports HTML OrgCharts to PDF.
/// </summary>
public class OrgColumnChartImgExporter : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    private const int NODE_WIDTH = 140;
    private const int NODE_HEIGHT = 20;
    private const int NODE_VERTICAL_SPACING = 20;
    private const int NODE_HORIZONTAL_SPACING = 20;
    private const int NODE_COLUMN_BY_ROWS = 4; // SOLO NUMEROS PARES

    /// <summary>
    /// Processes the request.
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "image/png";

        // get parameter PosID
        int _PosID;
        if (!int.TryParse(context.Request["PosID"], out _PosID))
            _PosID = -1;

        // generate tree
        DataSet ds = Generate(_PosID);

        // offset height by MaxPosByLevel
        int rows = 0;
        if (0 < ds.Tables.Count && ds.Tables[0].Rows.Count > 0)
        {
            rows = ds.Tables[0].Select("level=2").Count();
        }


        int height = Math.Max(
            (2) * (NODE_HEIGHT + NODE_VERTICAL_SPACING) + 10,
            (2 + (int)Math.Ceiling((double)rows / NODE_COLUMN_BY_ROWS)) * (NODE_HEIGHT + NODE_VERTICAL_SPACING) + 10);

        int width = (NODE_WIDTH + NODE_HORIZONTAL_SPACING) * Math.Max(Math.Min(rows, NODE_COLUMN_BY_ROWS), 1) + 10;

        // create grapichs
        Bitmap bmp = new Bitmap(width, height);

        Graphics g = Graphics.FromImage(bmp);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        GraphicsUnit gu = GraphicsUnit.Pixel;
        //g.FillRectangle(Brushes.White, Rectangle.Round(bmp.GetBounds(ref gu)));        
        g.FillRectangle(Brushes.WhiteSmoke, Rectangle.Round(bmp.GetBounds(ref gu)));

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

        // Make the default transparent color transparent for myBitmap.
        //bmp.MakeTransparent();
        g.Dispose();

        // Save    
        //bmp.Save(context.Response.OutputStream, ImageFormat.Png);

        // output client
        using (Bitmap image = bmp)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.WriteTo(context.Response.OutputStream);
            }
        }

    }

    private TreeNodePosition TreeNode;


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

        //if (t.Parent != null && t.Parent.TreeNodeItem.IndexOf(t) % Math.Ceiling(t.Parent.TreeNodeItem.Count / (float)NODE_COLUMN_BY_ROWS) == 0)
        if (t.Parent != null)
            new_y = y + level * (t.NodeHeight + t.NodeVerticalSpacing) + (t.Parent.TreeNodeItem.IndexOf(t) / NODE_COLUMN_BY_ROWS) * (t.NodeHeight + t.NodeVerticalSpacing);

        if (t.TreeNodeItem.Count > 0)
        {
            float new_x = t.TreeNodeItem.FirstOrDefault().Point.X + ((t.TreeNodeItem.Max(c=> c.Point.X) - t.TreeNodeItem.FirstOrDefault().Point.X) / 2F);
            t.Point = new PointF(new_x, new_y);
        }
        else
        {

            x = t.NodeWidth + t.NodeHorizontalSpacing;
            
            // 2 cols
            //if (t.Parent != null && (t.Parent.TreeNodeItem.IndexOf(t) + 1) % Math.Ceiling(t.Parent.TreeNodeItem.Count / (float)NODE_COLUMN_BY_ROWS) == 0)
            if (t.Parent != null)
                x = (t.NodeWidth + t.NodeHorizontalSpacing) * ((t.Parent.TreeNodeItem.IndexOf(t)) % NODE_COLUMN_BY_ROWS) + 10;

            t.Point = new PointF(x, new_y);
        }
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

    /// <summary>
    /// To be sure: Don't reuse this handler.
    /// </summary>
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}