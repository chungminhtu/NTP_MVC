using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTP_MVC.Models.VO
{
    public class SMSPatientVO
    {
        public long ID_BenhNhan { get; set; }
        public long ID_SoDieuTri { get; set; }
        public string Sodienthoai { get; set; }
        public string MA_HUYEN { get; set; }

        public int Tuan { get; set; }
        public int ID_PhanLoaiBenh { get; set; }
    }
}
