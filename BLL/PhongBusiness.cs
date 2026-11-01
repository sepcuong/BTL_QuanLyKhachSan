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
        private readonly IDatPhongRepository _datPhongRepository;
        public PhongBusiness(IPhongRepository res, IDatPhongRepository datPhongRepository, IConfiguration configuration)
        {
            _res = res;
            _datPhongRepository = datPhongRepository;
        }

        public List<Phong> GetAllPhong()
        {
            return ApplyBookingStatuses(_res.GetAllPhong());
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
            var room = _res.GetPhongById(maPhong);
            return room == null ? null : ApplyBookingStatuses(new List<Phong> { room })[0];
        }
        public List<Phong> Search(string keyword)
        {
            return ApplyBookingStatuses(_res.Search(keyword));
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

        private List<Phong> ApplyBookingStatuses(List<Phong> rooms)
        {
            var bookings = _datPhongRepository.GetAllDatPhong();
            var now = DateTime.Now;

            foreach (var room in rooms)
            {
                var roomBookings = bookings.Where(booking =>
                    booking.Ma_Phong == room.Ma_Phong &&
                    !IsCancelledOrCheckedOut(booking.Trang_Thai) &&
                    (IsCheckedIn(booking.Trang_Thai) || booking.Ngay_Tra_Du_Kien > now)).ToList();

                if (roomBookings.Any(booking => IsCheckedIn(booking.Trang_Thai)))
                {
                    room.Trang_Thai = "Đang ở";
                }
                else if (roomBookings.Count > 0)
                {
                    room.Trang_Thai = "Đã đặt";
                }
                else if (IsBookingStatus(room.Trang_Thai))
                {
                    room.Trang_Thai = "Trống";
                }
            }

            return rooms;
        }

        private static bool IsCancelledOrCheckedOut(string? status)
        {
            var value = (status ?? string.Empty).Trim().ToLowerInvariant();
            return value.Contains("hủy") || value.Contains("huy") || value.Contains("cancel") ||
                   value.Contains("trả") || value.Contains("tra") || value.Contains("checkout") ||
                   value.Contains("check-out") || value.Contains("checkedout");
        }

        private static bool IsCheckedIn(string? status)
        {
            var value = (status ?? string.Empty).Trim().ToLowerInvariant();
            return value.Contains("đang ở") || value.Contains("dang o") || value.Contains("checkin") ||
                   value.Contains("check-in") || value.Contains("checkedin") || value.Contains("checked-in");
        }

        private static bool IsBookingStatus(string? status)
        {
            var value = (status ?? string.Empty).Trim().ToLowerInvariant();
            return value is "trống" or "trong" or "đã đặt" or "da dat" or "đang ở" or "dang o" or
                "booked" or "reserved" or "occupied";
        }
    }
}
