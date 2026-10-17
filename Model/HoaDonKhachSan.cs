using System;
using System.Collections.Generic;

namespace Model;

public partial class HoaDonKhachSan
{
    public string Ma_Hoa_Don { get; set; } = null!;

    public string Ma_Dat_Phong { get; set; } = null!;

    public DateTime? Ngay_Lap { get; set; }

    public double Tong_Tien_Phong { get; set; }

    public double? Tong_Tien_Dich_Vu { get; set; }

    public double? Thue_Vat { get; set; }

    public double Tong_Thanh_Toan { get; set; }

    public string? Phuong_Thuc_Thanh_Toan { get; set; }

    //public virtual DatPhong MaDatPhongNavigation { get; set; } = null!;
}
