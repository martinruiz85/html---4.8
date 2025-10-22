using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms.DataVisualization.Charting;

namespace UtilETWeb
{
    public partial class frmGraficaXig : Form
    {
        public frmGraficaXig()
        {
            InitializeComponent();
        }

        public DataSet GetData()
        {
            List<SqlParameter> l = new List<SqlParameter>();
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection c = new SqlConnection(ConfigurationManager.AppSettings["ConnectionStringDEV"]))
                {
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_grap_list", c) { CommandType = CommandType.StoredProcedure })
                    {
                        c.Open();
                        cmd.Parameters.AddRange(l.ToArray());
                        cmd.CommandTimeout = 600;
                        using (SqlDataAdapter da = new SqlDataAdapter() { SelectCommand = cmd })
                        {
                            da.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void frmGraficaXig_Load(object sender, EventArgs e)
        {
            DataSet ds = GetData();
            if (ds.Tables.Count > 0) 
            {

                chart1.Series.Add("points");
                chart1.Series["points"].ChartType =  SeriesChartType.Point;
                chart1.Series["points"].BorderWidth = 2;

                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    chart1.Series["points"].Points.AddXY(item.Field<int>("_grado"), item.Field<decimal?>("_salary"));    
                }

                
                chart1.Series.Add("top");
                chart1.Series["top"].ChartType = SeriesChartType.Line;
                chart1.Series["top"].BorderWidth = 2;

                foreach (DataRow item in ds.Tables[1].Rows)
                {
                    chart1.Series["top"].Points.AddXY(item.Field<int>("_grado"), item.Field<decimal>("_top"));
                }

                chart1.Series.Add("bottom");
                chart1.Series["bottom"].ChartType = SeriesChartType.Line;
                chart1.Series["bottom"].BorderWidth = 2;

                foreach (DataRow item in ds.Tables[1].Rows)
                {
                    chart1.Series["bottom"].Points.AddXY(item.Field<int>("_grado"), item.Field<decimal>("_bottom"));
                }

                chart1.Series.Add("mediana");
                chart1.Series["mediana"].ChartType = SeriesChartType.Line;
                chart1.Series["mediana"].BorderWidth = 2;

                foreach (DataRow item in ds.Tables[1].Rows)
                {
                    chart1.Series["mediana"].Points.AddXY(item.Field<int>("_grado"), item.Field<decimal>("_mediana"));
                }
                
                                            
            }
        }
    }
}
