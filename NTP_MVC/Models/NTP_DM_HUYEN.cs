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
    
    public partial class NTP_DM_HUYEN
    {
        public NTP_DM_HUYEN()
        {
            this.NTP_QLVT_DM_KHO = new HashSet<NTP_QLVT_DM_KHO>();
        }
    
        public string MA_HUYEN { get; set; }
        public string TEN_HUYEN { get; set; }
        public string MA_TINH { get; set; }
    
        public virtual NTP_DM_TINH NTP_DM_TINH { get; set; }
        public virtual ICollection<NTP_QLVT_DM_KHO> NTP_QLVT_DM_KHO { get; set; }
    }
}
