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
    public class DichVuBusiness : IDichVuBusiness
    {
        private IDichVuRepository _res;
        public DichVuBusiness(IDichVuRepository res)
        {
            _res = res;
        }
        public List<DichVu> GetAllDichVu()
        {
            return _res.GetAllDichVu();
        }
        public bool Create(DichVu thongtin)
        {
            return _res.Create(thongtin);
        }
        public bool Update(DichVu thongtin)
        {
            return _res.Update(thongtin);
        }
        public bool Delete(string maDichVu)
        {
            return _res.Delete(maDichVu);
        }
        public DichVu GetDichVuById(string maDichVu)
        {
            return _res.GetDichVuById(maDichVu);
        }
        public List<DichVu> Search(string keyword)
        {
            return _res.Search(keyword);
        }
    }
}
