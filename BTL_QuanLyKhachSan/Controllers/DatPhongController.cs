using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Model;
using System;

namespace API.Controllers
{
    [ApiController]
    [Route("api-admin/[controller]")]
    [Route("api/[controller]")]
    public class DatPhongController : ControllerBase
    {
        private readonly IDatPhongBusiness _datPhongBusiness;
        public DatPhongController(IDatPhongBusiness datPhongBusiness)
        {
            _datPhongBusiness = datPhongBusiness;
        }

        private static string GetFormValue(Dictionary<string, object> formData, params string[] keys)
        {
            foreach (var key in keys)
            {
                var item = formData.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase));
                if (item.Value != null)
                    return item.Value.ToString() ?? string.Empty;
            }

            return string.Empty;
        }
        [Route("get-all-dat-phong")]
        [HttpGet]
        public List<DatPhong> GetAllDatPhong()
        {
            return _datPhongBusiness.GetAllDatPhong();
        }
        [Route("create-dat-phong")]
        [HttpPost]
        public IActionResult CreateDatPhong([FromBody] DatPhong thongtin)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(thongtin.Ma_Dat_Phong))
                    thongtin.Ma_Dat_Phong = Guid.NewGuid().ToString();
                thongtin.Ngay_Dat = DateTime.Now;
                var result = _datPhongBusiness.Create(thongtin);
                if (!result)
                {
                    return BadRequest(new { message = "Tạo đặt phòng thất bại." });
                }
                return Ok(thongtin);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại") || ex.Message.Contains("không hợp lệ") || ex.Message.Contains("không thể") || ex.Message.Contains("không được"))
                {
                    return BadRequest(new { message = ex.Message });
                }
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("update-dat-phong")]
        [HttpPost]
        public IActionResult UpdateDatPhong([FromBody] DatPhong thongtin)
        {
            try
            {
                var result = _datPhongBusiness.Update(thongtin);
                if (!result)
                {
                    return NotFound(new { message = "Cập nhật đặt phòng thất bại." });
                }
                return Ok(thongtin);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại") || ex.Message.Contains("không hợp lệ") || ex.Message.Contains("không thể") || ex.Message.Contains("không được") || ex.Message.Contains("Không tìm thấy"))
                {
                    return BadRequest(new { message = ex.Message });
                }
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("delete-dat-phong")]
        [HttpPost]
        public IActionResult DeleteDatPhong([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                string maDatPhong_Id = GetFormValue(formData, "maDatPhong_Id", "maDatPhongId", "Ma_Dat_Phong", "id");
                if (string.IsNullOrWhiteSpace(maDatPhong_Id))
                {
                    return BadRequest(new { message = "Mã đặt phòng không được để trống" });
                }
                var result = _datPhongBusiness.Delete(maDatPhong_Id);
                if (!result)
                {
                    return NotFound(new { message = "Không tìm thấy đặt phòng để xóa." });
                }
                return Ok(new { message = "Xóa thành công" });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("không được"))
                {
                    return BadRequest(new { message = ex.Message });
                }
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("cancel-dat-phong")]
        [HttpPost]
        public IActionResult CancelDatPhong([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                string maDatPhong_Id = GetFormValue(formData, "maDatPhong_Id", "maDatPhongId", "Ma_Dat_Phong", "id");
                if (string.IsNullOrWhiteSpace(maDatPhong_Id))
                {
                    return BadRequest(new { message = "Mã đặt phòng không được để trống" });
                }
                var result = _datPhongBusiness.Cancel(maDatPhong_Id);
                if (!result)
                {
                    return NotFound(new { message = "Hủy đặt phòng thất bại." });
                }
                return Ok(new { message = "Hủy thành công" });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("không thể") || ex.Message.Contains("không được") || ex.Message.Contains("Không tìm thấy"))
                {
                    return BadRequest(new { message = ex.Message });
                }
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("checkin-dat-phong")]
        [HttpPost]
        public IActionResult CheckInDatPhong([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                string maDatPhong_Id = GetFormValue(formData, "maDatPhong_Id", "maDatPhongId", "Ma_Dat_Phong", "id");
                if (string.IsNullOrWhiteSpace(maDatPhong_Id))
                {
                    return BadRequest(new { message = "Mã đặt phòng không được để trống" });
                }
                var result = _datPhongBusiness.CheckIn(maDatPhong_Id);
                if (!result)
                {
                    return NotFound(new { message = "Check-in thất bại." });
                }
                return Ok(new { message = "Check-in thành công" });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("không thể") || ex.Message.Contains("không được") || ex.Message.Contains("Không tìm thấy"))
                {
                    return BadRequest(new { message = ex.Message });
                }
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("checkout-dat-phong")]
        [HttpPost]
        public IActionResult CheckOutDatPhong([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                string maDatPhong_Id = GetFormValue(formData, "maDatPhong_Id", "maDatPhongId", "Ma_Dat_Phong", "id");
                if (string.IsNullOrWhiteSpace(maDatPhong_Id))
                {
                    return BadRequest(new { message = "Mã đặt phòng không được để trống" });
                }
                var result = _datPhongBusiness.CheckOut(maDatPhong_Id);
                if (!result)
                {
                    return NotFound(new { message = "Check-out thất bại." });
                }
                return Ok(new { message = "Check-out thành công" });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("không thể") || ex.Message.Contains("không được") || ex.Message.Contains("Không tìm thấy"))
                {
                    return BadRequest(new { message = ex.Message });
                }
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("search-dat-phong")]
        [HttpGet]
        public List<DatPhong> SearchDatPhong([FromQuery] string keyword)
        {
            return _datPhongBusiness.Search(keyword);
        }
        [Route("get-dat-phong-by-id/{phong_Id}")]
        [HttpGet]
        public IActionResult GetDatPhongById(string phong_Id)
        {
            try
            {
                var result = _datPhongBusiness.GetById(phong_Id);
                if (result == null)
                {
                    return NotFound(new { message = $"Không tìm thấy đặt phòng với mã: {phong_Id}" });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("get-dat-phong-by-khach-hang/{khachHang_Id}")]
        [HttpGet]
        public IActionResult GetDatPhongByKhachHang(string khachHang_Id)
        {
            try
            {
                var result = _datPhongBusiness.GetByKhachHang(khachHang_Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("get-dat-phong-by-phong/{phong_Id}")]
        [HttpGet]
        public IActionResult GetDatPhongByPhong(string phong_Id)
        {
            try
            {
                var result = _datPhongBusiness.GetByPhong(phong_Id);
                if (result == null)
                {
                    return NotFound(new { message = $"Không tìm thấy đặt phòng cho phòng với mã: {phong_Id}" });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}
