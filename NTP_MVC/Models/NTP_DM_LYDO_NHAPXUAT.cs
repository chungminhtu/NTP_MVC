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
    
    public partial class NTP_DM_LYDO_NHAPXUAT
    {
        public NTP_DM_LYDO_NHAPXUAT()
        {
            this.NTP_QLTTB_PHIEUNHAP = new HashSet<NTP_QLTTB_PHIEUNHAP>();
            this.NTP_QLTTB_PHIEUXUAT = new HashSet<NTP_QLTTB_PHIEUXUAT>();
            this.NTP_QLVT_PHIEUNHAP = new HashSet<NTP_QLVT_PHIEUNHAP>();
            this.NTP_QLVT_PHIEUXUAT = new HashSet<NTP_QLVT_PHIEUXUAT>();
        }
    
        public int ID { get; set; }
        public string MO_TA { get; set; }
        public string TYPE { get; set; }
    
        public virtual ICollection<NTP_QLTTB_PHIEUNHAP> NTP_QLTTB_PHIEUNHAP { get; set; }
        public virtual ICollection<NTP_QLTTB_PHIEUXUAT> NTP_QLTTB_PHIEUXUAT { get; set; }
        public virtual ICollection<NTP_QLVT_PHIEUNHAP> NTP_QLVT_PHIEUNHAP { get; set; }
        public virtual ICollection<NTP_QLVT_PHIEUXUAT> NTP_QLVT_PHIEUXUAT { get; set; }
    }
}
