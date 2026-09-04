using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

namespace UtilETWeb
{
    public partial class frmMigration : Form
    {
        public frmMigration()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmMigration_Load);
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

        private List<MigrationTable> TableNames
        {
            get
            {
                return richTextBox1.Text
                    .Split('\n')
                    .Select(s => s.Trim().Trim('\r'))
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Select(s =>
                    {
                        int idx = s.IndexOf('|');
                        return idx >= 0
                            ? new MigrationTable { Name = s.Substring(0, idx).Trim(), Filter = s.Substring(idx + 1).Trim() }
                            : new MigrationTable { Name = s.Trim(), Filter = null };
                    })
                    .GroupBy(t => t.Name)
                    .Select(g => g.First())
                    .ToList();
            }
        }

        void frmMigration_Load(object sender, EventArgs e)
        {
            this.cmbDatabaseSorce.DataSource = GetConnexions();
            this.cmbDatabaseSorce.ValueMember = "Code";
            this.cmbDatabaseSorce.DisplayMember = "Name";
            this.cmbDatabaseSorce.SelectedValue = "-1";

            this.cmbDatabaseTarget.DataSource = GetConnexions();
            this.cmbDatabaseTarget.ValueMember = "Code";
            this.cmbDatabaseTarget.DisplayMember = "Name";
            this.cmbDatabaseTarget.SelectedValue = "-1";

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
            if (e.UserState is string)
                this.lblStatus.Text = e.UserState as string;
        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.btnMigrateTables.Enabled = true;

            string result = e.Result as string;
            if (result == "OK")
            {
                this.progressBar1.Value = 100;
                this.lblStatus.Text = "Migración completada.";
                MessageBox.Show("Migración completada exitosamente.", "Migración", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.lblStatus.Text = "Error.";
                errorProvider1.SetError(btnMigrateTables, result);
            }
        }

        private void btnMigrateTables_Click(object sender, EventArgs e)
        {
            if (this.IsValid())
            {
                if (!backgroundWorker1.IsBusy)
                {
                    errorProvider1.SetError(btnMigrateTables, "");
                    this.progressBar1.Value = 0;
                    this.lblStatus.Text = "Iniciando...";

                    string connSource = this.cmbDatabaseSorce.SelectedValue as string;
                    string connTarget = this.cmbDatabaseTarget.SelectedValue as string;

                    this.btnMigrateTables.Enabled = false;

                    backgroundWorker1.RunWorkerAsync(new MigrationArgs
                    {
                        Tables = this.TableNames,
                        ConnectionStringSource = connSource,
                        ConnectionStringTarget = connTarget,
                        CreateIfNotExists = this.chkCreateTable.Checked
                    });
                }
            }
        }

        private void CreateTableIfNotExists(string tableName, string connSource, string connTarget)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connSource);
            ServerConnection srvConn = new ServerConnection(builder.DataSource);
            if (builder.IntegratedSecurity)
            {
                srvConn.LoginSecure = true;
            }
            else
            {
                srvConn.LoginSecure = false;
                srvConn.Login = builder.UserID;
                srvConn.Password = builder.Password;
            }

            Server srv = new Server(srvConn);
            Database db = srv.Databases[builder.InitialCatalog];

            string schemaName = "dbo";
            string tableNameOnly = tableName.Trim('[', ']');
            if (tableNameOnly.Contains("."))
            {
                string[] parts = tableNameOnly.Split('.');
                schemaName = parts[0].Trim('[', ']');
                tableNameOnly = parts[1].Trim('[', ']');
            }

            Table table = db.Tables[tableNameOnly, schemaName];
            if (table == null) return;

            ScriptingOptions options = new ScriptingOptions();
            options.ClusteredIndexes = true;
            options.Default = true;
            options.DriAll = true;
            options.Indexes = true;
            options.NoCollation = true;
            options.AnsiPadding = false;
            options.IncludeIfNotExists = true;

            using (SqlConnection dstConn = new SqlConnection(connTarget))
            {
                dstConn.Open();
                foreach (string sqlScript in table.Script(options))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlScript, dstConn))
                    {
                        cmd.CommandTimeout = 600;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                MigrationArgs args = e.Argument as MigrationArgs;
                List<MigrationTable> tables = args.Tables;

                for (int i = 0; i < tables.Count; i++)
                {
                    MigrationTable table = tables[i];

                    if (args.CreateIfNotExists)
                    {
                        backgroundWorker1.ReportProgress(
                            (int)((i / (float)tables.Count) * 100),
                            string.Format("Creando estructura: {0} ({1}/{2})", table.Name, i + 1, tables.Count));

                        CreateTableIfNotExists(table.Name, args.ConnectionStringSource, args.ConnectionStringTarget);
                    }

                    backgroundWorker1.ReportProgress(
                        (int)((i / (float)tables.Count) * 100),
                        string.Format("Copiando datos: {0} ({1}/{2})", table.Name, i + 1, tables.Count));

                    using (SqlConnection srcConn = new SqlConnection(args.ConnectionStringSource))
                    {
                        srcConn.Open();
                        // Se usa DataTable en lugar de SqlDataReader para evitar el error de
                        // locale/collation mismatch que SqlBulkCopy detecta al comparar metadatos
                        DataTable dt = new DataTable();
                        string query = string.IsNullOrEmpty(table.Filter)
                            ? string.Format("SELECT * FROM {0}", table.Name)
                            : string.Format("SELECT * FROM {0} WHERE {1}", table.Name, table.Filter);

                        using (SqlCommand cmd = new SqlCommand(query, srcConn))
                        {
                            cmd.CommandTimeout = 600;
                            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                                adapter.Fill(dt);
                        }

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(args.ConnectionStringTarget, SqlBulkCopyOptions.TableLock))
                        {
                            bulkCopy.DestinationTableName = table.Name;
                            bulkCopy.BulkCopyTimeout = 600;
                            bulkCopy.BatchSize = 5000;
                            bulkCopy.WriteToServer(dt);
                        }
                    }

                    backgroundWorker1.ReportProgress(
                        (int)(((i + 1) / (float)tables.Count) * 100),
                        string.Format("Completado: {0} ({1}/{2})", table.Name, i + 1, tables.Count));
                }

                e.Result = "OK";
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }

        private void cmbDatabaseSorce_Validating(object sender, CancelEventArgs e)
        {
            if (cmbDatabaseSorce.SelectedValue == null || cmbDatabaseSorce.SelectedValue.ToString() == "-1")
                errorProvider1.SetError(cmbDatabaseSorce, "debe seleccionar la base de datos origen");
            else
                errorProvider1.SetError(cmbDatabaseSorce, "");
        }

        private void cmbDatabaseTarget_Validating(object sender, CancelEventArgs e)
        {
            if (cmbDatabaseTarget.SelectedValue == null || cmbDatabaseTarget.SelectedValue.ToString() == "-1")
                errorProvider1.SetError(cmbDatabaseTarget, "debe seleccionar la base de datos destino");
            else
                errorProvider1.SetError(cmbDatabaseTarget, "");
        }

        private bool IsValid()
        {
            bool valid = true;
            if (cmbDatabaseSorce.SelectedValue == null || cmbDatabaseSorce.SelectedValue.ToString() == "-1")
            {
                errorProvider1.SetError(cmbDatabaseSorce, "debe seleccionar la base de datos origen");
                valid = false;
            }
            if (cmbDatabaseTarget.SelectedValue == null || cmbDatabaseTarget.SelectedValue.ToString() == "-1")
            {
                errorProvider1.SetError(cmbDatabaseTarget, "debe seleccionar la base de datos destino");
                valid = false;
            }
            return valid;
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
        }
    }

    public class MigrationTable
    {
        public string Name { get; set; }
        public string Filter { get; set; }
    }

    public class MigrationArgs
    {
        public List<MigrationTable> Tables { get; set; }
        public string ConnectionStringSource { get; set; }
        public string ConnectionStringTarget { get; set; }
        public bool CreateIfNotExists { get; set; }
    }
}
