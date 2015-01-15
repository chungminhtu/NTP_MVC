using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace NTP_MVC.Controllers
{
    public class PhieuXetNghiemController : Controller
    {
        // GET: PhieuXetNghiem
        public ActionResult Index()
        {
            return View();
        }


        NTP_MVC.Models.NTP_DBEntities db = new NTP_MVC.Models.NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult PhieuXetNghiemGridViewPartial()
        {
            var model = db.SO_PhieuXetNghiem;
            return PartialView("_PhieuXetNghiemGridViewPartial", model.ToList());
        }
    }
}