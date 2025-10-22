using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using System.IO;
using Microsoft.SqlServer;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.Reflection;
using UtilETWeb.Data;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;



namespace UtilETWeb
{
    public partial class frmGenerateProcedures : Form
    {

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);
        //from http://msdn.microsoft.com/en-us/library/system.drawing.bitmap.gethicon.aspx
        private Icon bitmapToIcon(Bitmap myBitmap)
        {
            myBitmap.SetResolution(72, 72);

            // Get an Hicon for myBitmap.
            IntPtr Hicon = myBitmap.GetHicon();

            // Create a new icon from the handle. 
            Icon newIcon = Icon.FromHandle(Hicon);

            return newIcon;
        }

        public frmGenerateProcedures()
        {
            /*
            String fileName = string.Format("{0}.ico", Guid.NewGuid());
            Stream IconStream = System.IO.File.OpenWrite(fileName);
            Icon icon = bitmapToIcon(Properties.Resources.arrow);
            this.Icon = icon;
            icon.Save(IconStream);
            */


            //this.Icon = Icon.FromHandle(Properties.Resources.arrow.GetHicon());
            //using (FileStream stream = File.OpenWrite(@"C:\temp\test.ico"))
            //{
            //    Bitmap bitmap = (Bitmap)Image.FromFile(@"c:\temp\test.png");
            //    Icon.FromHandle(bitmap.GetHicon()).Save(stream);
            //}

            InitializeComponent();


            //this.Load += new EventHandler(frmGenerateProcedures_Load);

            this.cmbDatabase.DataSource = GetConnexions();
            this.cmbDatabase.ValueMember = "Code";
            this.cmbDatabase.DisplayMember = "Name";
            //this.cmbDatabase.SelectedValue = System.Configuration.ConfigurationManager.AppSettings["ConnectionStringDMO"];
            this.cmbDatabase.SelectedValue = "-1";

            this.cmbDatabase.MaxDropDownItems = 40;
            this.cmbDatabase.IntegralHeight = false;



            List<Encoding> l = new List<Encoding>()
            {
                Encoding.UTF8,
                Encoding.GetEncoding(1252)
            };

            this.comboBox1.DisplayMember = "BodyName";
            this.comboBox1.DataSource = l;
            this.comboBox1.SelectedIndex = 0;

            bgwProcedure.WorkerReportsProgress = true;
            bgwProcedure.DoWork += new DoWorkEventHandler(bgwProcedure_DoWork);
            bgwProcedure.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwProcedure_RunWorkerCompleted);
            bgwProcedure.ProgressChanged += new ProgressChangedEventHandler(bgwProcedure_ProgressChanged);


            bgwTable.WorkerReportsProgress = true;
            bgwTable.DoWork += new DoWorkEventHandler(bgwTable_DoWork);
            bgwTable.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwTable_RunWorkerCompleted);
            bgwTable.ProgressChanged += new ProgressChangedEventHandler(bgwProcedure_ProgressChanged);


        }

        void bgwTable_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button3.Enabled = true;
            Process.Start("explorer.exe", Path.Combine(Environment.CurrentDirectory, @"Scripts"));
        }

        void bgwTable_DoWork(object sender, DoWorkEventArgs e)
        {
            List<SqlFile> _listsp = e.Argument as List<SqlFile>;

            using (SqlConnection sqlConn = new SqlConnection(this.ConnectionString))
            {
                sqlConn.Open();
                ServerConnection srvConn = new ServerConnection(sqlConn);
                Server srv = new Server(srvConn);
                Database db = srv.Databases[srvConn.DatabaseName];

                List<Table> tables = db.Tables.OfType<Table>().Where(sp => _listsp.Any(s => s.RealName.ToUpper().Equals(sp.Name.ToUpper()))).ToList();

                foreach (Table sp in tables)
                {
                    string myFileName = _listsp.FirstOrDefault(d => d.RealName.ToUpper().Equals(sp.Name.ToUpper())).Name;
                    using (TextWriter tw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, string.Format(@"Scripts\{0}", myFileName)), false, (Encoding)EncodingSelect))
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



                        options.Triggers = true;
                        options.IncludeIfNotExists = true;

                        foreach (string sqlScript in sp.Script(options))
                        {
                            tw.WriteLine(sqlScript);
                            tw.WriteLine("GO");
                        }
                        /////end

                        bgwTable.ReportProgress((int)(((tables.IndexOf(sp) + 1f) / tables.Count()) * 100.00));

                    }

                }
            }

        }

        void bgwProcedure_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        void bgwProcedure_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.btnGenerate.Enabled = true;
            //Process.Start("explorer.exe", Path.Combine(Environment.CurrentDirectory, @"Scripts"));
            string folderToOpen = Path.Combine(Environment.CurrentDirectory, @"Scripts");
            ExplorerHelper.FocusIfExplorerOpen(folderToOpen);

        }

        void bgwProcedure_DoWork(object sender, DoWorkEventArgs e)
        {

            var argument = e.Argument as object[];

            List<SqlFile> _listsp = argument[0] as List<SqlFile>;
            string _ConnectionString = argument[1] as string;

            using (SqlConnection sqlConn = new SqlConnection(this.ConnectionString))
            {
                sqlConn.Open();
                ServerConnection srvConn = new ServerConnection(sqlConn);
                Server srv = new Server(srvConn);
                Database db = srv.Databases[srvConn.DatabaseName];
                List<StoredProcedure> procedures = db.StoredProcedures.OfType<StoredProcedure>().Where(sp => _listsp.Any(s => s.RealName.ToUpper().Equals(sp.Name.ToUpper()))).ToList();

                foreach (StoredProcedure sp in procedures)
                {
                    string myFileName = _listsp.FirstOrDefault(d => d.RealName.ToUpper().Equals(sp.Name.ToUpper())).Name;
                    using (TextWriter tw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, string.Format(@"Scripts\{0}", myFileName)), false, (Encoding)EncodingSelect))
                    {
                        //foreach (string sqlScript in sp.Script())
                        //{
                        //    tw.WriteLine(sqlScript);
                        //}

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

                        bgwProcedure.ReportProgress((int)(((procedures.IndexOf(sp) + 1f) / procedures.Count()) * 100.00));

                        /////end
                    }
                }
            }


            //for (int i = 0; i < 10; i++)
            //{
            //    System.Threading.Thread.Sleep(1000);
            //    bgwProcedure.ReportProgress((int)(((i + 1) / 10f) * 100.00));
            //}
        }

        private void Procedure_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (!bgwProcedure.IsBusy)
            {
                Empty(Path.Combine(Environment.CurrentDirectory, @"Scripts"));
                btn.Enabled = false;
                progressBar1.Value = 0;

                object[] arrayDeObjetos = { listsp, this.cmbDatabase.SelectedValue };
                bgwProcedure.RunWorkerAsync(arrayDeObjetos);
            }
        }

        private string ConnectionStringPRO
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ConnectionStringPRO"];
            }
        }

        private string ConnectionStringDEV
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ConnectionStringDEV"];
            }
        }

        private string sps
        {
            get
            {
                return "sp_webSvXigAutoSendMail_WCR|XigAsociacionesAltaEnvioNotif|pfrmPersonXigMembresiaAsoc_List_CVitR|pfrmPersonXigMembresiaAsoc_Get_CVitR|pfrmPersonXigMembresiaAsoc_Save_CVitW|pfrmPersonXigMembresiaAsoc_Del_CVitW|pfrmXigRazonSocialOrg_List|pselXigRolOrganismo_Get|pselXigTipoCuotaOrganismo_Get|pselXigPeriodoOrganismo_Get|pselXigMonedaCuotaOrganismo_Get|pselXigEstatusMembresiaOrganismo_Get|pselXigMonedaCuotaExtraOrganismo_Get|pselXigPeriodoExtraOrganismo_Get|pPersonXigMembresiaAsocBuscaRS_Get|pPersonXigMembresiaAsocBuscaRS_Save|pselXigGiroOrganismo_Get";
            }
        }

        ToolTip tip = new ToolTip();

        void frmGenerateProcedures_Load(object sender, EventArgs e)
        {
            this.ActiveControl = this.richTextBox1;
            tip.IsBalloon = true;

            using (SqlConnection sqlConn = new SqlConnection(this.ConnectionStringPRO))
            {
                sqlConn.Open();

                ServerConnection srvConn = new ServerConnection(sqlConn);
                Server srv = new Server(srvConn);
                Database db = srv.Databases["Etwebdev103"];
                foreach (StoredProcedure sp in db.StoredProcedures.OfType<StoredProcedure>().Where(sp => this.sps.ToUpper().Split('|').Contains(sp.Name.ToUpper())))
                {
                    using (TextWriter tw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, string.Format(@"Scripts\{0}.sql", sp.Name))))
                    {
                        tw.WriteLine("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + sp.Name + "]') AND type in (N'P', N'PC'))");
                        tw.WriteLine("DROP PROCEDURE [dbo].[" + sp.Name + "]");
                        tw.WriteLine("GO");

                        foreach (string sqlScript in sp.Script())
                        {
                            tw.WriteLine(sqlScript);
                            tw.WriteLine("GO");
                        }
                    }
                }
            }
        }

        private List<UtilETWeb.MyConfigSection.MyConfigInstanceElement> GetConnexions()
        {

            List<UtilETWeb.MyConfigSection.MyConfigInstanceElement> l = new List<MyConfigSection.MyConfigInstanceElement>();
            MyConfigSection config = ConfigurationManager.GetSection("MyConnection") as MyConfigSection;
            l = config.Instances.OfType<UtilETWeb.MyConfigSection.MyConfigInstanceElement>().OrderBy(c=> c.Name).ToList();

            UtilETWeb.MyConfigSection.MyConfigInstanceElement item = new UtilETWeb.MyConfigSection.MyConfigInstanceElement();
            item.Name = "(sin especificar)";
            item.Code = "-1";
            l.Insert(0, item);

            return l;
        }


        delegate object GetCustomMethodCallback<T>(T cmb, string _Method) where T : ComboBoxIcon;

        public object GetCustomMethod<T>(T cmb, string _Method) where T : ComboBoxIcon
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (cmb.InvokeRequired)
            {
                //GetCustomMethodCallback<T> d = new GetCustomMethodCallback<T>(GetCustomMethod<T>);
                //return this.Invoke(d, new object[] { cmb, _Method });

                Type typeParameterType = this.GetType();
                MethodInfo MethodInf = typeParameterType.GetMethod("GetCustomMethod");
                MethodInfo generic = MethodInf.MakeGenericMethod(typeof(ComboBoxIcon));
                return generic.Invoke(this, new object[] { cmb, _Method });

            }
            else
            {
                Type typeParameterType = typeof(T);
                MethodInfo MethodInf = typeParameterType.GetMethod(_Method);
                MethodInfo generic = MethodInf.MakeGenericMethod(typeParameterType);
                return generic.Invoke(cmb, new object[] { });

            }
        }


        delegate object GetTextCallback(ComboBox cmb);

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


        private object GetSelectedItem(ComboBox cmb)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (cmb.InvokeRequired)
            {
                GetTextCallback d = new GetTextCallback(GetSelectedItem);
                return this.Invoke(d, new object[] { cmb });
            }
            else
            {
                return cmb.SelectedItem;
            }
        }


        private string _ConnectionString;
        private string ConnectionString
        {
            get
            {
                if (GetText(this.cmbDatabase).ToString().Equals("-1"))
                {
                    return System.Configuration.ConfigurationManager.AppSettings["ConnectionStringDMO"];
                }
                else
                    return GetText(this.cmbDatabase).ToString();
            }
        }



        private object EncodingSelect
        {
            get
            {
                return GetSelectedItem(this.comboBox1);
            }
        }



        public class SqlFile
        {
            public string Name { get; set; }
            public string RealName { get; set; }
        }

        private List<SqlFile> listsp
        {
            get
            {
                List<SqlFile> query;
                List<string> l = new List<string>() { };
                string input = this.richTextBox1.Text;
                string pattern = @"\[([^\[]*)\]";

                if (this.checkBox1.Checked)
                {
                    query = input
                        .Split("\n\r".ToCharArray())
                        .CustomSort()
                        .ToList()
                        .Select(s => new SqlFile()
                        {
                            Name = s,
                            RealName = Regex.Match(s, pattern).Value.Trim("[]".ToCharArray())
                        }).ToList();
                }
                else
                {
                    query = input
                        .Split("\n".ToCharArray())
                        .Select((s, index) => new SqlFile()
                        {
                            Name = string.Format("{1}-[{0}].sql", s, index.ToString("0000")),
                            RealName = s
                        }).ToList();
                }

                return query;

            }
        }

        public static void Empty(string directory)
        {
            foreach (string fileToDelete in System.IO.Directory.GetFiles(directory))
            {
                FileOperationAPIWrapper.MoveToRecycleBin(fileToDelete);
                System.IO.File.Delete(fileToDelete);
            }
            foreach (string subDirectoryToDeleteToDelete in System.IO.Directory.GetDirectories(directory))
            {
                FileOperationAPIWrapper.MoveToRecycleBin(subDirectoryToDeleteToDelete);
                System.IO.Directory.Delete(subDirectoryToDeleteToDelete, true);
            }
        }

        private BackgroundWorker bgwProcedure = new BackgroundWorker();

        private void Procedure_Click_Original(object sender, EventArgs e)
        {
            Empty(Path.Combine(Environment.CurrentDirectory, @"Scripts"));

            //System.IO.Directory.Delete(Path.Combine(Environment.CurrentDirectory, @"Scripts"), false);
            //System.IO.Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, @"Scripts"));

            //string value = string.Join("|", this.listsp.Select(x => string.Format("{0}-{1}", x.Name, x.RealName)).ToArray());
            //MessageBox.Show(value);

            using (SqlConnection sqlConn = new SqlConnection(this.ConnectionString))
            {
                sqlConn.Open();
                ServerConnection srvConn = new ServerConnection(sqlConn);
                Server srv = new Server(srvConn);
                Database db = srv.Databases[srvConn.DatabaseName];
                foreach (StoredProcedure sp in db.StoredProcedures.OfType<StoredProcedure>().Where(sp => this.listsp.Any(s => s.RealName.ToUpper().Equals(sp.Name.ToUpper()))))
                {
                    string myFileName = this.listsp.FirstOrDefault(d => d.RealName.ToUpper().Equals(sp.Name.ToUpper())).Name;
                    using (TextWriter tw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, string.Format(@"Scripts\{0}", myFileName)), false, (Encoding)this.comboBox1.SelectedItem))
                    {
                        //foreach (string sqlScript in sp.Script())
                        //{
                        //    tw.WriteLine(sqlScript);
                        //}

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

                        /////end
                    }
                }
            }

            Process.Start("explorer.exe", Path.Combine(Environment.CurrentDirectory, @"Scripts"));
        }

        private void Trigger_Click(object sender, EventArgs e)
        {
            Empty(Path.Combine(Environment.CurrentDirectory, @"Scripts"));

            //string value = string.Join("|", this.listsp.Select(x => string.Format("{0}-{1}", x.Name, x.RealName)).ToArray());
            //MessageBox.Show(value);

            using (SqlConnection sqlConn = new SqlConnection(this.ConnectionString))
            {
                sqlConn.Open();
                ServerConnection srvConn = new ServerConnection(sqlConn);
                Server srv = new Server(srvConn);
                Database db = srv.Databases[srvConn.DatabaseName];

                foreach (var itemTriggerName in this.listsp)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("SELECT tbl.name");
                    sb.AppendLine("FROM	  sysobjects tr");
                    sb.AppendLine("inner join sysobjects tbl");
                    sb.AppendLine("ON tbl.id = tr.parent_obj");
                    sb.AppendFormat("WHERE  tr.name like '{0}'", itemTriggerName.RealName);

                    DataTable dt = Querys.CommandTextDataTable(sb.ToString(), this.ConnectionString);
                    if (dt.Rows.Count == 0)
                        continue;

                    foreach (Table tb in db.Tables.OfType<Table>().Where(t => t.Name == dt.Rows[0].Field<string>("name")))
                    {
                        foreach (Trigger sp in tb.Triggers.OfType<Trigger>().Where(tr => tr.Name == itemTriggerName.RealName))
                        {
                            string myFileName = this.listsp.FirstOrDefault(d => d.RealName.ToUpper().Equals(sp.Name.ToUpper())).Name;
                            using (TextWriter tw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, string.Format(@"Scripts\{0}", myFileName)), false, (Encoding)this.comboBox1.SelectedItem))
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

                                /////end
                            }
                        }
                    }
                }
            }


            Process.Start("explorer.exe", Path.Combine(Environment.CurrentDirectory, @"Scripts"));
        }


        private BackgroundWorker bgwTable = new BackgroundWorker();
        private void Table_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (!bgwTable.IsBusy)
            {
                Empty(Path.Combine(Environment.CurrentDirectory, @"Scripts"));
                btn.Enabled = false;
                progressBar1.Value = 0;
                bgwTable.RunWorkerAsync(listsp);
            }
        }

        private void Table_Click_Original(object sender, EventArgs e)
        {
            Empty(Path.Combine(Environment.CurrentDirectory, @"Scripts"));

            //string value = string.Join("|", this.listsp.Select(x => string.Format("{0}-{1}", x.Name, x.RealName)).ToArray());
            //MessageBox.Show(value);

            using (SqlConnection sqlConn = new SqlConnection(this.ConnectionString))
            {
                sqlConn.Open();
                ServerConnection srvConn = new ServerConnection(sqlConn);
                Server srv = new Server(srvConn);
                Database db = srv.Databases[srvConn.DatabaseName];
                foreach (Table sp in db.Tables.OfType<Table>().Where(sp => this.listsp.Any(s => s.RealName.ToUpper().Equals(sp.Name.ToUpper()))))
                {
                    string myFileName = this.listsp.FirstOrDefault(d => d.RealName.ToUpper().Equals(sp.Name.ToUpper())).Name;
                    using (TextWriter tw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, string.Format(@"Scripts\{0}", myFileName)), false, (Encoding)this.comboBox1.SelectedItem))
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



                        options.Triggers = true;
                        options.IncludeIfNotExists = true;

                        foreach (string sqlScript in sp.Script(options))
                        {
                            tw.WriteLine(sqlScript);
                            tw.WriteLine("GO");
                        }
                        /////end
                    }

                }
            }


            Process.Start("explorer.exe", Path.Combine(Environment.CurrentDirectory, @"Scripts"));
        }

        private void View_Click(object sender, EventArgs e)
        {
            Empty(Path.Combine(Environment.CurrentDirectory, @"Scripts"));

            //string value = string.Join("|", this.listsp.Select(x => string.Format("{0}-{1}", x.Name, x.RealName)).ToArray());
            //MessageBox.Show(value);

            using (SqlConnection sqlConn = new SqlConnection(this.ConnectionString))
            {
                sqlConn.Open();
                ServerConnection srvConn = new ServerConnection(sqlConn);
                Server srv = new Server(srvConn);
                Database db = srv.Databases[srvConn.DatabaseName];
                foreach (Microsoft.SqlServer.Management.Smo.View sp in db.Views.OfType<Microsoft.SqlServer.Management.Smo.View>().Where(sp => this.listsp.Any(s => s.RealName.ToUpper().Equals(sp.Name.ToUpper()))))
                {
                    string myFileName = this.listsp.FirstOrDefault(d => d.RealName.ToUpper().Equals(sp.Name.ToUpper())).Name;
                    using (TextWriter tw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, string.Format(@"Scripts\{0}", myFileName)), false, (Encoding)this.comboBox1.SelectedItem))
                    {
                        //foreach (string sqlScript in sp.Script())
                        //{
                        //    tw.WriteLine(sqlScript);
                        //}

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

                        /////end
                    }
                }
            }

            Process.Start("explorer.exe", Path.Combine(Environment.CurrentDirectory, @"Scripts"));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Empty(Path.Combine(Environment.CurrentDirectory, @"Scripts"));

            //System.IO.Directory.Delete(Path.Combine(Environment.CurrentDirectory, @"Scripts"), false);
            //System.IO.Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, @"Scripts"));

            //string value = string.Join("|", this.listsp.Select(x => string.Format("{0}-{1}", x.Name, x.RealName)).ToArray());
            //MessageBox.Show(value);

            using (SqlConnection sqlConn = new SqlConnection(this.ConnectionString))
            {
                sqlConn.Open();
                ServerConnection srvConn = new ServerConnection(sqlConn);
                Server srv = new Server(srvConn);
                Database db = srv.Databases[srvConn.DatabaseName];
                foreach (UserDefinedFunction sp in db.UserDefinedFunctions.OfType<UserDefinedFunction>().Where(sp => this.listsp.Any(s => s.RealName.ToUpper().Equals(sp.Name.ToUpper()))))
                {
                    string myFileName = this.listsp.FirstOrDefault(d => d.RealName.ToUpper().Equals(sp.Name.ToUpper())).Name;
                    using (TextWriter tw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, string.Format(@"Scripts\{0}", myFileName)), false, (Encoding)this.comboBox1.SelectedItem))
                    {
                        //foreach (string sqlScript in sp.Script())
                        //{
                        //    tw.WriteLine(sqlScript);
                        //}

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

                        /////end
                    }
                }
            }

            Process.Start("explorer.exe", Path.Combine(Environment.CurrentDirectory, @"Scripts"));
        }


        private void richTextBox1_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void richTextBox1_MouseLeave(object sender, EventArgs e)
        {
            tip.Hide(richTextBox1);
        }

        private void richTextBox1_MouseHover(object sender, EventArgs e)
        {
            tip.Show("Type Or Paste Your Text...", richTextBox1, 0, -20);
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
            SplitterPanel panel = sender as SplitterPanel;
            Bitmap bmp = Properties.Resources.unnamed;
            PointF pnt = new PointF()
            {
                X = panel.Width - bmp.Width,
                Y = panel.Height - bmp.Height
            };
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            g.DrawImage(bmp, pnt);
        }


        private void button5_Click(object sender, EventArgs e)
        {
            Empty(Path.Combine(Environment.CurrentDirectory, @"Scripts"));

            //string value = string.Join("|", this.listsp.Select(x => string.Format("{0}-{1}", x.Name, x.RealName)).ToArray());
            //MessageBox.Show(value);

            using (SqlConnection sqlConn = new SqlConnection(this.ConnectionString))
            {
                sqlConn.Open();
                ServerConnection srvConn = new ServerConnection(sqlConn);
                Server srv = new Server(srvConn);
                Database db = srv.Databases[srvConn.DatabaseName];
                foreach (Table sp in db.Tables.OfType<Table>().Where(sp => this.listsp.Any(s => s.RealName.ToUpper().Equals(sp.Name.ToUpper()))))
                {
                    string myFileName = this.listsp.FirstOrDefault(d => d.RealName.ToUpper().Equals(sp.Name.ToUpper())).Name;
                    using (TextWriter tw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, string.Format(@"Scripts\{0}", myFileName)), false, (Encoding)this.comboBox1.SelectedItem))
                    {

                        ScriptingOptions options = new ScriptingOptions();
                        options.ClusteredIndexes = false;
                        options.Default = false;

                        //CONSTRAINTS
                        options.DriAll = true;
                        options.ScriptDrops = false;


                        options.Indexes = false;
                        options.IncludeHeaders = false;
                        options.NoCollation = false;
                        options.AnsiFile = false;
                        options.AnsiPadding = false;
                        options.SchemaQualifyForeignKeysReferences = false;

                        options.Triggers = false;
                        options.IncludeIfNotExists = true;

                        foreach (string sqlScript in sp.Script(options))
                        {
                            tw.WriteLine(sqlScript);
                            tw.WriteLine("GO");
                        }
                        /////end
                    }

                }
            }


            Process.Start("explorer.exe", Path.Combine(Environment.CurrentDirectory, @"Scripts"));

        }

        private void DefaultConstraint(object sender, EventArgs e)
        {
            Empty(Path.Combine(Environment.CurrentDirectory, @"Scripts"));

            //System.IO.Directory.Delete(Path.Combine(Environment.CurrentDirectory, @"Scripts"), false);
            //System.IO.Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, @"Scripts"));

            //string value = string.Join("|", this.listsp.Select(x => string.Format("{0}-{1}", x.Name, x.RealName)).ToArray());
            //MessageBox.Show(value);

            using (SqlConnection sqlConn = new SqlConnection(this.ConnectionString))
            {
                sqlConn.Open();
                ServerConnection srvConn = new ServerConnection(sqlConn);
                Server srv = new Server(srvConn);
                Database db = srv.Databases[srvConn.DatabaseName];
                foreach (Table tbl in db.Tables.OfType<Table>())
                {
                    //foreach (Index idx in tbl.Indexes.OfType<Index>().Where(id => id.IndexKeyType == IndexKeyType.DriPrimaryKey)) 
                    //{
                    //    idx.Script();
                    //}

                    //foreach (ForeignKey FKey in tbl.ForeignKeys) 
                    //{
                    //    FKey.Script();                       
                    //}

                    foreach (Column item in tbl.Columns.OfType<Column>().Where(c => this.listsp.Any(s => c.DefaultConstraint != null && s.RealName.ToLower().Equals(c.DefaultConstraint.Name.ToLower()))))
                    {
                        string myFileName = this.listsp.FirstOrDefault(d => d.RealName.ToUpper().Equals(item.DefaultConstraint.Name.ToUpper())).Name;
                        using (TextWriter tw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, string.Format(@"Scripts\{0}", myFileName)), false, (Encoding)this.comboBox1.SelectedItem))
                        {
                            //foreach (string sqlScript in sp.Script())
                            //{
                            //    tw.WriteLine(sqlScript);
                            //}

                            ScriptingOptions options = new ScriptingOptions();
                            options.ScriptDrops = false;
                            options.IncludeIfNotExists = true;
                            options.ClusteredIndexes = false;
                            options.Default = false;
                            options.DriAll = true;
                            options.Indexes = false;
                            options.IncludeHeaders = false;
                            options.NoCollation = false;
                            options.AnsiFile = false;
                            options.AnsiPadding = false;
                            options.SchemaQualifyForeignKeysReferences = false;
                            foreach (string sqlScript in item.DefaultConstraint.Script(options))
                            {
                                tw.WriteLine(sqlScript);
                                tw.WriteLine("GO");
                            }

                            //ScriptingOptions optionscreate = new ScriptingOptions();
                            ////optionscreate.ScriptDrops = true;
                            ////optionscreate.IncludeIfNotExists = true;
                            //optionscreate.ClusteredIndexes = true;
                            //optionscreate.Default = true;
                            //optionscreate.DriAll = true;
                            ////optionscreate.Indexes = true;
                            //optionscreate.IncludeHeaders = true;
                            //optionscreate.NoCollation = true;
                            //optionscreate.AnsiFile = true;
                            //optionscreate.AnsiPadding = false;
                            //optionscreate.SchemaQualifyForeignKeysReferences = true;
                            //foreach (string sqlScript in sp.Script(optionscreate))
                            //{
                            //    tw.WriteLine(sqlScript);
                            //    tw.WriteLine("GO");
                            //}

                            /////end
                        }

                    }
                }
            }

            Process.Start("explorer.exe", Path.Combine(Environment.CurrentDirectory, @"Scripts"));
        }


        private void TableTypes(object sender, EventArgs e)
        {
            Empty(Path.Combine(Environment.CurrentDirectory, @"Scripts"));

            //System.IO.Directory.Delete(Path.Combine(Environment.CurrentDirectory, @"Scripts"), false);
            //System.IO.Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, @"Scripts"));

            //string value = string.Join("|", this.listsp.Select(x => string.Format("{0}-{1}", x.Name, x.RealName)).ToArray());
            //MessageBox.Show(value);

            using (SqlConnection sqlConn = new SqlConnection(this.ConnectionString))
            {
                sqlConn.Open();
                ServerConnection srvConn = new ServerConnection(sqlConn);
                Server srv = new Server(srvConn);
                Database db = srv.Databases[srvConn.DatabaseName];
                //foreach (UserDefinedTableType userTableType in db.UserDefinedTableTypes.OfType<UserDefinedTableType>())                
                foreach (UserDefinedTableType userTableType in db.UserDefinedTableTypes.OfType<UserDefinedTableType>().Where(sp => this.listsp.Any(s => s.RealName.ToUpper().Equals(sp.Name.ToUpper()))))
                {

                    string myFileName = this.listsp.FirstOrDefault(d => d.RealName.ToUpper().Equals(userTableType.Name.ToUpper())).Name;
                    using (TextWriter tw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, string.Format(@"Scripts\{0}", myFileName)), false, (Encoding)this.comboBox1.SelectedItem))
                    {

                        ScriptingOptions options = new ScriptingOptions();
                        options.Default = true;
                        options.DriAll = true;
                        options.NoCollation = true;
                        options.AnsiFile = true;
                        options.AnsiPadding = false;

                        //options.ScriptDrops = true;
                        options.IncludeIfNotExists = true;


                        foreach (string sqlScript in userTableType.Script(options))
                        {
                            tw.WriteLine(sqlScript);
                            tw.WriteLine("GO");
                        }
                        /////end
                    }

                }
            }

            Process.Start("explorer.exe", Path.Combine(Environment.CurrentDirectory, @"Scripts"));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            TableTypes(sender, e);
        }

        private void AlterColumns(object sender, EventArgs e)
        {
            Empty(Path.Combine(Environment.CurrentDirectory, @"Scripts"));

            //System.IO.Directory.Delete(Path.Combine(Environment.CurrentDirectory, @"Scripts"), false);
            //System.IO.Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, @"Scripts"));

            //string value = string.Join("|", this.listsp.Select(x => string.Format("{0}-{1}", x.Name, x.RealName)).ToArray());
            //MessageBox.Show(value);

            using (SqlConnection sqlConn = new SqlConnection(this.ConnectionString))
            {
                sqlConn.Open();
                ServerConnection srvConn = new ServerConnection(sqlConn);
                Server srv = new Server(srvConn);
                Database db = srv.Databases[srvConn.DatabaseName];
                foreach (Table userTable in db.Tables.OfType<Table>().Where(sp => this.listsp.Any(s => s.RealName.ToUpper().Equals(sp.Name.ToUpper()))))
                {

                    string myFileName = this.listsp.FirstOrDefault(d => d.RealName.ToUpper().Equals(userTable.Name.ToUpper())).Name;
                    using (TextWriter tw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, string.Format(@"Scripts\{0}", myFileName)), false, (Encoding)this.comboBox1.SelectedItem))
                    {

                        tw.WriteLine($"---- Script de columnas ({userTable.Name}) ----");
                        // PRIMARY KEYS
                        foreach (Index idx in userTable.Indexes)
                        {
                            if (idx.IndexKeyType == IndexKeyType.DriPrimaryKey)
                                tw.WriteLine($"-- PRIMARY KEY: {idx.Name}");
                        }

                        // FOREIGN KEYS
                        foreach (ForeignKey fk in userTable.ForeignKeys)
                        {
                            tw.WriteLine($"-- FOREIGN KEY: {fk.Name} REFERENCES {fk.ReferencedTable}");
                        }

                        tw.WriteLine("-- ======= COLUMNAS =======");
                        foreach (Column col in userTable.Columns)
                        {
                            string dataType = col.DataType.Name.ToLower();
                            string typeWithPrecision = GetTypeWithPrecision(col);
                            string nullability = col.Nullable ? "NULL" : "NOT NULL";

                            string alter = string.Empty;
                            alter += $@"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'{col.Name}' AND Object_ID = Object_ID(N'[{userTable.Schema}].[{userTable.Name}]'))";
                            alter += $@"
BEGIN";
                            if (!col.InPrimaryKey && !col.Nullable)
                            {

                                if (col.DefaultConstraint != null)
                                {
                                    alter += $@"
    -- Agregar como NULL temporalmente para evitar error si hay filas existentes
    ALTER TABLE [{userTable.Schema}].[{userTable.Name}] ADD [{col.Name}] {typeWithPrecision};";
                                    string defText = col.DefaultConstraint.Text;
                                    alter += $@"
    -- Rellenar los valores nulos con def
    UPDATE [{userTable.Schema}].[{userTable.Name}] SET [{col.Name}] = {defText} WHERE [{col.Name}] IS NULL;
    -- Cambiar a NOT NULL";
                                }
                            }

                            alter += $@"
    ALTER TABLE [{userTable.Schema}].[{userTable.Name}] ADD [{col.Name}] {typeWithPrecision} {nullability};";

                            alter += $@"
END";

                            tw.WriteLine(alter);
                        }

                        tw.WriteLine("-- ======= DEFAULT CONSTRAINTS =======");
                        foreach (Column col in userTable.Columns)
                        {
                            if (col.DefaultConstraint != null)
                            {
                                string defName = col.DefaultConstraint.Name;
                                string defText = col.DefaultConstraint.Text;
                                string alter = $@"IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE Name = N'{defName}' AND Parent_Object_ID = Object_ID(N'[{userTable.Schema}].[{userTable.Name}]'))" +
$@"BEGIN
    ALTER TABLE [{userTable.Schema}].[{userTable.Name}] ADD CONSTRAINT [{defName}] DEFAULT ({defText}) FOR [{col.Name}];
END";
                                tw.WriteLine(alter);
                            }
                        }

                        tw.WriteLine("-- ======= CHECK CONSTRAINTS =======");
                        foreach (Check chk in userTable.Checks)
                        {
                            string alter = $@"IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE Name = N'{chk.Name}' AND Parent_Object_ID = Object_ID(N'[{userTable.Schema}].[{userTable.Name}]'))" +
$@"BEGIN
    ALTER TABLE [{userTable.Schema}].[{userTable.Name}] ADD CONSTRAINT [{chk.Name}] CHECK {chk.Text};
END";
                            tw.WriteLine(alter);
                        }

                        tw.WriteLine("-- ======= FOREIGN KEYS =======");
                        foreach (ForeignKey fk in userTable.ForeignKeys)
                        {
                            string cols = string.Join(",", fk.Columns.Cast<ForeignKeyColumn>().Select(c => $"[{c.Name}]"));
                            string refCols = string.Join(",", fk.Columns.Cast<ForeignKeyColumn>().Select(c => $"[{c.ReferencedColumn}]"));
                            string alter = $@"IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE Name = N'{fk.Name}' AND Parent_Object_ID = Object_ID(N'[{userTable.Schema}].[{userTable.Name}]'))" +
$@"BEGIN
    ALTER TABLE [{userTable.Schema}].[{userTable.Name}] ADD CONSTRAINT [{fk.Name}] FOREIGN KEY ({cols}) REFERENCES [{fk.ReferencedTableSchema}].[{fk.ReferencedTable}] ({refCols});
END";
                            tw.WriteLine(alter);

                        }
                        /////end
                    }
                }


                Process.Start("explorer.exe", Path.Combine(Environment.CurrentDirectory, @"Scripts"));
            }
        }



        private void button7_Click(object sender, EventArgs e)
        {
            AlterColumns(sender, e);
        }

        // Función auxiliar: genera tipo con longitud/precisión
        static string GetTypeWithPrecision(Column col)
        {
            var dt = col.DataType;
            string type = dt.Name.ToLower();

            if (dt.SqlDataType == SqlDataType.VarChar ||
                dt.SqlDataType == SqlDataType.NVarChar ||
                dt.SqlDataType == SqlDataType.Char ||
                dt.SqlDataType == SqlDataType.NChar)
            {
                return $"{type}({(dt.MaximumLength == -1 ? "MAX" : dt.MaximumLength.ToString())})";
            }

            if (dt.SqlDataType == SqlDataType.Decimal ||
                dt.SqlDataType == SqlDataType.Numeric)
            {
                return $"{type}({dt.NumericPrecision},{dt.NumericScale})";
            }

            // Para los demás tipos no necesitan precisión
            return type;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlConn = new SqlConnection(this.ConnectionString))
            {
                sqlConn.Open();
                ServerConnection srvConn = new ServerConnection(sqlConn);
                Server srv = new Server(srvConn);
                Database db = srv.Databases[srvConn.DatabaseName];
                foreach (Table userTable in db.Tables.OfType<Table>().Where(sp => this.listsp.Any(s => s.RealName.ToUpper().Equals(sp.Name.ToUpper()))))
                {
                    string myFileName = this.listsp.FirstOrDefault(d => d.RealName.ToUpper().Equals(userTable.Name.ToUpper())).Name;
                    using (TextWriter tw = new StreamWriter(Path.Combine(Environment.CurrentDirectory, string.Format(@"Scripts\{0}", myFileName)), false, (Encoding)this.comboBox1.SelectedItem))
                    {
                        tw.Write(GenerateInsertsWithIdentity(this.ConnectionString, "dbo", userTable.Name));
                    }
                }
            }

            Process.Start("explorer.exe", Path.Combine(Environment.CurrentDirectory, @"Scripts"));
        }

        static string GenerateInsertsWithIdentity(string connectionString, string schema, string tableName)
        {
            StringBuilder sb = new StringBuilder();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Conexión SMO para detectar Identity y columnas
                Server server = new Server(new ServerConnection(conn));
                Table table = server.Databases[conn.Database].Tables[tableName, schema];

                bool tieneIdentity = table.Columns.Cast<Column>().Any(c => c.Identity);

                // Obtener datos
                SqlCommand cmd = new SqlCommand($"SELECT * FROM [{schema}].[{tableName}]", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (tieneIdentity)
                {
                    sb.AppendLine($"SET IDENTITY_INSERT [{schema}].[{tableName}] ON;");
                    sb.AppendLine();
                }

                int count = 0;
                foreach (DataRow row in dt.Rows)
                {
                    StringBuilder columns = new StringBuilder();
                    StringBuilder values = new StringBuilder();

                    var cols = table.Columns.Cast<Column>().ToList();

                    for (int i = 0; i < cols.Count; i++)
                    {
                        var col = cols[i];
                        string colName = col.Name;
                        object value = row[colName];

                        columns.Append($"[{colName}]");
                        if (i < cols.Count - 1) columns.Append(", ");

                        values.Append(GetSqlValue(value, col.DataType));
                        if (i < cols.Count - 1) values.Append(", ");
                    }


                    sb.AppendLine($"INSERT INTO [{schema}].[{tableName}] ({columns}) VALUES ({values});");
                    count++;
                    if (count % 50 == 0)
                        sb.AppendLine($"GO");


                }

                if (tieneIdentity)
                {
                    sb.AppendLine();
                    sb.AppendLine($"SET IDENTITY_INSERT [{schema}].[{tableName}] OFF;");
                }
            }

            return sb.ToString();
        }

        // Convierte valores de .NET a SQL correctamente
        static string GetSqlValue(object value, Microsoft.SqlServer.Management.Smo.DataType dataType)
        {
            if (value == DBNull.Value)
                return "NULL";

            Type type = value.GetType();

            if (type == typeof(string) || type == typeof(char))
                return $"'{value.ToString().Replace("'", "''")}'";

            if (type == typeof(DateTime))
                return $"'{((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff")}'";

            if (type == typeof(bool))
                return (bool)value ? "1" : "0";

            if (type == typeof(Guid))
                return $"'{value}'";

            if (type == typeof(byte[]))
                return "0x" + BitConverter.ToString((byte[])value).Replace("-", "");

            // Decimal, int, double, float, etc.
            return Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}

public static class MyExtensions
{
    public static IEnumerable<string> CustomSort(this IEnumerable<string> list)
    {
        int maxLen = list.Select(s => s.Length).Max();

        return list.Select(s => new
        {
            OrgStr = s,
            SortStr = Regex.Replace(s, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, char.IsDigit(m.Value[0]) ? ' ' : '\xffff'))
        })
        .OrderBy(x => x.SortStr)
        .Select(x => x.OrgStr);
    }

}
