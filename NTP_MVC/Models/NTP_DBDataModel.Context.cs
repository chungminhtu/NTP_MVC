﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class NTP_DBEntities : DbContext
    {
        public NTP_DBEntities()
            : base("name=NTP_DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AD_Menu> AD_Menu { get; set; }
        public virtual DbSet<AD_RoleMenu> AD_RoleMenu { get; set; }
        public virtual DbSet<AD_Roles> AD_Roles { get; set; }
        public virtual DbSet<AD_Users> AD_Users { get; set; }
        public virtual DbSet<DM_BenhVien> DM_BenhVien { get; set; }
        public virtual DbSet<DM_Huyen> DM_Huyen { get; set; }
        public virtual DbSet<DM_KQDieuTri> DM_KQDieuTri { get; set; }
        public virtual DbSet<DM_LoaiBenhVien> DM_LoaiBenhVien { get; set; }
        public virtual DbSet<DM_LyDoNhapXuat> DM_LyDoNhapXuat { get; set; }
        public virtual DbSet<DM_Mien> DM_Mien { get; set; }
        public virtual DbSet<DM_NguonKinhPhi> DM_NguonKinhPhi { get; set; }
        public virtual DbSet<DM_Nuoc> DM_Nuoc { get; set; }
        public virtual DbSet<DM_PhanLoaiBenh> DM_PhanLoaiBenh { get; set; }
        public virtual DbSet<DM_PhanLoaiBenhNhan> DM_PhanLoaiBenhNhan { get; set; }
        public virtual DbSet<DM_PhanLoaiBenhNhan_BC> DM_PhanLoaiBenhNhan_BC { get; set; }
        public virtual DbSet<DM_PhanLoaiYTe> DM_PhanLoaiYTe { get; set; }
        public virtual DbSet<DM_Tinh> DM_Tinh { get; set; }
        public virtual DbSet<DM_Vung> DM_Vung { get; set; }
        public virtual DbSet<SO_BenhNhan> SO_BenhNhan { get; set; }
        public virtual DbSet<SO_PhieuXetNghiem> SO_PhieuXetNghiem { get; set; }
        public virtual DbSet<SO_SoDieuTri> SO_SoDieuTri { get; set; }
        public virtual DbSet<SO_SoKhamBenh> SO_SoKhamBenh { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<DM_Cap> DM_Cap { get; set; }
        public virtual DbSet<DM_DoiTuong> DM_DoiTuong { get; set; }
        public virtual DbSet<DM_DonViTinh> DM_DonViTinh { get; set; }
        public virtual DbSet<SO_PhieuXetNghiem_KQ> SO_PhieuXetNghiem_KQ { get; set; }
        public virtual DbSet<SO_SoDieuTri_KQ> SO_SoDieuTri_KQ { get; set; }
        public virtual DbSet<SO_XetNghiemKhamBenh> SO_XetNghiemKhamBenh { get; set; }
    }
}
