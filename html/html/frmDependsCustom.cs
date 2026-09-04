using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using html;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;
using UtilETWeb.Data;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

//http://www.worldbestlearningcenter.com/index_files/csharp-draw-lines.htm
namespace UtilETWeb
{
    public partial class frmDependsCustom : Form
    {
        public frmDependsCustom()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Load += new EventHandler(frmDepends_Load);
        }

        public static void Empty(string directory)
        {
            foreach (string fileToDelete in System.IO.Directory.GetFiles(directory))
            {
                System.IO.File.Delete(fileToDelete);
            }
            foreach (string subDirectoryToDeleteToDelete in System.IO.Directory.GetDirectories(directory))
            {
                System.IO.Directory.Delete(subDirectoryToDeleteToDelete, true);
            }
        }

        private List<UtilETWeb.MyConfigSection.MyConfigInstanceElement> GetConnexions()
        {

            List<UtilETWeb.MyConfigSection.MyConfigInstanceElement> l = new List<MyConfigSection.MyConfigInstanceElement>();
            MyConfigSection config = ConfigurationManager.GetSection("MyConnection") as MyConfigSection;
            l = config.Instances.OfType<UtilETWeb.MyConfigSection.MyConfigInstanceElement>().OrderBy(c => c.Name).ToList();

            UtilETWeb.MyConfigSection.MyConfigInstanceElement item = new UtilETWeb.MyConfigSection.MyConfigInstanceElement();
            item.Name = "(sin especificar)";
            item.Code = "-1";
            l.Insert(0, item);

            return l;
        }


        void frmDepends_Load(object sender, EventArgs e)
        {
            this.Paint += new PaintEventHandler(frmDependsCustom_Paint);

            this.cmbDatabase.DataSource = GetConnexions();
            this.cmbDatabase.ValueMember = "Code";
            this.cmbDatabase.DisplayMember = "Name";
            this.cmbDatabase.SelectedValue = "-1";
            this.cmbDatabase.SelectedValue = this.ConnectionStringPRO;
            this.cmbDatabase.DropDownStyle = ComboBoxStyle.DropDown;


            this.cmbCompareDatabase.DataSource = GetConnexions();
            this.cmbCompareDatabase.ValueMember = "Code";
            this.cmbCompareDatabase.DisplayMember = "Name";
            this.cmbCompareDatabase.SelectedValue = "-1";
            this.cmbCompareDatabase.SelectedValue = this.ConnectionStringDEV;
            this.cmbCompareDatabase.DropDownStyle = ComboBoxStyle.DropDown;


            List<EnumModel> enums = ((IEnumerable<EnumObjectType>)Enum
                    .GetValues(typeof(EnumObjectType)))
                    .OrderByDescending(c => (int)c)
                    .Select(c => new EnumModel()
                    {
                        Value = (int)c,
                        Name = c.GetDescription()
                    }).ToList();

            this.ddlType.DisplayMember = "Name";
            this.ddlType.ValueMember = "Value";
            this.ddlType.DataSource = enums;

            this.ddlType.DisplayMember = "Name";
            this.ddlType.ValueMember = "Value";
            this.ddlType.DataSource = enums;


            List<EnumModel> enumsDifference = ((IEnumerable<EnumDifference>)Enum
                .GetValues(typeof(EnumDifference)))
                .OrderBy(c => (int)c)
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();

            this.cmbDifference.DisplayMember = "Name";
            this.cmbDifference.ValueMember = "Value";
            this.cmbDifference.DataSource = enumsDifference;
            this.cmbDifference.DrawItem += new DrawItemEventHandler(cmbDifference_DrawItem);

            //this.cmbDifference.MeasureItem += new MeasureItemEventHandler(cmbDifference_MeasureItem);

            this.backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            this.contextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip1_ItemClicked);
            this.contextMenuStripTreeView.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStripTreeView_ItemClicked);

            this.txtSearch.Invalidate();
            this.txtSearch.Update();

            this.treeView1.ImageList = new ImageList();
            this.treeView1.ImageList.Images.Add("question", Properties.Resources.question);
            this.treeView1.ImageList.Images.Add("user table", Properties.Resources.table);
            this.treeView1.ImageList.Images.Add("param", Properties.Resources.at_sign);
            this.treeView1.ImageList.Images.Add("trigger", Properties.Resources.lightning);
            this.treeView1.ImageList.Images.Add("column", Properties.Resources.table_insert_column);
            this.treeView1.ImageList.Images.Add("scalar function", Properties.Resources.function);
            this.treeView1.ImageList.Images.Add("stored procedure", Properties.Resources.script);
            this.treeView1.ImageList.Images.Add("view", Properties.Resources.table_select_all);
            this.treeView1.ImageList.Images.Add("star", Properties.Resources.star);
            this.treeView1.ImageList.Images.Add("arrow", Properties.Resources.arrow);

            this.AcceptButton = this.btnGenerate;
            this.panel1.Paint += new PaintEventHandler(panel1_Paint);


        }

        void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(this.BackColor);
            g.FillEllipse(new SolidBrush(panel1.BackColor), new Rectangle(0, 0, this.panel1.Width - 2, this.panel1.Height - 2));
            Pen p = new Pen(Brushes.Black);
            p.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
            g.DrawEllipse(p, new Rectangle(0, 0, this.panel1.Width - 2, this.panel1.Height - 2));
        }

        void cmbDifference_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            switch (e.Index)
            {
                case 0:
                    e.ItemHeight = 25;
                    break;
                case 1:
                    e.ItemHeight = 25;
                    break;
                case 2:
                    e.ItemHeight = 25;
                    break;
            }
            e.ItemWidth = 260;
        }

        void frmDependsCustom_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rec = new Rectangle(0, 0, 100, 100);
            //g.DrawRectangle(Pens.Black, rec);
            //g.FillRectangle(Brushes.Black, rec);
            //g.DrawRoundedRectangle(rec, 2, Pens.Black);


        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            //Graphics g = e.Graphics;
            //Rectangle rec = new Rectangle(this.txtSearch.Location.X, this.txtSearch.Location.Y, 100, 100);
            //g.DrawRectangle(Pens.Black, rec);
            //g.FillRectangle(Brushes.Black, rec);
        }


        void cmbDifference_DrawItem(object sender, DrawItemEventArgs e)
        {
            var text = ((EnumModel)((ComboBox)sender).Items[e.Index]).Name;

            float size = 0;
            System.Drawing.Font myFont;
            FontFamily family = null;

            System.Drawing.Color animalColor = new System.Drawing.Color();
            switch (e.Index)
            {
                case 0:
                    size = 30;
                    animalColor = System.Drawing.Color.Yellow;
                    family = FontFamily.GenericSansSerif;
                    break;
                case 1:
                    size = 10;
                    animalColor = System.Drawing.Color.YellowGreen;
                    family = FontFamily.GenericMonospace;
                    break;
                case 2:
                    size = 15;
                    animalColor = System.Drawing.Color.Orange;
                    family = FontFamily.GenericSansSerif;
                    break;
            }

            // Draw the background of the item.
            e.DrawBackground();

            // Create a square filled with the animals color. Vary the size
            // of the rectangle based on the length of the animals name.
            Rectangle rectangle = new Rectangle(2, e.Bounds.Top + 2,
                    e.Bounds.Height, e.Bounds.Height - 4);
            e.Graphics.FillRectangle(new SolidBrush(animalColor), rectangle);

            // Draw each string in the array, using a different size, color,
            // and font for each item.
            myFont = new Font(family, size, FontStyle.Bold);
            //Control.DefaultFont              
            e.Graphics.DrawString(string.Format(" {0}", text), ComboBox.DefaultFont, System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X + rectangle.Width, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));

            // Draw the focus rectangle if the mouse hovers over an item.
            e.DrawFocusRectangle();
        }


        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int CountDiff = 0;
            GenTreeView Gen = e.Result as GenTreeView;
            this.treeView1.Nodes.Clear();
            Gen.RootNode.PrintNodeTreeView(null, this.treeView1, (EnumDifference)this.cmbDifference.SelectedValue, ref CountDiff);
            this.RootNode = Gen.RootNode;
            this.btnGenerate.Image = UtilETWeb.Properties.Resources.work;
            this.lblNoDiferences.Text = CountDiff.ToString();
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            GenTreeView Gen = e.Argument as GenTreeView;
            this.GenerateTree(Gen.RootNode, Gen.ObjectName, Gen.Type);
            e.Result = Gen;
        }

        void contextMenuStripTreeView_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "GenerateScript")
            {
                string ObjectName = contextMenuStripTreeView.Tag.ToString().Split('|')[1];
                string ObjectType = contextMenuStripTreeView.Tag.ToString().Split('|')[0];
                switch (ObjectType)
                {
                    case "stored procedure":
                        GenerateScriptSP(ObjectName);
                        break;
                    case "scalar function":
                        GenerateScriptFunction(ObjectName);
                        break;
                    case "view":
                        GenerateScriptView(ObjectName);
                        break;
                    case "trigger":
                        GenerateScriptTrigger(ObjectName, contextMenuStripTreeView.Tag.ToString());
                        break;
                    case "user table":
                        GenerateScriptTable(ObjectName);
                        break;
                    default:
                        break;
                }
            }
        }

        int currentSearch = 0;
        int loop = 0;
        int found = 0;

        private void cmbDifference_SelectionChangeCommitted(object sender, EventArgs e)
        {
            EnumDifference EnumDifference = (EnumDifference)this.cmbDifference.SelectedValue;
            switch (EnumDifference)
            {
                case frmDependsCustom.EnumDifference.Dev:
                    this.panel1.BackColor = Color.Yellow;
                    break;
                case frmDependsCustom.EnumDifference.Pro:
                    this.panel1.BackColor = Color.YellowGreen;
                    break;
                case frmDependsCustom.EnumDifference.All:
                    this.panel1.BackColor = Color.Orange;
                    break;
                default:
                    break;
            }
        }


        private void btnSerach_Click(object sender, EventArgs e)
        {
            currentSearch = 0;
            loop = 0;
            found = 0;

            FilterTreeNode(this.treeView1.Nodes, this.txtSearch.Text);
        }

        private bool CustomSearch(TreeNode Node, string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return Node.Text.Contains(keyword);
            }
            else
            {
                EnumDifference EnumDifference = (EnumDifference)this.cmbDifference.SelectedValue;
                switch (EnumDifference)
                {
                    case frmDependsCustom.EnumDifference.Dev:
                        return (Node.Text.EndsWith("[x][-]"));
                    case frmDependsCustom.EnumDifference.Pro:
                        return (Node.Text.EndsWith("[-][x]"));
                    case frmDependsCustom.EnumDifference.All:
                        return (Node.Text.EndsWith("[x][-]") || Node.Text.EndsWith("[-][x]"));
                    default:
                        return false;
                }
            }
        }


        private bool FilterTreeNode(TreeNodeCollection nodes, string keyword)
        {
            bool result = false;
            for (int i = 0; i < nodes.Count; i++)
            {
                //loop++;
                //if (currentSearch < loop)
                //{
                //currentSearch++;
                if (CustomSearch(nodes[i], keyword))
                {
                    found++;
                    nodes[i].NodeFont = new Font(this.treeView1.Font.FontFamily, this.treeView1.Font.Size, FontStyle.Underline | FontStyle.Bold);
                    this.treeView1.SelectedNode = nodes[i];
                    this.treeView1.SelectedNode.Expand();
                    this.treeView1.SelectedNode.EnsureVisible();
                    this.treeView1.Focus();

                    if (MessageBox.Show(string.Format("ObjectName: {0}. \n\r continue?", nodes[i].Text),
                        string.Format("Current result: {0}  on total {1} nodes.", found, this.treeView1.GetNodeCount(true)),
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question) == DialogResult.No)
                        break;

                    //MessageBox.Show(string.Format("Current result: {0}  on total {1} nodes. \n\r ObjectName: {2}",
                    //                    found, this.treeView1.GetNodeCount(true), nodes[i].Text));
                    result = true;
                }
                //}
                result = FilterTreeNode(nodes[i].Nodes, keyword);
            }

            return result;
        }

        void myConnection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            //TextWriter sw = new StreamWriter(this.FileName, true);
            //sw.WriteLine(e.Message);
            //sw.Close();
        }

        private void GenerateScriptView(string ObjectName)
        {
            string dirScripts = Path.Combine(Environment.CurrentDirectory, @"Scripts");
            Empty(dirScripts);

            string sqlScriptFilePro, sqlScriptFileDev;
            this.GenerateScriptView(ObjectName, this.ConnectionStringPRO, out sqlScriptFilePro, this.cmbDatabase.Text);
            this.GenerateScriptView(ObjectName, this.ConnectionStringDEV, out sqlScriptFileDev, this.cmbCompareDatabase.Text);

            // Crear el proceso
            var psi = new ProcessStartInfo
            {
                FileName = winMergePath,
                Arguments = $"\"{sqlScriptFilePro}\" \"{sqlScriptFileDev}\"", // entre comillas por si hay espacios
                UseShellExecute = false
            };

            // Iniciar WinMerge
            Process.Start(psi);
        }

        private void GenerateScriptView(string ObjectName, string ConnectionString, out string sqlScriptFile, string ambiente)
        {
            ObjectName = ObjectName.Replace("dbo.", "");
            string dir = Path.Combine(Environment.CurrentDirectory, @"Scripts");
            string FileName = Path.Combine(dir, string.Format("0000-[{1}][{0}].sql", ObjectName.Trim(), ambiente));

            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();

                ServerConnection srvConn = new ServerConnection(sqlConn);
                Server srv = new Server(srvConn);
                Database db = srv.Databases[srvConn.DatabaseName];
                foreach (Microsoft.SqlServer.Management.Smo.View sp in db.Views.OfType<Microsoft.SqlServer.Management.Smo.View>().Where(sp => ObjectName.ToUpper().Equals(sp.Name.ToUpper())))
                {
                    using (TextWriter tw = new StreamWriter(FileName, false, Encoding.GetEncoding(1252)))
                    {
                        ScriptingOptions options = new ScriptingOptions();
                        options.ScriptDrops = true;
                        options.IncludeIfNotExists = true;
                        options.ClusteredIndexes = true;
                        options.Default = true;
                        options.DriAll = true;
                        //options.Indexes = true;
                        options.IncludeHeaders = true;
                        options.NoCollation = true;
                        options.AnsiFile = true;
                        options.AnsiPadding = false;
                        options.SchemaQualifyForeignKeysReferences = true;
                        foreach (string sqlScript in sp.Script(options))
                        {
                            tw.WriteLine(sqlScript);
                            tw.WriteLine("GO");
                        }

                        ScriptingOptions optionscreate = new ScriptingOptions();
                        //optionscreate.ScriptDrops = true;
                        //optionscreate.IncludeIfNotExists = true;
                        optionscreate.ClusteredIndexes = true;
                        optionscreate.Default = true;
                        optionscreate.DriAll = true;
                        //optionscreate.Indexes = true;
                        optionscreate.IncludeHeaders = true;
                        optionscreate.NoCollation = true;
                        optionscreate.AnsiFile = true;
                        optionscreate.AnsiPadding = false;
                        optionscreate.SchemaQualifyForeignKeysReferences = true;
                        foreach (string sqlScript in sp.Script(optionscreate))
                        {
                            tw.WriteLine(sqlScript);
                            tw.WriteLine("GO");
                        }
                    }
                }
            }
            //if (File.Exists(FileName)) System.Diagnostics.Process.Start(FileName);
            sqlScriptFile = FileName;
        }

        private void GenerateScriptTrigger(string ObjectName, string triggerName)
        {
            string dirScripts = Path.Combine(Environment.CurrentDirectory, @"Scripts");
            Empty(dirScripts);

            string sqlScriptFilePro, sqlScriptFileDev;
            this.GenerateScriptTrigger(ObjectName, triggerName, this.ConnectionStringPRO, out sqlScriptFilePro, this.cmbDatabase.Text);
            this.GenerateScriptTrigger(ObjectName, triggerName, this.ConnectionStringDEV, out sqlScriptFileDev, this.cmbCompareDatabase.Text);

            // Crear el proceso
            var psi = new ProcessStartInfo
            {
                FileName = winMergePath,
                Arguments = $"\"{sqlScriptFilePro}\" \"{sqlScriptFileDev}\"", // entre comillas por si hay espacios
                UseShellExecute = false
            };

            // Iniciar WinMerge
            Process.Start(psi);
        }

        private void GenerateScriptTrigger(string ObjectName, string triggerName, string ConnectionString, out string sqlScriptFile, string ambiente)
        {
            ObjectName = ObjectName.Replace("dbo.", "");
            triggerName = triggerName.Contains("|") ? triggerName.Split('|')[2].Replace("dbo.", "") : triggerName;
            string dir = Path.Combine(Environment.CurrentDirectory, @"Scripts");
            string FileName = Path.Combine(dir, string.Format("0000-[{1}][{0}].sql", triggerName.Trim(), ambiente));

            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();

                ServerConnection srvConn = new ServerConnection(sqlConn);
                Server srv = new Server(srvConn);
                Database db = srv.Databases[srvConn.DatabaseName];
                Table MyTable = db.Tables.OfType<Table>().Where(sp => ObjectName.ToUpper().Equals(sp.Name.ToUpper())).FirstOrDefault();
                if (MyTable != null)
                {
                    foreach (Trigger sp in MyTable.Triggers.OfType<Trigger>().Where(tr => tr.Name.Equals(triggerName)))
                    {
                        using (TextWriter tw = new StreamWriter(FileName, false, Encoding.GetEncoding(1252)))
                        {
                            ScriptingOptions options = new ScriptingOptions();
                            options.ScriptDrops = true;
                            options.IncludeIfNotExists = true;
                            options.ClusteredIndexes = true;
                            options.Default = true;
                            options.DriAll = true;
                            //options.Indexes = true;
                            options.IncludeHeaders = true;
                            options.NoCollation = true;
                            options.AnsiFile = true;
                            options.AnsiPadding = false;
                            options.SchemaQualifyForeignKeysReferences = true;
                            foreach (string sqlScript in sp.Script(options))
                            {
                                tw.WriteLine(sqlScript);
                                tw.WriteLine("GO");
                            }

                            ScriptingOptions optionscreate = new ScriptingOptions();
                            //optionscreate.ScriptDrops = true;
                            //optionscreate.IncludeIfNotExists = true;
                            optionscreate.ClusteredIndexes = true;
                            optionscreate.Default = true;
                            optionscreate.DriAll = true;
                            //optionscreate.Indexes = true;
                            optionscreate.IncludeHeaders = true;
                            optionscreate.NoCollation = true;
                            optionscreate.AnsiFile = true;
                            optionscreate.AnsiPadding = false;
                            optionscreate.SchemaQualifyForeignKeysReferences = true;
                            foreach (string sqlScript in sp.Script(optionscreate))
                            {
                                tw.WriteLine(sqlScript);
                                tw.WriteLine("GO");
                            }
                        }
                    }
                }
            }
            //if (File.Exists(FileName)) System.Diagnostics.Process.Start(FileName);
            sqlScriptFile = FileName;
        }

        private void GenerateScriptTable(string ObjectName)
        {
            string dirScripts = Path.Combine(Environment.CurrentDirectory, @"Scripts");
            Empty(dirScripts);

            string sqlScriptFilePro, sqlScriptFileDev;
            this.GenerateScriptTable(ObjectName, this.ConnectionStringPRO, out sqlScriptFilePro, this.cmbDatabase.Text);
            this.GenerateScriptTable(ObjectName, this.ConnectionStringDEV, out sqlScriptFileDev, this.cmbCompareDatabase.Text);

            // Crear el proceso
            var psi = new ProcessStartInfo
            {
                FileName = winMergePath,
                Arguments = $"\"{sqlScriptFilePro}\" \"{sqlScriptFileDev}\"", // entre comillas por si hay espacios
                UseShellExecute = false
            };

            // Iniciar WinMerge
            Process.Start(psi);
        }


        private void GenerateScriptTable(string ObjectName, string ConnectionString, out string sqlScriptFile, string ambiente)
        {
            ObjectName = ObjectName.Replace("dbo.", "");
            string dir = Path.Combine(Environment.CurrentDirectory, @"Scripts");
            string FileName = Path.Combine(dir, string.Format("0000-[{1}][{0}].sql", ObjectName.Trim(), ambiente));


            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();

                ServerConnection srvConn = new ServerConnection(sqlConn);
                Server srv = new Server(srvConn);
                Database db = srv.Databases[srvConn.DatabaseName];
                foreach (Table sp in db.Tables.OfType<Table>().Where(sp => ObjectName.Trim().ToUpper().Equals(sp.Name.Trim().ToUpper())))
                {

                    //srv.ConnectionContext.SqlExecutionModes = SqlExecutionModes.CaptureSql;
                    //sp.Columns.Add(new Column(sp, "SomeSMOTest", DataType.DateTime));
                    //sp.Alter();                    
                    //foreach (string st in srv.ConnectionContext.CapturedSql.Text)
                    //{
                    //    Console.WriteLine(st);
                    //}

                    using (TextWriter tw = new StreamWriter(FileName, false, Encoding.GetEncoding(1252)))
                    {
                        ScriptingOptions options = new ScriptingOptions();
                        options.ClusteredIndexes = true;
                        options.Default = true;
                        options.DriAll = true;
                        options.Indexes = true;
                        options.IncludeHeaders = true;
                        options.NoCollation = true;
                        options.AnsiFile = true;
                        options.AnsiPadding = false;
                        options.SchemaQualifyForeignKeysReferences = true;

                        options.Triggers = true; //que cree su items
                        options.IncludeIfNotExists = true; // si no existe que se cree
                        foreach (string sqlScript in sp.Script(options))
                        {
                            tw.WriteLine(sqlScript);
                            tw.WriteLine("GO");
                        }
                    }
                }
            }
            //if (File.Exists(FileName)) System.Diagnostics.Process.Start(FileName);
            sqlScriptFile = FileName;
        }

		//string winMergePath = @"C:\Program Files\WinMerge\WinMergeU.exe";
		string winMergePath = @"C:\Program Files\TortoiseSVN\bin\TortoiseMerge.exe"; 

		private void GenerateScriptSP(string ObjectName)
        {
            string dirScripts = Path.Combine(Environment.CurrentDirectory, @"Scripts");
            Empty(dirScripts);

            string sqlScriptFilePro, sqlScriptFileDev;
            this.GenerateScriptSP(ObjectName, this.ConnectionStringPRO, out sqlScriptFilePro, this.cmbDatabase.Text);
            this.GenerateScriptSP(ObjectName, this.ConnectionStringDEV, out sqlScriptFileDev, this.cmbCompareDatabase.Text);

            // Crear el proceso
            var psi = new ProcessStartInfo
            {
                FileName = winMergePath,
                Arguments = $"\"{sqlScriptFilePro}\" \"{sqlScriptFileDev}\"", // entre comillas por si hay espacios
                UseShellExecute = false
            };

            // Iniciar WinMerge
            Process.Start(psi);
        }

        private void GenerateScriptSP(string ObjectName, string ConnectionString, out string sqlScriptFile, string ambiente)
        {
            ObjectName = ObjectName.Replace("dbo.", "");
            string dir = Path.Combine(Environment.CurrentDirectory, @"Scripts");
            string FileName = Path.Combine(dir, string.Format("0000-[{1}][{0}].sql", ObjectName.Trim(), ambiente));

            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();

                ServerConnection srvConn = new ServerConnection(sqlConn);
                Server srv = new Server(srvConn);
                Database db = srv.Databases[srvConn.DatabaseName];
                foreach (StoredProcedure sp in db.StoredProcedures.OfType<StoredProcedure>().Where(sp => ObjectName.ToLower().Equals(sp.Name.ToLower())))
                {
                    using (TextWriter tw = new StreamWriter(FileName, false, Encoding.GetEncoding(1252)))
                    {
                        ScriptingOptions options = new ScriptingOptions();
                        options.ScriptDrops = true;
                        options.IncludeIfNotExists = true;
                        options.ClusteredIndexes = true;
                        options.Default = true;
                        options.DriAll = true;
                        //options.Indexes = true;
                        options.IncludeHeaders = true;
                        options.NoCollation = true;
                        options.AnsiFile = true;
                        options.AnsiPadding = false;
                        options.SchemaQualifyForeignKeysReferences = true;
                        foreach (string sqlScript in sp.Script(options))
                        {
                            tw.WriteLine(sqlScript);
                            tw.WriteLine("GO");
                        }

                        ScriptingOptions optionscreate = new ScriptingOptions();
                        //optionscreate.ScriptDrops = true;
                        //optionscreate.IncludeIfNotExists = true;
                        optionscreate.ClusteredIndexes = true;
                        optionscreate.Default = true;
                        optionscreate.DriAll = true;
                        //optionscreate.Indexes = true;
                        optionscreate.IncludeHeaders = true;
                        optionscreate.NoCollation = true;
                        optionscreate.AnsiFile = true;
                        optionscreate.AnsiPadding = false;
                        optionscreate.SchemaQualifyForeignKeysReferences = true;
                        foreach (string sqlScript in sp.Script(optionscreate))
                        {
                            tw.WriteLine(sqlScript);
                            tw.WriteLine("GO");
                        }
                    }
                }
            }
            //if (File.Exists(FileName)) System.Diagnostics.Process.Start(FileName);
            sqlScriptFile = FileName;
        }

        private void GenerateScriptFunction(string ObjectName)
        {
            string dirScripts = Path.Combine(Environment.CurrentDirectory, @"Scripts");
            Empty(dirScripts);

            string sqlScriptFilePro, sqlScriptFileDev;
            this.GenerateScriptFunction(ObjectName, this.ConnectionStringPRO, out sqlScriptFilePro, this.cmbDatabase.Text);
            this.GenerateScriptFunction(ObjectName, this.ConnectionStringDEV, out sqlScriptFileDev, this.cmbCompareDatabase.Text);

            // Crear el proceso
            var psi = new ProcessStartInfo
            {
                FileName = winMergePath,
                Arguments = $"\"{sqlScriptFilePro}\" \"{sqlScriptFileDev}\"", // entre comillas por si hay espacios
                UseShellExecute = false
            };

            // Iniciar WinMerge
            Process.Start(psi);
        }

        private void GenerateScriptFunction(string ObjectName, string ConnectionString, out string sqlScriptFile, string ambiente)
        {
            ObjectName = ObjectName.Replace("dbo.", "");
            string dir = Path.Combine(Environment.CurrentDirectory, @"Scripts");
            string FileName = Path.Combine(dir, string.Format("0000-[{1}][{0}].sql", ObjectName.Trim(), ambiente));

            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();

                ServerConnection srvConn = new ServerConnection(sqlConn);
                Server srv = new Server(srvConn);
                Database db = srv.Databases[srvConn.DatabaseName];
                foreach (UserDefinedFunction sp in db.UserDefinedFunctions.OfType<UserDefinedFunction>().Where(sp => ObjectName.ToLower().Equals(sp.Name.ToLower())))
                {
                    using (TextWriter tw = new StreamWriter(FileName, false, Encoding.GetEncoding(1252)))
                    {
                        ScriptingOptions options = new ScriptingOptions();
                        options.ScriptDrops = true;
                        options.IncludeIfNotExists = true;
                        options.ClusteredIndexes = true;
                        options.Default = true;
                        options.DriAll = true;
                        //options.Indexes = true;
                        options.IncludeHeaders = true;
                        options.NoCollation = true;
                        options.AnsiFile = true;
                        options.AnsiPadding = false;
                        options.SchemaQualifyForeignKeysReferences = true;
                        foreach (string sqlScript in sp.Script(options))
                        {
                            tw.WriteLine(sqlScript);
                            tw.WriteLine("GO");
                        }

                        ScriptingOptions optionscreate = new ScriptingOptions();
                        //optionscreate.ScriptDrops = true;
                        //optionscreate.IncludeIfNotExists = true;
                        optionscreate.ClusteredIndexes = true;
                        optionscreate.Default = true;
                        optionscreate.DriAll = true;
                        //optionscreate.Indexes = true;
                        optionscreate.IncludeHeaders = true;
                        optionscreate.NoCollation = true;
                        optionscreate.AnsiFile = true;
                        optionscreate.AnsiPadding = false;
                        optionscreate.SchemaQualifyForeignKeysReferences = true;
                        foreach (string sqlScript in sp.Script(optionscreate))
                        {
                            tw.WriteLine(sqlScript);
                            tw.WriteLine("GO");
                        }
                    }
                }
            }
            //if (File.Exists(FileName)) System.Diagnostics.Process.Start(FileName);
            sqlScriptFile = FileName;
        }

        private void GenerateScriptOLD(string ObjectName)
        {
            string FileName = Path.Combine(Environment.CurrentDirectory, string.Format("{0}.txt", ObjectName.Trim()));
            File.WriteAllText(FileName, String.Empty);
            List<SqlParameter> l = new List<SqlParameter>();
            l.Add(new SqlParameter("@objname", ObjectName.Trim()));
            using (SqlConnection c = new SqlConnection(ConnectionStringPRO))
            {
                c.InfoMessage += new SqlInfoMessageEventHandler(myConnection_InfoMessage);
                using (SqlCommand cmd = new SqlCommand("dbo.sp_helptext", c) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.AddRange(l.ToArray());
                    c.Open();
                    cmd.CommandTimeout = 600;
                    SqlDataReader result = cmd.ExecuteReader();
                    do
                    {
                        while (result.Read())
                        {
                            TextWriter sw = new StreamWriter(FileName, false, Encoding.GetEncoding(1252));
                            string text = result[0].ToString();
                            sw.WriteLine(text);
                            sw.Close();
                        }
                    }
                    while (result.NextResult());
                }
            }
            if (File.Exists(FileName)) System.Diagnostics.Process.Start(FileName);
        }

        private void Example(string ObjectName)
        {
            ObjectName = ObjectName.Replace("dbo.", "");
            string FileName = Path.Combine(Environment.CurrentDirectory, string.Format("{0}.txt", ObjectName.Trim()));
            using (SqlConnection sqlConn = new SqlConnection(this.ConnectionStringPRO))
            {
                sqlConn.Open();
                ServerConnection srvConn = new ServerConnection(sqlConn);
                Server srv = new Server(srvConn);
                Database db = srv.Databases[srvConn.DatabaseName];
                var dependencyWalker = new DependencyWalker(srv);
                var dependencyTree = dependencyWalker.DiscoverDependencies(
                //db.StoredProcedures.OfType<StoredProcedure>().Where(sp => ObjectName.ToLower().Equals(sp.Name.ToLower())).ToArray(),                    
                db.Tables.OfType<Table>().Where(sp => ObjectName.ToLower().Equals(sp.Name.ToLower())).ToArray(),
                DependencyType.Parents);
                var dependencyCollection = dependencyWalker.WalkDependencies(dependencyTree);
                foreach (DependencyCollectionNode dependencyCollectionNode in dependencyCollection.Reverse())
                {
                    Console.WriteLine(dependencyCollectionNode.Urn);
                    var smoObject = db.Parent.GetSmoObject(dependencyCollectionNode.Urn);
                }
            }
            if (File.Exists(FileName))
                System.Diagnostics.Process.Start(FileName);
        }


        private BackgroundWorker BackgroundWorker = new BackgroundWorker();

        public void DrawTree()
        {
            contextMenuStrip1.Close();
            this.ValidRepit.Clear();
            Node<MySqlObjects> root = new Node<MySqlObjects>(new MySqlObjects() { name = this.ObjectName }) { Name = this.ObjectName };
            root.IsRoot = true;
            this.GenerateTree(root, this.ObjectName, this.ddlType.Text);
            frmRadialTree frm = new frmRadialTree();
            frm.radialTreePanelMySqlObjects1.TreeNode = root;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();

        }

        void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Draw")
                DrawTree();
            if (e.ClickedItem.Text == "Example")
                Example(this.ObjectName);
            else if (e.ClickedItem.Text == "TreeView")
                GenerateTreeview();
            else if (e.ClickedItem.Text == "TreeText")
                GenerateTreeText();
            else if (e.ClickedItem.Text == "ListText")
                GenerateListText();
            else if (e.ClickedItem.Text == "Generate SP Root")
            {
                switch (this.ddlType.Text)
                {
                    case "stored procedure":
                        GenerateScriptSP(this.ObjectName);
                        break;
                    case "scalar function":
                        GenerateScriptFunction(this.ObjectName);
                        break;
                    case "view":
                        GenerateScriptView(this.ObjectName);
                        break;
                    case "trigger":
                        string TableName = ParentObject(this.ObjectName, "trigger", this.ConnectionStringPRO);
                        GenerateScriptTrigger(TableName, this.ObjectName);
                        break;
                    case "user table":
                        GenerateScriptTable(this.ObjectName);
                        break;
                    default:
                        break;
                }

            }
            else if (e.ClickedItem.Text == "Print PostOrden")
            {
                List<EnumModel> enums = ((IEnumerable<EnumObjectType>)Enum
                    .GetValues(typeof(EnumObjectType)))
                    .Select(c => new EnumModel()
                    {
                        Value = (int)c,
                        Name = c.GetDescription()
                    }).ToList();

                List<MySqlObjects> l = new List<MySqlObjects>();

                List<MySqlObjects> l2 = new List<MySqlObjects>();
                Node<MySqlObjects>.DepthFirstSearch<MySqlObjects>(this.RootNode, ref l2);

                //Node<MySqlObjects>.PostOrden<MySqlObjects>(this.RootNode, ref l);
                Node<MySqlObjects>.PostOrden<MySqlObjects>(this.RootNode, ref l);

                List<MySqlObjectsModel> sqldependes = l.Select((sql, index) => new MySqlObjectsModel
                {
                    sqlindex = index,
                    sqlname = sql.name,
                    sqltype = sql.type
                }).ToList();

                //List<string> query = sqldependes
                //    .Join(enums, sd => sd.sqltype, enu => enu.Name, (sd, enu) => new { sd, enu })
                //    .OrderBy(o => o.enu.Value)
                //    .ThenByDescending(o => o.sd.sqlindex)
                //    .Select(o => o.sd.sqlname)
                //    .ToList();

                List<string> query = sqldependes
                    .Join(enums, sd => sd.sqltype, enu => enu.Name, (sd, enu) => new { sd, enu })
                    .OrderBy(o => o.enu.Value)
                    .ThenBy(o => o.sd.sqlindex)
                    .Select(o => o.sd.sqlname)
                    .ToList();


                string FileName = Path.Combine(Environment.CurrentDirectory, string.Format("{0}.txt", Guid.NewGuid()));
                TextWriter sw = new StreamWriter(FileName, true);
                query.ForEach(s => sw.WriteLine("{0}", s));
                sw.Close();
                if (File.Exists(FileName)) System.Diagnostics.Process.Start(FileName);

            }
        }

        public class MySqlObjectsModel
        {
            public int sqlindex { get; set; }
            public string sqlname { get; set; }
            public string sqltype { get; set; }
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
            public string type { get; set; }
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
            this.GenerateTree(root, this.ObjectName, this.ddlType.Text);
            root.PrintList(this.FileName);
            if (File.Exists(FileName)) System.Diagnostics.Process.Start(FileName);
        }

        private void GenerateTreeText()
        {
            this.ValidRepit.Clear();
            File.WriteAllText(this.FileName, String.Empty);
            Node<MySqlObjects> root = new Node<MySqlObjects>(new MySqlObjects() { name = this.ObjectName }) { Name = this.ObjectName };
            this.GenerateTree(root, this.ObjectName, this.ddlType.Text);
            root.PrintNode("", this.FileName, (EnumDifference)this.cmbDifference.SelectedValue);
            if (File.Exists(FileName)) System.Diagnostics.Process.Start(FileName);
        }

        public class GenTreeView
        {
            public Node<MySqlObjects> RootNode { get; set; }
            public string ObjectName { get; set; }
            public string Type { get; set; }
        }

        Node<MySqlObjects> RootNode = new Node<MySqlObjects>();

        private void GenerateTreeview()
        {
            if (this.backgroundWorker1.IsBusy != true)
            {
                this.btnGenerate.Image = UtilETWeb.Properties.Resources.generator;
                this.ValidRepit.Clear();
                RootNode.Nodes.Clear();
                this.backgroundWorker1.RunWorkerAsync(new GenTreeView()
                {
                    ObjectName = ObjectName,
                    Type = this.ddlType.Text,
                    RootNode = RootNode
                });
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "";
            contextMenuStrip1.Items.Clear();
            contextMenuStrip1.Items.Add("TreeView");
            contextMenuStrip1.Items.Add("TreeText");
            contextMenuStrip1.Items.Add("Draw");
            contextMenuStrip1.Items.Add("ListText");
            contextMenuStrip1.Items.Add("Generate SP Root");
            contextMenuStrip1.Items.Add("Print PostOrden");
            contextMenuStrip1.Items.Add("Example");
            contextMenuStrip1.Show(btnGenerate, new Point(0, btnGenerate.Height));
        }

        private string ParentObject(string ObjectName, string Type, string StringConnection)
        {
            string value = "";
            using (SqlConnection c = new SqlConnection(StringConnection))
            {

                StringBuilder strcommand = new StringBuilder();
                strcommand.Append(
                    "select	t2.name from	sysobjects t1 " +
                    "inner join sysobjects t2 on t1.parent_obj = t2.id " +
                    "where	t1.name ='{0}' and t1.xtype='tr' " +
                    "and t2.xtype='u'");

                using (SqlCommand cmd = new SqlCommand(string.Format(strcommand.ToString(), ObjectName), c))
                {
                    c.Open();
                    cmd.CommandTimeout = 600;
                    string result = (string)cmd.ExecuteScalar();
                    if (!string.IsNullOrEmpty(result))
                        value = result.ToString();
                }
            }
            return value;
        }

        private bool ExistObject(string ObjectName, string Type, string StringConnection)
        {
            bool value = false;
            using (SqlConnection c = new SqlConnection(StringConnection))
            {
                using (SqlCommand cmd = new SqlCommand("select isnull(object_id('" + ObjectName + "'),0)", c))
                {
                    c.Open();
                    cmd.CommandTimeout = 600;
                    var result = cmd.ExecuteScalar();
                    if ((int)result > 0) value = true;
                }
            }
            return value;
        }

        private bool ExistColumn(string ObjectName, string Type, string ColumnName, string StringConnection)
        {
            bool value = false;
            using (SqlConnection c = new SqlConnection(StringConnection))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(COL_LENGTH('" + ObjectName + "','" + ColumnName + "'),0)", c))
                {
                    c.Open();
                    cmd.CommandTimeout = 600;
                    var result = cmd.ExecuteScalar();
                    if ((short)result > 0) value = true;
                }
            }
            return value;
        }


        private bool ExistParam(string ObjectName, string Type, string ParamName, string StringConnection)
        {
            bool value = false;
            using (SqlConnection c = new SqlConnection(StringConnection))
            {
                using (SqlCommand cmd = new SqlCommand("select parameter_id from sys.parameters where object_id = object_id('" + ObjectName + "') and name='" + ParamName + "'", c))
                {
                    c.Open();
                    cmd.CommandTimeout = 600;
                    var result = cmd.ExecuteScalar();
                    if (result != null) value = true;
                }
            }
            return value;
        }


        private bool Exist(string Object, string Type, string StringConnection)
        {
            switch (Type)
            {
                case "param":
                    return ExistParam(Object.Split('|')[1], Type, Object.Split('|')[2], StringConnection);
                case "column":
                    return ExistColumn(Object.Split('|')[1], Type, Object.Split('|')[2], StringConnection);
                case "trigger":
                    return ExistObject(Object.Split('|')[2], Type, StringConnection);
                default:
                    return ExistObject(Object.Split('|')[1], Type, StringConnection);
            }
        }

        private void RefreshSqlModule(string ObjectName, string StringConnection)
        {
            try
            {
                List<SqlParameter> l = new List<SqlParameter>();
                l.Add(new SqlParameter("@name", ObjectName));
                Querys.ExecNonQuery("sp_refreshsqlmodule", l, StringConnection);

            }
            catch (Exception)
            {
                //throw;
            }
        }


        #region MyRegion
        delegate void SetRichTextCallback(RichTextBox RichTextBox, object text, object ConnectionString);

        private void SetRichText(RichTextBox RichTextBox, object text, object ConnectionString)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (RichTextBox.InvokeRequired)
            {
                SetRichTextCallback d = new SetRichTextCallback(SetRichText);
                this.Invoke(d, new object[] { RichTextBox, text, ConnectionString });
            }
            else
            {
                RichTextBox.Text += string.Format("{1}\n[Error]\t{2}\n\r", DateTime.Now, ConnectionString, text);
            }
        }

        #endregion

        delegate void SetTextCallback(ComboBox cmb, object text);
        delegate object GetTextCallback(ComboBox cmb);

        private void SetText(ComboBox cmb, object text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (cmb.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { cmb, text });
            }
            else
            {
                cmb.SelectedValue = text;
            }
        }

        private object GetText(ComboBox cmb)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (cmb.InvokeRequired)
            {
                GetTextCallback d = new GetTextCallback(GetText);
                return this.Invoke(d, new object[] { cmb });
            }
            else
            {
                return cmb.SelectedValue;
            }
        }

        private string ConnectionStringPRO
        {
            get
            {
                if (GetText(this.cmbDatabase).ToString().Equals("-1"))
                {
                    return System.Configuration.ConfigurationManager.AppSettings["ConnectionStringPRO"];
                    //return System.Configuration.ConfigurationManager.AppSettings["ConnectionStringDMO"];
                }
                else
                    return GetText(this.cmbDatabase).ToString();
            }
        }

        private string ConnectionStringDEV
        {
            get
            {
                if (GetText(this.cmbCompareDatabase).ToString().Equals("-1"))
                {
                    return System.Configuration.ConfigurationManager.AppSettings["ConnectionStringDMO"];
                    //return System.Configuration.ConfigurationManager.AppSettings["ConnectionStringDEV"];
                }
                else
                {
                    return GetText(this.cmbCompareDatabase).ToString();
                }
            }
        }

        private DataTable GetDependes(string ObjectName, string type)
        {
            string spro = GetDependes(ObjectName, type, this.ConnectionStringPRO);
            string sdev = GetDependes(ObjectName, type, this.ConnectionStringDEV);
            return Compare(spro, sdev);
        }

        private DataTable Compare(string spro, string sdev)
        {
            DataTable dt = new DataTable();
            List<SqlParameter> l = new List<SqlParameter>();
            l.Add(new SqlParameter("@Spro", spro.Replace("\n", "")));
            l.Add(new SqlParameter("@Sdev", sdev.Replace("\n", "")));
            DataSet ds = Querys.ExecDataSet("sp_CompareText", l);
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        private List<string> GetColumnsProcedures(string ObjectName, string type, string StringConnection)
        {
            List<string> list = new List<string>() { };
            List<SqlParameter> l = new List<SqlParameter>();
            l.Add(new SqlParameter("@objname", ObjectName));
            DataSet ds = Querys.ExecDataSet("sp_depends", l, StringConnection);
            StringBuilder sb = new StringBuilder();
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                switch (type)
                {
                    case "stored procedure":
                    //case "scalar function":
                        list = dt.Rows.OfType<DataRow>()
                            .Where(dr => dr.Field<String>("selected").Equals("yes"))
                            .OrderBy(dr => dr.Field<string>("column"))
                            .Select(dr =>
                                string.Format("column|{0}|{1}|{2}",
                                ObjectName,
                                dr.Field<string>("type"),
                                dr.Field<string>("column"))).ToList();
                        break;
                    case "view":
                    case "user table":
                        break;
                    default:
                        break;
                }
            }

            return list;
        }

        private List<string> GetColumns(string ObjectName, string type, string StringConnection)
        {
            List<string> list = new List<string>() { };
            List<SqlParameter> l = new List<SqlParameter>();
            l.Add(new SqlParameter("@objname", ObjectName));
            DataSet ds = Querys.ExecDataSet("sp_help", l, StringConnection);
            StringBuilder sb = new StringBuilder();
            if (1 < ds.Tables.Count)
            {
                DataTable dt = ds.Tables[1];
                switch (type)
                {
                    case "stored procedure":
                    case "scalar function":
                        foreach (DataTable item in ds.Tables)
                        {
                            if (item.Columns.Contains("Parameter_Name")) {
                                list = item.Rows.OfType<DataRow>()
                                    .OrderBy(dr => dr.Field<string>("Parameter_Name"))
                                    .Select(dr =>
                                    string.Format("param|{0}|{1}|{2}|{3}|",
                                    ObjectName,
                                    dr.Field<string>("Parameter_Name"),
                                    dr.Field<string>("Type"),
                                    dr.Field<short>("Length"))).ToList();
                            }                                                       
                        }
                        break;
                    case "view":
                    case "user table":
                        list = dt.Rows.OfType<DataRow>()
                            .OrderBy(dr => dr.Field<string>("Column_name"))
                            .Select(dr =>
                                string.Format("column|{0}|{1}|{2}|{3}|",
                                ObjectName,
                                dr.Field<string>("Column_name"),
                                dr.Field<string>("Type"),
                                dr.Field<int>("Length"))).ToList();
                        break;
                    default:
                        break;
                }
            }

            return list;
        }


        private List<string> GetTriggers(string ObjectName, string type, string StringConnection)
        {
            List<string> list = new List<string>() { };
            List<SqlParameter> l = new List<SqlParameter>();
            l.Add(new SqlParameter("@tabname", ObjectName));
            DataSet ds = Querys.ExecDataSet("sp_helptrigger", l, StringConnection);
            StringBuilder sb = new StringBuilder();
            if (0 < ds.Tables.Count)
            {
                DataTable dt = ds.Tables[0];
                switch (type)
                {
                    case "view":
                    case "user table":
                        list = dt.Rows.OfType<DataRow>()
                            .OrderBy(dr => dr.Field<string>("trigger_name"))
                            .Select(dr =>
                                string.Format("trigger|{0}|{1}|{2}|{3}|{4}|",
                                ObjectName,
                                dr.Field<string>("trigger_name"),
                                dr.Field<int>("isinsert") > 0 ? "i" : "-",
                                dr.Field<int>("isupdate") > 0 ? "u" : "-",
                                dr.Field<int>("isdelete") > 0 ? "d" : "-"
                                )).ToList();
                        break;
                    default:
                        break;
                }
            }

            return list;
        }

        private string GetDependes(string ObjectName, string type, string StringConnection)
        {

            List<string> list = new List<string>() { };
            try
            {
                if ("stored procedure|scalar function|table function|view".Split("|".ToCharArray()).Contains(type))
                    this.RefreshSqlModule(ObjectName, StringConnection);

                //list.AddRange(GetColumns(ObjectName, type, StringConnection));

                List<SqlParameter> l = new List<SqlParameter>();
                l.Add(new SqlParameter("@objname", ObjectName));
                DataTable dt = Querys.ExecDatatable("sp_depends", l, StringConnection);
                StringBuilder sb = new StringBuilder();
                if (dt.Rows.Count > 0)
                {
                    //dt.Rows.OfType<DataRow>().ToList().ForEach(dr => sb.AppendLine(string.Format("{0}|{1}",dr.Field<string>("name"), dr.Field<string>("type"))));                
                    switch (type)
                    {
                        case "stored procedure":
                        case "scalar function":
                            List<string> newListsp = dt.Rows.OfType<DataRow>()
                                .Where(row => "stored procedure|scalar function|table function|user table|view".Split("|".ToCharArray()).Contains(row.Field<string>("type")))
                                .GroupBy(row => new { name = row.Field<string>("name"), type = row.Field<string>("type") })
                                .OrderBy(col => col.Key.name)
                                .Select(g => string.Format("{0}|{1}", g.Key.type, g.Key.name)).ToList();
                            list.AddRange(newListsp);
                            list.AddRange(GetColumns(ObjectName, type, StringConnection));
                            list.AddRange(GetColumnsProcedures(ObjectName, type, StringConnection));
                            break;
                        case "view":
                            List<string> newListvw = dt.Rows.OfType<DataRow>()
                                .Where(row => "user table|view".Split("|".ToCharArray()).Contains(row.Field<string>("type")))
                                .GroupBy(row => new { name = row.Field<string>("name"), type = row.Field<string>("type") })
                                .OrderBy(col => col.Key.name)
                                .Select(g => string.Format("{0}|{1}", g.Key.type, g.Key.name)).ToList();
                            list.AddRange(GetColumns(ObjectName, type, StringConnection));
                            list.AddRange(newListvw);
                            break;
                        case "user table":
                            //No queremos triggers de otras tablas
                            List<string> newListtb = dt.Rows.OfType<DataRow>()
                               .Where(row => "trigger".Split("|".ToCharArray()).Contains(row.Field<string>("type")))
                               .GroupBy(row => new { name = row.Field<string>("name"), type = row.Field<string>("type") })
                               .OrderBy(col => col.Key.name)
                               .Select(g => string.Format("{0}|{1}|{2}", g.Key.type, ObjectName, g.Key.name)).ToList();
                            list.AddRange(newListtb);
                            list.AddRange(GetTriggers(ObjectName, type, StringConnection));
                            list.AddRange(GetColumns(ObjectName, type, StringConnection));
                            break;
                        case "trigger":
                            //no se implemente por que analizara toda la base de datos;
                            break;
                        default:
                            break;
                    }

                }
                list.ForEach(line => sb.AppendLine(line));
                return sb.ToString();
            }
            catch (Exception ex)
            {
                SetRichText(richTextBox1, ex.Message, Deserialize(StringConnection)["Initial Catalog"]);
                return "";
            }
        }

        public static string Serialize(StringDictionary data)
        {
            if (data == null) return null; // GIGO
            DbConnectionStringBuilder db = new DbConnectionStringBuilder();
            foreach (string key in data.Keys)
            {
                db[key] = data[key];
            }
            return db.ConnectionString;
        }
        public static StringDictionary Deserialize(string data)
        {
            if (data == null) return null; // GIGO
            DbConnectionStringBuilder db = new DbConnectionStringBuilder();
            StringDictionary lookup = new StringDictionary();
            db.ConnectionString = data;
            foreach (string key in db.Keys)
            {
                lookup[key] = Convert.ToString(db[key]);
            }
            return lookup;
        }

        public class EnumModel
        {
            public int Value { get; set; }
            public string Name { get; set; }
        }

        public enum EnumDifference
        {
            [DescriptionEnum("all differences")]
            All = 2,
            [DescriptionEnum("only first")]
            Pro = 1,
            [DescriptionEnum("only last")]
            Dev = 0
        }

        public enum EnumObjectType
        {
            [DescriptionEnum("stored procedure")]
            stored_procedure = 5,
            [DescriptionEnum("scalar function")]
            scalar_function = 4,
            [DescriptionEnum("view")]
            view = 3,
            [DescriptionEnum("trigger")]
            trigger = 2,
            [DescriptionEnum("user table")]
            user_table = 1
        }

        private void GenerateTree(Node<MySqlObjects> root, string ObjectName, string type)
        {

            DataTable dt = GetDependes(ObjectName, type);
            foreach (DataRow item in dt.Rows)
            {
                if (!item.Field<string>("object").Split('|')[1].StartsWith("dbo.tz") &&
                    !item.Field<string>("object").Split('|')[1].StartsWith("dbo.vz") &&
                    !item.Field<string>("object").Split('|')[1].StartsWith("dbo.pz") &&
                    !this.ValidRepit.Contains(item.Field<string>("object")))
                {

                    this.ValidRepit.Add(item.Field<string>("object"));

                    string PRO = item.Field<string>("PRO");
                    if (PRO.Equals("-") && !item.Field<string>("object").EndsWith("*"))
                        PRO = Exist(item.Field<string>("object"), item.Field<string>("object").Split("|".ToCharArray())[0], this.ConnectionStringPRO) ? "x" : "-";

                    string DEV = item.Field<string>("DEV");
                    if (DEV.Equals("-") && !item.Field<string>("object").EndsWith("*"))
                        DEV = Exist(item.Field<string>("object"), item.Field<string>("object").Split("|".ToCharArray())[0], this.ConnectionStringDEV) ? "x" : "-";

                    string value = string.Format("{0}|:[{1}][{2}]", item.Field<string>("object"), PRO, DEV);
                    string valuetype = item.Field<string>("object").Split("|".ToCharArray())[0];

                    Node<MySqlObjects> CurrentNode = new Node<MySqlObjects>(new MySqlObjects()
                    {
                        name = value,
                        type = valuetype

                    })
                    { Name = value };

                    CurrentNode.Parent = root;

                    root.Nodes.Add(CurrentNode);
                    if (!item.Field<string>("object").Split('|')[0].StartsWith("param") && !item.Field<string>("object").Split('|')[0].StartsWith("column"))
                        this.GenerateTree(CurrentNode, item.Field<string>("object").Split("|".ToCharArray())[1], item.Field<string>("object").Split("|".ToCharArray())[0]);
                }
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            MessageBox.Show(e.Node.Text);
        }

        ContextMenuStrip contextMenuStripTreeView = new ContextMenuStrip();

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && this.treeView1.SelectedNode != null && this.treeView1.SelectedNode.Bounds.Contains(e.Location))
            {
                contextMenuStripTreeView.Items.Clear();
                contextMenuStripTreeView.Items.Add("GenerateScript");
                contextMenuStripTreeView.Tag = this.treeView1.SelectedNode.Text;
                //contextMenuStripTreeView.Show(this.treeView1, e.Location);
                contextMenuStripTreeView.Show(this.treeView1, new Point((int)(this.treeView1.SelectedNode.Bounds.X - this.HorizontalScroll.Value), (int)(this.treeView1.SelectedNode.Bounds.Y + this.treeView1.SelectedNode.Bounds.Height - this.VerticalScroll.Value)));
            }
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyValue == (char)Keys.F2)
            {
                this.treeView1.SelectedNode.BeginEdit();
                //here it is editing the treenode once it is done user should rename the folder also in the drive
            }
            else if (e.KeyValue == 93)
            {
                contextMenuStripTreeView.Items.Clear();
                contextMenuStripTreeView.Items.Add("GenerateScript");
                contextMenuStripTreeView.Tag = this.treeView1.SelectedNode.Text;
                //contextMenuStripTreeView.Show(this.treeView1, e.Location);
                contextMenuStripTreeView.Show(this.treeView1, new Point((int)(this.treeView1.SelectedNode.Bounds.X - this.HorizontalScroll.Value), (int)(this.treeView1.SelectedNode.Bounds.Y + this.treeView1.SelectedNode.Bounds.Height - this.VerticalScroll.Value)));
            }

        }

        private void ddlType_SelectedValueChanged(object sender, EventArgs e)
        {
            switch (this.ddlType.Text)
            {
                case "stored procedure":
                    this.pictureBox1.Image = Properties.Resources.script;
                    break;
                case "scalar function":
                    this.pictureBox1.Image = Properties.Resources.function;
                    break;
                case "view":
                    this.pictureBox1.Image = Properties.Resources.table_select_all;
                    break;
                case "trigger":
                    this.pictureBox1.Image = Properties.Resources.lightning;
                    break;
                case "user table":
                    this.pictureBox1.Image = Properties.Resources.table;
                    break;
            }
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }



    }
}
