using NTP_MVC.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NTP_MVC.Job
{
    public class HTDTPreparePatientReminder : IJob
    {
        NTP_DBEntities db = new NTP_DBEntities();
        public void Execute(IJobExecutionContext context)
        {
            string provinceId = "45";
            GeneratePatientDrugReminder(provinceId, "4501");
            GeneratePatientDrugReminder(provinceId, "4502");
            GeneratePatientExamReminder(provinceId, "4501");
            GeneratePatientExamReminder(provinceId, "4502");
            CreateSMSStatistic("4501");
            CreateSMSStatistic("4502");
        }

        public void GeneratePatientDrugReminder(string provinceId, string districtId)
        {
            string sql = "SELECT CONCAT('('''," + districtId + ",''',',bn.ID_BENHNHAN,',',sdt.ID_SoDieuTri,',''',getdate(),''',''',bn.Sodienthoai,''',',0,',','1)') "
                + " FROM dbo.SO_BenhNhan bn, dbo.SO_SoDieuTri sdt "
                + " WHERE bn.MA_HUYEN=@P0 AND (bn.Huy IS NULL OR bn.Huy=0) AND bn.Sodienthoai IS NOT NULL AND bn.Sodienthoai != '' "
                + " AND datepart(dw,getdate()) IN (2,4,6) AND sdt.ID_BENHNHAN=bn.ID_BenhNhan  "
                + " AND sdt.NgayRV IS NULL AND (sdt.Huy IS NULL OR sdt.HUY=0) AND (sdt.ID_KetquaDT IS NULL OR sdt.ID_KetquaDT=0) "
                + " AND ((sdt.ID_PhacdoDT IN (4,8) AND DATEADD(day, 240, sdt.NgayDT) > getdate() AND DATEADD(day, 60, sdt.NgayDT) < getdate()) "
                + " OR (sdt.ID_PhacdoDT IN (1,2) AND DATEADD(day, 240, sdt.NgayDT) > getdate()) "
                + " OR (sdt.ID_PhacdoDT IN (5,6,7) AND DATEADD(day, 180, sdt.NgayDT) > getdate() AND DATEADD(day, 60, sdt.NgayDT) < getdate()) "
                + " OR (sdt.ID_PhacdoDT=3 AND DATEADD(day, 180, sdt.NgayDT) > getdate() AND DATEADD(day, 60, sdt.NgayDT) < getdate())) "
                + " group by sdt.ID_BENHNHAN,bn.ID_BenhNhan,sdt.ID_SoDieuTri,bn.Sodienthoai ";
            
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            SqlParameter[] parameters = parameterList.ToArray();
            List<String> listReminders = db.Database.SqlQuery<String>(sql, parameters).ToList();
            String reminders = "";
            if (listReminders != null && listReminders.Count > 0)
            {
                int uc = 0;
                int maxuc = 0;
                while (uc < listReminders.Count)
                {
                    reminders = "";
                    maxuc = listReminders.Count() > uc + 50 ? uc + 50 : listReminders.Count();
                    for (int uci = uc; uci < maxuc; uci++)
                    {
                        reminders += listReminders[uci] + ",";
                    }
                    reminders = reminders.Substring(0, reminders.Length - 1);

                    sql = "INSERT INTO dbo.HTDT_BenhNhanNhanTinNhan (MA_HUYEN,ID_BenhNhan,ID_SoDieuTri,NgayGui,Sodienthoai,GuiThanhCong,LoaiTinNhan) VALUES " + reminders;
                    
                    db.Database.ExecuteSqlCommand(sql);
                    uc += 50;
                }
            } 
        }

        public void GeneratePatientExamReminder(string provinceId, string districtId)
        {
            string sql = "SELECT CONCAT('('''," + districtId + ",''',',xn.ID_BenhNhan,',',xn.ID_SoDieuTri,',''',getdate(),''',''',bn.Sodienthoai,''',',0,',','3)') "
                + " FROM dbo.SO_BenhNhan bn, dbo.SO_SoDieuTri sdt, dbo.HTDT_LichHenXN xn "
                + " WHERE bn.MA_HUYEN=@P0 AND (bn.Huy IS NULL OR bn.Huy=0) AND bn.Sodienthoai IS NOT NULL AND bn.Sodienthoai != '' "
                + " AND xn.ID_BenhNhan=bn.ID_BenhNhan AND xn.ID_SoDieuTri=sdt.ID_SoDieuTri AND sdt.ID_BENHNHAN=bn.ID_BenhNhan "
                + " AND sdt.NgayRV IS NULL AND (sdt.Huy IS NULL OR sdt.HUY=0) AND (sdt.ID_KetquaDT IS NULL OR sdt.ID_KetquaDT=0) "
                + " AND ((xn.DaXN=2 AND DATEADD(day, -3, convert(date, getdate())) >= xn.NgayXN) "
                + " OR (xn.DaXN=0 AND DATEADD(day, 4, convert(date, getdate())) = xn.NgayXN)) "
                + " group by bn.Sodienthoai,xn.ID_BenhNhan,xn.ID_SoDieuTri ";
            
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));
            SqlParameter[] parameters = parameterList.ToArray();
            List<String> listReminders = db.Database.SqlQuery<String>(sql, parameters).ToList();
            String reminders = "";
            if (listReminders != null && listReminders.Count > 0)
            {
                int uc = 0;
                int maxuc = 0;
                while (uc < listReminders.Count)
                {
                    reminders = "";
                    maxuc = listReminders.Count() > uc + 50 ? uc + 50 : listReminders.Count();
                    for (int uci = uc; uci < maxuc; uci++)
                    {
                        reminders += listReminders[uci] + ",";
                    }
                    reminders = reminders.Substring(0, reminders.Length - 1);

                    sql = "INSERT INTO dbo.HTDT_BenhNhanNhanTinNhan (MA_HUYEN,ID_BenhNhan,ID_SoDieuTri,NgayGui,Sodienthoai,GuiThanhCong,LoaiTinNhan) VALUES " + reminders;
                    
                    db.Database.ExecuteSqlCommand(sql);
                    uc += 50;
                }
            }
        }

        public void CreateSMSStatistic(string districtId)
        {
            string sql = "INSERT INTO dbo.HTDT_ThongKeTinNhan (Ngay,SoTinNhanUongThuoc,SoTinNhanTaiKham,SoTinNhanGuiKhac,SoTinNhanPhanHoiUT, "
                        + " SoTinNhanTacDungPhu,MA_HUYEN) VALUES (@P0,@P1,@P2,@P3,@P4,@P5,@P6)";
            
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", DateTime.Now));
            parameterList.Add(new SqlParameter("@P1", Convert.ToInt32(0)));
            parameterList.Add(new SqlParameter("@P2", Convert.ToInt32(0)));
            parameterList.Add(new SqlParameter("@P3", Convert.ToInt32(0)));
            parameterList.Add(new SqlParameter("@P4", Convert.ToInt32(0)));
            parameterList.Add(new SqlParameter("@P5", Convert.ToInt32(0)));
            parameterList.Add(new SqlParameter("@P6", districtId));
            SqlParameter[] parameters = parameterList.ToArray();
            int result = db.Database.ExecuteSqlCommand(sql, parameters);
        }
    }
}