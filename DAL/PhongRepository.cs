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
    public partial class PhongRepository: IPhongRepository
    {
        private readonly IDatabaseHelper _dbHelper;
        public PhongRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public List<Phong> GetAllPhong()
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Query<Phong>(
                        "sp_Phong_GetAll",
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể trích xuất Phong: {ex.Message}", ex);
            }
        }
        public bool Create(Phong thongtin)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_Phong_Create",
                        new
                        {
                            thongtin.Ma_Phong,
                            thongtin.Ten_Phong,
                            thongtin.Ma_Loai_Phong,
                            thongtin.So_Tang,
                            thongtin.Trang_Thai,
                        },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể tạo Phong: {ex.Message}", ex);
            }
        }
        public bool Update(Phong thongtin)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_Phong_Update",
                        new
                        {
                            thongtin.Ma_Phong,
                            thongtin.Ten_Phong,
                            thongtin.Ma_Loai_Phong,
                            thongtin.So_Tang,
                            thongtin.Trang_Thai,
                        },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể cập nhật Phong: {ex.Message}", ex);
            }
        }
        public bool Delete(string maPhong)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Execute(
                        "sp_Phong_Delete",
                        new { Ma_Phong = maPhong },
                        commandType: CommandType.StoredProcedure
                    ) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể xóa Phong: {ex.Message}", ex);
            }
        }
        public Phong GetPhongById(string maPhong)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.QueryFirstOrDefault<Phong>(
                        "sp_Phong_GetById",
                        new { Ma_Phong = maPhong },
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể trích xuất Phong theo ID: {ex.Message}", ex);
            }
        }
        public List<Phong> Search(string keyword)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    return connection.Query<Phong>(
                        "sp_Phong_Search",
                        new { Keyword = keyword },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể tìm kiếm Phong: {ex.Message}", ex);
            }
        }
        public Phong Update_TrangThai(string maPhong, string trangThai)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Ma_Phong", maPhong);
                    parameters.Add("@Trang_Thai", trangThai);
                    connection.Execute(
                        "sp_Phong_UpdateTrangThai",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    return GetPhongById(maPhong);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể cập nhật trạng thái Phong: {ex.Message}", ex);
            }
        }
        public Phong GetAvailable(DateTime ngayNhan, DateTime ngayTra)
        {
            try
            {
                using (var connection = _dbHelper.TConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Ngay_Nhan", ngayNhan);
                    parameters.Add("@Ngay_Tra", ngayTra);
                    return connection.QueryFirstOrDefault<Phong>(
                        "sp_Phong_GetAvailable",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không thể trích xuất Phong khả dụng: {ex.Message}", ex);
            }
        }
    }
}
