//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NTP_MVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SO_BenhNhan
    {
        public long ID_BenhNhan { get; set; }
        public int ID_Doituong { get; set; }
        public string HoTen { get; set; }
        public string MaBNQL { get; set; }
        public string SoCMND { get; set; }
        public Nullable<int> Tuoi { get; set; }
        public Nullable<bool> Gioitinh { get; set; }
        public string MA_TINH { get; set; }
        public string MA_HUYEN { get; set; }
        public string MA_XA { get; set; }
        public string Diachi { get; set; }
        public string Sodienthoai { get; set; }
        public Nullable<System.DateTime> NGAY_NM { get; set; }
        public Nullable<int> NGUOI_NM { get; set; }
        public Nullable<System.DateTime> Ngay_SD { get; set; }
        public string Nguoi_SD { get; set; }
        public Nullable<bool> Huy { get; set; }
        public Nullable<bool> HuyenXN { get; set; }
        public bool TinhXN { get; set; }
        public Nullable<decimal> STT { get; set; }
        public Nullable<decimal> Namsinh { get; set; }
        public Nullable<byte> HIV { get; set; }
        public string BHYT { get; set; }
        public string NgheNghiep { get; set; }
        public string NoiGioiThieu { get; set; }
        public Nullable<int> MaNoiGioiThieu { get; set; }
    
        public virtual DM_Huyen DM_Huyen { get; set; }
    }
}
