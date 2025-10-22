using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.IO;
using System.Xml;
using System.Net;
using System.Collections.Specialized;
using UtilETWeb.Data;
using Microsoft.Data.SqlClient;
using System.Web;

namespace html
{
    public partial class frmGetText : Form
    {
        public frmGetText()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form1_Load);
        }

        string GetHtml(string url)
        {
            using (var client = new CookieAwareWebClient())
            {
                var values = new NameValueCollection { { "username", "cristina.lozoya@xignux.com" }, { "password", "cristina" }, };
                client.UploadValues(url, values);

                // If the previous call succeeded we now have a valid authentication cookie
                // so we could download the protected page
                string result = client.DownloadString(url);
                return result;
            }
        }

        void Form1_Load(object sender, EventArgs e)
        {
            this.backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DataTable dt = (DataTable)e.Result;
            if (dt.Rows.Count > 0)
            {
                //this.button1.Enabled = true;
                this.btnCompare.Image = UtilETWeb.Properties.Resources.work;
                this.dataGridView1.DataSource = dt;
            }
            else
            {
                this.dataGridView1.DataSource = dt;
                this.btnCompare.Image = UtilETWeb.Properties.Resources.exclamation;
            }
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string spro = ((object[])(e.Argument))[0].ToString();
            string sdev = ((object[])(e.Argument))[1].ToString();
            e.Result = this.Compare(spro, sdev);
        }

        private string GetLabels(string html)
        {
            string info;
            return GetLabels(html, out info);
        }

        public class IndexKey
        {
            public string Group { get; set; }
            public int Index { get; set; }
            public string Value { get; set; }
        }

        private string GetLabels(string html, out string info)
        {
            List<IndexKey> l = new List<IndexKey>() { };

            info = "";
            string values = "";
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            StringBuilder sb = new StringBuilder();
            StringBuilder sbinfo = new StringBuilder();
            if (doc.DocumentNode.ChildNodes.Count > 0)
            {

                //string pathListLayoutTable = @"//*[@title='Editar']/./../following-sibling::td[1]";
                string pathListLayoutTable = @"//*[@title='Editar']/./../following-sibling::td[position() < 3]";
                if (doc.DocumentNode.SelectNodes(pathListLayoutTable) != null)
                {
                    List<HtmlNode> lth = doc.DocumentNode.SelectNodes(pathListLayoutTable).ToList();
                    foreach (HtmlNode itemth in lth)
                    {
                        l.Add(new IndexKey()
                        {
                            Group = "Pivot",
                            Index = lth.IndexOf(itemth) % 2,
                            Value = HttpUtility.HtmlDecode(itemth.InnerText).Trim()
                        });
                        //sb.AppendLine(HttpUtility.HtmlDecode(itemth.InnerText).Trim());
                    }
                }
            }

            string[] query = l.Where(o => o.Index == 0).Select(o => o.Value).ToArray();

            //values = sb.ToString();
            values = string.Join("\r\n", query);

            //var queryinfo = l.GroupBy(t => t.index).Select(g1 => new
            //{
            //    Date = g1.Key,
            //    Details = g1.GroupBy(t => t.key).Select(g => new
            //    {
            //        Robot = g.Key,
            //        TaskStates = g.ToDictionary(t => t.index, t => t.key)
            //    }).ToList()
            //});


            var queryinfo = from d in l
                            group d by d.Index
                                into grp
                                select new
                                {
                                    Foo = grp.Key,
                                    Bars = grp.Select(d2 => d2.Value).ToArray()
                                };

            Dictionary<string, string> dic = new Dictionary<string, string>();


            if (queryinfo.Count() > 0)
            {
                //find the total number of (data) rows
                int rows = queryinfo.Max(grp => grp.Bars.Length);

                ////output columns
                //foreach (var grp in queryinfo)
                //{
                //    sb.Append(grp.Foo + "\t");
                //}
                //Console.WriteLine();


                //output data
                for (int i = 0; i < rows; i++)
                {
                    foreach (var grp in queryinfo)
                    {
                        sb.Append((i < grp.Bars.Length ? grp.Bars[i] : null) + ":");
                    }
                    sb.AppendLine();
                }
            }

            info = sb.ToString();
            return values;
        }

        private DataTable Compare(string spro, string sdev)
        {
            DataTable dt = new DataTable();
            try
            {
                List<SqlParameter> l = new List<SqlParameter>();
                l.Add(new SqlParameter("@Spro", spro));
                l.Add(new SqlParameter("@Sdev", sdev));
                DataSet ds = Querys.ExecDataSet("sp_CompareText", l);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
                return dt;
            }
            catch (Exception)
            {

                return dt;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.backgroundWorker1.IsBusy != true)
            {
                string spro = this.GetLabels(this.txtPro.Text);
                string sdev = this.GetLabels(this.txtDev.Text);
                this.btnCompare.Image = UtilETWeb.Properties.Resources.generator;
                this.backgroundWorker1.RunWorkerAsync(new object[] { spro, sdev });
            }

        }

        string FileName
        {
            get
            {
                return Path.Combine(Environment.CurrentDirectory, string.Format("{0}.txt", "pro"));
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            File.WriteAllText(this.FileName, String.Empty);
            string info;
            string spro = this.GetLabels(this.txtPro.Text, out info);
            TextWriter sw = new StreamWriter(this.FileName, true);
            sw.WriteLine(info);
            sw.Close();

            if (File.Exists(FileName)) System.Diagnostics.Process.Start(FileName);
        }
    }
}
