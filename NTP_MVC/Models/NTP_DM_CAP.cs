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
    
    public partial class NTP_DM_CAP
    {
        public NTP_DM_CAP()
        {
            this.NTP_QLT_DM_KHOTHUOC = new HashSet<NTP_QLT_DM_KHOTHUOC>();
            this.NTP_QLTTB_DM_KHO = new HashSet<NTP_QLTTB_DM_KHO>();
            this.NTP_QLVT_DM_KHO = new HashSet<NTP_QLVT_DM_KHO>();
        }
    
        public int ID_CAP { get; set; }
        public string TEN_CAP { get; set; }
    
        public virtual ICollection<NTP_QLT_DM_KHOTHUOC> NTP_QLT_DM_KHOTHUOC { get; set; }
        public virtual ICollection<NTP_QLTTB_DM_KHO> NTP_QLTTB_DM_KHO { get; set; }
        public virtual ICollection<NTP_QLVT_DM_KHO> NTP_QLVT_DM_KHO { get; set; }
    }
}
