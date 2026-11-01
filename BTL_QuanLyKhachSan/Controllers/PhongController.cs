using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Model;

namespace API.Controllers
{
    [ApiController]
    [Route("api-admin/[controller]")]
    [Route("api/[controller]")]
    public class PhongController : ControllerBase
    {
        private readonly IPhongBusiness _phongBusiness;
        public PhongController(IPhongBusiness phongBusiness)
        {
            _phongBusiness = phongBusiness;
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
        [Route("get-all-phong")]
        [HttpGet]
        public IActionResult GetAllPhong()
        {
            var result = _phongBusiness.GetAllPhong();
            return Ok(result);
        }
        [Route("create-phong")]
        [HttpPost]
        public IActionResult CreatePhong([FromBody] Phong thongtin)
        {
            try
            {
                thongtin.Ma_Phong = Guid.NewGuid().ToString(); // Tạo mã phòng mới
                var result = _phongBusiness.Create(thongtin);
                if (!result)
                {
                    return BadRequest(new { message = "Tạo phòng thất bại." });
                }
                return Ok(thongtin);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại") || ex.Message.Contains("không hợp lệ") || ex.Message.Contains("không được"))
                {
                    return BadRequest(new { message = ex.Message });
                }
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("update-phong")]
        [HttpPost]
        public IActionResult UpdatePhong([FromBody] Phong thongtin)
        {
            try
            {
                var result = _phongBusiness.Update(thongtin);
                if (!result)
                {
                    return NotFound(new { message = "Cập nhật phòng thất bại." });
                }
                return Ok(thongtin);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại") || ex.Message.Contains("không hợp lệ") || ex.Message.Contains("không được") || ex.Message.Contains("Không tìm thấy"))
                {
                    return BadRequest(new { message = ex.Message });
                }
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("delete-phong")]
        [HttpPost]
        public IActionResult DeletePhong([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                string Phong_Id = GetFormValue(formData, "Phong_Id", "maPhong", "maPhong_Id", "id");
                bool result = _phongBusiness.Delete(Phong_Id);
                if (result)
                {
                    return Ok(new { message = "Xóa phòng thành công!" });
                }else {                     
                    return NotFound(new { message = "Không tìm thấy phòng để xóa!" }); 
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("không tồn tại"))
                {
                    return BadRequest(new { message = ex.Message });
                }
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("get-by-id/{maPhong}")]
        [HttpGet]
        public Phong GetById(string maPhong)
        {
            try
            {
                var result = _phongBusiness.GetPhongById(maPhong);
                if (result == null)
                {
                    throw new Exception($"Không tìm thấy phòng với mã: {maPhong}");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy thông tin phòng: " + ex.Message);
            }
        }
        [Route("search-phong")]
        [HttpGet]
        public IActionResult SearchPhong([FromQuery] string keyword)
        {
            try
            {
                var result = _phongBusiness.Search(keyword);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("update-trangthai")]
        [HttpPost]
        public IActionResult UpdateTrangThai([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                string maPhong = GetFormValue(formData, "maPhong", "maPhong_Id", "Ma_Phong", "Phong_Id", "id");
                string trangThai = GetFormValue(formData, "trangThai", "Trang_Thai", "status");

                if (string.IsNullOrWhiteSpace(maPhong) || string.IsNullOrWhiteSpace(trangThai))
                {
                    return BadRequest(new { message = "Mã phòng và trạng thái không được để trống." });
                }

                var result = _phongBusiness.Update_TrangThai(maPhong, trangThai);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("không hợp lệ") || ex.Message.Contains("không được") || ex.Message.Contains("Không tìm thấy"))
                {
                    return BadRequest(new { message = ex.Message });
                }
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("get-available")]
        [HttpGet]
        public IActionResult GetAvailablePhong([FromQuery] DateTime ngaynhan, DateTime ngaytra)
        {
            try
            {
                var result = _phongBusiness.GetAvailable(ngaynhan, ngaytra);
                if (result == null)
                {
                    return NotFound(new { message = "Không có phòng trống trong khoảng thời gian đã chọn." });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Ngày nhận"))
                {
                    return BadRequest(new { message = ex.Message });
                }
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}
