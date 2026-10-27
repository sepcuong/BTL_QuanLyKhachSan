using BLL.Interfaces;
using DAL;
using Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PhongBusiness: IPhongBusiness
    {
        private IPhongRepository _res;
        public PhongBusiness(IPhongRepository res, IConfiguration configuration)
        {
            _res = res;
        }

        public List<Phong> GetAllPhong()
        {
            return _res.GetAllPhong();
        }
        public bool Create(Phong thongtin)
        {
            if (thongtin == null)
                throw new Exception("Thông tin phòng không hợp lệ.");

            if (string.IsNullOrWhiteSpace(thongtin.Ma_Loai_Phong))
                throw new Exception("Mã loại phòng không được để trống.");

            if (string.IsNullOrWhiteSpace(thongtin.Ten_Phong))
                throw new Exception("Tên phòng không được để trống.");

            if (string.IsNullOrWhiteSpace(thongtin.Trang_Thai))
                thongtin.Trang_Thai = "Trống";

            return _res.Create(thongtin);
        }
        public bool Update(Phong thongtin)
        {
            if (thongtin == null || string.IsNullOrWhiteSpace(thongtin.Ma_Phong))
                throw new Exception("Mã phòng không hợp lệ.");

            var existing = _res.GetPhongById(thongtin.Ma_Phong);
            if (existing == null)
                throw new Exception("Không tìm thấy phòng để cập nhật.");

            thongtin.Trang_Thai = existing.Trang_Thai;
            return _res.Update(thongtin);
        }
        public bool Delete(string maPhong)
        {
            if (string.IsNullOrWhiteSpace(maPhong))
                throw new Exception("Mã phòng không được để trống.");

            return _res.Delete(maPhong);
        }
        public Phong GetPhongById(string maPhong)
        {
            return _res.GetPhongById(maPhong);
        }
        public List<Phong> Search(string keyword)
        {
            return _res.Search(keyword);
        }
        public Phong Update_TrangThai(string maPhong, string trangThai)
        {
            if (string.IsNullOrWhiteSpace(maPhong))
                throw new Exception("Mã phòng không được để trống.");

            if (string.IsNullOrWhiteSpace(trangThai))
                throw new Exception("Trạng thái phòng không được để trống.");

            var existing = _res.GetPhongById(maPhong);
            if (existing == null)
                throw new Exception("Không tìm thấy phòng để cập nhật trạng thái.");

            return _res.Update_TrangThai(maPhong, trangThai);
        }
        public Phong GetAvailable(DateTime ngayNhan, DateTime ngayTra)
        {
            if (ngayNhan >= ngayTra)
                throw new Exception("Ngày nhận phải nhỏ hơn ngày trả.");

            return _res.GetAvailable(ngayNhan, ngayTra);
        }
    }
}
