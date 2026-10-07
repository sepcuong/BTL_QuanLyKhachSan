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
    public partial class LoaiPhongRepository: ILoaiPhongRepository
    {
        private readonly IDatabaseHelper _dbHelper;
        public LoaiPhongRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public List<LoaiPhong> GetAllLoaiPhong()
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Query<LoaiPhong>(
                        "sp_LoaiPhong_GetAll",
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể trích xuất LoaiPhong: {ex.Message}", ex);
            }
        }
        public bool Create(LoaiPhong thongtin)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_LoaiPhong_Create",
                        new
                        {
                            thongtin.Ma_Loai_Phong,
                            thongtin.Ten_Loai_Phong,
                            thongtin.Gia_Mac_Dinh,
                            thongtin.So_Nguoi_Chuan,
                            thongtin.Mo_Ta
                        },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể tạo LoaiPhong: {ex.Message}", ex);
            }
        }
        public bool Update(LoaiPhong thongtin)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_LoaiPhong_Update",
                        new
                        {
                            thongtin.Ma_Loai_Phong,
                            thongtin.Ten_Loai_Phong,
                            thongtin.Gia_Mac_Dinh,
                            thongtin.So_Nguoi_Chuan,
                            thongtin.Mo_Ta
                        },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể cập nhật LoaiPhong: {ex.Message}", ex);
            }
        }
        public bool Delete(string maLoaiPhong)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_LoaiPhong_Delete",
                        new { Ma_Loai_Phong = maLoaiPhong },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể xóa LoaiPhong: {ex.Message}", ex);
            }
        }
        public LoaiPhong GetLoaiPhongById(string maLoaiPhong)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.QueryFirstOrDefault<LoaiPhong>(
                        "sp_LoaiPhong_GetById",
                        new { Ma_Loai_Phong = maLoaiPhong },
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể trích xuất LoaiPhong theo ID: {ex.Message}", ex);
            }
        }
        public List<LoaiPhong> Search(string keyword)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Query<LoaiPhong>(
                        "sp_LoaiPhong_Search",
                        new { Keyword = keyword },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể tìm kiếm LoaiPhong: {ex.Message}", ex);
            }
        }
    }
}
