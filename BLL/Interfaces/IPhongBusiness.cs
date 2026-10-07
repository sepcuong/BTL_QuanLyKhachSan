using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BLL.Interfaces
{
    public partial interface IPhongBusiness
    {
        List<Phong> GetAllPhong();
        bool Create(Phong thongtin);
        bool Update(Phong thongtin);
        bool Delete(string maPhong);
        Phong GetPhongById(string maPhong);
        List<Phong> Search(string keyword);
        Phong Update_TrangThai(string maPhong, string trangThai);
        Phong GetAvailable(DateTime ngayNhan, DateTime ngayTra);
    }
}
