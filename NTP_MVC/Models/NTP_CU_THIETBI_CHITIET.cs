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
    
    public partial class NTP_CU_THIETBI_CHITIET
    {
        public decimal ID_CHITIET { get; set; }
        public decimal ID_BAOCAO { get; set; }
        public string ID_KHO { get; set; }
        public int TYPE { get; set; }
        public int ID_THIETBI { get; set; }
        public int ID_NGUONKINHPHI { get; set; }
        public string ID_MATINH { get; set; }
        public decimal TD_SOLUONG { get; set; }
        public Nullable<decimal> N_SOLUONG { get; set; }
        public Nullable<decimal> N_NAM { get; set; }
        public decimal XSD_TINH_SOLUONG { get; set; }
        public decimal XSD_TINH_NAM { get; set; }
        public decimal XSD_HUYEN_SOLUONG { get; set; }
        public decimal XSD_HUYEN_NAM { get; set; }
        public decimal HONG_TINH_SOLUONG { get; set; }
        public decimal HONG_TINH_NAM { get; set; }
        public decimal HONG_HUYEN_SOLUONG { get; set; }
        public decimal HONG_HUYEN_NAM { get; set; }
        public decimal CHO_THANHLY { get; set; }
        public decimal DA_THANHLY { get; set; }
        public string GHI_CHU { get; set; }
        public Nullable<System.DateTime> NGAY_NM { get; set; }
        public Nullable<int> NGUOI_NM { get; set; }
        public decimal TC_SOLUONG { get; set; }
    
        public virtual NTP_CU_THIETBI NTP_CU_THIETBI { get; set; }
    }
}
