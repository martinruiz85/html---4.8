using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Base64ToFile
{
	public partial class frmBase64toFile: Form
	{
		public frmBase64toFile() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			this.txtDecryptFileResult.Text = "[type_your_decrypt_file].zip";
			this.txtEncryptFileResult.Text = "[type_your_encrypt_file].txt";

			this.btnDecryptFile.Icon = "\uf016";
			this.btnEncrpytFile.Icon = "\uf0f6";

			bgwEncrypt.WorkerReportsProgress = true;
			bgwEncrypt.DoWork += new DoWorkEventHandler(bgwEncrypt_DoWork);
			bgwEncrypt.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwEncrypt_RunWorkerCompleted);

			bgwDecrypt.WorkerReportsProgress = true;
			bgwDecrypt.DoWork += new DoWorkEventHandler(bgwDecrypt_DoWork);
			bgwDecrypt.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwDecrypt_RunWorkerCompleted);

			this.txtDecryptFileResult.GotFocus += new EventHandler(txtDecryptFileResult_GotFocus);
			this.txtDecryptFileResult.Click += new EventHandler(txtDecryptFileResult_Click);

		}

		void txtDecryptFileResult_Click(object sender, EventArgs e) {
			TextBox txt = sender as TextBox;
			txt.Select(txt.Text.IndexOf(".") + 1, txt.Text.Length);
		}

		void txtDecryptFileResult_GotFocus(object sender, EventArgs e) {
			TextBox txt = sender as TextBox;
			txt.Select(txt.Text.IndexOf(".") + 1, txt.Text.Length);
		}


		void bgwEncrypt_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			this.btnEncryptGenerate.Enabled = true;
			prgEncrypt.Style = ProgressBarStyle.Continuous;
			prgEncrypt.MarqueeAnimationSpeed = 0;
			Process.Start("explorer.exe", Environment.CurrentDirectory);
		}

		void bgwEncrypt_DoWork(object sender, DoWorkEventArgs e) {
			try {
				string[] prms = e.Argument as string[];
				Byte[] bytes = File.ReadAllBytes(prms[0]);
				String file = Convert.ToBase64String(bytes);
				File.WriteAllText(prms[1], file);
				e.Result = "";
			}
			catch (Exception ex) {
				e.Result = ex.ToString();
			}
		}


		void bgwDecrypt_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			this.btnDecryptGenerate.Enabled = true;
			prgDecrypt.Style = ProgressBarStyle.Continuous;
			prgDecrypt.MarqueeAnimationSpeed = 0;
			Process.Start("explorer.exe", Environment.CurrentDirectory);
		}

		void bgwDecrypt_DoWork(object sender, DoWorkEventArgs e) {
			try {
				string[] prms = e.Argument as string[];
				string yourBase64String = System.IO.File.ReadAllText(prms[0]);
				yourBase64String = yourBase64String.Replace(@"\n", "");
				Byte[] bytes = Convert.FromBase64String(yourBase64String);
				File.WriteAllBytes(prms[1], bytes);
				e.Result = "";
			}
			catch (Exception ex) {
				e.Result = ex.ToString();
			}

		}

		private void btnDecryptFile_Click(object sender, EventArgs e) {
			if (dlgDecrypt.ShowDialog() == DialogResult.OK) {
				this.txtDecryptFile.Text = dlgDecrypt.FileName;
			}
		}

		private void btnDecryptGenerate_Click(object sender, EventArgs e) {
			Button btn = sender as Button;
			if (string.IsNullOrEmpty(this.txtDecryptFile.Text))
				return;

			if (!bgwDecrypt.IsBusy) {
				btn.Enabled = false;
				prgDecrypt.Style = ProgressBarStyle.Marquee;
				prgDecrypt.MarqueeAnimationSpeed = 30;
				bgwDecrypt.RunWorkerAsync(new string[]{
				this.txtDecryptFile.Text,
				this.txtDecryptFileResult.Text
			 });
			}

		}

		private void btnEncrpytFile_Click(object sender, EventArgs e) {
			if (this.dlgEncrypt.ShowDialog() == DialogResult.OK) {
				this.txtEncryptFile.Text = dlgEncrypt.FileName;
			}
		}

		private void btnEncryptGenerate_Click(object sender, EventArgs e) {
			Button btn = sender as Button;
			if (string.IsNullOrEmpty(this.txtEncryptFile.Text))
				return;

			if (!bgwEncrypt.IsBusy) {
				btn.Enabled = false;
				prgEncrypt.Style = ProgressBarStyle.Marquee;
				prgEncrypt.MarqueeAnimationSpeed = 30;
				bgwEncrypt.RunWorkerAsync(new string[]{
				this.txtEncryptFile.Text,
				this.txtEncryptFileResult.Text
			 });
			}
		}

		private void btnFile_Click(object sender, EventArgs e) {

		}

		private void txtDecryptFileResult_Enter(object sender, EventArgs e) {

		}


	}
}
