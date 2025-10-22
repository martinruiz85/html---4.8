using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;

namespace UtilETWeb
{
    public partial class frmGenerateScriptsETWeb : Form
    {
        public frmGenerateScriptsETWeb()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmGenerateScriptsETWeb_Load);
        }

        private List<UtilETWeb.MyConfigSection.MyConfigInstanceElement> GetConnexions()
        {

            List<UtilETWeb.MyConfigSection.MyConfigInstanceElement> l = new List<MyConfigSection.MyConfigInstanceElement>();
            MyConfigSection config = ConfigurationManager.GetSection("MyConnection") as MyConfigSection;
            l = config.Instances.OfType<UtilETWeb.MyConfigSection.MyConfigInstanceElement>().ToList();

            UtilETWeb.MyConfigSection.MyConfigInstanceElement item = new UtilETWeb.MyConfigSection.MyConfigInstanceElement();
            item.Name = "(sin especificar)";
            item.Code = "-1";
            l.Insert(0, item);

            return l;
        }

        private object EncodingSelect
        {
            get
            {
                return GetSelectedItem(this.comboBox1);
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


        void frmGenerateScriptsETWeb_Load(object sender, EventArgs e)
        {
            this.cmbDatabase.DataSource = GetConnexions();
            this.cmbDatabase.ValueMember = "Code";
            this.cmbDatabase.DisplayMember = "Name";
            this.cmbDatabase.SelectedValue = "-1";

            this.cmbDatabase.SelectedValue = this.ConnectionString;

            List<Encoding> l = new List<Encoding>()
            {
                Encoding.UTF8,
                //Encoding.GetEncoding(1252),
                Encoding.Unicode
            };

            this.comboBox1.DisplayMember = "BodyName";
            this.comboBox1.DataSource = l;
            this.comboBox1.SelectedIndex = 0;


            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.btnGenerate.Enabled = true;
            Process.Start("explorer.exe", Path.Combine(Environment.CurrentDirectory, @"Scripts"));
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            List<UtilETWeb.frmGenerateProcedures.SqlFile> _listsp = e.Argument as List<UtilETWeb.frmGenerateProcedures.SqlFile>;

            foreach (UtilETWeb.frmGenerateProcedures.SqlFile item in _listsp)
            {
                Random r = new Random();
                //Thread.Sleep(1000 * r.Next(0, 5));
                Guid tree;
                string filename = item.Name;
                if (item.RealName.TryParseGuid(out tree))
                {
                    SqlHandlerTree sqltree = new SqlHandlerTree();
                    sqltree.ObjectName = item.RealName;
                    sqltree.FileName = Path.Combine(Environment.CurrentDirectory, string.Format(@"Scripts\{0}", item.Name));
                    sqltree.ConnectionString = this.ConnectionString;
                    sqltree.EncodingFile = (Encoding)EncodingSelect;
                    sqltree.ProcessData();
                }
                else
                {
                    SqlHandlerObject sqlobject = new SqlHandlerObject();
                    sqlobject.ObjectName = item.RealName;
                    sqlobject.FileName = Path.Combine(Environment.CurrentDirectory, string.Format(@"Scripts\{0}", item.Name));
                    sqlobject.ConnectionString = this.ConnectionString;
                    sqlobject.EncodingFile = (Encoding)EncodingSelect;
                    sqlobject.ProcessData();
                }

                backgroundWorker1.ReportProgress((int)(((_listsp.IndexOf(item) + 1) / (float)_listsp.Count()) * 100));
            }
        }


        private List<UtilETWeb.frmGenerateProcedures.SqlFile> listsp
        {
            get
            {
                List<UtilETWeb.frmGenerateProcedures.SqlFile> query = new List<frmGenerateProcedures.SqlFile>();
                List<string> l = new List<string>() { };
                string input = this.txtResult.Text;
                string pattern = @"\[([^\[]*)\]";

                if (string.IsNullOrEmpty(input))
                    return query;

                if (this.checkBox1.Checked)
                {
                    query = input
                        .Split("\n".ToCharArray())
                        .CustomSort()
                        .ToList()
                        .Select(s => new UtilETWeb.frmGenerateProcedures.SqlFile()
                        {
                            Name = s,
                            RealName = Regex.Match(s, pattern).Value.Trim("[]".ToCharArray())
                        }).ToList();
                }
                else
                {
                    query = input
                        .Split("\n".ToCharArray())
                        .Select((s, index) => new UtilETWeb.frmGenerateProcedures.SqlFile()
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
                System.IO.File.Delete(fileToDelete);
            }
            foreach (string subDirectoryToDeleteToDelete in System.IO.Directory.GetDirectories(directory))
            {
                System.IO.Directory.Delete(subDirectoryToDeleteToDelete, true);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (!backgroundWorker1.IsBusy)
            {
                btn.Enabled = false;
                this.progressBar1.Value = 0;
                Empty(Path.Combine(Environment.CurrentDirectory, @"Scripts"));
                this.backgroundWorker1.RunWorkerAsync(this.listsp);
            }
        }

        private void btnGenerate_Click_Original(object sender, EventArgs e)
        {
            Empty(Path.Combine(Environment.CurrentDirectory, @"Scripts"));

            foreach (UtilETWeb.frmGenerateProcedures.SqlFile item in this.listsp)
            {
                Random r = new Random();
                //Thread.Sleep(1000 * r.Next(0, 5));
                Guid tree;
                string filename = item.Name;
                if (item.RealName.TryParseGuid(out tree))
                {
                    SqlHandlerTree sqltree = new SqlHandlerTree();
                    sqltree.ObjectName = item.RealName;
                    sqltree.FileName = Path.Combine(Environment.CurrentDirectory, string.Format(@"Scripts\{0}", item.Name));
                    sqltree.ConnectionString = this.ConnectionString;
                    sqltree.EncodingFile = (Encoding)this.comboBox1.SelectedItem;
                    sqltree.ProcessData();
                }
                else
                {
                    SqlHandlerObject sqlobject = new SqlHandlerObject();
                    sqlobject.ObjectName = item.RealName;
                    sqlobject.FileName = Path.Combine(Environment.CurrentDirectory, string.Format(@"Scripts\{0}", item.Name));
                    sqlobject.ConnectionString = this.ConnectionString;
                    sqlobject.EncodingFile = (Encoding)this.comboBox1.SelectedItem;
                    sqlobject.ProcessData();
                }
            }

            Process.Start("explorer.exe", Path.Combine(Environment.CurrentDirectory, @"Scripts"));
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

        private void btnClean_Click(object sender, EventArgs e)
        {
            Empty(Path.Combine(Environment.CurrentDirectory, @"Scripts"));
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

    }

    public class SqlHandlerObject : SqlHandlerTree
    {
        public override void ProcessData()
        {
            File.WriteAllText(FileName, String.Empty);
            List<SqlParameter> l = new List<SqlParameter>();
            l.Add(new SqlParameter("@vstrObjectName", ObjectName));
            using (SqlConnection c = new SqlConnection(this.ConnectionString))
            {
                c.InfoMessage += new SqlInfoMessageEventHandler(OnInfoMessage);
                using (SqlCommand cmd = new SqlCommand("dbo.sp_zScriptObject", c) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.AddRange(l.ToArray());
                    c.Open();
                    cmd.CommandTimeout = 600;
                    SqlDataReader result = cmd.ExecuteReader();
                    do
                    {
                        while (result.Read())
                        {
                            TextWriter sw = new StreamWriter(FileName, true, this.EncodingFile);
                            string text = result[0].ToString();
                            sw.WriteLine(text);
                            sw.Close();
                        }
                    }
                    while (result.NextResult());
                }
            }
        }
    }

    public class SqlHandlerTree
    {
        public string FileName { get; set; }
        public string ObjectName { get; set; }
        public string ConnectionString { get; set; }
        public Encoding EncodingFile { get; set; }
        // or get it from AppConfig, as noted by FirebladeDan

        public virtual void ProcessData()
        {
            File.WriteAllText(FileName, String.Empty);
            List<SqlParameter> l = new List<SqlParameter>();
            l.Add(new SqlParameter("@vridTreeGuid", ObjectName));
            using (SqlConnection c = new SqlConnection(this.ConnectionString))
            {
                c.InfoMessage += new SqlInfoMessageEventHandler(OnInfoMessage);
                using (SqlCommand cmd = new SqlCommand("dbo.sp_zScriptTreeByGuid", c) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.AddRange(l.ToArray());
                    c.Open();
                    cmd.CommandTimeout = 600;
                    SqlDataReader result = cmd.ExecuteReader();
                    do
                    {
                        while (result.Read())
                        {
                            TextWriter sw = new StreamWriter(FileName, true, this.EncodingFile);
                            string text = result[0].ToString();
                            sw.WriteLine(text);
                            sw.Close();
                        }
                    }
                    while (result.NextResult());
                }
            }
        }

        public void OnInfoMessage(object sender, SqlInfoMessageEventArgs args)
        {
            TextWriter sw = new StreamWriter(this.FileName, true);
            sw.WriteLine(args.Message);
            sw.Close();
        }
    }
}
