using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTP_MVC.Models.VO
{
    public class SearchPatientDue
    {
        public string type { get; set; }
        public string provinceId { get; set; }
        public string districtId { get; set; }
        public string communeId { get; set; }
        public string datefilter { get; set; }
        public short statusId { get; set; }
    }
}