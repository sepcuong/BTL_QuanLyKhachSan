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
    public class LoaiPhongBusiness: ILoaiPhongBusiness
    {
        private ILoaiPhongRepository _res;
        public LoaiPhongBusiness(ILoaiPhongRepository res, IConfiguration configuration)
        {
            _res = res;
        }
        public List<LoaiPhong> GetAllLoaiPhong()
        {
            return _res.GetAllLoaiPhong();
        }
        public bool Create(LoaiPhong thongtin)
        {
            return _res.Create(thongtin);
        }
        public bool Update(LoaiPhong thongtin)
        {
            return _res.Update(thongtin);
        }
        public bool Delete(string maLoaiPhong)
        {
            return _res.Delete(maLoaiPhong);
        }
        public LoaiPhong GetLoaiPhongById(string maLoaiPhong)
        {
            return _res.GetLoaiPhongById(maLoaiPhong);
        }
        public List<LoaiPhong> Search(string keyword)
        {
            return _res.Search(keyword);
        }
    }
}
