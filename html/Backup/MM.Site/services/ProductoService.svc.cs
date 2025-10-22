using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using MM.Library;
using System.Data;
using Newtonsoft.Json;
using System.ServiceModel.Activation;

namespace MM.Site.services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    // NOTE: If you change the class name "ProductoService" here, you must also update the reference to "ProductoService" in Web.config.
    public class ProductoService : IProductoService
    {

        public Response<string> List(int pageNumber)
        {
            Response<string> result = new Response<string>();
            var dict = Productos.Lista(pageNumber);
            result.Datos = JsonConvert.SerializeObject(dict);
            return result;
        }

        public Response<string> Get(int ProductoID)
        {
            Response<string> result = new Response<string>();
            var dict = Productos.Obtener(ProductoID);
            result.Datos = JsonConvert.SerializeObject(dict);
            return result;
        }

        public Response<string> Save(
            int? ProductoID,
            string Codigo,
            string NombreProducto,
            int? CategoriaID,
            string DescripcionProducto,
            decimal Precio,
            int NoDisponibles,
            bool EsActivo)
        {
            System.Threading.Thread.Sleep(1000);
            Response<string> result = new Response<string>();
            bool flag = Productos.Guardar(ProductoID, Codigo, NombreProducto, CategoriaID, DescripcionProducto, Precio, NoDisponibles, EsActivo);
            result.Datos = JsonConvert.SerializeObject(flag);
            return result;
        }

    }
}
