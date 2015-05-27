using NghiepCT.Utilities.Security;
using NTP_MVC.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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

        private const string PARTNER_CODE = "126fa5f1-6e98-11e2-8420-18a90565e6c8";
        private AdvertMessage.FO_API_Advert.Service m_WSInstance = null;
        private const string WebAppDir = "C:/inetpub/wwwroot/ntp";
        private AdvertMessage.FO_API_Advert.Service WSInstance
        {
            get
            {
                if (m_WSInstance == null)
                {
                    m_WSInstance = new AdvertMessage.FO_API_Advert.Service();
                }

                return m_WSInstance;
            }
        }
        private string GenerateSignature(string Message, string Phone)
        {
            string OriginalData = string.Empty;
            string CertFilePath = WebAppDir + "/Content/HTDT_TemplateFiles/SMS.pfx";
            string CertPassword = "smsgatevn";
            OriginalData = string.Format("{0}{1}{2}", PARTNER_CODE, Message, Phone);
            return DigitalSignature.RSASignData(CertFilePath, CertPassword, OriginalData);
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

                    int MessageCode = 0;

                    bool IsSendNow = true;

                    string Telco = "viettel";
                    string Phone = "84915164626";
                    string ServiceNum = "8100";

                    string Message = "Test gui tu vitimes chuong trinh Lao QG";

                    string Signature = GenerateSignature(Message, Phone);

                    string EncryptMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(Message));

                    DateTime DateSend = DateTime.Parse("2015-05-27 15:00:00"); // yyyy-mm-dd HH:MM:ss

                    int Result = WSInstance.Send(Telco
                                                 , Phone
                                                 , ServiceNum
                                                 , MessageCode
                                                 , EncryptMessage
                                                 , PARTNER_CODE
                                                 , Signature
                                                 , DateSend
                                                 , IsSendNow);

                    if (Result == 0)
                    {
                        // Gui thanh cong.
                    }

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