using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace NTP_MVC.Controllers
{
    public class DMDonViTinhController : Controller
    {
        // GET: DMDonViTinh
        public ActionResult Index()
        {
            return View();
        }

        NTP_MVC.Models.NTP_DBEntities db = new NTP_MVC.Models.NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult DMDonViTinhGridViewPartial()
        {
            var model = db.DM_DonViTinh;
            return PartialView("_DMDonViTinhGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DMDonViTinhGridViewPartialAddNew(NTP_MVC.Models.DM_DonViTinh item)
        {
            var model = db.DM_DonViTinh;
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
            return PartialView("_DMDonViTinhGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DMDonViTinhGridViewPartialUpdate(NTP_MVC.Models.DM_DonViTinh item)
        {
            var model = db.DM_DonViTinh;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_DVT == item.ID_DVT);
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
            return PartialView("_DMDonViTinhGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DMDonViTinhGridViewPartialDelete(System.Int32 ID_DVT)
        {
            var model = db.DM_DonViTinh;
            if (ID_DVT >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_DVT == ID_DVT);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_DMDonViTinhGridViewPartial", model.ToList());
        }
    }
}