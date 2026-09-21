using System;
using System.Collections.Generic;

namespace Model;

public partial class KhachHang
{
    public string Ma_Khach_Hang { get; set; } = null!;

    public string Ho_Ten { get; set; } = null!;

    public string? Cmnd_Cccd { get; set; }

    public string? So_Dien_Thoai { get; set; }

    public string? Email { get; set; }

    public string? Dia_Chi { get; set; }

    //public virtual ICollection<DatPhong> DatPhongs { get; set; } = new List<DatPhong>();
}
