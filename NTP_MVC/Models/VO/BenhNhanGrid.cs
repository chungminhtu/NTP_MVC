﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTP_MVC.Models.VO
{
    public class BenhNhanGrid
    {
        public List<BenhNhanVO> patients { get; set; }
        public int total { get; set; }
        public Nullable<int> totalTuChoiNhanTinNhan { get; set; }
        public Nullable<int> totalSoTinNhanUT { get; set; }
        public Nullable<int> totalSoTinNhanXN { get; set; }
        public Nullable<int> totalSoTinNhanTruyenThong { get; set; }
    }
}