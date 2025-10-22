using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Net;

namespace MM.Library
{
    public class Productos
    {

        public static Dictionary<string, ArrayList>
               Lista(int page)
        {
            

            List<SqlParameter> prms = new List<SqlParameter>();
            SqlParameter total = new SqlParameter("@total", SqlDbType.Int);
            total.Direction = ParameterDirection.Output;

            prms.Add(total);
            prms.Add(new SqlParameter("@page", page));
            prms.Add(new SqlParameter("@records", 24));

            return MM.Data.Querys.ExecArrayDictionary("sp_Producto_Lista", prms);
        }

        public static Dictionary<string, ArrayList>
            Obtener(int ProductoID)
        {
            List<SqlParameter> prms = new List<SqlParameter>();
            prms.Add(new SqlParameter("@ProductoID", ProductoID));
            return MM.Data.Querys.ExecArrayDictionary("sp_Producto_Obtener", prms);
        }

        public static bool
            Guardar(
            int? ProductoID,
            string Codigo,
            string NombreProducto,
            int? CategoriaID,
            string DescripcionProducto,
            decimal Precio,
            int NoDisponibles,
            bool EsActivo)
        {

            List<SqlParameter> prms = new List<SqlParameter>();
            prms.Add(new SqlParameter("@intProductoID", ProductoID));
            prms.Add(new SqlParameter("@strCodigo", Codigo));
            prms.Add(new SqlParameter("@strNombreProducto", NombreProducto));
            prms.Add(new SqlParameter("@intCategoriaID", CategoriaID));
            prms.Add(new SqlParameter("@strDescripcionProducto", DescripcionProducto));
            prms.Add(new SqlParameter("@mnyPrecio", Precio));
            prms.Add(new SqlParameter("@intNoDisponibles", NoDisponibles));
            prms.Add(new SqlParameter("@bitEsActivo", EsActivo));

            return MM.Data.Querys.ExecNonQuery("sp_Producto_Guardar", prms);
        }




    }
}
