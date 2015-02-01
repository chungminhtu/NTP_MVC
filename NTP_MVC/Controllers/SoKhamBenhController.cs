using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using NTP_MVC.Models;

namespace NTP_MVC.Controllers
{
    public class SoKhamBenhController : Controller
    {
        NTP_DBEntities db = new NTP_DBEntities();

        public ActionResult Index()
        {
            Session["ID_BenhNhan"] = Request.Params["ID_BenhNhan"] + "";
            var s = Request.Params["ID_BenhNhan"] + "";
            if (s != "")//Nếu có ID_BenhNhan thì Edit
            {
                ViewData["ListSoKhamBenh"] = db.SO_SoKhamBenh.Where(bn => bn.ID_BENHNHAN.ToString().Contains(s)).ToList();
                var model = (SO_BenhNhan)db.SO_BenhNhan.Where(bn => bn.ID_BenhNhan.ToString().Equals(s)).SingleOrDefault();
                return View(model);
            }
            return View(); //Nếu không edit thì addnew
        }

        [ValidateInput(false)]
        public ActionResult GridSoKhamBenhList()
        {
            GetSoKhamBenhCuaBenhNhan();
            return PartialView("_GridSoKhamBenh", ViewData["ListSoKhamBenh"]);
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

        public void GetSoKhamBenhCuaBenhNhan()
        {
            var s = Session["ID_BenhNhan"] + "";
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


        public ActionResult ComboSearchBenhNhan()
        {
            string s = Request.Params["SearchCombobox"] + "";
            string MaHuyen = HttpContext.Session["MAHUYEN"] + "";// Request.Params["MaHuyen"] + "";

            ViewData["BenhNhan"] = (from d in db.SO_BenhNhan
                                    where (d.MA_HUYEN.Equals(MaHuyen)) && (
                                            d.SoCMND.Contains(s) ||
                                            d.MaBNQL.Contains(s) ||
                                            d.HoTen.Contains(s))
                                    orderby d.MA_TINH
                                    select d).ToList();
            return PartialView("_ComboSearchBenhNhan", ViewData["BenhNhan"]);
        }
    }
}