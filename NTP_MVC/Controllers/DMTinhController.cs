using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace NTP_MVC.Controllers
{
    public class DMTinhController : Controller
    {
        // GET: DMTinh
        public ActionResult Index()
        {
            return View();
        }

        NTP_MVC.Models.NTP_DBEntities db = new NTP_MVC.Models.NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult DMTinhGridViewPartial()
        {
            var model = db.DM_Tinh;
            return PartialView("_DMTinhGridViewPartial", model.ToList());
        }
    }
}