using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace MM.Site
{
    public partial class WebForm2 : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {

            string YourJsonArray = "var json=[];";
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(YourJsonArray);
            Response.End();


        }
    }
}
