using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Diagnostics;

namespace UtilETWeb
{
    public partial class frmRoles : Form
    {
        private System.ComponentModel.BackgroundWorker backgroundWorker1;

        public frmRoles()
        {
            InitializeComponent();
            this.backgroundWorker1 = new BackgroundWorker();
            this.Load += new EventHandler(frmRoles_Load);
        }

        void frmRoles_Load(object sender, EventArgs e)
        {
            MyConfigSection config = ConfigurationManager.GetSection("MyConnection") as MyConfigSection;

            this.comboBox1.DataSource = config.Instances.OfType<UtilETWeb.MyConfigSection.MyConfigInstanceElement>().ToList();
            this.comboBox1.ValueMember = "Code";
            this.comboBox1.DisplayMember = "Name";

            this.backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);

        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result.ToString().Equals("OK"))
                this.button1.Image = UtilETWeb.Properties.Resources.work;
            else
                this.button1.Image = UtilETWeb.Properties.Resources.exclamation;

            //this.button1.Enabled = true;

            Process.Start("explorer.exe", Path.Combine(Environment.CurrentDirectory, @"Scripts"));
        }

        public int RolID { get; set; }

        void myConnection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            string FileName = Path.Combine(Environment.CurrentDirectory, string.Format(@"Scripts\Rol_{0}.txt", this.RolID));
            TextWriter sw = new StreamWriter(FileName, true,Encoding.UTF8);
            sw.WriteLine(e.Message);
            sw.Close();
        }


        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
               
                int[] roles = new int[] { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 22, 25 };
                for (int i = 0; i < roles.Length; i++)
                {
                    this.RolID = roles[i];

                    string FileName = Path.Combine(Environment.CurrentDirectory, string.Format(@"Scripts\Rol_{0}.txt", roles[i]));

                    File.WriteAllText(FileName, String.Empty);
                    List<SqlParameter> l = new List<SqlParameter>();
                    l.Add(new SqlParameter("@intRolID", roles[i]));
                    using (SqlConnection c = new SqlConnection(@"Data Source=VXMTYMXINTDEV01\INTSQLDEV01;Initial Catalog=IndicadoresGRI;Integrated Security=False;Uid=sa;Pwd=chopito;"))
                    {
                        c.InfoMessage += new SqlInfoMessageEventHandler(myConnection_InfoMessage);

                        using (SqlCommand cmd = new SqlCommand("dbo.pfrmGenerateRol", c) { CommandType = CommandType.StoredProcedure })
                        {
                            cmd.Parameters.AddRange(l.ToArray());
                            c.Open();
                            cmd.CommandTimeout = 600;
                            SqlDataReader result = cmd.ExecuteReader();
                            do
                            {
                                while (result.Read())
                                {
                                    TextWriter sw = new StreamWriter(FileName, true, Encoding.UTF8);
                                    string text = result[0].ToString();
                                    sw.WriteLine(text);
                                    sw.Close();
                                }
                            }
                            while (result.NextResult());
                        }

                    }

                }
                e.Result = "OK";
            }
            catch (Exception)
            {
                e.Result = "ERROR";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.backgroundWorker1.IsBusy != true)
            {
                //this.button1.Enabled = false;
                this.button1.Image = UtilETWeb.Properties.Resources.generator;
                this.backgroundWorker1.RunWorkerAsync();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
