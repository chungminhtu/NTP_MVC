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
    
    public partial class NTP_QLTTB_DM_KHO
    {
        public NTP_QLTTB_DM_KHO()
        {
            this.NTP_QLTTB_KHO = new HashSet<NTP_QLTTB_KHO>();
            this.NTP_QLTTB_KY_KIEMKE_CHITIET = new HashSet<NTP_QLTTB_KY_KIEMKE_CHITIET>();
            this.NTP_QLTTB_PHIEUNHAP = new HashSet<NTP_QLTTB_PHIEUNHAP>();
            this.NTP_QLTTB_PHIEUXUAT = new HashSet<NTP_QLTTB_PHIEUXUAT>();
            this.NTP_QLTTB_PHIEUXUAT1 = new HashSet<NTP_QLTTB_PHIEUXUAT>();
        }
    
        public long ID_KHO { get; set; }
        public string MA_KHO { get; set; }
        public string TEN_KHO { get; set; }
        public Nullable<int> ID_VUNG { get; set; }
        public Nullable<long> ID_KHO_CAPTREN { get; set; }
        public Nullable<int> ID_MIEN { get; set; }
        public Nullable<int> ID_CAP { get; set; }
        public string MA_TINH { get; set; }
        public string MA_HUYEN { get; set; }
        public string DIA_CHI { get; set; }
    
        public virtual NTP_DM_CAP NTP_DM_CAP { get; set; }
        public virtual NTP_DM_MIEN NTP_DM_MIEN { get; set; }
        public virtual NTP_DM_VUNG NTP_DM_VUNG { get; set; }
        public virtual ICollection<NTP_QLTTB_KHO> NTP_QLTTB_KHO { get; set; }
        public virtual ICollection<NTP_QLTTB_KY_KIEMKE_CHITIET> NTP_QLTTB_KY_KIEMKE_CHITIET { get; set; }
        public virtual ICollection<NTP_QLTTB_PHIEUNHAP> NTP_QLTTB_PHIEUNHAP { get; set; }
        public virtual ICollection<NTP_QLTTB_PHIEUXUAT> NTP_QLTTB_PHIEUXUAT { get; set; }
        public virtual ICollection<NTP_QLTTB_PHIEUXUAT> NTP_QLTTB_PHIEUXUAT1 { get; set; }
    }
}