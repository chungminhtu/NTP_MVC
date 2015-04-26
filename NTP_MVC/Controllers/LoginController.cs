using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTP_MVC.Models;

namespace NTP_MVC.Controllers
{
    public class LoginController : Controller
    {
        NTP_DBEntities db = new NTP_DBEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(AD_Users user)
        {
            var User = (AD_Users)db.AD_Users.Where(u => u.Username.Equals(user.Username)).Where(u => u.Password.Equals(user.Password)).SingleOrDefault();
            string Name = user.Username;
            string Pass = user.Password;
            DM_BenhVien s = (DM_BenhVien)(from u in db.AD_Users
                                          join kho in db.AD_User_Kho on u.UserID equals kho.USERID
                                          join bv in db.DM_BenhVien on kho.MA_BENHVIEN equals bv.ID_BENHVIEN
                                          where u.Username.Equals(Name) && u.Password.Equals(Pass)
                                          select bv).SingleOrDefault();
            if (User != null)
            {
                if (s.CAPQUANLY==2)
                {
                    HttpContext.Session["MATINH"] = s.ID_MATINH + ""; 
                }
                else
                {
                    HttpContext.Session["MATINH"] = s.ID_MATINH + "";
                    HttpContext.Session["MAHUYEN"] = s.ID_HUYEN + ""; 
                }
                HttpContext.Session["TenBV"] = s.TEN_BENHVIEN + "";

                return RedirectToAction("Index", "SoKhamBenh");
            }
            else
            {
                ViewData["EditError"] = "Tên đăng nhập hoặc mật khẩu không đúng! Hãy thử lại.";
                return View("Index");
            }
        }

    }
}