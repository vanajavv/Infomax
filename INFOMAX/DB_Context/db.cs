using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace INFOMAX.DB_Context
{
    public class db
    {

        SqlConnection con = new SqlConnection("server=.;uid=sa;password=admin@123;database=infomax;");
        //Get Category
        public DataSet get_MainMenu()
        {
            SqlCommand com = new SqlCommand("Select * from MainMenu", con);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        //Get Subcategory()
        public DataSet get_SubMenu(int mid)
        {
            SqlCommand com = new SqlCommand("Select * from SubMenu where Menu_id=@Menu_id", con);
            com.Parameters.AddWithValue("@Menu_id", mid);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

    }
}