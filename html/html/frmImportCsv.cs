using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace UtilETWeb
{
    public partial class frmImportCsv : Form
    {
        public frmImportCsv()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmImportCsv_Load);
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

        void frmImportCsv_Load(object sender, EventArgs e)
        {
            this.cmbDatabase.DataSource = GetConnexions();
            this.cmbDatabase.ValueMember = "Code";
            this.cmbDatabase.DisplayMember = "Name";
            this.cmbDatabase.SelectedValue = "-1";

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                dlg.Title = "Seleccionar archivo CSV";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = dlg.FileName;
                    // Sugerir nombre de tabla basado en el nombre del archivo
                    if (string.IsNullOrEmpty(txtTableName.Text))
                        txtTableName.Text = Path.GetFileNameWithoutExtension(dlg.FileName);
                }
            }
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
            if (e.UserState is string)
                this.lblStatus.Text = e.UserState as string;
        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.btnImport.Enabled = true;
            string result = e.Result as string;
            if (result == "OK")
            {
                this.progressBar1.Value = 100;
                this.lblStatus.Text = "Importación completada.";
                MessageBox.Show("Importación completada exitosamente.", "Importar CSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.lblStatus.Text = "Error.";
                errorProvider1.SetError(btnImport, result);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (!IsValid()) return;
            if (backgroundWorker1.IsBusy) return;

            errorProvider1.SetError(btnImport, "");
            this.progressBar1.Value = 0;
            this.lblStatus.Text = "Iniciando...";
            this.btnImport.Enabled = false;

            backgroundWorker1.RunWorkerAsync(new ImportCsvArgs
            {
                FilePath = txtFilePath.Text.Trim(),
                ConnectionString = this.cmbDatabase.SelectedValue as string,
                TableName = txtTableName.Text.Trim(),
                HasHeaders = chkHasHeaders.Checked,
                Delimiter = GetDelimiter(),
                Encoding = GetEncoding(),
                CreateTable = chkCreateTable.Checked,
                BatchSize = 5000
            });
        }

        private char GetDelimiter()
        {
            switch (cmbDelimiter.SelectedIndex)
            {
                case 1: return ';';
                case 2: return '\t';
                case 3: return '|';
                default: return ',';
            }
        }

        private Encoding GetEncoding()
        {
            switch (cmbEncoding.SelectedIndex)
            {
                case 1: return Encoding.GetEncoding(1252);
                case 2: return Encoding.GetEncoding("iso-8859-1");
                default: return Encoding.UTF8;
            }
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ImportCsvArgs args = e.Argument as ImportCsvArgs;

                // Leer CSV completo para conocer total de filas
                backgroundWorker1.ReportProgress(0, "Leyendo archivo...");
                List<string[]> rows = new List<string[]>();
                string[] headers = null;

                using (StreamReader sr = new StreamReader(args.FilePath, args.Encoding))
                {
                    string line;
                    bool firstLine = true;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        string[] fields = ParseCsvLine(line, args.Delimiter);
                        if (firstLine && args.HasHeaders)
                        {
                            headers = fields;
                            firstLine = false;
                            continue;
                        }
                        rows.Add(fields);
                        firstLine = false;
                    }
                }

                if (rows.Count == 0)
                {
                    e.Result = "El archivo no contiene datos.";
                    return;
                }

                int columnCount = rows[0].Length;
                if (headers == null)
                    headers = Enumerable.Range(1, columnCount).Select(i => "Col" + i).ToArray();

                // Crear tabla en BD si se requiere
                if (args.CreateTable)
                {
                    backgroundWorker1.ReportProgress(0, "Creando tabla...");
                    CreateSqlTable(args.ConnectionString, args.TableName, headers);
                }

                // Insertar en lotes
                int total = rows.Count;
                int inserted = 0;

                while (inserted < total)
                {
                    int batchCount = Math.Min(args.BatchSize, total - inserted);
                    DataTable dt = BuildDataTable(headers, rows.GetRange(inserted, batchCount));

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(args.ConnectionString, SqlBulkCopyOptions.TableLock))
                    {
                        bulkCopy.DestinationTableName = args.TableName;
                        bulkCopy.BulkCopyTimeout = 600;
                        bulkCopy.BatchSize = args.BatchSize;
                        bulkCopy.WriteToServer(dt);
                    }

                    inserted += batchCount;
                    int pct = (int)((inserted / (float)total) * 100);
                    backgroundWorker1.ReportProgress(pct,
                        string.Format("Insertando... {0}/{1} filas", inserted, total));
                }

                e.Result = "OK";
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }

        private string[] ParseCsvLine(string line, char delimiter)
        {
            List<string> fields = new List<string>();
            bool inQuotes = false;
            StringBuilder current = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == delimiter && !inQuotes)
                {
                    fields.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }
            fields.Add(current.ToString());
            return fields.ToArray();
        }

        private DataTable BuildDataTable(string[] headers, List<string[]> rows)
        {
            DataTable dt = new DataTable();
            foreach (string h in headers)
                dt.Columns.Add(h, typeof(string));

            foreach (string[] row in rows)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                    dr[i] = i < row.Length ? (object)row[i] : DBNull.Value;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        private void CreateSqlTable(string connectionString, string tableName, string[] headers)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{0}')\n", tableName.Trim('[', ']'));
            sb.AppendFormat("CREATE TABLE {0} (\n", tableName);
            for (int i = 0; i < headers.Length; i++)
            {
                string colName = "[" + headers[i].Replace("]", "]]").Trim() + "]";
                sb.AppendFormat("    {0} NVARCHAR(MAX){1}\n", colName, i < headers.Length - 1 ? "," : "");
            }
            sb.Append(")");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sb.ToString(), conn))
                {
                    cmd.CommandTimeout = 120;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private bool IsValid()
        {
            bool valid = true;
            if (string.IsNullOrEmpty(txtFilePath.Text) || !File.Exists(txtFilePath.Text))
            {
                errorProvider1.SetError(txtFilePath, "seleccione un archivo CSV válido");
                valid = false;
            }
            else
                errorProvider1.SetError(txtFilePath, "");

            if (cmbDatabase.SelectedValue == null || cmbDatabase.SelectedValue.ToString() == "-1")
            {
                errorProvider1.SetError(cmbDatabase, "debe seleccionar una base de datos");
                valid = false;
            }
            else
                errorProvider1.SetError(cmbDatabase, "");

            if (string.IsNullOrWhiteSpace(txtTableName.Text))
            {
                errorProvider1.SetError(txtTableName, "ingrese el nombre de la tabla destino");
                valid = false;
            }
            else
                errorProvider1.SetError(txtTableName, "");

            return valid;
        }
    }

    public class ImportCsvArgs
    {
        public string FilePath { get; set; }
        public string ConnectionString { get; set; }
        public string TableName { get; set; }
        public bool HasHeaders { get; set; }
        public char Delimiter { get; set; }
        public Encoding Encoding { get; set; }
        public bool CreateTable { get; set; }
        public int BatchSize { get; set; }
    }
}
