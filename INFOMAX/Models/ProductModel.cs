using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace INFOMAX.Models
{
    public class ProductModel
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

    }
}