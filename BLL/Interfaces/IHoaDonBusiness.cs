using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BLL.Interfaces
{
    public interface IHoaDonBusiness
    {
        List<HoaDonKhachSan> GetAllHoaDon();
        HoaDonKhachSan GetHoaDonById(string maHoaDon);
        HoaDonKhachSan GetByDatPhong(string maDatPhong);
        List<HoaDonKhachSan> Search(string keyword);
        bool Create(HoaDonKhachSan thongtin);
        bool Update(HoaDonKhachSan thongtin);
        bool Delete(string maHoaDon);
    }
}
