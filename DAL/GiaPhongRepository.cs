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
    public partial class GiaPhongRepository : IGiaPhongRepository
    {
        private readonly IDatabaseHelper _dbHelper;
        public GiaPhongRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public List<GiaPhong> GetAllGiaPhong()
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Query<GiaPhong>(
                        "sp_GiaPhong_GetAll",
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi truy xuất tất cả các bản ghi GiaPhong: {ex.Message}", ex);
            }
        }
        public bool Create(GiaPhong thongtin)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_GiaPhong_Create",
                        new
                        {
                            thongtin.Ma_Gia,
                            thongtin.Ma_Loai_Phong,
                            thongtin.Ten_Chinh_Sach,
                            thongtin.Tu_Ngay,
                            thongtin.Den_Ngay,
                            thongtin.Gia_Theo_Dem,
                            thongtin.Gia_Theo_Gio
                        },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo bản ghi GiaPhong: {ex.Message}", ex);
            }
        }
        public bool Update(GiaPhong thongtin)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_GiaPhong_Update",
                        new
                        {
                            thongtin.Ma_Gia,
                            thongtin.Ma_Loai_Phong,
                            thongtin.Ten_Chinh_Sach,
                            thongtin.Tu_Ngay,
                            thongtin.Den_Ngay,
                            thongtin.Gia_Theo_Dem,
                            thongtin.Gia_Theo_Gio
                        },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật bản ghi GiaPhong: {ex.Message}", ex);
            }
        }
        public bool Delete(string maGia)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_GiaPhong_Delete",
                        new { Ma_Gia = maGia },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa bản ghi GiaPhong: {ex.Message}", ex);
            }
        }
        public GiaPhong GetGiaPhongById(string maGia)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.QueryFirstOrDefault<GiaPhong>(
                        "sp_GiaPhong_GetById",
                        new { Ma_Gia = maGia },
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi truy xuất bản ghi GiaPhong theo ID: {ex.Message}", ex);
            }
        }
        public GiaPhong GetGiaPhongByMaLoaiPhong(string maLoaiPhong)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.QueryFirstOrDefault<GiaPhong>(
                        "sp_GiaPhong_GetByLoaiPhong",
                        new { Ma_Loai_Phong = maLoaiPhong },
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi truy xuất các bản ghi GiaPhong theo Ma_Loai_Phong: {ex.Message}", ex);
            }
        }
        public List<GiaPhong> GiaPhong_GetCurrent(string maLoaiPhong, DateTime ngay)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Query<GiaPhong>(
                        "sp_GiaPhong_GetCurrent",
                        new
                        {
                            MaLoaiPhong = maLoaiPhong,
                            Ngay = ngay.Date
                        },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi truy xuất giá phòng hiện tại: {ex.Message}", ex);
            }
        }
    }
}
