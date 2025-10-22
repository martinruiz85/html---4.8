using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace MM.Library
{
    public class ImagenProducto
    {
        public static DataTable Obtener(int ProductoID)
        {
            List<SqlParameter> prms = new List<SqlParameter>();
            prms.Add(new SqlParameter("@ProductoID", ProductoID));
            return MM.Data.Querys.Execute<DataTable>("sp_ImagenProducto_Obtener", prms);
        }

        public static Dictionary<string, ArrayList> Lista(int ProductoID)
        {
            List<SqlParameter> prms = new List<SqlParameter>();
            prms.Add(new SqlParameter("@ProductoID", ProductoID));
            return MM.Data.Querys.ExecArrayDictionary("sp_ImagenProducto_Lista", prms);
        }

        
    }
}
