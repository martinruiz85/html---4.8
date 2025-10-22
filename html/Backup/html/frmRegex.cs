using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;

//https://regex101.com/#javascript
namespace UtilETWeb
{
    public partial class frmRegex : Form
    {
        public frmRegex()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmRegex_Load);
        }

        void frmRegex_Load(object sender, EventArgs e)
        {
            this.comboBox1.ValueMember = "pattern";
            this.comboBox1.DisplayMember = "pattern";
            this.comboBox1.DataSource = RegexList;

            this.cmbFormat.DataSource = this.FormatResult;
        }

        public Regex Regex
        {
            get
            {
                if (!string.IsNullOrEmpty(this.txtRegex.Text))
                    return new Regex(this.txtRegex.Text);
                else
                    return new Regex("\"([^\"]*)\"");
            }
        }

        public List<Regex> RegexList
        {
            get
            {
                return new List<Regex>()
                {
                    new Regex("^.*$", RegexOptions.Multiline),
                    new Regex(@"\D"),
                    new Regex("\"([^\"]*)\""),      // get varibles string "mach"
                    new Regex("\"@([^\"]*)\""),     // get params "@mach"
                    new Regex(@"(\w+)"),            // palabras
                    new Regex(@"\s+"),              // spacios
                    new Regex("\".*?\""),           // todo lo que esta entre comillas dobles ""
                    new Regex(@"\d+"),              // digitos
                    new Regex(@"[A-Za-z0-9\-]+"),   // alphanumeric 
                    new Regex(@"(?<!\S)(\d*\.?\d+|\d{1,3}(,\d{3})*(\.\d+)?)(?!\S)"), //numeros embebidos
                    new Regex(@"(?:^|\s)(\d*\.?\d+|\d{1,3}(?:,\d{3})*(?:\.\d+)?)(?!\S)"), //numeros embebidos dos
                    new Regex(@"\|(dbo.*?[^\\])\||\|(pd.*?[^\\])\|"), //tablas y data sps
                    new Regex(@">.*?<"),
                    new Regex(@"\@([^=<>\s\']+)")
                    //http://stackoverflow.com/questions/171480/regex-grabbing-values-between-quotation-marks
                    //new Regex("\"(.*?[^\\])\""), //Double quotes only (use value of capture group #1):
                    //new Regex(@"'(.*?[^\\])'"), //Single quotes only (use value of capture group #1):
                    //new Regex("([\"'])(.*?[^\\])\\1") //Both (use value of capture group #2):
                };
            }
        }

        List<string> FormatResult
        {
            get
            {
                return
                new List<string>
                {
                        "{0}",
                        "\"{0}\",",
                        "dt.Columns.Add(\"{0}\", typeof(String));"                                                
                };
            }
        }

        List<StringValue> _MachList = new List<StringValue>();
        public List<StringValue> MachList
        {
            get
            {
                return _MachList;
            }
        }


        private void btnRegex_Click(object sender, EventArgs e)
        {

            MachList.Clear();
            this.rtxInput.SelectAll();
            this.rtxInput.SelectionBackColor = System.Drawing.Color.White;
            this.rtxInput.DeselectAll();

            Match match = !this.chkRegex.Checked ?
                Regex.Match(this.rtxInput.Text.ToLower())
                : ((Regex)this.comboBox1.SelectedValue).Match(this.rtxInput.Text.ToLower());

            while (match.Success)
            {
                MachList.Add(new StringValue(match.Value));

                this.rtxInput.Select(match.Index, match.Length);
                this.rtxInput.SelectionBackColor = Color.Yellow;

                Console.WriteLine("Match");
                for (int i = 1; i <= 2; i++)
                {
                    Group g = match.Groups[i];
                    Console.WriteLine("Group" + i + "='" + g + "'");
                    CaptureCollection cc = g.Captures;
                    for (int j = 0; j < cc.Count; j++)
                    {
                        Capture c = cc[j];
                        System.Console.WriteLine("Capture" + j + "='" + c + "', Position=" + c.Index);
                    }
                }
                match = match.NextMatch();
            }
            this.dataGridView1.DataSource = MachList.ToArray();
        }

        //http://stackoverflow.com/questions/19582570/how-to-read-load-text-txt-file-values-in-datagridview-using-c-sharp
        private void btnExport_Click(object sender, EventArgs e)
        {
            string filename = string.Format("{0}.txt", Guid.NewGuid());
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
            {
                foreach (DataGridViewRow line in this.dataGridView1.Rows)
                {
                    foreach (DataGridViewColumn item in this.dataGridView1.Columns)
                    {
                        //file.WriteLine("dt.Columns.Add(\"{0}\", typeof(String));",line.Cells[item.Name].FormattedValue);
                        //file.WriteLine("\"{0}\",", line.Cells[item.Name].FormattedValue);
                        //file.WriteLine("{0}", line.Cells[item.Name].FormattedValue);

                        string format = this.cmbFormat.SelectedValue != null ? this.cmbFormat.SelectedValue.ToString() : this.cmbFormat.Text;
                        file.WriteLine(format, line.Cells[item.Name].FormattedValue);

                    }
                }
            }
            Process.Start(filename);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://regex101.com/#javascript");
            Process.Start(sInfo);
        }
    }

    public class StringValue
    {
        string _value;
        public string Value { get { return _value; } set { _value = value; } }
        public StringValue(string s)
        {
            _value = s;
        }
    }
}
