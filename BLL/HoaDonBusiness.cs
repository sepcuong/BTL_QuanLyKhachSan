using BLL.Interfaces;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class HoaDonBusiness : IHoaDonBusiness
    {
        private readonly IHoaDonRepository _res;
        private readonly IDatPhongRepository _datPhongRepository;

        public HoaDonBusiness(IHoaDonRepository res, IDatPhongRepository datPhongRepository)
        {
            _res = res;
            _datPhongRepository = datPhongRepository;
        }

        private void ValidateHoaDonInput(HoaDonKhachSan thongtin, bool isUpdate)
        {
            if (thongtin == null)
                throw new Exception("Thông tin hóa đơn không hợp lệ.");

            if (isUpdate && string.IsNullOrWhiteSpace(thongtin.Ma_Hoa_Don))
                throw new Exception("Mã hóa đơn không được để trùng.");

            if (string.IsNullOrWhiteSpace(thongtin.Ma_Dat_Phong))
                throw new Exception("Mã đặt phòng không được để trùng.");

        }
        public List<HoaDonKhachSan> GetAllHoaDon()
        {
            return _res.GetAllHoaDon();
        }

        public HoaDonKhachSan GetHoaDonById(string maHoaDon)
        {
            return _res.GetHoaDonById(maHoaDon);
        }

        public HoaDonKhachSan GetByDatPhong(string maDatPhong)
        {
            return _res.GetByDatPhong(maDatPhong);
        }

        public List<HoaDonKhachSan> Search(string keyword)
        {
            return _res.Search(keyword);
        }

        public bool Create(HoaDonKhachSan thongtin)
        {
            ValidateHoaDonInput(thongtin, false);

            if (string.IsNullOrWhiteSpace(thongtin.Ma_Hoa_Don))
            {
                thongtin.Ma_Hoa_Don = Guid.NewGuid().ToString();
            }

            var existed = _res.GetByDatPhong(thongtin.Ma_Dat_Phong);
            if (existed != null)
            {
                throw new Exception("Đặt phòng này đã có hóa đơn.");
            }

            return _res.Create(thongtin);
        }

        public bool Update(HoaDonKhachSan thongtin)
        {
            ValidateHoaDonInput(thongtin, true);

            var existing = _res.GetHoaDonById(thongtin.Ma_Hoa_Don);
            if (existing == null)
            {
                throw new Exception("Không tìm thấy hóa đơn để cập nhật.");
            }

            var invoiceByDatPhong = _res.GetByDatPhong(thongtin.Ma_Dat_Phong);
            if (invoiceByDatPhong != null && invoiceByDatPhong.Ma_Hoa_Don != thongtin.Ma_Hoa_Don)
            {
                throw new Exception("Mã đặt phòng đã được gán cho hóa đơn khác.");
            }

            return _res.Update(thongtin);
        }

        public bool Delete(string maHoaDon)
        {
            if (string.IsNullOrWhiteSpace(maHoaDon))
            {
                throw new Exception("Mã hóa đơn không được để trùng.");
            }

            return _res.Delete(maHoaDon);
        }
    }
}
