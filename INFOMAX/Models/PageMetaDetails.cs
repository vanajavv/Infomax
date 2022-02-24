using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using INFOMAX.DB_Context;

namespace INFOMAX.Models
{
    public class PageMetaDetails
    {
        public static string UpdateMetaDetails(string pageUrl)
        {
            //--- StringBuilder object to store MetaTags information.
            StringBuilder sbMetaTags = new StringBuilder();

            //--Step1 Get data from database.
            using (infomaxEntities db = new infomaxEntities())
            {
                var obj = db.PageMetaDetails.FirstOrDefault(a => a.PageUrl == pageUrl);
                if (obj == null)
                {
                    pageUrl = "Infomax Software Solutions";
                    obj = db.PageMetaDetails.FirstOrDefault(a => a.PageUrl == pageUrl);
                    //---- Step2 In this step we will add <title> tag to our StringBuilder Object.
                    sbMetaTags.Append("<title>" + obj.Title + "</title>");
                    sbMetaTags.Append(Environment.NewLine);

                    //---- Step3 In this step we will add "Meta Description" to our StringBuilder Object.
                    sbMetaTags.Append("<meta name='description'" + " content = " + obj.MetaDescription + "/>");
                    sbMetaTags.Append(Environment.NewLine);
                    //---- Step4 In this step we will add "Meta Keywords" to our StringBuilder Object.
                    sbMetaTags.Append("<meta name='keywords'" + " content = " + obj.MetaKeywords + "/>");
                }
                else
                {
                    //---- Step2 In this step we will add <title> tag to our StringBuilder Object.
                    sbMetaTags.Append("<title>" + obj.Title + "</title>");
                    sbMetaTags.Append(Environment.NewLine);

                    //---- Step3 In this step we will add "Meta Description" to our StringBuilder Object.
                    sbMetaTags.Append("<meta name="+"'"+"description"+"'" + " content = " + obj.MetaDescription + "/>");
                    sbMetaTags.Append(Environment.NewLine);
                    //---- Step4 In this step we will add "Meta Keywords" to our StringBuilder Object.
                    sbMetaTags.Append("<meta name= "+"'"+"keywords"+"'"+" " + " content = " + obj.MetaKeywords + "/>");
                }
            }
            return sbMetaTags.ToString();
        }
    }
}