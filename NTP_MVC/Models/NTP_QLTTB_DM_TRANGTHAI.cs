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
    
    public partial class NTP_QLTTB_DM_TRANGTHAI
    {
        public NTP_QLTTB_DM_TRANGTHAI()
        {
            this.NTP_QLTTB_KIEMKE_CHITIET = new HashSet<NTP_QLTTB_KIEMKE_CHITIET>();
        }
    
        public int ID_TRANGTHAI { get; set; }
        public string TEN_TRANGTHAI { get; set; }
    
        public virtual ICollection<NTP_QLTTB_KIEMKE_CHITIET> NTP_QLTTB_KIEMKE_CHITIET { get; set; }
    }
}
