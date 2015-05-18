using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTP_MVC.Models.VO
{
    public class BenhNhanGS
    {
        public long ID_BenhNhan { get; set; }
        public long ID_SoDieuTri { get; set; }
        public string HoTen { get; set; }
        public int Tuoi { get; set; }
        public bool Gioitinh { get; set; }
        public string TEN_XA { get; set; }
        public string Diachi { get; set; }
        public string Sodienthoai { get; set; }
        public DateTime NgayDT { get; set; }
        public byte ID_PhacDoDT { get; set; }
        public int ID_PhanLoaiBenh { get; set; }
        public Nullable<int> SoLanGiamSat { get; set; }
    }
}