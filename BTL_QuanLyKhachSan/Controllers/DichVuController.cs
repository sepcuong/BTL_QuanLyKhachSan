using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Model;

namespace API.Controllers
{
    [ApiController]
    [Route("api-admin/[controller]")]
    [Route("api/[controller]")]
    public class DichVuController : ControllerBase
    {
        private readonly IDichVuBusiness _dichvubusiness;
        public DichVuController(IDichVuBusiness dichvubusiness)
        {
            _dichvubusiness = dichvubusiness;
        }
        [Route("get-all-dich-vu")]
        [HttpGet]
        public List<DichVu> GetAllDichVu()
        {
            return _dichvubusiness.GetAllDichVu();
        }
        [Route("create-dich-vu")]
        [HttpPost]
        public IActionResult Create([FromBody] DichVu thongtin)
        {
            try
            {
                thongtin.Ma_Dich_Vu = Guid.NewGuid().ToString();
                var result =  _dichvubusiness.Create(thongtin);
                return Ok(result);
            } catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại"))
                {
                    return BadRequest(new { message = ex.Message });
                }
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        [Route("get-dich-vu/{id}")]
        [HttpGet]
        public IActionResult GetById(string id)
        {
            try
            {
                var item = _dichvubusiness.GetDichVuById(id);
                if (item == null) return NotFound();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        [Route("update-dich-vu")]
        [HttpPost]
        public IActionResult Update([FromBody] DichVu thongtin)
        {
            try
            {
                if (thongtin == null)
                    return BadRequest(new { message = "Dữ liệu rỗng." });
                if (string.IsNullOrEmpty(thongtin.Ma_Dich_Vu))
                    return BadRequest(new { message = "Ma_Dich_Vu không được để trống." });

                var result = _dichvubusiness.Update(thongtin);
                if (!result)
                    return NotFound(new { message = "Cập nhật thất bại: không tìm thấy dịch vụ." });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        [Route("delete-dich-vu")]
        [HttpPost]
        public IActionResult Delete([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                string maDichVu_Id = "";
                if (formData.ContainsKey("maDichVu_Id") && formData["maDichVu_Id"] != null)
                {
                    maDichVu_Id = formData["maDichVu_Id"].ToString();
                }
                var result = _dichvubusiness.Delete(maDichVu_Id);
                if (!result)
                    return NotFound(new { message = "Xóa thất bại: không tìm thấy dịch vụ." });
                return Ok(new { message = "Xóa thành công" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        [Route("search")]
        [HttpGet]
        public IActionResult Search([FromQuery] string keyword)
        {
            try
            {
                var list = _dichvubusiness.Search(keyword);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        
    }
}
