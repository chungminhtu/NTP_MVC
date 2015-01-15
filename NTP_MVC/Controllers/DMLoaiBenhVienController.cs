using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace NTP_MVC.Controllers
{
    public class DMLoaiBenhVienController : Controller
    {
        // GET: DMLoaiBenhVien
        public ActionResult Index()
        {
            return View();
        }

        NTP_MVC.Models.NTP_DBEntities db = new NTP_MVC.Models.NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult DMTinhGridViewPartial()
        {
            var model = db.DM_Tinh;
            return PartialView("_DMTinhGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DMTinhGridViewPartialAddNew(NTP_MVC.Models.DM_Tinh item)
        {
            var model = db.DM_Tinh;
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
            return PartialView("_DMTinhGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DMTinhGridViewPartialUpdate(NTP_MVC.Models.DM_Tinh item)
        {
            var model = db.DM_Tinh;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.MA_TINH == item.MA_TINH);
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
            return PartialView("_DMTinhGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DMTinhGridViewPartialDelete(System.String MA_TINH)
        {
            var model = db.DM_Tinh;
            if (MA_TINH != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.MA_TINH == MA_TINH);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_DMTinhGridViewPartial", model.ToList());
        }

        NTP_MVC.Models.NTP_DBEntities db1 = new NTP_MVC.Models.NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult DMLoaiBenhVienGridViewPartial()
        {
            var model = db1.DM_LoaiBenhVien;
            return PartialView("_DMLoaiBenhVienGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DMLoaiBenhVienGridViewPartialAddNew(NTP_MVC.Models.DM_LoaiBenhVien item)
        {
            var model = db1.DM_LoaiBenhVien;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    db1.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_DMLoaiBenhVienGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DMLoaiBenhVienGridViewPartialUpdate(NTP_MVC.Models.DM_LoaiBenhVien item)
        {
            var model = db1.DM_LoaiBenhVien;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_LOAIBV == item.ID_LOAIBV);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db1.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_DMLoaiBenhVienGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DMLoaiBenhVienGridViewPartialDelete(System.String ID_LOAIBV)
        {
            var model = db1.DM_LoaiBenhVien;
            if (ID_LOAIBV != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_LOAIBV == ID_LOAIBV);
                    if (item != null)
                        model.Remove(item);
                    db1.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_DMLoaiBenhVienGridViewPartial", model.ToList());
        }
    }
}