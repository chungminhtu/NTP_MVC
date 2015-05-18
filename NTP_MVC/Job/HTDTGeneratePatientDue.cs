using NTP_MVC.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NTP_MVC.Job
{
    public class HTDTGeneratePatientDue : IJob
    {
        NTP_DBEntities db = new NTP_DBEntities();

        public void Execute(IJobExecutionContext context)
        {
            string provinceId = "45";
            GeneratePatientDues(provinceId,"4501");
            GeneratePatientDues(provinceId, "4502");
        }

        public bool GeneratePatientDues(string provinceId, string districtId)
        {         
            //AFB+ phac do 1, 8 thang
            string sql = "INSERT INTO dbo.HTDT_LichHenXN (ID_BenhNhan,ID_SoDieuTri,NgayXN,LanXN,DaXN) "
                + " SELECT sdt.ID_BENHNHAN,sdt.ID_SoDieuTri,DATEADD(day, 59, sdt.NgayDT),1, CASE WHEN DATEADD(day, 59, sdt.NgayDT) >= current_timestamp THEN 0 ELSE 2 END "
                + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                + " WHERE sdt.ID_PhanLoaiBenh=1 AND (sdt.ID_PhacdoDT=1 OR sdt.ID_PhacdoDT=2) AND sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_HUYEN=@P0"
                + " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL AND sdt.ID_PhacdoDT > 0";
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            SqlParameter[] parameters = parameterList.ToArray();
            int result = db.Database.ExecuteSqlCommand(sql, parameters);
            //if (lastDateDueUpdate != null && lastDateCreatedTreatmentUpdate != null)
            //{
            //    sql += " AND ngay_nm > \"" + lastDateCreatedTreatmentUpdate + "\" AND last_modified >\"" + lastDateDueUpdate + "\"";
            //}

            sql = "INSERT INTO dbo.HTDT_LichHenXN (ID_BenhNhan,ID_SoDieuTri,NgayXN,LanXN,DaXN) "
                + " SELECT sdt.ID_BENHNHAN,sdt.ID_SoDieuTri,DATEADD(day, 149, sdt.NgayDT),2, CASE WHEN DATEADD(day, 149, sdt.NgayDT) >= current_timestamp THEN 0 ELSE 2 END "
                + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                + " WHERE sdt.ID_PhanLoaiBenh=1 AND (sdt.ID_PhacdoDT=1 OR sdt.ID_PhacdoDT=2) AND sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_HUYEN=@P0"
                + " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL AND sdt.ID_PhacdoDT > 0";
            parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            parameters = parameterList.ToArray();
            result = db.Database.ExecuteSqlCommand(sql, parameters);

            sql = "INSERT INTO dbo.HTDT_LichHenXN (ID_BenhNhan,ID_SoDieuTri,NgayXN,LanXN,DaXN) "
                + " SELECT sdt.ID_BENHNHAN,sdt.ID_SoDieuTri,DATEADD(day, 209, sdt.NgayDT),3, CASE WHEN DATEADD(day, 209, sdt.NgayDT) >= current_timestamp THEN 0 ELSE 2 END "
                + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                + " WHERE sdt.ID_PhanLoaiBenh=1 AND (sdt.ID_PhacdoDT=1 OR sdt.ID_PhacdoDT=2) AND sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_HUYEN=@P0"
                + " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL AND sdt.ID_PhacdoDT > 0";
            parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            parameters = parameterList.ToArray();
            result = db.Database.ExecuteSqlCommand(sql, parameters);

            //AFB+ phac do 1, 6 thang
            sql = "INSERT INTO dbo.HTDT_LichHenXN (ID_BenhNhan,ID_SoDieuTri,NgayXN,LanXN,DaXN) "
                + " SELECT sdt.ID_BENHNHAN,sdt.ID_SoDieuTri,DATEADD(day, 59, sdt.NgayDT),1, CASE WHEN DATEADD(day, 59, sdt.NgayDT) >= current_timestamp THEN 0 ELSE 2 END "
                + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                + " WHERE sdt.ID_PhanLoaiBenh=1 AND sdt.ID_PhacdoDT=3 AND sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_HUYEN=@P0"
                + " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL AND sdt.ID_PhacdoDT > 0";
            parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            parameters = parameterList.ToArray();
            result = db.Database.ExecuteSqlCommand(sql, parameters);
            //if (lastDateDueUpdate != null && lastDateCreatedTreatmentUpdate != null)
            //{
            //    sql += " AND ngay_nm > \"" + lastDateCreatedTreatmentUpdate + "\" AND last_modified >\"" + lastDateDueUpdate + "\"";
            //}

            sql = "INSERT INTO dbo.HTDT_LichHenXN (ID_BenhNhan,ID_SoDieuTri,NgayXN,LanXN,DaXN) "
                + " SELECT sdt.ID_BENHNHAN,sdt.ID_SoDieuTri,DATEADD(day, 119, sdt.NgayDT),2, CASE WHEN DATEADD(day, 119, sdt.NgayDT) >= current_timestamp THEN 0 ELSE 2 END "
                + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                + " WHERE sdt.ID_PhanLoaiBenh=1 AND sdt.ID_PhacdoDT=3 AND sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_HUYEN=@P0"
                + " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL AND sdt.ID_PhacdoDT > 0";
            parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            parameters = parameterList.ToArray();
            result = db.Database.ExecuteSqlCommand(sql, parameters);

            sql = "INSERT INTO dbo.HTDT_LichHenXN (ID_BenhNhan,ID_SoDieuTri,NgayXN,LanXN,DaXN) "
                + " SELECT sdt.ID_BENHNHAN,sdt.ID_SoDieuTri,DATEADD(day, 179, sdt.NgayDT),3, CASE WHEN DATEADD(day, 179, sdt.NgayDT) >= current_timestamp THEN 0 ELSE 2 END "
                + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                + " WHERE sdt.ID_PhanLoaiBenh=1 AND sdt.ID_PhacdoDT=3 AND sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_HUYEN=@P0"
                + " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL AND sdt.ID_PhacdoDT > 0";
            parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            parameters = parameterList.ToArray();
            result = db.Database.ExecuteSqlCommand(sql, parameters);

            //AFB+ phac do 1, 6 thang
            sql = "INSERT INTO dbo.HTDT_LichHenXN (ID_BenhNhan,ID_SoDieuTri,NgayXN,LanXN,DaXN) "
                + " SELECT sdt.ID_BENHNHAN,sdt.ID_SoDieuTri,DATEADD(day, 59, sdt.NgayDT),1, CASE WHEN DATEADD(day, 59, sdt.NgayDT) >= current_timestamp THEN 0 ELSE 2 END "
                + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                + " WHERE sdt.ID_PhanLoaiBenh=1 AND (sdt.ID_PhacdoDT=6 OR sdt.ID_PhacdoDT=7) AND sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_HUYEN=@P0"
                + " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL AND sdt.ID_PhacdoDT > 0";
            parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            parameters = parameterList.ToArray();
            result = db.Database.ExecuteSqlCommand(sql, parameters);
            //if (lastDateDueUpdate != null && lastDateCreatedTreatmentUpdate != null)
            //{
            //    sql += " AND ngay_nm > \"" + lastDateCreatedTreatmentUpdate + "\" AND last_modified >\"" + lastDateDueUpdate + "\"";
            //}

            sql = "INSERT INTO dbo.HTDT_LichHenXN (ID_BenhNhan,ID_SoDieuTri,NgayXN,LanXN,DaXN) "
                + " SELECT sdt.ID_BENHNHAN,sdt.ID_SoDieuTri,DATEADD(day, 149, sdt.NgayDT),2, CASE WHEN DATEADD(day, 149, sdt.NgayDT) >= current_timestamp THEN 0 ELSE 2 END "
                + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                + " WHERE sdt.ID_PhanLoaiBenh=1 AND (sdt.ID_PhacdoDT=6 OR sdt.ID_PhacdoDT=7) AND sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_HUYEN=@P0"
                + " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL AND sdt.ID_PhacdoDT > 0";
            parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            parameters = parameterList.ToArray();
            result = db.Database.ExecuteSqlCommand(sql, parameters);

            sql = "INSERT INTO dbo.HTDT_LichHenXN (ID_BenhNhan,ID_SoDieuTri,NgayXN,LanXN,DaXN) "
                + " SELECT sdt.ID_BENHNHAN,sdt.ID_SoDieuTri,DATEADD(day, 179, sdt.NgayDT),3, CASE WHEN DATEADD(day, 179, sdt.NgayDT) >= current_timestamp THEN 0 ELSE 2 END "
                + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                + " WHERE sdt.ID_PhanLoaiBenh=1 AND (sdt.ID_PhacdoDT=6 OR sdt.ID_PhacdoDT=7) AND sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_HUYEN=@P0"
                + " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL AND sdt.ID_PhacdoDT > 0";
            parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            parameters = parameterList.ToArray();
            result = db.Database.ExecuteSqlCommand(sql, parameters);

            //AFB+ phac do 2
            sql = "INSERT INTO dbo.HTDT_LichHenXN (ID_BenhNhan,ID_SoDieuTri,NgayXN,LanXN,DaXN) "
                + " SELECT sdt.ID_BENHNHAN,sdt.ID_SoDieuTri,DATEADD(day, 89, sdt.NgayDT),1, CASE WHEN DATEADD(day, 89, sdt.NgayDT) >= current_timestamp THEN 0 ELSE 2 END "
                + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                + " WHERE sdt.ID_PhanLoaiBenh=1 AND (sdt.ID_PhacdoDT=4 OR sdt.ID_PhacdoDT=8) AND sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_HUYEN=@P0"
                + " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL AND sdt.ID_PhacdoDT > 0";
            parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            parameters = parameterList.ToArray();
            result = db.Database.ExecuteSqlCommand(sql, parameters);
            //if (lastDateDueUpdate != null && lastDateCreatedTreatmentUpdate != null)
            //{
            //    sql += " AND ngay_nm > \"" + lastDateCreatedTreatmentUpdate + "\" AND last_modified >\"" + lastDateDueUpdate + "\"";
            //}

            sql = "INSERT INTO dbo.HTDT_LichHenXN (ID_BenhNhan,ID_SoDieuTri,NgayXN,LanXN,DaXN) "
                + " SELECT sdt.ID_BENHNHAN,sdt.ID_SoDieuTri,DATEADD(day, 149, sdt.NgayDT),2, CASE WHEN DATEADD(day, 149, sdt.NgayDT) >= current_timestamp THEN 0 ELSE 2 END "
                + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                + " WHERE sdt.ID_PhanLoaiBenh=1 AND (sdt.ID_PhacdoDT=4 OR sdt.ID_PhacdoDT=8) AND sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_HUYEN=@P0"
                + " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL AND sdt.ID_PhacdoDT > 0";
            parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            parameters = parameterList.ToArray();
            result = db.Database.ExecuteSqlCommand(sql, parameters);

            sql = "INSERT INTO dbo.HTDT_LichHenXN (ID_BenhNhan,ID_SoDieuTri,NgayXN,LanXN,DaXN) "
                + " SELECT sdt.ID_BENHNHAN,sdt.ID_SoDieuTri,DATEADD(day, 209, sdt.NgayDT),3, CASE WHEN DATEADD(day, 209, sdt.NgayDT) >= current_timestamp THEN 0 ELSE 2 END "
                + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                + " WHERE sdt.ID_PhanLoaiBenh=1 AND (sdt.ID_PhacdoDT=4 OR sdt.ID_PhacdoDT=8) AND sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_HUYEN=@P0"
                + " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL AND sdt.ID_PhacdoDT > 0";
            parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            parameters = parameterList.ToArray();
            result = db.Database.ExecuteSqlCommand(sql, parameters);

            //AFB+ phac do 3
            sql = "INSERT INTO dbo.HTDT_LichHenXN (ID_BenhNhan,ID_SoDieuTri,NgayXN,LanXN,DaXN) "
                + " SELECT sdt.ID_BENHNHAN,sdt.ID_SoDieuTri,DATEADD(day, 59, sdt.NgayDT),1, CASE WHEN DATEADD(day, 59, sdt.NgayDT) >= current_timestamp THEN 0 ELSE 2 END "
                + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                + " WHERE sdt.ID_PhanLoaiBenh=1 AND sdt.ID_PhacdoDT=5 AND sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_HUYEN=@P0"
                + " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL AND sdt.ID_PhacdoDT > 0";
            parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            parameters = parameterList.ToArray();
            result = db.Database.ExecuteSqlCommand(sql, parameters);
            //if (lastDateDueUpdate != null && lastDateCreatedTreatmentUpdate != null)
            //{
            //    sql += " AND ngay_nm > \"" + lastDateCreatedTreatmentUpdate + "\" AND last_modified >\"" + lastDateDueUpdate + "\"";
            //}

            sql = "INSERT INTO dbo.HTDT_LichHenXN (ID_BenhNhan,ID_SoDieuTri,NgayXN,LanXN,DaXN) "
                + " SELECT sdt.ID_BENHNHAN,sdt.ID_SoDieuTri,DATEADD(day, 109, sdt.NgayDT),2, CASE WHEN DATEADD(day, 109, sdt.NgayDT) >= current_timestamp THEN 0 ELSE 2 END "
                + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                + " WHERE sdt.ID_PhanLoaiBenh=1 AND sdt.ID_PhacdoDT=5 AND sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_HUYEN=@P0"
                + " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL AND sdt.ID_PhacdoDT > 0";
            parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            parameters = parameterList.ToArray();
            result = db.Database.ExecuteSqlCommand(sql, parameters);

            sql = "INSERT INTO dbo.HTDT_LichHenXN (ID_BenhNhan,ID_SoDieuTri,NgayXN,LanXN,DaXN) "
                + " SELECT sdt.ID_BENHNHAN,sdt.ID_SoDieuTri,DATEADD(day, 179, sdt.NgayDT),3, CASE WHEN DATEADD(day, 179, sdt.NgayDT) >= current_timestamp THEN 0 ELSE 2 END "
                + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                + " WHERE sdt.ID_PhanLoaiBenh=1 AND sdt.ID_PhacdoDT=5 AND sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_HUYEN=@P0"
                + " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL AND sdt.ID_PhacdoDT > 0";
            parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            parameters = parameterList.ToArray();
            result = db.Database.ExecuteSqlCommand(sql, parameters);

            //AFB-
            sql = "INSERT INTO dbo.HTDT_LichHenXN (ID_BenhNhan,ID_SoDieuTri,NgayXN,LanXN,DaXN) "
                + " SELECT sdt.ID_BENHNHAN,sdt.ID_SoDieuTri,DATEADD(day, 59, sdt.NgayDT),1, CASE WHEN DATEADD(day, 59, sdt.NgayDT) >= current_timestamp THEN 0 ELSE 2 END "
                + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                + " WHERE sdt.ID_PhanLoaiBenh=2 AND sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_HUYEN=@P0"
                + " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL AND sdt.ID_PhacdoDT > 0";
            parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            parameters = parameterList.ToArray();
            result = db.Database.ExecuteSqlCommand(sql, parameters);
            //if (lastDateDueUpdate != null && lastDateCreatedTreatmentUpdate != null)
            //{
            //    sql += " AND ngay_nm > \"" + lastDateCreatedTreatmentUpdate + "\" AND last_modified >\"" + lastDateDueUpdate + "\"";
            //}

            sql = "INSERT INTO dbo.HTDT_LichHenXN (ID_BenhNhan,ID_SoDieuTri,NgayXN,LanXN,DaXN) "
                + " SELECT sdt.ID_BENHNHAN,sdt.ID_SoDieuTri,DATEADD(day, 149, sdt.NgayDT),2, CASE WHEN DATEADD(day, 149, sdt.NgayDT) >= current_timestamp THEN 0 ELSE 2 END "
                + " FROM dbo.SO_SoDieuTri sdt, dbo.SO_BenhNhan bn "
                + " WHERE sdt.ID_PhanLoaiBenh=2 AND sdt.ID_BENHNHAN=bn.ID_BenhNhan AND bn.MA_HUYEN=@P0"
                + " AND (bn.Huy=0 OR bn.Huy IS NULL) AND (sdt.Huy=0 OR sdt.Huy IS NULL) AND sdt.NgayDT IS NOT NULL AND sdt.ID_PhacdoDT > 0";
            parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            parameters = parameterList.ToArray();
            result = db.Database.ExecuteSqlCommand(sql, parameters);

            return true;
        }
    }
}