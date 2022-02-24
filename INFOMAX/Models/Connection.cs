using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace INFOMAX.Models
{
    public class Connection
    {
        public string s;
        public string rt;
        public SqlConnection con = new SqlConnection("server=.;uid=sa;password=admin@123;database=infomax");
        public SqlCommand cmd = new SqlCommand();
        SqlDataAdapter ada = new SqlDataAdapter();
        public DataTable dt;
        public DataSet ds = new DataSet();
        public SqlDataAdapter ad;

        private SqlConnection getConnect()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            else
            {
                con.Open();
            }
            return con;
        }
        public string exeUpdate(String sql)
        {
            cmd.Connection = getConnect();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            s = cmd.ExecuteNonQuery().ToString();
            cmd.Connection = getConnect();
            return s;

        }

        public DataTable getData(string sql)
        {
            dt = new DataTable();
            cmd.Connection = getConnect();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            ada.SelectCommand = cmd;
            ada.Fill(dt);
            return dt;

        }

        public string execSp(string sql)
        {
            try
            {
                cmd.Connection = getConnect();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sql;
                s = cmd.ExecuteNonQuery().ToString();
                cmd.Connection = getConnect();
                return s;

            }
            catch (Exception ex)
            {
                return ex.ToString();

            }
        }
    }
}