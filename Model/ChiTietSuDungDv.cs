using System;
using System.Collections.Generic;

namespace Model;

public partial class ChiTietSuDungDv
{
    public string Ma_Chi_Tiet_Dv { get; set; } = null!;

    public string Ma_Dat_Phong { get; set; } = null!;

    public string Ma_Dich_Vu { get; set; } = null!;

    public int So_Luong { get; set; }

    public double Thanh_Tien { get; set; }

    public DateTime? Ngay_Su_Dung { get; set; }

    //public virtual DatPhong MaDatPhongNavigation { get; set; } = null!;

    //public virtual DichVu MaDichVuNavigation { get; set; } = null!;
}
