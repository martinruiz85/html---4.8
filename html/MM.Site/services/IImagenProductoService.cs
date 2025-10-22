using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using MM.Library;

namespace MM.Site.services
{
    // NOTE: If you change the interface name "IImagenProductoService" here, you must also update the reference to "IImagenProductoService" in Web.config.
    [ServiceContract]
    public interface IImagenProductoService
    {
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            //UriTemplate = "/List?pageNumber={pageNumber}",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest
            )]
        Response<string> List(int ProductoID);
    }
}
