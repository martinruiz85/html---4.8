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
    public partial class frmGenerateInserts : Form
    {
        public frmGenerateInserts()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmGenerateInserts_Load);
        }

        void frmGenerateInserts_Load(object sender, EventArgs e)
        {
            this.cmbDatabase.DataSource = GetConnexions();
            this.cmbDatabase.ValueMember = "Code";
            this.cmbDatabase.DisplayMember = "Name";
            this.cmbDatabase.SelectedValue = "-1";

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
                    return System.Configuration.ConfigurationManager.AppSettings["ConnectionStringPRO"];
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
                this.beginProcess();
                e.Result = "OK";
            }
            catch (Exception)
            {
                e.Result = "ERROR";
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (this.backgroundWorker1.IsBusy != true)
            {
                //this.button1.Enabled = false;
                this.btnGenerate.Image = UtilETWeb.Properties.Resources.generator;
                this.backgroundWorker1.RunWorkerAsync();
            }
        }

        string ObjectName
        {
            get
            {
                return this.txtObjectName.Text.Trim();
            }
        }

        String From
        {
            get
            {
                if (string.IsNullOrEmpty(this.txtFrom.Text))
                    return null;
                else
                    return this.txtFrom.Text.Trim();
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
            TextWriter sw = new StreamWriter(this.FileName, true, Encoding.UTF8);
            sw.WriteLine(e.Message);
            sw.Close();
        }

        private void beginProcess()
        {
            Generate();
        }

        private void Generate()
        {
            File.WriteAllText(this.FileName, String.Empty);
            List<SqlParameter> l = new List<SqlParameter>();
            l.Add(new SqlParameter("@table_name", this.ObjectName));
            l.Add(new SqlParameter("@from", this.From));

            using (SqlConnection c = new SqlConnection(this.ConnectionString))
            //using (SqlConnection c = new SqlConnection(@"Data Source=vxmtymxintdev01\intsqldev01;Initial Catalog=ETWebDMO114;Integrated Security=True"))
            {
                c.InfoMessage += new SqlInfoMessageEventHandler(myConnection_InfoMessage);
                using (SqlCommand cmd = new SqlCommand("dbo.sp_generate_inserts", c) { CommandType = CommandType.StoredProcedure })
                {
                    int count = 0;
                    cmd.Parameters.AddRange(l.ToArray());
                    c.Open();
                    cmd.CommandTimeout = 600;
                    SqlDataReader result = cmd.ExecuteReader();
                    do
                    {
                        while (result.Read())
                        {
                            //TextWriter sw = new StreamWriter(this.FileName, true, Encoding.GetEncoding(1252));
                            TextWriter sw = new StreamWriter(this.FileName, true, Encoding.UTF8);
                            string text = result[0].ToString();

                            if (count % 50 == 0) sw.WriteLine("GO");

                            sw.WriteLine(text);
                            sw.Close();
                            count += 1;
                        }
                    }
                    while (result.NextResult());
                }

            }
        }

    }
}
