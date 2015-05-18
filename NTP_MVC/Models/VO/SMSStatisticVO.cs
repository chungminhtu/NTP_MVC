using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTP_MVC.Models.VO
{
    public class SMSStatisticVO
    {
        public List<HTDT_ThongKeTinNhan> smss { get; set; }
        public int total { get; set; }
        public int totalSoTinNhanUT { get; set; }
        public int totalSoTinNhanXN { get; set; }
        public int totalSoTinNhanTruyenThong { get; set; }
        public int totalSoTinNhanPhanHoiUT { get; set; }
        public int totalSoTinNhanTacDungPhu { get; set; }
    }
}