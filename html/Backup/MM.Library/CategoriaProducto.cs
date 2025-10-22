using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections;

namespace MM.Library
{
    public class CategoriaProducto
    {
        public DataTable ListDataTable() 
        {
            return MM.Data.Querys.Execute<DataTable>( "dbo.sp_Categoria_Lista");
        }

        public SqlDataReader ListDataReader()
        {
            return MM.Data.Querys.Execute<SqlDataReader>("dbo.sp_Categoria_Lista");
        }

        public DataSet ListDataSet()
        {
            return MM.Data.Querys.Execute<DataSet>("dbo.sp_Categoria_Lista");
        }

        public string ListScalarStr()
        {
            return MM.Data.Querys.Execute<string>("dbo.sp_Categoria_Str");
        }

        public int ListScalarInt()
        {
            return MM.Data.Querys.ExecuteSingle<int>("dbo.sp_Categoria_Int");
        }        

        public DateTime ListScalarDtm()
        {
            return MM.Data.Querys.ExecuteSingle<DateTime>("dbo.sp_Categoria_Dtm");
        }

        public bool ListScalarBit()
        {
            return MM.Data.Querys.ExecuteSingle<bool>("dbo.sp_Categoria_Bit");
        }

        public decimal ListScalarDec()
        {
            return MM.Data.Querys.ExecuteSingle<decimal>("dbo.sp_Categoria_Mny");
        }

        public string ListDic()
        {
            ArrayList dict = new ArrayList();

            dict = MM.Data.Querys.ExecDictionary("dbo.sp_Categoria_Lista");

            return JsonConvert.SerializeObject(dict);

        }

        public ArrayList TestArrayListDic()
        {
            ArrayList dict = new ArrayList();

            dict = MM.Data.Querys.ExecDictionary("dbo.sp_Categoria_Lista");

            return dict;

        }

        public string ArrayListDic()
        {
            Dictionary<string, ArrayList> dict = new Dictionary<string, ArrayList>();

            dict = MM.Data.Querys.ExecArrayDictionary("dbo.sp_Categoria_Lista");

            return JsonConvert.SerializeObject(dict); 

        }

    
       


    }
}
