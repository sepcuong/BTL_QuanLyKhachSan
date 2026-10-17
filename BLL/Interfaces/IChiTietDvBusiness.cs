using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BLL.Interfaces
{
    public interface IChiTietDvBusiness
    {
        List<ChiTietSuDungDv> GetAllChiTietSuDungDv();
        ChiTietSuDungDv GetChiTietDvById(string maChiTietDv);
        List<ChiTietSuDungDv> GetByDatPhong(string maDatPhong);
        bool Create(ChiTietSuDungDv thongtin);
        bool Update(ChiTietSuDungDv thongtin);
        bool Delete(string maChiTietDv);
    }
}
