using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilETWeb.Data;
using System.IO;
using JLR.Utils;
using System.Threading;

// https://stackoverflow.com/questions/5082345/base64-encoding-in-sql-server-2005-t-sql

//select * from Images as T ORDER by T.FileName desc

//drop table Images

//create table Images(
//FileAttach image,
//[FileName] varchar(255)
//)

namespace UtilETWeb
{
    public partial class frmDownloadImage : Form
    {
        List<BackgroundWorker> lbw = new List<BackgroundWorker>();

        public frmDownloadImage()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmDownloadImage_Load);
            this.dataGridView1.CellContentClick += new DataGridViewCellEventHandler(dataGridView1_CellContentClick);
            this.dataGridView1.DataError += new DataGridViewDataErrorEventHandler(DataGridView1_DataError);
            this.dataGridView1.CellFormatting += new DataGridViewCellFormattingEventHandler(dataGridView1_CellFormatting);
        }

        void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewDisableButtonCell CellDownload = this.dataGridView1["ColDownload", e.RowIndex] as DataGridViewDisableButtonCell;
            if (e.RowIndex > -1 && this.dataGridView1.Columns[e.ColumnIndex].Name == "ColIcon")
            {
                if (!File.Exists(this.dataGridView1["FileName", e.RowIndex].FormattedValue.ToString()))
                    File.Create(this.dataGridView1["FileName", e.RowIndex].FormattedValue.ToString());

                Icon iconForFile = SystemIcons.WinLogo;
                iconForFile = Icon.ExtractAssociatedIcon(this.dataGridView1["FileName", e.RowIndex].FormattedValue.ToString());
                if (e.Value != null)
                {
                    e.Value = iconForFile;
                }
                //File.Delete(this.dataGridView1["FileName", e.RowIndex].FormattedValue.ToString());
            }
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            FileParams fileparams = e.Result as FileParams;
            DataGridViewDisableButtonCell CellDownload = this.dataGridView1["ColDownload", fileparams.RowIndex] as DataGridViewDisableButtonCell;
            CellDownload.Enabled = true;
            this.dataGridView1["ColImage", fileparams.RowIndex].Value = WhiteImage.Instance.Bmp;
            this.dataGridView1.Refresh();
            if (File.Exists(fileparams.Name))
                System.Diagnostics.Process.Start(fileparams.Name);
        }

        private Object thisLock = new Object();

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            FileParams fileparams = e.Argument as FileParams;
            try
            {
                lock (thisLock)
                {
                    File.WriteAllBytes(fileparams.Name, fileparams.Data);
                }
                e.Result = fileparams;
                Thread.Sleep(1000);
            }
            catch (System.IO.IOException ex)
            {
                e.Result = fileparams;
            }
        }

        void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && this.dataGridView1.Columns[e.ColumnIndex].Name == "ColDownload")
            {
                string name = this.dataGridView1["FileName", e.RowIndex].FormattedValue.ToString();
                byte[] data = this.dataGridView1["FileAttach", e.RowIndex].Value as byte[];
                //File.WriteAllBytes(name, data);
                //System.Diagnostics.Process.Start(name);
                DataGridViewDisableButtonCell CellDownload = this.dataGridView1["ColDownload", e.RowIndex] as DataGridViewDisableButtonCell;
                if (!this.lbw[e.RowIndex].IsBusy && CellDownload.Enabled)
                {
                    CellDownload.Enabled = false;
                    this.dataGridView1["ColImage", e.RowIndex].Value = Properties.Resources.generator;
                    this.lbw[e.RowIndex].RunWorkerAsync(new FileParams()
                    {
                        Data = data,
                        Name = name,
                        RowIndex = e.RowIndex
                    });
                }
            }
            else if (e.RowIndex > -1 && this.dataGridView1.Columns[e.ColumnIndex].Name == "ColDel")
            {
                string name = this.dataGridView1["FileName", e.RowIndex].FormattedValue.ToString();
                this.DeleteData(name);
                this.DataBind();
            }
        }

        protected DataGridViewColumn[] CreateCols()
        {

            DataGridViewDisableButtonColumn ColDownload = new DataGridViewDisableButtonColumn() { };
            ColDownload.Name = "ColDownload";
            ColDownload.HeaderText = "Download";
            ColDownload.UseColumnTextForButtonValue = true;
            ColDownload.Text = "Download";

            DataGridViewImageColumn ColImage = new DataGridViewImageColumn() { };
            ColImage.Name = "ColImage";
            ColImage.HeaderText = "";
            ColImage.Image = WhiteImage.Instance.Bmp;
            ColImage.Width = 20;
            ColImage.ReadOnly = true;
            ColImage.DefaultCellStyle.BackColor = Color.White;


            DataGridViewTextBoxColumn ColData = new DataGridViewTextBoxColumn();
            ColData.Name = "FileAttach";
            ColData.HeaderText = "Data";
            ColData.Visible = false;
            ColData.DataPropertyName = "FileAttach";

            DataGridViewTextBoxColumn ColName = new DataGridViewTextBoxColumn();
            ColName.Name = "FileName";
            ColName.HeaderText = "Name";
            ColName.DataPropertyName = "FileName";

            DataGridViewImageColumn ColIcon = new DataGridViewImageColumn() { };
            ColIcon.Name = "ColIcon";
            ColIcon.HeaderText = "";
            ColIcon.Image = new Bitmap(16, 16);
            ColIcon.Width = 20;
            ColIcon.ReadOnly = true;
            ColIcon.ImageLayout = DataGridViewImageCellLayout.Stretch;

            DataGridViewLinkColumn ColDel = new DataGridViewLinkColumn() { };
            ColDel.Name = "ColDel";
            ColDel.HeaderText = "";
            ColDel.UseColumnTextForLinkValue = true;
            ColDel.Text = "Delete";


            return new DataGridViewColumn[] 
            { 
                ColDownload, 
                ColImage,
                ColData,
                ColName,
                ColIcon,
                ColDel
            };

        }

        protected virtual void DeleteData(string path)
        {
            Querys.CommandTextScalar(
                string.Format("delete T from Images as T where T.FileName = '{0}'", Path.GetFileName(path)),
                @"Data Source=vxmtymxintdev01\intsqldev01;Initial Catalog=Becas;Integrated Security=False;Uid=sa;Pwd=chopito;"
                //"Data Source=(local);Initial Catalog=Mortgage;Integrated Security=True"
                );
        }

        protected virtual DataTable GetData()
        {
            return Querys.CommandText(
                EnumType.DataTable,
                "select * from Images as T ORDER by T.FileName desc",
                @"Data Source=vxmtymxintdev01\intsqldev01;Initial Catalog=Becas;Integrated Security=False;Uid=sa;Pwd=chopito;"
                //"Data Source=(local);Initial Catalog=Mortgage;Integrated Security=True"
                )
                as DataTable;
        }

        protected virtual void InsertData(string path)
        {

            
            Byte[] bytes = File.ReadAllBytes(path);
            String file = Convert.ToBase64String(bytes);

            //SELF SERVER
            //StringBuilder commandtext = new StringBuilder();
            //commandtext.Append("INSERT INTO Images(FileAttach,[FileName])");
            //commandtext.AppendFormat("SELECT	x.BulkColumn as FileAttach,'{1}' FROM	OPENROWSET(BULK '{0}', SINGLE_BLOB) AS x;", path, Path.GetFileName(path));

            StringBuilder commandtext = new StringBuilder();
            commandtext.Append("DECLARE @str VARCHAR(MAX)");
            commandtext.AppendFormat("SET @str = '{0}'", file);
            commandtext.Append("INSERT INTO Images(FileAttach,[FileName])");
            commandtext.AppendFormat("SELECT CAST(N'' AS XML).value('xs:base64Binary(sql:variable(\"@str\"))', 'VARBINARY(MAX)'),'{0}'", Path.GetFileName(path));

            Querys.CommandTextScalar(
                commandtext.ToString(),
                @"Data Source=vxmtymxintdev01\intsqldev01;Initial Catalog=Becas;Integrated Security=False;Uid=sa;Pwd=chopito;"
                //"Data Source=(local);Initial Catalog=Mortgage;Integrated Security=True"
                );       
        }

        protected virtual void DataBind()
        {
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.DataSource = GetData();
            this.lbw.Clear();
            foreach (DataGridViewRow item in this.dataGridView1.Rows)
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                this.lbw.Add(bw);
            }
        }

        //http://stackoverflow.com/questions/4570128/how-to-include-an-animated-gif-in-a-datagridview
        //https://docs.google.com/open?id=0B1r6und31C6BQXktM2VQN1Jza2c        
        private DataGridViewImageAnimator dataGridImageAnimator;

        void frmDownloadImage_Load(object sender, EventArgs e)
        {
            this.dataGridImageAnimator = new DataGridViewImageAnimator(dataGridView1);
            this.dataGridView1.Columns.AddRange(this.CreateCols());
            this.DataBind();
        }

        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {

            //MessageBox.Show("Error happened " + anError.Context.ToString());

            if (anError.Context == DataGridViewDataErrorContexts.Commit)
            {
                MessageBox.Show("Commit error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {
                MessageBox.Show("Cell change");
            }
            if (anError.Context == DataGridViewDataErrorContexts.Parsing)
            {
                MessageBox.Show("parsing error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
            {
                MessageBox.Show("leave control error");
            }

            if ((anError.Exception) is ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[anError.RowIndex].ErrorText = "an error";
                view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";

                anError.ThrowException = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            this.InsertData(this.textBox1.Text);
            this.DataBind();

        }

        private void btnDialog_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = dialog.FileName;
            }
            dialog.Dispose();
        }
    }

    public class FileParams
    {
        public byte[] Data { get; set; }
        public string Name { get; set; }
        public int RowIndex { get; set; }
    }

    public class WhiteImage
    {
        private static WhiteImage _instance;

        private WhiteImage()
        {
            Graphics g = Graphics.FromImage(this.Bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;

            //g.DrawEllipse(Pens.Black, new Rectangle(0, 0, this.Bmp.Width - 1, this.Bmp.Height - 1));

            //g.DrawEllipse(Pens.CadetBlue, CircleFromRectangle(new PointF(16 / 2, 16 / 2), 7));
            //g.FillEllipse(Brushes.CadetBlue, Rectangle.Round(CircleFromRectangle(new PointF(16 / 2, 16 / 2), 5)));

            g.DrawImage(Properties.Resources.work, 0, 0);
        }

        RectangleF CircleFromRectangle(PointF midPoint, float radius)
        {
            return new RectangleF(midPoint.X - radius,
                                 midPoint.Y - radius,
                                 radius * 2,
                                 radius * 2);
        }

        public static WhiteImage Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new WhiteImage();

                return _instance;
            }
        }

        // Your properties can then be whatever you want
        Bitmap bmp = new Bitmap(16, 16);
        public Bitmap Bmp
        {
            get
            {
                return bmp;
            }
        }
    }
}
