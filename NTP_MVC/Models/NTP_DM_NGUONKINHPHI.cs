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
    
    public partial class NTP_DM_NGUONKINHPHI
    {
        public NTP_DM_NGUONKINHPHI()
        {
            this.NTP_QLT_PHIEUNHAP = new HashSet<NTP_QLT_PHIEUNHAP>();
            this.NTP_QLT_PHIEUXUAT = new HashSet<NTP_QLT_PHIEUXUAT>();
            this.NTP_QLTTB_PHIEUNHAP = new HashSet<NTP_QLTTB_PHIEUNHAP>();
            this.NTP_QLVT_PHIEUNHAP = new HashSet<NTP_QLVT_PHIEUNHAP>();
            this.NTP_QLVT_PHIEUXUAT = new HashSet<NTP_QLVT_PHIEUXUAT>();
        }
    
        public int ID_NGUONKINHPHI { get; set; }
        public string MO_TA { get; set; }
    
        public virtual ICollection<NTP_QLT_PHIEUNHAP> NTP_QLT_PHIEUNHAP { get; set; }
        public virtual ICollection<NTP_QLT_PHIEUXUAT> NTP_QLT_PHIEUXUAT { get; set; }
        public virtual ICollection<NTP_QLTTB_PHIEUNHAP> NTP_QLTTB_PHIEUNHAP { get; set; }
        public virtual ICollection<NTP_QLVT_PHIEUNHAP> NTP_QLVT_PHIEUNHAP { get; set; }
        public virtual ICollection<NTP_QLVT_PHIEUXUAT> NTP_QLVT_PHIEUXUAT { get; set; }
    }
}
