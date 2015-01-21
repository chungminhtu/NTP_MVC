using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTP_MVC.Models;

namespace NTP_MVC.Controllers
{
    public class HomeController : Controller
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
            if (User != null)
            {
                ControllerContext.HttpContext.Session["MATINH"] = User.MA_TINH + "";
                ControllerContext.HttpContext.Session["MAHUYEN"] = User.MA_HUYEN + "";

                return RedirectToAction("Index", "BenhNhan"); 
            }
            else
            {
                ViewData["EditError"] = "Tên đăng nhập hoặc mật khẩu không đúng! Hãy thử lại.";
                return RedirectToAction("Index", "Home");
            }
        }

    }
}