using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace INFOMAX.Models
{

    public class ServicesModel
    {
        public int ID { get; set; }
        public string STitle { get; set; }
        public string Desc { get; set; }
        public string Image { get; set; }


    }
    public class ManageServicesModel
    {
        string strConString = "Data Source=.;Initial Catalog=infomax;Integrated Security=True";

        public int ID { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Image { get; set; }

        public DataTable AllService()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Select * from service" , con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }



        public DataTable ServiceById(int id)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Select * from service where S_ID=" + id, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
  
}