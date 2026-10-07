using System;
using System.Collections.Generic;

namespace Model;

public partial class DatPhong
{
    public string Ma_Dat_Phong { get; set; } = null!;

    public string Ma_Khach_Hang { get; set; } = null!;

    public string Ma_Phong { get; set; } = null!;

    public DateTime? Ngay_Dat { get; set; }

    public DateTime Ngay_Nhan_Du_Kien { get; set; }

    public DateTime Ngay_Tra_Du_Kien { get; set; }

    public double? Tien_Coc { get; set; }

    public string? Trang_Thai { get; set; }

    public string? Ghi_Chu { get; set; }

    //public virtual ICollection<ChiTietSuDungDv> ChiTietSuDungDvs { get; set; } = new List<ChiTietSuDungDv>();

    //public virtual ICollection<HoaDonKhachSan> HoaDonKhachSans { get; set; } = new List<HoaDonKhachSan>();

    //public virtual KhachHang MaKhachHangNavigation { get; set; } = null!;

    //public virtual Phong MaPhongNavigation { get; set; } = null!;
}
