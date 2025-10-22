using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UtilETWeb {
	public partial class frmSeparateInserts: Form {
		public frmSeparateInserts() {
			InitializeComponent();
			this.Load += FrmSeparateInserts_Load;
		}

		private void FrmSeparateInserts_Load(object sender, EventArgs e) {

			this.btnSelectFolder.CausesValidation = false;

			this.backgroundWorker1.DoWork += BackgroundWorker1_DoWork;
			this.backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;

			this.progressBar1.MarqueeAnimationSpeed = 10;
		}

		private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			this.button1.Enabled = true;
			this.progressBar1.Style = ProgressBarStyle.Blocks;
			string fileNameOutPut = e.Result as string;
			if (File.Exists(fileNameOutPut))
				System.Diagnostics.Process.Start(fileNameOutPut);
		}

		private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
			string arg = e.Argument as string;
			string[] prms = arg.Split("|".ToCharArray());
			string fileName = prms[0];
			string tableName = prms[1];
			e.Result = Process(fileName, tableName);
		}

		private string Process(string fileName, string tableName) {

			System.Threading.Thread.Sleep(1000);

			string fileNameOutPut = Path.Combine(Path.GetDirectoryName(fileName), string.Format("{0}Output.sql", Path.GetFileNameWithoutExtension(fileName)));

			if (File.Exists(fileNameOutPut))
				File.Delete(fileNameOutPut);

			int count = 0;
			using (StreamReader reader = new StreamReader(fileName, Encoding.UTF8)) {
				string line;
				TextWriter sw = new StreamWriter(fileNameOutPut, true, Encoding.UTF8);
				while ((line = reader.ReadLine()) != null) {

					if (count % 50 == 0) {
						sw.WriteLine("GO");
						if (!string.IsNullOrEmpty(tableName))
							sw.WriteLine(string.Format("SET IDENTITY_INSERT {0} ON", tableName));
					}

					sw.WriteLine(line);

					if (count % 50 == 49) {
						if (!string.IsNullOrEmpty(tableName))
							sw.WriteLine(string.Format("SET IDENTITY_INSERT {0} OFF", tableName));
					}

					count++;
				}
				sw.Close();
			}

			return fileNameOutPut;
		}

		private void button1_Click(object sender, EventArgs e) {
			if (this.ValidateChildren()) {
				if (!backgroundWorker1.IsBusy) {
					string fileName = this.textBox1.Text;
					string tableName = this.textBox2.Text;
					string prms = string.Format("{0}|{1}", fileName, tableName);
					this.progressBar1.Style = ProgressBarStyle.Marquee;
					this.button1.Enabled = false;
					this.backgroundWorker1.RunWorkerAsync(prms);
				}
			}
		}

		private void btnSelectFolder_Click(object sender, EventArgs e) {
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				this.textBox1.Text = openFileDialog1.FileName;
			}
		}

		private void label2_Click(object sender, EventArgs e) {

		}


		private void textBox1_Validating(object sender, CancelEventArgs e) {
			if (textBox1.Text == "") {
				errorProvider1.SetError(textBox1, "debe seleccionar una archivo");
				e.Cancel = true;
			}
			else if (Path.GetExtension(textBox1.Text).ToLower() != ".sql") {
				errorProvider1.SetError(textBox1, "solo se permiten archivo tipo .sql");
				e.Cancel = true;
			}
			else {
				errorProvider1.SetError(textBox1, "");
			}
		}
	}
}
