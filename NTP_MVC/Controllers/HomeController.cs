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
        public ActionResult Index()
        {
            var MaTinh = Session["MATINH"] + "";
            if (MaTinh != "")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}