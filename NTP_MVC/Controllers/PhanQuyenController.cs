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

        [ValidateInput(false)]
        public ActionResult MenuTreeListPartial()
        {
            var model = db.AD_RoleMenu;
            return PartialView("_MenuTreeListPartial", model.ToList());
        }

        NTP_DBEntities db1 = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult MenuTreeListPartial1()
        {
            var model = db1.AD_Menu;
            return PartialView("_MenuTreeListPartial1", model.ToList());
        }

        [ValidateInput(false)]
        public ActionResult DSUserGridViewPartial()
        {
            var model = db.AD_Users.ToList();
            return PartialView("_DSUserGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DSUserGridViewPartialAddNew(AD_Users item)
        {
            var model = db.AD_Users;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_DSUserGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DSUserGridViewPartialUpdate(AD_Users item)
        {
            var model = db.AD_Users;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.UserID == item.UserID);
                    if (modelItem != null)
                    {
                        UpdateModel(modelItem);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_DSUserGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DSUserGridViewPartialDelete(System.Int32 UserID)
        {
            var model = db.AD_Users;
            if (UserID >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.UserID == UserID);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_DSUserGridViewPartial", model);
        }
    }
}