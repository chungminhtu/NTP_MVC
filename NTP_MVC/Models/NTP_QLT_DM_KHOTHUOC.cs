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
    
    public partial class NTP_QLT_DM_KHOTHUOC
    {
        public NTP_QLT_DM_KHOTHUOC()
        {
            this.NTP_QLT_KHOTHUOC = new HashSet<NTP_QLT_KHOTHUOC>();
            this.NTP_QLT_PHIEUNHAP = new HashSet<NTP_QLT_PHIEUNHAP>();
            this.NTP_QLT_PHIEUXUAT = new HashSet<NTP_QLT_PHIEUXUAT>();
        }
    
        public long ID_KHO { get; set; }
        public string MA_KHO { get; set; }
        public string TEN_KHO { get; set; }
        public Nullable<int> ID_VUNG { get; set; }
        public Nullable<int> ID_KHO_CAPTREN { get; set; }
        public string DIA_CHI { get; set; }
        public Nullable<int> ID_CAP { get; set; }
        public Nullable<int> ID_MIEN { get; set; }
        public string MA_HUYEN { get; set; }
        public string MA_TINH { get; set; }
    
        public virtual NTP_DM_CAP NTP_DM_CAP { get; set; }
        public virtual NTP_DM_VUNG NTP_DM_VUNG { get; set; }
        public virtual ICollection<NTP_QLT_KHOTHUOC> NTP_QLT_KHOTHUOC { get; set; }
        public virtual ICollection<NTP_QLT_PHIEUNHAP> NTP_QLT_PHIEUNHAP { get; set; }
        public virtual ICollection<NTP_QLT_PHIEUXUAT> NTP_QLT_PHIEUXUAT { get; set; }
    }
}
