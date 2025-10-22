using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace UtilETWeb
{
    public partial class frmRenameNumberPrefix : Form
    {
        public frmRenameNumberPrefix()
        {
            InitializeComponent();
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            List<string> pathfiles = Directory.GetFiles(this.textBox1.Text).CustomSort().ToList();
            foreach (string filename in pathfiles)
            {
                string oldName = Path.GetFileName(filename);
                int pos = oldName.IndexOfAny(new char[] { '-' });
                if (pos == 0)
                    continue;

                // mantener numeracion
                //int number;
                //if (int.TryParse(oldName.Substring(0, pos), out number))
                //{
                //    string newName = number.ToString("0000") + oldName.Substring(pos);

                //    int res = rename(filename, Path.Combine(this.textBox1.Text, newName));
                //}

                int prefix = this.numericTextBox1.IntValue + pathfiles.IndexOf(filename);

                string pattern = @"\[([^\[]*)\]";
                if (!Regex.IsMatch(oldName, pattern))
                {
                    oldName = oldName.Substring(pos).Trim("-".ToCharArray()).TrimEnd(".sql".ToCharArray());
                    oldName = string.Format("-[{0}].sql", oldName);

                    string newName = prefix.ToString("0000") + oldName;
                    int res = rename(filename, Path.Combine(this.textBox1.Text, newName));
                }
                else
                {
                    string newName = prefix.ToString("0000") + oldName.Substring(pos);
                    int res = rename(filename, Path.Combine(this.textBox1.Text, newName));
                }

            }

            Process.Start("explorer.exe", this.textBox1.Text);
        }

        [DllImport("msvcrt", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int rename(
                [MarshalAs(UnmanagedType.LPStr)]
            string oldpath,
                [MarshalAs(UnmanagedType.LPStr)]
            string newpath);
    }


}
