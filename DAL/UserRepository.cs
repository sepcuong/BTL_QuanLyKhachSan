using DAL.Helper.Interfaces;
using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public partial class UserRepository : IUserRepository
    {
        private readonly IDatabaseHelper _dbHelper;
        public UserRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        // Implement the correct interface method
        public List<User> GetAllUsers()
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Query<User>(
                        "sp_User_GetAll",
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Create(User thongtin)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_User_Create",
                        new
                        {
                            thongtin.User_Id,
                            thongtin.Hoten,
                            thongtin.Ngaysinh,
                            thongtin.Diachi,
                            thongtin.Gioitinh,
                            thongtin.Email,
                            thongtin.Taikhoan,
                            thongtin.Matkhau,
                            thongtin.Role,
                            thongtin.Image_Url
                        },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
                //return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool Update(User thongtin)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_User_Update",
                        new
                        {
                            thongtin.User_Id,
                            thongtin.Hoten,
                            thongtin.Ngaysinh,
                            thongtin.Diachi,
                            thongtin.Gioitinh,
                            thongtin.Email,
                            thongtin.Taikhoan,
                            thongtin.Matkhau,
                            thongtin.Role,
                            thongtin.Image_Url
                        },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Delete(string user_Id)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_User_Delete",
                        new { User_Id = user_Id },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public User GetUserById(string user_Id)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.QueryFirstOrDefault<User>(
                        "sp_User_GetById",
                        new { User_Id = user_Id },
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<User> Search(string keyword)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Query<User>(
                        "sp_User_Search",
                        new { Keyword = $"%{keyword}%" },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
