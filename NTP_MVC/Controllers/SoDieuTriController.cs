using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using NTP_MVC.Models;
using System.Data.Entity;

namespace NTP_MVC.Controllers
{
    public class SoDieuTriController : Controller
    {
        // GET: SoDieuTri
        public ActionResult Index()
        {
            var MaTinh = Session["MATINH"] + "";
            if (MaTinh != "")
            {
                var MaHuyen = Session["MAHUYEN"] + "";
                ViewData["Tinhs"] = db.DM_Tinh.Where(t => t.MA_TINH.Equals(MaTinh)).ToList();
                ViewData["Huyens"] = db.DM_Huyen.Where(t => t.MA_HUYEN.Equals(MaHuyen)).ToList();

                SoDieuTriModel model = new SoDieuTriModel();
                model.ListBN = db.SO_BenhNhan.Where(b => b.MA_HUYEN == MaHuyen).ToList();
                model.ListSDT = new List<SO_SoDieuTri>();

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        NTP_DBEntities db = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult GridSoDieuTriList()
        {
            GetSoDieuTriCuaBenhNhan();
            return PartialView("_GridSoDieuTri", ViewData["ListSoDieuTri"]);
        }

        public ActionResult Edit()
        {
            long ID_BenhNhan = 0;
            long ID_SoDieuTri = 0;
            if (Request.Params["ID_BenhNhan"] != "")
            {
                ID_BenhNhan = Convert.ToInt64(Request.Params["ID_BenhNhan"]);
            }

            if (Request.Params["ID_SoDieuTri"] != "")
            {
                ID_SoDieuTri = Convert.ToInt64(Request.Params["ID_SoDieuTri"]);
            }

            SoDieuTriModel model = new SoDieuTriModel();

            model.BN = db.SO_BenhNhan.Where(b => b.ID_BenhNhan == ID_BenhNhan).SingleOrDefault();
            model.BN.ID_BenhNhan = ID_BenhNhan;
            model.SDT = db.SO_SoDieuTri.Where(b => b.ID_SoDieuTri == ID_SoDieuTri).SingleOrDefault();

            if (model.BN == null)
            {
                model.BN = new SO_BenhNhan();
            }
            if (model.SDT == null)
            {
                model.SDT = new SO_SoDieuTri();
            }

            return View(model);
        }



        [HttpPost, ValidateInput(false)]
        public string Edit(SO_BenhNhan bn, SO_SoDieuTri sdt)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bn.MA_HUYEN = Session["MAHUYEN"] + "";
                    bn.MA_TINH = Session["MATINH"] + "";
                    bn.ID_BenhNhan = Request.Params["ID_BenhNhan"] + "" != "" ? Convert.ToInt64(Request.Params["ID_BenhNhan"].Split(',')[0]) : 0;
                    sdt.ID_SoDieuTri = Request.Params["ID_SoDieuTri"] + "" != "" ? Convert.ToInt64(Request.Params["ID_SoDieuTri"]) : 0;
                    if (bn.ID_BenhNhan != 0)
                    {
                        db.SO_BenhNhan.Attach(bn);
                        db.Entry(bn).State = EntityState.Modified;
                        db.SaveChanges();
                        sdt.ID_BENHNHAN = bn.ID_BenhNhan;
                        if (sdt.ID_SoDieuTri != 0)
                        {
                            db.SO_SoDieuTri.Attach(sdt);
                            db.Entry(sdt).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            db.SO_SoDieuTri.Add(sdt);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        db.SO_BenhNhan.Add(bn);
                        db.SaveChanges();

                        sdt.ID_BENHNHAN = bn.ID_BenhNhan;
                        db.SO_SoDieuTri.Add(sdt);
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

        public void GetSoDieuTriCuaBenhNhan()
        {
            var s = Session["ID_BenhNhan"] + "";
            if (s != "")
            {
                ViewData["ListSoDieuTri"] = db.SO_SoDieuTri.Where(bn => bn.ID_BENHNHAN.ToString().Contains(s)).ToList();
            }
            ViewData["ListHuyen"] = db.DM_Huyen.ToList(); 
        }


        [ValidateInput(false)]
        public ActionResult GridSoDieuTri()
        {
            var model = db.SO_SoDieuTri;
            return PartialView("_GridSoDieuTri", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridSoDieuTriAddNew(SO_SoDieuTri item)
        {
            var model = db.SO_SoDieuTri;
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
            GetSoDieuTriCuaBenhNhan();
            return PartialView("_GridSoDieuTri", ViewData["ListSoDieuTri"]);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridSoDieuTriUpdate(SO_SoDieuTri item)
        {
            var model = db.SO_SoDieuTri;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_SoDieuTri == item.ID_SoDieuTri);
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
            GetSoDieuTriCuaBenhNhan();
            return PartialView("_GridSoDieuTri", ViewData["ListSoDieuTri"]);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridSoDieuTriDelete(Int64 ID_SoDieuTri)
        {
            var model = db.SO_SoDieuTri;
            if (ID_SoDieuTri >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_SoDieuTri == ID_SoDieuTri);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            GetSoDieuTriCuaBenhNhan();
            return PartialView("_GridSoDieuTri", ViewData["ListSoDieuTri"]);
        }
    }
}