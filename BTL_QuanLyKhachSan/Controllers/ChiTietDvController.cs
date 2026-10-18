using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Model;

namespace API.Controllers
{
    [ApiController]
    [Route("api-admin/[controller]")]
    [Route("api/[controller]")]
    public class ChiTietDvController : ControllerBase
    {
        private readonly IChiTietDvBusiness _business;
        public ChiTietDvController(IChiTietDvBusiness business)
        {
            _business = business;
        }

        [Route("get-all-chi-tiet-dv")]
        [HttpGet]
        public List<ChiTietSuDungDv> GetAll()
        {
            return _business.GetAllChiTietSuDungDv();
        }

        [Route("get-by-id/{id}")]
        [HttpGet]
        public IActionResult GetById(string id)
        {
            var item = _business.GetChiTietDvById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [Route("get-by-datphong/{maDatPhong}")]
        [HttpGet]
        public List<ChiTietSuDungDv> GetByDatPhong(string maDatPhong)
        {
            return _business.GetByDatPhong(maDatPhong);
        }

        [Route("create-chi-tiet-dv")]
        [HttpPost]
        public IActionResult Create([FromBody] ChiTietSuDungDv thongtin)
        {
            try
            {
                thongtin.Ma_Chi_Tiet_Dv = Guid.NewGuid().ToString();
                var result = _business.Create(thongtin);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        [Route("update-chi-tiet-dv")]
        [HttpPost]
        public IActionResult Update([FromBody] ChiTietSuDungDv thongtin)
        {
            try
            {
                var result = _business.Update(thongtin);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        [Route("delete-chi-tiet-dv")]
        [HttpPost]
        public IActionResult Delete([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                string maChiTietDv_Id = "";
                if (formData.ContainsKey("maChiTietDv_Id") && formData["maChiTietDv_Id"] != null)
                {
                    maChiTietDv_Id = formData["maChiTietDv_Id"].ToString();
                }
                else
                {
                    return BadRequest(new { message = "maChiTietDv_Id không được để trống." });
                }
                var result = _business.Delete(maChiTietDv_Id);
                if (result)
                {
                    return Ok(new { message = "Xóa chi tiết dịch vụ thành công!" });
                }
                else
                {
                    return NotFound(new { message = "Không tìm thấy chi tiết dịch vụ để xóa." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}
