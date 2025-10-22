using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilETWeb;
using Base64ToFile;

namespace html
{
	public partial class frmMain: Form
	{
		public frmMain() {
			InitializeComponent();
		}

		private void generateAlterTableToolStripMenuItem_Click(object sender, EventArgs e) {
			frmGenerateAlterColumn frm = new frmGenerateAlterColumn();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void getInfoLabelsToolStripMenuItem_Click(object sender, EventArgs e) {
			frmGetInfoLabels frm = new frmGetInfoLabels();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void compareTwoListOfStringToolStripMenuItem_Click(object sender, EventArgs e) {
			frmCompareTwoListOfString frm = new frmCompareTwoListOfString();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void generateWithSpzScriptObjectToolStripMenuItem_Click(object sender, EventArgs e) {
			frmGenerateScriptObject frm = new frmGenerateScriptObject();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void generateToolStripMenuItem_Click(object sender, EventArgs e) {
			frmGenerateInserts frm = new frmGenerateInserts();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void dependsToolStripMenuItem_Click(object sender, EventArgs e) {
			frmDepends frm = new frmDepends();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void dependsCustomToolStripMenuItem_Click(object sender, EventArgs e) {

			frmDependsCustom frm = new frmDependsCustom();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void generateWithSpzScriptTreeByGuidToolStripMenuItem_Click(object sender, EventArgs e) {
			frmScriptTreeByGuid frm = new frmScriptTreeByGuid();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void generateProceduresToolStripMenuItem_Click(object sender, EventArgs e) {
			frmGenerateProcedures frm = new frmGenerateProcedures();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void getResourceTextToolStripMenuItem_Click(object sender, EventArgs e) {
			frmGetText frm = new frmGetText();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void reToolStripMenuItem_Click(object sender, EventArgs e) {
			frmRenameNumberPrefix frm = new frmRenameNumberPrefix();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void generateInsertsDependsToolStripMenuItem_Click(object sender, EventArgs e) {
			frmGenerateInsertsCustom frm = new frmGenerateInsertsCustom();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void executeBachToolStripMenuItem_Click(object sender, EventArgs e) {
			frmExecuteBach frm = new frmExecuteBach();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void generateSearchToolStripMenuItem1_Click(object sender, EventArgs e) {
			frmGenerateSearchFiles frm = new frmGenerateSearchFiles();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void generateScriptsETWebToolStripMenuItem1_Click(object sender, EventArgs e) {
			frmGenerateScriptsETWeb frm = new frmGenerateScriptsETWeb();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void generateMenuToolStripMenuItem_Click(object sender, EventArgs e) {
			frmGenerateMenu frm = new frmGenerateMenu();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void textToolStripMenuItem_Click(object sender, EventArgs e) {
			frmExtrarRegex frm = new frmExtrarRegex();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void pngToIconToolStripMenuItem_Click(object sender, EventArgs e) {
			ImageToIcon frm = new ImageToIcon();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void reziseToolStripMenuItem_Click(object sender, EventArgs e) {
			ResizeImage frm = new ResizeImage();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void regexToolStripMenuItem_Click(object sender, EventArgs e) {
			frmRegex frm = new frmRegex();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void duplicateToolStripMenuItem_Click(object sender, EventArgs e) {
			frmDuplicate frm = new frmDuplicate();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void downloadImageToolStripMenuItem_Click(object sender, EventArgs e) {
			frmDownloadImage frm = new frmDownloadImage();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void htmlEncodeToolStripMenuItem_Click(object sender, EventArgs e) {
			frmHtmlEncode frm = new frmHtmlEncode();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void drawToolStripMenuItem_Click(object sender, EventArgs e) {
			frmTimeLine frm = new frmTimeLine();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void basToolStripMenuItem_Click(object sender, EventArgs e) {
			frmBase64toFile frm = new frmBase64toFile();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void toolStripMenuItem1_Click(object sender, EventArgs e) {

		}

		private void threadToolStripMenuItem_Click(object sender, EventArgs e) {
			frmConcatenate frm = new frmConcatenate();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void generateAlterColumnToolStripMenuItem_Click(object sender, EventArgs e) {
			frmGenerateAlterColumn frm = new frmGenerateAlterColumn();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();

		}

		Timer t = new Timer();

		private void frmMain_Load(object sender, EventArgs e) {
			t.Interval = 1000;
			t.Tick += new EventHandler(t_Tick);
			//t.Start();
		}

		void t_Tick(object sender, EventArgs e) {
			//this.Text = ((DateTime.Now.TimeOfDay.TotalSeconds / 86400.00) * 24.00).ToString("c");
		}

		private void calendarToolStripMenuItem_Click(object sender, EventArgs e) {
			frmCalendarP2P frm = new frmCalendarP2P();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();

		}

		private void generateInsertsMultipleToolStripMenuItem_Click(object sender, EventArgs e) {
			frmGenerateInsertsMultiple frm = new frmGenerateInsertsMultiple();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void fontToolStripMenuItem_Click(object sender, EventArgs e) {
			frmFont frm = new frmFont();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void effectToolStripMenuItem_Click(object sender, EventArgs e) {
			frmEffect frm = new frmEffect();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void concurrenciaToolStripMenuItem_Click(object sender, EventArgs e) {
			frmConcurrencia frm = new frmConcurrencia();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();

		}

		private void graficaToolStripMenuItem_Click(object sender, EventArgs e) {

		}

		private void graficaToolStripMenuItem1_Click(object sender, EventArgs e) {
			frmGrafica frm = new frmGrafica();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void grafica2ToolStripMenuItem_Click(object sender, EventArgs e) {
			frmGraficaXig frm = new frmGraficaXig();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void resizeHeightToolStripMenuItem_Click(object sender, EventArgs e) {
			frmResize frm = new frmResize();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void toolStripMenuItem1_Click_1(object sender, EventArgs e) {

		}

		private void xmlNodeToolStripMenuItem_Click(object sender, EventArgs e) {
			frmReportNodes frm = new frmReportNodes();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void xmlNode2ToolStripMenuItem_Click(object sender, EventArgs e) {
			frmReportNodes2 frm = new frmReportNodes2();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void xmlNodeFileToolStripMenuItem_Click(object sender, EventArgs e) {
			frmGetReportDataSorce frm = new frmGetReportDataSorce();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void generateCodeTableToolStripMenuItem_Click(object sender, EventArgs e) {
			frmGenerateTableCode frm = new frmGenerateTableCode();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void separateInsertsToolStripMenuItem_Click(object sender, EventArgs e) {
			frmSeparateInserts frm = new frmSeparateInserts();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

		private void generateSpPaginateToolStripMenuItem_Click(object sender, EventArgs e) {
			frmGenerateSpPaging frm = new frmGenerateSpPaging();
			frm.WindowState = FormWindowState.Maximized;
			frm.MdiParent = this;
			frm.Show();
		}

        private void joinSqlFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMergeFiles frm = new frmMergeFiles();
            frm.WindowState = FormWindowState.Maximized;
            frm.MdiParent = this;
            frm.Show();
        }
    }
}
