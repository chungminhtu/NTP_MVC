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
                    var modelItem = model.FirstOrDefault(it => it.ID_PhieuXetNghiem == item.ID_PhieuXetNghiem);
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
        public ActionResult GridPhieuXetNghiemDelete(Int64 ID_PhieuXetNghiem)
        {
            var model = db.SO_PhieuXetNghiem;
            if (ID_PhieuXetNghiem >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_PhieuXetNghiem == ID_PhieuXetNghiem);
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


        [ValidateInput(false)]
        public ActionResult GridKetQuaXetNghiem()
        {
            GetKetQuaXetNghiem_PXN();
            return PartialView("_GridKetQuaXetNghiem");
        }

        private void GetKetQuaXetNghiem_PXN()
        {
            var ID_PhieuXetNghiem = Request.Params["ID_PhieuXetNghiem"] != "" ? Convert.ToInt32(Request.Params["ID_PhieuXetNghiem"]) : 0;
            HttpContext.Session["ID_PhieuXetNghiem"] = ID_PhieuXetNghiem;
            HttpContext.Session["SoXN"] = Request.Params["SoXN"] + "";
            HttpContext.Session["NgayNhanMau"] = Request.Params["NgayNhanMau"];
            ViewData["ListKQXN"] = db.SO_PhieuXetNghiem_KQ.Where(m => m.ID_PhieuXetNghiem.Equals(ID_PhieuXetNghiem)).ToList();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridKetQuaXetNghiemAddNew(SO_PhieuXetNghiem_KQ item)
        {
            var model = db.SO_PhieuXetNghiem_KQ;
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
            GetKetQuaXetNghiem_PXN();
            return PartialView("_GridKetQuaXetNghiem");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridKetQuaXetNghiemUpdate(SO_PhieuXetNghiem_KQ item)
        {
            var model = db.SO_PhieuXetNghiem_KQ;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_PhieuXetNghiem == item.ID_PhieuXetNghiem);
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
            GetKetQuaXetNghiem_PXN();
            return PartialView("_GridKetQuaXetNghiem");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridKetQuaXetNghiemDelete(Int32 ID_PhieuXetNghiem)
        {
            var model = db.SO_PhieuXetNghiem_KQ;
            if (ID_PhieuXetNghiem >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_PhieuXetNghiem == ID_PhieuXetNghiem);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            GetKetQuaXetNghiem_PXN();
            return PartialView("_GridKetQuaXetNghiem");
        }
    }
}