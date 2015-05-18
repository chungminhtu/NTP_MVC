using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTP_MVC.Models.VO
{
    public class BenhNhanGrid
    {
        public List<BenhNhanVO> patients { get; set; }
        public int total { get; set; }
        public int totalTuChoiNhanTinNhan { get; set; }
        public int totalSoTinNhanUT { get; set; }
        public int totalSoTinNhanXN { get; set; }
        public int totalSoTinNhanTruyenThong { get; set; }
    }
}