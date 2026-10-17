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
    public partial class ChiTietDvRepository : IChiTietDvRepository
    {
        private readonly IDatabaseHelper _dbHelper;
        public ChiTietDvRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public List<ChiTietSuDungDv> GetAllChiTietSuDungDv()
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Query<ChiTietSuDungDv>(
                        "sp_ChiTietDv_GetAll",
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            } catch (Exception ex)
            {
                throw new Exception($"Lỗi khi truy xuất tất cả các bản ghi ChiTietDv: {ex.Message}", ex);
            }
        }

        public ChiTietSuDungDv GetChiTietDvById(string maChiTietDv)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.QueryFirstOrDefault<ChiTietSuDungDv>(
                        "sp_ChiTietDv_GetById",
                        new { Ma_Chi_Tiet_Dv = maChiTietDv },
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi truy xuất bản ghi ChiTietDv theo ID: {ex.Message}", ex);
            }
        }

        public List<ChiTietSuDungDv> GetByDatPhong(string maDatPhong)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Query<ChiTietSuDungDv>(
                        "sp_ChiTietDv_GetByDatPhong",
                        new { Ma_Dat_Phong = maDatPhong },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi truy xuất ChiTietDv theo DatPhong: {ex.Message}", ex);
            }
        }

        public bool Create(ChiTietSuDungDv thongtin)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_ChiTietDv_Create",
                        new
                        {
                            thongtin.Ma_Chi_Tiet_Dv,
                            thongtin.Ma_Dat_Phong,
                            thongtin.Ma_Dich_Vu,
                            thongtin.So_Luong,
                            thongtin.Thanh_Tien,
                            thongtin.Ngay_Su_Dung
                        },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo bản ghi ChiTietDv: {ex.Message}", ex);
            }
        }

        public bool Update(ChiTietSuDungDv thongtin)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_ChiTietDv_Update",
                        new
                        {
                            thongtin.Ma_Chi_Tiet_Dv,
                            thongtin.Ma_Dat_Phong,
                            thongtin.Ma_Dich_Vu,
                            thongtin.So_Luong,
                            thongtin.Thanh_Tien,
                            thongtin.Ngay_Su_Dung
                        },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật bản ghi ChiTietDv: {ex.Message}", ex);
            }
        }

        public bool Delete(string maChiTietDv)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_ChiTietDv_Delete",
                        new { Ma_Chi_Tiet_Dv = maChiTietDv },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa bản ghi ChiTietDv: {ex.Message}", ex);
            }
        }
    }
}
