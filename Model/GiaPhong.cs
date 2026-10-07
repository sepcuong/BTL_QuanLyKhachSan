using System;
using System.Collections.Generic;

namespace Model;

public partial class GiaPhong
{
    public string Ma_Gia { get; set; } = null!;

    public string Ma_Loai_Phong { get; set; } = null!;

    public string Ten_Chinh_Sach { get; set; } = null!;

    public DateOnly Tu_Ngay { get; set; }

    public DateOnly Den_Ngay { get; set; }

    public double Gia_Theo_Dem { get; set; }

    public double Gia_Theo_Gio { get; set; }

    //public virtual LoaiPhong MaLoaiPhongNavigation { get; set; } = null!;
}
