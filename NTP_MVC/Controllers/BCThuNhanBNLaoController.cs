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
    public class BCThuNhanBNLaoController : Controller
    {
        NTP_DBEntities db = new NTP_DBEntities();

        public ActionResult Index()
        {
            Session["MATINH"] = "07";
            Session["MAHUYEN"] = "0703";
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
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [ValidateInput(false)]
        public ActionResult GridBCThuNhanBNLao()
        {

            GetListBCThuNhanBNLao();
            return PartialView("GridBCThuNhanBNLao");
        }

        private void GetListBCThuNhanBNLao()
        {
            string matinh = Session["MATINH"] + "";

            int MaQuy = Request.Params["MaQuy"] != null ? Convert.ToInt16(Request.Params["MaQuy"]) : 0;
            int Nam = Request.Params["Nam"] != null ? Convert.ToInt32(Request.Params["Nam"]) : 0;

            ViewData["ListBCThuNhanBNLao"] = (from d in db.BC_ThuNhanBNLao
                                              where d.MA_TINH.Equals(matinh)
                                                  && d.Quy == MaQuy
                                                  && d.Nam == Nam
                                              select d).ToList();
        }

        [ValidateInput(false)]
        public ActionResult Edit()
        {

            int id = Request.Params["id"] != null ? Convert.ToInt32(Request.Params["id"]) : 0;
            if (id != 0)
            {
                var BCThuNhan = db.BC_ThuNhanBNLao.Where(a => a.ID_BCThunhanBNLao == id).FirstOrDefault();
                var MaTinh = BCThuNhan.MA_TINH;
                var MaHuyen = BCThuNhan.MA_HUYEN;
                ViewData["Tinhs"] = db.DM_Tinh.Where(t => t.MA_TINH.Equals(BCThuNhan.MA_TINH)).ToList();
                if (BCThuNhan.MA_HUYEN != null && BCThuNhan.MA_HUYEN != "")
                {
                    List<DM_Huyen> ListHuyen = new List<DM_Huyen>();
                    DM_Huyen h = new DM_Huyen() { TEN_HUYEN = "", MA_HUYEN = "", MA_TINH = MaTinh };
                    ListHuyen.Add(h);
                    ListHuyen.AddRange(db.DM_Huyen.Where(m => m.MA_TINH.Equals(MaTinh)).ToList());
                    ViewData["Huyens"] = ListHuyen;
                }
                else
                {
                    ViewData["Huyens"] = db.DM_Huyen.Where(b => b.MA_HUYEN.Equals(MaHuyen)).ToList();
                }
                return View("EditBCThuNhanBNLao", BCThuNhan);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost, ValidateInput(false)]

        public ActionResult Edit([ModelBinder(typeof(DevExpressEditorsBinder))] BC_ThuNhanBNLao bctn)
        {
            return null;
        }


        public ActionResult GridBCThuNhanBNLao1()
        {
            var MaTinh = Session["MATINH"] + "";
            if (Request.Params["MaHuyen"] != null)
            {
                var MaHuyen = Session["MAHUYEN"] + "";
                int id = Request.Params["id"] != null ? Convert.ToInt32(Request.Params["id"]) : 0;

                string nguoibaocao = Request.Params["NguoiBaoCao"] + "";
                DateTime ngaybaocao = Request.Params["NgayBaoCao"] != "" ? Convert.ToDateTime(Request.Params["NgayBaoCao"]) : DateTime.MinValue;

                int MaQuy = Request.Params["MaQuy"] != null ? Convert.ToInt16(Request.Params["MaQuy"]) : 0;
                int Nam = Request.Params["Nam"] != null ? Convert.ToInt32(Request.Params["Nam"]) : 0;

                var id_BCThuNhanNBLao = (from d in db.BC_ThuNhanBNLao
                                         where d.MA_TINH.Equals(MaTinh)
                                             && d.MA_HUYEN.Equals(MaHuyen)
                                             && d.Quy == MaQuy
                                             && d.Nam == Nam
                                         select d.ID_BCThunhanBNLao).SingleOrDefault();
                if (id_BCThuNhanNBLao != 0)
                {
                    ViewData["ListBCThuNhanBNLao1"] = (from d in db.BC_ThuNhanBNLao1
                                                       where d.ID_BCThunhanBNLao == id_BCThuNhanNBLao
                                                       select d).ToList();
                }
                else
                {
                    BC_ThuNhanBNLao item = new BC_ThuNhanBNLao();
                    item.MA_TINH = MaTinh;
                    item.MA_HUYEN = MaHuyen;
                    item.Nam = Nam;
                    item.Quy = (byte)MaQuy;
                    if (ngaybaocao != DateTime.MinValue)
                    {
                        item.NgayBC = ngaybaocao;
                    }

                    item.NguoiBC = nguoibaocao;
                    item.DVBaocao = MaHuyen;
                    db.BC_ThuNhanBNLao.Add(item);
                    db.SaveChanges();

                    for (int i = 0; i < 3; i++)
                    {
                        BC_ThuNhanBNLao1 subitem = new BC_ThuNhanBNLao1();
                        subitem.ID_BCThunhanBNLao = item.ID_BCThunhanBNLao;
                        subitem.Phanloai = i;
                        db.BC_ThuNhanBNLao1.Add(subitem);
                    }
                    db.SaveChanges();
                    ViewData["ListBCThuNhanBNLao1"] = (from d in db.BC_ThuNhanBNLao1
                                                       where d.ID_BCThunhanBNLao.Equals(item.ID_BCThunhanBNLao)
                                                       select d).ToList();

                }
            }
            return PartialView("GridBCThuNhanBNLao1");
        }

        [ValidateInput(false)]
        public ActionResult GridBCThuNhanBNLao1BatchEditingUpdateModel(MVCxGridViewBatchUpdateValues<BC_ThuNhanBNLao1, int> updateValues)
        {
            foreach (var item in updateValues.Update)
            {
                if (updateValues.IsValid(item))
                    UpdateBCThuNhanBNLao1(item, updateValues);
            }
            return GridBCThuNhanBNLao1();
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


        [HttpPost, ValidateInput(false)]
        public ActionResult GridBCThuNhanBNLao1AddNew(BC_ThuNhanBNLao1 item)
        {
            var model = db.BC_ThuNhanBNLao1;
            if (ModelState.IsValid)
            {
                try
                {
                    db.BC_ThuNhanBNLao1.Add(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("GridBCThuNhanBNLao1", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridBCThuNhanBNLao1Update(BC_ThuNhanBNLao1 item)
        {
            var model = db.BC_ThuNhanBNLao1;
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
            return PartialView("GridBCThuNhanBNLao1", model.ToList());
        }


        public ActionResult GridBCThuNhanBNLao2()
        {
            //var MaTinh = "65";// Session["MATINH"] + "";  
            var MaTinh = Session["MATINH"] + "";
            if (Request.Params["MaHuyen"] != null)
            {

                //var MaHuyen = Request.Params["MaHuyen"] + "";// Session["MAHUYEN"] + "";
                var MaHuyen = Session["MAHUYEN"] + "";
                int id = Request.Params["id"] != null ? Convert.ToInt32(Request.Params["id"]) : 0;

                string nguoibaocao = Request.Params["NguoiBaoCao"] + "";
                DateTime ngaybaocao = Request.Params["NgayBaoCao"] != "" ? Convert.ToDateTime(Request.Params["NgayBaoCao"]) : DateTime.MinValue;

                int MaQuy = Request.Params["MaQuy"] != null ? Convert.ToInt16(Request.Params["MaQuy"]) : 0;
                int Nam = Request.Params["Nam"] != null ? Convert.ToInt32(Request.Params["Nam"]) : 0;

                var id_BCThuNhanNBLao = (from d in db.BC_ThuNhanBNLao
                                         where d.MA_TINH.Equals(MaTinh)
                                             && d.MA_HUYEN.Equals(MaHuyen)
                                             && d.Quy == MaQuy
                                             && d.Nam == Nam
                                         select d.ID_BCThunhanBNLao).SingleOrDefault();
                if (id_BCThuNhanNBLao != 0)
                {
                    ViewData["ListBCThuNhanBNLao2"] = (from d in db.BC_ThuNhanBNLao1
                                                       where d.ID_BCThunhanBNLao == id_BCThuNhanNBLao
                                                       select d).ToList();
                }
                else
                {
                    BC_ThuNhanBNLao item = new BC_ThuNhanBNLao();
                    item.MA_TINH = MaTinh;
                    item.MA_HUYEN = MaHuyen;
                    item.Nam = Nam;
                    item.Quy = (byte)MaQuy;
                    if (ngaybaocao != DateTime.MinValue)
                    {
                        item.NgayBC = ngaybaocao;
                    }

                    item.NguoiBC = nguoibaocao;
                    item.DVBaocao = MaHuyen;
                    db.BC_ThuNhanBNLao.Add(item);
                    db.SaveChanges();

                    for (int i = 0; i < 2; i++)
                    {
                        BC_ThuNhanBNLao1 subitem = new BC_ThuNhanBNLao1();
                        subitem.ID_BCThunhanBNLao = item.ID_BCThunhanBNLao;
                        subitem.Phanloai = i;
                        db.BC_ThuNhanBNLao1.Add(subitem);
                    }
                    db.SaveChanges();
                    ViewData["ListBCThuNhanBNLao1"] = (from d in db.BC_ThuNhanBNLao1
                                                       where d.ID_BCThunhanBNLao.Equals(item.ID_BCThunhanBNLao)
                                                       select d).ToList();

                }
            }
            return PartialView("GridBCThuNhanBNLao1");
        }

        public ActionResult GridBCThuNhanBNLao3()
        {
            //var MaTinh = "65";// Session["MATINH"] + "";  
            var MaTinh = Session["MATINH"] + "";
            if (Request.Params["MaHuyen"] != null)
            {

                //var MaHuyen = Request.Params["MaHuyen"] + "";// Session["MAHUYEN"] + "";
                var MaHuyen = Session["MAHUYEN"] + "";
                int id = Request.Params["id"] != null ? Convert.ToInt32(Request.Params["id"]) : 0;

                string nguoibaocao = Request.Params["NguoiBaoCao"] + "";
                DateTime ngaybaocao = Request.Params["NgayBaoCao"] != "" ? Convert.ToDateTime(Request.Params["NgayBaoCao"]) : DateTime.MinValue;

                int MaQuy = Request.Params["MaQuy"] != null ? Convert.ToInt16(Request.Params["MaQuy"]) : 0;
                int Nam = Request.Params["Nam"] != null ? Convert.ToInt32(Request.Params["Nam"]) : 0;

                var id_BCThuNhanNBLao = (from d in db.BC_ThuNhanBNLao
                                         where d.MA_TINH.Equals(MaTinh)
                                             && d.MA_HUYEN.Equals(MaHuyen)
                                             && d.Quy == MaQuy
                                             && d.Nam == Nam
                                         select d.ID_BCThunhanBNLao).SingleOrDefault();
                if (id_BCThuNhanNBLao != 0)
                {
                    ViewData["ListBCThuNhanBNLao2"] = (from d in db.BC_ThuNhanBNLao2
                                                       where d.ID_BCThunhanBNLao == id_BCThuNhanNBLao
                                                       select d).ToList();
                }
                else
                {
                    BC_ThuNhanBNLao item = new BC_ThuNhanBNLao();
                    item.MA_TINH = MaTinh;
                    item.MA_HUYEN = MaHuyen;
                    item.Nam = Nam;
                    item.Quy = (byte)MaQuy;
                    if (ngaybaocao != DateTime.MinValue)
                    {
                        item.NgayBC = ngaybaocao;
                    }

                    item.NguoiBC = nguoibaocao;
                    item.DVBaocao = MaHuyen;
                    db.BC_ThuNhanBNLao.Add(item);
                    db.SaveChanges();

                    for (int i = 0; i < 2; i++)
                    {
                        BC_ThuNhanBNLao2 subitem = new BC_ThuNhanBNLao2();
                        subitem.ID_BCThunhanBNLao = item.ID_BCThunhanBNLao;
                        subitem.Phanloai = i;
                        db.BC_ThuNhanBNLao2.Add(subitem);
                    }
                    db.SaveChanges();
                    ViewData["ListBCThuNhanBNLao2"] = (from d in db.BC_ThuNhanBNLao2
                                                       where d.ID_BCThunhanBNLao.Equals(item.ID_BCThunhanBNLao)
                                                       select d).ToList();

                }
            }
            return PartialView("GridBCThuNhanBNLao2");
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult BC_ThuNhanBNLaoGridViewPartialAddNew(BC_ThuNhanBNLao item)
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
            return PartialView("_BC_ThuNhanBNLaoGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult BC_ThuNhanBNLaoGridViewPartialUpdate(BC_ThuNhanBNLao item)
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
            return PartialView("_BC_ThuNhanBNLaoGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult BC_ThuNhanBNLaoGridViewPartialDelete(int ID_BCThunhanBNLao)
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
            return PartialView("_BC_ThuNhanBNLaoGridViewPartial", model.ToList());
        }
    }
}