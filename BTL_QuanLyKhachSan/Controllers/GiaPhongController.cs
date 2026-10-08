using BLL;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GiaPhongController : ControllerBase
    {
        private readonly IGiaPhongBusiness _giaPhongBusiness;
        public GiaPhongController(IGiaPhongBusiness giaPhongBusiness)
        {
            _giaPhongBusiness = giaPhongBusiness;
        }
        [Route("get-all-gia-phong")]
        [HttpGet]
        public List<GiaPhong> GetAllGiaPhong()
        {
            return _giaPhongBusiness.GetAllGiaPhong();
        }
        [Route("create-gia-phong")]
        [HttpPost]
        public IActionResult CreateGiaPhong([FromBody] GiaPhong thongtin)
        {
            try
            {
                thongtin.Ma_Gia = Guid.NewGuid().ToString(); // Tạo mã giá mới
                var result = _giaPhongBusiness.Create(thongtin);
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
        [Route("update-gia-phong")]
        [HttpPost]
        public IActionResult UpdateGiaPhong([FromBody] GiaPhong thongtin)
        {
            try
            {
                var result = _giaPhongBusiness.Update(thongtin);
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
        [Route("delete-gia-phong")]
        [HttpPost]
        public IActionResult DeleteGiaPhong([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                string maGia_Id = "";
                if (formData.Keys.Contains("maGia_Id") && !string.IsNullOrEmpty(Convert.ToString(formData["maGia_Id"]))) { maGia_Id = Convert.ToString(formData["maGia_Id"]); }
                var isDelete = _giaPhongBusiness.Delete(maGia_Id);
                if (isDelete)
                {
                    return Ok(new { message = "Xóa khách hàng thành công!" });
                }
                else
                {
                    return NotFound(new { message = "Không tìm thấy khách hàng để xóa!" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("get-gia-phong-by-id/{maGia}")]
        [HttpGet]
        public IActionResult GetGiaPhongById(string maGia)
        {
            try
            {
                var result = _giaPhongBusiness.GetGiaPhongById(maGia);
                if (result == null)
                {
                    return NotFound(new { message = $"Không tìm thấy giá phòng với mã: {maGia}" });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("get-gia-phong-by-loai-phong/{maLoaiPhong}")]
        [HttpGet]
        public IActionResult GetGiaPhongByLoaiPhong(string maLoaiPhong)
        {
            try
            {
                var result = _giaPhongBusiness.GetGiaPhongByMaLoaiPhong(maLoaiPhong);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("gia-phong-get-current")]
        [HttpGet]
        public IActionResult GiaPhong_GetCurrent([FromQuery] string maLoaiPhong, [FromQuery] DateTime ngay)
        {
            try
            {
                var result = _giaPhongBusiness.GiaPhong_GetCurrent(maLoaiPhong, ngay);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}
