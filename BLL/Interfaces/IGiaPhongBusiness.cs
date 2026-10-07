using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BLL.Interfaces
{
    public partial interface IGiaPhongBusiness
    {
        List<GiaPhong> GetAllGiaPhong();
        bool Create(GiaPhong thongtin);
        bool Update(GiaPhong thongtin);
        bool Delete(string maGia);
        GiaPhong GetGiaPhongById(string maGia);
        GiaPhong GetGiaPhongByMaLoaiPhong(string maLoaiPhong);
        List<GiaPhong> GiaPhong_GetCurrent(string maLoaiPhong, DateTime ngay);
    }
}
