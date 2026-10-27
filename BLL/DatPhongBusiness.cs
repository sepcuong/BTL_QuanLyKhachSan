using BLL.Interfaces;
using DAL;
using Microsoft.Extensions.Configuration;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DatPhongBusiness: IDatPhongBusiness
    {
        private IDatPhongRepository _res;
        public DatPhongBusiness(IDatPhongRepository res, IConfiguration configuration)
        {
            _res = res;
        }

        private static bool IsCancelledStatus(string? status)
        {
            var value = (status ?? string.Empty).Trim().ToLowerInvariant();
            return value.Contains("hủy") || value.Contains("huy") || value.Contains("cancel");
        }

        private static bool IsCheckedOutStatus(string? status)
        {
            var value = (status ?? string.Empty).Trim().ToLowerInvariant();
            return value.Contains("trả") || value.Contains("tra") || value.Contains("check out") ||
                   value.Contains("check-out") || value.Contains("checkout") || value.Contains("checkedout") || value.Contains("checked-out");
        }

        private static bool IsCheckedInStatus(string? status)
        {
            var value = (status ?? string.Empty).Trim().ToLowerInvariant();
            return value.Contains("đang ở") || value.Contains("dang o") || value.Contains("check in") ||
                   value.Contains("check-in") || value.Contains("checkin") || value.Contains("checkedin") || value.Contains("checked-in");
        }

        private static bool IsActiveBookingStatus(string? status)
        {
            return !IsCancelledStatus(status) && !IsCheckedOutStatus(status);
        }

        private static bool IsDateOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            return start1 < end2 && start2 < end1;
        }

        private void ValidateDatPhongInput(DatPhong thongtin, bool isUpdate)
        {
            if (thongtin == null)
                throw new Exception("Thông tin đặt phòng không hợp lệ.");

            if (isUpdate && string.IsNullOrWhiteSpace(thongtin.Ma_Dat_Phong))
                throw new Exception("Mã đặt phòng không được để trống.");

            if (string.IsNullOrWhiteSpace(thongtin.Ma_Khach_Hang))
                throw new Exception("Mã khách hàng không được để trống.");

            if (string.IsNullOrWhiteSpace(thongtin.Ma_Phong))
                throw new Exception("Mã phòng không được để trống.");

            if (thongtin.Ngay_Nhan_Du_Kien >= thongtin.Ngay_Tra_Du_Kien)
                throw new Exception("Ngày nhận dự kiến phải nhỏ hơn ngày trả dự kiến.");

            if (thongtin.Tien_Coc.HasValue && thongtin.Tien_Coc.Value < 0)
                throw new Exception("Tiền cọc không được âm.");
        }

        private void ValidateNoRoomConflict(DatPhong thongtin, bool isUpdate)
        {
            if (!IsActiveBookingStatus(thongtin.Trang_Thai))
            {
                return;
            }

            var allBookings = _res.GetAllDatPhong();
            var hasConflict = allBookings.Any(x =>
                x.Ma_Phong == thongtin.Ma_Phong &&
                (!isUpdate || x.Ma_Dat_Phong != thongtin.Ma_Dat_Phong) &&
                IsActiveBookingStatus(x.Trang_Thai) &&
                IsDateOverlap(
                    thongtin.Ngay_Nhan_Du_Kien,
                    thongtin.Ngay_Tra_Du_Kien,
                    x.Ngay_Nhan_Du_Kien,
                    x.Ngay_Tra_Du_Kien));

            if (hasConflict)
                throw new Exception("Phòng đã được đặt trong khoảng thời gian này.");
        }

        public List<DatPhong> GetAllDatPhong()
        {
            return _res.GetAllDatPhong();
        }
        public bool Create(DatPhong thongtin)
        {
            if (thongtin == null)
            {
                throw new Exception("Thông tin đặt phòng không hợp lệ.");
            }

            if (string.IsNullOrWhiteSpace(thongtin.Trang_Thai))
                thongtin.Trang_Thai = "Pending";

            ValidateDatPhongInput(thongtin, false);
            if (string.IsNullOrWhiteSpace(thongtin.Ma_Dat_Phong))
            {
                thongtin.Ma_Dat_Phong = Guid.NewGuid().ToString();
            }

            if (!thongtin.Ngay_Dat.HasValue)
            {
                thongtin.Ngay_Dat = DateTime.Now;
            }

            ValidateNoRoomConflict(thongtin, false);

            return _res.Create(thongtin);
        }
        public bool Update(DatPhong thongtin)
        {
            ValidateDatPhongInput(thongtin, true);

            var existing = _res.GetById(thongtin.Ma_Dat_Phong);
            if (existing == null)
                throw new Exception("Không tìm thấy đặt phòng để cập nhật.");

            thongtin.Trang_Thai = existing.Trang_Thai;
            ValidateNoRoomConflict(thongtin, true);

            return _res.Update(thongtin);
        }
        public bool Delete(string maDatPhong)
        {
            if (string.IsNullOrWhiteSpace(maDatPhong))
                throw new Exception("Mã đặt phòng không được để trống.");

            return _res.Delete(maDatPhong);
        }
        public bool Cancel(string maDatPhong)
        {
            if (string.IsNullOrWhiteSpace(maDatPhong))
                throw new Exception("Mã đặt phòng không được để trống.");

            var booking = _res.GetById(maDatPhong);
            if (booking == null)
                throw new Exception("Không tìm thấy đặt phòng để hủy.");

            if (IsCheckedInStatus(booking.Trang_Thai) || IsCheckedOutStatus(booking.Trang_Thai))
                throw new Exception("Không thể hủy đặt phòng đã check-in hoặc đã check-out.");

            return _res.Cancel(maDatPhong);
        }
        public bool CheckIn(string maDatPhong)
        {
            if (string.IsNullOrWhiteSpace(maDatPhong))
                throw new Exception("Mã đặt phòng không được để trống.");

            var booking = _res.GetById(maDatPhong);
            if (booking == null)
                throw new Exception("Không tìm thấy đặt phòng để check-in.");

            if (IsCancelledStatus(booking.Trang_Thai) || IsCheckedOutStatus(booking.Trang_Thai))
                throw new Exception("Không thể check-in cho đặt phòng đã hủy hoặc đã check-out.");

            if (IsCheckedInStatus(booking.Trang_Thai))
                throw new Exception("Đặt phòng đã check-in.");

            return _res.CheckIn(maDatPhong);
        }
        public bool CheckOut(string maDatPhong)
        {
            if (string.IsNullOrWhiteSpace(maDatPhong))
                throw new Exception("Mã đặt phòng không được để trống.");

            var booking = _res.GetById(maDatPhong);
            if (booking == null)
                throw new Exception("Không tìm thấy đặt phòng để check-out.");

            if (IsCancelledStatus(booking.Trang_Thai))
                throw new Exception("Không thể check-out cho đặt phòng đã hủy.");

            if (IsCheckedOutStatus(booking.Trang_Thai))
                throw new Exception("Đặt phòng đã check-out.");

            if (!IsCheckedInStatus(booking.Trang_Thai))
                throw new Exception("Chỉ có thể check-out sau khi đặt phòng đã check-in.");

            return _res.CheckOut(maDatPhong);
        }
        public List<DatPhong> Search(string keyword)
        {
            return _res.Search(keyword);
        }
        public DatPhong GetById(string maDatPhong)
        {
            return _res.GetById(maDatPhong);
        }
        public DatPhong GetByKhachHang(string maKhachHang)
        {
            return _res.GetByKhachHang(maKhachHang);
        }
        public DatPhong GetByPhong(string maPhong)
        {
            return _res.GetByPhong(maPhong);
        }
    }
}
