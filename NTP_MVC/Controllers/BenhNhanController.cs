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
            var MaTinh = Session["MATINH"] + "";
            var MaHuyen = Session["MAHUYEN"] + "";

            ViewData["Tinhs"] = MaTinh == null ? db.DM_Tinh.ToList() : db.DM_Tinh.Where(t => t.MA_TINH.Equals(MaTinh)).ToList();
            ViewData["Huyens"] = MaHuyen == null ? null : db.DM_Huyen.Where(t => t.MA_HUYEN.Equals(MaHuyen)).ToList();

            var model = db.SO_BenhNhan.Where(b => b.MA_HUYEN == MaHuyen).ToList();
            ViewData["MaBNQL"] = null;
            ViewData["StartDate"] = null;
            ViewData["EndDate"] = null;
            return View(model);
        }

        NTP_DBEntities db = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult GridBenhNhan()
        {
            string s = Request.Params["MaHuyen"] + "";
            var model = (from d in db.SO_BenhNhan
                         where d.MA_HUYEN.Equals(s)
                         select d).ToList();
            return PartialView("_GridBenhNhan", model.ToList());
        }

        //Cập nhật hoặc thêm mới bệnh nhân (bằng external form)
        [HttpPost, ValidateInput(false)]
        public ActionResult CapNhatBenhNhan(SO_BenhNhan item)
        {
            item.MA_HUYEN = Session["MAHUYEN"] + "";
            item.MA_TINH = Session["MATINH"] + "";
            if (Session["ID_BenhNhan"] + "" != "")
            {

                item.ID_BenhNhan = Convert.ToInt64(Session["ID_BenhNhan"]);
            }
            var model = db.SO_BenhNhan;
            if (ModelState.IsValid)
            {
                try
                {
                    if (item.ID_BenhNhan != 0)
                    {
                        this.UpdateModel(item);
                        db.SaveChanges();
                    }
                    else
                    {
                        model.Add(item);
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
            string s = Session["MAHUYEN"] + "";
            var ListBenhNhan = (from d in db.SO_BenhNhan
                                where d.MA_HUYEN.Equals(s)
                                select d).ToList();
            return PartialView("_GridBenhNhan", ListBenhNhan);
        }


        //[HttpPost, ValidateInput(false)]
        //public ActionResult GridBenhNhanAddNew(SO_BenhNhan item)
        //{
        //    var model = db.SO_BenhNhan;
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            model.Add(item);
        //            db.SaveChanges();
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    else
        //        ViewData["EditError"] = "Please, correct all errors.";
        //    return PartialView("_GridBenhNhan", model.ToList());
        //}

        //[HttpPost, ValidateInput(false)]
        //public ActionResult GridBenhNhanUpdate(SO_BenhNhan item)
        //{
        //    var model = db.SO_BenhNhan;
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var modelItem = model.FirstOrDefault(it => it.ID_BenhNhan == item.ID_BenhNhan);
        //            if (modelItem != null)
        //            {
        //                this.UpdateModel(modelItem);
        //                db.SaveChanges();
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    else
        //        ViewData["EditError"] = "Please, correct all errors.";
        //    return PartialView("_GridBenhNhan");
        //}
        //[HttpPost, ValidateInput(false)]
        //public ActionResult GridBenhNhanDelete(Int64 IDBenhNhan)
        //{
        //    var model = db.SO_BenhNhan;
        //    if (IDBenhNhan >= 0)
        //    {
        //        try
        //        {
        //            var item = model.FirstOrDefault(it => it.ID_BenhNhan == IDBenhNhan);
        //            if (item != null)
        //                model.Remove(item);
        //            db.SaveChanges();
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    return PartialView("_GridBenhNhan", model.ToList());
        //}

        public ActionResult ComboBoxTinhList()
        {
            ViewData["Tinhs"] = db.DM_Tinh.ToList();
            return PartialView("_ComboBoxTinh", ViewData["Tinhs"]);
        }

        public ActionResult ComboBoxHuyenList()
        {
            string maTinh = HttpContext.Session["MATINH"] + "";
            ViewData["Huyens"] = db.DM_Huyen.Where(m => m.MA_TINH == maTinh).ToList();
            return PartialView("_ComboBoxHuyen", ViewData["Huyens"]);
        }

        public IEnumerable<SO_BenhNhan> SearchBenhNhan()
        {
            string s = Request.Params["MaTinh"] + "";
            var total = (from d in db.SO_BenhNhan
                         where d.MA_TINH.Equals(s)
                         select d).ToList();
            return total;
        }
        public ActionResult ComboSearchBenhNhan()
        {
            string s = Request.Params["SearchCombobox"] + "";
            string MaHuyen = HttpContext.Session["MAHUYEN"] + "";// Request.Params["MaHuyen"] + "";

            ViewData["BenhNhan"] = (from d in db.SO_BenhNhan
                                    where (d.MA_HUYEN.Equals(MaHuyen)) && (d.SoCMND.Contains(s) || d.MaBNQL.Contains(s) || d.HoTen.Contains(s))
                                    orderby d.MA_TINH
                                    select d).ToList();
            //{
            //    SoCMND = d.SoCMND,
            //    MaBNQL = d.MaBNQL,
            //    HoTen = d.HoTen,
            //    GioiTinh = (d.Gioitinh == true && d.Gioitinh != null ? "Nam" : "Nữ"),
            //    Tuoi = d.Tuoi,
            //    ID_BenhNhan = d.ID_BenhNhan,
            //    ID_Doituong = d.ID_Doituong,
            //    MA_TINH = d.MA_TINH,
            //    MA_HUYEN = d.MA_HUYEN,
            //    Diachi = d.Diachi,
            //    Sodienthoai = d.Sodienthoai,
            //    NGAY_NM = d.NGAY_NM,
            //    NGUOI_NM = d.NGUOI_NM,
            //    Ngay_SD = d.Ngay_SD,
            //    Nguoi_SD = d.Nguoi_SD,
            //    Huy = d.Huy,
            //    HuyenXN = d.HuyenXN,
            //    TinhXN = d.TinhXN,
            //    STT = d.STT,
            //    Namsinh = d.Namsinh,
            //    HIV = d.HIV,
            //    BHYT = d.BHYT,
            //    NgheNghiep = d.NgheNghiep,
            //    NoiGioiThieu = d.NoiGioiThieu,
            //    MaNoiGioiThieu = d.MaNoiGioiThieu
            //}).Distinct().ToList();

            return PartialView("~/Views/BenhNhan/_ComboSearchBenhNhan.cshtml");
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