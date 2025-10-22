using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace UtilETWeb.Data
{
    public static class Querys
    {
        #region "Fields"
        public static string ConnectionDefault
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DefaultConnectionString"]))
                    return ConfigurationManager.AppSettings["DefaultConnectionString"];
                else
                    return "";
            }
        }


        //public static string ConnectionDefault = ConfigurationManager.ConnectionStrings.Count > 0 ? ConfigurationManager.ConnectionStrings["InsertAplication.Properties.Settings.connectionString"].ConnectionString : null;            

        #endregion

        #region "Connection"

        public static SqlConnection Connection()
        {
            return new SqlConnection(ConnectionDefault);
        }
        #endregion

        #region "Asyncro Methods"
        public static DataTable GetDataTable(string Procedure, ref List<SqlParameter> ParameterList)
        {
            return GetDataTable(Procedure, ref ParameterList, ConnectionDefault);
        }
        public static DataTable GetDataTable(string Procedure, ref List<SqlParameter> ParameterList, string Connection)
        {
            using (SqlConnection con = new SqlConnection(Connection))
            {
                using (SqlCommand cmd = new SqlCommand(Procedure, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ParameterList.ForEach(p => cmd.Parameters.Add(p));
                    con.Open();
                    using (SqlDataReader Reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(Reader);
                        return dt;
                    }
                }
            }
        }
        #endregion

        #region "Command Text Methods"
        public static DataSet CommandTextDataSet(string CommandText)
        {
            return CommandTextDataSet(CommandText, ConnectionDefault);
        }
        public static DataSet CommandTextDataSet(string CommandText, string Connection)
        {
            using (SqlConnection c = new SqlConnection(Connection))
            {
                using (SqlDataAdapter a = new SqlDataAdapter(CommandText, c))
                {
                    c.Open();
                    DataSet ds = new DataSet();
                    a.Fill(ds);
                    return ds;
                }
            }
        }

        public static DataTable CommandTextDataTable(string CommandText)
        {
            return CommandTextDataTable(CommandText, ConnectionDefault);
        }
        public static DataTable CommandTextDataTable(string CommandText, string Connection)
        {
            using (SqlConnection c = new SqlConnection(Connection))
            {
                using (SqlCommand cmd = new SqlCommand(CommandText, c))
                {
                    c.Open();
                    using (SqlDataReader Reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(Reader);
                        return dt;
                    }
                }
            }
        }

        public static SqlDataReader CommandTextReader(string CommandText)
        {
            return CommandTextReader(CommandText, ConnectionDefault);
        }
        public static SqlDataReader CommandTextReader(string CommandText, string Connection)
        {
            SqlConnection c = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                c = new SqlConnection(Connection);
                c.Open();
                cmd = new SqlCommand(CommandText, c);
                return cmd.ExecuteReader();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Object CommandTextScalar(string CommandText)
        {
            return CommandTextScalar(CommandText, ConnectionDefault);
        }
        public static Object CommandTextScalar(string CommandText, string Connection)
        {
            using (SqlConnection c = new SqlConnection(Connection))
            {
                using (SqlCommand cmd = new SqlCommand(CommandText, c))
                {
                    c.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static object CommandText(EnumType Type, string Commandtext)
        {
            return CommandText(Type, Commandtext, ConnectionDefault);
        }
        public static object CommandText(EnumType Type, string CommandText, string Connection)
        {
            switch (Type)
            {
                case EnumType.Reader:
                    return CommandTextReader(CommandText, Connection);
                case EnumType.DataTable:
                    return CommandTextDataTable(CommandText, Connection);
                case EnumType.DataSet:
                    return CommandTextDataSet(CommandText, Connection);
                case EnumType.Scalar:
                    return CommandTextScalar(CommandText, Connection);
                default:
                    throw new Exception("No se encontro un EnumType valido");
            }
        }
        #endregion

        #region "Procedure Methods"
        public static Object ExecScalar(string Procedure)
        {
            return ExecScalar(Procedure, new List<SqlParameter>() { }, ConnectionDefault);
        }
        public static Object ExecScalar(string Procedure, string Connection)
        {
            return ExecScalar(Procedure, new List<SqlParameter>() { }, Connection);
        }
        public static Object ExecScalar(string Procedure, List<SqlParameter> ParameterList)
        {
            return ExecScalar(Procedure, ParameterList, ConnectionDefault);
        }
        public static Object ExecScalar(string Procedure, List<SqlParameter> ParameterList, string Connection)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                con = new SqlConnection(Connection);
                con.Open();
                cmd = new SqlCommand(Procedure, con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddRange(ParameterList.ToArray());
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }
        public static Object ExecScalar(string Procedure, List<SqlParameter> ParameterList, string Connection, SqlTransaction tran)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                con = new SqlConnection(Connection);
                con.Open();
                if (tran != null) tran = con.BeginTransaction();
                cmd = new SqlCommand(Procedure, con, tran) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddRange(ParameterList.ToArray());
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }



        public static bool ExecNonQuery(string Procedure)
        {
            return ExecNonQuery(Procedure, new List<SqlParameter>() { }, ConnectionDefault);
        }
        public static bool ExecNonQuery(string Procedure, string Connection)
        {
            return ExecNonQuery(Procedure, new List<SqlParameter>() { }, Connection);
        }
        public static bool ExecNonQuery(string Procedure, List<SqlParameter> ParameterList)
        {
            return ExecNonQuery(Procedure, ParameterList, ConnectionDefault);
        }
        public static bool ExecNonQuery(string Procedure, List<SqlParameter> ParameterList, string Connection)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                con = new SqlConnection(Connection);
                con.Open();
                cmd = new SqlCommand(Procedure, con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddRange(ParameterList.ToArray());
                if (cmd.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }
        public static bool ExecNonQuery(string Procedure, List<SqlParameter> ParameterList, string Connection, SqlTransaction tran)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                con = new SqlConnection(Connection);
                con.Open();
                if (tran != null) tran = con.BeginTransaction();
                cmd = new SqlCommand(Procedure, con, tran) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddRange(ParameterList.ToArray());
                if (cmd.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }



        public static DataSet ExecDataSet(string Procedure)
        {
            return ExecDataSet(Procedure, new List<SqlParameter>() { }, ConnectionDefault);
        }
        public static DataSet ExecDataSet(string Procedure, string Connection)
        {
            return ExecDataSet(Procedure, new List<SqlParameter>() { }, Connection);
        }
        public static DataSet ExecDataSet(string Procedure, List<SqlParameter> ParameterList)
        {
            return ExecDataSet(Procedure, ParameterList, ConnectionDefault);
        }
        public static DataSet ExecDataSet(string Procedure, List<SqlParameter> ParameterList, string Connection)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            try
            {
                con = new SqlConnection(Connection);
                con.Open();
                cmd = new SqlCommand(Procedure, con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddRange(ParameterList.ToArray());
                da = new SqlDataAdapter() { SelectCommand = cmd };
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                da.Dispose();
                cmd.Dispose();
                con.Close();
            }
        }

        public static DataTable ExecDatatable(string Procedure)
        {
            return ExecDatatable(Procedure, new List<SqlParameter>() { }, ConnectionDefault);
        }
        public static DataTable ExecDatatable(string Procedure, string Connection)
        {
            return ExecDatatable(Procedure, new List<SqlParameter>() { }, Connection);
        }
        public static DataTable ExecDatatable(string Procedure, List<SqlParameter> ParameterList)
        {
            return ExecDatatable(Procedure, ParameterList, ConnectionDefault);
        }
        public static DataTable ExecDatatable(string Procedure, List<SqlParameter> ParameterList, string Connection)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                con = new SqlConnection(Connection);
                cmd = new SqlCommand(Procedure, con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddRange(ParameterList.ToArray());
                con.Open();
                SqlDataReader Reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(Reader);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                con.Dispose();
            }
        }

        public static SqlDataReader ExecDataReader(string Procedure)
        {
            return ExecDataReader(Procedure, new List<SqlParameter>() { }, ConnectionDefault);
        }
        public static SqlDataReader ExecDataReader(string Procedure, string Connection)
        {
            return ExecDataReader(Procedure, new List<SqlParameter>() { }, Connection);
        }
        public static SqlDataReader ExecDataReader(string Procedure, List<SqlParameter> ParameterList)
        {
            return ExecDataReader(Procedure, ParameterList, ConnectionDefault);
        }
        public static SqlDataReader ExecDataReader(string Procedure, List<SqlParameter> ParameterList, string Connection)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                con = new SqlConnection(Connection);
                cmd = new SqlCommand(Procedure, con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddRange(ParameterList.ToArray());
                con.Open();
                return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //cmd.Dispose();
                //con.Dispose();
            }
        }


        public static System.Xml.XmlReader ExecDataXmlReader(string Procedure)
        {
            return ExecDataXmlReader(Procedure, new List<SqlParameter>() { }, ConnectionDefault);
        }
        public static System.Xml.XmlReader ExecDataXmlReader(string Procedure, string Connection)
        {
            return ExecDataXmlReader(Procedure, new List<SqlParameter>() { }, Connection);
        }
        public static System.Xml.XmlReader ExecDataXmlReader(string Procedure, List<SqlParameter> ParameterList)
        {
            return ExecDataXmlReader(Procedure, ParameterList, ConnectionDefault);
        }
        public static System.Xml.XmlReader ExecDataXmlReader(string Procedure, List<SqlParameter> ParameterList, string Connection)
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                con = new SqlConnection(Connection);
                cmd = new SqlCommand(Procedure, con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddRange(ParameterList.ToArray());
                con.Open();
                return cmd.ExecuteXmlReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //cmd.Dispose();
                //con.Dispose();
            }
        }




        public static object Exec(EnumType Type, string Procedure)
        {
            return Exec(Type, Procedure, new List<SqlParameter>() { }, ConnectionDefault);
        }
        public static object Exec(EnumType Type, string Procedure, string Connection)
        {
            return Exec(Type, Procedure, new List<SqlParameter>() { }, Connection);
        }
        public static object Exec(EnumType Type, string Procedure, List<SqlParameter> ParameterList)
        {
            return Exec(Type, Procedure, ParameterList, ConnectionDefault);
        }
        public static object Exec(EnumType Type, string Procedure, List<SqlParameter> ParameterList, string Connection)
        {
            switch (Type)
            {
                case EnumType.Reader:
                    return ExecDataReader(Procedure, ParameterList, Connection);
                case EnumType.XmlReader:
                    return ExecDataXmlReader(Procedure, ParameterList, Connection);
                case EnumType.DataTable:
                    return ExecDatatable(Procedure, ParameterList, Connection);
                case EnumType.DataSet:
                    return ExecDataSet(Procedure, ParameterList, Connection);
                case EnumType.Scalar:
                    return ExecScalar(Procedure, ParameterList, Connection);
                case EnumType.NonQuery:
                    return ExecNonQuery(Procedure, ParameterList, Connection);
                default:
                    throw new Exception("No se encontro un EnumType valido");
            }
        }
        #endregion

    }
}
