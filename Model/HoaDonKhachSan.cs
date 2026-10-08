using System;
using System.Collections.Generic;

namespace Model;

public partial class HoaDonKhachSan
{
    public string MaHoaDon { get; set; } = null!;

    public string MaDatPhong { get; set; } = null!;

    public DateTime? NgayLap { get; set; }

    public double TongTienPhong { get; set; }

    public double? TongTienDichVu { get; set; }

    public double? ThueVat { get; set; }

    public double TongThanhToan { get; set; }

    public string? PhuongThucThanhToan { get; set; }

    //public virtual DatPhong MaDatPhongNavigation { get; set; } = null!;
}
