using BLL.Interfaces;
using DAL;
using Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PhongBusiness: IPhongBusiness
    {
        private IPhongRepository _res;
        public PhongBusiness(IPhongRepository res, IConfiguration configuration)
        {
            _res = res;
        }
        public List<Phong> GetAllPhong()
        {
            return _res.GetAllPhong();
        }
        public bool Create(Phong thongtin)
        {
            return _res.Create(thongtin);
        }
        public bool Update(Phong thongtin)
        {
            return _res.Update(thongtin);
        }
        public bool Delete(string maPhong)
        {
            return _res.Delete(maPhong);
        }
        public Phong GetPhongById(string maPhong)
        {
            return _res.GetPhongById(maPhong);
        }
        public List<Phong> Search(string keyword)
        {
            return _res.Search(keyword);
        }
        public Phong Update_TrangThai(string maPhong, string trangThai)
        {
            return _res.Update_TrangThai(maPhong, trangThai);
        }
        public Phong GetAvailable(DateTime ngayNhan, DateTime ngayTra)
        {
            return _res.GetAvailable(ngayNhan, ngayTra);
        }
    }
}
