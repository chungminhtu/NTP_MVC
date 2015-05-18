using NTP_MVC.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NTP_MVC.Job
{
    public class HTDTDrugReminderJob : IJob
    {
        NTP_DBEntities db = new NTP_DBEntities();
        public void Execute(IJobExecutionContext context)
        {
            string provinceId = "45";
            DoReminder(provinceId, "4501");
            DoReminder(provinceId, "4502");
        }

        public void DoReminder(string provinceId, string districtId)
        {
            string sql = "SELECT tk.ID FROM dbo.HTDT_ThongKeTinNhan tk "
                + " WHERE tk.MA_HUYEN=@P0 AND convert(date, DATEADD(day, -1, getdate()))=convert(date,tk.Ngay) ";
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            SqlParameter[] parameters = parameterList.ToArray();
            long ID = db.Database.SqlQuery<long>(sql, parameters).SingleOrDefault();

            sql = "SELECT * "
                + " FROM dbo.HTDT_BenhNhanNhanTinNhan ut "
                + " WHERE ut.LoaiTinNhan=" + HTDTGS.LOAI_TIN_NHAN_NHAC_UONG_THUOC +" AND convert(date, getdate())=convert(date,ut.NgayGui) AND ut.MA_HUYEN=@P0 ";
            List<SqlParameter> parameterList2 = new List<SqlParameter>();
            parameterList2.Add(new SqlParameter("@P0", districtId));
            SqlParameter[] parameters2 = parameterList2.ToArray();
            List<HTDT_BenhNhanNhanTinNhan> patients = db.Database.SqlQuery<HTDT_BenhNhanNhanTinNhan>(sql, parameters2).ToList();

            if (patients != null && patients.Count > 0)
            {
                int TongSoTinNhan = 0;
			    string SQLUpdateTinNhan = "";
                HTDT_BenhNhanNhanTinNhan p = null;
                for (int i = 0; i < patients.Count;i++ )
                {
                    p = patients[i];
                    TongSoTinNhan++;
                    SQLUpdateTinNhan += p.ID + ",";
                }

                if (ID > 0)
                {
                    sql = "UPDATE dbo.HTDT_ThongKeTinNhan SET SoTinNhanUongThuoc=@P0 WHERE ID=@P1";
                    List<SqlParameter> parameterList3 = new List<SqlParameter>();
                    parameterList3.Add(new SqlParameter("@P0", TongSoTinNhan));
                    parameterList3.Add(new SqlParameter("@P1", ID));
                    SqlParameter[] parameters3 = parameterList3.ToArray();
                    db.Database.ExecuteSqlCommand(sql, parameters3);
                }

                if (SQLUpdateTinNhan.Length > 0)
                {
                    SQLUpdateTinNhan = SQLUpdateTinNhan.Substring(0, SQLUpdateTinNhan.Length - 1);
                    sql = "UPDATE dbo.HTDT_BenhNhanNhanTinNhan SET GuiThanhCong=1 WHERE ID IN (" + SQLUpdateTinNhan + ")";
                    db.Database.ExecuteSqlCommand(sql);
                }
            }

        }
    }
}