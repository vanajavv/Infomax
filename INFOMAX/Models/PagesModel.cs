using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace INFOMAX.Models
{
    public class PagesModel
    {
        public List<SelectListItem> PagesList { get; set; }

        public int? Page_ID { get; set; }
        public string PageName { get; set; }
    }
}