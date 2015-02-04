﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using NTP_MVC.Models;

namespace NTP_MVC.Controllers
{
    public class SoDieuTriController : Controller
    {
        // GET: SoDieuTri
        public ActionResult Index()
        {
            return View();
        }

        NTP_DBEntities db = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult GridSoDieuTriList()
        {
            GetSoDieuTriCuaBenhNhan();
            return PartialView("_GridSoDieuTri", ViewData["ListSoDieuTri"]);
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
            return PartialView("_GridSoDieuTri", ViewData["ListSoKhamBenh"]);
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
            return PartialView("_GridSoDieuTri", ViewData["ListSoKhamBenh"]);
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
            return PartialView("_GridSoDieuTri", ViewData["ListSoKhamBenh"]);
        }
    }
}