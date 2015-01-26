using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using NTP_MVC.Models;

namespace NTP_MVC.Controllers
{
    public class NXController : Controller
    {
        // GET: NX
        public ActionResult Index()
        {
            return View();
        }

        NTP_DBEntities db = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult NXGridViewPartial()
        {
            var model = db.DM_LyDoNhapXuat;
            return PartialView("_NXGridViewPartial", model.ToList());
        }
    }
}