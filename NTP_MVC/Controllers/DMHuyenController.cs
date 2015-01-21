using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace NTP_MVC.Controllers
{
    public class DMHuyenController : Controller
    {
        // GET: DMHuyen
        public ActionResult Index()
        {
            return View();
        }

        NTP_MVC.Models.NTP_DBEntities db = new NTP_MVC.Models.NTP_DBEntities();
        
        public ActionResult ComboBoxHuyenPartial()
        {
            string maTinh = Request.Params["MaTinh"] + "";
            ViewData["Huyens"] = db.DM_Huyen.Where(m => m.MA_TINH == maTinh).ToList();
            return PartialView("_ComboBoxHuyenPartial");
        }

        [ValidateInput(false)]
        public ActionResult DMHuyenGridViewPartial()
        {
            var model = db.DM_Huyen;
            return PartialView("_DMHuyenGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DMHuyenGridViewPartialAddNew(NTP_MVC.Models.DM_Huyen item)
        {
            var model = db.DM_Huyen;
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
            return PartialView("_DMHuyenGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DMHuyenGridViewPartialUpdate(NTP_MVC.Models.DM_Huyen item)
        {
            var model = db.DM_Huyen;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.MA_HUYEN == item.MA_HUYEN);
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
            return PartialView("_DMHuyenGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DMHuyenGridViewPartialDelete(string MA_HUYEN)
        {
            var model = db.DM_Huyen;
            if (MA_HUYEN != "")
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.MA_HUYEN == MA_HUYEN);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_DMHuyenGridViewPartial", model.ToList());
        }
    }
}