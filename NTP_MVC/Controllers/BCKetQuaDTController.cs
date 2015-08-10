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
    public class BCKetQuaDTController : Controller
    {
        // GET: BC_KetQuaDT
        public ActionResult Index()
        {
            //Session["MATINH"] = "07";
            //Session["MAHUYEN"] = "0703";
            var MaTinh = Session["MATINH"] + "";
            if (MaTinh != "")
            {
                var MaHuyen = Session["MAHUYEN"] + "";
                ViewData["Tinhs"] = db.DM_Tinh.Where(t => t.MA_TINH.Equals(MaTinh)).ToList();
                if (MaHuyen == "")
                {
                    List<DM_Huyen> ListHuyen = new List<DM_Huyen>();
                    DM_Huyen h = new DM_Huyen() { TEN_HUYEN = "-- Tất cả--", MA_HUYEN = "0", MA_TINH = MaTinh };
                    ListHuyen.Add(h);
                    ListHuyen.AddRange(db.DM_Huyen.Where(m => m.MA_TINH.Equals(MaTinh)).ToList());
                    ViewData["Huyens"] = ListHuyen;
                }
                else
                {
                    ViewData["Huyens"] = db.DM_Huyen.Where(b => b.MA_HUYEN.Equals(MaHuyen)).ToList();
                }
                GetListBCKetQuaDT();

                BCDieuTriModel model = new BCDieuTriModel();
                model.BC_KQDieuTriBNLao = ViewData["ListBCKetQuaDT"] as List<BC_KetQuaDT>;

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        NTP_DBEntities db = new NTP_DBEntities();
        #region GridBCKetQuaDT
        [HttpPost, ValidateInput(false)]
        public ActionResult BC_TNBNLAddNew(BC_KetQuaDT item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int MaQuy = Request.Params["MaQuy"] != null ? Convert.ToInt16(Request.Params["MaQuy"]) : 0;
                    int Nam = Request.Params["Nam"] != null ? Convert.ToInt32((Request.Params["Nam"] + "").TrimStart('"').TrimEnd('"')) : 0;
                    if (MaQuy == 0) MaQuy = 1;

                    item.MA_TINH = Session["MATINH"] + "";
                    item.Quy = (byte)MaQuy;
                    item.Nam = Nam;
                    db.BC_KetQuaDT.Add(item);
                    db.SaveChanges();

                    for (int i = 0; i < 6; i++)
                    {
                        BC_KetQuaDT1 subitem = new BC_KetQuaDT1();
                        subitem.ID_BCKetQuaDT = item.ID_BCKetQuaDT;
                        // subitem.Phanloai = i;
                        db.BC_KetQuaDT1.Add(subitem);
                    }

                    for (int i = 0; i < 2; i++)
                    {
                        BC_KetQuaDT1 subitem = new BC_KetQuaDT1();
                        subitem.ID_BCKetQuaDT = item.ID_BCKetQuaDT;
                        //subitem.Phanloai = i;
                        db.BC_KetQuaDT1.Add(subitem);
                    }

                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return GridBCKetQuaDT();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult BC_TNBNLUpdate(BC_KetQuaDT item)
        {
            var model = db.BC_KetQuaDT;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.ID_BCKetQuaDT == item.ID_BCKetQuaDT);
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
            return GridBCKetQuaDT();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update3Grid(int ID_BCKetQuaDT)
        {
            try
            {

                for (int i = 0; i < 6; i++)
                {
                    var BC1 = db.BC_KetQuaDT1.Any(a => a.ID_BCKetQuaDT == ID_BCKetQuaDT);
                    if (BC1 == false)
                    {
                        BC_KetQuaDT1 subitem = new BC_KetQuaDT1();
                        subitem.ID_BCKetQuaDT = ID_BCKetQuaDT;
                        //subitem.Phanloai = i;
                        db.BC_KetQuaDT1.Add(subitem);
                    }
                }

                for (int i = 0; i < 2; i++)
                {
                    var BC2 = db.BC_KetQuaDT1.Any(a => a.ID_BCKetQuaDT == ID_BCKetQuaDT);
                    if (BC2 == false)
                    {
                        BC_KetQuaDT1 subitem = new BC_KetQuaDT1();
                        subitem.ID_BCKetQuaDT = ID_BCKetQuaDT;
                        //subitem.Phanloai = i;
                        db.BC_KetQuaDT1.Add(subitem);
                    }
                }
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            return null;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult BC_TNBNLDelete(string ID_BCKetQuaDT)
        {
            return null;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult BC_TNBNLDelete(int ID_BCKetQuaDT)
        {
            if (ID_BCKetQuaDT != 0)
            {
                try
                {

                    foreach (var item1 in db.BC_KetQuaDT1.Where(a => a.ID_BCKetQuaDT.Equals(ID_BCKetQuaDT)))
                        db.BC_KetQuaDT1.Remove(item1);

                    var item = db.BC_KetQuaDT.FirstOrDefault(it => it.ID_BCKetQuaDT == ID_BCKetQuaDT);
                    if (item != null)
                        db.BC_KetQuaDT.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return GridBCKetQuaDT();
        }


        [ValidateInput(false)]
        public ActionResult GridBCKetQuaDT()
        {
            GetListBCKetQuaDT();
            BCDieuTriModel model = new BCDieuTriModel();
            var MaTinh = Session["MATINH"] + "";
            List<DM_Huyen> ListHuyen = new List<DM_Huyen>();
            DM_Huyen h = new DM_Huyen() { TEN_HUYEN = "", MA_HUYEN = "", MA_TINH = MaTinh };
            ListHuyen.Add(h);
            ListHuyen.AddRange(db.DM_Huyen.Where(m => m.MA_TINH.Equals(MaTinh)).ToList());
            ViewData["Huyens"] = ListHuyen;
            model.ListHuyen = ViewData["Huyens"] as List<DM_Huyen>;
            model.BC_KQDieuTriBNLao = ViewData["ListBCKetQuaDT"] as List<BC_KetQuaDT>;

            return PartialView("GridBCKetQuaDT", model);
        }

        private void GetListBCKetQuaDT()
        {
            string matinh = Session["MATINH"] + "";

            int MaQuy = Request.Params["MaQuy"] != null ? Convert.ToInt16(Request.Params["MaQuy"]) : 1;
            int Nam = Request.Params["Nam"] != null ? Convert.ToInt32((Request.Params["Nam"] + "").TrimStart('"').TrimEnd('"')) : DateTime.Now.Year;
            
            ViewData["ListBCKetQuaDT"] = (from d in db.BC_KetQuaDT
                                          where d.MA_TINH.Equals(matinh)
                                              && d.Quy == MaQuy
                                              && d.Nam == Nam
                                          select d).ToList();
        }
        #endregion


        #region GridBCTN1
        /*--------------------GridBCTN1----------------------*/
        public ActionResult GridBCTN1()
        {
            if (Request.Params["ID_BCKetQuaDT"] != null)
            {
                var ID_BCKetQuaDT = Convert.ToInt32(Request.Params["ID_BCKetQuaDT"]);

                ViewData["ListBCKetQuaDT1"] = (from d in db.BC_KetQuaDT1
                                               where d.ID_BCKetQuaDT == ID_BCKetQuaDT
                                               select d).ToList();

            }
            return PartialView("GridBCTN1");
        }

        [ValidateInput(false)]
        public ActionResult GridBCTN1BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<BC_KetQuaDT1, int> updateValues)
        {
            foreach (var item in updateValues.Update)
            {
                if (updateValues.IsValid(item))
                    UpdateBCKetQuaDT1(item, updateValues);
            }
            return GridBCTN1();
        }
        protected void UpdateBCKetQuaDT1(BC_KetQuaDT1 item, MVCxGridViewBatchUpdateValues<BC_KetQuaDT1, int> updateValues)
        {
            try
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(item, e.Message);
            }
        }
        /*-------------------------------------------------------------------*/

        #endregion


        #region GridBCTN2
        /*--------------------GridBCTN2----------------------*/
        public ActionResult GridBCTN2()
        {
            if (Request.Params["ID_BCKetQuaDT"] != null)
            {
                var ID_BCKetQuaDT = Convert.ToInt32(Request.Params["ID_BCKetQuaDT"]);

                ViewData["ListBCKetQuaDT2"] = (from d in db.BC_KetQuaDT1
                                               where d.ID_BCKetQuaDT == ID_BCKetQuaDT
                                               select d).ToList();

            }
            return PartialView("GridBCTN2");
        }

        [ValidateInput(false)]
        public ActionResult GridBCTN2BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<BC_KetQuaDT1, int> updateValues)
        {
            foreach (var item in updateValues.Update)
            {
                if (updateValues.IsValid(item))
                    UpdateBCKetQuaDT2(item, updateValues);
            }
            return GridBCTN2();
        }
        protected void UpdateBCKetQuaDT2(BC_KetQuaDT1 item, MVCxGridViewBatchUpdateValues<BC_KetQuaDT1, int> updateValues)
        {
            try
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(item, e.Message);
            }
        }
        /*-------------------------------------------------------------------*/
        #endregion

        #region GridBCTN3
        /*--------------------GridBCTN3----------------------*/
        public ActionResult GridBCTN3()
        {
            if (Request.Params["ID_BCKetQuaDT"] != null)
            {
                var ID_BCKetQuaDT = Convert.ToInt32(Request.Params["ID_BCKetQuaDT"]);

                ViewData["ListBCKetQuaDT3"] = (from d in db.BC_KetQuaDT1
                                               where d.ID_BCKetQuaDT == ID_BCKetQuaDT
                                               select d).ToList();

            }
            return PartialView("GridBCTN3");
        }

        [ValidateInput(false)]
        public ActionResult GridBCTN3BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<BC_KetQuaDT1, int> updateValues)
        {
            foreach (var item in updateValues.Update)
            {
                if (updateValues.IsValid(item))
                    UpdateBCKetQuaDT3(item, updateValues);
            }
            return GridBCTN3();
        }
        protected void UpdateBCKetQuaDT3(BC_KetQuaDT1 item, MVCxGridViewBatchUpdateValues<BC_KetQuaDT1, int> updateValues)
        {
            try
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(item, e.Message);
            }
        }
        /*-------------------------------------------------------------------*/
        #endregion

        #region GridBCTN4
        /*--------------------GridBCTN4----------------------*/
        public ActionResult GridBCTN4()
        {
            if (Request.Params["ID_BCKetQuaDT"] != null)
            {
                var ID_BCKetQuaDT = Convert.ToInt32(Request.Params["ID_BCKetQuaDT"]);

                ViewData["ListBCKetQuaDT4"] = (from d in db.BC_KetQuaDT1
                                               where d.ID_BCKetQuaDT == ID_BCKetQuaDT
                                               select d).ToList();

            }
            return PartialView("GridBCTN4");
        }

        [ValidateInput(false)]
        public ActionResult GridBCTN4BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<BC_KetQuaDT1, int> updateValues)
        {
            foreach (var item in updateValues.Update)
            {
                if (updateValues.IsValid(item))
                    UpdateBCKetQuaDT1(item, updateValues);
            }
            return GridBCTN4();
        }


        #endregion


        #region GridBCTN5
        /*--------------------GridBCTN5----------------------*/
        public ActionResult GridBCTN5()
        {
            if (Request.Params["ID_BCKetQuaDT"] != null)
            {
                var ID_BCKetQuaDT = Convert.ToInt32(Request.Params["ID_BCKetQuaDT"]);

                ViewData["ListBCKetQuaDT4"] = (from d in db.BC_KetQuaDT1
                                               where d.ID_BCKetQuaDT == ID_BCKetQuaDT
                                               select d).ToList();

            }
            return PartialView("GridBCTN5");
        }

        [ValidateInput(false)]
        public ActionResult GridBCTN5BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<BC_KetQuaDT1, int> updateValues)
        {
            foreach (var item in updateValues.Update)
            {
                if (updateValues.IsValid(item))
                    UpdateBCKetQuaDT1(item, updateValues);
            }
            return GridBCTN5();
        }
        /*-------------------------------------------------------------------*/
        #endregion

        #region GridBCTN6
        /*--------------------GridBCTN6----------------------*/
        public ActionResult GridBCTN6()
        {
            if (Request.Params["ID_BCKetQuaDT"] != null)
            {
                var ID_BCKetQuaDT = Convert.ToInt32(Request.Params["ID_BCKetQuaDT"]);

                ViewData["ListBCKetQuaDT6"] = (from d in db.BC_KetQuaDT1
                                               where d.ID_BCKetQuaDT == ID_BCKetQuaDT
                                               select d).ToList();

            }
            return PartialView("GridBCTN6");
        }

        [ValidateInput(false)]
        public ActionResult GridBCTN6BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<BC_KetQuaDT1, int> updateValues)
        {
            foreach (var item in updateValues.Update)
            {
                if (updateValues.IsValid(item))
                    UpdateBCKetQuaDT1(item, updateValues);
            }
            return GridBCTN6();
        }
        /*-------------------------------------------------------------------*/
        #endregion
    }
}