using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Model;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhongController : ControllerBase
    {
        private readonly IPhongBusiness _phongBusiness;
        public PhongController(IPhongBusiness phongBusiness)
        {
            _phongBusiness = phongBusiness;
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
                var result = _phongBusiness.Create(thongtin);
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
        [Route("update-phong")]
        [HttpPost]
        public IActionResult UpdatePhong([FromBody] Phong thongtin)
        {
            try
            {
                var result = _phongBusiness.Create(thongtin);
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
        [Route("delete-phong")]
        [HttpPost]
        public IActionResult DeletePhong([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                string Phong_Id = "";
                if (formData.Keys.Contains("Phong_Id") && !string.IsNullOrEmpty(Convert.ToString(formData["Phong_Id"]))) { Phong_Id = Convert.ToString(formData["Phong_Id"]); }
                var result = _phongBusiness.Delete(Phong_Id);
                return Ok(result);
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
                string maPhong = "";
                string trangThai = "";
                if (formData.Keys.Contains("maPhong") && !string.IsNullOrEmpty(Convert.ToString(formData["maPhong"]))) { maPhong = Convert.ToString(formData["maPhong"]); }
                if (formData.Keys.Contains("trangThai") && !string.IsNullOrEmpty(Convert.ToString(formData["trangThai"]))) { trangThai = Convert.ToString(formData["trangThai"]); }
                var result = _phongBusiness.Update_TrangThai(maPhong, trangThai);
                return Ok(result);
            }
            catch (Exception ex)
            {
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
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}
