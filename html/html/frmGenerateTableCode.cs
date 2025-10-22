using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UtilETWeb
{
	public partial class frmGenerateTableCode: Form
	{
		public frmGenerateTableCode() {
			InitializeComponent();
		}

		private void frmGenerateTableCode_Load(object sender, EventArgs e) {

		}

		private void textBox1_TextChanged(object sender, EventArgs e) {

		}

		private void btnGenerar_Click(object sender, EventArgs e) {


			Empty(Path.Combine(Environment.CurrentDirectory, @"Scripts"));

			string fileName = Path.Combine("Scripts", string.Format("001-[script table {0}].sql", this.txtAliasTableName.Text));

			string script = Properties.Resources._001__Crear_Tabla_Generica_;
			string text = string.Format(script, this.txtAliasTableName.Text, DateTime.Now);

			TextWriter sw = new StreamWriter(fileName, false, Encoding.GetEncoding(1252));
			sw.WriteLine(text);
			sw.Close();

			Process.Start("explorer.exe", fileName);
		}

		public static void Empty(string directory) {
			foreach (string fileToDelete in System.IO.Directory.GetFiles(directory)) {
				FileOperationAPIWrapper.MoveToRecycleBin(fileToDelete);
				System.IO.File.Delete(fileToDelete);
			}
			foreach (string subDirectoryToDeleteToDelete in System.IO.Directory.GetDirectories(directory)) {
				FileOperationAPIWrapper.MoveToRecycleBin(subDirectoryToDeleteToDelete);
				System.IO.Directory.Delete(subDirectoryToDeleteToDelete, true);
			}
		}
	}
}
