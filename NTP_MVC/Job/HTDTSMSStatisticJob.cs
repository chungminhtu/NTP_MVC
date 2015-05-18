using NTP_MVC.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NTP_MVC.Job
{
    public class HTDTSMSStatisticJob : IJob
    {
        NTP_DBEntities db = new NTP_DBEntities();
        public void Execute(IJobExecutionContext context)
        {
            string provinceId = "45";
            CountSMS(provinceId, "4501");
            CountSMS(provinceId, "4502");
        }

        public void CountSMS(string provinceId, string districtId)
        {
            string sql = "SELECT count(ut.ID) FROM dbo.HTDT_BenhNhanNhanTinNhan ut "
                + " WHERE ut.LoaiTinNhan=1 AND ut.MA_HUYEN=@P0 AND ut.PhanHoi=1 AND convert(date, DATEADD(day, -1, getdate()))=convert(date,ut.NgayPhanHoiGanNhat) ";
            
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@P0", districtId));            
            SqlParameter[] parameters = parameterList.ToArray();
            int SoTinNhanPhanHoiUT = db.Database.SqlQuery<int>(sql, parameters).SingleOrDefault();

            sql = "SELECT count(tc.ID) FROM dbo.HTDT_BenhNhanPhanHoiTacDungPhu tc,dbo.SO_BenhNhan bn "
                + " WHERE tc.ID_BenhNhan=bn.ID_BenhNhan AND bn.MA_HUYEN=@P0 AND convert(date, DATEADD(day, -1, getdate()))=convert(date,tc.NgayNhan) ";
            
            List<SqlParameter> parameterList2 = new List<SqlParameter>();
            parameterList2.Add(new SqlParameter("@P0", districtId));
            SqlParameter[] parameters2 = parameterList2.ToArray();
            int SoTinNhanTacDungPhu = db.Database.SqlQuery<int>(sql, parameters2).SingleOrDefault();

            sql = "SELECT tk.ID FROM dbo.HTDT_ThongKeTinNhan tk "
                + " WHERE tk.MA_HUYEN=@P0 AND convert(date, DATEADD(day, -1, getdate()))=convert(date,tk.Ngay) ";
            
            List<SqlParameter> parameterList4 = new List<SqlParameter>();
            parameterList4.Add(new SqlParameter("@P0", districtId));
            SqlParameter[] parameters4 = parameterList4.ToArray();
            long ID = db.Database.SqlQuery<long>(sql, parameters4).FirstOrDefault();
            
            List<SqlParameter> parameterList3 = new List<SqlParameter>();

            if (ID > 0)
            {
                sql = "UPDATE dbo.HTDT_ThongKeTinNhan SET SoTinNhanPhanHoiUT=@P1,SoTinNhanTacDungPhu=@P2 WHERE ID=@P3";
                
                parameterList3.Add(new SqlParameter("@P1", SoTinNhanPhanHoiUT));
                parameterList3.Add(new SqlParameter("@P2", SoTinNhanTacDungPhu));
                parameterList3.Add(new SqlParameter("@P3", ID));
                
            }
            else
            {
                sql = "INSERT INTO dbo.HTDT_ThongKeTinNhan (Ngay,SoTinNhanUongThuoc,SoTinNhanTaiKham,SoTinNhanGuiKhac,SoTinNhanPhanHoiUT, "
                        + " SoTinNhanTacDungPhu,MA_HUYEN) VALUES (@P0,@P1,@P2,@P3,@P4,@P5,@P6)";
                
                parameterList3.Add(new SqlParameter("@P0", DateTime.Now.AddDays(-1)));
                parameterList3.Add(new SqlParameter("@P1", Convert.ToInt32(0)));
                parameterList3.Add(new SqlParameter("@P2", Convert.ToInt32(0)));
                parameterList3.Add(new SqlParameter("@P3", Convert.ToInt32(0)));
                parameterList3.Add(new SqlParameter("@P4", SoTinNhanPhanHoiUT));
                parameterList3.Add(new SqlParameter("@P5", SoTinNhanTacDungPhu));
                parameterList3.Add(new SqlParameter("@P6", districtId));

            }
            SqlParameter[] parameters3 = parameterList3.ToArray();
            int result = db.Database.ExecuteSqlCommand(sql, parameters3);
        }
    }
}