using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Model;


namespace API.Controllers
{
    [Authorize(Roles = "Admin,LeTan")]
    [ApiController]
    [Route("api-admin/[controller]")]
    [Route("api/[controller]")]
    public class LoaiPhongController : ControllerBase
    {
        private readonly ILoaiPhongBusiness _loaiPhongBusiness;
        public LoaiPhongController(ILoaiPhongBusiness loaiPhongBusiness)
        {
            _loaiPhongBusiness = loaiPhongBusiness;
        }

        [Route("get-all-loaiphong")]
        [HttpGet]
        public IActionResult GetAllLoaiPhong()
        {
            var result = _loaiPhongBusiness.GetAllLoaiPhong();
            return Ok(result);
        }
        [Route("create-loaiphong")]
        [HttpPost]
        public IActionResult CreateLoaiPhong([FromBody] LoaiPhong thongtin)
        {
            try
            {
                thongtin.Ma_Loai_Phong = Guid.NewGuid().ToString();
                var result = _loaiPhongBusiness.Create(thongtin);
                return Ok(new
                {
                    success = true,
                    message = "Thêm loại phòng thành công."
                });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại"))
                {
                    return BadRequest(new { success = false, message = "Dữ liệu loại phòng đã tồn tại." });
                }
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi hệ thống." });
            }
        }
        [Route("update-loaiphong")]
        [HttpPost]
        public IActionResult UpdateLoaiPhong([FromBody] LoaiPhong thongtin)
        {
            try
            {
                var result = _loaiPhongBusiness.Update(thongtin);
                return Ok(new
                {
                    success = true,
                    message = "Cập nhật loại phòng thành công."
                });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("không tồn tại"))
                {
                    return BadRequest(new { success = false, message = "Không tìm thấy loại phòng để cập nhật." });
                }
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi hệ thống." });
            }
        }
        [Route("delete-loaiphong")]
        [HttpPost]
        public IActionResult DeleteLoaiPhong([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                string loaiPhong_Id = "";
                if (formData.Keys.Contains("loaiPhong_Id") && !string.IsNullOrWhiteSpace(Convert.ToString(formData["loaiPhong_Id"])))
                {
                    loaiPhong_Id = Convert.ToString(formData["loaiPhong_Id"]) ?? string.Empty;
                }
                else
                {
                    return BadRequest(new { success = false, message = "Mã loại phòng không được để trống." });
                }
                bool result = _loaiPhongBusiness.Delete(loaiPhong_Id);
                if (result)
                {
                    return Ok(new { success = true, message = "Xóa loại phòng thành công." });
                }
                else
                {
                    return NotFound(new { success = false, message = "Không tìm thấy loại phòng để xóa." });
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("không tồn tại"))
                {
                    return BadRequest(new { success = false, message = "Không tìm thấy loại phòng để xóa." });
                }
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi hệ thống." });
            }
        }
        [Route("get-by-id/{loaiPhong_Id}")]
        [HttpGet]
        public IActionResult GetLoaiPhongById(string loaiPhong_Id)
        {
            var result = _loaiPhongBusiness.GetLoaiPhongById(loaiPhong_Id);
            if (result == null)
            {
                return NotFound(new { success = false, message = "Không tìm thấy loại phòng." });
            }
            return Ok(result);
        }
        [Route("search-loaiphong")]
        [HttpGet]
        public IActionResult SearchLoaiPhong([FromQuery] string keyword)
        {
            try
            {
                var result = _loaiPhongBusiness.Search(keyword);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi hệ thống." });
            }
        }
    }
}
