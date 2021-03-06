﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using NTP_MVC.Models;

namespace NTP_MVC.Controllers
{
    public class DMCapController : Controller
    {
        // GET: DMCap
        public ActionResult Index()
        {
            return View();
        }

        NTP_DBEntities db = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult DMCapGridView1Partial()
        {
            var model = db.DM_Cap;
            return PartialView("_DMCapGridView1Partial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DMCapGridView1PartialAddNew(DM_Cap item)
        {
            var model = db.DM_Cap;
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
            return PartialView("_DMCapGridView1Partial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DMCapGridView1PartialUpdate(DM_Cap item)
        {
            var model = db.DM_Cap;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_CAP == item.ID_CAP);
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
            return PartialView("_DMCapGridView1Partial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DMCapGridView1PartialDelete(Int32 ID_CAP)
        {
            var model = db.DM_Cap;
            if (ID_CAP >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_CAP == ID_CAP);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_DMCapGridView1Partial", model.ToList());
        }
    }
}