using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NTP_MVC.Models;

namespace NTP_MVC.Controllers
{
    public class BenhNhanController : Controller
    {
        // GET: BenhNhan
        public ActionResult Index()
        {
            ViewData["Tinhs"] = db.DM_Tinh.ToList();
            ViewData["Huyens"] = null;
            ViewData["BenhNhan"] = null;
            ViewData["MaBNQL"] = null;
            ViewData["StartDate"] = null;
            ViewData["EndDate"] = null;
            return View();
        }

        NTP_DBEntities db = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult BenhNhanGridViewPartial()
        {
            string s = Request.Params["MaTinh"] + "";
            var model = (from d in db.SO_BenhNhan
                         where d.MA_TINH.Equals(s)
                         select d).ToList();
            return PartialView("_BenhNhanGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult BenhNhanGridViewPartialAddNew(SO_BenhNhan item)
        {
            var model = db.SO_BenhNhan;
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
            return PartialView("_BenhNhanGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult BenhNhanGridViewPartialUpdate(SO_BenhNhan item)
        {
            var model = db.SO_BenhNhan;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.IDBenhNhan == item.IDBenhNhan);
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
            return PartialView("_BenhNhanGridViewPartial", model.Take(1000).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult BenhNhanGridViewPartialDelete(Int64 IDBenhNhan)
        {
            var model = db.SO_BenhNhan;
            if (IDBenhNhan >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.IDBenhNhan == IDBenhNhan);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_BenhNhanGridViewPartial", model.ToList());
        }

        public ActionResult ComboBoxTinhPartial()
        {
            ViewData["Tinhs"] = db.DM_Tinh.ToList();
            return PartialView("_ComboBoxTinhPartial");
        }

        public ActionResult ComboBoxHuyenPartial()
        {
            string maTinh = Request.Params["MaTinh"] + "";
            ViewData["Huyens"] = db.DM_Huyen.Where(m => m.MA_TINH == maTinh).ToList();
            return PartialView("_ComboBoxHuyenPartial");
        }

        public IEnumerable<SO_BenhNhan> SearchBenhNhan()
        {
            string s = Request.Params["MaTinh"] + "";
            var total = (from d in db.SO_BenhNhan
                         where d.MA_TINH.Equals(s)
                         select d).ToList();
            return total;
        }
        public ActionResult SearchBenhNhanPartial()
        {
            string s = Request.Params["SearchCombobox"] + "";
            
            ViewData["BenhNhan"] = (from d in db.SO_BenhNhan
                                    join e in db.DM_Huyen on d.MA_HUYEN equals e.MA_HUYEN into Huyen
                                    from h in Huyen
                                    join f in db.DM_Tinh on h.MA_TINH equals f.MA_TINH into Tinh
                                    from tinh in Tinh.DefaultIfEmpty()
                                    from huyen in Huyen.DefaultIfEmpty()
                                    where d.SoCMND.Contains(s) || d.MaBNQL.Contains(s) || d.HoTen.Contains(s)
                                    orderby tinh.TEN_TINH 
                                    select new
                                    {
                                        SoCMND = d.SoCMND,
                                        MaBNQL = d.MaBNQL,
                                        HoTen = d.HoTen,
                                        GioiTinh = (d.Gioitinh == true && d.Gioitinh != null ? "Nam" : "Nữ"),
                                        Tuoi = d.Tuoi,
                                        TenTinh = tinh.TEN_TINH,
                                        TenHuyen = huyen.TEN_HUYEN,
                                        IDBenhNhan = d.IDBenhNhan,
                                        ID_Doituong = d.ID_Doituong,
                                        MA_TINH = d.MA_TINH,
                                        MA_HUYEN = d.MA_HUYEN,
                                        Diachi = d.Diachi,
                                        Sodienthoai = d.Sodienthoai,
                                        NGAY_NM = d.NGAY_NM,
                                        NGUOI_NM = d.NGUOI_NM,
                                        Ngay_SD = d.Ngay_SD,
                                        Nguoi_SD = d.Nguoi_SD,
                                        Huy = d.Huy,
                                        HuyenXN = d.HuyenXN,
                                        TinhXN = d.TinhXN,
                                        STT = d.STT,
                                        Namsinh = d.Namsinh,
                                        HIV = d.HIV,
                                        BHYT = d.BHYT,
                                        NgheNghiep = d.NgheNghiep,
                                        NoiGioiThieu = d.NoiGioiThieu,
                                        MaNoiGioiThieu = d.MaNoiGioiThieu
                                    }).Distinct().ToList();

            return PartialView("~/Views/BenhNhan/_SearchBenhNhanPartial.cshtml");
        }

        //public ActionResult MaBNQLPartial()
        //{
        //    string s = Request.Params["MaBNQLComboBox"] + "";
        //    if (s == "")
        //    {
        //        ViewData["MaBNQL"] = null;
        //    }
        //    else
        //    {
        //        ViewData["MaBNQL"] = (from d in db.SO_BenhNhan
        //                              join e in db.DM_Huyen on d.MA_HUYEN equals e.MA_HUYEN
        //                              join f in db.DM_Tinh on e.MA_TINH equals f.MA_TINH
        //                              where d.MaBNQL.Contains(s)
        //                              select new
        //                              {
        //                                  MaBNQL = d.MaBNQL,
        //                                  HoTen = d.HoTen,
        //                                  GioiTinh = (d.Gioitinh == true && d.Gioitinh != null ? "Nam" : "Nữ"),
        //                                  Tuoi = d.Tuoi,
        //                                  TenTinh = f.TEN_TINH,
        //                                  TenHuyen = e.TEN_HUYEN
        //                              }).Distinct().ToList();
        //    }
        //    return PartialView("~/Views/BenhNhan/_MaBNQLPartial.cshtml");
        //}
    }
}