using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INFOMAX.Models
{
    public class OfficeModel
    {
        public string Office_Name { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string LandLine { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Whatsapp { get; set; }
        public string Email { get; set; }
        public string Web { get; set; }
        public string Loc_Map { get; set; }
        public string Loc_Image { get; set; }
        public int Status { get; set; }
    }
}