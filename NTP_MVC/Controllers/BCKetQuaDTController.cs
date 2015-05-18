using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using NTP_MVC.Models;

namespace NTP_MVC.Controllers
{
    public class BCKetQuaDTController : Controller
    {
        // GET: BC_ThuNhanBNLao
        public ActionResult Index()
        {
            return View();
        }

        NTP_DBEntities db = new NTP_DBEntities();

        [ValidateInput(false)]
        public ActionResult Edit()
        {
            //var model = db.BC_ThuNhanBNLao;
            return View("EditBCKetQuaDT");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult BC_TNBNLAddNew(BC_ThuNhanBNLao item)
        {
            var model = db.BC_ThuNhanBNLao;
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
            return PartialView("_BC_TNBNL", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult BC_TNBNLUpdate(BC_ThuNhanBNLao item)
        {
            var model = db.BC_ThuNhanBNLao;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_BCThunhanBNLao == item.ID_BCThunhanBNLao);
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
            return PartialView("_BC_TNBNL", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult BC_TNBNLDelete(int ID_BCThunhanBNLao)
        {
            var model = db.BC_ThuNhanBNLao;
            if (ID_BCThunhanBNLao != 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.ID_BCThunhanBNLao == ID_BCThunhanBNLao);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_BC_TNBNL", model.ToList());
        }
    }
}