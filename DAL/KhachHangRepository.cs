using DAL.Helper.Interfaces;
using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                using (var connection = _dbHelper.CreateConnection())
                {
                    string query = "SELECT TOP (1000) * FROM [dbo].[khach_hang]";
                    var result = connection.Query<KhachHang>(query).ToList();
                    return result;
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
                using (var connection = _dbHelper.CreateConnection())
                {
                    string checksql = "SELECT COUNT(1) FROM [dbo].[khach_hang] WHERE [ma_khach_hang] = @Ma_Khach_Hang";
                    int count = connection.ExecuteScalar<int>(checksql, new { Ma_Khach_Hang = thongtin.Ma_Khach_Hang });
                    if (count > 0)
                    {
                        throw new Exception("Mã khách hàng đã tồn tại. Vui lòng sử dụng mã khác.");
                    }
                    string sql = "INSERT INTO [dbo].[khach_hang] " +
                        "([ma_khach_hang], [ho_ten], [cmnd_cccd], [so_dien_thoai], [email], [dia_chi]) " +
                        "VALUES " +
                        "(@Ma_Khach_Hang, @Ho_Ten, @Cmnd_Cccd, @So_Dien_Thoai, @Email, @Dia_Chi)";
                    int rowsAffected = connection.Execute(sql, thongtin);
                    return rowsAffected > 0;
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
                using (var connection = _dbHelper.CreateConnection())
                {
                    string query = "UPDATE [dbo].[khach_hang] " +
                        "SET [ho_ten] = @Ho_Ten, " +
                        "[cmnd_cccd] = @Cmnd_Cccd, " +
                        "[so_dien_thoai] = @So_Dien_Thoai, " +
                        "[email] = @Email, " +
                        "[dia_chi] = @Dia_Chi " +
                        "WHERE [ma_khach_hang] = @Ma_Khach_Hang";
                    int rowsAffected = connection.Execute(query, thongtin);
                    return rowsAffected > 0;
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
                using (var connection = _dbHelper.CreateConnection())
                {
                    string query = "DELETE FROM [dbo].[khach_hang] WHERE [ma_khach_hang] = @Ma_Khach_Hang";
                    int rowsAffected = connection.Execute(query, new { Ma_Khach_Hang = maKhachHang });
                    return rowsAffected > 0;
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
                using (var connection = _dbHelper.CreateConnection())
                {
                    string query = "SELECT TOP (1000) * FROM [dbo].[khach_hang] WHERE [ma_khach_hang] = @MaKhachHang";
                    var result = connection.QueryFirstOrDefault<KhachHang>(query, new { MaKhachHang = maKhachHang });
                    return result;
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
                using (var connection = _dbHelper.CreateConnection())
                {
                    string query = "SELECT TOP (1000) * FROM [khach_hang] " +
                        "WHERE [ma_khach_hang] LIKE @Keyword OR" +
                        "[ho_ten] LIKE @Keyword OR " +
                        "[cmnd_cccd] LIKE @Keyword OR " +
                        "[so_dien_thoai] LIKE @Keyword OR " +
                        "[email] LIKE @Keyword OR " +
                        "[dia_chi] LIKE @Keyword";
                    var result = connection.Query<KhachHang>(query, new { Keyword = $"%{keyword}%" }).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
