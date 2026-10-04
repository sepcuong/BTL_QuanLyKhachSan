using System;
using System.Collections.Generic;

namespace Model;

public partial class LoaiPhong
{
    public string Ma_Loai_Phong { get; set; } = null!;

    public string Ten_Loai_Phong { get; set; } = null!;

    public double Gia_Mac_Dinh { get; set; }

    public int? So_Nguoi_Chuan { get; set; }

    public string? Mo_Ta { get; set; }

    //public virtual ICollection<GiaPhong> GiaPhongs { get; set; } = new List<GiaPhong>();

    //public virtual ICollection<Phong> Phongs { get; set; } = new List<Phong>();
}
