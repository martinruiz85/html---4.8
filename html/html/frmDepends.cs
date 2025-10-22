using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilETWeb.Data;
using Microsoft.Data.SqlClient;
using System.IO;

namespace UtilETWeb
{
    public partial class frmDepends : Form
    {
        public frmDepends()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmDepends_Load);
        }

        public struct Conn
        {
            public string name { get; set; }
            public string value { get; set; }
        }

        void frmDepends_Load(object sender, EventArgs e)
        {
            List<Conn> l = new List<Conn>() { };
            l.Add(new Conn() { name = "PRO", value = @"Data Source=XMTYMXINT01\SQLAPPS;Initial Catalog=ETWebDEV103;Integrated Security=True" });
            l.Add(new Conn() { name = "DEV", value = @"Data Source=vxmtymxintdev01\intsqldev01;Initial Catalog=ETWebDEV114;Integrated Security=True" });
            this.comboBox1.DisplayMember = "name";
            this.comboBox1.ValueMember = "Value";
            this.comboBox1.DataSource = l;

            this.contextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip1_ItemClicked);
        }

        void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            if (e.ClickedItem.Text == "TreeView")
                GenerateTreeview();
            else if (e.ClickedItem.Text == "TreeText")
                GenerateTreeText();
            else if (e.ClickedItem.Text == "ListText")
                GenerateListText();

        }


        string ObjectName
        {
            get
            {
                return this.txtObjectName.Text.Trim();
            }
        }

        string FileName
        {
            get
            {
                return Path.Combine(Environment.CurrentDirectory, string.Format("{0}.txt", this.ObjectName));
            }
        }

        public class MySqlObjects
        {
            public string name { get; set; }
        }

        private void FillTree()
        {

        }

        ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();

        private List<string> ValidRepit = new List<string>() { };

        private void GenerateListText()
        {
            this.ValidRepit.Clear();
            File.WriteAllText(this.FileName, String.Empty);
            Node<MySqlObjects> root = new Node<MySqlObjects>(new MySqlObjects() { name = this.ObjectName }) { Name = this.ObjectName };
            this.GenerateTree(root, this.ObjectName, "stored procedure");
            this.treeView1.Nodes.Clear();
            root.PrintList(this.FileName);
            if (File.Exists(FileName)) System.Diagnostics.Process.Start(FileName);
        }

        private void GenerateTreeText()
        {
            this.ValidRepit.Clear();
            File.WriteAllText(this.FileName, String.Empty);
            Node<MySqlObjects> root = new Node<MySqlObjects>(new MySqlObjects() { name = this.ObjectName }) { Name = this.ObjectName };
            this.GenerateTree(root, this.ObjectName, "stored procedure");
            this.treeView1.Nodes.Clear();
            root.PrintNode("", this.FileName, UtilETWeb.frmDependsCustom.EnumDifference.Dev);
            if (File.Exists(FileName)) System.Diagnostics.Process.Start(FileName);
        }

        private void GenerateTreeview()
        {
            int count = 0;
            this.ValidRepit.Clear();
            Node<MySqlObjects> root = new Node<MySqlObjects>(new MySqlObjects() { name = this.ObjectName }) { Name = this.ObjectName };
            this.GenerateTree(root, this.ObjectName, "stored procedure");
            this.treeView1.Nodes.Clear();
            root.PrintNodeTreeView(null, this.treeView1, UtilETWeb.frmDependsCustom.EnumDifference.Dev,ref count);
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Items.Clear();
            contextMenuStrip1.Items.Add("TreeView");
            contextMenuStrip1.Items.Add("TreeText");
            contextMenuStrip1.Items.Add("ListText");
            contextMenuStrip1.Show(btnGenerate, new Point(0, btnGenerate.Height));
        }

        private void RefreshSqlModule(string ObjectName)
        {
            List<SqlParameter> l = new List<SqlParameter>();
            l.Add(new SqlParameter("@name", ObjectName));
            Querys.ExecNonQuery("sp_refreshsqlmodule", l, this.comboBox1.SelectedValue.ToString());
        }

        private void GenerateTree(Node<MySqlObjects> root, string ObjectName, string type)
        {
            if ("stored procedure|scalar function|view".Split("|".ToCharArray()).Contains(type))
                this.RefreshSqlModule(ObjectName);

            List<SqlParameter> l = new List<SqlParameter>();
            l.Add(new SqlParameter("@objname", ObjectName));
            DataTable dt = Querys.ExecDatatable("sp_depends", l, this.comboBox1.SelectedValue.ToString());
            if (dt.Rows.Count > 0)
            {
                List<string> list = new List<string>() { };
                switch (type)
                {
                    case "stored procedure":
                    case "scalar function":
                        list = dt.Rows.OfType<DataRow>()
                            .Where(row => "stored procedure|scalar function|user table|view".Split("|".ToCharArray()).Contains(row.Field<string>("type")))
                            .GroupBy(row => new { name = row.Field<string>("name"), type = row.Field<string>("type") })
                            .OrderBy(col => col.Key.name)
                            .Select(g => string.Format("{0}|{1}", g.Key.name, g.Key.type)).ToList();
                        break;
                    case "view":
                        list = dt.Rows.OfType<DataRow>()
                            .Where(row => "user table|view".Split("|".ToCharArray()).Contains(row.Field<string>("type")))
                            .GroupBy(row => new { name = row.Field<string>("name"), type = row.Field<string>("type") })
                            .OrderBy(col => col.Key.name)
                            .Select(g => string.Format("{0}|{1}", g.Key.name, g.Key.type)).ToList();
                        break;
                    case "user table":
                        list = dt.Rows.OfType<DataRow>()
                           .Where(row => "trigger".Split("|".ToCharArray()).Contains(row.Field<string>("type")))
                           .GroupBy(row => new { name = row.Field<string>("name"), type = row.Field<string>("type") })
                           .OrderBy(col => col.Key.name)
                           .Select(g => string.Format("{0}|{1}", g.Key.name, g.Key.type)).ToList();
                        break;
                    default:
                        break;
                }

                foreach (string item in list)
                {
                    if (!item.StartsWith("dbo.tz") && !item.StartsWith("dbo.vz") && !item.StartsWith("dbo.pz") && !this.ValidRepit.Contains(item))
                    {
                        this.ValidRepit.Add(item);
                        Node<MySqlObjects> CurrentNode = new Node<MySqlObjects>(new MySqlObjects() { name = item }) { Name = item };
                        root.Nodes.Add(CurrentNode);
                        this.GenerateTree(CurrentNode, item.Split("|".ToCharArray())[0], item.Split("|".ToCharArray())[1]);
                    }
                }
            }
        }
    }
}
