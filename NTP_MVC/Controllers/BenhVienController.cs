using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using NTP_MVC.Models;

namespace NTP_MVC.Controllers
{
    public class BenhVienController : Controller
    {
        // GET: BenhVien
        public ActionResult Index()
        {
            return View();
        }

        NTP_DBEntities db = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult BenhVienGridViewPartial()
        {
            var model = db.DM_BenhVien;
            return PartialView("_BenhVienGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult BenhVienGridViewPartialAddNew(DM_BenhVien item)
        {
            var model = db.DM_BenhVien;
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
            return PartialView("_BenhVienGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult BenhVienGridViewPartialUpdate(DM_BenhVien item)
        {
            var model = db.DM_BenhVien;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_BENHVIEN == item.ID_BENHVIEN);
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
            return PartialView("_BenhVienGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult BenhVienGridViewPartialDelete(string ID_BENHVIEN)
        {
            var model = db.DM_BenhVien;
            if (ID_BENHVIEN !="")
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_BENHVIEN == ID_BENHVIEN);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_BenhVienGridViewPartial", model.ToList());
        }
    }
}