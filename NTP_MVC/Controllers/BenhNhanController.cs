using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NTP_MVC.Models;

namespace NTP_MVC.Controllers
{
    public class BenhNhanController : Controller
    {
        NTP_DBEntities db = new NTP_DBEntities();
        // GET: BenhNhan
        public ActionResult Index()
        {
            var MaTinh = Session["MATINH"] + "";

            if (MaTinh != "")
            {
                var MaHuyen = Session["MAHUYEN"] + "";

                ViewData["Tinhs"] = MaTinh == null ? db.DM_Tinh.ToList() : db.DM_Tinh.Where(t => t.MA_TINH.Equals(MaTinh)).ToList();
                ViewData["Huyens"] = MaHuyen == null ? null : db.DM_Huyen.Where(t => t.MA_HUYEN.Equals(MaHuyen)).ToList();

                var model = db.SO_BenhNhan.Where(b => b.MA_HUYEN == MaHuyen).ToList();
                ViewData["MaBNQL"] = null;
                ViewData["StartDate"] = null;
                ViewData["EndDate"] = null;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult GetDetailBenhNhan()
        {
            var s = Request.Params["ID_BenhNhan"] + "";
            var model = (SO_BenhNhan)(from bn in db.SO_BenhNhan
                                      where bn.SoCMND.Contains(s) || bn.MaBNQL.Contains(s) || bn.HoTen.Contains(s) || bn.ID_BenhNhan.ToString().Contains(s)
                                      select bn).FirstOrDefault();
            //ViewData["IDBenhNhan"] = model.ID_BenhNhan;
            return PartialView("_FormBenhNhan", model);
        }

        public ActionResult ComboSearchBenhNhan()
        {
            string s = Request.Params["SearchCombobox"] + "";


            string MaHuyen = HttpContext.Session["MAHUYEN"] + "";
            string MaTinh = HttpContext.Session["MATINH"] + "";

            ViewData["BenhNhan"] =
                                    (from d in db.SO_BenhNhan
                                     join e in db.DM_Huyen on d.MA_HUYEN equals e.MA_HUYEN
                                     join f in db.DM_Tinh on e.MA_TINH equals f.MA_TINH
                                     where (
                                         //d.MA_HUYEN.Equals(MaHuyen)) && (
                                         // d.MA_TINH.Equals(MaTinh)) && (
                                             d.SoCMND.Contains(s) ||
                                             d.MaBNQL.Contains(s) ||
                                             d.HoTen.Contains(s))
                                     orderby d.MA_TINH
                                     select new
                                     {
                                         ID_BenhNhan = d.ID_BenhNhan,
                                         MaBNQL = d.MaBNQL,
                                         SoCMND = d.SoCMND,
                                         HoTen = d.HoTen,
                                         GioiTinh = (d.Gioitinh == true && d.Gioitinh != null ? "Nam" : "Nữ"),
                                         Tuoi = d.Tuoi,
                                         TenTinh = f.TEN_TINH,
                                         TenHuyen = e.TEN_HUYEN
                                     }).Distinct().ToList();
            return PartialView("_ComboSearchBenhNhan");
        }

        [ValidateInput(false)]
        public ActionResult GridBenhNhan()
        {
            string matinh = Request.Params["MaTinh"] + "";
            string mahuyen = Request.Params["MaHuyen"] + "";

            string hoten = Request.Params["HoTen"] + "";
            string mabnql = Request.Params["MaBNQL"] + "";
            string socmnd = Request.Params["SoCMND"] + "";
            DateTime nkbfrom = Request.Params["NKBFrom"] + "" != "" ? DateTime.Parse(Request.Params["NKBFrom"] + "") : DateTime.MinValue;
            DateTime nkbto = Request.Params["NKBTo"] + "" != "" ? DateTime.Parse(Request.Params["NKBTo"] + "") : DateTime.MinValue;

            DateTime ndtfrom = Request.Params["NDTFrom"] + "" != "" ? DateTime.Parse(Request.Params["NDTFrom"] + "") : DateTime.MinValue;
            DateTime ndtto = Request.Params["NDTTo"] + "" != "" ? DateTime.Parse(Request.Params["NDTTo"] + "") : DateTime.MinValue;

            DateTime nxnfrom = Request.Params["NXNFrom"] + "" != "" ? DateTime.Parse(Request.Params["NXNFrom"] + "") : DateTime.MinValue;
            DateTime nxnto = Request.Params["NXNTo"] + "" != "" ? DateTime.Parse(Request.Params["NXNTo"] + "") : DateTime.MinValue;

            var ListBN = from d in db.SO_BenhNhan
                         where d.MA_HUYEN.Equals(mahuyen)
                         && d.MA_TINH.Equals(matinh)
                         && (hoten != "" ? d.HoTen.Contains(hoten) : true)
                         && (mabnql != "" ? d.MaBNQL.Contains(mabnql) : true)
                         && (socmnd != "" ? d.SoCMND.Contains(socmnd) : true)
                         select d;

            var ListSKB = from bn in ListBN
                          join e in db.SO_SoKhamBenh on bn.ID_BenhNhan equals e.ID_BENHNHAN into j
                          from m in j.DefaultIfEmpty()
                          where (nkbfrom != DateTime.MinValue ? m.NgayKhamBenh >= nkbfrom : true)
                          && (nkbto != DateTime.MinValue ? m.NgayKhamBenh <= nkbto : true)
                          select new
                          {
                              ID_BenhNhan = bn.ID_BenhNhan,
                              ID_Doituong = bn.ID_Doituong,
                              HoTen = bn.HoTen,
                              MaBNQL = bn.MaBNQL,
                              SoCMND = bn.SoCMND,
                              Tuoi = bn.Tuoi,
                              Gioitinh = bn.Gioitinh,
                              MA_TINH = bn.MA_TINH,
                              MA_HUYEN = bn.MA_HUYEN,
                              Diachi = bn.Diachi,
                              Sodienthoai = bn.Sodienthoai,
                              NGAY_NM = bn.NGAY_NM,
                              NGUOI_NM = bn.NGUOI_NM,
                              Ngay_SD = bn.Ngay_SD,
                              Nguoi_SD = bn.Nguoi_SD,
                              Huy = bn.Huy,
                              HuyenXN = bn.HuyenXN,
                              TinhXN = bn.TinhXN,
                              STT = bn.STT,
                              Namsinh = bn.Namsinh,
                              HIV = bn.HIV,
                              BHYT = bn.BHYT,
                              NgheNghiep = bn.NgheNghiep,
                              NoiGioiThieu = bn.NoiGioiThieu,
                              MaNoiGioiThieu = bn.MaNoiGioiThieu,
                              NgayKB = m.NgayKhamBenh
                          };
            var ListBNSearch = (from skb in ListSKB
                               join bn in ListBN on skb.ID_BenhNhan equals bn.ID_BenhNhan into j
                               from m in j
                               select m).Distinct();
            return PartialView("_GridBenhNhan", ListBNSearch.ToList());
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
                        var modelItem = model.FirstOrDefault(it => it.ID_BenhNhan == item.ID_BenhNhan);
                        UpdateModel(modelItem);
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
            SO_BenhNhan bn = (SO_BenhNhan)db.SO_BenhNhan.Where(i => i.ID_BenhNhan == item.ID_BenhNhan).SingleOrDefault();
            return PartialView("_FormBenhNhan", bn);
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
        //public ActionResult ComboSearchBenhNhan()
        //{
        //    string s = Request.Params["SearchCombobox"] + "";
        //    string MaHuyen = HttpContext.Session["MAHUYEN"] + "";// Request.Params["MaHuyen"] + "";

        //    ViewData["BenhNhan"] = (from d in db.SO_BenhNhan
        //                            where (d.MA_HUYEN.Equals(MaHuyen)) && (d.SoCMND.Contains(s) || d.MaBNQL.Contains(s) || d.HoTen.Contains(s))
        //                            orderby d.MA_TINH
        //                            select d).ToList();
        //    //{
        //    //    SoCMND = d.SoCMND,
        //    //    MaBNQL = d.MaBNQL,
        //    //    HoTen = d.HoTen,
        //    //    GioiTinh = (d.Gioitinh == true && d.Gioitinh != null ? "Nam" : "Nữ"),
        //    //    Tuoi = d.Tuoi,
        //    //    ID_BenhNhan = d.ID_BenhNhan,
        //    //    ID_Doituong = d.ID_Doituong,
        //    //    MA_TINH = d.MA_TINH,
        //    //    MA_HUYEN = d.MA_HUYEN,
        //    //    Diachi = d.Diachi,
        //    //    Sodienthoai = d.Sodienthoai,
        //    //    NGAY_NM = d.NGAY_NM,
        //    //    NGUOI_NM = d.NGUOI_NM,
        //    //    Ngay_SD = d.Ngay_SD,
        //    //    Nguoi_SD = d.Nguoi_SD,
        //    //    Huy = d.Huy,
        //    //    HuyenXN = d.HuyenXN,
        //    //    TinhXN = d.TinhXN,
        //    //    STT = d.STT,
        //    //    Namsinh = d.Namsinh,
        //    //    HIV = d.HIV,
        //    //    BHYT = d.BHYT,
        //    //    NgheNghiep = d.NgheNghiep,
        //    //    NoiGioiThieu = d.NoiGioiThieu,
        //    //    MaNoiGioiThieu = d.MaNoiGioiThieu
        //    //}).Distinct().ToList();

        //    return PartialView("~/Views/BenhNhan/_ComboSearchBenhNhan.cshtml");
        //}

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