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
    
    public partial class NTP_QLT_KY_KIEMKE
    {
        public NTP_QLT_KY_KIEMKE()
        {
            this.NTP_QLT_KIEMKE = new HashSet<NTP_QLT_KIEMKE>();
            this.NTP_QLT_KHOTHUOC = new HashSet<NTP_QLT_KHOTHUOC>();
        }
    
        public long ID_KY_KIEMKE { get; set; }
        public string TEN_KY_KIEMKE { get; set; }
        public long ID_KHO { get; set; }
        public Nullable<System.DateTime> NGAY_BATDAU { get; set; }
        public Nullable<System.DateTime> NGAY_KETTHUC { get; set; }
        public int TRANG_THAI { get; set; }
        public int THANG { get; set; }
        public int NAM { get; set; }
    
        public virtual ICollection<NTP_QLT_KIEMKE> NTP_QLT_KIEMKE { get; set; }
        public virtual ICollection<NTP_QLT_KHOTHUOC> NTP_QLT_KHOTHUOC { get; set; }
    }
}
