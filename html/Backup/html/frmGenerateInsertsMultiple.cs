using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using System.IO;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using System.Diagnostics;
using System.Configuration;
using System.Threading;
using System.Text.RegularExpressions;
using System.Transactions;

namespace UtilETWeb
{
    public partial class frmGenerateInsertsMultiple : Form
    {
        public frmGenerateInsertsMultiple()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmGenerateInsertsMultiple_Load);
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

        private string _ConnectionString;
        private string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
        }

        //https://stackoverflow.com/questions/1606679/remove-duplicates-in-the-list-using-linq
        class DistinctItemComparer : IEqualityComparer<SqlFile>
        {

            public bool Equals(SqlFile x, SqlFile y)
            {
                return x.RealName == y.RealName;
            }

            public int GetHashCode(SqlFile obj)
            {
                return obj.RealName.GetHashCode();
            }
        }

        private List<SqlFile> listsp
        {
            get
            {
                List<SqlFile> query;
                List<string> l = new List<string>() { };
                string input = this.richTextBox1.Text;
                string pattern = @"\[([^\[]*)\]";

                if (this.checkBox1.Checked)
                {
                    query = input
                       .Split("\n\r".ToCharArray())
                       .CustomSort()
                       .Select(s => new SqlFile()
                       {
                           Name = s,
                           RealName = Regex.Match(s, pattern).Value.Trim("[]".ToCharArray())
                       })
                       .GroupBy(grp => new { grp.RealName }) //remover duplicados
                       .Select(f => f.First()) // tomar el primero de los duplicados
                       .ToList();
                }
                else
                {
                    query = input
                        .Split("\n".ToCharArray())
                        .Distinct() //remover duplicados
                        .Select((s, index) => new SqlFile()
                        {
                            Name = string.Format("{1}-[{0}].sql", s, index.ToString("0000")),
                            RealName = s
                        }).ToList();
                }

                return query;

            }
        }

        void frmGenerateInsertsMultiple_Load(object sender, EventArgs e)
        {
            this.cmbDatabase.DataSource = GetConnexions();
            this.cmbDatabase.ValueMember = "Code";
            this.cmbDatabase.DisplayMember = "Name";
            this.cmbDatabase.SelectedValue = "-1";

            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

            timer1.Interval = 200;
            timer1.Tick += new EventHandler(timer1_Tick);
        }

 

       

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;            
        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string result = e.Result as string;
            current_sql_file = "";
            if (result == "OK")
            {
                this.btnGenerate.Image = Properties.Resources.work;
                Process.Start("explorer.exe", Path.Combine(Environment.CurrentDirectory, @"Scripts"));
            }
            else
            {
                this.btnGenerate.Image = Properties.Resources.exclamation;
                errorProvider1.SetError(btnGenerate, result);
            }

            this.timer1.Stop();

            this.richTextBox1.SelectAll();
            //this.richTextBox1.SelectionIndent = 0;
            this.richTextBox1.SelectionBackColor = System.Drawing.Color.White;
            this.richTextBox1.DeselectAll();
            this.richTextBox1.Enabled = true;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren())
            {
                if (!backgroundWorker1.IsBusy)
                {
                    // clean richTextBox1
                    this.richTextBox1.SelectAll();
                    //this.richTextBox1.SelectionIndent = 0;
                    this.richTextBox1.SelectionBackColor = System.Drawing.Color.White;
                    this.richTextBox1.DeselectAll();
                    this.richTextBox1.Enabled = false;


                    this.timer1.Start();

                    this.progressBar1.Value = 0;
                    errorProvider1.SetError(btnGenerate, "");
                    this.clean();
                    _ConnectionString = this.cmbDatabase.SelectedValue as string;
                    this.btnGenerate.Image = Properties.Resources.generator;
                    backgroundWorker1.RunWorkerAsync(this.listsp);
                }
            }
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            this.richTextBox1.SelectAll();
            //this.richTextBox1.SelectionIndent = 0;
            this.richTextBox1.SelectionBackColor = System.Drawing.Color.White;
            this.richTextBox1.DeselectAll();

            string pattern = current_sql_file
                .Replace("$", "\\$")
                .Replace("[", "\\[")
                .Replace("]", "\\]")
                .ToLower();

            Match match = new Regex(pattern, RegexOptions.Multiline).Match(richTextBox1.Text.ToLower());

            while (match.Success)
            {
                //richTextBox1.SelectionIndent = 0;
                this.richTextBox1.Select(match.Index, match.Length);
                this.richTextBox1.SelectionEnd = match.Index + match.Length;
                
                //SIN CONTROL COSTUMIZADO
                //this.richTextBox1.SelectionBackColor = Color.FromArgb(255, 255, 255, current_sql_file_alpha);
                
                //CON CONTROL COSTUMIZADO
                this.richTextBox1.Alpha = current_sql_file_alpha;
                this.richTextBox1.Porcent = current_sql_file_porcet;
                
                //richTextBox1.SelectionIndent = 20;
                match = match.NextMatch();
            }
            
            //this.Text = current_sql_file_alpha.ToString();

           
        }

        void clean()
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, @"Scripts"));
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }

        public string current_sql_file;
        public int current_sql_file_alpha;
        public double current_sql_file_porcet;

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                current_sql_file = "";
                List<SqlFile> objects_names = e.Argument as List<SqlFile>;
                foreach (SqlFile object_name in objects_names.Where(n => !string.IsNullOrEmpty(n.RealName)))
                {
                    backgroundWorker1.ReportProgress((int)(((objects_names.IndexOf(object_name)) / (float)objects_names.Count) * 100.00));                    
                    GeneratorInserts gen = new GeneratorInserts(object_name, this.ConnectionString);
                    gen.PropertyChanged += new PropertyChangedEventHandler(gen_PropertyChanged);
                    gen.Generate();
                    backgroundWorker1.ReportProgress((int)(((objects_names.IndexOf(object_name) + 1) / (float)objects_names.Count) * 100.00));
                    //Thread.Sleep(1000);
                }

                e.Result = "OK";
            }
            catch (Exception ex)
            {
                e.Result = ex.ToString();

            }
        }

        void gen_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            current_sql_file =  ((GeneratorInserts)sender).ObjectName;
            current_sql_file_alpha = 255 - (int)((((GeneratorInserts)sender).Progress / 100.00) * 255);
            current_sql_file_porcet =((GeneratorInserts)sender).Progress / 100.00;
        }

        private void cmbDatabase_Validating(object sender, CancelEventArgs e)
        {
            if (cmbDatabase.SelectedValue.ToString() == "-1")
            {
                errorProvider1.SetError(cmbDatabase, "debe seleccionar una conexion base de datos");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(cmbDatabase, "");
            }
        }
    }

    public class SqlFile
    {
        public string Name { get; set; }
        public string RealName { get; set; }
    }

    public class GeneratorInserts : INotifyPropertyChanged
    {
        private string FileName { get; set; }
        public string ObjectName { get; set; }
        private int Index { get; set; }
        private string ConnectionString { get; set; }

        private int _progress = 0;
        public int Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("Progress");

            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public GeneratorInserts(SqlFile sql_file, string _ConnectionString)
        {
            this.FileName = Path.Combine(Environment.CurrentDirectory, Path.Combine(@"Scripts", sql_file.Name));
            this.ObjectName = sql_file.RealName;
            this.ConnectionString = _ConnectionString;
        }

        void myConnection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            TextWriter sw = new StreamWriter(this.FileName, true, Encoding.UTF8);
            sw.WriteLine(e.Message);
            sw.Close();
        }


        public void Generate()
        {
            try
            {
                //File.WriteAllText(this.FileName, String.Empty);
                List<SqlParameter> l = new List<SqlParameter>();
                l.Add(new SqlParameter("@table_name", ObjectName));

                using (SqlConnection c = new SqlConnection(this.ConnectionString))
                {
                    c.Open();

                    int _total = 0;

                    using (SqlCommand cmd = new SqlCommand(string.Format("select count(*) from {0}", ObjectName), c) { CommandType = CommandType.Text })
                    {

                        cmd.CommandTimeout = 600;
                        SqlDataReader result_total = cmd.ExecuteReader();
                        if (result_total.Read())
                        {
                            _total = (int)result_total[0];
                            result_total.Close();
                        }
                    }

                    c.InfoMessage += new SqlInfoMessageEventHandler(myConnection_InfoMessage);
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_generate_inserts", c) { CommandType = CommandType.StoredProcedure })
                    {
                        int count = 0;
                        cmd.Parameters.AddRange(l.ToArray());
                        cmd.CommandTimeout = 600;
                        SqlDataReader result = cmd.ExecuteReader();

                        do
                        {
                            while (result.Read())
                            {
                                //TextWriter sw = new StreamWriter(this.FileName, true, Encoding.GetEncoding(1252));
                                TextWriter sw = new StreamWriter(this.FileName, true, Encoding.UTF8);
                                string text = result[0].ToString();

                                if (count % 50 == 0) sw.WriteLine("GO");

                                sw.WriteLine(text);
                                sw.Close();
                                count += 1;
                                this.Progress = (int)((count / (double)_total) * 100);

                            }
                        }
                        while (result.NextResult());
                    }
                }
            }
            catch (SqlException sql_ex)
            {
                if (sql_ex.ErrorCode == -2146232060)
                    return;
                else
                    throw sql_ex;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

