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
    
    public partial class NTP_QLTTB_PHIEUXUAT
    {
        public NTP_QLTTB_PHIEUXUAT()
        {
            this.NTP_QLTTB_PHIEUXUAT_CHITIET = new HashSet<NTP_QLTTB_PHIEUXUAT_CHITIET>();
        }
    
        public decimal ID_PHIEUXUAT { get; set; }
        public string MA_PHIEUXUAT { get; set; }
        public System.DateTime NGAY_XUAT { get; set; }
        public string NGUOI_XUAT { get; set; }
        public long ID_KHOXUAT { get; set; }
        public Nullable<long> ID_KHONHAP { get; set; }
        public int ID_LYDO_NHAPXUAT { get; set; }
        public string GHI_CHU { get; set; }
        public System.DateTime NGAY_NM { get; set; }
        public int NGUOI_NM { get; set; }
        public Nullable<int> ID_PHIEUNHAP { get; set; }
        public Nullable<long> ID_KY_KIEMKE { get; set; }
        public int TRANG_THAI { get; set; }
        public int NAM { get; set; }
        public Nullable<int> THANG { get; set; }
        public Nullable<int> ID_NGUONKINHPHI { get; set; }
    
        public virtual NTP_DM_LYDO_NHAPXUAT NTP_DM_LYDO_NHAPXUAT { get; set; }
        public virtual NTP_QLTTB_DM_KHO NTP_QLTTB_DM_KHO { get; set; }
        public virtual NTP_QLTTB_DM_KHO NTP_QLTTB_DM_KHO1 { get; set; }
        public virtual ICollection<NTP_QLTTB_PHIEUXUAT_CHITIET> NTP_QLTTB_PHIEUXUAT_CHITIET { get; set; }
    }
}
