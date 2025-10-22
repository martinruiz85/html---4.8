using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.IO;

namespace WebApplication1
{
    public partial class ChartAP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // get parameter UserID
            int _UserID;
            if (!int.TryParse(this.Request["vintUserID"], out _UserID))
                _UserID = -1;

            // get parameter HRPlanPerID
            int _HRPlanPerID;
            if (!int.TryParse(this.Request["vintHRPlanPerID"], out _HRPlanPerID))
                _HRPlanPerID = 0;

            // get parameter FeedbackPgmID
            int intFeedbackPgmID;
            int? _FeedbackPgmID = null;
            if (int.TryParse(this.Request["vintFeedbackPgmID"], out intFeedbackPgmID))
                _FeedbackPgmID = intFeedbackPgmID;


            this.Image1.ImageUrl = string.Format("ChartAP.ashx?vintHRPlanPerID={0}&vintFeedbackPgmID={1}&vintUserID={2}", _HRPlanPerID, _FeedbackPgmID, _UserID);

            this.lblPeriodCode.Text = this.Request["vintHRPlanPerIDtext"];
            this.lblProgramTitle.Text = this.Request["vintFeedbackPgmIDtext"] != "" ? this.Request["vintFeedbackPgmIDtext"] : "Todos";

        }
    }
}
