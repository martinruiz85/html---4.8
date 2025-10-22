using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MM.Library;
using System.ServiceModel.Web;

namespace MM.Site.services
{
    // NOTE: If you change the interface name "IProductoService" here, you must also update the reference to "IProductoService" in Web.config.
    [ServiceContract]
    public interface IProductoService
    {
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            //UriTemplate = "/List?pageNumber={pageNumber}",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest
            )]
        Response<string> List(int pageNumber);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            //UriTemplate = "/List?pageNumber={pageNumber}",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest
            )]
        Response<string> Get(int ProductoID);


        [OperationContract]
        [WebInvoke(
            Method = "POST",
            //UriTemplate = "/List?pageNumber={pageNumber}",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest
            )]
        Response<string> Save(
           int? ProductoID,
           string Codigo,
           string NombreProducto,
           int? CategoriaID,
           string DescripcionProducto,
           decimal Precio,
           int NoDisponibles,
           bool EsActivo);

    }
}
