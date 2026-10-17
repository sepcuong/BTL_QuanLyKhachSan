using System;
using System.Collections.Generic;

namespace Model;

public partial class DichVu
{
    public string Ma_Dich_Vu { get; set; } = null!;

    public string Ten_Dich_Vu { get; set; } = null!;

    public double Don_Gia { get; set; }

    public string? Don_Vi_Tinh { get; set; }

    //public virtual ICollection<ChiTietSuDungDv> ChiTietSuDungDvs { get; set; } = new List<ChiTietSuDungDv>();
}
