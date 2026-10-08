using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Model;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatPhongController : ControllerBase
    {
        private readonly IDatPhongBusiness _datPhongBusiness;
        public DatPhongController(IDatPhongBusiness datPhongBusiness)
        {
            _datPhongBusiness = datPhongBusiness;
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
                thongtin.Ma_Dat_Phong = Guid.NewGuid().ToString();
                var result = _datPhongBusiness.Create(thongtin);
                return Ok(thongtin);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại"))
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
                return Ok(thongtin);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại"))
                {
                    return BadRequest(new { message = ex.Message });
                }
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("delete-dat-phong")]
        [HttpPost]
        public IActionResult DeleteDatPhong([FromBody] string maDatPhong)
        {
            try
            {
                var result = _datPhongBusiness.Delete(maDatPhong);
                return Ok(new { message = "Xóa thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("cancel-dat-phong")]
        [HttpPost]
        public IActionResult CancelDatPhong([FromBody] string maDatPhong)
        {
            try
            {
                var result = _datPhongBusiness.Cancel(maDatPhong);
                return Ok(new { message = "Hủy thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("checkin-dat-phong")]
        [HttpPost]
        public IActionResult CheckInDatPhong([FromBody] string maDatPhong)
        {
            try
            {
                var result = _datPhongBusiness.CheckIn(maDatPhong);
                return Ok(new { message = "Check-in thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("checkout-dat-phong")]
        [HttpPost]
        public IActionResult CheckOutDatPhong([FromBody] string maDatPhong)
        {
            try
            {
                var result = _datPhongBusiness.CheckOut(maDatPhong);
                return Ok(new { message = "Check-out thành công" });
            }
            catch (Exception ex)
            {
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
