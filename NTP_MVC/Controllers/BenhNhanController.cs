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
            Session["MATINH"] = "07";
            Session["MAHUYEN"] = "0703";
            var MaTinh = Session["MATINH"] + "";
            if (MaTinh != "")
            {
                var MaHuyen = Session["MAHUYEN"] + "";
                //ViewData["Tinhs"] = MaTinh == null ? db.DM_Tinh.ToList() : db.DM_Tinh.Where(t => t.MA_TINH.Equals(MaTinh)).ToList();
                //ViewData["Huyens"] = MaHuyen == null ? null : db.DM_Huyen.Where(t => t.MA_HUYEN.Equals(MaHuyen)).ToList();

                ViewData["Tinhs"] = db.DM_Tinh.ToList();
                ViewData["Huyens"] = db.DM_Huyen.Where(t => t.MA_HUYEN.Equals(MaHuyen)).ToList();

                //ViewData["STinh"] = db.DM_Tinh.ToList();
                //ViewData["SHuyen"] = db.DM_Huyen.Where(t => t.MA_HUYEN.Equals(MaHuyen)).ToList();
                //ViewData["SXa"] = db.DM_Xa.ToList();

                SoKhamBenhModel model = new SoKhamBenhModel();


                model.ListBN = db.SO_BenhNhan.Where(b => b.MA_HUYEN == MaHuyen).ToList();
                model.ListSKB = new List<SO_SoKhamBenh>();

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

        public ActionResult GridBenhNhanChuyenDen()
        {
            string matinh = Request.Params["MaTinh"] + "";
            string mahuyen = Request.Params["MaHuyen"] + "";

            List<SO_BenhNhan> ListBN = (from d in db.SO_BenhNhan
                                        join e in db.SO_SoDieuTri on d.ID_BenhNhan equals e.ID_BENHNHAN
                                        where d.MA_HUYEN.Equals(mahuyen)
                                        && d.MA_TINH.Equals(matinh)
                                        select d).ToList();
            return PartialView("_GridBenhNhanChuyenDen", ListBN);
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

            //DateTime nxnfrom = Request.Params["NXNFrom"] + "" != "" ? DateTime.Parse(Request.Params["NXNFrom"] + "") : DateTime.MinValue;
            //DateTime nxnto = Request.Params["NXNTo"] + "" != "" ? DateTime.Parse(Request.Params["NXNTo"] + "") : DateTime.MinValue;

            List<SO_BenhNhan> ListBN = (from d in db.SO_BenhNhan
                                        where d.MA_HUYEN.Equals(mahuyen)
                                        && d.MA_TINH.Equals(matinh)
                                        && (hoten != "" ? d.HoTen.Contains(hoten) : true)
                                        && (mabnql != "" ? d.MaBNQL.Contains(mabnql) : true)
                                        && (socmnd != "" ? d.SoCMND.Contains(socmnd) : true)
                                        select d).ToList();
            List<long> ListIDBN = ListBN.Select(b => b.ID_BenhNhan).ToList();
            List<long> ListIDBNFromSKB = new List<long>();
            List<long> ListIDBNFromSDT = new List<long>();
            List<SO_BenhNhan> ListBNSearched = ListBN;
            if ((nkbfrom != DateTime.MinValue || nkbto != DateTime.MinValue) && (ndtfrom != DateTime.MinValue || ndtto != DateTime.MinValue))
            {
                ListIDBNFromSKB = (from a in db.SO_SoKhamBenh
                                   where ListIDBN.Contains(a.ID_BENHNHAN)
                                    && (nkbfrom != DateTime.MinValue ? a.NgayKhamBenh >= nkbfrom : true)
                                    && (nkbto != DateTime.MinValue ? a.NgayKhamBenh <= nkbto : true)
                                   select a.ID_BENHNHAN).ToList();
                ListIDBNFromSDT = (from a in db.SO_SoDieuTri
                                   where ListIDBN.Contains(a.ID_BENHNHAN)
                                    && (ndtfrom != DateTime.MinValue ? a.NgayDT >= ndtfrom : true)
                                    && (ndtto != DateTime.MinValue ? a.NgayDT <= ndtto : true)
                                   select a.ID_BENHNHAN).ToList();
                ListBNSearched = (from a in ListBN
                                  where ListIDBNFromSKB.Contains(a.ID_BenhNhan)
                                     && ListIDBNFromSDT.Contains(a.ID_BenhNhan)
                                  select a).ToList();
            }
            else if (nkbfrom != DateTime.MinValue || nkbto != DateTime.MinValue)
            {
                ListIDBNFromSKB = (from a in db.SO_SoKhamBenh
                                   where ListIDBN.Contains(a.ID_BENHNHAN)
                                    && (nkbfrom != DateTime.MinValue ? a.NgayKhamBenh >= nkbfrom : true)
                                    && (nkbto != DateTime.MinValue ? a.NgayKhamBenh <= nkbto : true)
                                   select a.ID_BENHNHAN).ToList();
                ListBNSearched = (from a in ListBN
                                  where ListIDBNFromSKB.Contains(a.ID_BenhNhan)
                                  select a).ToList();
            }
            else if (ndtfrom != DateTime.MinValue || ndtto != DateTime.MinValue)
            {

                ListIDBNFromSDT = (from a in db.SO_SoDieuTri
                                   where ListIDBN.Contains(a.ID_BENHNHAN)
                                    && (ndtfrom != DateTime.MinValue ? a.NgayDT >= ndtfrom : true)
                                    && (ndtto != DateTime.MinValue ? a.NgayDT <= ndtto : true)
                                   select a.ID_BENHNHAN).ToList();
                ListBNSearched = (from a in ListBN
                                  where ListIDBNFromSDT.Contains(a.ID_BenhNhan)
                                  select a).ToList();
            }
            return PartialView("_GridBenhNhan", ListBNSearched);
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
            string maTinh = Request.Params["MaTinh"] + "";
            ViewData["Huyens"] = db.DM_Huyen.Where(m => m.MA_TINH == maTinh).ToList();
            return PartialView("_ComboBoxHuyen", ViewData["Huyens"]);
        }

        public ActionResult ComboBoxXaList()
        {
            ViewData["Xas"] = db.DM_Xa.ToList();
            return PartialView("_ComboBoxXa", ViewData["Xas"]);
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