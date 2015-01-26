using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using NTP_MVC.Models;

namespace NTP_MVC.Controllers
{
    public class DMLoaiBenhVienController : Controller
    {
        // GET: DMLoaiBenhVien
        public ActionResult Index()
        {
            return View();
        }

        NTP_DBEntities db = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult DMTinhGridViewPartial()
        {
            var model = db.DM_Tinh;
            return PartialView("_DMTinhGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DMTinhGridViewPartialAddNew(DM_Tinh item)
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
        public ActionResult DMTinhGridViewPartialUpdate(DM_Tinh item)
        {
            var model = db.DM_Tinh;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.MA_TINH == item.MA_TINH);
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
            return PartialView("_DMTinhGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DMTinhGridViewPartialDelete(String MA_TINH)
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

        NTP_DBEntities db1 = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult DMLoaiBenhVienGridViewPartial()
        {
            var model = db1.DM_LoaiBenhVien;
            return PartialView("_DMLoaiBenhVienGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DMLoaiBenhVienGridViewPartialAddNew(DM_LoaiBenhVien item)
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
        public ActionResult DMLoaiBenhVienGridViewPartialUpdate(DM_LoaiBenhVien item)
        {
            var model = db1.DM_LoaiBenhVien;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_LOAIBV == item.ID_LOAIBV);
                    if (modelItem != null)
                    {
                        UpdateModel(modelItem);
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
        public ActionResult DMLoaiBenhVienGridViewPartialDelete(String ID_LOAIBV)
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