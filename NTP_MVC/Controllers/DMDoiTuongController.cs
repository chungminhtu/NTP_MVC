using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using NTP_MVC.Models;

namespace NTP_MVC.Controllers
{
    public class DMDoiTuongController : Controller
    {
        // GET: DMDoiTuong
        public ActionResult Index()
        {
            return View();
        }

        NTP_DBEntities db = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult DMDoiTuongGridViewPartial()
        {
            var model = db.DM_DoiTuong;
            return PartialView("_DMDoiTuongGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DMDoiTuongGridViewPartialAddNew(DM_DoiTuong item)
        {
            var model = db.DM_DoiTuong;
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
            return PartialView("_DMDoiTuongGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DMDoiTuongGridViewPartialUpdate(DM_DoiTuong item)
        {
            var model = db.DM_DoiTuong;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_Doituong == item.ID_Doituong);
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
            return PartialView("_DMDoiTuongGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DMDoiTuongGridViewPartialDelete(Int32 ID_Doituong)
        {
            var model = db.DM_DoiTuong;
            if (ID_Doituong >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_Doituong == ID_Doituong);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_DMDoiTuongGridViewPartial", model.ToList());
        }
    }
}