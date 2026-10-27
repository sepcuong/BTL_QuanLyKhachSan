using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace APIUser.Controllers
{
    [ApiController]
    [Route("api-user/[controller]")]
    public class DatPhongController : ControllerBase
    {
        private readonly IDatPhongBusiness _datPhongBusiness;
        public DatPhongController(IDatPhongBusiness datPhongBusiness)
        {
            _datPhongBusiness = datPhongBusiness;
        }
        [Route("create-dat-phong")]
        [HttpPost]
        public IActionResult CreateDatPhong([FromBody] DatPhong thongtin)
        {
            try
            {
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
        [Route("get-dat-phong-by-khach-hang/{khachHang_Id}")]
        [HttpGet]
        public IActionResult GetDatPhongByKhachHang(string khachHang_Id)
        {
            try
            {
                var result = _datPhongBusiness.GetByKhachHang(khachHang_Id);
                if (result == null)
                {
                    return NotFound(new { message = $"Không tìm thấy đặt phòng cho khách hàng với mã: {khachHang_Id}" });
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
