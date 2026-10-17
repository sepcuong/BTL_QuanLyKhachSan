using BLL.Interfaces;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class HoaDonBusiness : IHoaDonBusiness
    {
        private IHoaDonRepository _res;
        public HoaDonBusiness(IHoaDonRepository res)
        {
            _res = res;
        }

        public List<HoaDonKhachSan> GetAllHoaDon()
        {
            return _res.GetAllHoaDon();
        }

        public HoaDonKhachSan GetHoaDonById(string maHoaDon)
        {
            return _res.GetHoaDonById(maHoaDon);
        }

        public HoaDonKhachSan GetByDatPhong(string maDatPhong)
        {
            return _res.GetByDatPhong(maDatPhong);
        }

        public List<HoaDonKhachSan> Search(string keyword)
        {
            return _res.Search(keyword);
        }

        public bool Create(HoaDonKhachSan thongtin)
        {
            if (string.IsNullOrEmpty(thongtin.Ma_Hoa_Don))
            {
                thongtin.Ma_Hoa_Don = Guid.NewGuid().ToString();
            }
            return _res.Create(thongtin);
        }

        public bool Update(HoaDonKhachSan thongtin)
        {
            return _res.Update(thongtin);
        }

        public bool Delete(string maHoaDon)
        {
            return _res.Delete(maHoaDon);
        }
    }
}
