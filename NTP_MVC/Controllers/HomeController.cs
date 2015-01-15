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
            //if (db.AD_Users.Where(u=>u.Username.Equals(user.Username)).Where(u=>u.Password.Equals(user.Password)).Any() )
            //{
            //    return View("/SoKhamBenh/Index.cshtml");
            //}
            //return View();
            return RedirectToAction("Index","SoKhamBenh");
        }
    
    }
}