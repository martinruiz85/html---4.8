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
using System.Data.SqlClient;
using System.Web;

namespace html
{
    public partial class frmGetInfoLabels : Form
    {
        public frmGetInfoLabels()
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

        private string GetLabels(string html, out string info)
        {
            info = "";
            string values = "";
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            StringBuilder sb = new StringBuilder();
            StringBuilder sbinfo = new StringBuilder();
            if (doc.DocumentNode.ChildNodes.Count > 0)
            {

                string pathFormLayoutTable = @"//td[@class='ListForm']//form";
                if (doc.DocumentNode.SelectNodes(pathFormLayoutTable) != null)
                {
                    HtmlNode myform = doc.DocumentNode.SelectNodes(pathFormLayoutTable).FirstOrDefault();
                    sbinfo.AppendLine(string.Format("{0}", Path.GetFileName(myform.Attributes["action"].Value)));
                    sbinfo.AppendLine(string.Format("{0}", "".PadLeft(100, "-".ToCharArray().FirstOrDefault())));
                }

                string pathListLayoutTableDiv = @"//div[@id='PageHeader']/div[substring(@class, string-length(@class) - string-length('FormTitle') +1) = 'FormTitle']";                                
                if (doc.DocumentNode.SelectNodes(pathListLayoutTableDiv) != null)
                {
                    List<HtmlNode> lth = doc.DocumentNode.SelectNodes(pathListLayoutTableDiv).ToList();
                    foreach (HtmlNode itemth in lth)
                    {
                        sbinfo.AppendLine(string.Format("|{0,-30}|", HttpUtility.HtmlDecode(itemth.InnerText).Trim()));
                    }
                    sbinfo.AppendLine(string.Format("{0}", "".PadLeft(100, "-".ToCharArray().FirstOrDefault())));
                }

                string pathListLayoutTable = @"//table[@class='ListLayoutTable']//th";
                if (doc.DocumentNode.SelectNodes(pathListLayoutTable) != null)
                {
                    List<HtmlNode> lth = doc.DocumentNode.SelectNodes(pathListLayoutTable).ToList();
                    foreach (HtmlNode itemth in lth)
                    {
                        sbinfo.AppendLine(string.Format("|{0,-30}|", HttpUtility.HtmlDecode(itemth.InnerText).Trim()));
                    }
                    sbinfo.AppendLine(string.Format("{0}", "".PadLeft(100, "-".ToCharArray().FirstOrDefault())));
                }

                string[] namestag = new string[] { "input", "select", "textarea" };
                string pathFormLayoutTableLabel = @"//table[@class='FormLayoutTable']//label/..";
                if (doc.DocumentNode.SelectNodes(pathFormLayoutTableLabel) != null)
                {                    
                    List<HtmlNode> l = doc.DocumentNode.SelectNodes(pathFormLayoutTableLabel).ToList();
                    foreach (var item in l)
                    {
                        HtmlNode label = item.SelectNodes("./label").FirstOrDefault();
                        if (item.SelectNodes("./following-sibling::td//*[local-name()='input' or local-name()='select' or local-name()='textarea']") != null)
                        {
                            HtmlNode control = item.SelectNodes("./following-sibling::td//*[local-name()='input' or local-name()='select' or local-name()='textarea']").FirstOrDefault();
                            sbinfo.AppendLine(string.Format("|{0,30}|{1,-30}|", HttpUtility.HtmlDecode(label.InnerText).Trim(), control.Attributes["name"].Value.Trim()));
                        }
                        else if (item.SelectNodes(".//*[local-name()='input' or local-name()='select' or local-name()='textarea']") != null)
                        {
                            HtmlNode control = item.SelectNodes(".//*[local-name()='input' or local-name()='select' or local-name()='textarea']").FirstOrDefault();
                            sbinfo.AppendLine(string.Format("|{0,30}|{1,-30}|", HttpUtility.HtmlDecode(label.InnerText).Trim(), control.Attributes["name"].Value.Trim())); 
                        }
                    }                    
                }

                string pathFormLayoutTableTr = @"//table[@class='FormLayoutTable']/tr//*";
                if (doc.DocumentNode.SelectNodes(pathFormLayoutTableTr) != null)
                {
                    foreach (HtmlNode ElemetNode in doc.DocumentNode.SelectNodes(pathFormLayoutTableTr))
                    {
                        if (namestag.Contains(ElemetNode.OriginalName))
                        {
                            HtmlAttribute att = ElemetNode.Attributes["id"];
                            //sb.AppendLine(att.Value.Trim());
                            sb.AppendLine(string.Format("{0}|{1}|{2}", ElemetNode.OriginalName, ElemetNode.GetAttributeValue("type", ""), att.Value.Trim()));
                        }
                    }
                }


                values = sb.ToString();
                info = sbinfo.ToString();
            }
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
