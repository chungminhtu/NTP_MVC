using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using NTP_MVC.Models;
using System.Data.Entity;
using DevExpress.XtraReports.UI;
using System.Data.Linq;

namespace NTP_MVC.Controllers
{
    public class SoKhamBenhController : Controller
    {
        NTP_DBEntities db = new NTP_DBEntities();
        public List<long?> ListIDBN { get; set; }

        public ActionResult Index()
        {

            Session["MATINH"] = "07";
            Session["MAHUYEN"] = "0703";
            var MaTinh = Session["MATINH"] + "";
            if (MaTinh != "")
            {
                var MaHuyen = Session["MAHUYEN"] + "";
                ViewData["Tinhs"] = db.DM_Tinh.Where(t => t.MA_TINH.Equals(MaTinh)).ToList();
                ViewData["Huyens"] = db.DM_Huyen.Where(t => t.MA_HUYEN.Equals(MaHuyen)).ToList();

                SoKhamBenhModel model = new SoKhamBenhModel();
                model.ListBN = db.SO_BenhNhan.Where(b => b.MA_HUYEN == MaHuyen).ToList();
                Session["ListIDBN"] = model.ListBN.Select(a => a.ID_BenhNhan).ToList();
                model.ListSKB = new List<SO_SoKhamBenh>();

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult GridPhieuXetNghiem()
        {
            GetPhieuXetNghiem_BenhNhan();
            return PartialView("_GridPhieuXetNghiem");
        }

        public void GetPhieuXetNghiem_BenhNhan()
        {
            if (HttpContext.Session["ID_BenhNhan"] != "")
            {
                var id = HttpContext.Session["ID_BenhNhan"] + "";
                var data = db.SO_PhieuXetNghiem.Where(p => p.ID_Benhnhan.ToString().Equals(id)).ToList();
                ViewData["ListPXNBenhNhan"] = data;
                HttpContext.Session["Max1SoXN"] = Convert.ToInt32(data.Max(m => m.SoXN)) + 1;
            }
        }


        public ActionResult DeleteBN()
        {
            long ID_BenhNhan = 0;

            if (Request.Params["ID_BenhNhan"] != "")
            {
                ID_BenhNhan = Convert.ToInt64(Request.Params["ID_BenhNhan"]);
            }
            var bnn = db.SO_BenhNhan.Where(b => b.ID_BenhNhan == ID_BenhNhan).SingleOrDefault();
            db.SO_BenhNhan.Remove(bnn);
            db.SaveChanges();
            return RedirectToAction("Index", "SoKhamBenh");
        }

        public ActionResult DeleteSKB()
        {
            long ID_SoKhamBenh = 0;

            if (Request.Params["ID_SoKhamBenh"] != "")
            {
                ID_SoKhamBenh = Convert.ToInt64(Request.Params["ID_SoKhamBenh"]);
            }
            var bnn = db.SO_SoKhamBenh.Where(b => b.ID_SoKhamBenh == ID_SoKhamBenh).SingleOrDefault();
            db.SO_SoKhamBenh.Remove(bnn);
            db.SaveChanges();
            return RedirectToAction("Index", "SoKhamBenh");
        }


        public ActionResult Edit()
        {
            long ID_BenhNhan = 0;
            long ID_SoKhamBenh = 0;
            if (Request.Params["ID_BenhNhan"] != "")
            {
                ID_BenhNhan = Convert.ToInt64(Request.Params["ID_BenhNhan"]);
            }

            if (Request.Params["ID_SoKhamBenh"] != "")
            {
                ID_SoKhamBenh = Convert.ToInt64(Request.Params["ID_SoKhamBenh"]);
            }

            SoKhamBenhModel model = new SoKhamBenhModel();

            model.BN = db.SO_BenhNhan.Where(b => b.ID_BenhNhan == ID_BenhNhan).SingleOrDefault();
            model.BN.ID_BenhNhan = ID_BenhNhan;
            model.SKB = db.SO_SoKhamBenh.Where(b => b.ID_SoKhamBenh == ID_SoKhamBenh).SingleOrDefault();

            if (model.BN == null)
            {
                model.BN = new SO_BenhNhan();
            }
            if (model.SKB == null)
            {
                model.SKB = new SO_SoKhamBenh();
            }

            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public string Edit(SO_BenhNhan bn, SO_SoKhamBenh skb)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bn.MA_HUYEN = Session["MAHUYEN"] + "";
                    bn.MA_TINH = Session["MATINH"] + "";
                    bn.ID_BenhNhan = Request.Params["ID_BenhNhan"] + "" != "" ? Convert.ToInt64(Request.Params["ID_BenhNhan"]) : 0;
                    skb.ID_SoKhamBenh = Request.Params["ID_SoKhamBenh"] + "" != "" ? Convert.ToInt64(Request.Params["ID_SoKhamBenh"]) : 0;
                    if (bn.ID_BenhNhan != 0)
                    {
                        db.SO_BenhNhan.Attach(bn);
                        db.Entry(bn).State = EntityState.Modified;
                        db.SaveChanges();
                        skb.ID_BENHNHAN = bn.ID_BenhNhan;
                        if (skb.ID_SoKhamBenh != 0)
                        {
                            db.SO_SoKhamBenh.Attach(skb);
                            db.Entry(skb).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            db.SO_SoKhamBenh.Add(skb);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        db.SO_BenhNhan.Add(bn);
                        db.SaveChanges();

                        skb.ID_BENHNHAN = bn.ID_BenhNhan;
                        db.SO_SoKhamBenh.Add(skb);
                        db.SaveChanges();
                    }
                    return "Cập nhật thành công";
                }
                catch (Exception e)
                {
                    return "Cập nhật thất bại, lỗi:" + e.Message;
                }
            }
            else
                return "Có lỗi khi nhập dữ liệu, hãy kiểm tra và thử lại";
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SearchBenhNhan(string matinh, string mahuyen, string hoten, string mabnql, string socmnd, DateTime? nkbfrom, DateTime? nkbto)
        {
            List<SO_BenhNhan> ListBN = (from d in db.SO_BenhNhan
                                        where (mahuyen != "" ? d.MA_HUYEN.Equals(mahuyen) : true
                                     && d.MA_TINH.Equals(matinh))
                                     && ((hoten != "" ? d.HoTen.Contains(hoten) : true)
                                     && (mabnql != "" ? d.MaBNQL.Contains(mabnql) : true)
                                     && (socmnd != "" ? d.SoCMND.Contains(socmnd) : true))
                                        select d).ToList();

            if (nkbfrom != null || nkbto != null)
            {

                List<long> ListID_BN = ListBN.Select(a => a.ID_BenhNhan).ToList();
                List<long> ListID_SKB = (from d in db.SO_SoKhamBenh
                                         where
                                        ListID_BN.Contains(d.ID_BENHNHAN)
                                      && (nkbfrom != null ? d.NgayKhamBenh >= nkbfrom : true)
                                      && (nkbto != null ? d.NgayKhamBenh <= nkbto : true)
                                         select d.ID_BENHNHAN).ToList();
                ListBN = ListBN.Where(a => ListID_SKB.Contains(a.ID_BenhNhan)).ToList();
            }
            return PartialView("_GridBenhNhan", ListBN);
        }

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
                HttpContext.Session["ID_PhieuXetNghiem"] = id_PhieuXetNghiem;
                var data = db.SO_PhieuXetNghiem.Where(p => p.ID_PhieuXetNghiem.Equals(id_PhieuXetNghiem)).SingleOrDefault();
                if (data != null)
                {
                    HttpContext.Session["SoXN"] = ((SO_PhieuXetNghiem)data).SoXN;
                    HttpContext.Session["NgayNhanMau"] = ((SO_PhieuXetNghiem)data).NgayXN;
                }
                var dataKQ = db.SO_PhieuXetNghiem_KQ.Where(m => m.ID_PhieuXetNghiem.Equals(id_PhieuXetNghiem)).ToList();
                ViewData["ListKQXN"] = dataKQ;
                if (dataKQ.Count != 0)
                {
                    HttpContext.Session["MaxMauDom"] = Convert.ToInt32(dataKQ.Max(m => m.MauDom)) + 1;
                }
                else
                {
                    HttpContext.Session["MaxMauDom"] = 1;
                }
            }
        }

        [ValidateInput(false)]
        public ActionResult GridSoKhamBenhList()
        {
            GetSoKhamBenhCuaBenhNhan();
            return PartialView("_GridSoKhamBenh", ViewData["ListSoKhamBenh"]);
        }


        public ActionResult viewDoc()
        {
            return View();
        }

      
        public ActionResult DocumentViewerPartial()
        {
            XtraReport report = new XtraReport();
            NTP_DBEntities db = new NTP_DBEntities();
            List<long> listidbn = Session["ListIDBN"] as List<long>;

            report.DataSource = (from a in db.InPhieuXetNghiems
                                 where listidbn.Contains(a.ID_BenhNhan.Value)
                                 select a).ToList();
            return PartialView("_DocumentViewerPartial", report);
        }

        public ActionResult DocumentViewerExport()
        {
            XtraReport report = new XtraReport();
            NTP_DBEntities db = new NTP_DBEntities();
            List<long> listidbn = Session["ListIDBN"] as List<long>;

            report.DataSource = (from a in db.InPhieuXetNghiems
                                 where listidbn.Contains(a.ID_BenhNhan.Value)
                                 select a).ToList();
            return DocumentViewerExtension.ExportTo(report);
        }

        public void GetSoKhamBenhCuaBenhNhan()
        {
            var s = Request.Params["ID_BenhNhan"] + "";
            if (s != "")
            {
                ViewData["ListSoKhamBenh"] = db.SO_SoKhamBenh.Where(bn => bn.ID_BENHNHAN.ToString().Contains(s)).ToList();
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridSoKhamBenhAddNew(SO_SoKhamBenh item)
        {
            var model = db.SO_SoKhamBenh;
            var modelPXN = db.SO_PhieuXetNghiem;
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
            GetSoKhamBenhCuaBenhNhan();
            return PartialView("_GridSoKhamBenh", ViewData["ListSoKhamBenh"]);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridSoKhamBenhUpdate(SO_SoKhamBenh item)
        {
            var model = db.SO_SoKhamBenh;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_SoKhamBenh == item.ID_SoKhamBenh);
                    if (modelItem != null)
                    {
                        //Update Ket qua xet nghiem cho So kham benh
                        var id = HttpContext.Session["ID_BenhNhan"] + "";
                        var data = db.SO_PhieuXetNghiem.Where(p => p.ID_Benhnhan.ToString().Equals(id)).ToList();
                        if (data.Count != 0)
                        {
                            modelItem.KetQuaXetNghiemDom = data.Single(m => m.ID_PhieuXetNghiem.Equals(data.Max(n => n.ID_PhieuXetNghiem))).KetQuaPXN + "";
                        }
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
            GetSoKhamBenhCuaBenhNhan();
            return PartialView("_GridSoKhamBenh", ViewData["ListSoKhamBenh"]);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridSoKhamBenhDelete(Int64 ID_SoKhamBenh)
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
            GetSoKhamBenhCuaBenhNhan();
            return PartialView("_GridSoKhamBenh", ViewData["ListSoKhamBenh"]);
        }
    }
}