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
    public partial class HoaDonRepository: IHoaDonRepository
    {
        private readonly IDatabaseHelper _dbHelper;
        public HoaDonRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public List<HoaDonKhachSan> GetAllHoaDon()
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Query<HoaDonKhachSan>(
                        "sp_HoaDon_GetAll",
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi truy xuất tất cả các bản ghi HoaDon: {ex.Message}", ex);
            }
        }

        public HoaDonKhachSan GetHoaDonById(string maHoaDon)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.QueryFirstOrDefault<HoaDonKhachSan>(
                        "sp_HoaDon_GetById",
                        new { Ma_Hoa_Don = maHoaDon },
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi truy xuất HoaDon theo ID: {ex.Message}", ex);
            }
        }

        public HoaDonKhachSan GetByDatPhong(string maDatPhong)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.QueryFirstOrDefault<HoaDonKhachSan>(
                        "sp_HoaDon_GetByDatPhong",
                        new { Ma_Dat_Phong = maDatPhong },
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi truy xuất HoaDon theo DatPhong: {ex.Message}", ex);
            }
        }

        public List<HoaDonKhachSan> Search(string keyword)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Query<HoaDonKhachSan>(
                        "sp_HoaDon_Search",
                        new { Keyword = keyword },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm HoaDon: {ex.Message}", ex);
            }
        }

        public bool Create(HoaDonKhachSan thongtin)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_HoaDon_Create",
                        new
                        {
                            thongtin.Ma_Hoa_Don,
                            thongtin.Ma_Dat_Phong,
                            thongtin.Ngay_Lap,
                            thongtin.Tong_Tien_Phong,
                            thongtin.Tong_Tien_Dich_Vu,
                            thongtin.Thue_Vat,
                            thongtin.Tong_Thanh_Toan,
                            thongtin.Phuong_Thuc_Thanh_Toan
                        },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo bản ghi HoaDon: {ex.Message}", ex);
            }
        }

        public bool Update(HoaDonKhachSan thongtin)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_HoaDon_Update",
                        new
                        {
                            thongtin.Ma_Hoa_Don,
                            thongtin.Ma_Dat_Phong,
                            thongtin.Ngay_Lap,
                            thongtin.Tong_Tien_Phong,
                            thongtin.Tong_Tien_Dich_Vu,
                            thongtin.Thue_Vat,
                            thongtin.Tong_Thanh_Toan,
                            thongtin.Phuong_Thuc_Thanh_Toan
                        },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật bản ghi HoaDon: {ex.Message}", ex);
            }
        }

        public bool Delete(string maHoaDon)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_HoaDon_Delete",
                        new { Ma_Hoa_Don = maHoaDon },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa bản ghi HoaDon: {ex.Message}", ex);
            }
        }
    }
}
