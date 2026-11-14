using BLL;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace API.Controllers
{
    [Authorize(Roles = "Admin,KeToan")]
    [ApiController]
    [Route("api-admin/[controller]")]
    [Route("api/[controller]")]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonBusiness _business;
        public HoaDonController(IHoaDonBusiness business)
        {
            _business = business;
        }

        [Route("get-all-hoa-don")]
        [HttpGet]
        public List<HoaDonKhachSan> GetAll()
        {
            return _business.GetAllHoaDon();
        }

        [Route("get-by-id/{id}")]
        [HttpGet]
        public IActionResult GetById(string id)
        {
            var item = _business.GetHoaDonById(id);
            if (item == null)
            {
                return NotFound(new { success = false, message = "Không tìm thấy hóa đơn." });
            }
            return Ok(item);
        }

        [Route("get-by-datphong/{maDatPhong}")]
        [HttpGet]
        public IActionResult GetByDatPhong(string maDatPhong)
        {
            var item = _business.GetByDatPhong(maDatPhong);
            if (item == null)
            {
                return NotFound(new { success = false, message = "Không tìm thấy hóa đơn cho đặt phòng này." });
            }
            return Ok(item);
        }

        [Route("search")]
        [HttpGet]
        public IActionResult Search([FromQuery] string keyword)
        {
            var list = _business.Search(keyword);
            return Ok(list);
        }

        [Route("create-hoa-don")]
        [HttpPost]
        public IActionResult Create([FromBody] List<HoaDonKhachSan> thongtin)
        {
            try
            {
                double tongTienPhongDoan = 0;
                double tongTienDichVuDoan = 0;
                double tongVatDoan = 0;
                var danhSachMaHoaDon = new List<string>();
                foreach (var item in thongtin)
                {
                    item.Ma_Hoa_Don = Guid.NewGuid().ToString();

                    double tienDvPhong = 0;
                    if (item.listjson_dichvu != null && item.listjson_dichvu.Count > 0)
                    {
                        foreach (var dv in item.listjson_dichvu)
                        {
                            if (string.IsNullOrEmpty(dv.Ma_Chi_Tiet_Dv))
                                dv.Ma_Chi_Tiet_Dv = Guid.NewGuid().ToString();
                            tienDvPhong += dv.Thanh_Tien;
                        }
                    }

                    _business.Create(item);

                    danhSachMaHoaDon.Add(item.Ma_Hoa_Don);
                    tongTienPhongDoan += item.Tong_Tien_Phong;
                    tongTienDichVuDoan += tienDvPhong;
                    tongVatDoan += item.Thue_Vat ?? 0;
                }

                double tongThanhToanDoan = tongTienPhongDoan + tongTienDichVuDoan + tongVatDoan;

                return Ok(new
                {
                    message = $"Thanh toán và xuất hóa đơn thành công cho {thongtin.Count} phòng!",
                    so_luong_phong = thongtin.Count,
                    danh_sach_ma_hoa_don = danhSachMaHoaDon,
                    tong_tien_phong_doan = tongTienPhongDoan,
                    tong_tien_dich_vu_doan = tongTienDichVuDoan,
                    tong_thue_vat_doan = tongVatDoan,
                    tong_thanh_toan_cuoi_cung = tongThanhToanDoan
                });
                //thongtin.Ma_Hoa_Don = Guid.NewGuid().ToString();
                //var result = _business.Create(thongtin);
                //return Ok(new { success = true, message = "Tạo hóa đơn thành công." });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("không hợp lệ") || ex.Message.Contains("không được") || ex.Message.Contains("Không tìm thấy") || ex.Message.Contains("đã có"))
                {
                    return BadRequest(new { success = false, message = "Thông tin hóa đơn không hợp lệ." });
                }
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi hệ thống." });
            }
        }

        [Route("update-hoa-don")]
        [HttpPost]
        public IActionResult Update([FromBody] HoaDonKhachSan thongtin)
        {
            try
            {
                var result = _business.Update(thongtin);
                return Ok(new { success = true, message = "Cập nhật hóa đơn thành công." });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("không hợp lệ") || ex.Message.Contains("không được") || ex.Message.Contains("Không tìm thấy") || ex.Message.Contains("đã"))
                {
                    return BadRequest(new { success = false, message = "Thông tin hóa đơn không hợp lệ." });
                }
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi hệ thống." });
            }
        }

        [Route("delete-hoa-don")]
        [HttpPost]
        public IActionResult Delete([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                string maHoaDon_Id = "";
                if (formData.Keys.Contains("maHoaDon_Id") && !string.IsNullOrEmpty(Convert.ToString(formData["maHoaDon_Id"])))
                {
                    maHoaDon_Id = Convert.ToString(formData["maHoaDon_Id"]);
                }
                else
                {
                    return BadRequest(new { success = false, message = "Mã hóa đơn không được để trống." });
                }

                var isDelete = _business.Delete(maHoaDon_Id);
                if (isDelete)
                {
                    return Ok(new { success = true, message = "Xóa hóa đơn thành công." });
                }
                else
                {
                    return NotFound(new { success = false, message = "Không tìm thấy hóa đơn để xóa." });
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("không được"))
                {
                    return BadRequest(new { success = false, message = "Không thể xóa hóa đơn." });
                }
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi hệ thống." });
            }
        }
    }
}
