using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INFOMAX.Models;
using INFOMAX.DB_Context;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;

namespace INFOMAX.Controllers
{
    public class HomeController : Controller
    {
        infomaxEntities objCon = new infomaxEntities();
        INFOMAX.DB_Context.db dblayer = new INFOMAX.DB_Context.db();
        // GET: Home
        public ActionResult Index()
        {
            ShowMenu();
            return View();
        }
        public void ShowMenu()
        {
            DataSet ds = dblayer.get_MainMenu();
            ViewBag.category = ds.Tables[0];
        }
        public void get_Submenu(int menuid)
        {
            DataSet ds = dblayer.get_SubMenu(menuid);
            List<SubMenuModel> submenulist = new List<SubMenuModel>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                submenulist.Add(new SubMenuModel
                {
                    SubMenu_id = Convert.ToInt32(dr["SubMenu_id"]),
                    Sub_Menu = dr["Sub_Menu"].ToString()
                });
            }
            Session["submenu"] = submenulist;
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel LgnUsr)
        {
            Session["UserName"] = LgnUsr.EmailId;
            var _passWord = LgnUsr.Password;
            bool Isvalid = objCon.logins.Any(x => x.Email == LgnUsr.EmailId &&
            x.Password == _passWord);
            if (Isvalid)
            {
                int timeout = LgnUsr.Rememberme ? 60 : 5; // Timeout in minutes, 60 = 1 hour.    
                var ticket = new FormsAuthenticationTicket(LgnUsr.EmailId, false, timeout);
                string encrypted = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                cookie.Expires = System.DateTime.Now.AddMinutes(timeout);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);
                return RedirectToAction("AdminHome", "AdminDash");
            }
            else
            {
                ModelState.AddModelError("", "Invalid Information... Please try again!");
            }
            return View();
        }

        public ActionResult Req_Demo()
        {
            ShowMenu();
            return View();
        }
        [HttpPost]
        public ActionResult Req_Demo(Req_DemoModel req)
        {
            Connection conn = new Connection();
            conn.cmd.Parameters.AddWithValue("@Name", req.Name);
            conn.cmd.Parameters.AddWithValue("@Email", req.Email);
            conn.cmd.Parameters.AddWithValue("@Contact", req.Contact);
            conn.cmd.Parameters.AddWithValue("@Subject", req.Subject);
            conn.cmd.Parameters.AddWithValue("@Message", req.Message);
            conn.cmd.Parameters.AddWithValue("@Status", "0");
            conn.cmd.Parameters.AddWithValue("@Req_Date", DateTime.Now.ToShortDateString());
            conn.cmd.Parameters.AddWithValue("@image", "~/Clients/" + "images.png");
            SqlParameter p = new SqlParameter("@val", SqlDbType.Int);
            p.Direction = ParameterDirection.ReturnValue;
            conn.cmd.Parameters.Add(p);
            conn.execSp("sp_RequestDemo");
            SendEmailToUser(req.Name,req.Email,req.Contact,req.Subject,req.Message);
            if (p.Value.ToString() == "0")
            {
                ViewBag.Message = "Demo Requested Successfully";
            }
            else
            {
                ViewBag.Message = "Requesting Failed";


            }
            return View();
        }
        public void SendEmailToUser(string Name,string emailId, string Contact,   string Subject,string Message)
        {

            //var GenarateUserVerificationLink = "/Register/UserVerification/" + activationCode;
            //var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, GenarateUserVerificationLink);

            var fromMail = new MailAddress("vanaja992@gmail.com", "Demo Request"); // set your email    
            var fromEmailpassword = "Vanaja@1122"; // Set your password     
            var toEmail = new MailAddress(emailId);

            var smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword);

            MailMessage message = new MailMessage(fromMail, toEmail);
            message.Subject = Subject;
            message.Body = "<br/> Name:" +Name+
                           "<br/> Email: " +emailId+
                           "<br/> Contact: " + Contact +
                           "<br/><br/>Message: "+Message;
            message.IsBodyHtml = true;
            smtp.Send(message);

        }

    }
}