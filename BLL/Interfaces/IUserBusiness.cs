using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BLL.Interfaces
{
    public partial interface IUserBusiness
    {
        List<User> GetAllUsers();
        bool Create(User thongtin);
        bool Update(User thongtin);
        bool Delete(string user_Id);
        User GetUserById(string user_Id);
        List<User> Search(string keyword);
        User Login(string taikhoan, string matkhauHashed);
    }
}
