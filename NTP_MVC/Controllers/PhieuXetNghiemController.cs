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
        public ActionResult Index()
        {
            return null;
        }

        NTP_DBEntities db = new NTP_DBEntities();

        ////////////////////////Phieu Xet Nghiem
        [ValidateInput(false)]
        public ActionResult GridPhieuXetNghiem()
        {
            GetPhieuXetNghiem_BenhNhan();
            return PartialView("_GridPhieuXetNghiem");
        }

        public ActionResult PrintPXN()
        {
            ViewData["ReportPhieuXetNghiem"] = new NTP_MVC.Reports.InPhieuXetNghiem();
            return PartialView("_PrintPXN");
        }

        public ActionResult ExportPXN()
        {
            return DevExpress.Web.Mvc.DocumentViewerExtension.ExportTo(new NTP_MVC.Reports.InPhieuXetNghiem());
        }

        public void GetPhieuXetNghiem_BenhNhan()
        {
            if (Session["ID_BenhNhan"] != null)
            {
                var id = Session["ID_BenhNhan"] + "";
                var data = db.SO_PhieuXetNghiem.Where(p => p.ID_Benhnhan.ToString().Equals(id)).ToList();
                ViewData["ListPXNBenhNhan"] = data;
                Session["Max1SoXN"] = Convert.ToInt32(data.Max(m => m.SoXN)) + 1;
            }
        }


        ////////////////////////Ket Qua Xet Nghiem      
        [ValidateInput(false)]
        public ActionResult GridKetQuaXetNghiem()
        {
            GetKetQuaXetNghiem_PXN();
            return PartialView("_GridKetQuaXetNghiem");
        }

        private void GetKetQuaXetNghiem_PXN()
        {
            if (Request.Params["ID_PhieuXetNghiem"] != "")
            {
                var id_PhieuXetNghiem = Convert.ToInt64(Request.Params["ID_PhieuXetNghiem"]);
                Session["ID_PhieuXetNghiem"] = id_PhieuXetNghiem;
                var data = db.SO_PhieuXetNghiem.Where(p => p.ID_PhieuXetNghiem.Equals(id_PhieuXetNghiem)).SingleOrDefault();
                if (data != null)
                {
                    Session["SoXN"] = ((SO_PhieuXetNghiem)data).SoXN;
                    Session["NgayNhanMau"] = ((SO_PhieuXetNghiem)data).NgayXN;
                }
                var dataKQ = db.SO_PhieuXetNghiem_KQ.Where(m => m.ID_PhieuXetNghiem.Equals(id_PhieuXetNghiem)).ToList();
                ViewData["ListKQXN"] = dataKQ;
                if (dataKQ.Count != 0)
                {
                    Session["MaxMauDom"] = Convert.ToInt32(dataKQ.Max(m => m.MauDom)) + 1;
                }
                else
                {
                    Session["MaxMauDom"] = 1;
                }
            }
        }

        #region CRUD PhieuXetNghiem
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
            return PartialView("_GridPhieuXetNghiem");
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
            return PartialView("_GridPhieuXetNghiem");
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
            return PartialView("_GridPhieuXetNghiem");
        }
        #endregion

        #region CRUD Ket Qua Xet Nghiem
        [HttpPost, ValidateInput(false)]
        public ActionResult GridKetQuaXetNghiemAddNew(SO_PhieuXetNghiem_KQ item)
        {
            var model = db.SO_PhieuXetNghiem_KQ;
            var modelPXN = db.SO_PhieuXetNghiem;
            if (ModelState.IsValid)
            {
                try
                {
                    //Addnew KQ XN 
                    model.Add(item);
                    db.SaveChanges();

                    //Update Phieu Xet Nghiem
                    var id_PhieuXetNghiem = Convert.ToInt64(Request.Params["ID_PhieuXetNghiem"]);
                    var dataKQ = db.SO_PhieuXetNghiem_KQ.Where(m => m.ID_PhieuXetNghiem.Equals(id_PhieuXetNghiem)).ToList();
                    if (dataKQ.Count != 0)
                    {
                        var MaxKQXN = Convert.ToInt32(dataKQ.Max(m => m.KetQua));
                        var modelItemPXN = modelPXN.FirstOrDefault(it => it.ID_PhieuXetNghiem == id_PhieuXetNghiem);
                        modelItemPXN.KetQuaPXN = MaxKQXN;
                        modelItemPXN.Soluong = (byte)dataKQ.Count;
                        UpdateModel(modelItemPXN);
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
        public ActionResult GridKetQuaXetNghiemUpdate(SO_PhieuXetNghiem_KQ item)
        {
            var model = db.SO_PhieuXetNghiem_KQ;
            var modelPXN = db.SO_PhieuXetNghiem;
            if (ModelState.IsValid)
            {
                try
                {
                    //Save Ket qua XN
                    var modelItem = model.FirstOrDefault(it => it.ID_PhieuXetNghiem == item.ID_PhieuXetNghiem);
                    if (modelItem != null)
                    {
                        UpdateModel(modelItem);
                        db.SaveChanges();
                    }
                    //Update Phieu Xet Nghiem
                    var id_PhieuXetNghiem = Convert.ToInt64(Request.Params["ID_PhieuXetNghiem"]);
                    var dataKQ = db.SO_PhieuXetNghiem_KQ.Where(m => m.ID_PhieuXetNghiem.Equals(id_PhieuXetNghiem)).ToList();
                    if (dataKQ.Count != 0)
                    {
                        var MaxKQXN = Convert.ToInt32(dataKQ.Max(m => m.KetQua));
                        var modelItemPXN = modelPXN.FirstOrDefault(it => it.ID_PhieuXetNghiem == id_PhieuXetNghiem);
                        modelItemPXN.KetQuaPXN = MaxKQXN;
                        modelItemPXN.Soluong = (byte)dataKQ.Count;
                        UpdateModel(modelItemPXN);
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

        public ActionResult DocumentViewerPartial()
        {
            Reports.InPhieuXetNghiem report = new Reports.InPhieuXetNghiem();
            NTP_DBEntities db = new NTP_DBEntities();
            if (Session["ID_BenhNhan"] != null)
            {
                var ID_BenhNhan = Session["ID_BenhNhan"] + "";
                report.DataSource = (from a in db.InPhieuXetNghiems
                                     where a.ID_BenhNhan.ToString() == ID_BenhNhan
                                     select a).ToList();
            }
            return PartialView("_DocumentViewerPartial", report);
        }

        public ActionResult DocumentViewerExport()
        {
            Reports.InPhieuXetNghiem report = new Reports.InPhieuXetNghiem();
            NTP_DBEntities db = new NTP_DBEntities();
            if (Session["ID_BenhNhan"] != null)
            {
                var ID_BenhNhan = Session["ID_BenhNhan"] + "";
                report.DataSource = (from a in db.InPhieuXetNghiems
                                     where a.ID_BenhNhan.ToString() == ID_BenhNhan
                                     select a).ToList();
            }
            return DocumentViewerExtension.ExportTo(report);
        }


        #endregion
    }
}