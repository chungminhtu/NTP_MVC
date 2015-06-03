/*
 * TB Treatment & Supervision Supply System. Copyright (C) 2013 PATH (www.path.org)
 *  
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
 * Author: Tran Trung Hieu
 * Email: htran282@gmail.com / htran@path.org
 * Mobile: +84-0-915164626
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NTP_MVC.Models;
using System.Web.Services;
using System.Web.Script.Serialization;
using NTP_MVC.Models.VO;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data.Entity;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Globalization;
using System.Data;
using NghiepCT.Utilities.Security;
using System.Text;

namespace NTP_MVC.Controllers
{
    public class HoTroDieuTriController : Controller
    {
        NTP_DBEntities db = new NTP_DBEntities();        

        public ActionResult Index()
        {
            HTDTGS model = new HTDTGS();
            return View(model);
        }

        public ActionResult DSBenhNhanXN()
        {
            HTDTGS model = new HTDTGS();
            return View(model);
        }

        public ActionResult DSBenhNhanNhanTinNhan()
        {
            HTDTGS model = new HTDTGS();
            return View(model);
        }

        public ActionResult DSBenhNhanPhanHoiTacDungPhu()
        {
            HTDTGS model = new HTDTGS();
            return View(model);
        }

        public ActionResult DSBenhNhanGS()
        {
            HTDTGS model = new HTDTGS();
            return View(model);
        }

        public ActionResult ThongKeSMS()
        {
            HTDTGS model = new HTDTGS();
            return View(model);
        }

        public ActionResult DMThongDiep()
        {
            HTDTGS model = new HTDTGS();
            return View(model);
        }

        public ActionResult DSBenhNhanDoiDiaChi()
        {
            HTDTGS model = new HTDTGS();
            return View(model);
        }

        public ActionResult DSBenhNhanTuChoiNhanTinNhan()
        {
            HTDTGS model = new HTDTGS();
            return View(model);
        }

        [WebMethod]
        public string GetProvinces()
        {
            var model = db.DM_Tinh;
            var provinces = model.ToList();
            if (provinces != null && provinces.Count > 0)
            {
                JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
                return TheSerializer.Serialize(provinces);
            }
            else
            {
                return null;
            }
        }

        [WebMethod]
        public string GetDistricts()
        {
            string provinceId = Request.QueryString["provinceId"];
            List<DM_Huyen_Base> districts = (from d in db.DM_Huyen
                                             where d.MA_TINH.Equals(provinceId)
                                             select new DM_Huyen_Base { MA_HUYEN = d.MA_HUYEN, TEN_HUYEN = d.TEN_HUYEN }).ToList();
            if (districts != null && districts.Count > 0)
            {
                JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
                return TheSerializer.Serialize(districts);
            }
            else
            {
                return null;
            }
        }

        [WebMethod]
        public string GetCommunes()
        {
            string districtId = Request.QueryString["districtId"];
            List<DM_Xa> communes = (from c in db.DM_Xa
                                    where c.MA_HUYEN.Equals(districtId)
                                    select c).ToList();
            if (communes != null && communes.Count > 0)
            {
                JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
                return TheSerializer.Serialize(communes);
            }
            else
            {
                return null;
            }
        }

        [WebMethod]
        public string GetTherapies()
        {
            var model = db.DM_PhacDoDT;
            var therapies = model.ToList();
            if (therapies != null && therapies.Count > 0)
            {
                JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
                return TheSerializer.Serialize(therapies);
            }
            else
            {
                return null;
            }
        }

        [WebMethod]
        public string GetLoaiBenhs()
        {
            var model = db.DM_PhanLoaiBenh;
            var loaibenhs = model.ToList();
            if (loaibenhs != null && loaibenhs.Count > 0)
            {
                JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
                return TheSerializer.Serialize(loaibenhs);
            }
            else
            {
                return null;
            }
        }

        [WebMethod]
        public string GetDsBenhNhanXetNghiem()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JObject req = JObject.Parse(input);
            BenhNhanGrid grid = GetGridBenhNhanXetNghiem(req, true);
            if (grid != null)
            {
                JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
                return TheSerializer.Serialize(grid);
            }
            else
            {
                return null;
            }
        }

        [WebMethod]
        public string GetBenhNhanTuChoiNhanTinNhan()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JObject req = JObject.Parse(input);
            BenhNhanGrid grid = GetGridBenhNhanTuChoiNhanTinNhan(req, true);
            if (grid != null)
            {
                JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
                return TheSerializer.Serialize(grid);
            }
            else
            {
                return null;
            }
        }

        private BenhNhanGrid GetGridBenhNhanTuChoiNhanTinNhan(JObject req, bool isPaging)
        {
            string provinceId = (string)req["provinceId"];
            string districtId = (string)req["districtId"];
            string communeId = (string)req["communeId"];
            int pageSize = (int)req["pageSize"];
            int skip = (int)req["skip"];
            string mobile = (string)req["mobile"];
            string sql = "SELECT count(bn.ID_BenhNhan) FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                    + " WHERE sdt.ID_BENHNHAN=bn.ID_BenhNhan ";
            if (!communeId.Equals("0"))
            {
                sql += " AND bn.MA_XA=@PMAXA ";
            }
            else if (!districtId.Equals("0"))
            {
                sql += " AND bn.MA_HUYEN=@PMAHUYEN ";
            }
            else if (!provinceId.Equals("0"))
            {
                sql += " AND bn.MA_TINH=@PMATINH ";
            }
            sql += " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL";
            if (mobile.Length > 0)
            {
                sql += " AND (bn.Sodienthoai like @PMobile OR bn.HoTen like @PHoTen) ";
            }                
            
            List<SqlParameter> parameterList = new List<SqlParameter>();
            if (!communeId.Equals("0"))
            {
                parameterList.Add(new SqlParameter("@PMAXA", communeId));
            }
            else if (!districtId.Equals("0"))
            {
                parameterList.Add(new SqlParameter("@PMAHUYEN", districtId));
            }
            else if (!provinceId.Equals("0"))
            {
                parameterList.Add(new SqlParameter("@PMATINH", provinceId));
            }
            if (mobile.Length > 0)
            {
                mobile = "%" + mobile + "%";
                parameterList.Add(new SqlParameter("@PMobile", mobile));
                parameterList.Add(new SqlParameter("@PHoTen", mobile));
            }
            SqlParameter[] parameters = parameterList.ToArray();
            int total = db.Database.SqlQuery<int>(sql, parameters).Single();

            string sql3 = "SELECT count(DISTINCT(tc.ID_BenhNhan)) "
                    + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn, dbo.HTDT_BenhNhanTuChoiNhanTin AS tc "
                    + " WHERE sdt.ID_BENHNHAN=bn.ID_BenhNhan AND tc.ID_BenhNhan=bn.ID_BenhNhan AND tc.TuChoiNhanTinNhanUT=1 ";
            if (!communeId.Equals("0"))
            {
                sql3 += " AND bn.MA_XA=@PMAXA ";
            }
            else if (!districtId.Equals("0"))
            {
                sql3 += " AND bn.MA_HUYEN=@PMAHUYEN ";
            }
            else if (!provinceId.Equals("0"))
            {
                sql3 += " AND bn.MA_TINH=@PMATINH ";
            }
            sql3 += " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL ";
            if (mobile.Length > 0)
            {
                sql3 += " AND (bn.Sodienthoai like @PMobile OR bn.HoTen like @PHoTen) ";
            }
            List<SqlParameter> parameterList3 = new List<SqlParameter>();
            if (!communeId.Equals("0"))
            {
                parameterList3.Add(new SqlParameter("@PMAXA", communeId));
            }
            else if (!districtId.Equals("0"))
            {
                parameterList3.Add(new SqlParameter("@PMAHUYEN", districtId));
            }
            else if (!provinceId.Equals("0"))
            {
                parameterList3.Add(new SqlParameter("@PMATINH", provinceId));
            }
            if (mobile.Length > 0)
            {
                mobile = "%" + mobile + "%";
                parameterList3.Add(new SqlParameter("@PMobile", mobile));
                parameterList3.Add(new SqlParameter("@PHoTen", mobile));
            }
            SqlParameter[] parameters3 = parameterList3.ToArray();
            int totalTuChoiTinNhanUT = db.Database.SqlQuery<int>(sql3, parameters3).Single();

            string sql4 = "SELECT count(DISTINCT(tc.ID_BenhNhan)) "
                    + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn, dbo.HTDT_BenhNhanTuChoiNhanTin AS tc "
                    + " WHERE sdt.ID_BENHNHAN=bn.ID_BenhNhan AND tc.ID_BenhNhan=bn.ID_BenhNhan AND tc.TuChoiNhanTinNhanXN=1 ";
            if (!communeId.Equals("0"))
            {
                sql4 += " AND bn.MA_XA=@PMAXA ";
            }
            else if (!districtId.Equals("0"))
            {
                sql4 += " AND bn.MA_HUYEN=@PMAHUYEN ";
            }
            else if (!provinceId.Equals("0"))
            {
                sql4 += " AND bn.MA_TINH=@PMATINH ";
            }
            sql4 += " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL ";
            if (mobile.Length > 0)
            {
                sql4 += " AND (bn.Sodienthoai like @PMobile OR bn.HoTen like @PHoTen) ";
            }
            List<SqlParameter> parameterList4 = new List<SqlParameter>();
            if (!communeId.Equals("0"))
            {
                parameterList4.Add(new SqlParameter("@PMAXA", communeId));
            }
            else if (!districtId.Equals("0"))
            {
                parameterList4.Add(new SqlParameter("@PMAHUYEN", districtId));
            }
            else if (!provinceId.Equals("0"))
            {
                parameterList4.Add(new SqlParameter("@PMATINH", provinceId));
            }
            if (mobile.Length > 0)
            {
                mobile = "%" + mobile + "%";
                parameterList4.Add(new SqlParameter("@PMobile", mobile));
                parameterList4.Add(new SqlParameter("@PHoTen", mobile));
            }
            SqlParameter[] parameters4 = parameterList4.ToArray();
            int totalTuChoiTinNhanXN = db.Database.SqlQuery<int>(sql4, parameters4).Single();

            string sql5 = "SELECT count(DISTINCT(tc.ID_BenhNhan)) "
                    + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn, dbo.HTDT_BenhNhanTuChoiNhanTin AS tc "
                    + " WHERE sdt.ID_BENHNHAN=bn.ID_BenhNhan AND tc.ID_BenhNhan=bn.ID_BenhNhan AND tc.TuChoiNhanTinNhanTT=1 ";
            if (!communeId.Equals("0"))
            {
                sql5 += " AND bn.MA_XA=@PMAXA ";
            }
            else if (!districtId.Equals("0"))
            {
                sql5 += " AND bn.MA_HUYEN=@PMAHUYEN ";
            }
            else if (!provinceId.Equals("0"))
            {
                sql5 += " AND bn.MA_TINH=@PMATINH ";
            }
            sql5 += " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL ";
            if (mobile.Length > 0)
            {
                sql5 += " AND (bn.Sodienthoai like @PMobile OR bn.HoTen like @PHoTen) ";
            }
            List<SqlParameter> parameterList5 = new List<SqlParameter>();
            if (!communeId.Equals("0"))
            {
                parameterList5.Add(new SqlParameter("@PMAXA", communeId));
            }
            else if (!districtId.Equals("0"))
            {
                parameterList5.Add(new SqlParameter("@PMAHUYEN", districtId));
            }
            else if (!provinceId.Equals("0"))
            {
                parameterList5.Add(new SqlParameter("@PMATINH", provinceId));
            }
            if (mobile.Length > 0)
            {
                mobile = "%" + mobile + "%";
                parameterList5.Add(new SqlParameter("@PMobile", mobile));
                parameterList5.Add(new SqlParameter("@PHoTen", mobile));
            }
            SqlParameter[] parameters5 = parameterList5.ToArray();
            int totalTuChoiTinNhanTruyenThong = db.Database.SqlQuery<int>(sql5, parameters5).Single();

            string sql2 = "SELECT bn.ID_BenhNhan AS ID_BenhNhan, sdt.ID_SoDieuTri AS ID_SoDieuTri,bn.HoTen AS HoTen, bn.Tuoi AS Tuoi, "
                + " xa.TEN_XA AS TEN_XA, bn.Gioitinh AS Gioitinh, bn.Diachi AS Diachi, bn.Sodienthoai AS Sodienthoai, "
                + " sdt.NgayDT AS NgayDT, sdt.ID_PhacdoDT AS ID_PhacDoDT, sdt.ID_PhanLoaiBenh AS ID_PhanLoaiBenh, "
                + " (CASE WHEN tc.ID IS NULL THEN 0 ELSE tc.ID END) AS TuChoiTinNhanID, "
                + " tc.TuChoiNhanTinNhanUT AS TuChoiNhanTinNhanUT, tc.NgayTuChoiTinNhanUT AS NgayTuChoiTinNhanUT, tc.LyDoTuChoiTinNhanUT AS LyDoTuChoiTinNhanUT, "
                + " tc.TuChoiNhanTinNhanXN AS TuChoiNhanTinNhanXN, tc.NgayTuChoiTinNhanXN AS NgayTuChoiTinNhanXN, tc.LyDoTuChoiTinNhanXN AS LyDoTuChoiTinNhanXN, "
                + " tc.TuChoiNhanTinNhanTT AS TuChoiNhanTinNhanTT, tc.NgayTuChoiTinNhanTT AS NgayTuChoiTinNhanTT, tc.LyDoTuChoiTinNhanTT AS LyDoTuChoiTinNhanTT "
                + " FROM dbo.SO_SoDieuTri sdt, dbo.DM_Xa xa, dbo.SO_BenhNhan bn "
                + " LEFT JOIN dbo.HTDT_BenhNhanTuChoiNhanTin tc ON tc.ID_BenhNhan=bn.ID_BenhNhan "
                + " WHERE sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_XA=xa.MA_XA ";
            if (!communeId.Equals("0"))
            {
                sql2 += " AND bn.MA_XA=@PMAXA ";
            }
            else if (!districtId.Equals("0"))
            {
                sql2 += " AND bn.MA_HUYEN=@PMAHUYEN ";
            }
            else if (!provinceId.Equals("0"))
            {
                sql2 += " AND bn.MA_TINH=@PMATINH ";
            }
            if (mobile.Length > 0)
            {
                sql2 += " AND (bn.Sodienthoai like @PMobile OR bn.HoTen like @PHoTen) ";
            }
            sql2 += " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL "
                 + "ORDER BY tc.TuChoiNhanTinNhanUT DESC, tc.TuChoiNhanTinNhanXN DESC, tc.TuChoiNhanTinNhanTT DESC ";           
           
            List<SqlParameter> parameterList2 = new List<SqlParameter>();
            if (!communeId.Equals("0"))
            {
                parameterList2.Add(new SqlParameter("@PMAXA", communeId));
            }
            else if (!districtId.Equals("0"))
            {
                parameterList2.Add(new SqlParameter("@PMAHUYEN", districtId));
            }
            else if (!provinceId.Equals("0"))
            {
                parameterList2.Add(new SqlParameter("@PMATINH", provinceId));
            }
            if (mobile.Length > 0)
            {
                mobile = "%" + mobile + "%";
                parameterList2.Add(new SqlParameter("@PMobile", mobile));
                parameterList2.Add(new SqlParameter("@PHoTen", mobile));
            }
            SqlParameter[] parameters2 = parameterList2.ToArray();

            List<BenhNhanVO> patients = new List<BenhNhanVO>();
            if (isPaging)
            {
                patients = db.Database.SqlQuery<BenhNhanVO>(sql2, parameters2).Skip(skip).Take(pageSize).ToList();
            }
            else
            {
                patients = db.Database.SqlQuery<BenhNhanVO>(sql2, parameters2).ToList();
            }
            BenhNhanGrid grid = new BenhNhanGrid();
            if (patients != null && patients.Count > 0)
            {
                grid.total = total;
                grid.patients = patients;
            }
            else
            {
                grid.total = 0;
                grid.patients = null;
            }
            grid.totalTuChoiNhanTinNhanUT = totalTuChoiTinNhanUT;
            grid.totalTuChoiNhanTinNhanXN = totalTuChoiTinNhanXN;
            grid.totalTuChoiNhanTinNhanTT = totalTuChoiTinNhanTruyenThong;
            return grid;
        }

        private BenhNhanGrid GetGridBenhNhanXetNghiem(JObject req, bool isPaging)
        {
            string provinceId = (string)req["provinceId"];
            string districtId = (string)req["districtId"];
            string communeId = (string)req["communeId"];
            int pageSize = (int)req["pageSize"];
            int skip = (int)req["skip"];
            short statusId = (short)req["statusId"];
            string strfromDate = (string)req["fromDate"];
            string strtoDate = (string)req["toDate"];
            DateTime fromDate = strfromDate.Length > 0 ? DateTime.ParseExact(strfromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime toDate = strtoDate.Length > 0 ? DateTime.ParseExact(strtoDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : DateTime.Now;

            string sql = "SELECT count(xn.ID) FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn, dbo.HTDT_LichHenXN xn "
                    + " WHERE sdt.ID_BENHNHAN=bn.ID_BenhNhan AND xn.ID_SoDieuTri=sdt.ID_SoDieuTri ";
            if (!communeId.Equals("0")) {
                sql += " AND bn.MA_XA=@PMAXA ";
            } else if (!districtId.Equals("0")) {
                sql += " AND bn.MA_HUYEN=@PMAHUYEN ";
            } else if (!provinceId.Equals("0")) {
                sql += " AND bn.MA_TINH=@PMATINH ";
            }

            sql += " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL ";
            if (strfromDate.Length > 0)
                sql += " AND convert(date, xn.NgayXN) >= convert(date,@fromDate) ";
            if (strtoDate.Length > 0)
                sql += " AND convert(date, xn.NgayXN) <= convert(date,@toDate) ";
            if (statusId == 0)
            {
                sql += " AND (xn.DaXN = 0 OR xn.DaXN = 2) ";
            }
            else if (statusId == 1)
            {
                sql += " AND xn.DaXN = 2 ";
            }
            else
            {
                sql += " AND xn.DaXN = 1";
            }
            List<SqlParameter> parameterList = new List<SqlParameter>();
            if (!communeId.Equals("0"))
            {
                parameterList.Add(new SqlParameter("@PMAXA", communeId));
            }
            else if (!districtId.Equals("0"))
            {
                parameterList.Add(new SqlParameter("@PMAHUYEN", districtId));
            }
            else if (!provinceId.Equals("0"))
            {
                parameterList.Add(new SqlParameter("@PMATINH", provinceId));
            }            
            parameterList.Add(new SqlParameter("@fromDate", fromDate));
            parameterList.Add(new SqlParameter("@toDate", toDate));
            SqlParameter[] parameters = parameterList.ToArray();
            int total = db.Database.SqlQuery<int>(sql, parameters).Single();

            string sql2 = "SELECT xn.ID AS ID, bn.ID_BenhNhan AS ID_BenhNhan, bn.HoTen AS HoTen, bn.Tuoi AS Tuoi, "
                + " bn.Gioitinh AS Gioitinh, bn.Diachi AS Diachi, bn.Sodienthoai AS Sodienthoai, sdt.NgayDT AS NgayDT, "
                + " sdt.ID_PhacdoDT AS ID_PhacDoDT, sdt.ID_PhanLoaiBenh AS ID_PhanLoaiBenh, xn.LanXN AS LanXN, xn.NgayXN AS NgayXN, "
                + " xn.DaXN AS DaXN, xa.TEN_XA AS TEN_XA, DATEDIFF(day,xn.NgayXN,getdate()) AS SoNgayTreHen "
                + " FROM dbo.SO_BenhNhan bn, dbo.SO_SoDieuTri sdt, dbo.HTDT_LichHenXN xn, dbo.DM_Xa xa "
                + " WHERE sdt.ID_BENHNHAN=bn.ID_BenhNhan AND xn.ID_SoDieuTri=sdt.ID_SoDieuTri AND bn.MA_XA=xa.MA_XA ";
            if (!communeId.Equals("0"))
            {
                sql2 += " AND bn.MA_XA=@PMAXA ";
            }
            else if (!districtId.Equals("0"))
            {
                sql2 += " AND bn.MA_HUYEN=@PMAHUYEN ";
            }
            else if (!provinceId.Equals("0"))
            {
                sql2 += " AND bn.MA_TINH=@PMATINH ";
            }
            sql2 += " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL ";
            if (strfromDate.Length > 0)
                sql2 += " AND convert(date, xn.NgayXN) >= convert(date,@fromDate) ";
            if (strtoDate.Length > 0)
                sql2 += " AND convert(date, xn.NgayXN) <= convert(date,@toDate) ";
            if (statusId == 0)
            {
                sql2 += " AND (xn.DaXN = 0 OR xn.DaXN = 2) ";
            }
            else if (statusId == 1)
            {
                sql2 += " AND xn.DaXN = 2 ";
            }
            else
            {
                sql2 += " AND xn.DaXN = 1";
            }
            //sql2 += " GROUP BY bn.ID_BenhNhan,bn.HoTen,bn.Gioitinh,bn.Diachi,bn.Sodienthoai,sdt.ID_SoDieuTri,sdt.NgayDT,sdt.ID_PhacdoDT,sdt.ID_PhanLoaiBenh ";
            List<SqlParameter> parameterList2 = new List<SqlParameter>();
            if (!communeId.Equals("0"))
            {
                parameterList2.Add(new SqlParameter("@PMAXA", communeId));
            }
            else if (!districtId.Equals("0"))
            {
                parameterList2.Add(new SqlParameter("@PMAHUYEN", districtId));
            }
            else if (!provinceId.Equals("0"))
            {
                parameterList2.Add(new SqlParameter("@PMATINH", provinceId));
            } 
            parameterList2.Add(new SqlParameter("@fromDate", fromDate));
            parameterList2.Add(new SqlParameter("@toDate", toDate));
            SqlParameter[] parameters2 = parameterList2.ToArray();

            List<BenhNhanVO> patients = new List<BenhNhanVO>();
            if (isPaging)
            {
                patients = db.Database.SqlQuery<BenhNhanVO>(sql2, parameters2).Skip(skip).Take(pageSize).ToList();
            }
            else
            {
                patients = db.Database.SqlQuery<BenhNhanVO>(sql2, parameters2).ToList();
            }
            BenhNhanGrid grid = new BenhNhanGrid();
            if (patients != null && patients.Count > 0)
            {
                grid.total = total;
                grid.patients = patients;
            }
            else
            {
                grid.total = 0;
                grid.patients = null;
            }
            return grid;
        }

        [WebMethod]
        public string SaveBenhNhanXN()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JArray req = JArray.Parse(input);

            for (int i = 0; i < req.Count; i++)
            {
                JObject due = (JObject)req[i];
                string sql = "UPDATE dbo.HTDT_LichHenXN SET NgayXN=@P0,DaXN=@P1 WHERE ID=@P2";
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@P0", (string)due["NgayXN"]));
                parameterList.Add(new SqlParameter("@P1", (short)due["DaXN"]));
                parameterList.Add(new SqlParameter("@P2", (long)due["ID"]));
                SqlParameter[] parameters = parameterList.ToArray();
                int result = db.Database.ExecuteSqlCommand(sql, parameters);
            }

            return "";
        }

        [WebMethod]
        public int SaveBenhNhanTuChoiNhanTinNhan()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JObject req = JObject.Parse(input);

            short TuChoiNhanTinNhanUT = (short)req["TuChoiNhanTinNhanUT"];
            string LyDoTuChoiTinNhanUT = (string)req["LyDoTuChoiTinNhanUT"];
            string NgayTuChoiTinNhanUT = (string)req["NgayTuChoiTinNhanUT"];
            short TuChoiNhanTinNhanXN = (short)req["TuChoiNhanTinNhanXN"];
            string LyDoTuChoiTinNhanXN = (string)req["LyDoTuChoiTinNhanXN"];
            string NgayTuChoiTinNhanXN = (string)req["NgayTuChoiTinNhanXN"];
            short TuChoiNhanTinNhanTT = (short)req["TuChoiNhanTinNhanTT"];
            string LyDoTuChoiTinNhanTT = (string)req["LyDoTuChoiTinNhanTT"];
            string NgayTuChoiTinNhanTT = (string)req["NgayTuChoiTinNhanTT"];
            var tmp = req["TuChoiTinNhanID"]; long TuChoiTinNhanID = 0;
            if (tmp != null)
                TuChoiTinNhanID = (long)tmp;
            string sql = "";
            List<SqlParameter> parameterList = new List<SqlParameter>();
            if (TuChoiTinNhanID > 0)
            {
                sql = "UPDATE dbo.HTDT_BenhNhanTuChoiNhanTin SET TuChoiNhanTinNhanUT=@P1,NgayTuChoiTinNhanUT=@P2,LyDoTuChoiTinNhanUT=@P3,"
                + " TuChoiNhanTinNhanXN=@P4,NgayTuChoiTinNhanXN=@P5,LyDoTuChoiTinNhanXN=@P6,"
                + " TuChoiNhanTinNhanTT=@P7,NgayTuChoiTinNhanTT=@P8,LyDoTuChoiTinNhanTT=@P9 "
                + " WHERE ID=@P10";
                parameterList.Add(new SqlParameter("@P10", TuChoiTinNhanID));
            }
            else
            {
                sql = "INSERT INTO dbo.HTDT_BenhNhanTuChoiNhanTin (ID_BenhNhan,TuChoiNhanTinNhanUT,NgayTuChoiTinNhanUT,LyDoTuChoiTinNhanUT, "
                + " TuChoiNhanTinNhanXN,NgayTuChoiTinNhanXN,LyDoTuChoiTinNhanXN, "
                + " TuChoiNhanTinNhanTT,NgayTuChoiTinNhanTT,LyDoTuChoiTinNhanTT)"
                + " VALUES (@P0,@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8,@P9)";
                parameterList.Add(new SqlParameter("@P0", (long)req["ID_BenhNhan"]));
            }

            parameterList.Add(new SqlParameter("@P1", TuChoiNhanTinNhanUT));
            parameterList.Add(new SqlParameter("@P2", NgayTuChoiTinNhanUT.Length > 0 ? DateTime.ParseExact(NgayTuChoiTinNhanUT, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : DateTime.Now));
            parameterList.Add(new SqlParameter("@P3", LyDoTuChoiTinNhanUT));            
            parameterList.Add(new SqlParameter("@P4", TuChoiNhanTinNhanXN));
            parameterList.Add(new SqlParameter("@P5", NgayTuChoiTinNhanXN.Length > 0 ? DateTime.ParseExact(NgayTuChoiTinNhanXN, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : DateTime.Now));
            parameterList.Add(new SqlParameter("@P6", LyDoTuChoiTinNhanXN));            
            parameterList.Add(new SqlParameter("@P7", TuChoiNhanTinNhanTT));
            parameterList.Add(new SqlParameter("@P8", NgayTuChoiTinNhanTT.Length > 0 ? DateTime.ParseExact(NgayTuChoiTinNhanTT, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : DateTime.Now));
            parameterList.Add(new SqlParameter("@P9", LyDoTuChoiTinNhanTT));
            SqlParameter[] parameters = parameterList.ToArray();
            return db.Database.ExecuteSqlCommand(sql, parameters);

        }

        
        [WebMethod]
        public string XuatExcelBenhNhanXN()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JObject req = JObject.Parse(input);
            BenhNhanGrid grid = GetGridBenhNhanXetNghiem(req, false);
            List<BenhNhanVO> patients = grid.patients;

            FileStream fs = new FileStream(Server.MapPath("~") + "/Content/HTDT_TemplateFiles/ListPatientsDue_Template.xls", FileMode.Open, FileAccess.Read);
            HSSFWorkbook wb = new HSSFWorkbook(fs, true);
            HSSFSheet s = (HSSFSheet)wb.GetSheetAt(0);
            HSSFRow r = null;
            HSSFCell c = null;
            ICreationHelper ch = wb.GetCreationHelper();
            HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
            cs.BorderBottom = BorderStyle.Thin;
            cs.BorderTop = BorderStyle.Thin;
            cs.BorderRight = BorderStyle.Thin;
            cs.BorderLeft = BorderStyle.Thin;
            cs.WrapText = true;
            cs.VerticalAlignment = VerticalAlignment.Top;

            HSSFCellStyle csDate = (HSSFCellStyle)wb.CreateCellStyle();
            csDate.BorderBottom = BorderStyle.Thin;
            csDate.BorderTop = BorderStyle.Thin;
            csDate.BorderRight = BorderStyle.Thin;
            csDate.BorderLeft = BorderStyle.Thin;
            csDate.Alignment = HorizontalAlignment.Center;
            csDate.VerticalAlignment = VerticalAlignment.Top;
            csDate.DataFormat = ch.CreateDataFormat().GetFormat("dd/MM/yyyy");

            HSSFCellStyle cs1 = (HSSFCellStyle)wb.CreateCellStyle();
            cs1.BorderBottom = BorderStyle.Thin;
            cs1.BorderTop = BorderStyle.Thin;
            cs1.BorderRight = BorderStyle.Thin;
            cs1.BorderLeft = BorderStyle.Thin;
            cs1.VerticalAlignment = VerticalAlignment.Top;
            cs1.Alignment = HorizontalAlignment.Center;

            var phacdos = db.DM_PhacDoDT.ToList();
            var loaibenhs = db.DM_PhanLoaiBenh.ToList();
            string provinceId = (string)req["provinceId"];
            short statusId = (short)req["statusId"];
            string strfromDate = (string)req["fromDate"];
            string strtoDate = (string)req["toDate"];

            DateTime fromDate = DateTime.ParseExact(strfromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(strtoDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            strfromDate = fromDate.ToString("dd/MM/yyyy");
            strtoDate = toDate.ToString("dd/MM/yyyy");
            string duration = "";
            if (strfromDate == strtoDate)
            {
                duration = strfromDate;
            }
            else
            {
                duration = strfromDate + " - " + strtoDate;
            }
            string filepath = "/Content/HTDT_ReportFiles/ThongKeBenhNhanTaiKham_" + provinceId + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xls";

            using (FileStream ms = new FileStream(Server.MapPath("~") + filepath, FileMode.Create, FileAccess.Write))
            {
                r = (HSSFRow)s.GetRow(4);
                c = (HSSFCell)r.GetCell(1);

                c = (HSSFCell)r.GetCell(7);
                c.SetCellValue(duration);

                for (int rownum = 11; rownum < patients.Count + 11; rownum++)
                {
                    r = (HSSFRow)s.CreateRow(rownum);

                    c = (HSSFCell)r.CreateCell(1);
                    c.CellStyle = cs;
                    c.SetCellValue(rownum - 10);

                    c = (HSSFCell)r.CreateCell(2);
                    c.CellStyle = cs;
                    c.SetCellValue(patients[rownum - 11].HoTen);

                    c = (HSSFCell)r.CreateCell(3, CellType.Numeric);
                    c.CellStyle = cs1;
                    c.SetCellValue(patients[rownum - 11].Tuoi);

                    c = (HSSFCell)r.CreateCell(4);
                    c.CellStyle = cs1;
                    c.SetCellValue(patients[rownum - 11].Gioitinh ? "Nam" : "Nữ");

                    c = (HSSFCell)r.CreateCell(5);
                    c.CellStyle = cs;
                    c.SetCellValue(patients[rownum - 11].Sodienthoai);

                    c = (HSSFCell)r.CreateCell(6);
                    c.CellStyle = cs;
                    c.SetCellValue(patients[rownum - 11].TEN_XA);

                    c = (HSSFCell)r.CreateCell(7);
                    c.CellStyle = cs;
                    c.SetCellValue(patients[rownum - 11].Diachi);

                    c = (HSSFCell)r.CreateCell(8);
                    c.CellStyle = csDate;
                    c.SetCellValue(patients[rownum - 11].NgayDT);

                    c = (HSSFCell)r.CreateCell(9);
                    c.CellStyle = cs;
                    c.SetCellValue(phacdos[patients[rownum - 11].ID_PhacDoDT - 1].Ten_PhacdoDT);

                    c = (HSSFCell)r.CreateCell(10);
                    c.CellStyle = cs;
                    c.SetCellValue(loaibenhs[patients[rownum - 11].ID_PhanLoaiBenh - 1].Ten_PhanLoaiBenh);

                    c = (HSSFCell)r.CreateCell(11);
                    c.CellStyle = cs1;
                    c.SetCellValue(patients[rownum - 11].LanXN);

                    c = (HSSFCell)r.CreateCell(12);
                    c.CellStyle = csDate;
                    c.SetCellValue(patients[rownum - 11].NgayXN);

                    c = (HSSFCell)r.CreateCell(13);
                    c.CellStyle = cs1;
                    c.SetCellValue(patients[rownum - 11].SoNgayTreHen);

                    c = (HSSFCell)r.CreateCell(14);
                    c.CellStyle = cs1;
                    if (statusId == 0)
                    {
                        c.SetCellValue("□");
                    }
                    else
                        c.SetCellValue(patients[rownum - 11].DaXN == 1 ? "Có" : "Không");
                }
                wb.Write(ms);
            }
            JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
            return TheSerializer.Serialize(filepath);
        }

        [WebMethod]
        public string GetBenhNhanNhanTinNhan()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JObject req = JObject.Parse(input);
            BenhNhanGrid grid = GetGridBenhNhanNhanTinNhan(req, true);
            if (grid != null)
            {
                JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
                return TheSerializer.Serialize(grid);
            }
            else
            {
                return null;
            }
        }

        private BenhNhanGrid GetGridBenhNhanNhanTinNhan(JObject req, bool isPaging)
        {
            string provinceId = (string)req["provinceId"];
            string districtId = (string)req["districtId"];
            string communeId = (string)req["communeId"];
            int pageSize = (int)req["pageSize"];
            int skip = (int)req["skip"];
            short statusId = (short)req["statusId"];
            string strfromDate = (string)req["fromDate"];
            string strtoDate = (string)req["toDate"];
            DateTime fromDate = strfromDate.Length > 0 ? DateTime.ParseExact(strfromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime toDate = strtoDate.Length > 0 ? DateTime.ParseExact(strtoDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : DateTime.Now;

            string sql = "SELECT count(sdt.ID_BenhNhan) FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn, dbo.HTDT_BenhNhanNhanTinNhan tn "
                + " WHERE sdt.ID_BENHNHAN=bn.ID_BenhNhan AND tn.ID_SoDieuTri=sdt.ID_SoDieuTri ";
            if (!communeId.Equals("0"))
            {
                sql += " AND bn.MA_XA=@PMAXA ";
            }
            else if (!districtId.Equals("0"))
            {
                sql += " AND bn.MA_HUYEN=@PMAHUYEN ";
            }
            else if (!provinceId.Equals("0"))
            {
                sql += " AND bn.MA_TINH=@PMATINH ";
            }
            sql += " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL ";
            if (strfromDate.Length > 0)
                sql += " AND convert(date, tn.NgayGui) >= convert(date,@fromDate) ";
            if (strtoDate.Length > 0)
                sql += " AND convert(date, tn.NgayGui) <= convert(date,@toDate) ";
            if (statusId == 0)
            {
                sql += " AND sdt.NgayRV IS NULL ";
            }
            else
            {
                sql += " AND sdt.NgayRV IS NOT NULL ";
            }
            List<SqlParameter> parameterList = new List<SqlParameter>();
            if (!communeId.Equals("0"))
            {
                parameterList.Add(new SqlParameter("@PMAXA", communeId));
            }
            else if (!districtId.Equals("0"))
            {
                parameterList.Add(new SqlParameter("@PMAHUYEN", districtId));
            }
            else if (!provinceId.Equals("0"))
            {
                parameterList.Add(new SqlParameter("@PMATINH", provinceId));
            }
            parameterList.Add(new SqlParameter("@fromDate", fromDate));
            parameterList.Add(new SqlParameter("@toDate", toDate));
            SqlParameter[] parameters = parameterList.ToArray();
            int total = db.Database.SqlQuery<int>(sql, parameters).Single();

            string sql2 = "SELECT bn.ID_BenhNhan AS ID_BenhNhan, sdt.ID_SoDieuTri AS ID_SoDieuTri,bn.HoTen AS HoTen, AVG(bn.Tuoi) AS Tuoi, bn.Gioitinh AS Gioitinh, "
                + " bn.Diachi AS Diachi, bn.Sodienthoai AS Sodienthoai, sdt.NgayDT AS NgayDT, sdt.ID_PhacdoDT AS ID_PhacDoDT, xa.TEN_XA AS TEN_XA, "
                + " sdt.ID_PhanLoaiBenh AS ID_PhanLoaiBenh, SUM(CASE WHEN ISNULL(tn.LoaiTinNhan,0)=1 THEN 1 ELSE 0 END) AS SoTinNhanUT, "
                + " SUM(CASE WHEN ISNULL(tn.LoaiTinNhan,0)=3 THEN 1 ELSE 0 END) AS SoTinNhanXN, "
                + " SUM(CASE WHEN ISNULL(tn.LoaiTinNhan,0)=2 THEN 1 ELSE 0 END) AS SoTinNhanGD "
                + " FROM dbo.SO_BenhNhan bn, dbo.SO_SoDieuTri sdt, dbo.HTDT_BenhNhanNhanTinNhan tn, dbo.DM_Xa xa "
                + " WHERE sdt.ID_BENHNHAN=bn.ID_BenhNhan AND tn.ID_SoDieuTri=sdt.ID_SoDieuTri AND bn.MA_XA=xa.MA_XA ";
            if (!communeId.Equals("0"))
            {
                sql2 += " AND bn.MA_XA=@PMAXA ";
            }
            else if (!districtId.Equals("0"))
            {
                sql2 += " AND bn.MA_HUYEN=@PMAHUYEN ";
            }
            else if (!provinceId.Equals("0"))
            {
                sql2 += " AND bn.MA_TINH=@PMATINH ";
            }
            sql2 += " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL ";
            if (strfromDate.Length > 0)
                sql2 += " AND convert(date, tn.NgayGui) >= convert(date,@fromDate) ";
            if (strtoDate.Length > 0)
                sql2 += " AND convert(date, tn.NgayGui) <= convert(date,@toDate) ";

            if (statusId == 0)
            {
                sql2 += " AND sdt.NgayRV IS NULL ";
            }
            else
            {
                sql2 += " AND sdt.NgayRV IS NOT NULL ";
            }
            sql2 += " GROUP BY bn.ID_BenhNhan,bn.HoTen,bn.Gioitinh,bn.Diachi,bn.Sodienthoai,sdt.ID_SoDieuTri,sdt.NgayDT,sdt.ID_PhacdoDT,sdt.ID_PhanLoaiBenh,xa.TEN_XA ";
            List<SqlParameter> parameterList2 = new List<SqlParameter>();
            if (!communeId.Equals("0"))
            {
                parameterList2.Add(new SqlParameter("@PMAXA", communeId));
            }
            else if (!districtId.Equals("0"))
            {
                parameterList2.Add(new SqlParameter("@PMAHUYEN", districtId));
            }
            else if (!provinceId.Equals("0"))
            {
                parameterList2.Add(new SqlParameter("@PMATINH", provinceId));
            }
            parameterList2.Add(new SqlParameter("@fromDate", fromDate));
            parameterList2.Add(new SqlParameter("@toDate", toDate));
            SqlParameter[] parameters2 = parameterList2.ToArray();

            List<BenhNhanVO> patients = new List<BenhNhanVO>();
            if (isPaging)
            {
                patients = db.Database.SqlQuery<BenhNhanVO>(sql2, parameters2).Skip(skip).Take(pageSize).ToList();
            }
            else
            {
                patients = db.Database.SqlQuery<BenhNhanVO>(sql2, parameters2).ToList();
            }

            string sql3 = "SELECT SUM(CASE WHEN ISNULL(tn.LoaiTinNhan,0)=1 THEN 1 ELSE 0 END) AS totalSoTinNhanUT, "
                + " SUM(CASE WHEN ISNULL(tn.LoaiTinNhan,0)=3 THEN 1 ELSE 0 END) AS totalSoTinNhanXN, "
                + " SUM(CASE WHEN ISNULL(tn.LoaiTinNhan,0)=2 THEN 1 ELSE 0 END) AS totalSoTinNhanTruyenThong "
                + " FROM dbo.SO_BenhNhan bn, dbo.SO_SoDieuTri sdt, dbo.HTDT_BenhNhanNhanTinNhan tn "
                + " WHERE sdt.ID_BENHNHAN=bn.ID_BenhNhan AND tn.ID_SoDieuTri=sdt.ID_SoDieuTri ";
            if (!communeId.Equals("0"))
            {
                sql3 += " AND bn.MA_XA=@PMAXA ";
            }
            else if (!districtId.Equals("0"))
            {
                sql3 += " AND bn.MA_HUYEN=@PMAHUYEN ";
            }
            else if (!provinceId.Equals("0"))
            {
                sql3 += " AND bn.MA_TINH=@PMATINH ";
            }
            sql3 += " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL ";
            if (strfromDate.Length > 0)
                sql3 += " AND convert(date, tn.NgayGui) >= convert(date,@fromDate) ";
            if (strtoDate.Length > 0)
                sql3 += " AND convert(date, tn.NgayGui) <= convert(date,@toDate) ";

            if (statusId == 0)
            {
                sql3 += " AND sdt.NgayRV IS NULL ";
            }
            else
            {
                sql3 += " AND sdt.NgayRV IS NOT NULL ";
            }
            
            List<SqlParameter> parameterList3 = new List<SqlParameter>();
            if (!communeId.Equals("0"))
            {
                parameterList3.Add(new SqlParameter("@PMAXA", communeId));
            }
            else if (!districtId.Equals("0"))
            {
                parameterList3.Add(new SqlParameter("@PMAHUYEN", districtId));
            }
            else if (!provinceId.Equals("0"))
            {
                parameterList3.Add(new SqlParameter("@PMATINH", provinceId));
            }
            parameterList3.Add(new SqlParameter("@fromDate", fromDate));
            parameterList3.Add(new SqlParameter("@toDate", toDate));
            SqlParameter[] parameters3 = parameterList3.ToArray();

            BenhNhanGrid grid = new BenhNhanGrid();

            var tmplist = db.Database.SqlQuery<BenhNhanGrid>(sql3, parameters3);
            if (tmplist != null)
            {
                grid = tmplist.FirstOrDefault();
            }
            
            if (patients != null && patients.Count > 0)
            {
                grid.total = total;
                grid.patients = patients;
            }
            else
            {
                grid.total = 0;
                grid.patients = null;
            }
            return grid;
        }

        [WebMethod]
        public string XuatExcelBenhNhanNhanTinNhan()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JObject req = JObject.Parse(input);
            BenhNhanGrid grid = GetGridBenhNhanNhanTinNhan(req, false);
            List<BenhNhanVO> patients = grid.patients;

            FileStream fs = new FileStream(Server.MapPath("~") + "/Content/HTDT_TemplateFiles/ListPatientsRemind_Template.xls", FileMode.Open, FileAccess.Read);
            HSSFWorkbook wb = new HSSFWorkbook(fs, true);
            HSSFSheet s = (HSSFSheet)wb.GetSheetAt(0);
            HSSFRow r = null;
            HSSFCell c = null;
            ICreationHelper ch = wb.GetCreationHelper();
            HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
            cs.BorderBottom = BorderStyle.Thin;
            cs.BorderTop = BorderStyle.Thin;
            cs.BorderRight = BorderStyle.Thin;
            cs.BorderLeft = BorderStyle.Thin;
            cs.WrapText = true;
            cs.VerticalAlignment = VerticalAlignment.Top;

            HSSFCellStyle csDate = (HSSFCellStyle)wb.CreateCellStyle();
            csDate.BorderBottom = BorderStyle.Thin;
            csDate.BorderTop = BorderStyle.Thin;
            csDate.BorderRight = BorderStyle.Thin;
            csDate.BorderLeft = BorderStyle.Thin;
            csDate.Alignment = HorizontalAlignment.Center;
            csDate.VerticalAlignment = VerticalAlignment.Top;
            csDate.DataFormat = ch.CreateDataFormat().GetFormat("dd/MM/yyyy");

            HSSFCellStyle cs1 = (HSSFCellStyle)wb.CreateCellStyle();
            cs1.BorderBottom = BorderStyle.Thin;
            cs1.BorderTop = BorderStyle.Thin;
            cs1.BorderRight = BorderStyle.Thin;
            cs1.BorderLeft = BorderStyle.Thin;
            cs1.VerticalAlignment = VerticalAlignment.Top;
            cs1.Alignment = HorizontalAlignment.Center;

            var phacdos = db.DM_PhacDoDT.ToList();
            var loaibenhs = db.DM_PhanLoaiBenh.ToList();
            string provinceId = (string)req["provinceId"];
            short statusId = (short)req["statusId"];
            string strfromDate = (string)req["fromDate"];
            string strtoDate = (string)req["toDate"];
            string filepath = "/Content/HTDT_ReportFiles/ThongKeBenhNhanNhanTinNhan_" + provinceId + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xls";
            DateTime fromDate = DateTime.ParseExact(strfromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(strtoDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            strfromDate = fromDate.ToString("dd/MM/yyyy");
            strtoDate = toDate.ToString("dd/MM/yyyy");
            string duration = "";
            if (strfromDate == strtoDate)
            {
                duration = strfromDate;
            }
            else
            {
                duration = strfromDate + " - " + strtoDate;
            }            
           
            using (FileStream ms = new FileStream(Server.MapPath("~") + filepath, FileMode.Create, FileAccess.Write))
            {
                r = (HSSFRow)s.GetRow(4);
                c = (HSSFCell)r.GetCell(1);

                c = (HSSFCell)r.GetCell(7);
                c.SetCellValue(duration);

                for (int rownum = 11; rownum < patients.Count + 11; rownum++)
                {
                    r = (HSSFRow)s.CreateRow(rownum);

                    c = (HSSFCell)r.CreateCell(1);
                    c.CellStyle = cs;
                    c.SetCellValue(rownum - 10);

                    c = (HSSFCell)r.CreateCell(2);
                    c.CellStyle = cs;
                    c.SetCellValue(patients[rownum - 11].HoTen);

                    c = (HSSFCell)r.CreateCell(3, CellType.Numeric);
                    c.CellStyle = cs1;
                    c.SetCellValue(patients[rownum - 11].Tuoi);

                    c = (HSSFCell)r.CreateCell(4);
                    c.CellStyle = cs1;
                    c.SetCellValue(patients[rownum - 11].Gioitinh ? "Nam" : "Nữ");

                    c = (HSSFCell)r.CreateCell(5);
                    c.CellStyle = cs;
                    c.SetCellValue(patients[rownum - 11].Sodienthoai);

                    c = (HSSFCell)r.CreateCell(6);
                    c.CellStyle = cs;
                    c.SetCellValue(patients[rownum - 11].TEN_XA);

                    c = (HSSFCell)r.CreateCell(7);
                    c.CellStyle = cs;
                    c.SetCellValue(patients[rownum - 11].Diachi);

                    c = (HSSFCell)r.CreateCell(8);
                    c.CellStyle = csDate;
                    c.SetCellValue(patients[rownum - 11].NgayDT);

                    c = (HSSFCell)r.CreateCell(9);
                    c.CellStyle = cs;
                    int ID_PhacDoDT = patients[rownum - 11].ID_PhacDoDT;
                    c.SetCellValue(ID_PhacDoDT > 0 ? phacdos[ID_PhacDoDT - 1].Ten_PhacdoDT : "");

                    c = (HSSFCell)r.CreateCell(10);
                    c.CellStyle = cs;
                    int ID_PhanLoaiBenh = patients[rownum - 11].ID_PhanLoaiBenh;
                    c.SetCellValue(ID_PhanLoaiBenh > 0 ? loaibenhs[ID_PhanLoaiBenh - 1].Ten_PhanLoaiBenh : "");

                    c = (HSSFCell)r.CreateCell(11);
                    c.CellStyle = cs1;
                    c.SetCellValue(patients[rownum - 11].SoTinNhanUT);

                    c = (HSSFCell)r.CreateCell(12);
                    c.CellStyle = cs1;
                    c.SetCellValue(patients[rownum - 11].SoTinNhanXN);

                    c = (HSSFCell)r.CreateCell(13);
                    c.CellStyle = cs1;
                    c.SetCellValue(patients[rownum - 11].SoTinNhanGD);
                    
                }
                wb.Write(ms);
            }
            JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
            return TheSerializer.Serialize(filepath);
        }

        [WebMethod]
        public string XuatExcelBenhNhanGuiTacDungPhu()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JObject req = JObject.Parse(input);
            List<BenhNhanVO> patients = GetDsTacDungPhu(req);

            FileStream fs = new FileStream(Server.MapPath("~") + "/Content/HTDT_TemplateFiles/ListPatientSymptoms_Template.xls", FileMode.Open, FileAccess.Read);
            HSSFWorkbook wb = new HSSFWorkbook(fs, true);
            HSSFSheet s = (HSSFSheet)wb.GetSheetAt(0);
            HSSFRow r = null;
            HSSFCell c = null;
            ICreationHelper ch = wb.GetCreationHelper();
            HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
            cs.BorderBottom = BorderStyle.Thin;
            cs.BorderTop = BorderStyle.Thin;
            cs.BorderRight = BorderStyle.Thin;
            cs.BorderLeft = BorderStyle.Thin;
            cs.WrapText = true;
            cs.VerticalAlignment = VerticalAlignment.Top;

            HSSFCellStyle csDate = (HSSFCellStyle)wb.CreateCellStyle();
            csDate.BorderBottom = BorderStyle.Thin;
            csDate.BorderTop = BorderStyle.Thin;
            csDate.BorderRight = BorderStyle.Thin;
            csDate.BorderLeft = BorderStyle.Thin;
            csDate.Alignment = HorizontalAlignment.Center;
            csDate.VerticalAlignment = VerticalAlignment.Top;
            csDate.DataFormat = ch.CreateDataFormat().GetFormat("dd/MM/yyyy");

            HSSFCellStyle cs1 = (HSSFCellStyle)wb.CreateCellStyle();
            cs1.BorderBottom = BorderStyle.Thin;
            cs1.BorderTop = BorderStyle.Thin;
            cs1.BorderRight = BorderStyle.Thin;
            cs1.BorderLeft = BorderStyle.Thin;
            cs1.VerticalAlignment = VerticalAlignment.Top;
            cs1.Alignment = HorizontalAlignment.Center;

            var phacdos = db.DM_PhacDoDT.ToList();
            var loaibenhs = db.DM_PhanLoaiBenh.ToList();
            string provinceId = (string)req["provinceId"];
            string strfromDate = (string)req["fromDate"];
            string strtoDate = (string)req["toDate"];
            string filepath = "/Content/HTDT_ReportFiles/ThongKeTrieuChungPhanHoi_" + provinceId + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xls";
            DateTime fromDate = DateTime.ParseExact(strfromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(strtoDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            strfromDate = fromDate.ToString("dd/MM/yyyy");
            strtoDate = toDate.ToString("dd/MM/yyyy");
            string duration = "";
            if (strfromDate == strtoDate)
            {
                duration = strfromDate;
            }
            else
            {
                duration = strfromDate + " - " + strtoDate;
            }
            using (FileStream ms = new FileStream(Server.MapPath("~") + filepath, FileMode.Create, FileAccess.Write))
            {
                r = (HSSFRow)s.GetRow(4);
                c = (HSSFCell)r.GetCell(1);

                c = (HSSFCell)r.GetCell(7);
                c.SetCellValue(duration);

                for (int rownum = 11; rownum < patients.Count + 11; rownum++)
                {
                    r = (HSSFRow)s.CreateRow(rownum);

                    c = (HSSFCell)r.CreateCell(1);
                    c.CellStyle = cs;
                    c.SetCellValue(rownum - 10);

                    c = (HSSFCell)r.CreateCell(2);
                    c.CellStyle = cs;
                    c.SetCellValue(patients[rownum - 11].HoTen);

                    c = (HSSFCell)r.CreateCell(3, CellType.Numeric);
                    c.CellStyle = cs1;
                    c.SetCellValue(patients[rownum - 11].Tuoi);

                    c = (HSSFCell)r.CreateCell(4);
                    c.CellStyle = cs1;
                    c.SetCellValue(patients[rownum - 11].Gioitinh ? "Nam" : "Nữ");

                    c = (HSSFCell)r.CreateCell(5);
                    c.CellStyle = cs;
                    c.SetCellValue(patients[rownum - 11].Sodienthoai);

                    c = (HSSFCell)r.CreateCell(6);
                    c.CellStyle = cs;
                    c.SetCellValue(patients[rownum - 11].TEN_XA);

                    c = (HSSFCell)r.CreateCell(7);
                    c.CellStyle = cs;
                    c.SetCellValue(patients[rownum - 11].Diachi);

                    c = (HSSFCell)r.CreateCell(8);
                    c.CellStyle = csDate;
                    c.SetCellValue(patients[rownum - 11].NgayDT);

                    c = (HSSFCell)r.CreateCell(9);
                    c.CellStyle = cs;
                    int ID_PhacDoDT = patients[rownum - 11].ID_PhacDoDT;
                    c.SetCellValue(ID_PhacDoDT > 0 ? phacdos[ID_PhacDoDT - 1].Ten_PhacdoDT : "");

                    c = (HSSFCell)r.CreateCell(10);
                    c.CellStyle = cs;
                    int ID_PhanLoaiBenh = patients[rownum - 11].ID_PhanLoaiBenh;
                    c.SetCellValue(ID_PhanLoaiBenh > 0 ? loaibenhs[ID_PhanLoaiBenh - 1].Ten_PhanLoaiBenh : "");

                    c = (HSSFCell)r.CreateCell(11);
                    c.CellStyle = csDate;
                    if (patients[rownum - 11].NgayNhanTinTacDungPhu != null)
                    {
                        c.SetCellValue((DateTime)patients[rownum - 11].NgayNhanTinTacDungPhu);
                    }

                    c = (HSSFCell)r.CreateCell(12);
                    c.CellStyle = cs1;
                    c.SetCellValue(patients[rownum - 11].TacDungPhu);

                }
                wb.Write(ms);
            }
            JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
            return TheSerializer.Serialize(filepath);
        }

        [WebMethod]
        public string XuatExcelBenhNhanGS()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JObject req = JObject.Parse(input);
            BenhNhanGSGrid grid = GetGridBenhNhanGS(req, false);
            List<BenhNhanGS> patients = grid.patients;

            FileStream fs = new FileStream(Server.MapPath("~") + "/Content/HTDT_TemplateFiles/ListSupervisionPatients_Template.xls", FileMode.Open, FileAccess.Read);
            HSSFWorkbook wb = new HSSFWorkbook(fs, true);
            HSSFSheet s = (HSSFSheet)wb.GetSheetAt(0);
            HSSFRow r = null;
            HSSFCell c = null;
            ICreationHelper ch = wb.GetCreationHelper();
            HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
            cs.BorderBottom = BorderStyle.Thin;
            cs.BorderTop = BorderStyle.Thin;
            cs.BorderRight = BorderStyle.Thin;
            cs.BorderLeft = BorderStyle.Thin;
            cs.WrapText = true;
            cs.VerticalAlignment = VerticalAlignment.Top;

            HSSFCellStyle csDate = (HSSFCellStyle)wb.CreateCellStyle();
            csDate.BorderBottom = BorderStyle.Thin;
            csDate.BorderTop = BorderStyle.Thin;
            csDate.BorderRight = BorderStyle.Thin;
            csDate.BorderLeft = BorderStyle.Thin;
            csDate.Alignment = HorizontalAlignment.Center;
            csDate.VerticalAlignment = VerticalAlignment.Top;
            csDate.DataFormat = ch.CreateDataFormat().GetFormat("dd/MM/yyyy");

            HSSFCellStyle cs1 = (HSSFCellStyle)wb.CreateCellStyle();
            cs1.BorderBottom = BorderStyle.Thin;
            cs1.BorderTop = BorderStyle.Thin;
            cs1.BorderRight = BorderStyle.Thin;
            cs1.BorderLeft = BorderStyle.Thin;
            cs1.VerticalAlignment = VerticalAlignment.Top;
            cs1.Alignment = HorizontalAlignment.Center;

            var phacdos = db.DM_PhacDoDT.ToList();
            var loaibenhs = db.DM_PhanLoaiBenh.ToList();
            string provinceId = (string)req["provinceId"];
            string strfromDate = (string)req["fromDate"];
            string strtoDate = (string)req["toDate"];
            string filepath = "/Content/HTDT_ReportFiles/DsBenhNhanGiamSat_" + provinceId + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xls";
            DateTime fromDate = DateTime.ParseExact(strfromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(strtoDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            strfromDate = fromDate.ToString("dd/MM/yyyy");
            strtoDate = toDate.ToString("dd/MM/yyyy");
            string duration = "";
            if (strfromDate == strtoDate)
            {
                duration = strfromDate;
            }
            else
            {
                duration = strfromDate + " - " + strtoDate;
            }
            using (FileStream ms = new FileStream(Server.MapPath("~") + filepath, FileMode.Create, FileAccess.Write))
            {
                r = (HSSFRow)s.GetRow(4);
                c = (HSSFCell)r.GetCell(1);

                c = (HSSFCell)r.GetCell(7);
                c.SetCellValue(duration);

                for (int rownum = 11; rownum < patients.Count + 11; rownum++)
                {
                    r = (HSSFRow)s.CreateRow(rownum);

                    c = (HSSFCell)r.CreateCell(1);
                    c.CellStyle = cs;
                    c.SetCellValue(rownum - 10);

                    c = (HSSFCell)r.CreateCell(2);
                    c.CellStyle = cs;
                    c.SetCellValue(patients[rownum - 11].HoTen);

                    c = (HSSFCell)r.CreateCell(3, CellType.Numeric);
                    c.CellStyle = cs1;
                    c.SetCellValue(patients[rownum - 11].Tuoi);

                    c = (HSSFCell)r.CreateCell(4);
                    c.CellStyle = cs1;
                    c.SetCellValue(patients[rownum - 11].Gioitinh ? "Nam" : "Nữ");

                    c = (HSSFCell)r.CreateCell(5);
                    c.CellStyle = cs;
                    c.SetCellValue(patients[rownum - 11].Sodienthoai);

                    c = (HSSFCell)r.CreateCell(6);
                    c.CellStyle = cs;
                    c.SetCellValue(patients[rownum - 11].TEN_XA);

                    c = (HSSFCell)r.CreateCell(7);
                    c.CellStyle = cs;
                    c.SetCellValue(patients[rownum - 11].Diachi);

                    c = (HSSFCell)r.CreateCell(8);
                    c.CellStyle = csDate;
                    c.SetCellValue(patients[rownum - 11].NgayDT);

                    c = (HSSFCell)r.CreateCell(9);
                    c.CellStyle = cs;
                    int ID_PhacDoDT = patients[rownum - 11].ID_PhacDoDT;
                    c.SetCellValue(ID_PhacDoDT > 0 ? phacdos[ID_PhacDoDT - 1].Ten_PhacdoDT : "");

                    c = (HSSFCell)r.CreateCell(10);
                    c.CellStyle = cs;
                    int ID_PhanLoaiBenh = patients[rownum - 11].ID_PhanLoaiBenh;
                    c.SetCellValue(ID_PhanLoaiBenh > 0 ? loaibenhs[ID_PhanLoaiBenh - 1].Ten_PhanLoaiBenh : "");

                    c = (HSSFCell)r.CreateCell(11);
                    c.CellStyle = cs1;
                    c.SetCellValue((int)patients[rownum - 11].SoLanGiamSat);

                }
                wb.Write(ms);
            }
            JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
            return TheSerializer.Serialize(filepath);
        }

        [WebMethod]
        public string XuatBaoCaoGiamSatCuaBenhNhan()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JObject req = JObject.Parse(input);
            long benhNhanId = (long)req["benhNhanId"];
            var model = db.HTDT_BaoCaoGiamSat;
            List<HTDT_BaoCaoGiamSat> giamsats = model.ToList().Where(a => a.ID_BenhNhan == benhNhanId).ToList();

            if (giamsats != null && giamsats.Count() > 0)
            {
                SO_BenhNhan benhNhan = db.SO_BenhNhan.Where(p => p.ID_BenhNhan == benhNhanId).FirstOrDefault();
                SO_SoDieuTri dieuTri = db.SO_SoDieuTri.Where(p => p.ID_BENHNHAN == benhNhanId).FirstOrDefault();
                FileStream fs = new FileStream(Server.MapPath("~") + "/Content/HTDT_TemplateFiles/PatientSupervision_Template.xls", FileMode.Open, FileAccess.Read);
                HSSFWorkbook wb = new HSSFWorkbook(fs, true);
                HSSFSheet s = (HSSFSheet)wb.GetSheetAt(0);
                HSSFRow r = null;
                HSSFCell c = null;
                ICreationHelper ch = wb.GetCreationHelper();
                HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
                cs.BorderBottom = BorderStyle.Thin;
                cs.BorderTop = BorderStyle.Thin;
                cs.BorderRight = BorderStyle.Thin;
                cs.BorderLeft = BorderStyle.Thin;
                cs.WrapText = true;
                cs.VerticalAlignment = VerticalAlignment.Top;

                HSSFCellStyle csDate = (HSSFCellStyle)wb.CreateCellStyle();
                csDate.BorderBottom = BorderStyle.Thin;
                csDate.BorderTop = BorderStyle.Thin;
                csDate.BorderRight = BorderStyle.Thin;
                csDate.BorderLeft = BorderStyle.Thin;
                csDate.Alignment = HorizontalAlignment.Center;
                csDate.VerticalAlignment = VerticalAlignment.Top;
                csDate.DataFormat = ch.CreateDataFormat().GetFormat("dd/MM/yyyy");

                HSSFCellStyle cs1 = (HSSFCellStyle)wb.CreateCellStyle();
                cs1.BorderBottom = BorderStyle.Thin;
                cs1.BorderTop = BorderStyle.Thin;
                cs1.BorderRight = BorderStyle.Thin;
                cs1.BorderLeft = BorderStyle.Thin;
                cs1.VerticalAlignment = VerticalAlignment.Top;
                cs1.Alignment = HorizontalAlignment.Center;

                var phacdos = db.DM_PhacDoDT.ToList();
                var loaibenhs = db.DM_PhanLoaiBenh.ToList();

                string filepath = "/Content/HTDT_ReportFiles/ThongTinGiamSatDieuTriBN_" + benhNhanId + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xls";

                using (FileStream ms = new FileStream(Server.MapPath("~") + filepath, FileMode.Create, FileAccess.Write))
                {
                    r = (HSSFRow)s.GetRow(5);
                    c = (HSSFCell)r.GetCell(1);
                    c.SetCellValue("Họ tên bệnh nhân: " + benhNhan.HoTen);

                    c = (HSSFCell)r.GetCell(10);
                    c.SetCellValue("Tuổi: " + benhNhan.Tuoi);

                    c = (HSSFCell)r.GetCell(11);
                    String strGender = (bool)benhNhan.Gioitinh ? "Nam" : "Nữ";
                    c.SetCellValue("Giới: " + strGender);

                    r = (HSSFRow)s.GetRow(6);
                    c = (HSSFCell)r.GetCell(1);
                    c.SetCellValue("Địa chỉ: " + benhNhan.Diachi);

                    r = (HSSFRow)s.GetRow(7);
                    c = (HSSFCell)r.GetCell(1);
                    c.SetCellValue("Thể bệnh: " + loaibenhs[(int)dieuTri.ID_PhanLoaiBenh - 1].Ten_PhanLoaiBenh);

                    r = (HSSFRow)s.GetRow(8);
                    c = (HSSFCell)r.GetCell(1);
                    c.SetCellValue("Ngày bắt đầu điều trị: " + ((DateTime)dieuTri.NgayDT).ToString("dd/MM/yyyy"));

                    c = (HSSFCell)r.GetCell(9);
                    c.SetCellValue("Phác đồ điều trị: " + phacdos[(int)dieuTri.ID_PhacdoDT - 1].Ten_PhacdoDT);

                    r = (HSSFRow)s.GetRow(9);
                    c = (HSSFCell)r.GetCell(1);
                    c.SetCellValue("Liều lượng thuốc/ngày: H: " + giamsats[0].LieuLuong_H + "       E: " + giamsats[0].LieuLuong_E + "        RH: " + giamsats[0].LieuLuong_RH);

                    for (int rownum = 15; rownum < giamsats.Count + 15; rownum++)
                    {
                        r = (HSSFRow)s.CreateRow(rownum);

                        c = (HSSFCell)r.CreateCell(1);
                        c.CellStyle = csDate;
                        c.SetCellValue(giamsats[rownum - 15].NgayGiamSat);

                        c = (HSSFCell)r.CreateCell(2);
                        c.CellStyle = csDate;
                        c.SetCellValue(giamsats[rownum - 15].NgayCapThuocGanNhat);

                        c = (HSSFCell)r.CreateCell(3, CellType.Numeric);
                        c.CellStyle = cs1;
                        c.SetCellValue((int)giamsats[rownum - 15].LuongCap_H);

                        c = (HSSFCell)r.CreateCell(4);
                        c.CellStyle = cs1;
                        c.SetCellValue((int)giamsats[rownum - 15].LuongCap_E);

                        c = (HSSFCell)r.CreateCell(5);
                        c.CellStyle = cs1;
                        c.SetCellValue((int)giamsats[rownum - 15].LuongCap_RH);

                        c = (HSSFCell)r.CreateCell(6);
                        c.CellStyle = cs1;
                        c.SetCellValue((int)giamsats[rownum - 15].ConLai_H);

                        c = (HSSFCell)r.CreateCell(7);
                        c.CellStyle = cs1;
                        c.SetCellValue((int)giamsats[rownum - 15].ConLai_E);

                        c = (HSSFCell)r.CreateCell(8);
                        c.CellStyle = cs1;
                        c.SetCellValue((int)giamsats[rownum - 15].ConLai_RH);

                        c = (HSSFCell)r.CreateCell(9);
                        c.CellStyle = cs;
                        c.SetCellValue(giamsats[rownum - 15].NhanXetCuaGiamSatVien);

                        c = (HSSFCell)r.CreateCell(10);
                        c.CellStyle = cs;
                        c.SetCellValue(giamsats[rownum - 15].DienBienDieuTri);

                        c = (HSSFCell)r.CreateCell(11);
                        c.CellStyle = cs;
                        c.SetCellValue(giamsats[rownum - 15].DanDoBenhNhan);

                        c = (HSSFCell)r.CreateCell(12);
                        c.CellStyle = cs;
                        c = (HSSFCell)r.CreateCell(13);
                        c.CellStyle = cs;
                    }
                    wb.Write(ms);
                }
                JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
                return TheSerializer.Serialize(filepath);
            }
            else
            {
                return null;
            }
        }

        [WebMethod]
        public int SaveBaoCaoGiamSatBenhNhan()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JObject req = JObject.Parse(input);            

            return SaveBaoCaoGiamSatBenhNhan(req);
        }

        private int SaveBaoCaoGiamSatBenhNhan(JObject req)
        {
            var tmp = req["ID"]; long idGS = 0;
            if (tmp != null)
                idGS = (long)tmp;
            string sql = "";
            List<SqlParameter> parameterList = new List<SqlParameter>();
            if (idGS > 0)
            {
                sql = "UPDATE dbo.HTDT_BaoCaoGiamSat SET Diachi=@P2,Tuoi=@P3,LieuLuong_E=@P4,LieuLuong_H=@P5,LieuLuong_RH=@P6,"
                + " LuongCap_E=@P7,LuongCap_H=@P8,LuongCap_RH=@P9,ConLai_E=@P10,ConLai_H=@P11,ConLai_RH=@P12,Sodienthoai=@P13,"
                + " NgayGiamSat=@P14,NgayCapThuocGanNhat=@P15,NhanXetCuaGiamSatVien=@P16,DienBienDieuTri=@P17,"
                + " DanDoBenhNhan=@P18,NhapTuMobile=@P19,NgayNhapBaoCao=@P20"
                + " WHERE ID=@P21";
                parameterList.Add(new SqlParameter("@P21", (long)req["ID"]));
            }
            else
            {
                sql = "INSERT INTO dbo.HTDT_BaoCaoGiamSat (ID_BenhNhan,ID_SoDieuTri,Diachi,Tuoi,LieuLuong_E,LieuLuong_H,LieuLuong_RH, "
                + " LuongCap_E,LuongCap_H,LuongCap_RH,ConLai_E,ConLai_H,ConLai_RH,Sodienthoai,NgayGiamSat,NgayCapThuocGanNhat,"
                + " NhanXetCuaGiamSatVien,DienBienDieuTri,DanDoBenhNhan,NhapTuMobile,NgayNhapBaoCao)"
                + " VALUES (@P0,@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8,@P9,@P10,@P11,@P12,@P13,@P14,@P15,@P16,@P17,@P18,@P19,@P20)";
                parameterList.Add(new SqlParameter("@P0", (long)req["ID_BenhNhan"]));
                parameterList.Add(new SqlParameter("@P1", (long)req["ID_SoDieuTri"]));
            }

            parameterList.Add(new SqlParameter("@P2", (string)req["Diachi"]));
            parameterList.Add(new SqlParameter("@P3", (int)req["Tuoi"]));
            parameterList.Add(new SqlParameter("@P4", (double)req["LieuLuong_E"]));
            parameterList.Add(new SqlParameter("@P5", (double)req["LieuLuong_H"]));
            parameterList.Add(new SqlParameter("@P6", (double)req["LieuLuong_RH"]));
            parameterList.Add(new SqlParameter("@P7", (double)req["LuongCap_E"]));
            parameterList.Add(new SqlParameter("@P8", (double)req["LuongCap_H"]));
            parameterList.Add(new SqlParameter("@P9", (double)req["LuongCap_RH"]));
            parameterList.Add(new SqlParameter("@P10", (double)req["ConLai_E"]));
            parameterList.Add(new SqlParameter("@P11", (double)req["ConLai_H"]));
            parameterList.Add(new SqlParameter("@P12", (double)req["ConLai_RH"]));
            parameterList.Add(new SqlParameter("@P13", (string)req["Sodienthoai"]));
            parameterList.Add(new SqlParameter("@P14", DateTime.ParseExact((string)req["NgayGiamSat"], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            parameterList.Add(new SqlParameter("@P15", DateTime.ParseExact((string)req["NgayCapThuocGanNhat"], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            parameterList.Add(new SqlParameter("@P16", (string)req["NhanXetCuaGiamSatVien"]));
            parameterList.Add(new SqlParameter("@P17", (string)req["DienBienDieuTri"]));
            parameterList.Add(new SqlParameter("@P18", (string)req["DanDoBenhNhan"]));
            parameterList.Add(new SqlParameter("@P19", (bool)req["NhapTuMobile"]));
            parameterList.Add(new SqlParameter("@P20", DateTime.Now));

            SqlParameter[] parameters = parameterList.ToArray();
            int result = db.Database.ExecuteSqlCommand(sql, parameters);

            return result;

        }

        [WebMethod]
        public string GetDsTacDungPhu()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JObject req = JObject.Parse(input);
            List<BenhNhanVO> patients = GetDsTacDungPhu(req);
            if (patients != null && patients.Count > 0)
            {
                JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
                return TheSerializer.Serialize(patients);
            }
            else
            {
                return null;
            }
        }

        [WebMethod]
        public string GetDsBenhNhanGS()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JObject req = JObject.Parse(input);
            BenhNhanGSGrid grid = GetGridBenhNhanGS(req, true);
            if (grid != null)
            {
                JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
                return TheSerializer.Serialize(grid);
            }
            else
            {
                return null;
            }
        }

        [WebMethod]
        public string GetThongKeSMS()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JObject req = JObject.Parse(input);

            string provinceId = (string)req["provinceId"];
            string districtId = (string)req["districtId"];
            string strfromDate = (string)req["fromDate"];
            string strtoDate = (string)req["toDate"];
            DateTime fromDate = strfromDate.Length > 0 ? DateTime.ParseExact(strfromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime toDate = strtoDate.Length > 0 ? DateTime.ParseExact(strtoDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : DateTime.Now;

            string sql = "SELECT * FROM dbo.HTDT_ThongKeTinNhan td WHERE td.MA_HUYEN=@P0 "
                    + " AND convert(date, td.Ngay) >= convert(date,@fromDate) "
                    + " AND convert(date, td.Ngay) <= convert(date,@toDate) ";

            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            parameterList.Add(new SqlParameter("@fromDate", fromDate));
            parameterList.Add(new SqlParameter("@toDate", toDate));
            SqlParameter[] parameters = parameterList.ToArray();
            List<HTDT_ThongKeTinNhan> smss = db.Database.SqlQuery<HTDT_ThongKeTinNhan>(sql,parameters).ToList();
            
            SMSStatisticVO result = new SMSStatisticVO();

            if (smss != null && smss.Count > 0)
            {
                

                string sql3 = "SELECT SUM(tk.SoTinNhanUongThuoc) AS totalSoTinNhanUT, "
                + " SUM(tk.SoTinNhanTaiKham) AS totalSoTinNhanXN, "
                + " SUM(tk.SoTinNhanGuiKhac) AS totalSoTinNhanTruyenThong, "
                + " SUM(tk.SoTinNhanPhanHoiUT) AS totalSoTinNhanPhanHoiUT, "
                + " SUM(tk.SoTinNhanTacDungPhu) AS totalSoTinNhanTacDungPhu "
                + " FROM dbo.HTDT_ThongKeTinNhan tk WHERE tk.MA_HUYEN=@P0 "
                + " AND convert(date, tk.Ngay) >= convert(date,@fromDate) "
                + " AND convert(date, tk.Ngay) <= convert(date,@toDate) ";

                List<SqlParameter> parameterList3 = new List<SqlParameter>();
                parameterList3.Add(new SqlParameter("@P0", districtId));
                parameterList3.Add(new SqlParameter("@fromDate", fromDate));
                parameterList3.Add(new SqlParameter("@toDate", toDate));
                SqlParameter[] parameters3 = parameterList3.ToArray();

                result = db.Database.SqlQuery<SMSStatisticVO>(sql3, parameters3).SingleOrDefault();
                result.smss = smss;
                JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
                return TheSerializer.Serialize(result);
            }
            else
            {
                return null;
            }
        }

        private BenhNhanGSGrid GetGridBenhNhanGS(JObject req, bool isPaging)
        {
            string provinceId = (string)req["provinceId"];
            string districtId = (string)req["districtId"];
            string communeId = (string)req["communeId"];
            int pageSize = (int)req["pageSize"];
            int skip = (int)req["skip"];
            short statusId = (short)req["statusId"];
            string strfromDate = (string)req["fromDate"];
            string strtoDate = (string)req["toDate"];
            DateTime fromDate = strfromDate.Length > 0 ? DateTime.ParseExact(strfromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime toDate = strtoDate.Length > 0 ? DateTime.ParseExact(strtoDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : DateTime.Now;

            string sql = "SELECT count(sdt.ID_BenhNhan) FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                + " WHERE sdt.ID_BENHNHAN=bn.ID_BenhNhan ";
            if (!communeId.Equals("0"))
            {
                sql += " AND bn.MA_XA=@PMAXA ";
            }
            else if (!districtId.Equals("0"))
            {
                sql += " AND bn.MA_HUYEN=@PMAHUYEN ";
            }
            else if (!provinceId.Equals("0"))
            {
                sql += " AND bn.MA_TINH=@PMATINH ";
            }
            sql += " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL ";
            if (strfromDate.Length > 0)
                sql += " AND convert(date, sdt.NgayDT) >= convert(date,@fromDate) ";
            if (strtoDate.Length > 0)
                sql += " AND convert(date, sdt.NgayDT) <= convert(date,@toDate) ";

            if (statusId == 0)
            {
                sql += " AND sdt.NgayRV IS NULL ";
            }
            else
            {
                sql += " AND sdt.NgayRV IS NOT NULL ";
            }
            List<SqlParameter> parameterList = new List<SqlParameter>();
            if (!communeId.Equals("0"))
            {
                parameterList.Add(new SqlParameter("@PMAXA", communeId));
            }
            else if (!districtId.Equals("0"))
            {
                parameterList.Add(new SqlParameter("@PMAHUYEN", districtId));
            }
            else if (!provinceId.Equals("0"))
            {
                parameterList.Add(new SqlParameter("@PMATINH", provinceId));
            }
            parameterList.Add(new SqlParameter("@fromDate", fromDate));
            parameterList.Add(new SqlParameter("@toDate", toDate));
            SqlParameter[] parameters = parameterList.ToArray();
            int total = db.Database.SqlQuery<int>(sql, parameters).Single();

            string sql2 = "SELECT bn.ID_BenhNhan AS ID_BenhNhan, sdt.ID_SoDieuTri AS ID_SoDieuTri,bn.HoTen AS HoTen, AVG(bn.Tuoi) AS Tuoi, "
                + " bn.Gioitinh AS Gioitinh, xa.TEN_XA AS TEN_XA, "
                + " bn.Diachi AS Diachi, bn.Sodienthoai AS Sodienthoai, sdt.NgayDT AS NgayDT, sdt.ID_PhacdoDT AS ID_PhacDoDT, "
                + " sdt.ID_PhanLoaiBenh AS ID_PhanLoaiBenh, COUNT(gs.ID) AS SoLanGiamSat "
                + " FROM dbo.SO_BenhNhan bn, dbo.DM_Xa xa, dbo.SO_SoDieuTri sdt"
                + " LEFT JOIN dbo.HTDT_BaoCaoGiamSat gs ON gs.ID_SoDieuTri=sdt.ID_SoDieuTri "
                + " WHERE sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_XA=xa.MA_XA ";
            if (!communeId.Equals("0"))
            {
                sql2 += " AND bn.MA_XA=@PMAXA ";
            }
            else if (!districtId.Equals("0"))
            {
                sql2 += " AND bn.MA_HUYEN=@PMAHUYEN ";
            }
            else if (!provinceId.Equals("0"))
            {
                sql2 += " AND bn.MA_TINH=@PMATINH ";
            }
            sql2 += " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL ";
            if (strfromDate.Length > 0)
                sql2 += " AND convert(date, sdt.NgayDT) >= convert(date,@fromDate) ";
            if (strtoDate.Length > 0)
                sql2 += " AND convert(date, sdt.NgayDT) <= convert(date,@toDate) ";
            if (statusId == 0)
            {
                sql2 += " AND sdt.NgayRV IS NULL ";
            }
            else
            {
                sql2 += " AND sdt.NgayRV IS NOT NULL ";
            }
            sql2 += " GROUP BY bn.ID_BenhNhan,bn.HoTen,bn.Gioitinh,bn.Diachi,bn.Sodienthoai,sdt.ID_SoDieuTri,sdt.NgayDT,sdt.ID_PhacdoDT,sdt.ID_PhanLoaiBenh,xa.TEN_XA ";
            List<SqlParameter> parameterList2 = new List<SqlParameter>();
            if (!communeId.Equals("0"))
            {
                parameterList2.Add(new SqlParameter("@PMAXA", communeId));
            }
            else if (!districtId.Equals("0"))
            {
                parameterList2.Add(new SqlParameter("@PMAHUYEN", districtId));
            }
            else if (!provinceId.Equals("0"))
            {
                parameterList2.Add(new SqlParameter("@PMATINH", provinceId));
            }
            parameterList2.Add(new SqlParameter("@fromDate", fromDate));
            parameterList2.Add(new SqlParameter("@toDate", toDate));
            SqlParameter[] parameters2 = parameterList2.ToArray();

            List<BenhNhanGS> patients = new List<BenhNhanGS>();
            if (isPaging)
            {
                patients = db.Database.SqlQuery<BenhNhanGS>(sql2, parameters2).Skip(skip).Take(pageSize).ToList();
            }
            else
            {
                patients = db.Database.SqlQuery<BenhNhanGS>(sql2, parameters2).ToList();
            }
            BenhNhanGSGrid grid = new BenhNhanGSGrid();
            if (patients != null && patients.Count > 0)
            {
                grid.total = total;
                grid.patients = patients;
            }
            else
            {
                grid.total = 0;
                grid.patients = null;
            }
            
            return grid;
        }

        [WebMethod]
        public string GetDsGiamSatCuaBenhNhan()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JObject req = JObject.Parse(input);

            List<BaoCaoGiamSatVO> giamsats = GetDsGiamSatCuaBenhNhan(req);
            if (giamsats != null && giamsats.Count() > 0)
            {
                JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
                return TheSerializer.Serialize(giamsats);
            }
            else
            {
                return null;
            }

        }

        private List<BaoCaoGiamSatVO> GetDsGiamSatCuaBenhNhan(JObject req)
        {
            long benhNhanId = (long)req["benhNhanId"];

            string sql = "SELECT ID, ID_BenhNhan, ID_SoDieuTri, Diachi, Tuoi, LieuLuong_E, LieuLuong_H, LieuLuong_RH, LuongCap_E, "
                + " LuongCap_H, LuongCap_RH, ConLai_E, ConLai_H, ConLai_RH, Sodienthoai, NgayGiamSat, NgayCapThuocGanNhat, "
                + " NhanXetCuaGiamSatVien, DienBienDieuTri, DanDoBenhNhan, NhapTuMobile, NgayNhapBaoCao, "
                + " CONVERT(VARCHAR(10),NgayGiamSat,103) AS StrNgayGiamSat  "
                + " FROM dbo.HTDT_BaoCaoGiamSat "
                + " WHERE ID_BENHNHAN=@P0";

            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", benhNhanId));
            SqlParameter[] parameters = parameterList.ToArray();

            List<BaoCaoGiamSatVO> giamsats = db.Database.SqlQuery<BaoCaoGiamSatVO>(sql, parameters).ToList();
            return giamsats;
        }

        [WebMethod]
        public string XoaBaoCaoGiamSat()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JObject req = JObject.Parse(input);
            XoaBaoCaoGiamSat(req);
            
            BenhNhanGSGrid grid = GetGridBenhNhanGS(req, true);
            if (grid != null)
            {
                JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
                return TheSerializer.Serialize(grid);
            }
            else
            {
                return null;
            }
        }

        private int XoaBaoCaoGiamSat(JObject req)
        {
            long ID = (long)req["ID"];
            string sql = "DELETE FROM dbo.HTDT_BaoCaoGiamSat WHERE ID=@P0 ";
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", ID));
            SqlParameter[] parameters = parameterList.ToArray();
            int total = db.Database.ExecuteSqlCommand(sql, parameters);
            return total;
        }

        private List<BenhNhanVO> GetDsTacDungPhu(JObject req)
        {
            string provinceId = (string)req["provinceId"];
            string districtId = (string)req["districtId"];
            string communeId = (string)req["communeId"];            
            string strfromDate = (string)req["fromDate"];
            string strtoDate = (string)req["toDate"];
            DateTime fromDate = strfromDate.Length > 0 ? DateTime.ParseExact(strfromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime toDate = strtoDate.Length > 0 ? DateTime.ParseExact(strtoDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : DateTime.Now;
            
            string sql2 = "SELECT tc.ID AS ID, bn.ID_BenhNhan AS ID_BenhNhan, bn.HoTen AS HoTen, bn.Tuoi AS Tuoi, "
                + " bn.Gioitinh AS Gioitinh, bn.Diachi AS Diachi, bn.Sodienthoai AS Sodienthoai, sdt.NgayDT AS NgayDT, "
                + " xa.TEN_XA AS TEN_XA, sdt.ID_PhacdoDT AS ID_PhacDoDT, sdt.ID_PhanLoaiBenh AS ID_PhanLoaiBenh, "
                + " tc.TacDungPhu AS TacDungPhu, tc.NgayNhan AS NgayNhanTinTacDungPhu "
                + " FROM dbo.SO_BenhNhan bn, dbo.SO_SoDieuTri sdt, dbo.HTDT_BenhNhanPhanHoiTacDungPhu tc, dbo.DM_Xa xa "
                + " WHERE sdt.ID_BENHNHAN=bn.ID_BenhNhan AND tc.ID_SoDieuTri=sdt.ID_SoDieuTri AND bn.MA_XA=xa.MA_XA ";
            if (!communeId.Equals("0"))
            {
                sql2 += " AND bn.MA_XA=@PMAXA ";
            }
            else if (!districtId.Equals("0"))
            {
                sql2 += " AND bn.MA_HUYEN=@PMAHUYEN ";
            }
            else if (!provinceId.Equals("0"))
            {
                sql2 += " AND bn.MA_TINH=@PMATINH ";
            }
            sql2 += " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL ";
            if (strfromDate.Length > 0)
                sql2 += " AND convert(date, tc.NgayNhan) >= convert(date,@fromDate) ";
            if (strtoDate.Length > 0)
                sql2 += " AND convert(date, tc.NgayNhan) <= convert(date,@toDate) ";
                        
            List<SqlParameter> parameterList2 = new List<SqlParameter>();
            if (!communeId.Equals("0"))
            {
                parameterList2.Add(new SqlParameter("@PMAXA", communeId));
            }
            else if (!districtId.Equals("0"))
            {
                parameterList2.Add(new SqlParameter("@PMAHUYEN", districtId));
            }
            else if (!provinceId.Equals("0"))
            {
                parameterList2.Add(new SqlParameter("@PMATINH", provinceId));
            }
            parameterList2.Add(new SqlParameter("@fromDate", fromDate));
            parameterList2.Add(new SqlParameter("@toDate", toDate));
            SqlParameter[] parameters2 = parameterList2.ToArray();

            List<BenhNhanVO> patients = new List<BenhNhanVO>();
            patients = db.Database.SqlQuery<BenhNhanVO>(sql2, parameters2).ToList();
            return patients;            
        }

        [WebMethod]
        public string GetDsBenhNhanDoiDiaChi()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JObject req = JObject.Parse(input);
            List<BenhNhanVO> patients = GetBenhNhanDoiDiaChi(req);
            if (patients != null && patients.Count > 0)
            {
                JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
                return TheSerializer.Serialize(patients);
            }
            else
            {
                return null;
            }
        }

        private List<BenhNhanVO> GetBenhNhanDoiDiaChi(JObject req)
        {
            string provinceId = (string)req["provinceId"];
            string districtId = (string)req["districtId"];
            string communeId = (string)req["communeId"];
            string sql = "SELECT dc.ID AS ID, bn.ID_BenhNhan AS ID_BenhNhan, bn.HoTen AS HoTen, bn.Tuoi AS Tuoi, "
                + " bn.Gioitinh AS Gioitinh, xa.TEN_XA AS TEN_XA, bn.Diachi AS Diachi, bn.Sodienthoai AS Sodienthoai, "
                + " dc.NgayTao AS NgayTaoBaoCaoDoiDiaChi, dc.CapNhap AS CapNhapDiaChi "
                + " FROM dbo.SO_BenhNhan bn, dbo.HTDT_BenhNhanDoiDiaChi dc, dbo.DM_Xa xa "
                + " WHERE dc.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_XA=xa.MA_XA ";
            if (!communeId.Equals("0"))
            {
                sql += " AND bn.MA_XA=@PMAXA ";
            }
            else if (!districtId.Equals("0"))
            {
                sql += " AND bn.MA_HUYEN=@PMAHUYEN ";
            }
            else if (!provinceId.Equals("0"))
            {
                sql += " AND bn.MA_TINH=@PMATINH ";
            }
           
            List<SqlParameter> parameterList = new List<SqlParameter>();
            if (!communeId.Equals("0"))
            {
                parameterList.Add(new SqlParameter("@PMAXA", communeId));
            }
            else if (!districtId.Equals("0"))
            {
                parameterList.Add(new SqlParameter("@PMAHUYEN", districtId));
            }
            else if (!provinceId.Equals("0"))
            {
                parameterList.Add(new SqlParameter("@PMATINH", provinceId));
            }
            SqlParameter[] parameters = parameterList.ToArray();

            List<BenhNhanVO> patients = db.Database.SqlQuery<BenhNhanVO>(sql, parameters).ToList();
            return patients;
        }

        [WebMethod]
        public string SaveBenhNhanDoiDiaChi()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JArray req = JArray.Parse(input);

            for (int i = 0; i < req.Count; i++)
            {
                JObject due = (JObject)req[i];
                string sql = "UPDATE dbo.HTDT_BenhNhanDoiDiaChi SET NgayCapNhap=@P0,CapNhap=@P1 WHERE ID=@P2";
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@P0", DateTime.Now));
                parameterList.Add(new SqlParameter("@P1", (short)due["CapNhapDiaChi"]));
                parameterList.Add(new SqlParameter("@P2", (long)due["ID"]));
                SqlParameter[] parameters = parameterList.ToArray();
                int result = db.Database.ExecuteSqlCommand(sql, parameters);
            }

            return "";
        }

        [WebMethod]
        public string GetDMThongDiep()
        {
            string giaidoan = Request.QueryString["giaidoan"]; giaidoan = giaidoan != null ? giaidoan : "";
            string loai = Request.QueryString["loai"]; loai = loai != null ? loai : "";
            string sql = "SELECT * FROM dbo.HTDT_ThongDiepTruyenThong td WHERE 1=1 ";
            if (giaidoan.Length > 0)
            {
                sql += " AND td.GiaiDoan=@P1";
            }
            if (loai.Length > 0)
            {
                sql += " AND td.Loai=@P2";
            }

            List<SqlParameter> parameterList = new List<SqlParameter>();
            if (giaidoan.Length > 0)
            {
                parameterList.Add(new SqlParameter("@P1", giaidoan));
            }
            if (loai.Length > 0)
            {
                parameterList.Add(new SqlParameter("@P2", loai));
            }
            List<HTDT_ThongDiepTruyenThong> dmthongdiep = null;
            if (giaidoan.Length > 0 || loai.Length > 0)
            {
                SqlParameter[] parameters = parameterList.ToArray();
                dmthongdiep = db.Database.SqlQuery<HTDT_ThongDiepTruyenThong>(sql, parameters).ToList();
            }
            else
            {
                dmthongdiep = db.Database.SqlQuery<HTDT_ThongDiepTruyenThong>(sql).ToList();
            }
            if (dmthongdiep != null && dmthongdiep.Count > 0)
            {
                JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
                return TheSerializer.Serialize(dmthongdiep);
            }
            else
            {
                return null;
            }        
        }

        [WebMethod]
        public long SaveThongDiep()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JObject req = JObject.Parse(input);
            var tmp = req["ID"];
            long ID = 0;
            if (tmp != null)
                ID = (long)tmp;
            string sql = "";
            List<SqlParameter> parameterList = new List<SqlParameter>();
            if (ID > 0)
            {
                sql = "UPDATE dbo.HTDT_ThongDiepTruyenThong SET NoiDung=@P0,LoaiBenhNhan=@P1,LoaiTinNhan=@P2,T1=@P3,T2=@P4,T3=@P5,"
                + " T4=@P6,T5=@P7,T6=@P8,T7=@P9,T8=@P10,T9=@P11,T10=@P12,T11=@P13,T12=@P14,T13=@P15,T14=@P16,T15=@P17,T16=@P18,T17=@P19 "
                + " ,T18=@P20,T19=@P21,T20=@P22,T21=@P23,T22=@P24,T23=@P25,T24=@P26,T25=@P27,T26=@P28 WHERE ID=@P29";
                parameterList.Add(new SqlParameter("@P29", ID));
            }
            else
            {
                sql = "SET NOCOUNT ON;INSERT INTO dbo.HTDT_ThongDiepTruyenThong "
                + " (NoiDung,LoaiBenhNhan,LoaiTinNhan,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18,T19,T20,T21,T22,T23,T24,T25,T26)"
                + " VALUES (@P0,@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8,@P9,@P10,@P11,@P12,@P13,@P14,@P15,@P16,@P17,@P18,@P19,@P20,@P21,@P22,@P23,@P24,@P25,@P26,@P27,@P28);SELECT SCOPE_IDENTITY();";
            }
            parameterList.Add(new SqlParameter("@P0", (string)req["NoiDung"]));
            parameterList.Add(new SqlParameter("@P1", (short)req["LoaiBenhNhan"]));
            parameterList.Add(new SqlParameter("@P2", (short)req["LoaiTinNhan"]));
            parameterList.Add(new SqlParameter("@P3", (short)req["T1"]));
            parameterList.Add(new SqlParameter("@P4", (short)req["T2"]));
            parameterList.Add(new SqlParameter("@P5", (short)req["T3"]));
            parameterList.Add(new SqlParameter("@P6", (short)req["T4"]));
            parameterList.Add(new SqlParameter("@P7", (short)req["T5"]));
            parameterList.Add(new SqlParameter("@P8", (short)req["T6"]));
            parameterList.Add(new SqlParameter("@P9", (short)req["T7"]));
            parameterList.Add(new SqlParameter("@P10", (short)req["T8"]));
            parameterList.Add(new SqlParameter("@P11", (short)req["T9"]));
            parameterList.Add(new SqlParameter("@P12", (short)req["T10"]));
            parameterList.Add(new SqlParameter("@P13", (short)req["T11"]));
            parameterList.Add(new SqlParameter("@P14", (short)req["T12"]));
            parameterList.Add(new SqlParameter("@P15", (short)req["T13"]));
            parameterList.Add(new SqlParameter("@P16", (short)req["T14"]));
            parameterList.Add(new SqlParameter("@P17", (short)req["T15"]));
            parameterList.Add(new SqlParameter("@P18", (short)req["T16"]));
            parameterList.Add(new SqlParameter("@P19", (short)req["T17"]));
            parameterList.Add(new SqlParameter("@P20", (short)req["T18"]));
            parameterList.Add(new SqlParameter("@P21", (short)req["T19"]));
            parameterList.Add(new SqlParameter("@P22", (short)req["T20"]));
            parameterList.Add(new SqlParameter("@P23", (short)req["T21"]));
            parameterList.Add(new SqlParameter("@P24", (short)req["T22"]));
            parameterList.Add(new SqlParameter("@P25", (short)req["T23"]));
            parameterList.Add(new SqlParameter("@P26", (short)req["T24"]));
            parameterList.Add(new SqlParameter("@P27", (short)req["T25"]));
            parameterList.Add(new SqlParameter("@P28", (short)req["T26"]));
            SqlParameter[] parameters = parameterList.ToArray();
            long result = 0;
            if (ID > 0)
            {
                result = db.Database.ExecuteSqlCommand(sql, parameters);
            }
            else
            {
                result = (long)db.Database.SqlQuery<decimal>(sql, parameters).FirstOrDefault();
            }

            return result;
        }

        [WebMethod]
        public int DeleteThongDiep()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            JObject req = JObject.Parse(input);
            var tmp = req["ID"];
            long ID = 0;
            if (tmp != null)
                ID = (long)tmp;
            string sql = "";
            if (ID > 0)
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                sql = "DELETE FROM dbo.HTDT_ThongDiepTruyenThong WHERE ID=@P0";
                parameterList.Add(new SqlParameter("@P0", ID));
                SqlParameter[] parameters = parameterList.ToArray();
                return db.Database.ExecuteSqlCommand(sql, parameters);
            }
            else
            {
                return 0;
            }
        }

        [WebMethod]
        public JObject mservice()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            if (input.Length > 5)
            {
                input = input.Substring(5);
            }
            JObject req = JObject.Parse(input);
            JObject result = new JObject();
            short tor = (short)req["tor"];

            switch (tor)
            {
                case 1:
                    result = mbgetPatients(req);
                    break;
                case 2:
                    result = mbgetPatientDetail(req);
                    break;
                case 3:
                    result = mbgetPatientDues(req);
                    break;
                case 4:
                    result = mbgetPatientSupervisions(req);
                    break;
                case 5:
                    result = mbgetPatientSupervisionDetail(req);
                    break;
                case 8:
                    result = mbsavePatientSupervision(req);
                    break;
                case 9:
                    result = mbremoveSupervision(req);
                    break;
                case 10:
                    result = mbreportWrongAddress(req);
                    break;
                default:
                    
                    break;
            }

            return result;
        }

        [WebMethod]
        public JObject mlogin()
        {
            string input;
            using (var reader = new StreamReader(Request.InputStream))
            {
                input = reader.ReadToEnd();
            }
            if (input.Length > 5)
            {
                input = input.Substring(5);
            }
            JObject req = JObject.Parse(input);
            string username = (string)req["username"]; string password = (string)req["password"];
            var User = (AD_Users)db.AD_Users.Where(u => u.Username.Equals(username)).Where(u => u.Password.Equals(password)).SingleOrDefault();

            DM_BenhVien s = (DM_BenhVien)(from u in db.AD_Users
                                          join kho in db.AD_User_Kho on u.UserID equals kho.USERID
                                          join bv in db.DM_BenhVien on kho.MA_BENHVIEN equals bv.ID_BENHVIEN
                                          where u.Username.Equals(username) && u.Password.Equals(password)
                                          select bv).SingleOrDefault();
            JObject result = new JObject();
            if (User != null)
            {
                var phacdos = db.DM_PhacDoDT.ToList();
                var loaibenhs = db.DM_PhanLoaiBenh.ToList();
                result.Add("authenticate", true);
                result.Add("provinceId", s.ID_MATINH + "");
                result.Add("districtId", s.ID_HUYEN + "");
                result.Add("communeId", s.ID_XA);
                result.Add("userId", User.UserID + "");

                JArray jsonArrayTherapy = new JArray();
                foreach (DM_PhacDoDT r in phacdos)
                {
                    JObject js = new JObject();
                    js.Add("id", r.ID_PhacdoDT + "");
                    js.Add("name", r.Ten_PhacdoDT);
                    js.Add("type", r.Phanloai + "");
                    jsonArrayTherapy.Add(js);
                }
                result.Add("therapies", jsonArrayTherapy);

                JArray jsonArrayDiseases = new JArray();
                foreach (DM_PhanLoaiBenh d in loaibenhs)
                {
                    JObject js = new JObject();
                    js.Add("id", d.ID_PhanLoaiBenh + "");
                    js.Add("name", d.Ten_PhanLoaiBenh);
                    jsonArrayDiseases.Add(js);
                }
                result.Add("diseases", jsonArrayDiseases);
            }
            else
            {
                result.Add("authenticate", false);
            }	

            return result;
        }

        private JObject mbgetPatients(JObject req) {
            var phacdos = db.DM_PhacDoDT.ToList();
            var loaibenhs = db.DM_PhanLoaiBenh.ToList();
		    BenhNhanGrid rg = GetGridBenhNhan(req,true);
		    List<BenhNhanVO> re = rg.patients;
		    JArray jsonArray = new JArray();
		    foreach (BenhNhanVO p in re) {
			    JObject js = new JObject();
			    js.Add("id", p.ID_BenhNhan + "");
			    js.Add("age", p.Tuoi + "");
                js.Add("gender", (p.Gioitinh + "").ToLower());
			    js.Add("fullName", p.HoTen);
			    js.Add("loaiBenh", loaibenhs[p.ID_PhanLoaiBenh-1].Ten_PhanLoaiBenh);
                js.Add("treatmentDate", ((DateTime)p.NgayDT).ToString("dd/MM/yyyy"));

                if (p.ID_PhacDoDT <= phacdos.Count() && p.ID_PhacDoDT >= 1)
                    js.Add("therapy", phacdos[p.ID_PhacDoDT - 1].Ten_PhacdoDT);
			    else
				    js.Add("therapy", "0");
			    js.Add("address", p.Diachi);
			    js.Add("mobileNumber", p.Sodienthoai);
			    jsonArray.Add(js);			
		    }
		    JObject result = new JObject();
		    result.Add("totalResultsAvailable", rg.total);
            result.Add("totalResultsReturned", re != null ? re.Count : 0);
		    result.Add("Result", jsonArray);
		    return result;
	    }

        private BenhNhanGrid GetGridBenhNhan(JObject req, bool isPaging)
        {
            string provinceId = (string)req["provinceId"];
            string districtId = (string)req["districtId"];
            string communeId = (string)req["communeId"];
            int pageSize = (int)req["pageSize"];
            int skip = (int)req["skip"];
            short statusId = (short)req["statusId"];
            string strfromDate = (string)req["fromDate"];
            string strtoDate = (string)req["toDate"];
            DateTime fromDate = strfromDate.Length > 0 ? DateTime.ParseExact(strfromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime toDate = strtoDate.Length > 0 ? DateTime.ParseExact(strtoDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) : DateTime.Now;

            string sql = "SELECT count(sdt.ID_BenhNhan) FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                + " WHERE sdt.ID_BENHNHAN=bn.ID_BenhNhan ";
            if (!communeId.Equals("0"))
            {
                sql += " AND bn.MA_XA=@PMAXA ";
            }
            else if (!districtId.Equals("0"))
            {
                sql += " AND bn.MA_HUYEN=@PMAHUYEN ";
            }
            else if (!provinceId.Equals("0"))
            {
                sql += " AND bn.MA_TINH=@PMATINH ";
            }
            sql += " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL ";
            if (strfromDate.Length > 0)
                sql += " AND convert(date, sdt.NgayDT) >= convert(date,@fromDate) ";
            if (strtoDate.Length > 0)
                sql += " AND convert(date, sdt.NgayDT) <= convert(date,@toDate) ";
            if (statusId == 0)
            {
                sql += " AND sdt.NgayRV IS NULL ";
            }
            else
            {
                sql += " AND sdt.NgayRV IS NOT NULL ";
            }
            List<SqlParameter> parameterList = new List<SqlParameter>();
            if (!communeId.Equals("0"))
            {
                parameterList.Add(new SqlParameter("@PMAXA", communeId));
            }
            else if (!districtId.Equals("0"))
            {
                parameterList.Add(new SqlParameter("@PMAHUYEN", districtId));
            }
            else if (!provinceId.Equals("0"))
            {
                parameterList.Add(new SqlParameter("@PMATINH", provinceId));
            }
            parameterList.Add(new SqlParameter("@fromDate", fromDate));
            parameterList.Add(new SqlParameter("@toDate", toDate));
            SqlParameter[] parameters = parameterList.ToArray();
            int total = db.Database.SqlQuery<int>(sql, parameters).Single();

            string sql2 = "SELECT bn.ID_BenhNhan AS ID_BenhNhan, sdt.ID_SoDieuTri AS ID_SoDieuTri,bn.HoTen AS HoTen, "
                + " bn.Tuoi AS Tuoi, bn.Gioitinh AS Gioitinh, "
                + " bn.Diachi AS Diachi, bn.Sodienthoai AS Sodienthoai, sdt.NgayDT AS NgayDT, sdt.ID_PhacdoDT AS ID_PhacDoDT, "
                + " sdt.ID_PhanLoaiBenh AS ID_PhanLoaiBenh "
                + " FROM dbo.SO_BenhNhan bn, dbo.SO_SoDieuTri sdt, dbo.DM_Xa xa "
                + " WHERE sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_XA=xa.MA_XA ";
            if (!communeId.Equals("0"))
            {
                sql2 += " AND bn.MA_XA=@PMAXA ";
            }
            else if (!districtId.Equals("0"))
            {
                sql2 += " AND bn.MA_HUYEN=@PMAHUYEN ";
            }
            else if (!provinceId.Equals("0"))
            {
                sql2 += " AND bn.MA_TINH=@PMATINH ";
            }
            sql2 += " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL ";
            if (strfromDate.Length > 0)
                sql2 += " AND convert(date, sdt.NgayDT) >= convert(date,@fromDate) ";
            if (strtoDate.Length > 0)
                sql2 += " AND convert(date, sdt.NgayDT) <= convert(date,@toDate) ";
            if (statusId == 0)
            {
                sql2 += " AND sdt.NgayRV IS NULL ";
            }
            else
            {
                sql2 += " AND sdt.NgayRV IS NOT NULL ";
            }
           
            List<SqlParameter> parameterList2 = new List<SqlParameter>();
            if (!communeId.Equals("0"))
            {
                parameterList2.Add(new SqlParameter("@PMAXA", communeId));
            }
            else if (!districtId.Equals("0"))
            {
                parameterList2.Add(new SqlParameter("@PMAHUYEN", districtId));
            }
            else if (!provinceId.Equals("0"))
            {
                parameterList2.Add(new SqlParameter("@PMATINH", provinceId));
            }
            parameterList2.Add(new SqlParameter("@fromDate", fromDate));
            parameterList2.Add(new SqlParameter("@toDate", toDate));
            SqlParameter[] parameters2 = parameterList2.ToArray();

            List<BenhNhanVO> patients = new List<BenhNhanVO>();
            if (isPaging)
            {
                patients = db.Database.SqlQuery<BenhNhanVO>(sql2, parameters2).Skip(skip).Take(pageSize).ToList();
            }
            else
            {
                patients = db.Database.SqlQuery<BenhNhanVO>(sql2, parameters2).ToList();
            }
            BenhNhanGrid grid = new BenhNhanGrid();
            if (patients != null && patients.Count > 0)
            {
                grid.total = total;
                grid.patients = patients;
            }
            else
            {
                grid.total = 0;
                grid.patients = null;
            }
            
            return grid;
        }

        private JObject mbgetPatientDues(JObject req)
        {		
		    var phacdos = db.DM_PhacDoDT.ToList();
            var loaibenhs = db.DM_PhanLoaiBenh.ToList();
            BenhNhanGrid rg = GetGridBenhNhanXetNghiem(req, true);
            List<BenhNhanVO> re = rg.patients;
            JArray jsonArray = new JArray();
            foreach (BenhNhanVO p in re)
            {
                JObject js = new JObject();
                js.Add("id", p.ID + "");
                js.Add("age", p.Tuoi + "");
                js.Add("gender", (p.Gioitinh + "").ToLower());
                js.Add("fullName", p.HoTen);
                js.Add("loaiBenh", loaibenhs[p.ID_PhanLoaiBenh - 1].Ten_PhanLoaiBenh);
                js.Add("treatmentDate", ((DateTime)p.NgayDT).ToString("dd/MM/yyyy"));

                if (p.ID_PhacDoDT <= phacdos.Count() && p.ID_PhacDoDT >= 1)
                    js.Add("therapy", phacdos[p.ID_PhacDoDT - 1].Ten_PhacdoDT);
                else
                    js.Add("therapy", "0");
                js.Add("address", p.Diachi);
                js.Add("mobileNumber", p.Sodienthoai);
                js.Add("lanKham", p.LanXN + "");
                js.Add("dueDate", ((DateTime)p.NgayXN).ToString("dd/MM/yyyy"));
                js.Add("overdueDay", p.SoNgayTreHen + "");
                jsonArray.Add(js);		
		    }

		    JObject result = new JObject();
		    result.Add("totalResultsAvailable", rg.total);
            result.Add("totalResultsReturned", re != null ? re.Count : 0);
		    result.Add("Result", jsonArray);
		    return result;
	    }

        private JObject mbgetPatientSupervisions(JObject req) {		
		    var phacdos = db.DM_PhacDoDT.ToList();
            var loaibenhs = db.DM_PhanLoaiBenh.ToList();
            BenhNhanGSGrid rg = GetGridBenhNhanGS(req, true);
            List<BenhNhanGS> re = rg.patients;
            JArray jsonArray = new JArray();
		    foreach (BenhNhanGS p in re) {
                JObject js = new JObject();
                js.Add("id", p.ID_BenhNhan + "");
                js.Add("age", p.Tuoi + "");
                js.Add("gender", (p.Gioitinh + "").ToLower());
                js.Add("fullName", p.HoTen);
                js.Add("loaiBenh", loaibenhs[p.ID_PhanLoaiBenh - 1].Ten_PhanLoaiBenh);
                js.Add("loaiBenhId", p.ID_PhanLoaiBenh + "");
                js.Add("treatmentDate", ((DateTime)p.NgayDT).ToString("dd/MM/yyyy"));
                js.Add("totalSupervision", p.SoLanGiamSat + "");
                js.Add("address", p.Diachi);
                js.Add("mobileNumber", p.Sodienthoai);
                js.Add("patientId", p.ID_BenhNhan + "");
                js.Add("patientTreatmentId", p.ID_SoDieuTri + "");
                if (p.ID_PhacDoDT <= phacdos.Count() && p.ID_PhacDoDT >= 1)
                {
                    js.Add("therapy", phacdos[p.ID_PhacDoDT - 1].Ten_PhacdoDT);
                    js.Add("therapyId", p.ID_PhacDoDT + "");
                }
                else
                {
                    js.Add("therapy", "0");
                    js.Add("therapyId", "0");
                }
                HTDT_BenhNhanDoiDiaChi pa = GetBenhNhanDoiDiaChi(p.ID_BenhNhan);
                if (pa != null) {
                        js.Add("changeaddress", "true");
                } else
                        js.Add("changeaddress", "false");                
                jsonArray.Add(js);			
		    }
            JObject result = new JObject();
            result.Add("totalResultsAvailable", rg.total);
            result.Add("totalResultsReturned", re != null ? re.Count : 0);
            result.Add("Result", jsonArray);
            return result;
	    }

        private HTDT_BenhNhanDoiDiaChi GetBenhNhanDoiDiaChi(long ID_BenhNhan)
        {
            string sql = "SELECT * FROM dbo.HTDT_BenhNhanDoiDiaChi dc WHERE dc.ID_BENHNHAN=@P0 AND CapNhap=0 ORDER BY ID DESC";
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", ID_BenhNhan));
            SqlParameter[] parameters = parameterList.ToArray();
            HTDT_BenhNhanDoiDiaChi benhnhan = db.Database.SqlQuery<HTDT_BenhNhanDoiDiaChi>(sql, parameters).FirstOrDefault();
            return benhnhan;
        }

        private JObject mbgetPatientDetail(JObject req)
        {
            var phacdos = db.DM_PhacDoDT.ToList();
            var loaibenhs = db.DM_PhanLoaiBenh.ToList();
            SO_SoDieuTri benhnhan = GetBenhNhanDieuTriDetail(req);

            if (benhnhan == null) return null;

            JObject result = new JObject();
            string ngayVaoVien = "";
            ngayVaoVien = benhnhan.NgayVV != null ? ((DateTime)benhnhan.NgayVV).ToString("dd/MM/yyyy") : "";
            result.Add("ngayVaoVien", ngayVaoVien);
            string treatmentDate = "";
            treatmentDate = benhnhan.NgayDT != null ? ((DateTime)benhnhan.NgayDT).ToString("dd/MM/yyyy") : "";
            result.Add("treatmentDate", treatmentDate);
            result.Add("loaiBenh", loaibenhs[(int)benhnhan.ID_PhanLoaiBenh - 1].Ten_PhanLoaiBenh);
            result.Add("chanDoan", benhnhan.Chandoan != null ? benhnhan.Chandoan : "");
            String ngayChupXQ = "";
            ngayChupXQ = benhnhan.NgayChupXQ != null ? ((DateTime)benhnhan.NgayChupXQ).ToString("dd/MM/yyyy") : "";
            result.Add("ngayChupXQ", ngayChupXQ);
            result.Add("ketQuaChupXQ", benhnhan.KetquaXQ != null ? benhnhan.KetquaXQ + "" : "");
            result.Add("art", benhnhan.ART + "");
            result.Add("cpt", benhnhan.CPT + "");
            result.Add("lympho", benhnhan.LymPho + "");
            result.Add("hiv1", benhnhan.XNHIV1 + "");
            result.Add("hiv2", benhnhan.XNHIV2 + "");
            //result.Add("therapy", therapies.get(ptr.getTherapyId().intValue()-1).getName());
            if (benhnhan.ID_PhacdoDT <= phacdos.Count && benhnhan.ID_PhacdoDT >= 1)
                result.Add("therapy", phacdos[(int)benhnhan.ID_PhacdoDT - 1].Ten_PhacdoDT);
            else
                result.Add("therapy", ".");
            result.Add("ketQuaDieuTri", benhnhan.ID_KetquaDT != null ? benhnhan.ID_KetquaDT + "" : "");
            String ngayRaVien = "";
            ngayRaVien = benhnhan.NgayRV != null ? ((DateTime)benhnhan.NgayRV).ToString("dd/MM/yyyy") : "";
            result.Add("ngayRaVien", ngayRaVien);
            return result;
        }

        private SO_SoDieuTri GetBenhNhanDieuTriDetail(JObject req)
        {
            long BenhNhanID = (long)req["ID_BenhNhan"];
            string sql = "SELECT * FROM dbo.SO_SoDieuTri sdt WHERE sdt.ID_BENHNHAN=@P0 ORDER BY ID_SoDieuTri DESC";

            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", BenhNhanID));
            SqlParameter[] parameters = parameterList.ToArray();
            SO_SoDieuTri benhnhan = db.Database.SqlQuery<SO_SoDieuTri>(sql, parameters).FirstOrDefault();
            return benhnhan;
        }

        private SO_BenhNhan GetBenhNhanDetail(JObject req)
        {
            long BenhNhanID = (long)req["ID_BenhNhan"];
            string sql = "SELECT * FROM dbo.SO_BenhNhan bn WHERE bn.ID_BENHNHAN=@P0 ";

            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", BenhNhanID));
            SqlParameter[] parameters = parameterList.ToArray();
            SO_BenhNhan benhnhan = db.Database.SqlQuery<SO_BenhNhan>(sql, parameters).FirstOrDefault();
            return benhnhan;
        }

        private JObject mbgetPatientSupervisionDetail(JObject req) {		
		    var phacdos = db.DM_PhacDoDT.ToList();
            var loaibenhs = db.DM_PhanLoaiBenh.ToList();
            long BenhNhanID = (long)req["benhNhanId"];
            long SoDieuTriID = (long)req["soDieuTriId"];
            
            SO_BenhNhan benhnhan = db.SO_BenhNhan.Where(p => p.ID_BenhNhan==BenhNhanID).SingleOrDefault();
            SO_SoDieuTri dieutri = db.SO_SoDieuTri.Where(p => p.ID_SoDieuTri == SoDieuTriID).SingleOrDefault();
		    List<BaoCaoGiamSatVO> re = GetDsGiamSatCuaBenhNhan(req);
		    JArray jsonArray = new JArray();
            
		    foreach (BaoCaoGiamSatVO p in re) {
			    JObject js = new JObject();
			    js.Add("id", p.ID + "");
			    js.Add("age", p.Tuoi + "");
			    js.Add("gender", benhnhan.Gioitinh + "");
			    js.Add("fullName", benhnhan.HoTen);
			    js.Add("loaiBenh", loaibenhs[(int)dieutri.ID_PhanLoaiBenh-1].Ten_PhanLoaiBenh);
                js.Add("treatmentDate", ((DateTime)dieutri.NgayDT).ToString("dd/MM/yyyy"));			
			    js.Add("address", p.Diachi);	
			    js.Add("patientId", p.ID_BenhNhan + "");
			    js.Add("patientTreatmentId", p.ID_SoDieuTri + "");
                js.Add("supervisoryDate", ((DateTime)p.NgayGiamSat).ToString("dd/MM/yyyy"));
			    js.Add("comments", p.NhanXetCuaGiamSatVien);
			    js.Add("recommendations", p.DanDoBenhNhan + "");
			    js.Add("treatmentDevelopment", p.DienBienDieuTri + "");
                js.Add("lastedMedicineIssuedDate", ((DateTime)p.NgayCapThuocGanNhat).ToString("dd/MM/yyyy"));
			    String strHAmount = p.LuongCap_H + ""; if (strHAmount.EndsWith(".0")) strHAmount = strHAmount.Substring(0,strHAmount.IndexOf(".")); js.Add("hAmount", strHAmount);
			    String strEAmount = p.LuongCap_E + ""; if (strEAmount.EndsWith(".0")) strEAmount = strEAmount.Substring(0,strEAmount.IndexOf(".")); js.Add("eAmount", strEAmount);
			    String strRHAmount = p.LuongCap_RH + ""; if (strRHAmount.EndsWith(".0")) strRHAmount = strRHAmount.Substring(0,strRHAmount.IndexOf(".")); js.Add("rhAmount", strRHAmount);
                String strHRemainingAmount = p.ConLai_H + ""; if (strHRemainingAmount.EndsWith(".0")) strHRemainingAmount = strHRemainingAmount.Substring(0, strHRemainingAmount.IndexOf(".")); js.Add("hRemainingAmount", strHRemainingAmount);
                String strERemainingAmount = p.ConLai_E + ""; if (strERemainingAmount.EndsWith(".0")) strERemainingAmount = strERemainingAmount.Substring(0, strERemainingAmount.IndexOf(".")); js.Add("eRemainingAmount", strERemainingAmount);
                String strRHRemainingAmount = p.ConLai_RH + ""; if (strRHRemainingAmount.EndsWith(".0")) strRHRemainingAmount = strRHRemainingAmount.Substring(0, strRHRemainingAmount.IndexOf(".")); js.Add("rhRemainingAmount", strRHRemainingAmount);
                js.Add("mobileNumber", p.Sodienthoai != null ? p.Sodienthoai : "");
                String strHDose = p.LieuLuong_H + ""; if (strHDose.EndsWith(".0")) strHDose = strHDose.Substring(0, strHDose.IndexOf(".")); js.Add("hDose", strHDose);
                String strEDose = p.LieuLuong_E + ""; if (strEDose.EndsWith(".0")) strEDose = strEDose.Substring(0, strEDose.IndexOf(".")); js.Add("eDose", strEDose);
                String strRHDose = p.LieuLuong_RH + ""; if (strRHDose.EndsWith(".0")) strRHDose = strRHDose.Substring(0, strRHDose.IndexOf(".")); js.Add("rhDose", strRHDose);			
			    js.Add("therapy", phacdos[(int)dieutri.ID_PhacdoDT-1].Ten_PhacdoDT);
			
			    jsonArray.Add(js);

		    }

		    JObject result = new JObject();
            result.Add("totalResultsAvailable", re != null ? re.Count : 0);
            result.Add("totalResultsReturned", re != null ? re.Count : 0);
            result.Add("Result", jsonArray);
		    return result;
	    }

        private JObject mbsavePatientSupervision(JObject req)
        {
            JObject result = new JObject();
            int item = SaveBaoCaoGiamSatBenhNhan(req);
            result.Add("totalResultsReturned", item);
            return result;
        }

        private JObject mbremoveSupervision(JObject req)
        {
            JObject result = new JObject();
            int total = XoaBaoCaoGiamSat(req);            
            result.Add("totalResultsReturned", total);
            return result;
        }
        private JObject mbreportWrongAddress(JObject req)
        {
            JObject result = new JObject();
            long ID_BenhNhan = (long)req["ID_BenhNhan"];
            if (ID_BenhNhan > 0)
            {
                SO_BenhNhan p = GetBenhNhanDetail(req);
                HTDT_BenhNhanDoiDiaChi pa = new HTDT_BenhNhanDoiDiaChi();
                pa.DiaChiCu = p.Diachi;
                pa.ID_BenhNhan = ID_BenhNhan;
                pa.NgayTao = DateTime.Now;
                pa.CapNhap = 0;

                List<SqlParameter> parameterList = new List<SqlParameter>();

                string sql = "INSERT INTO dbo.HTDT_BenhNhanDoiDiaChi (ID_BenhNhan,DiaChiCu,CapNhap,NgayTao)"
                    + " VALUES (@P0,@P1,@P2,@P3)";
                parameterList.Add(new SqlParameter("@P0", (long)req["ID_BenhNhan"]));
                parameterList.Add(new SqlParameter("@P1", p.Diachi));
                parameterList.Add(new SqlParameter("@P2", Convert.ToInt32(0)));
                parameterList.Add(new SqlParameter("@P3", DateTime.Now));
                
                SqlParameter[] parameters = parameterList.ToArray();
                int total = db.Database.ExecuteSqlCommand(sql, parameters);
                result.Add("totalResultsReturned", total);
            }
            else
                result.Add("totalResultsReturned", 0);
            return result;
        }

    }
}