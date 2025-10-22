using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MM.Library;
using System.Data;

namespace MM.Site.handdlers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class hdlImage : IHttpHandler
    {
        public int ProductoID
        {
            get
            {
                int value;
                if (int.TryParse(HttpContext.Current.Request["ProductoID"], out value))
                    return value;
                else
                    return -1;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");

            DataTable dt = ImagenProducto.Obtener(this.ProductoID);
            if (dt.Rows.Count > 0) 
            {
                context.Response.Clear();
                context.Response.ContentType = "Image/jpeg";
                byte[] buffer = dt.Rows[0].Field<byte[]>("Data");
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                context.Response.End();
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
