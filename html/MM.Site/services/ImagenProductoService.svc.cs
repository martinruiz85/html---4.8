using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MM.Library;
using Newtonsoft.Json;
using System.ServiceModel.Activation;

namespace MM.Site.services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    // NOTE: If you change the class name "ImagenProductoService" here, you must also update the reference to "ImagenProductoService" in Web.config.
    public class ImagenProductoService : IImagenProductoService
    {
        public Response<string> List(int ProductoID)
        {
            Response<string> result = new Response<string>();
            var dict = ImagenProducto.Lista(ProductoID);
            result.Datos = JsonConvert.SerializeObject(dict);
            return result;
        }

    }
}
