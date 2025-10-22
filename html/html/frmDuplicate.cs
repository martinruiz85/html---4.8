using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;

namespace UtilETWeb
{
    public partial class frmDuplicate : Form
    {
        public frmDuplicate()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmDuplicate_Load);

            // Font
            FontFamily family = new FontFamily("Times New Roman");
            Font font = new Font(family, 16.0f,
            FontStyle.Bold | FontStyle.Italic);

            this.comboBox1.Font = font;
            this.comboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            this.comboBox1.DrawItem += new DrawItemEventHandler(cbxDesign_DrawItem);

            richTextBox1.WordWrap = false;
            richTextBox2.WordWrap = false;


        }




        List<string> FormatResult
        {
            get
            {
                StringBuilder sb1 = new StringBuilder();
                sb1.AppendLine("exec dbo.pfrmXigSubject_Save_CataW");
                sb1.AppendLine("@vintCurUserID = -1,");
                sb1.AppendLine("@vintCurSysLID = 3082,");
                sb1.AppendLine("@rintSubjectID = null,");
                sb1.AppendLine("@vintSysLID    = 3082,");
                sb1.AppendLine("@vstrCode      = '{0}',");
                sb1.AppendLine("@vstrCodeDesc  = '',");
                sb1.AppendLine("@vblnIsDefault = 1,");
                sb1.AppendLine("@vintDegreeID  = 4,");
                sb1.AppendLine("@vintSortOrder = 10,");
                sb1.AppendLine("@vblnHide      = 0");
                sb1.AppendLine("GO");

                return
                new List<string>
                {
                    "{0}",
                    "\'{0}\',",
                    "\"{0}\",",
                    "[{0}],",
                    "T.{0},",
                    "dt.Columns.Add(\"{0}\", typeof(String));",
                    "prms.Add(new SqlParameter(\"{0}\", {0}));",
                    @"mcmdSaveForm.Parameters(""{0}"").Value = mctlForm.GetControl(""{0}"").Value",
                    sb1.ToString()
                };
            }
        }


        private void cbxDesign_DrawItem(object sender, DrawItemEventArgs e)
        {
            // By using Sender, one method could handle multiple ComboBoxes
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                // Always draw the background
                e.DrawBackground();

                // Drawing one of the items?
                if (e.Index >= 0)
                {
                    // Set the string alignment.  Choices are Center, Near and Far
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings
                    // Assumes Brush is solid
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                        brush = SystemBrushes.HighlightText;

                    // Font
                    FontFamily family = new FontFamily("Times New Roman");
                    Font font = new Font(family, 16.0f,
                    FontStyle.Bold | FontStyle.Italic);

                    // Draw the string
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), font, brush, e.Bounds, sf);
                }
            }
        }

        void frmDuplicate_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;

            this.cmbFormat.DataSource = this.FormatResult;
        }


        public string[] TextString
        {
            get
            {
                return this.richTextBox1.Text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {

            IEnumerable<KeyCount> query = TextString
                .GroupBy(s => s)
                .Select((s, i) => new KeyCount
                {
                    Key = s.Key,
                    Index = i + (this.chkStartIndex.Checked ? 0 : 1),
                    Count = s.Count()
                });

            var dt = new DataTable();
            dt.Columns.Add("Key", typeof(string));
            dt.Columns.Add("Index", typeof(int));
            dt.Columns.Add("Count", typeof(int));

            foreach (var item in query)
                dt.Rows.Add(item.Key, item.Index, item.Count);


            this.dataGridView1.DataSource = dt;
            this.richTextBox2.Text = string.Join(this.comboBox1.Text, query.Select(kc => kc.Key).ToArray());

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string filename = string.Format("{0}.txt", Guid.NewGuid());
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
            {
                foreach (DataGridViewRow line in this.dataGridView1.Rows)
                {
                    string format = this.cmbFormat.SelectedValue != null ? this.cmbFormat.SelectedValue.ToString() : this.cmbFormat.Text;
                    if (this.chkSingleLine.Checked)
                        file.Write(format, line.Cells[0].FormattedValue, line.Cells[1].Value, line.Cells[2].Value);
                    else
                        file.WriteLine(format, line.Cells[0].FormattedValue, line.Cells[1].Value, line.Cells[2].Value);
                }
            }
            Process.Start(filename);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }

    public class KeyCount
    {
        public string Key
        {
            get; set;
        }

        public int Index
        {
            get; set;
        }

        public int Count
        {
            get; set;
        }
    }
}
