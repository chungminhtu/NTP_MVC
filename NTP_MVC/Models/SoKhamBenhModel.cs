using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTP_MVC.Models
{
    public class SoDieuTriModel
    {
        public SO_SoDieuTri SDT { get; set; }
        public SO_BenhNhan BN { get; set; }
        public SO_PhieuXetNghiem PXN { get; set; }

        public List<SO_BenhNhan> ListBN { get; set; }

        public List<SO_SoDieuTri> ListSDT { get; set; }
        public List<SO_PhieuXetNghiem> ListPXN { get; set; }
        public List<SO_PhieuXetNghiem_KQ> ListKQXN { get; set; }
    }
}