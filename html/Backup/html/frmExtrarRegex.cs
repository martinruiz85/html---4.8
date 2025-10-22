using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace UtilETWeb
{
    public partial class frmExtrarRegex : Form
    {
        public frmExtrarRegex()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmExtrarRegex_Load);
        }

        List<string> IgnoreList = new List<string>();

        private StringBuilder sb = new StringBuilder();

        void frmExtrarRegex_Load(object sender, EventArgs e)
        {
            List<UtilETWeb.frmDependsCustom.EnumModel> enums = ((IEnumerable<FindByKeyValue>)Enum
                   .GetValues(typeof(FindByKeyValue)))
                   .OrderByDescending(c => (int)c)
                   .Select(c => new UtilETWeb.frmDependsCustom.EnumModel()
                   {
                       Value = (int)c,
                       Name = c.GetDescription()
                   }).ToList();

            this.cmbFindby.DisplayMember = "Name";
            this.cmbFindby.ValueMember = "Value";
            this.cmbFindby.DataSource = enums;
        }

        public void RecursivityAsp(string datapage)
        {
            string path = Path.Combine(this.txtDirectory.Text, datapage);


            List<string> files = new List<string>();
            files.Add(path);
            //Regex regex = new Regex("\"([^\"]*)\"");
            List<Regex> lregex = new List<Regex>();

            switch ((FindByKeyValue)this.cmbFindby.SelectedValue)
            {
                case FindByKeyValue.Asp:
                    lregex.Add(new Regex("\"([^\"]*){0,1}.*asp"));
                    break;
                case FindByKeyValue.Sps:
                    lregex.AddRange(new List<Regex>
                    {
                        new Regex("\"([^\"]*)._List[_]{0,1}.*\""),
                        new Regex("\"([^\"]*)._Get[_]{0,1}.*\""),
                        new Regex("\"([^\"]*)._Save[_]{0,1}.*\""),
                        new Regex("\"([^\"]*)._Del[_]{0,1}.*\"")
                    });
                    break;
                case FindByKeyValue.All:
                    lregex.AddRange(new List<Regex>
                    {                        
                        new Regex("\"([^\"]*)rpt{0,1}.*\""),
                        new Regex("\"([^\"]*){0,1}.*asp"),
                        new Regex("\"([^\"]*)._List[_]{0,1}.*\""),
                        new Regex("\"([^\"]*)._Get[_]{0,1}.*\""),
                        new Regex("\"([^\"]*)._Save[_]{0,1}.*\""),
                        new Regex("\"([^\"]*)._Del[_]{0,1}.*\"")
                    });
                    break;

            }


            foreach (var item in files)
            {
                if (!File.Exists(item))
                    continue;

                string[] lines = File.ReadAllLines(item, Encoding.UTF8);

                foreach (string line in lines)
                {
                    for (int i = 0; i < lregex.Count; i++)
                    {
                        Regex regex = lregex[i];
                        
                        string value = regex.Match(line)
                            .Value
                            .Trim('"')
                            .Split("/\"".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).LastOrDefault();

                        if (!string.IsNullOrEmpty(value) && regex.IsMatch(line) && !MyContainsStringList(IgnoreList, value))
                        {
                            sb.AppendFormat("{0}\n", value);

                            if (value.EndsWith(".asp") && !IgnoreList.Contains(value))
                            {
                                IgnoreList.Add(value);

                                if (this.chkRecursive.Checked && !FilePathHasInvalidChars(value))
                                {
                                    RecursivityAsp(value);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static bool FilePathHasInvalidChars(string path)
        {

            return (!string.IsNullOrEmpty(path) && path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0);
        }

        public bool MyContainsStringList(List<string> l, string key)
        {
            bool flag = false;
            for (int i = 0; i < l.Count; i++)
            {
                if (key.ToLower().Contains(l[i].ToLower()))
                    flag = true;
            }
            return flag;
        }

        public enum FindByKeyValue
        {
            All = 3,
            Sps = 2,
            Asp = 1
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sb = new StringBuilder();
            IgnoreList.Clear();
            IgnoreList.AddRange(new List<string>()
            {
                "incAll.asp",
                "incEvents.asp",
                "incAuthorization.asp",
                "incAuthentication.asp",
                "incBanner.asp",
                "incForm.asp",
                "ScrollScript.asp",
                "freeASPUpload.asp",
                "incAll.asp",
                "incAppointment.asp",
                "incAuthentication.asp",
                "incAuthorization.asp",
                "incBanner.asp",
                "incCascadingGoalControls.asp",
                "incCompRating.asp",
                "incConfig.asp",
                "incController.asp",
                "incCustomControls.asp",
                "incDashboardControls.asp",
                "incEMailLink.asp",
                "incErrorNumber.asp",
                "incEvalPerfExitoControls.asp",
                "incEvents.asp",
                "incExcelTemplate.asp",
                "incFeedbackControls.asp",
                "incForm.asp",
                "incFormCalendarControl.asp",
                "incLogin.asp",
                "incMap.asp",
                "incOrgCharter.asp",
                "incPerfPlanControls - 17-03-2016.asp",
                "incPerfPlanControls Backup.asp",
                "incPerfPlanControls(original).asp",
                "incPerfPlanControls.asp",
                "incPowersetAuthorization.asp",
                "incSalaryReviewControls.asp",
                "incTdControls.asp",
                "incTristateBoolean.asp",
                "incUserAgentFilter.asp",
                "incWorkflow.asp",
                "incWorkflowCopy - Copy.asp",
                "incWorkflowCopy.asp",
                "MyResponse.asp"
            });

            RecursivityAsp(this.txtDataPage.Text.Trim());
            this.richTextBox1.Text = sb.ToString();
        }

    }
}
