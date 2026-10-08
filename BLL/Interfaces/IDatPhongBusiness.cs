using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public partial interface IDatPhongBusiness
    {
        List<DatPhong> GetAllDatPhong();
        bool Create(DatPhong thongtin);
        bool Update(DatPhong thongtin);
        bool Delete(string maDatPhong);
        bool Cancel(string maDatPhong);
        bool CheckIn(string maDatPhong);
        bool CheckOut(string maDatPhong);
        List<DatPhong> Search(string keyword);
        DatPhong GetById(string maDatPhong);
        DatPhong GetByKhachHang(string maKhachHang);
        DatPhong GetByPhong(string maPhong);
    }
}
