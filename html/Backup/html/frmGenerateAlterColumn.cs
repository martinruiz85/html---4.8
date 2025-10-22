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
using UtilETWeb;

namespace html
{
    public partial class frmGenerateAlterColumn : Form
    {
        public frmGenerateAlterColumn()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmGenerateAlterColumn_Load);
        }

        void frmGenerateAlterColumn_Load(object sender, EventArgs e)
        {
            MyConfigSection config = ConfigurationManager.GetSection("MyConnection") as MyConfigSection;

            this.comboBox1.DataSource = config.Instances.OfType<UtilETWeb.MyConfigSection.MyConfigInstanceElement>().ToList();
            this.comboBox1.ValueMember = "Code";
            this.comboBox1.DisplayMember = "Name";

            //foreach (UtilETWeb.MyConfigSection.MyConfigInstanceElement ei in config.Instances)
            //{
            //    Console.WriteLine("Name: {0}, Code: {1}", ei.Name, ei.Code);
            //}

            this.backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result.ToString().Equals("OK"))
            {
                //this.button1.Enabled = true;
                this.button1.Image = UtilETWeb.Properties.Resources.work;
                if (File.Exists(FileName)) System.Diagnostics.Process.Start(FileName);
            }
            else 
            {
                this.button1.Image = UtilETWeb.Properties.Resources.exclamation;
            }
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                TableAndConeccion tc = e.Argument as TableAndConeccion;
                this.beginProcess(tc.Table, tc.Connexion);
                e.Result = "OK";
            }
            catch (Exception)
            {
                e.Result = "ERROR";
            }
            
        }

        string TableName
        {
            get
            {
                return this.txtTableName.Text.Trim();
            }
        }

        string FileName
        {
            get
            {
                return Path.Combine(Environment.CurrentDirectory, string.Format("{0}.txt", this.TableName));
            }
        }

        private class TableAndConeccion 
        {
            public string Table { get; set; }
            public string Connexion { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.backgroundWorker1.IsBusy != true)
            {
                TableAndConeccion tc = new TableAndConeccion();
                tc.Table = this.TableName;
                tc.Connexion = this.comboBox1.SelectedValue as string;

                //this.button1.Enabled = false;
                this.button1.Image = UtilETWeb.Properties.Resources.generator;
                this.backgroundWorker1.RunWorkerAsync(tc);
            }
        }

        void myConnection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            TextWriter sw = new StreamWriter(this.FileName, true);
            sw.WriteLine(e.Message);
            sw.Close();
        }

        private void beginProcess(string TableName, string Connection)
        {
            string ColsNames = "";
            DataTable dt = Querys.ExecDatatable("sp_columns", new List<SqlParameter>() { new SqlParameter("@table_name", TableName) }, Connection);
            if (dt.Rows.Count > 0)
            {
                string[] values = dt.Rows.OfType<DataRow>().Select<DataRow, string>(row => row.Field<string>("COLUMN_NAME")).ToArray();
                ColsNames = string.Join("|", values);
                GenerateAlter(ColsNames, Connection);
            }
            
        }

        private void GenerateAlter(string ColsNames, string Connection)
        {
            File.WriteAllText(this.FileName, String.Empty);
            List<SqlParameter> l = new List<SqlParameter>();
            l.Add(new SqlParameter("@TABLE_NAME", this.TableName));
            l.Add(new SqlParameter("@COLUMN_NAME", ColsNames));
            using (SqlConnection c = new SqlConnection(Connection))
            {
                c.InfoMessage += new SqlInfoMessageEventHandler(myConnection_InfoMessage);
                using (SqlCommand cmd = new SqlCommand("dbo.sp_AddColumn", c) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.AddRange(l.ToArray());
                    c.Open();
                    cmd.CommandTimeout = 600;
                    SqlDataReader result = cmd.ExecuteReader();
                    do
                    {
                        while (result.Read())
                        {
                            TextWriter sw = new StreamWriter(this.FileName, false, Encoding.GetEncoding(1252));
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
