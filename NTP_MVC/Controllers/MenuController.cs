using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using NTP_MVC.Models;

namespace NTP_MVC.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }

        NTP_DBEntities db = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult MenuTreeListPartial()
        {
            var model = db.AD_Menu;
            return PartialView("_MenuTreeListPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult MenuTreeListPartialAddNew(AD_Menu item)
        {
            var model = db.AD_Menu;
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
            return PartialView("_MenuTreeListPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult MenuTreeListPartialUpdate(AD_Menu item)
        {
            var model = db.AD_Menu;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.MenuID == item.MenuID);
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
            return PartialView("_MenuTreeListPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult MenuTreeListPartialDelete(Int32 MenuID)
        {
            var model = db.AD_Menu;
            if (MenuID != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.MenuID == MenuID);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_MenuTreeListPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult MenuTreeListPartialMove(Int32 MenuID, Int32? MenuParentID)
        {
            var model = db.AD_Menu;
            try
            {
                var item = model.FirstOrDefault(it => it.MenuID == MenuID);
                if (item != null)
                    item.MenuParentID = MenuParentID;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            return PartialView("_MenuTreeListPartial", model.ToList());
        }
    }
}