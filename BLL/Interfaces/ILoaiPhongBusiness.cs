using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BLL.Interfaces
{
    public partial interface ILoaiPhongBusiness
    {
        List<LoaiPhong> GetAllLoaiPhong();
        bool Create(LoaiPhong thongtin);
        bool Update(LoaiPhong thongtin);
        bool Delete(string maLoaiPhong);
        LoaiPhong GetLoaiPhongById(string maLoaiPhong);
        List<LoaiPhong> Search(string keyword);

    }
}
