using BLL.Interfaces;
using DAL;
using Microsoft.Extensions.Configuration;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class GiaPhongBusiness: IGiaPhongBusiness
    {
        private IGiaPhongRepository _res;
        public GiaPhongBusiness(IGiaPhongRepository res, IConfiguration configuration)
        {
            _res = res;
        }
        public List<GiaPhong> GetAllGiaPhong()
        {
            return _res.GetAllGiaPhong();
        }
        public bool Create(GiaPhong thongtin)
        {
            return _res.Create(thongtin);
        }
        public bool Update(GiaPhong thongtin)
        {
            return _res.Update(thongtin);
        }
        public bool Delete(string maGia)
        {
            return _res.Delete(maGia);
        }
        public GiaPhong GetGiaPhongById(string maGia)
        {
            return _res.GetGiaPhongById(maGia);
        }
        public GiaPhong GetGiaPhongByMaLoaiPhong(string maLoaiPhong)
        {
            return _res.GetGiaPhongByMaLoaiPhong(maLoaiPhong);
        }
        public List<GiaPhong> GiaPhong_GetCurrent(string maLoaiPhong, DateTime ngay)
        {
            return _res.GiaPhong_GetCurrent(maLoaiPhong, ngay);
        }
    }
}
