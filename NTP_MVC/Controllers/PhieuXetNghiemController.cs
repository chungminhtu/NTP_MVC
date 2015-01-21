using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace NTP_MVC.Controllers
{
    public class PhieuXetNghiemController : Controller
    {
        // GET: PhieuXetNghiem
        public ActionResult Index()
        {
            return null;
        }

        NTP_MVC.Models.NTP_DBEntities db = new NTP_MVC.Models.NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult GridPhieuXetNghiem()
        {
            var id = HttpContext.Session["ID_BenhNhan"] + "";
            ViewData["ListPXNBenhNhan"] = db.SO_PhieuXetNghiem.Where(p => p.ID_Benhnhan.ToString().Equals(id)).ToList();
            return PartialView("_GridPhieuXetNghiem", ViewData["ListPXNBenhNhan"]);
        }

        [ValidateInput(false)]
        public ActionResult PhieuXetNghiemGridViewPartial()
        {
            var model = db.SO_PhieuXetNghiem;
            return PartialView("_PhieuXetNghiemGridViewPartial", model.ToList());
        }

        #region CRUD
        [HttpPost, ValidateInput(false)]
        public ActionResult GridPhieuXetNghiemAddNew(NTP_MVC.Models.SO_PhieuXetNghiem item)
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
            return PartialView("_GridPhieuXetNghiem", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridPhieuXetNghiemUpdate(NTP_MVC.Models.SO_PhieuXetNghiem item)
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
            return PartialView("_GridPhieuXetNghiem", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridPhieuXetNghiemDelete(System.Int64 ID_Phieuxetnghiem)
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
            return PartialView("_GridPhieuXetNghiem", model.ToList());
        }
        #endregion
    }
}