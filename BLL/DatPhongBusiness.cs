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
    public class DatPhongBusiness: IDatPhongBusiness
    {
        private IDatPhongRepository _res;
        public DatPhongBusiness(IDatPhongRepository res, IConfiguration configuration)
        {
            _res = res;
        }
        public List<DatPhong> GetAllDatPhong()
        {
            return _res.GetAllDatPhong();
        }
        public bool Create(DatPhong thongtin)
        {
            return _res.Create(thongtin);
        }
        public bool Update(DatPhong thongtin)
        {
            return _res.Update(thongtin);
        }
        public bool Delete(string maDatPhong)
        {
            return _res.Delete(maDatPhong);
        }
        public bool Cancel(string maDatPhong)
        {
            return _res.Cancel(maDatPhong);
        }
        public bool CheckIn(string maDatPhong)
        {
            return _res.CheckIn(maDatPhong);
        }
        public bool CheckOut(string maDatPhong)
        {
            return _res.CheckOut(maDatPhong);
        }
        public List<DatPhong> Search(string keyword)
        {
            return _res.Search(keyword);
        }
        public DatPhong GetById(string maDatPhong)
        {
            return _res.GetById(maDatPhong);
        }
        public DatPhong GetByKhachHang(string maKhachHang)
        {
            return _res.GetByKhachHang(maKhachHang);
        }
        public DatPhong GetByPhong(string maPhong)
        {
            return _res.GetByPhong(maPhong);
        }
    }
}
