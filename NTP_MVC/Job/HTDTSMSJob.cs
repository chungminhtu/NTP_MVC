using NghiepCT.Utilities.Security;
using NTP_MVC.Models;
using NTP_MVC.Models.VO;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace NTP_MVC.Job
{
    public class HTDTSMSJob : IJob
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
        private const string WebAppDir = "C:/inetpub/wwwroot/ntp_mvc";
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
           string sql = "select distinct data.ID_BenhNhan,data.Tuan,data.ID_SoDieuTri, data.Sodienthoai, data.ID_PhanLoaiBenh, data.MA_HUYEN from "
                + " (select (DATEDIFF(day,sdt.NgayDT,getdate())/7 + 1) as Tuan, bn.ID_BenhNhan,sdt.NgayDT,sdt.ID_PhacdoDT, "
                + " sdt.ID_SoDieuTri,bn.Sodienthoai,sdt.ID_PhanLoaiBenh,bn.MA_HUYEN "
                + " from so_benhnhan bn, SO_SoDieuTri sdt where bn.ID_BenhNhan=sdt.ID_BENHNHAN and sdt.ID_PhanLoaiBenh IN (1,2,3,4) "
                + " and bn.MA_HUYEN=@P0 and sdt.NgayRV is null and (sdt.ID_KetquaDT is null OR sdt.ID_KetquaDT=0) "
                + " and sdt.NgayDT is not null and bn.Sodienthoai is not null and bn.Sodienthoai != N'0' "
                + " and (sdt.Huy=0 or sdt.Huy is null) and (bn.Huy=0 or bn.Huy is null) "
                + " and DATEDIFF(day,sdt.NgayDT,getdate()) <= 182) data "
                + " where Tuan <= 26 "
                + " order by Tuan asc ";

            List<SqlParameter> parameterList2 = new List<SqlParameter>();
            parameterList2.Add(new SqlParameter("@P0", districtId));
            SqlParameter[] parameters2 = parameterList2.ToArray(); 
            List<SMSPatientVO> patients = db.Database.SqlQuery<SMSPatientVO>(sql, parameters2).ToList();
            
            List<List<HTDT_ThongDiepTruyenThong>> smss = new List<List<HTDT_ThongDiepTruyenThong>>();

            DateTime ClockInfoFromSystem = DateTime.Now;
            int day1 = (int)ClockInfoFromSystem.DayOfWeek;

            for (int i = 1; i <= 26;i++ )
            {
                string sql3 = "SELECT * FROM dbo.HTDT_ThongDiepTruyenThong sms WHERE sms.T" + i + " > 0 ";
                List<HTDT_ThongDiepTruyenThong> sms = db.Database.SqlQuery<HTDT_ThongDiepTruyenThong>(sql3).ToList();
                smss.Add(sms);
            }

            if (patients != null && patients.Count > 0)
            {
                int TongSoTinNhan = 0;

                SMSPatientVO p = null;
                DateTime DateSend = DateTime.Now;
                int MessageCode = 0;
                string ServiceNum = "8100";
                bool IsSendNow = true;
                string Telco = "viettel";
                for (int i = 0; i < patients.Count;i++ )
                {
                    p = patients[i];
                   
                    string Phone = p.Sodienthoai;
                    if (!Phone.StartsWith("0"))
                    {
                        Phone = "84" + Phone;
                    }
                    else
                    {
                        Phone = "84" + Phone.Substring(1, Phone.Length-1);
                    }                   
                    
                    List<HTDT_ThongDiepTruyenThong> psms = smss.ElementAt((int)p.Tuan - 1);
                    int lpsms = psms.Count();
                    
                    bool smsUT = false; int smsUTid = -1; bool smsXN = false;int smsXNid = -1;
                    string Message = "";short LoaiTinNhan = 0;
                    for (int cp = 0; cp < lpsms;cp++ )
                    {
                        if (psms.ElementAt(cp).LoaiTinNhan == 2)
                        {
                            smsUT = true;smsUTid = cp;
                        }
                        if (psms.ElementAt(cp).LoaiTinNhan == 3)
                        {
                            smsXN = true;smsXNid=cp;
                        }
                    }
                    if (p.ID_PhanLoaiBenh == 3 || p.ID_PhanLoaiBenh == 4) {
                        if (smsUT && day1 == 1)
                        {
                            Message = psms.ElementAt(smsUTid).NoiDung;
                        }
                        else if (day1 > 1 && day1 <6)
                        {
                            if (day1 - 1 >= smsUTid)
                            {
                                if (day1 < lpsms && psms.ElementAt(day1).LoaiBenhNhan == 2)
                                {
                                    Message = psms.ElementAt(day1).NoiDung;LoaiTinNhan=2;
                                }
                            }
                            else if (day1 - 1 < lpsms && psms.ElementAt(day1-1).LoaiBenhNhan == 2)
                            {
                                Message = psms.ElementAt(day1 - 1).NoiDung;LoaiTinNhan=psms.ElementAt(day1-1).LoaiTinNhan;
                            }
                            
                        }
                    }
                    else
                    {
                        if (smsXN && day1 == 1 && (p.Tuan == 7 || p.Tuan == 15 || p.Tuan == 23 || p.Tuan == 8 || p.Tuan == 16 || p.Tuan == 24))
                        {
                            Message = psms.ElementAt(smsXNid).NoiDung;LoaiTinNhan=3;
                        }
                        else if (smsXN && day1 == 2 && (p.Tuan == 8 || p.Tuan == 16 || p.Tuan == 24))
                        {
                            Message = psms.ElementAt(smsXNid).NoiDung;LoaiTinNhan=3;
                        } 
                        else if (smsUT && day1 == 1)
                        {
                            Message = psms.ElementAt(smsUTid).NoiDung;LoaiTinNhan=2;
                        }
                        else if (day1 > 1 && day1 < 6)
                        {
                            if (day1 - 1 >= smsUTid || day1 - 1 >= smsXNid)
                            {
                                if (day1 < lpsms)
                                {
                                    Message = psms.ElementAt(day1).NoiDung;LoaiTinNhan=psms.ElementAt(day1).LoaiTinNhan;
                                }
                            }
                            else if (day1 - 1 < lpsms)
                            {
                                Message = psms.ElementAt(day1 - 1).NoiDung;LoaiTinNhan=psms.ElementAt(day1-1).LoaiTinNhan;
                            }

                        }
                    }
                    
                    if (Message.Length > 0) {

                        string Signature = GenerateSignature(Message, Phone);
                       
                        string EncryptMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(Message));
                        
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
                        
                        List<SqlParameter> uparameterList = new List<SqlParameter>();
                        String usql = "INSERT INTO dbo.HTDT_BenhNhanNhanTinNhan (ID_BenhNhan,ID_SoDieuTri,Sodienthoai,MA_HUYEN, "
                            + " NgayGui,LoaiTinNhan,TinNhan,Tuan,ID_PhanLoaiBenh)"
                            + " VALUES (@P0,@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8)";
                        uparameterList.Add(new SqlParameter("@P0", p.ID_BenhNhan));
                        uparameterList.Add(new SqlParameter("@P1", p.ID_SoDieuTri));
                        uparameterList.Add(new SqlParameter("@P2", Phone));
                        uparameterList.Add(new SqlParameter("@P3", p.MA_HUYEN));
                        uparameterList.Add(new SqlParameter("@P4", DateTime.Now));
                        uparameterList.Add(new SqlParameter("@P5", LoaiTinNhan));
                        uparameterList.Add(new SqlParameter("@P6", Message));
                        uparameterList.Add(new SqlParameter("@P7", p.Tuan));
                        uparameterList.Add(new SqlParameter("@P8", p.ID_PhanLoaiBenh));
                        SqlParameter[] uparameters = uparameterList.ToArray();
                        db.Database.ExecuteSqlCommand(usql, uparameters);
                        TongSoTinNhan++;
                    }

      
                    
                }
                string reportPhone = "84915164626";
                string reportMessage = "Da gui " + TongSoTinNhan + " Tin nhan";

                WSInstance.Send(Telco
                                         , reportPhone
                                         , ServiceNum
                                         , MessageCode
                                         , Convert.ToBase64String(Encoding.UTF8.GetBytes(reportMessage))
                                         , PARTNER_CODE
                                         , GenerateSignature(reportMessage, reportPhone)
                                         , DateSend
                                         , IsSendNow);

            }

        }
    }
}
