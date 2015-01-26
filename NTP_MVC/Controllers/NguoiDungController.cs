using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using NTP_MVC.Models;

namespace NTP_MVC.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }

        NTP_DBEntities db = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult NguoiDungGridViewPartial()
        {
            var model = db.AD_Users;
            return PartialView("_NguoiDungGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult NguoiDungGridViewPartialAddNew(AD_Users item)
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
            return PartialView("_NguoiDungGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult NguoiDungGridViewPartialUpdate(AD_Users item)
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
            return PartialView("_NguoiDungGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult NguoiDungGridViewPartialDelete(Int32 UserID)
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
            return PartialView("_NguoiDungGridViewPartial", model.ToList());
        }
    }
}