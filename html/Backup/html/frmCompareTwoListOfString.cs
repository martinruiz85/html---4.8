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
using Excel = Microsoft.Office.Interop.Excel; 

namespace UtilETWeb
{
    public partial class frmCompareTwoListOfString : Form
    {
        public frmCompareTwoListOfString()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmCompareTwoListOfString_Load);
        }

        void frmCompareTwoListOfString_Load(object sender, EventArgs e)
        {
            this.backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);

            this.txtPro.Click += new EventHandler(txtPro_Click);
            this.txtDev.Click += new EventHandler(txtPro_Click);
        }

        void txtPro_Click(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            txt.SelectAll();
            txt.Focus();
        }



        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DataTable dt = (DataTable)e.Result;
            if (dt.Rows.Count > 0)
            {
                //this.button1.Enabled = true;
                this.btnCompare.Image = UtilETWeb.Properties.Resources.work;
                this.dataGridView1.DataSource = dt;
            }
            else
            {
                this.dataGridView1.DataSource = dt;
                this.btnCompare.Image = UtilETWeb.Properties.Resources.exclamation;
            }
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string spro = ((object[])(e.Argument))[0].ToString();
            string sdev = ((object[])(e.Argument))[1].ToString();
            e.Result = this.Compare(spro, sdev);
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

        private void btnCompare_Click(object sender, EventArgs e)
        {
            if (this.backgroundWorker1.IsBusy != true)
            {                
                string spro = this.txtPro.Text;
                string sdev = this.txtDev.Text;
                this.btnCompare.Image = UtilETWeb.Properties.Resources.generator;
                this.backgroundWorker1.RunWorkerAsync(new object[] { spro, sdev });
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string fileName = @"data.xml";
            DataTable dataSet = (DataTable)dataGridView1.DataSource;
            dataSet.WriteXml(fileName);          
        }

    }
}
