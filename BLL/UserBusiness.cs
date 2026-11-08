using BLL.Interfaces;
using DAL;
using Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Helper;
using System.Security.Claims;

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
            return _res.GetAllUsers().Select(Sanitize).ToList();
        }
        public bool Create(User thongtin)
        {
            thongtin.Matkhau = SecurityHelper.HashPassword(thongtin.Matkhau, thongtin.Taikhoan);
            var result = _res.Create(thongtin);
            Sanitize(thongtin);
            return result;
        }
        public bool Update(User thongtin)
        {
            var currentUser = _res.GetUserById(thongtin.User_Id);
            if (currentUser == null)
                return false;

            thongtin.Matkhau = string.IsNullOrWhiteSpace(thongtin.Matkhau)
                ? currentUser.Matkhau
                : SecurityHelper.HashPassword(thongtin.Matkhau, thongtin.Taikhoan);
            var result = _res.Update(thongtin);
            Sanitize(thongtin);
            return result;
        }
        public bool Delete(string user_Id)
        {
            return _res.Delete(user_Id);
        }
        public User GetUserById(string user_Id)
        {
            var user = _res.GetUserById(user_Id);
            return user == null ? null : Sanitize(user);
        }
        public List<User> Search(string keyword)
        {
            return _res.Search(keyword).Select(Sanitize).ToList();
        }
        public User Login(string taikhoan, string matkhau)
        {
            if (string.IsNullOrWhiteSpace(taikhoan) || string.IsNullOrEmpty(matkhau))
                return null;

            var hashed = SecurityHelper.HashPassword(matkhau, taikhoan);
            var user = _res.Login(taikhoan, hashed);
            if (user == null)
                user = _res.Login(taikhoan, SecurityHelper.HashLegacyPassword(matkhau));
            if (user == null)
                user = _res.Login(taikhoan, matkhau);

            if (user != null && user.Matkhau?.StartsWith("PBKDF2$", StringComparison.Ordinal) != true)
            {
                user.Matkhau = SecurityHelper.HashPassword(matkhau, taikhoan);
                _res.Update(user);
            }

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Hoten ?? string.Empty),
                    new Claim(ClaimTypes.NameIdentifier, user.User_Id),
                    new Claim(ClaimTypes.StreetAddress, user.Diachi ?? string.Empty),
                    new Claim(ClaimTypes.Role, user.Role ?? string.Empty)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            return Sanitize(user);
        }

        private static User Sanitize(User user)
        {
            user.Matkhau = null;
            return user;
        }
    }
}
