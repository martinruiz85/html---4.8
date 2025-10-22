using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using UtilETWeb.Data;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace UtilETWeb
{
    public partial class frmGetReportDataSorce : Form
    {
        public frmGetReportDataSorce()
        {
            InitializeComponent();
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


        private void frmAddFielsReportRdl_Load(object sender, EventArgs e)
        {
            this.txtPath.Text = @"W:\ASP\Private\Reports\SlotPosSummary.rdl";

            this.cmbDatabase.DataSource = GetConnexions();
            this.cmbDatabase.ValueMember = "Code";
            this.cmbDatabase.DisplayMember = "Name";
            //this.cmbDatabase.SelectedValue = Querys.ConnectionDefault;
            //this.cmbDatabase.Text = "(sin especificar)";
            this.cmbDatabase.Text = "Produccion";

            this.btnOpenFile.CausesValidation = false;

        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtPath.Text = this.openFileDialog1.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren())
            {
                string html = File.ReadAllText(this.txtPath.Text);

                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(html);

                //Create an XmlNamespaceManager for resolving namespaces.
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
                nsmgr.AddNamespace("ab", "http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition");
                nsmgr.AddNamespace("rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");

                XmlNode n_CommandText = xdoc.DocumentElement.SelectSingleNode("//*[@Name='Data']//ab:Query//ab:CommandText//text()", nsmgr);
                string datasources = n_CommandText.InnerText.Replace("{{:}}", "|").Replace("{{&}}", "|").Replace("\r\n", "");

                List<SqlParameter> prms = new List<SqlParameter>();
                prms.Add(new SqlParameter("@vstrReportDatasources", datasources));

                DataTable dt = Querys.ExecDatatable("sp_GetReportDataSource", prms, this.cmbDatabase.SelectedValue.ToString());

                this.dataGridView1.DataSource = dt;

                this.dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                this.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }

        }

        private void cmbDatabase_Validating(object sender, CancelEventArgs e)
        {
            if (this.cmbDatabase.Text == "(sin especificar)")
            {
                errorProvider1.SetError(cmbDatabase, "debe seleccionar una opcion de base de datos");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(cmbDatabase, "");
            }
        }

        private void txtFile_TextChanged(object sender, EventArgs e)
        {


            string datasources = this.txtFile.Text.Replace("\r\n", "|");

            List<SqlParameter> prms = new List<SqlParameter>();
            prms.Add(new SqlParameter("@vstrReportDatasources", datasources));

            DataTable dt = Querys.ExecDatatable("sp_GetReportDataSource", prms, this.cmbDatabase.SelectedValue.ToString());

            this.dataGridView1.DataSource = dt;

            this.dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }

        private void txtFile_Enter(object sender, EventArgs e)
        {
            this.txtFile.SelectAll();
            this.txtFile.Focus();
        }

        private void txtFile_MouseClick(object sender, MouseEventArgs e)
        {
            this.txtFile.SelectAll();
            this.txtFile.Focus();
        }

        private void txtPath_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtPath.Text == "")
            {
                errorProvider1.SetError(txtPath, "debe seleccionar una archivo");
                e.Cancel = true;
            }
            if (Path.GetExtension(this.txtPath.Text) != ".rdl")
            {
                errorProvider1.SetError(txtPath, "debe seleccionar una archivo tipo .rdl");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtPath, "");
            }
        }
    }
}
