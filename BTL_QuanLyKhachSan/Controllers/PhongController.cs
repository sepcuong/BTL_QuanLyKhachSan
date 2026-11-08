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
                return Ok(new
                {
                    success = true,
                    message = "Tạo phòng thành công."
                });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại") || ex.Message.Contains("không hợp lệ") || ex.Message.Contains("không được"))
                {
                    return BadRequest(new { success = false, message = "Thông tin phòng không hợp lệ hoặc đã tồn tại." });
                }
                throw;
            }
        }
        [Route("update-phong")]
        [HttpPost]
        public IActionResult UpdatePhong([FromBody] Phong thongtin)
        {
            try
            {
                var result = _phongBusiness.Update(thongtin);
                return Ok(new
                {
                    success = true,
                    message = "Cập nhật phòng thành công."
                });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại") || ex.Message.Contains("không hợp lệ") || ex.Message.Contains("không được") || ex.Message.Contains("Không tìm thấy"))
                {
                    return BadRequest(new { success = false, message = "Thông tin phòng không hợp lệ hoặc đã tồn tại." });
                }
                throw;
            }
        }
        [Route("delete-phong")]
        [HttpPost]
        public IActionResult DeletePhong([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                string Phong_Id = GetFormValue(formData, "Phong_Id", "maPhong", "maPhong_Id", "id");
                if (string.IsNullOrWhiteSpace(Phong_Id))
                {
                    return BadRequest(new { success = false, message = "Mã phòng không được để trống." });
                }
                bool result = _phongBusiness.Delete(Phong_Id);
                if (result)
                {
                    return Ok(new { success = true, message = "Xóa phòng thành công." });
                }else {                     
                    return NotFound(new { success = false, message = "Không tìm thấy phòng để xóa." }); 
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("không tồn tại"))
                {
                    return BadRequest(new { success = false, message = "Không thể xóa phòng do dữ liệu không hợp lệ." });
                }
                throw;
            }
        }
        [Route("get-by-id/{maPhong}")]
        [HttpGet]
        public IActionResult GetById(string maPhong)
        {
            var result = _phongBusiness.GetPhongById(maPhong);
            return result == null ? NotFound(new { success = false, message = "Không tìm thấy phòng." }) : Ok(result);
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
                throw;
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
                    return BadRequest(new { success = false, message = "Mã phòng và trạng thái không được để trống." });
                }

                var result = _phongBusiness.Update_TrangThai(maPhong, trangThai);
                if (result == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy phòng để cập nhật trạng thái." });
                }
                return Ok(new
                {
                    success = true,
                    message = "Cập nhật trạng thái phòng thành công."
                });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("không hợp lệ") || ex.Message.Contains("không được") || ex.Message.Contains("Không tìm thấy"))
                {
                    return BadRequest(new { success = false, message = "Trạng thái phòng không hợp lệ." });
                }
                throw;
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
                    return NotFound(new { success = false, message = "Không có phòng trống trong khoảng thời gian đã chọn." });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Ngày nhận"))
                {
                    return BadRequest(new { success = false, message = "Khoảng ngày không hợp lệ." });
                }
                throw;
            }
        }
    }
}
