using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Model;


namespace API.Controllers
{
    [ApiController]
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
                var result = _loaiPhongBusiness.Create(thongtin);
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
        [Route("update-loaiphong")]
        [HttpPost]
        public IActionResult UpdateLoaiPhong([FromBody] LoaiPhong thongtin)
        {
            try
            {
                var result = _loaiPhongBusiness.Update(thongtin);
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
        [Route("delete-loaiphong")]
        [HttpPost]
        public IActionResult DeleteLoaiPhong([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                string loaiPhong_Id = "";
                if (formData.Keys.Contains("loaiPhong_Id") && !string.IsNullOrEmpty(Convert.ToString(formData["loaiPhong_Id"]))) { loaiPhong_Id = Convert.ToString(formData["loaiPhong_Id"]); }
                var result = _loaiPhongBusiness.Delete(loaiPhong_Id);
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
        [Route("get-by-id/{loaiPhong_Id}")]
        [HttpGet]
        public LoaiPhong GetLoaiPhongById(string loaiPhong_Id)
        {
            try
            {
                var result = _loaiPhongBusiness.GetLoaiPhongById(loaiPhong_Id);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi không thể lấy dữ liệu: " + ex.Message);
            }
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
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}
