using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using UtilETWeb.Data;

namespace UtilETWeb
{
    public partial class frmGenerateMenu : Form
    {
        public frmGenerateMenu()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmGenerateMenu_Load);
        }

        void frmGenerateMenu_Load(object sender, EventArgs e)
        {
            this.comboBox1.ValueMember = "key";
            this.comboBox1.DisplayMember = "value";
            this.comboBox1.DataSource = this.pselObjectType_Get();

        }

        public class keyvalue
        {
            public int key { get; set; }
            public String value { get; set; }
        }

        private List<keyvalue> pselObjectType_Get()
        {
            List<keyvalue> lkv = new List<keyvalue>() { };
            DataTable dt = new DataTable();
            List<SqlParameter> l = new List<SqlParameter>();
            l.Add(new SqlParameter("@vintCurUserID", -1));
            l.Add(new SqlParameter("@vintCurSysLID", "3082"));
            DataSet ds = Querys.ExecDataSet("pselObjectType_Get", l, @"Data Source=vxmtymxintdev01\intsqldev01;Initial Catalog=ETWebDEV114;Integrated Security=True");
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                foreach (DataRow item in dt.Rows)
                {
                    keyvalue kv = new keyvalue()
                    {
                        key = item.Field<int>("ObjTypeID"),
                        value = item.Field<String>("Name")
                    };
                    lkv.Add(kv);
                }

                keyvalue nullkv = new keyvalue()
                {
                    key = -99999,
                    value = "ALL"
                };
                lkv.Insert(0, nullkv);

                keyvalue nodekv = new keyvalue()
                {
                    key = 0,
                    value = "Node"
                };
                lkv.Add(nodekv);

            }
            return lkv;
        }

        private int? ObjTypeID
        {
            get
            {
                if ((int)this.comboBox1.SelectedValue == -99999)
                    return null;
                else
                    return (int)this.comboBox1.SelectedValue;
            }
        }

        private DataTable GenerateTree()
        {
            DataTable dt = new DataTable();
            List<SqlParameter> l = new List<SqlParameter>();
            l.Add(new SqlParameter("@CtTreeGUID", this.txtNode.Text));
            l.Add(new SqlParameter("@intObjTypeID", this.ObjTypeID));

            DataSet ds = Querys.ExecDataSet("sp_GenerateTree", l, @"Data Source=vxmtymxintdev01\intsqldev01;Initial Catalog=ETWebDEV114;Integrated Security=True");
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = GenerateTree();
            this.dataGridView1.DataSource = dt;
            foreach (DataGridViewColumn col in this.dataGridView1.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
        }
        /////////////////////////////////////////////////////////////////////
    }
}
