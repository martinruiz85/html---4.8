using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

namespace WebApplication1
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [ScriptService]
    [WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod(Description = "Devuelve Fecha.", MessageName = "DoWork1")]
        public string DoWork()
        {
            return DateTime.Now.ToString();
        }

        [WebMethod(Description = "Devuelve Parametro y Fecha.", MessageName = "DoWork2")]
        //[ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string DoWork(string prms)
        {
            return string.Format("{0}, {1}", prms, DateTime.Now);

            /*
            string s = string.Format("{0}, {1}", prms, DateTime.Now);    
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(s));
            */
        }

        [WebMethod(Description = "Devuelve Lista de cadenas.")]
        public string[] List()
        {
            string[] l = new string[4];

            for (int i = 0; i < l.Length; i++)
            {
                l[i] = i.ToString();
            }

            return l;
        }
    }
}
