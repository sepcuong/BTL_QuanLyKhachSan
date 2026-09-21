using BLL.Interfaces;
using DAL;
using Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Helper;

namespace BLL
{
    public class UserBusiness: IUserBusiness
    {
        private IUserRepository _res;
        private string Secret;
        public UserBusiness(IUserRepository res, IConfiguration configuration)
        {
            Secret = configuration["AppSettings:Secret"];
            _res = res;
        }
        public List<User> GetAllUsers()
        {
            return _res.GetAllUsers();
        }
        public bool Create(User thongtin)
        {
            thongtin.Matkhau = SecurityHelper.HashPassword(thongtin.Matkhau);
            return _res.Create(thongtin);
        }
        public bool Update(User thongtin)
        {
            thongtin.Matkhau = SecurityHelper.HashPassword(thongtin.Matkhau);
            return _res.Update(thongtin);
        }
        public bool Delete(string user_Id)
        {
            return _res.Delete(user_Id);
        }
        public User GetUserById(string user_Id)
        {
            return _res.GetUserById(user_Id);
        }
        public List<User> Search(string keyword)
        {
            return _res.Search(keyword);
        }
    }
}
