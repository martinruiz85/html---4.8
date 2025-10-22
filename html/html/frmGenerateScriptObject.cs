using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilETWeb.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;

namespace UtilETWeb
{
    public partial class frmGenerateScriptObject : Form
    {
        public frmGenerateScriptObject()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmGenerateScriptObject_Load);
        }

        void frmGenerateScriptObject_Load(object sender, EventArgs e)
        {
            this.cmbDatabase.DataSource = GetConnexions();
            this.cmbDatabase.ValueMember = "Code";
            this.cmbDatabase.DisplayMember = "Name";
            this.cmbDatabase.SelectedValue = "-1";
            this.cmbDatabase.SelectedValue = this.ConnectionString;

            List<Encoding> l = new List<Encoding>()
            {
                Encoding.UTF8,
                Encoding.GetEncoding(1252)
            };

            this.comboBox1.DisplayMember = "BodyName";
            this.comboBox1.DataSource = l;
            this.comboBox1.SelectedIndex = 0;

            this.backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
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

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result.ToString().Equals("OK"))
            {
                //this.button1.Enabled = true;
                this.btnGenerate.Image = UtilETWeb.Properties.Resources.work;
                if (File.Exists(FileName)) System.Diagnostics.Process.Start(FileName);
            }
            else
            {
                this.btnGenerate.Image = UtilETWeb.Properties.Resources.exclamation;
            }
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                AsynParams asyparam = e.Argument as AsynParams;
                this.beginProcess(asyparam.ObjName, asyparam.Encod);
                e.Result = "OK";
            }
            catch (Exception ex)
            {
                e.Result = "ERROR";
            }
        }

        public class AsynParams 
        {
            public Encoding Encod { get; set; }
            public string ObjName { get; set; }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (this.backgroundWorker1.IsBusy != true)
            {
                //this.button1.Enabled = false;
                this.btnGenerate.Image = UtilETWeb.Properties.Resources.generator;
                this.backgroundWorker1.RunWorkerAsync(new AsynParams()
                {
                    Encod = (Encoding)this.comboBox1.SelectedItem,
                    ObjName = this.ObjectName
                });
            }
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

        void myConnection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            TextWriter sw = new StreamWriter(this.FileName, true);
            sw.WriteLine(e.Message);
            sw.Close();
        }

        private void beginProcess(string ObjectName, Encoding Encod)
        {

            Generate(ObjectName, Encod);
        }

        private void Generate(string ObjectName, Encoding Encod)
        {
            File.WriteAllText(this.FileName, String.Empty);
            List<SqlParameter> l = new List<SqlParameter>();
            l.Add(new SqlParameter("@vstrObjectName", ObjectName));
            using (SqlConnection c = new SqlConnection(this.ConnectionString))
            {
                c.InfoMessage += new SqlInfoMessageEventHandler(myConnection_InfoMessage);
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
                            TextWriter sw = new StreamWriter(this.FileName, true, Encod);
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
}
