using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using NTP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NTP_MVC.Controllers
{
    public class BaoCaoController : Controller
    {
        NTP_OLDEntities db = new NTP_OLDEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ShowReport()
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = "generic.rpt";
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("mm/dd/yyyy");
            string strToDate = DateTime.Now.ToString("mm/dd/yyyy");
            try
            {
                if (isValid)
                {
                    ReportDocument rd = new ReportDocument();
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "App_Data//" + strReportName;
                    //var rptSource = GetStudents();
                    rd.Load(strRptPath);
                    //if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                    //    rd.SetDataSource(rptSource);
                    if (!string.IsNullOrEmpty(strFromDate))
                        rd.SetParameterValue("fromDate", strFromDate);
                    if (!string.IsNullOrEmpty(strToDate))
                        rd.SetParameterValue("toDate", strFromDate);
                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");

                }
                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);
        }

        public void te()
        {

            string Tinh = null;
            string DVBaocao = null;

            int Nam = 0;
            string Quy = null;
            int Opt = 0;
            string Tentinh = null;
            string sNguoibc = null;
            string sNgaybc = null;
            int Capin = 0;
            FormulaFieldDefinitions Myformulas = default(FormulaFieldDefinitions);
            ReportDocument rpt = new ReportDocument();

            if (string.IsNullOrEmpty(Request.Params["Tinh"]))
            {
                Tinh = "ALL";
            }
            else
            {
                Tinh = Request.Params["Tinh"];
            }

            if (string.IsNullOrEmpty((Request.Params["DVBaocao"] + "").Trim()))
            {
                DVBaocao = "ALL";
            }
            else
            {
                DVBaocao = Request.Params["DVBaocao"];
            }

            if (string.IsNullOrEmpty(Request.Params["Tentinh"]))
            {
                Tentinh = "";
            }
            else
            {
                Tentinh = Request.Params["Tentinh"];
            }
            Nam = Convert.ToInt32(Request.Params["Nam"]);
            if (string.IsNullOrEmpty(Request.Params["Quy"]))
            {
                Quy = "0";
            }
            else
            {
                Quy = Request.Params["Quy"];
            }
            Opt = Convert.ToInt32(Request.Params["Opt"]);
            sNguoibc = Request.Params["Nguoibc"];
            sNgaybc = Request.Params["Ngaybc"];


            if (Opt == 1)
            {
                db.NTP_BN_BCTONGHOPTUYENTINH_IN(Tinh, Nam, Quy, 0, DVBaocao);
                //ds = obj.ListBaoCaoTuyenTinh(Tinh, Nam, Quy, 0, DVBaocao);
            }
            else if (Opt == 2)
            {
                //ds = obj.ListBaoCaoTuyenTinh(Tinh, Nam, Quy, 1, DVBaocao);
                //dsP2 = obj.ListBaoCaoTuyenTinh(Tinh, Nam, Quy, 1, DVBaocao);
                //dsP3 = obj.ListBaoCaoTuyenTinh(Tinh, Nam, Quy, 1, DVBaocao);
            }
            else if (Opt == 3)
            {
                //ds = obj.ListBaoCaoTuyenTinh(Tinh, Nam, Quy, 2, DVBaocao);
            }
            else if (Opt == 4)
            {
                //ds = obj.ListBaoCaoTuyenTinh(Tinh, Nam, Quy, 3, DVBaocao);
                //dsP2 = obj.ListBaoCaoTuyenTinh(Tinh, Nam, Quy, 3, DVBaocao);
                //dsP3 = obj.ListBaoCaoTuyenTinh(Tinh, Nam, Quy, 3, DVBaocao);
            }
            else if (Opt == 5)
            {
                //    ds = objKD.ListBaoCaoTuyenTinh(Tinh, Nam, Quy, 0, DVBaocao);
                //    dsP2 = objKD.ListBaoCaoTuyenTinh(Tinh, Nam, Quy, 0, DVBaocao);
            }
            else if (Opt == 6)
            {
                Capin = Convert.ToInt32(Request.Params["capin"]);
                if (Capin == 1)
                {
                    //ds = objKD.ListBaoCaoTuyenTinh(Tinh, Nam, Quy, 1, DVBaocao);
                    //dsP2 = objKD.ListBaoCaoTuyenTinh(Tinh, Nam, Quy, 1, DVBaocao);
                }
                else
                {
                    //ds = objKD.ListBaoCaoTuyenTinh(Tinh, Nam, Quy, 2, DVBaocao);
                    //dsP2 = objKD.ListBaoCaoTuyenTinh(Tinh, Nam, Quy, 2, DVBaocao);
                }
            }

            string sPath = null;
            if (Opt == 1)
            {
                sPath = this.Request.PhysicalApplicationPath + "\\BN_THUNHANBNLAO_TINH.rpt";
            }
            else if (Opt == 2)
            {
                sPath = this.Request.PhysicalApplicationPath + "\\BN_KETQUADIEUTRILAO_TINH.rpt";
            }
            else if (Opt == 3)
            {
                sPath = this.Request.PhysicalApplicationPath + "\\BN_KETQUAAMHOADOM_TINH.rpt";
            }
            else if (Opt == 4)
            {
                sPath = this.Request.PhysicalApplicationPath + "\\BN_CHUONGTRINHCHONGLAO_TINH.rpt";
                // Kiem dinh TB
            }
            else if (Opt == 5)
            {
                sPath = this.Request.PhysicalApplicationPath + "\\BN_HOATDONGXN_TINH.rpt";
            }
            else if (Opt == 6)
            {
                sPath = this.Request.PhysicalApplicationPath + "\\BN_KIEMDINHTB_TINH.rpt";
            }

            rpt.Load(sPath);
            if (Opt == 1 | Opt == 3)
            {
                Myformulas = rpt.DataDefinition.FormulaFields;
                if (Opt != 3)
                {
                    //Myformulas.Item("Quy").Text = "'" + Quy + "'";
                    //Myformulas.Item("Nam").Text = "'" + Nam + "'";
                }
                else
                {
                    if (Quy.Length == 1)
                    {
                        //Myformulas.Item("Quy").Text = "'" + (Quy == 1 ? 4 : Quy - 1) + "'";
                        //Myformulas.Item("Nam").Text = "'" + (Quy == 1 ? Nam - 1 : Nam) + "'";
                    }
                    else
                    {
                        //Myformulas.Item("Quy").Text = "'" + Quy + "'";
                        //Myformulas.Item("Nam").Text = "'" + Nam + "'";
                    }
                }
                // Myformulas.Item("Tinh").Text = "'" & Tentinh & "'"
                //Myformulas.Item("Nguoibc").Text = "'" + Strings.UCase(sNguoibc) + "'";
                //Myformulas.Item("Ngaybc").Text = "'" + sNgaybc + "'";
                //rpt.SetDataSource(ds.Tables(0));
            }
            else if (Opt == 2)
            {
                //ReportDocument rptP2 = new ReportDocument();
                //FormulaFieldDefinitions Formular = default(FormulaFieldDefinitions);
                //rptP2 = rpt.Subreports("BN_KETQUADIEUTRILAO_TINHP2.rpt");
                //ReportDocument rptP3 = new ReportDocument();
                //rptP3 = rpt.Subreports("BN_KETQUADIEUTRILAO_TINHP3.rpt");
                //Myformulas = rpt.DataDefinition.FormulaFields;
                //Formular = rptP3.DataDefinition.FormulaFields;
                //Myformulas.Item("Quy").Text = "'" + Quy + "'";
                //Myformulas.Item("Nam").Text = "'" + Nam + "'";
                ////Myformulas.Item("Tinh").Text = "'" & Tentinh & "'"
                //Myformulas.Item("Nguoibc").Text = "'" + Strings.UCase(sNguoibc) + "'";
                //Myformulas.Item("Ngaybc").Text = "'" + Strings.UCase(sNguoibc) + "'";
                //rptP2.SetDataSource(dsP2.Tables(0));
                //rptP3.SetDataSource(dsP3.Tables(0));
                //rpt.SetDataSource(ds.Tables(0));

            }
            else if (Opt == 4)
            {
                //ReportDocument rptP22 = new ReportDocument();
                //rptP22 = rpt.Subreports("BN_CHUONGTRINHCHONGLAO_TINHP2.rpt");
                //ReportDocument rptP33 = new ReportDocument();
                //rptP33 = rpt.Subreports("BN_CHUONGTRINHCHONGLAO_TINHP3.rpt");
                //Myformulas = rpt.DataDefinition.FormulaFields;
                ////Myformulas.Item("Quy").Text = "'" & Quy & "'"
                //Myformulas.Item("Nam").Text = "'" + Nam + "'";
                ////Myformulas.Item("Tinh").Text = "'" & Tentinh & "'"
                //Myformulas.Item("Nguoibc").Text = "'" + sNguoibc + "'";
                //rptP22.SetDataSource(dsP2.Tables(0));
                //rptP33.SetDataSource(dsP3.Tables(0));

                //rpt.SetDataSource(ds.Tables(0));
            }
            else if (Opt == 5 | Opt == 6)
            {
                ReportDocument rptP2 = new ReportDocument();
                if (Opt == 5)
                {
                    //rptP2 = rpt.Subreports("BN_HOATDONGXN_TINHP2.rpt");
                    //Myformulas = rpt.DataDefinition.FormulaFields;
                    //Myformulas.Item("Quy").Text = "'" + Quy + "'";
                    //Myformulas.Item("Nam").Text = "'" + Nam + "'";
                    ////Myformulas.Item("Tinh").Text = "'" & UCase(Tentinh) & "'"
                }
                else
                {
                    //rptP2 = rpt.Subreports("BN_KIEMDINHTB_TINHP2.rpt");
                    //Myformulas = rpt.DataDefinition.FormulaFields;
                    //Myformulas.Item("Quy").Text = "'" + Quy + "'";
                    //Myformulas.Item("Nam").Text = "'" + Nam + "'";
                    //Myformulas.Item("Tinh").Text = "'" + Tentinh + "'";
                }

                if (Opt == 5)
                {
                    //FormulaFieldDefinitions Formular = default(FormulaFieldDefinitions);
                    //Formular = rptP2.DataDefinition.FormulaFields;
                    //Formular.Item("Nguoibc").Text = "'" + Strings.UCase(sNguoibc) + "'";
                }
                //rptP2.SetDataSource(dsP2.Tables(0));
                //rpt.SetDataSource(ds.Tables(0));

            }
            //this.CrystalReportViewer1.ReportSource = rpt;
            //this.CrystalReportViewer1.DataBind();
            //ds = null;

        }
        public ActionResult RefreshReport()
        {
            return PartialView("_ReportsDisplay");
        }
    }
}