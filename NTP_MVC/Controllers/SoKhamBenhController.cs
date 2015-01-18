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
            ViewData["ListPXNBenhNhan"] = null;
            var model = db.SO_SoKhamBenh.Take(0).ToList();
            return PartialView("_SoKhamBenhGridViewPartial", model);
        }

        public ActionResult GetDetailBenhNhan()
        {
            var s = Request.Params["ID_BenhNhan"] + "";
            var model = (SO_BenhNhan)(from bn in db.SO_BenhNhan
                                      where bn.SoCMND.Contains(s) || bn.MaBNQL.Contains(s) || bn.HoTen.Contains(s) || bn.ID_BenhNhan.ToString().Contains(s)
                                      select bn).FirstOrDefault();
            //ViewData["IDBenhNhan"] = model.ID_BenhNhan;
            return PartialView("ThongTinBenhNhan", model);
        }
         
        public ActionResult GetListSoKhamBenh()
        {
            long IDBenhNhan = Convert.ToInt64(Request.Params["ID_BenhNhan"].ToString().Split(',')[0]);
            var model = db.SO_SoKhamBenh.Where(bn => bn.ID_BENHNHAN == IDBenhNhan).ToList();
            ViewData["IDBenhNhan"] = IDBenhNhan;
            return PartialView("_SoKhamBenhGridViewPartial", model);
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

        [ValidateInput(false)]
        public ActionResult PhieuXetNghiemGridPartial()
        {
            ViewData["ListPXNBenhNhan"] = db.SO_PhieuXetNghiem.Take(8).ToList();
            return PartialView("_PhieuXetNghiemGridPartial", ViewData["ListPXNBenhNhan"]);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PhieuXetNghiemGridPartialAddNew(NTP_MVC.Models.SO_PhieuXetNghiem item)
        {
            var model = db.SO_PhieuXetNghiem;
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
            return PartialView("_PhieuXetNghiemGridPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult PhieuXetNghiemGridPartialUpdate(NTP_MVC.Models.SO_PhieuXetNghiem item)
        {
            var model = db.SO_PhieuXetNghiem;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_Phieuxetnghiem == item.ID_Phieuxetnghiem);
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
            return PartialView("_PhieuXetNghiemGridPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult PhieuXetNghiemGridPartialDelete(System.Int64 ID_Phieuxetnghiem)
        {
            var model = db.SO_PhieuXetNghiem;
            if (ID_Phieuxetnghiem >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_Phieuxetnghiem == ID_Phieuxetnghiem);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_PhieuXetNghiemGridPartial", model.ToList());
        }
    }
}