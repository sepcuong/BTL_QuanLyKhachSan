using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL
{
    public interface IKhachHangRepository
    {
        List<KhachHang> GetAllKhachHang();
        bool Create(KhachHang thongtin);
        bool Update(KhachHang thongtin);
        bool Delete(string maKhachHang);
        KhachHang GetById(string maKhachHang);
        List<KhachHang> Search(string keyword);

    }
}
