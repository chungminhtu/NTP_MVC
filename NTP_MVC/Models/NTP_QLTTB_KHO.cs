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
    
    public partial class NTP_QLTTB_KHO
    {
        public long ID_KY_KIEMKE { get; set; }
        public long ID_KHO { get; set; }
        public int ID_THIETBI { get; set; }
        public decimal TON_DAU { get; set; }
        public decimal NHAP { get; set; }
        public decimal XUAT { get; set; }
        public decimal TON_CUOI { get; set; }
        public string NGUOI_NM { get; set; }
        public System.DateTime NGAY_NM { get; set; }
        public decimal THUA { get; set; }
        public decimal THIEU { get; set; }
        public decimal HONG { get; set; }
        public decimal THANH_LY { get; set; }
        public decimal CHO_THANHLY { get; set; }
    
        public virtual NTP_QLTTB_DM_KHO NTP_QLTTB_DM_KHO { get; set; }
        public virtual NTP_QLTTB_DM_THIETBI NTP_QLTTB_DM_THIETBI { get; set; }
        public virtual NTP_QLTTB_KY_KIEMKE NTP_QLTTB_KY_KIEMKE { get; set; }
    }
}
