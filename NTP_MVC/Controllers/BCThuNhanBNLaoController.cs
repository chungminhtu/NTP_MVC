﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using NTP_MVC.Models;
using System.Data.Entity;

namespace NTP_MVC.Controllers
{
    public class BCThuNhanBNLaoController : Controller
    {
        NTP_DBEntities db = new NTP_DBEntities();

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
                GetListBCThuNhanBNLao();

                BCThuNhanModel model = new BCThuNhanModel();
                model.BC_ThuNhanBNLao = ViewData["ListBCThuNhanBNLao"] as List<BC_ThuNhanBNLao>;

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #region GridBCThuNhanBNLao
        [HttpPost, ValidateInput(false)]
        public ActionResult BC_TNBNLAddNew(BC_ThuNhanBNLao item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int MaQuy = Request.Params["MaQuy"] != null ? Convert.ToInt16(Request.Params["MaQuy"]) : 0;
                    int Nam = Request.Params["Nam"] != null ? Convert.ToInt32((Request.Params["Nam"] + "").TrimStart('"').TrimEnd('"')) : 0;


                    item.MA_TINH = Session["MATINH"] + "";
                    item.Quy = (byte)MaQuy;
                    item.Nam = Nam;
                    db.BC_ThuNhanBNLao.Add(item);
                    db.SaveChanges();

                    for (int i = 0; i < 6; i++)
                    {
                        BC_ThuNhanBNLao1 subitem = new BC_ThuNhanBNLao1();
                        subitem.ID_BCThunhanBNLao = item.ID_BCThunhanBNLao;
                        subitem.Phanloai = i;
                        db.BC_ThuNhanBNLao1.Add(subitem);
                    }

                    for (int i = 0; i < 2; i++)
                    {
                        BC_ThuNhanBNLao2 subitem = new BC_ThuNhanBNLao2();
                        subitem.ID_BCThunhanBNLao = item.ID_BCThunhanBNLao;
                        subitem.Phanloai = i;
                        db.BC_ThuNhanBNLao2.Add(subitem);
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
            return GridBCThuNhanBNLao();
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
            return GridBCThuNhanBNLao();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Update3Grid(int ID_BCThunhanBNLao)
        {
            try
            {

                for (int i = 0; i < 6; i++)
                {
                    var BC1 = db.BC_ThuNhanBNLao1.Any(a => a.ID_BCThunhanBNLao == ID_BCThunhanBNLao && a.Phanloai.Equals(i));
                    if (BC1 == false)
                    {
                        BC_ThuNhanBNLao1 subitem = new BC_ThuNhanBNLao1();
                        subitem.ID_BCThunhanBNLao = ID_BCThunhanBNLao;
                        subitem.Phanloai = i;
                        db.BC_ThuNhanBNLao1.Add(subitem);
                    }
                }

                for (int i = 0; i < 2; i++)
                {
                    var BC2 = db.BC_ThuNhanBNLao2.Any(a => a.ID_BCThunhanBNLao == ID_BCThunhanBNLao && a.Phanloai == i);
                    if (BC2 == false)
                    {
                        BC_ThuNhanBNLao2 subitem = new BC_ThuNhanBNLao2();
                        subitem.ID_BCThunhanBNLao = ID_BCThunhanBNLao;
                        subitem.Phanloai = i;
                        db.BC_ThuNhanBNLao2.Add(subitem);
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
        public ActionResult BC_TNBNLDelete(string ID_BCThunhanBNLao)
        {
            return null;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult BC_TNBNLDelete(int ID_BCThunhanBNLao)
        {
            if (ID_BCThunhanBNLao != 0)
            {
                try
                {

                    foreach (var item1 in db.BC_ThuNhanBNLao1.Where(a => a.ID_BCThunhanBNLao.Equals(ID_BCThunhanBNLao)))
                        db.BC_ThuNhanBNLao1.Remove(item1);

                    var item = db.BC_ThuNhanBNLao.FirstOrDefault(it => it.ID_BCThunhanBNLao == ID_BCThunhanBNLao);
                    if (item != null)
                        db.BC_ThuNhanBNLao.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return GridBCThuNhanBNLao();
        }


        [ValidateInput(false)]
        public ActionResult GridBCThuNhanBNLao()
        {
            GetListBCThuNhanBNLao();
            BCThuNhanModel model = new BCThuNhanModel();
            var MaTinh = Session["MATINH"] + "";
            List<DM_Huyen> ListHuyen = new List<DM_Huyen>();
            DM_Huyen h = new DM_Huyen() { TEN_HUYEN = "", MA_HUYEN = "", MA_TINH = MaTinh };
            ListHuyen.Add(h);
            ListHuyen.AddRange(db.DM_Huyen.Where(m => m.MA_TINH.Equals(MaTinh)).ToList());
            ViewData["Huyens"] = ListHuyen;
            model.ListHuyen = ViewData["Huyens"] as List<DM_Huyen>;
            model.BC_ThuNhanBNLao = ViewData["ListBCThuNhanBNLao"] as List<BC_ThuNhanBNLao>;

            return PartialView("GridBCThuNhanBNLao", model);
        }

        private void GetListBCThuNhanBNLao()
        {
            string matinh = Session["MATINH"] + "";

            int MaQuy = Request.Params["MaQuy"] != null ? Convert.ToInt16(Request.Params["MaQuy"]) : 0;
            int Nam = Request.Params["Nam"] != null ? Convert.ToInt32((Request.Params["Nam"] + "").TrimStart('"').TrimEnd('"')) : 0;

            ViewData["ListBCThuNhanBNLao"] = (from d in db.BC_ThuNhanBNLao
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
            if (Request.Params["ID_BCThuNhanBNLao"] != null)
            {
                var ID_BCThuNhanBNLao = Convert.ToInt32(Request.Params["ID_BCThuNhanBNLao"]);

                ViewData["ListBCThuNhanBNLao1"] = (from d in db.BC_ThuNhanBNLao1
                                                   where d.ID_BCThunhanBNLao == ID_BCThuNhanBNLao && (d.Phanloai == 0 || d.Phanloai == 1 || d.Phanloai == 2 || d.Phanloai == 3)
                                                   select d).ToList();

            }
            return PartialView("GridBCTN1");
        }

        [ValidateInput(false)]
        public ActionResult GridBCTN1BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<BC_ThuNhanBNLao1, int> updateValues)
        {
            foreach (var item in updateValues.Update)
            {
                if (updateValues.IsValid(item))
                    UpdateBCThuNhanBNLao1(item, updateValues);
            }
            return GridBCTN1();
        }
        protected void UpdateBCThuNhanBNLao1(BC_ThuNhanBNLao1 item, MVCxGridViewBatchUpdateValues<BC_ThuNhanBNLao1, int> updateValues)
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
            if (Request.Params["ID_BCThuNhanBNLao"] != null)
            {
                var ID_BCThuNhanBNLao = Convert.ToInt32(Request.Params["ID_BCThuNhanBNLao"]);

                ViewData["ListBCThuNhanBNLao2"] = (from d in db.BC_ThuNhanBNLao2
                                                   where d.ID_BCThunhanBNLao == ID_BCThuNhanBNLao
                                                   select d).ToList();

            }
            return PartialView("GridBCTN2");
        }

        [ValidateInput(false)]
        public ActionResult GridBCTN2BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<BC_ThuNhanBNLao2, int> updateValues)
        {
            foreach (var item in updateValues.Update)
            {
                if (updateValues.IsValid(item))
                    UpdateBCThuNhanBNLao2(item, updateValues);
            }
            return GridBCTN2();
        }
        protected void UpdateBCThuNhanBNLao2(BC_ThuNhanBNLao2 item, MVCxGridViewBatchUpdateValues<BC_ThuNhanBNLao2, int> updateValues)
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
            if (Request.Params["ID_BCThuNhanBNLao"] != null)
            {
                var ID_BCThuNhanBNLao = Convert.ToInt32(Request.Params["ID_BCThuNhanBNLao"]);

                ViewData["ListBCThuNhanBNLao3"] = (from d in db.BC_ThuNhanBNLao1
                                                   where d.ID_BCThunhanBNLao == ID_BCThuNhanBNLao && (d.Phanloai == 0 || d.Phanloai == 4 || d.Phanloai == 5)
                                                   select d).ToList();

            }
            return PartialView("GridBCTN3");
        }

        [ValidateInput(false)]
        public ActionResult GridBCTN3BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<BC_ThuNhanBNLao1, int> updateValues)
        {
            foreach (var item in updateValues.Update)
            {
                if (updateValues.IsValid(item))
                    UpdateBCThuNhanBNLao3(item, updateValues);
            }
            return GridBCTN3();
        }
        protected void UpdateBCThuNhanBNLao3(BC_ThuNhanBNLao1 item, MVCxGridViewBatchUpdateValues<BC_ThuNhanBNLao1, int> updateValues)
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

    }
}