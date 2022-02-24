using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using INFOMAX.DB_Context;
using System.IO;
using INFOMAX.Models;
using System.Configuration;

namespace INFOMAX.Controllers
{
    public class AdminDashController : Controller
    {

        infomaxEntities con = new infomaxEntities();
        string strConString = "Data Source=.;Initial Catalog=Infomax;Integrated Security=True";
        // GET: AdminDash
        public ActionResult AdminHome()
        {
            GetName();
                Count();

            return View();
        }
        public void GetName()
        {
            Connection conn = new Connection();
            conn.getData("select * from login where Email='"+ Session["UserName"].ToString() + "'" );
            ViewBag.User = conn.dt.Rows[0]["Name"].ToString();
        }
        [HttpGet]
        public ActionResult UploadSliderImage()
        {
            GetName();
            Count();
            List<Models.CarouselSlider> sliderimages = new List<Models.CarouselSlider>();
            //string CS = ConfigurationManager.ConnectionStrings["infomaxEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strConString))
            {
                SqlCommand cmd = new SqlCommand("spGetAllSliderImage", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Models.CarouselSlider slider = new Models.CarouselSlider();
                    slider.ID = Convert.ToInt32(rdr["ID"]);
                    slider.Name = rdr["Name"].ToString();
                    slider.FileSize = Convert.ToInt32(rdr["FileSize"]);
                    slider.FilePath = rdr["FilePath"].ToString();

                    sliderimages.Add(slider);
                }
            }
            return View(sliderimages);
        }
        [HttpPost]
        public ActionResult UploadSliderImage(HttpPostedFileBase fileupload)
        {
            if (fileupload != null)
            {
                string fileName = Path.GetFileName(fileupload.FileName);
                int fileSize = fileupload.ContentLength;
                int Size = fileSize / 1000;
                fileupload.SaveAs(Server.MapPath("~/SliderImages/" + fileName));

                // string CS = ConfigurationManager.ConnectionStrings["infomaxEntities"].ConnectionString;
                using (SqlConnection con = new SqlConnection(strConString))
                {
                    SqlCommand cmd = new SqlCommand("spAddNewSliderImage", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.AddWithValue("@Name", fileName);
                    cmd.Parameters.AddWithValue("@FileSize", Size);
                    cmd.Parameters.AddWithValue("FilePath", "~/SliderImages/" + fileName);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("UploadSliderImage");
        }
        public ActionResult Delete(int id)
        {
            SqlConnection con = new SqlConnection(strConString);
            SqlCommand cmd = new SqlCommand("delete from CarouselSlider where ID=" + id, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return RedirectToAction("UploadSliderImage");
        }
        public ActionResult Products()
        {
            GetName();
            Count();
            return View();
        }
        [HttpPost]
        public ActionResult Products(HttpPostedFileBase fileupload, ProductModel objPro)
        {

            if (fileupload != null)
            {
                string fileName = Path.GetFileName(fileupload.FileName);
                int fileSize = fileupload.ContentLength;
                int Size = fileSize / 1000;
                fileupload.SaveAs(Server.MapPath("~/Products/" + fileName));

                // string CS = ConfigurationManager.ConnectionStrings["infomaxEntities"].ConnectionString;
                using (SqlConnection con = new SqlConnection(strConString))
                {
                    SqlCommand cmd = new SqlCommand("insert into Products values(@P_Name,@FileSize,@file,@P_Desc,@p_Features,@P_Price)", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    cmd.Parameters.AddWithValue("@P_Name", objPro.Product);
                    cmd.Parameters.AddWithValue("@FileSize", Size);
                    cmd.Parameters.AddWithValue("@file", "~/Products/" + fileName);
                    cmd.Parameters.AddWithValue("@P_Desc", objPro.Desc);
                    cmd.Parameters.AddWithValue("@p_Features", objPro.Features);
                    cmd.Parameters.AddWithValue("@P_Price", objPro.Price);
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    ModelState.Clear();
                    if (i == 1)
                    {
                        ViewBag.Message = "Product Successfully Added";
                    }
                    else
                    {
                        ViewBag.Message = "Failed";
                    }
                }

            }
            return View();
        }
        public ActionResult ManageProducts()
        {
            GetName();
            Count();
            ManageProductModel model = new ManageProductModel();
            DataTable dt = model.AllProducts();
            return View(dt);
        }
        public ActionResult EditProduct(int id)
        {
            ManageProductModel model = new ManageProductModel();
            DataTable dt = model.ProductById(id);
            return PartialView(dt);
            //return View(dt);
        }

        [HttpPost]
        public ActionResult UpdateProduct(HttpPostedFileBase fileupload, ManageProductModel mpm)
        {
            
            //string strConString = "Data Source=.;Initial Catalog=Infomax;Integrated Security=True";
            if (fileupload != null)
            {
                string fileName = Path.GetFileName(fileupload.FileName);
                int fileSize = fileupload.ContentLength;
                int Size = fileSize / 1000;
                fileupload.SaveAs(Server.MapPath("~/Products/" + fileName));
                SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=Infomax;Integrated Security=True");
                // string CS = ConfigurationManager.ConnectionStrings["infomaxEntities"].ConnectionString;
                //{
                SqlCommand cmd = new SqlCommand("update Products set P_Name=@P_Name,FileSize=@FileSize,FilePath=@file,P_Desc=@P_Desc,p_Features=@p_Features,P_Price=@P_Price where P_ID=@P_ID", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.Parameters.AddWithValue("@P_ID", mpm.ID);
                cmd.Parameters.AddWithValue("@P_Name", mpm.Product);
                cmd.Parameters.AddWithValue("@FileSize", Size);
                cmd.Parameters.AddWithValue("@file", "~/Products/" + fileName);
                cmd.Parameters.AddWithValue("@P_Desc", mpm.Desc);
                cmd.Parameters.AddWithValue("@p_Features", mpm.Features);
                cmd.Parameters.AddWithValue("@P_Price", mpm.Price);
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                ModelState.Clear();
                if (i == 1)
                {
                    ViewBag.Message = "Product Successfully Added";
                }
                else
                {
                    ViewBag.Message = "Failed";
                }
                //}
            }
            return RedirectToAction("ManageProducts");
        }
        public ActionResult DeleteProduct(int id)
        {
            SqlConnection con = new SqlConnection(strConString);
            SqlCommand cmd = new SqlCommand("delete from Products where P_ID=" + id, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            //if(i==1)
            //{
            //    ViewBag.Result = "Image Successfully Deleted";
            //}
            //else
            //{
            //    ViewBag.Result = "Can't Delete! Something went Wrong...";
            //}
            return RedirectToAction("ManageProducts");
        }
        public ActionResult Clients()
        {
            GetName();
            Count();
            List<Models.ClientModel> Objclient = new List<Models.ClientModel>();
            //string CS = ConfigurationManager.ConnectionStrings["infomaxEntities"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strConString))
            {
                SqlCommand cmd = new SqlCommand("Select * from clients", con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Models.ClientModel client = new Models.ClientModel();
                    client.ID = Convert.ToInt32(rdr["ID"]);
                    client.Name = rdr["Name"].ToString();
                    //slider.FileSize = Convert.ToInt32(rdr["FileSize"]);
                    client.image = rdr["image"].ToString();

                    Objclient.Add(client);
                }
            }
            return View(Objclient);
        }
        [HttpPost]
        public ActionResult Clients(HttpPostedFileBase image, ClientModel cm)
        {
            if (image != null)
            {
                string fileName = Path.GetFileName(image.FileName);
                int fileSize = image.ContentLength;
                int Size = fileSize / 1000;
                image.SaveAs(Server.MapPath("~/Clients/" + fileName));

                // string CS = ConfigurationManager.ConnectionStrings["infomaxEntities"].ConnectionString;
                using (SqlConnection con = new SqlConnection(strConString))
                {
                    SqlCommand cmd = new SqlCommand("insert into clients values(@Name,@image)", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    cmd.Parameters.AddWithValue("@Name", cm.Name);
                    //cmd.Parameters.AddWithValue("@FileSize", Size);
                    cmd.Parameters.AddWithValue("@image", "~/Clients/" + fileName);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Clients");
        }
        public ActionResult DeleteClient(int id)
        {
            SqlConnection con = new SqlConnection(strConString);
            SqlCommand cmd = new SqlCommand("delete from clients where id=" + id, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            //if(i==1)
            //{
            //    ViewBag.Result = "Image Successfully Deleted";
            //}
            //else
            //{
            //    ViewBag.Result = "Can't Delete! Something went Wrong...";
            //}
            return RedirectToAction("Clients");
        }
        public ActionResult ViewReq_Demo()
        {
            GetName();
            Connection conn1 = new Connection();
            conn1.getData("SELECT *  FROM RequestDemo where Status='0' UNION SELECT * FROM RequestDemo where Status = '1' ORDER BY status Asc, Req_date Asc ");
            ViewBag.TimeMsg = CalCulateTime(Convert.ToDateTime(conn1.dt.Rows[0]["Req_date"].ToString()));
            Count();
            return View(conn1.dt);
        }
        public ActionResult ViewChat(int id)
        {
            Connection conn = new Connection();
            Connection obj = new Connection();
            conn.getData("select * from RequestDemo where Demo_ID=" + id);
            ViewBag.Data = conn.dt;
            ViewBag.TimeMsg = CalCulateTime(Convert.ToDateTime(conn.dt.Rows[0]["Req_date"].ToString()));
            ViewBag.Name = conn.dt.Rows[0]["Name"].ToString();
            ViewBag.Message = conn.dt.Rows[0]["Message"].ToString();
            ViewBag.image = conn.dt.Rows[0]["image"].ToString();
            obj.exeUpdate("update RequestDemo set status='1' where Demo_ID=" + id);
            Count();
            return View("ViewReq_Demo");
        }
        Connection conn = new Connection();
        public void Count()
        {
            Connection objCount = new Connection();
            objCount.getData("SELECT count(*) as'Count' FROM RequestDemo where Status=0");
            ViewBag.count = objCount.dt.Rows[0]["Count"].ToString();
        }
        public string CalCulateTime(DateTime date)
        {
            string message = "";
            DateTime currentDate = DateTime.Now;
            TimeSpan timegap = currentDate - date;
            message = string.Concat(date.ToString("MMMM dd, yyyy"), " at ", date.ToString("hh:mm tt"));
            if (timegap.Days > 365)
            {
                message = string.Concat((((timegap.Days) / 30) / 12), " years/s ago");
            }
            else if (timegap.Days > 31)
            {
                message = string.Concat(((timegap.Days) / 30), " month/s ago");
            }
            else if (timegap.Days > 1)
            {
                message = string.Concat(timegap.Days, " days ago");
            }
            else if (timegap.Days == 1)
            {
                message = " Yesterday";
            }
            else if (timegap.Hours >= 2)
            {
                message = string.Concat(timegap.Hours, " hour/s ago");
            }
            else if (timegap.Hours >= 1)
            {
                message = "an hour ago";
            }
            else if (timegap.Minutes >= 60)
            {
                message = "more than an hour ago";
            }
            else if (timegap.Minutes >= 5)
            {
                message = string.Concat(timegap.Minutes, " minute/s ago");
            }
            else if (timegap.Minutes >= 1)
            {
                message = "a few minute/s ago";
            }
            else
            {
                message = "less than a minute ago";
            }

            return message;
        }

        private SelectList GetPageOptions()           //fetch the tournament type details from the table
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("SELECT * FROM Pages", conn);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {

                    MenuPage.Add(new SelectListItem { Text = myReader["Page"].ToString(), Value = myReader["Page"].ToString() });
                }

                conn.Close();
                return new SelectList(MenuPage, "Value", "Text", "id");  //return the list objects

            }
        }

        string connection = "Data Source=.;Initial Catalog=Infomax;Integrated Security=True";
        List<SelectListItem> MenuPage = new List<SelectListItem>();
        List<SelectListItem> ServicesList = new List<SelectListItem>();
        public ActionResult HeaderImage()
        {
            GetName();
            Connection obj = new Connection();
            Count();
            ViewBag.var1 = GetPageOptions();
            List<Models.HeaderModel> ObjHeader = new List<Models.HeaderModel>();
            //string CS = ConfigurationManager.ConnectionStrings["infomaxEntities"].ConnectionString;
            obj.getData("select * from HeaderImages");
            SqlDataReader rdr = obj.cmd.ExecuteReader();

            while (rdr.Read())
            {
                Models.HeaderModel header = new Models.HeaderModel();
                header.ID = Convert.ToInt32(rdr["HID"]);
                header.Header = rdr["HeaderName"].ToString();
                //slider.FileSize = Convert.ToInt32(rdr["FileSize"]);
                header.Image = rdr["FilePath"].ToString();

                ObjHeader.Add(header);
            }
            //ViewBag.VBMobileList = new SelectList(PopulatePages(), "Page_ID", "Page");
            // Viewbag

            return View(ObjHeader);
        }
        [HttpPost]
        public ActionResult HeaderImage(HttpPostedFileBase Image, HeaderModel hm, FormCollection frm)
        {
            PagesModel pag = new PagesModel();
            //var selectedItem = pag.PagesList.Find(p => p.Value == pag.Page_ID.ToString());
            var Mpage = frm["MenuPage"].ToString();

            if (Image != null)
            {
                string fileName = Path.GetFileName(Image.FileName);
                int fileSize = Image.ContentLength;
                int Size = fileSize / 1000;
                Image.SaveAs(Server.MapPath("~/Headers/" + fileName));

                conn.cmd.Parameters.AddWithValue("@HeaderName", Mpage);
                //conn.cmd.Parameters.AddWithValue("FileSize", Size);
                conn.cmd.Parameters.AddWithValue("@FilePath", "~/Headers/" + fileName);
                SqlParameter p = new SqlParameter("@val", SqlDbType.Int);
                p.Direction = ParameterDirection.ReturnValue;
                conn.cmd.Parameters.Add(p);
                conn.execSp("sp_HeaderImages");
                if (p.Value.ToString() == "0")
                {
                    ViewBag.Message = "Header Image Successfully Added";
                }
                else
                {
                    ViewBag.Message = "Failed..";
                }


            }
            return RedirectToAction("HeaderImage");
        }
        [HttpPost]
        public ActionResult Pages(PagesModel objpage)
        {
            Connection con = new Connection();
            con.exeUpdate("insert into Pages values('" + objpage.PageName + "')");
            ViewBag.var1 = GetPageOptions();
            // ViewBag.VBMobileList = new SelectList(PopulatePages(), "Page_ID", "Page");
            return View("HeaderImage");
        }
        public ActionResult Utilities()
        {
            GetName();
            Count();
            ViewBag.var1 = GetPageOptions();
            //conn.getData("select * from PageMetaDetail where ID=1")
            return View();
        }
        [HttpPost]
        public ActionResult Utilities(PageMetaDetail objMeta, string MenuPage, string Check)
        {
            ViewBag.var1 = GetPageOptions();
            if (Check == "on")
            {
                //MetaPageModel obj = new MetaPageModel();
               
                objMeta.PageUrl = "Infomax Software Solutions";
                con.PageMetaDetails.Add(objMeta);
                con.SaveChanges();
                ModelState.Clear();
            }
            else
            {
                //MetaPageModel obj = new MetaPageModel();
                //ViewBag.var1 = GetPageOptions();
                objMeta.PageUrl = "Home/" + MenuPage; 
                con.PageMetaDetails.Add(objMeta);
                con.SaveChanges();
                ModelState.Clear();
            }
            

            return View();
        }

        public ActionResult Services()
        {
            GetName();
            Count();
            ViewBag.Serv = GetServiceList();

            List<Models.ServicesModel> ObjService = new List<Models.ServicesModel>();
            //string CS = ConfigurationManager.ConnectionStrings["infomaxEntities"].ConnectionString;
            conn.getData("select * from Service");
            SqlDataReader rdr = conn.cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    Models.ServicesModel obj = new Models.ServicesModel();
                    obj.ID = Convert.ToInt32(rdr["S_ID"]);
                    obj.STitle = rdr["S_Title"].ToString();
                    //slider.FileSize = Convert.ToInt32(rdr["FileSize"]);
                    obj.Image = rdr["Image"].ToString();

                    ObjService.Add(obj);
                }
            }
            else
            {
                ViewBag.Message = "No Records Found !!!";
            }


            return View(ObjService);
        }
        [HttpPost]
        public ActionResult Services(ServicesModel sm, HttpPostedFileBase Image)
        {
            if (Image != null)
            {
                string fileName = Path.GetFileName(Image.FileName);
                int fileSize = Image.ContentLength;
                int Size = fileSize / 1000;
                Image.SaveAs(Server.MapPath("~/Service/" + fileName));

                conn.cmd.Parameters.AddWithValue("@S_Title", sm.STitle);
                conn.cmd.Parameters.AddWithValue("@Ser_Desc", sm.Desc);
                conn.cmd.Parameters.AddWithValue("@Image", "~/Service/" + fileName);
                SqlParameter p = new SqlParameter("@val", SqlDbType.Int);
                p.Direction = ParameterDirection.ReturnValue;
                conn.cmd.Parameters.Add(p);
                conn.execSp("sp_Services");
                if (p.Value.ToString() == "0")
                {
                    ViewBag.Message = "Services Added Successfully ";
                }
                else
                {
                    ViewBag.Message = "Failed..";
                }
                AddSubMenu(sm.STitle);


                ModelState.Clear();
            }
            return View();
        }

        public ActionResult ManageServices(int id)
        {
            GetName();
            Count();
            ManageServicesModel model = new ManageServicesModel();
            DataTable dt = model.ServiceById(id);
            return View(dt);
        }

        [HttpPost]
        public ActionResult EditService(ManageServicesModel msm, HttpPostedFileBase Image)
        {
            if (Image != null)
            {
                string fileName = Path.GetFileName(Image.FileName);
                int fileSize = Image.ContentLength;
                int Size = fileSize / 1000;
                Image.SaveAs(Server.MapPath("~/Service/" + fileName));

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    SqlCommand cmd = new SqlCommand("update service set S_title=@S_Title,Ser_desc=@Ser_Desc,Image=@Image where S_ID=" + msm.ID, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    cmd.Parameters.AddWithValue("@S_Title", msm.Title);
                    cmd.Parameters.AddWithValue("@Ser_Desc", msm.Desc);
                    cmd.Parameters.AddWithValue("@Image", "~/Service/" + fileName);
                    cmd.ExecuteNonQuery();
                }

            }
            DataTable dt = msm.ServiceById(msm.ID);
            return View("ManageServices", dt);

        }

        public ActionResult DeleteService(int id)
        {
            SqlConnection con = new SqlConnection(strConString);
            SqlCommand cmd = new SqlCommand("delete from service where S_ID=" + id, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return View("Services");
        }








        public void AddSubMenu(string Submenu)
        {
            try
            {

                Connection connect = new Connection();
                connect.cmd.Parameters.AddWithValue("@Sub_Menu", Submenu);
                connect.cmd.Parameters.AddWithValue("@Menu_id", "3");
                // conn.cmd.Parameters.AddWithValue("@Ser_Desc", sm.Service_Desc);
                SqlParameter p = new SqlParameter("@val", SqlDbType.Int);
                p.Direction = ParameterDirection.ReturnValue;
                connect.cmd.Parameters.Add(p);
                connect.execSp("sp_SubMenu");
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }


        private SelectList GetServiceList()           //fetch the tournament type details from the table
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("SELECT * FROM Service", conn);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {

                    ServicesList.Add(new SelectListItem { Text = myReader["S_Title"].ToString(), Value = myReader["S_Title"].ToString() });
                }

                conn.Close();
                return new SelectList(ServicesList, "Value", "Text", "id");  //return the list objects

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            try
            {
                // Setting.    
                //var ctx = Request.GetOwinContext();
                //var authenticationManager = ctx.Authentication;
                //// Sign Out.    
                //authenticationManager.SignOut();
            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
            // Info.    
            return this.RedirectToAction("Login", "Account");
        }
    }


}