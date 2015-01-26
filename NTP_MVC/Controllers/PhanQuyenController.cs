using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using NTP_MVC.Models;

namespace NTP_MVC.Controllers
{
    public class PhanQuyenController : Controller
    {
        // GET: UserRole
        public ActionResult Index()
        {
            return View();
        }

        NTP_DBEntities db = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult RoleGridViewPartial()
        {
            var model = db.AD_Roles;
            return PartialView("_RoleGridViewPartial", model.ToList());
        }

        NTP_DBEntities db1 = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult MenuTreeListPartial()
        {
            var model = db1.AD_RoleMenu;
            return PartialView("_MenuTreeListPartial", model.ToList());
        }
    }
}