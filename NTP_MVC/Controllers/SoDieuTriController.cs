using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace NTP_MVC.Controllers
{
    public class SoDieuTriController : Controller
    {
        // GET: SoDieuTri
        public ActionResult Index()
        {
            return View();
        }

        NTP_MVC.Models.NTP_DBEntities db = new NTP_MVC.Models.NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult SoDieuTriDataViewPartial()
        {
            var model = db.SO_SoDieuTri;
            return PartialView("_SoDieuTriDataViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SoDieuTriDataViewPartialAddNew(NTP_MVC.Models.SO_SoDieuTri item)
        {
            var model = db.SO_SoDieuTri;
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
            return PartialView("_SoDieuTriDataViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SoDieuTriDataViewPartialUpdate(NTP_MVC.Models.SO_SoDieuTri item)
        {
            var model = db.SO_SoDieuTri;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_Dieutri == item.ID_Dieutri);
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
            return PartialView("_SoDieuTriDataViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SoDieuTriDataViewPartialDelete(System.Int64 ID_Dieutri)
        {
            var model = db.SO_SoDieuTri;
            if (ID_Dieutri >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_Dieutri == ID_Dieutri);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_SoDieuTriDataViewPartial", model.ToList());
        }
    }
}