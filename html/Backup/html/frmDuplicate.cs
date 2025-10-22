using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

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

                StringBuilder sb2 = new StringBuilder();
                sb2.AppendLine("exec dbo.pfrmXigSubject_Save_CataW");
                sb2.AppendLine("@vintCurUserID = -1,");
                sb2.AppendLine("@vintCurSysLID = 3082,");
                sb2.AppendLine("@rintSubjectID = null,");
                sb2.AppendLine("@vintSysLID	   = 3082,");
                sb2.AppendLine("@vstrCode	   = '{0}',");
                sb2.AppendLine("@vstrCodeDesc  = '',");
                sb2.AppendLine("@vblnIsDefault = 1,");
                sb2.AppendLine("@vintDegreeID  = 2,");
                sb2.AppendLine("@vintSortOrder = 10,");
                sb2.AppendLine("@vblnHide      = 0");
                sb2.AppendLine("GO");


                return
                new List<string>
                {
                        "{0}",
                        "\"{0}\",",
                        "dt.Columns.Add(\"{0}\", typeof(String));",
                        "prms.Add(new SqlParameter(\"{0}\", {0}));",
                        sb1.ToString(),
                        sb2.ToString()
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
                .Select(s => new KeyCount
                {
                    Key = s.Key,
                    Count = s.Count()
                });



            this.dataGridView1.DataSource = query.ToList();
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
                    file.WriteLine(format, line.Cells[0].FormattedValue);


                }
            }
            Process.Start(filename);
        }
    }

    public class KeyCount
    {
        public string Key { get; set; }
        public int Count { get; set; }
    }
}
