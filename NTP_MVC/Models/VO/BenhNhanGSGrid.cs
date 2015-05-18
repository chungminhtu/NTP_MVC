using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTP_MVC.Models.VO
{
    public class BenhNhanGSGrid
    {
        public List<BenhNhanGS> patients { get; set; }
        public int total { get; set; }
    }
}