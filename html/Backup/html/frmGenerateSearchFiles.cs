using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace UtilETWeb
{
    public partial class frmGenerateSearchFiles : Form
    {
        public frmGenerateSearchFiles()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmGenerateSearchFiles_Load);
        }

        private string DirectoryPath
        {
            get
            {
                return this.txtPath.Text;
            }
        }

        private string SearchPattern
        {
            get
            {
                return this.txtSearchPatern.Text;
            }
        }



        void frmGenerateSearchFiles_Load(object sender, EventArgs e)
        {

            List<UtilETWeb.frmDependsCustom.EnumModel> enums = ((IEnumerable<SearchOption>)Enum
                    .GetValues(typeof(SearchOption)))
                    .OrderBy(c => (int)c)
                    .Select(c => new UtilETWeb.frmDependsCustom.EnumModel()
                    {
                        Value = (int)c,
                        Name = c.GetDescription()
                    }).ToList();

            this.cmbSearchOption.DisplayMember = "Name";
            this.cmbSearchOption.ValueMember = "Value";
            this.cmbSearchOption.DataSource = enums;

            this.txtSearchPatern.Text = "*.asp";


            List<UtilETWeb.frmDependsCustom.EnumModel> enumstyperesult = ((IEnumerable<ResultType>)Enum
                    .GetValues(typeof(ResultType)))
                    .OrderBy(c => (int)c)
                    .Select(c => new UtilETWeb.frmDependsCustom.EnumModel()
                    {
                        Value = (int)c,
                        Name = c.GetDescription()
                    }).ToList();

            this.cmbTypeResult.DisplayMember = "Name";
            this.cmbTypeResult.ValueMember = "Value";
            this.cmbTypeResult.DataSource = enumstyperesult;


        }

        public enum ResultType
        {
            WindowSearch,
            WindowLikeSearch,
            List,
            ListSeparate,
            ListWithRegex,
            ListWithRegexSeparate,
            Xml,
            XmlWithRegex,
            XMLEncoding,
            StarWdth,
            DoesntContain
        }


        //https://answers.microsoft.com/en-us/windows/forum/windows_7-files/search-for-filenames-starting-with/c5b54309-0778-4f4e-8e28-a5ca739abea2
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            //string[] filePaths = Directory.GetFiles(DirectoryPath, "*pfrmXigPersonAPP4to_*.sql", (SearchOption)this.cmbSearchOption.SelectedValue)
            string[] filePaths = Directory.GetFiles(DirectoryPath, SearchPattern, (SearchOption)this.cmbSearchOption.SelectedValue);
            string query = "";
            XDocument doc = new XDocument();
            string pattern = @"\[([^\[]*)\]";
            switch ((ResultType)Enum.Parse(typeof(ResultType), this.cmbTypeResult.Text))
            {
                case ResultType.WindowSearch:
                    // OLD
                    //query = string.Join(" OR ", filePaths.Select<string, string>(s => string.Format("\"{0}\"", Path.GetFileName(s))).ToArray());
                    //query = string.Format("name:({0})", query);

                    // NEW
                    query = string.Join(" OR ", filePaths.Select<string, string>(s => string.Format("name:=\"{0}\"", Path.GetFileName(s))).ToArray());
                    break;
                case ResultType.WindowLikeSearch:
                    // OLD
                    //query = string.Join(" OR ", filePaths.Select<string, string>(s => string.Format("\"{0}\"", Path.GetFileName(s))).ToArray());
                    //query = string.Format("name:=~({0})", query);

                    // NEW
                    query = string.Join(" OR ", filePaths.Select<string, string>(s => string.Format("name:=(\"{0}\")", Path.GetFileName(s))).ToArray());
                    break;
                case ResultType.List:
                    query = string.Join("\r\n", filePaths.Select<string, string>(s => string.Format("{0}", Path.GetFileName(s))).ToArray());
                    break;
                case ResultType.ListWithRegex:
                    query = string.Join("\r\n", filePaths.Select<string, string>(s => string.Format("{0}", Regex.Match(Path.GetFileName(s), pattern).Value.Trim("[]".ToCharArray()))).ToArray());
                    break;
                case ResultType.Xml:
                    doc.Add(new XElement("root", filePaths.Select(s => new XElement("item", Path.GetFileName(s)))));
                    query = doc.ToString();
                    break;
                case ResultType.XmlWithRegex:                    
                    doc.Add(new XElement("root", filePaths.Select(s => new XElement("item", Regex.Match(Path.GetFileName(s), pattern).Value.Trim("[]".ToCharArray())))));
                    query = doc.ToString();
                    break;
                case ResultType.XMLEncoding:
                    doc.Add(new XElement("root", filePaths.Select(s => new XElement("item", GetFileEncoding(s)))));
                    query = doc.ToString();
                    break;
                case ResultType.ListSeparate:
                    query = string.Join(", ", filePaths.Select<string, string>(s => string.Format("{0}", Path.GetFileName(s))).ToArray());
                    break;
                case ResultType.ListWithRegexSeparate:
                    query = string.Join(", ", filePaths.Select<string, string>(s => string.Format("{0}", Regex.Match(Path.GetFileName(s), pattern).Value.Trim("[]".ToCharArray()))).ToArray());
                    break;
                case ResultType.StarWdth:
                    query = string.Join("\r\n", filePaths.Select<string, string>(s => string.Format(@"name:~<""{0}""", Path.GetFileName(s))).ToArray());
                    break;
                case ResultType.DoesntContain:
                    query = string.Join("\r\n", filePaths.Select<string, string>(s => string.Format(@"name:~!""{0}""", Path.GetFileName(s))).ToArray());
                    break;
                default:
                    break;
            }

            this.txtResult.Text = query;
        }

        public Encoding GetFileEncoding(string srcFile)
        {
            // *** Use Default of Encoding.Default (Ansi CodePage)
            Encoding enc = Encoding.Default;

            // *** Detect byte order mark if any - otherwise assume default
            byte[] buffer = new byte[10];
            FileStream file = new FileStream(srcFile, FileMode.Open);
            file.Read(buffer, 0, 10);
            file.Close();

            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
                enc = Encoding.UTF8;
            else if (buffer[0] == 0xfe && buffer[1] == 0xff)
                enc = Encoding.Unicode;
            else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
                enc = Encoding.UTF32;
            else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
                enc = Encoding.UTF7;
            else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                // 1201 unicodeFFFE Unicode (Big-Endian)
                enc = Encoding.GetEncoding(1201);
            else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                // 1200 utf-16 Unicode
                enc = Encoding.GetEncoding(1200);
            else if (validatUtf8whitBOM(srcFile))

                enc = new UTF8Encoding(false);
            return enc;
        }

        private bool validatUtf8whitBOM(string FileSource)
        {

            bool bReturn = false;

            string TextUTF8 = "", TextANSI = "";

            //lread the file as utf8

            StreamReader srFileWhitBOM = new StreamReader(FileSource);

            TextUTF8 = srFileWhitBOM.ReadToEnd();

            srFileWhitBOM.Close();


            //lread the file as  ANSI

            srFileWhitBOM = new StreamReader(FileSource, Encoding.Default, false);

            TextANSI = srFileWhitBOM.ReadToEnd();

            srFileWhitBOM.Close();

            // if the file contains special characters is UTF8 text read ansi show signs

            if (TextANSI.Contains("Ã") || TextANSI.Contains("±"))
                bReturn = true;

            return bReturn;

        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtPath.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void txtResult_TextChanged(object sender, EventArgs e)
        {
            this.txtResult.SelectAll();
            this.txtResult.Focus();
        }

        private void txtResult_MouseClick(object sender, MouseEventArgs e)
        {
            this.txtResult.SelectAll();
            this.txtResult.Focus();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            string query;
            
            string[] list = richTextBox1.Text.Split("\n\r".ToCharArray());
            query = string.Join(" OR ", list.Select<string, string>(s => string.Format("\"{0}\"", Path.GetFileName(s))).ToArray());
            query = string.Format("name:=~({0})", query);

            this.txtResult.Text = query;
        }
    }

    public static class DirectoryInfoExtensions
    {
        public static System.IO.FileInfo[] GetFiles(this System.IO.DirectoryInfo directoryInfo, string[] searchPatterns)
        {
            return GetFiles(directoryInfo, searchPatterns, System.IO.SearchOption.TopDirectoryOnly);
        }
        public static System.IO.FileInfo[] GetFiles(this System.IO.DirectoryInfo directoryInfo, string[] searchPatterns, System.IO.SearchOption searchOptions)
        {
            List<System.IO.FileInfo> oFileListing = new List<System.IO.FileInfo>();
            foreach (string sSearchPattern in searchPatterns)
            {
                oFileListing.AddRange(directoryInfo.GetFiles(sSearchPattern, searchOptions));
            }
            return oFileListing.ToArray();
        }
    }
}
