using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTP_MVC.Models.VO
{
    public class BaoCaoGiamSatVO
    {
        public long ID { get; set; }
        public long ID_BenhNhan { get; set; }
        public long ID_SoDieuTri { get; set; }
        public string Diachi { get; set; }
        public int Tuoi { get; set; }
        public decimal LieuLuong_E { get; set; }
        public decimal LieuLuong_H { get; set; }
        public decimal LieuLuong_RH { get; set; }
        public decimal LuongCap_E { get; set; }
        public decimal LuongCap_H { get; set; }
        public decimal LuongCap_RH { get; set; }
        public decimal ConLai_E { get; set; }
        public decimal ConLai_H { get; set; }
        public decimal ConLai_RH { get; set; }
        public string Sodienthoai { get; set; }
        public System.DateTime NgayGiamSat { get; set; }
        public string StrNgayGiamSat { get; set; }
        public System.DateTime NgayCapThuocGanNhat { get; set; }
        public string NhanXetCuaGiamSatVien { get; set; }
        public string DienBienDieuTri { get; set; }
        public string DanDoBenhNhan { get; set; }
        public bool NhapTuMobile { get; set; }
        public System.DateTime NgayNhapBaoCao { get; set; }
    }
}