using DAL.Helper.Interfaces;
using Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DAL
{
    public partial class DatPhongRepository : IDatPhongRepository
    {
        private readonly IDatabaseHelper _dbHelper;
        public DatPhongRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public List<DatPhong> GetAllDatPhong()
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Query<DatPhong>(
                        "sp_DatPhong_GetAll",
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi truy xuất tất cả các bản ghi DatPhong: {ex.Message}", ex);
            }
        }
        public bool Create(DatPhong thongtin)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_DatPhong_Create",
                        new
                        {
                            thongtin.Ma_Dat_Phong,
                            thongtin.Ma_Khach_Hang,
                            thongtin.Ma_Phong,
                            thongtin.Ngay_Dat,
                            thongtin.Ngay_Nhan_Du_Kien,
                            thongtin.Ngay_Tra_Du_Kien,
                            thongtin.Tien_Coc,
                            thongtin.Trang_Thai,
                            thongtin.Ghi_Chu
                        },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể tạo DatPhong: {ex.Message}", ex);
            }
        }
        public bool Update(DatPhong thongtin)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_DatPhong_Update",
                        new
                        {
                            thongtin.Ma_Dat_Phong,
                            thongtin.Ma_Khach_Hang,
                            thongtin.Ma_Phong,
                            thongtin.Ngay_Dat,
                            thongtin.Ngay_Nhan_Du_Kien,
                            thongtin.Ngay_Tra_Du_Kien,
                            thongtin.Tien_Coc,
                            thongtin.Trang_Thai,
                            thongtin.Ghi_Chu
                        },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể cập nhật DatPhong: {ex.Message}", ex);
            }
        }
        public bool Delete(string maDatPhong)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_DatPhong_Delete",
                        new { Ma_Dat_Phong = maDatPhong },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể xóa DatPhong: {ex.Message}", ex);
            }
        }
        public bool Cancel(string maDatPhong)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_DatPhong_Cancel",
                        new { Ma_Dat_Phong = maDatPhong },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể hủy DatPhong: {ex.Message}", ex);
            }
        }
        public bool CheckIn(string maDatPhong)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_DatPhong_CheckIn",
                        new { Ma_Dat_Phong = maDatPhong },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể check-in DatPhong: {ex.Message}", ex);
            }
        }
        public bool CheckOut(string maDatPhong)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_DatPhong_CheckOut",
                        new { Ma_Dat_Phong = maDatPhong },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể check-out DatPhong: {ex.Message}", ex);
            }
        }
        public List<DatPhong> Search(string keyword)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Query<DatPhong>(
                        "sp_DatPhong_Search",
                        new { Keyword = keyword },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm DatPhong: {ex.Message}", ex);
            }
        }
        public DatPhong GetById(string maDatPhong)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.QueryFirstOrDefault<DatPhong>(
                        "sp_DatPhong_GetById",
                        new { Ma_Dat_Phong = maDatPhong },
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi truy xuất DatPhong theo id: {ex.Message}", ex);
            }
        }
        public DatPhong GetByKhachHang(string maKhachHang)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.QueryFirstOrDefault<DatPhong>(
                        "sp_DatPhong_GetByKhachHang",
                        new { Ma_Khach_Hang = maKhachHang },
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi truy xuất DatPhong theo khách hàng: {ex.Message}", ex);
            }
        }
        public DatPhong GetByPhong(string maPhong)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.QueryFirstOrDefault<DatPhong>(
                        "sp_DatPhong_GetByPhong",
                        new { Ma_Khach_Hang = maPhong },
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi truy xuất DatPhong theo phòng: {ex.Message}", ex);
            }
        }
    }
}
