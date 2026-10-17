using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BLL.Interfaces
{
    public partial interface IDichVuBusiness
    {
        List<DichVu> GetAllDichVu();
        bool Create(DichVu thongtin);
        bool Update(DichVu thongtin);
        bool Delete(string maDichVu);
        DichVu GetDichVuById(string maDichVu);
        List<DichVu> Search(string keyword);
    }
}
