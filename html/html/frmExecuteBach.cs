using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace UtilETWeb
{
    public partial class frmExecuteBach : Form
    {
        public frmExecuteBach()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmExecuteBach_Load);
            this.AutoValidate = AutoValidate.Disable;

            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            this.lblPorcent.Text = string.Format("Completed [{0:000}%]...", e.ProgressPercentage);
            this.progressBar1.Value = e.ProgressPercentage;
            if (e.ProgressPercentage > 0)
                this.progressBar1.Value = e.ProgressPercentage - 1;
                        
        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
			//MessageBox.Show("termino");
			string result = e.Result as string;		
			if (result == "OK")
			{
				this.progressBar1.Value = 100;
				this.btnGenerate.Image = Properties.Resources.work;
				errorProvider1.SetError(btnGenerate, "");
			}
			else
			{
				this.btnGenerate.Image = Properties.Resources.exclamation;
				errorProvider1.SetError(btnGenerate, result);
			}
		}

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string filename = "";
            try            
            {
				List<UtilETWeb.frmGenerateProcedures.SqlFile> l = e.Argument as List<UtilETWeb.frmGenerateProcedures.SqlFile>;
				foreach (var item in l)
				{
                    filename = item.Name;
					string script = File.ReadAllText(item.Name, Encoding.UTF8);
					SqlConnection conn = new SqlConnection(ConnectionString);
					Server server = new Server(new ServerConnection(conn));
					server.ConnectionContext.ExecuteNonQuery(script);

					float percent = ((float)(l.IndexOf(item) + 1) / (float)l.Count) * (100.0F);
					this.backgroundWorker1.ReportProgress((int)percent);
				}
				e.Result = "OK";
			}
            catch (Exception ex)
            {
                e.Result = string.Format("{0}|{1}", filename, ex.Message);
            }
            
        }

        private string DirectoryPath
        {
            get
            {
                return this.txtPath.Text;
            }
        }

        private string SearchPattern
        {
            get
            {
                return this.txtSearchPatern.Text;
            }
        }

        void frmExecuteBach_Load(object sender, EventArgs e)
        {

            this.cmbDatabase.DataSource = GetConnexions();
            this.cmbDatabase.ValueMember = "Code";
            this.cmbDatabase.DisplayMember = "Name";
            this.cmbDatabase.SelectedValue = System.Configuration.ConfigurationManager.AppSettings["ConnectionStringDEV"];


            List<UtilETWeb.frmDependsCustom.EnumModel> enums = ((IEnumerable<SearchOption>)Enum
                   .GetValues(typeof(SearchOption)))
                   .OrderBy(c => (int)c)
                   .Select(c => new UtilETWeb.frmDependsCustom.EnumModel()
                   {
                       Value = (int)c,
                       Name = c.GetDescription()
                   }).ToList();

            this.cmbSearchOption.DisplayMember = "Name";
            this.cmbSearchOption.ValueMember = "Value";
            this.cmbSearchOption.DataSource = enums;

            this.txtSearchPatern.Text = "*.sql";

            //this.txtPath.Text = @"C:\Users\consultorin\Documents\Visual Studio 2008\Projects\html\html\bin\Debug\Scripts";

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
                    return System.Configuration.ConfigurationManager.AppSettings["ConnectionStringDEV"];
                }
                else
                    return GetText(this.cmbDatabase).ToString();
            }
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtPath.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private List<UtilETWeb.frmGenerateProcedures.SqlFile> listsp
        {
            get
            {
                List<UtilETWeb.frmGenerateProcedures.SqlFile> query;
                List<string> l = new List<string>() { };
                string input = string.Join("\n", Directory.GetFiles(DirectoryPath, SearchPattern, SearchOption.TopDirectoryOnly));
                query = input
                    .Split("\n".ToCharArray())
                    .CustomSort(this.chkDesc.Checked)
                    .ToList()
                    .Select(s => new UtilETWeb.frmGenerateProcedures.SqlFile()
                    {
                        Name = s
                    }).ToList();
                return query;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren() && this.backgroundWorker1.IsBusy != true)
            {
                this.backgroundWorker1.RunWorkerAsync(this.listsp);
                this.progressBar1.Value = 0;
                this.lblPorcent.Text = "";
				this.btnGenerate.Image = Properties.Resources.generator;
			}
        }

        private void cmbDatabase_Validating(object sender, CancelEventArgs e)
        {            
        }

        private void txtPath_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtPath.Text == "")
            {
                e.Cancel = true;
                this.errorProvider1.SetError(txtPath, "se debe especificar un path");
            }
            else
            {
                e.Cancel = false;
                this.errorProvider1.SetError(txtPath, "");
            }
        }

        private void chkDesc_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
