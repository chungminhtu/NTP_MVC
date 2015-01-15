using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace NTP_MVC.Controllers
{
    public class BenhVienController : Controller
    {
        // GET: BenhVien
        public ActionResult Index()
        {
            return View();
        }

        NTP_MVC.Models.NTP_DBEntities db = new NTP_MVC.Models.NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult BenhVienGridViewPartial()
        {
            var model = db.DM_BenhVien;
            return PartialView("_BenhVienGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult BenhVienGridViewPartialAddNew(NTP_MVC.Models.DM_BenhVien item)
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
        public ActionResult BenhVienGridViewPartialUpdate(NTP_MVC.Models.DM_BenhVien item)
        {
            var model = db.DM_BenhVien;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_BENHVIEN_KHO == item.ID_BENHVIEN_KHO);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
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
        public ActionResult BenhVienGridViewPartialDelete(System.Int64 ID_BENHVIEN_KHO)
        {
            var model = db.DM_BenhVien;
            if (ID_BENHVIEN_KHO >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_BENHVIEN_KHO == ID_BENHVIEN_KHO);
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