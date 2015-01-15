using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using NTP_MVC.Models;

namespace NTP_MVC.Controllers
{
    public class SoKhamBenhController : Controller
    {
        // GET: SoKhamBenh
        public ActionResult Index()
        { 
            return View();
        }

        NTP_DBEntities db = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult SoKhamBenhGridViewPartial()
        {
            var model =   db.SO_SoKhamBenh;
            return PartialView("_SoKhamBenhGridViewPartial",  model.Take(3000).ToList());
        } 

        [HttpPost, ValidateInput(false)]
        public ActionResult SoKhamBenhGridViewPartialAddNew(SO_SoKhamBenh item)
        {
            var model = db.SO_SoKhamBenh;
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
            return PartialView("_SoKhamBenhGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SoKhamBenhGridViewPartialUpdate(SO_SoKhamBenh item)
        {
            var model = db.SO_SoKhamBenh;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_SoKhamBenh == item.ID_SoKhamBenh);
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
            return PartialView("_SoKhamBenhGridViewPartial", model.Take(1000).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult SoKhamBenhGridViewPartialDelete(Int64 ID_SoKhamBenh)
        {
            var model = db.SO_SoKhamBenh;
            if (ID_SoKhamBenh >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_SoKhamBenh == ID_SoKhamBenh);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_SoKhamBenhGridViewPartial", model.ToList());
        }
     
    }
}