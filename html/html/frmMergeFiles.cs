using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace UtilETWeb
{
    public partial class frmMergeFiles : Form
    {
        public frmMergeFiles()
        {
            InitializeComponent();
        }


        private void frmJoinSqlFiles_Load(object sender, EventArgs e)
        {
            //this.textBox1.Text = Path.Combine(Environment.CurrentDirectory, "Scripts");
        }

        private void WriteFile(StringBuilder sb, string dir)
        {


            //Console.WriteLine(dir);
            string[] archivosSql = Directory.GetFiles(dir, "*.sql", SearchOption.TopDirectoryOnly);
            string nombreCarpeta = Path.GetFileName(dir);

            int index = 0;
            foreach (string archivo in archivosSql)
            {
                if (this.chkShowPath.Checked)
                    sb.AppendLine($"/*===== Archivo: {archivo} =====*/");


                sb.AppendLine($"/*===== {nombreCarpeta} : {index + 1} =====*/");
                string contenido = File.ReadAllText(archivo, Encoding.UTF8);
                sb.AppendLine(contenido);
                sb.AppendLine(); // Línea vacía entre archivos

                index++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string folderPath = this.textBox1.Text;

            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
            {
                MessageBox.Show("La carpeta seleccionada no existe.");
                return;
            }

            //Regex regex = new Regex(@"^\d{2}");
            string outputFile = Path.Combine(folderPath, string.Format("ScriptCombinado_{0:yyyyMMdd_HHmmss}.sql", DateTime.Now));
            StringBuilder sb = new StringBuilder();

            var dirs = Directory.GetDirectories(folderPath)
                //.Where(dir => regex.IsMatch(Path.GetFileName(dir)))
                .ToList();

            if (dirs.Count > 0)
                dirs.ForEach(d => this.WriteFile(sb, d));
            else
                this.WriteFile(sb, folderPath);

            // Guardar todo el contenido en un archivo
            File.WriteAllText(outputFile, sb.ToString(), new UTF8Encoding(true));

            System.Diagnostics.Process.Start("explorer.exe", outputFile);

        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
