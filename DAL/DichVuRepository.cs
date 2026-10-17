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
    public partial class DichVuRepository: IDichVuRepository
    {
        private readonly IDatabaseHelper _dbHelper;
        public DichVuRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public List<DichVu> GetAllDichVu()
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Query<DichVu>(
                        "sp_DichVu_GetAll",
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi truy xuất tất cả các bản ghi DichVu: {ex.Message}", ex);
            }
        }
        public bool Create(DichVu thongtin)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_DichVu_Create",
                        new
                        {
                            thongtin.Ma_Dich_Vu,
                            thongtin.Ten_Dich_Vu,
                            thongtin.Don_Gia,
                            thongtin.Don_Vi_Tinh
                        },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo bản ghi DichVu: {ex.Message}", ex);
            }
        }
        public bool Update(DichVu thongtin)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_DichVu_Update",
                        new
                        {
                            thongtin.Ma_Dich_Vu,
                            thongtin.Ten_Dich_Vu,
                            thongtin.Don_Gia,
                            thongtin.Don_Vi_Tinh
                        },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật bản ghi DichVu: {ex.Message}", ex);
            }
        }
        public bool Delete(string maDichVu)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_DichVu_Delete",
                        new { Ma_Dich_Vu = maDichVu },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa bản ghi DichVu: {ex.Message}", ex);
            }
        }
        public DichVu GetDichVuById(string maDichVu)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.QueryFirstOrDefault<DichVu>(
                        "sp_DichVu_GetById",
                        new { Ma_Dich_Vu = maDichVu },
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi truy xuất bản ghi DichVu theo ID: {ex.Message}", ex);
            }
        }
        public List<DichVu> Search(string keyword)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Query<DichVu>(
                        "sp_DichVu_Search",
                        new { Keyword = keyword },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm bản ghi DichVu: {ex.Message}", ex);
            }
        }
    }
}
