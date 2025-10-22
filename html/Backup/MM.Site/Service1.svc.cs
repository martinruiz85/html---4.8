using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using MM.Library;
using System.Collections;

namespace MM.Site
{

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in Web.config.
    public class Service1 : IService1
    {
        public Response<string> DoWork2()
        {
            Response<string> result = new Response<string>();
            result.Datos = DateTime.Now.ToLongTimeString();
            return result;
        }

        public Response<List<Dictionary<string, object>>> DoWork3()
        {
            Response<List<Dictionary<string, object>>> result = new Response<List<Dictionary<string, object>>>();

            CategoriaProducto cp = new CategoriaProducto();

            // simple array json
            ArrayList json = cp.TestArrayListDic();

            result.Datos = json.OfType<Dictionary<string, object>>().ToList();
            return result;
        }

        public Response<string> DoWork()
        {
            Response<string> result = new Response<string>();

            CategoriaProducto cp = new CategoriaProducto();

            // simple array json
            string json = cp.ListDic();

            result.Datos = json;
            return result;
        }

        
    }
}
