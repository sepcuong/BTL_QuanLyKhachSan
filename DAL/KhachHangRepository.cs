using DAL.Helper.Interfaces;
using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DAL
{
    public partial class KhachHangRepository : IKhachHangRepository
    {
        private readonly IDatabaseHelper _dbHelper;
        public KhachHangRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public List<KhachHang> GetAllKhachHang()
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Query<KhachHang>(
                        "sp_KhachHang_GetAll",
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể trích xuất dữ liệu: {ex.Message}", ex);
            }
        }
        public bool Create(KhachHang thongtin)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_KhachHang_Create",
                        new
                        {
                            thongtin.Ma_Khach_Hang,
                            thongtin.Ho_Ten,
                            thongtin.Cmnd_Cccd,
                            thongtin.So_Dien_Thoai,
                            thongtin.Email,
                            thongtin.Dia_Chi
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
        public bool Update(KhachHang thongtin)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_KhachHang_Update",
                        new
                        {
                            thongtin.Ma_Khach_Hang,
                            thongtin.Ho_Ten,
                            thongtin.Cmnd_Cccd,
                            thongtin.So_Dien_Thoai,
                            thongtin.Email,
                            thongtin.Dia_Chi
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
        public bool Delete(string maKhachHang)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_KhachHang_Delete",
                        new { Ma_Khach_Hang = maKhachHang },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public KhachHang GetById(string maKhachHang)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.QueryFirstOrDefault<KhachHang>(
                        "sp_KhachHang_GetById",
                        new { Ma_Khach_Hang = maKhachHang },
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<KhachHang> Search(string keyword)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Query<KhachHang>(
                        "sp_KhachHang_Search",
                        new { Keyword = keyword },
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
