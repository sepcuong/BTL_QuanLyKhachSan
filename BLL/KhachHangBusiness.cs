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
    public class KhachHangBusiness : IKhachHangBusiness
    {
        private IKhachHangRepository _res;
        public KhachHangBusiness(IKhachHangRepository res, IConfiguration configuration)
        {
            _res = res;
        }
        public List<KhachHang> GetAllKhachHang()
        {
            return _res.GetAllKhachHang();
        }
        public bool Create(KhachHang thongtin)
        {
            return _res.Create(thongtin);
        }
        public bool Update(KhachHang thongtin)
        {
            return _res.Update(thongtin);
        }
        public bool Delete(string maKhachHang)
        {
            return _res.Delete(maKhachHang);
        }
        public KhachHang GetById(string maKhachHang)
        {
            return _res.GetById(maKhachHang);
        }
        public List<KhachHang> Search(string keyword)
        {
            return _res.Search(keyword);
        }
    }
}
