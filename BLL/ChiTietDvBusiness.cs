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
    public class ChiTietDvBusiness : IChiTietDvBusiness
    {
        private IChiTietDvRepository _res;
        public ChiTietDvBusiness(IChiTietDvRepository res)
        {
            _res = res;
        }

        public List<ChiTietSuDungDv> GetAllChiTietSuDungDv()
        {
            return _res.GetAllChiTietSuDungDv();
        }

        public ChiTietSuDungDv GetChiTietDvById(string maChiTietDv)
        {
            return _res.GetChiTietDvById(maChiTietDv);
        }

        public List<ChiTietSuDungDv> GetByDatPhong(string maDatPhong)
        {
            return _res.GetByDatPhong(maDatPhong);
        }

        public bool Create(ChiTietSuDungDv thongtin)
        {
            if (string.IsNullOrEmpty(thongtin.Ma_Chi_Tiet_Dv))
            {
                thongtin.Ma_Chi_Tiet_Dv = Guid.NewGuid().ToString();
            }
            return _res.Create(thongtin);
        }

        public bool Update(ChiTietSuDungDv thongtin)
        {
            return _res.Update(thongtin);
        }

        public bool Delete(string maChiTietDv)
        {
            return _res.Delete(maChiTietDv);
        }
    }
}
