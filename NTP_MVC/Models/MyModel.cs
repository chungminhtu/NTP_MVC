using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NTP_MVC.Models
{
    [MetadataType(typeof(SO_SoKhamBenhMD))]
    public partial class SO_SoKhamBenh
    {
        public class SO_SoKhamBenhMD
        {

            public long ID_SoKhamBenh { get; set; }
            public long ID_BENHNHAN { get; set; }

            [Display(Name = "Triệu chứng")]
            public string TrieuChung { get; set; }

            [Display(Name = "Kết quả XN đờm")]
            public string KetQuaXetNghiemDom { get; set; }

            [Display(Name = "Kết quả XQ")]
            public string XQ { get; set; }

            [Display(Name = "Ngày chụp XQ")]
            public Nullable<DateTime> NgayChupXQ { get; set; }

            [Display(Name = "Kết quả khác")]
            public string Khac { get; set; }

            [Display(Name = "Nơi giới thiệu")]
            public string NoiGioiThieu { get; set; }

            [Display(Name = "Tổ chống lao")]
            public string ToChongLao { get; set; }

            [Display(Name = "Xử trí")]
            public string XuTri { get; set; }

            [Display(Name = "Bác sỹ")]
            public string BSKhamBenh { get; set; }

            [Display(Name = "Ghi chú")]
            public string Ghichu { get; set; }

            [Display(Name = "Ngày khám bệnh")]
            public Nullable<DateTime> NgayKhamBenh { get; set; }

            [Display(Name = "Huyện xét nghiệm")]
            public Nullable<bool> HuyenKB { get; set; }

            [Display(Name = "Tỉnh xét nghiệm")]
            public Nullable<bool> TinhKB { get; set; }

            [Display(Name = "Đã hủy")]
            public Nullable<bool> HUY { get; set; }

            [Display(Name = "Ngày hủy")]
            public Nullable<DateTime> NGAYHUY { get; set; }

            [Display(Name = "Mã bệnh nhân")]
            public string MaBNQL { get; set; }
        }
    }

    [MetadataType(typeof(SO_BenhNhanMD))]
    public partial class SO_BenhNhan
    {
        public partial class SO_BenhNhanMD
        {
            public long ID_BenhNhan { get; set; }
            public int ID_Doituong { get; set; }
            [Display(Name = "Họ và tên")]
            public string HoTen { get; set; }

            [Display(Name = "Mã bệnh nhân")]
            public string MaBNQL { get; set; }

            [Display(Name = "Số CMND")]
            public string SoCMND { get; set; }

            [Display(Name = "Tuổi")]
            public Nullable<int> Tuoi { get; set; }

            [Display(Name = "Giới tính")]
            public Nullable<bool> Gioitinh { get; set; }

            [Display(Name = "Tỉnh")]
            public string MA_TINH { get; set; }

            [Display(Name = "Huyện")]
            public string MA_HUYEN { get; set; }

            [Display(Name = "Địa chỉ")]
            public string Diachi { get; set; }

            [Display(Name = "Số điện thoại")]
            public string Sodienthoai { get; set; }

            [Display(Name = "Ngày NM")]
            public Nullable<DateTime> NGAY_NM { get; set; }

            [Display(Name = "Triệu chứng")]
            public Nullable<int> NGUOI_NM { get; set; }

            [Display(Name = "Ngày soi đờm")]
            public Nullable<DateTime> Ngay_SD { get; set; }

            [Display(Name = "Người soi đờm")]
            public string Nguoi_SD { get; set; }

            [Display(Name = "Hủy")]
            public Nullable<bool> Huy { get; set; }

            [Display(Name = "Huyện XN")]
            public Nullable<bool> HuyenXN { get; set; }

            [Display(Name = "Tỉnh XN")]
            public bool TinhXN { get; set; }

            [Display(Name = "STT")]
            public Nullable<decimal> STT { get; set; }

            [Display(Name = "Năm sinh")]
            public Nullable<decimal> Namsinh { get; set; }

            [Display(Name = "HIV")]
            public Nullable<byte> HIV { get; set; }

            [Display(Name = "BHYT")]
            public string BHYT { get; set; }

            [Display(Name = "Nghề nghiệp")]
            public string NgheNghiep { get; set; }

            [Display(Name = "Nơi giới thiệu")]
            public string NoiGioiThieu { get; set; }

            [Display(Name = "Mã nơi giới thiệu")]
            public Nullable<int> MaNoiGioiThieu { get; set; }
        }
    }

    [MetadataType(typeof(SO_SoDieuTriMD))]
    public partial class SO_SoDieuTri
    {
        public partial class SO_SoDieuTriMD
        {
            [Display(Name = "ID_SoDieuTri")]
            public decimal ID_SoDieuTri { get; set; }
            [Display(Name = "ID_BENHNHAN")]
            public string ID_BENHNHAN { get; set; }
            [Display(Name = "Số Đăng ký ĐT")]
            public string SoDKDT { get; set; }
            [Display(Name = "Đơn vị ĐT")]
            public string DVDieutri { get; set; }
            [Display(Name = "Ngày vào viện")]
            public Nullable<System.DateTime> NgayVV { get; set; }
            [Display(Name = "Ngày ĐT")]
            public Nullable<System.DateTime> NgayDT { get; set; }
            [Display(Name = "Phân loại y tế")]
            public Nullable<int> ID_PHANLOAIYTE { get; set; }
            [Display(Name = "Đơn vị giới thiệu")]
            public string DVGioiThieu { get; set; }
            [Display(Name = "Phân loại bệnh")] 
            public Nullable<int> ID_PhanLoaiBenh { get; set; }
            [Display(Name = "Phân loại bệnh nhân")]
            public Nullable<int> ID_PhanLoaiBN { get; set; }
            [Display(Name = "Ngày chụp XQ")]
            public Nullable<System.DateTime> NgayChupXQ { get; set; }
            [Display(Name = "Kết quả XQ")]
            public Nullable<byte> KetquaXQ { get; set; }
            [Display(Name = "Trong quý")]
            public Nullable<byte> XNHIV1 { get; set; }
            [Display(Name = "Trong QT ĐT lao")]
            public Nullable<byte> XNHIV2 { get; set; }
            [Display(Name = "Điều trị ART")]
            public Nullable<byte> ART { get; set; }
            [Display(Name = "Điều trị CPT")]
            public Nullable<byte> CPT { get; set; }
            [Display(Name = "CD4/ tổng số lympho")]
            public string LymPho { get; set; }
            [Display(Name = "Người giám sát")]
            public string GiamSatDT { get; set; }
            [Display(Name = "Kết quả điều trị")]
            public Nullable<int> ID_KetquaDT { get; set; }
            [Display(Name = "Ngày ra viện")]
            public Nullable<System.DateTime> NgayRV { get; set; }
            [Display(Name = "Nhận xét")]
            public string Ghichu { get; set; }
            [Display(Name = "Ngày NM")]
            public Nullable<System.DateTime> NGAY_NM { get; set; }
            [Display(Name = "Người NM")]
            public Nullable<int> NGUOI_NM { get; set; }
            [Display(Name = "Ngày SĐ")]
            public Nullable<System.DateTime> Ngay_SD { get; set; }
            [Display(Name = "Người SĐ")]
            public Nullable<int> Nguoi_SD { get; set; }
            [Display(Name = "Huyện XN")]
            public Nullable<bool> HuyenXN { get; set; }
            [Display(Name = "Tỉnh XN")]
            public Nullable<bool> TinhXN { get; set; }
            [Display(Name = "Chuyển")]
            public Nullable<int> ID_DieutriChuyen { get; set; }
            [Display(Name = "Phác đồ điều trị")]
            public Nullable<byte> ID_PhacdoDT { get; set; }
            [Display(Name = "Phác đồ khác")]
            public string PhacdoKhac { get; set; }
            [Display(Name = "Chẩn đoán")]
            public string Chandoan { get; set; }
            [Display(Name = "Hủy")]
            public Nullable<bool> HUY { get; set; }
            [Display(Name = "Ngày Hủy")]
            public Nullable<System.DateTime> NGAYHUY { get; set; }
            [Display(Name = "Mã BNQL")]
            public string MaBNQL { get; set; }
            [Display(Name = "Thay đổi phác đồ")]
            public Nullable<byte> ID_PhacdoTD { get; set; }
            [Display(Name = "Phác đồ thay đổi khác")]
            public string PhacdoTDKhac { get; set; }
            [Display(Name = "Tuổi")]
            public Nullable<int> Tuoi { get; set; }
        }
    }
}