using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Library;
using System.Data;

namespace MM.Site
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CategoriaProducto cp = new CategoriaProducto();

            // simple array json
            string json = cp.ListDic();
            
            // json
            string arrayjson = cp.ArrayListDic();

            //SqlDataReader
            DataTable dt = new DataTable();
            dt.Load(cp.ListDataReader());

            //DataTable
            dt = cp.ListDataTable();

            //DataSet
            DataSet ds = cp.ListDataSet();

            //String
            string _texto = cp.ListScalarStr();

            //Integer
            int _entero = cp.ListScalarInt();

            //Decimal
            decimal _decimal = cp.ListScalarDec();

            //Datetime
            DateTime dtime = cp.ListScalarDtm();

            //Bolean
            bool bit = cp.ListScalarBit();

            
          
        }
    }
}
