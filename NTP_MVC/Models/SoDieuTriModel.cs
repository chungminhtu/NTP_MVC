using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTP_MVC.Models
{
    public class SoKhamBenhModel
    {
        public SO_SoKhamBenh SKB { get; set; }
        public SO_BenhNhan BN { get; set; }
        public SO_PhieuXetNghiem PXN { get; set; }

        public List<SO_BenhNhan> ListBN { get; set; }

        public List<SO_SoKhamBenh> ListSKB { get; set; }
        public List<SO_PhieuXetNghiem> ListPXN { get; set; }
        public List<SO_PhieuXetNghiem_KQ> ListKQXN { get; set; }
    }
}