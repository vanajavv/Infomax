using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace INFOMAX.Models
{
    public class ManageProductModel
    {
        public int ID { get; set; }
        [Display(Name = "Product")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Product Name Required")]
        public string Product { get; set; }

        //public string Name { get; set; }
        public Nullable<int> FileSize { get; set; }
        [Display(Name = "Choose File")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Image Required")]
        public string file { get; set; }
        [Display(Name = "Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Product Description Required")]
        public string Desc { get; set; }
        [Display(Name = "Features")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Product Features Required")]
        public string Features { get; set; }
        [Display(Name = "Price")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Product Price Required")]
        public string Price { get; set; }
      //  public Product Product { get; set; }
        string strConString = "Data Source=.;Initial Catalog=infomax;Integrated Security=True";


        public DataTable AllProducts()
        {
            DataTable dt = new DataTable();
            
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Select * from Products", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
        public DataTable ProductById(int id)
        {
            DataTable dt = new DataTable();
          
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Select * from Products where P_ID="+id, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}