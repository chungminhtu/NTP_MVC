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
    
    public partial class NTP_QLTTB_PHIEUXUAT_CHITIET
    {
        public decimal ID_PHIEUXUAT_CHITIET { get; set; }
        public decimal ID_PHIEUXUAT { get; set; }
        public int ID_THIETBI { get; set; }
        public decimal SO_LUONG { get; set; }
    
        public virtual NTP_QLTTB_DM_THIETBI NTP_QLTTB_DM_THIETBI { get; set; }
        public virtual NTP_QLTTB_PHIEUXUAT NTP_QLTTB_PHIEUXUAT { get; set; }
    }
}
