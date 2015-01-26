using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using NTP_MVC.Models;

namespace NTP_MVC.Controllers
{
    public class DMNguonKinhPhiController : Controller
    {
        // GET: DMNguonKinhPhi
        public ActionResult Index()
        {
            return View();
        }

        NTP_DBEntities db = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult DMNguonKinhPhiGridViewPartial()
        {
            var model = db.DM_NguonKinhPhi;
            return PartialView("_DMNguonKinhPhiGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DMNguonKinhPhiGridViewPartialAddNew(DM_NguonKinhPhi item)
        {
            var model = db.DM_NguonKinhPhi;
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
            return PartialView("_DMNguonKinhPhiGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DMNguonKinhPhiGridViewPartialUpdate(DM_NguonKinhPhi item)
        {
            var model = db.DM_NguonKinhPhi;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_NGUONKINHPHI == item.ID_NGUONKINHPHI);
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
            return PartialView("_DMNguonKinhPhiGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DMNguonKinhPhiGridViewPartialDelete(Int32 ID_NGUONKINHPHI)
        {
            var model = db.DM_NguonKinhPhi;
            if (ID_NGUONKINHPHI >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_NGUONKINHPHI == ID_NGUONKINHPHI);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_DMNguonKinhPhiGridViewPartial", model.ToList());
        }
    }
}