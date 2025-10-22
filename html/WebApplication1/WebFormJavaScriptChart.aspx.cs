using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace WebApplication1
{
    public partial class WebFormJavaScriptChart : System.Web.UI.Page
    {
        private const int NODE_WIDTH = 175;
        private const int NODE_HEIGHT = 20;
        private const int NODE_VERTICAL_SPACING = 10;
        private const int NODE_HORIZONTAL_SPACING = 40;

        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<string, object> _tree = new Dictionary<string, object>();

            if (!Page.IsPostBack)
            {
                // get parameter PosID
                int _PosID;
                if (!int.TryParse(Request["PosID"], out _PosID))
                    _PosID = -1;

                //Populate it here...
                DataSet ds = Generate(_PosID, ref  _tree);

                // offset height by MaxPosByLevel
                int MaxPosByLevel = 1;
                if (1 < ds.Tables.Count && ds.Tables[1].Rows.Count > 0)
                    MaxPosByLevel = ds.Tables[1].Rows[0].Field<int?>("MaxPosByLevel") ?? 1;

                int height = Math.Max(
                    NODE_HEIGHT + NODE_VERTICAL_SPACING + 10,
                    MaxPosByLevel * (NODE_HEIGHT + NODE_VERTICAL_SPACING) + 10);


                string myJsonString = (new JavaScriptSerializer()).Serialize(_tree);

                string myScriptValue = string.Format("var tree = {0};", myJsonString);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myScriptName", myScriptValue, true);


                this.myCanvas.Attributes["height"] = height.ToString();
            }
        }

        [WebMethod]
        //public static string BuildTree(int PosID)
        public static Dictionary<string, object> BuildTree(int PosID)
        {
            Dictionary<string, object> _tree = new Dictionary<string, object>();

            //Populate it here...
            DataSet ds = Generate(PosID, ref _tree);

            // offset height by MaxPosByLevel
            int MaxPosByLevel = 1;
            if (1 < ds.Tables.Count && ds.Tables[1].Rows.Count > 0)
                MaxPosByLevel = ds.Tables[1].Rows[0].Field<int?>("MaxPosByLevel") ?? 1;

            int height = Math.Max(
                NODE_HEIGHT + NODE_VERTICAL_SPACING + 10,
                MaxPosByLevel * (NODE_HEIGHT + NODE_VERTICAL_SPACING) + 10);

            _tree.Add("height", height);

            return _tree;

            //return (new JavaScriptSerializer()).Serialize(_tree);
        }

        public static DataSet Generate(int PosID, ref Dictionary<string, object> _tree)
        {
            DataSet ds = GetData(PosID);
            if (0 < ds.Tables.Count && ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] rows = ds.Tables[0].Select("level = 0");
                if (rows.Length > 0)
                {

                    _tree = new Dictionary<string, object>();
                    _tree.Add("text", rows[0].Field<string>("Title"));
                    _tree.Add("nodes", GenerateTreeNodes(ds.Tables[0], ref  _tree, rows[0].Field<int?>("SlotID")));
                }
            }
            return ds;
        }

        public static List<Dictionary<string, object>> GenerateTreeNodes(DataTable dt, ref Dictionary<string, object> tree, int? SlotID)
        {
            List<Dictionary<string, object>> array = new List<Dictionary<string, object>>();

            DataRow[] rows = dt.Select(string.Format("ParentSlotID = {0}", SlotID ?? 0));
            foreach (DataRow item in rows)
            {
                Dictionary<string, object> tree_node = new Dictionary<string, object>();
                tree_node.Add("text", item.Field<string>("Title"));
                tree_node.Add("nodes", GenerateTreeNodes(dt, ref  tree_node, item.Field<int?>("SlotID")));
                array.Add(tree_node);
            }

            return array;
        }


        public static DataSet GetData(int PosID)
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
