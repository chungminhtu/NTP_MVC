using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTP_MVC.Models.VO
{
    public class BenhNhanVO
    {
        public long ID { get; set; }
        public long ID_BenhNhan { get; set; }
        public string HoTen { get; set; }
        public string Sodienthoai { get; set; }
        public string Diachi { get; set; }
        public int Tuoi { get; set; }
        public bool Gioitinh { get; set; }
        public string MA_TINH { get; set; }
        public string MA_HUYEN { get; set; }
        public short LanXN { get; set; }
        public DateTime NgayXN { get; set; }
        public DateTime NgayDT { get; set; }
        public byte ID_PhacDoDT { get; set; }
        public int SoNgayTreHen { get; set; }
        public int ID_PhanLoaiBenh { get; set; }
        public short DaXN { get; set; }
        public string TEN_XA { get; set; }

        public int SoTinNhanUT { get; set; }
        public int SoTinNhanXN { get; set; }
        public int SoTinNhanGD { get; set; }
        public Nullable<bool> GuiThanhCong { get; set; }
        public Nullable<bool> PhanHoi { get; set; }
        public Nullable<DateTime> NgayPhanHoiGanNhat { get; set; }

        public string TacDungPhu { get; set; }
        public DateTime NgayNhanTinTacDungPhu { get; set; }

        public Nullable<int> SoLanGiamSat { get; set; }

        public DateTime NgayTaoBaoCaoDoiDiaChi { get; set; }
        public short CapNhapDiaChi { get; set; }

        public string LyDoTuChoi { get; set; }

        public Nullable<DateTime> NgayTuChoi { get; set; }
    }
}