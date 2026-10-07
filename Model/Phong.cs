using System;
using System.Collections.Generic;

namespace Model;

public partial class Phong
{
    public string Ma_Phong { get; set; } = null!;

    public string Ma_Loai_Phong { get; set; } = null!;

    public string Ten_Phong { get; set; } = null!;

    public int So_Tang { get; set; }

    public string? Trang_Thai { get; set; }

    //public virtual ICollection<DatPhong> DatPhongs { get; set; } = new List<DatPhong>();

    //public virtual LoaiPhong MaLoaiPhongNavigation { get; set; } = null!;
}
