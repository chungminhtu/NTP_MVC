using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using NTP_MVC.Models;

namespace NTP_MVC.Controllers
{
    public class PhieuXetNghiemController : Controller
    {
        // GET: PhieuXetNghiem
        public ActionResult Index()
        {
            return null;
        }

        NTP_DBEntities db = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult GridPhieuXetNghiem()
        {
            GetPhieuXetNghiem_BenhNhan();
            return PartialView("_GridPhieuXetNghiem");
        }

        [ValidateInput(false)]
        public ActionResult PhieuXetNghiemGridViewPartial()
        {
            var model = db.SO_PhieuXetNghiem;
            return PartialView("_PhieuXetNghiemGridViewPartial", model.ToList());
        }

        public void GetPhieuXetNghiem_BenhNhan()
        {
            var id = HttpContext.Session["ID_BenhNhan"] + "";
            ViewData["ListPXNBenhNhan"] = db.SO_PhieuXetNghiem.Where(p => p.ID_Benhnhan.ToString().Equals(id)).ToList();
        }

        #region CRUD
        [HttpPost, ValidateInput(false)]
        public ActionResult GridPhieuXetNghiemAddNew(SO_PhieuXetNghiem item)
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
            GetPhieuXetNghiem_BenhNhan();
            return PartialView("_GridPhieuXetNghiem", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridPhieuXetNghiemUpdate(SO_PhieuXetNghiem item)
        {
            var model = db.SO_PhieuXetNghiem;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_Phieuxetnghiem == item.ID_Phieuxetnghiem);
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
            GetPhieuXetNghiem_BenhNhan();
            return PartialView("_GridPhieuXetNghiem", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridPhieuXetNghiemDelete(Int64 ID_Phieuxetnghiem)
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
            GetPhieuXetNghiem_BenhNhan();
            return PartialView("_GridPhieuXetNghiem", model.ToList());
        }
        #endregion

        NTP_DBEntities db1 = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult GridKetQuaXetNghiem()
        {
            var model = db1.SO_PhieuXetNghiem_KQ;
            return PartialView("_GridKetQuaXetNghiem", model.Take(3).ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridKetQuaXetNghiemAddNew(SO_PhieuXetNghiem_KQ item)
        {
            var model = db1.SO_PhieuXetNghiem_KQ;
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
            return PartialView("_GridKetQuaXetNghiem", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridKetQuaXetNghiemUpdate(SO_PhieuXetNghiem_KQ item)
        {
            var model = db1.SO_PhieuXetNghiem_KQ;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_Phieuxetnghiem == item.ID_Phieuxetnghiem);
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
            return PartialView("_GridKetQuaXetNghiem", model.Take(3).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridKetQuaXetNghiemDelete(Int32 ID_Phieuxetnghiem)
        {
            var model = db1.SO_PhieuXetNghiem_KQ;
            if (ID_Phieuxetnghiem >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_Phieuxetnghiem == ID_Phieuxetnghiem);
                    if (item != null)
                        model.Remove(item);
                    db1.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_GridKetQuaXetNghiem", model.ToList());
        }
    }
}