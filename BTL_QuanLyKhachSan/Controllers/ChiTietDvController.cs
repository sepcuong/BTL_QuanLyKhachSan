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
            if (item == null)
            {
                return NotFound(new { success = false, message = "Không tìm thấy chi tiết dịch vụ." });
            }
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
                return Ok(new
                {
                    success = true,
                    message = "Thêm chi tiết dịch vụ thành công."
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi hệ thống." });
            }
        }

        [Route("update-chi-tiet-dv")]
        [HttpPost]
        public IActionResult Update([FromBody] ChiTietSuDungDv thongtin)
        {
            try
            {
                var result = _business.Update(thongtin);
                return Ok(new
                {
                    success = true,
                    message = "Cập nhật chi tiết dịch vụ thành công."
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi hệ thống." });
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
                    return BadRequest(new { success = false, message = "maChiTietDv_Id không được để trống." });
                }
                var result = _business.Delete(maChiTietDv_Id);
                if (result)
                {
                    return Ok(new { success = true, message = "Xóa chi tiết dịch vụ thành công." });
                }
                else
                {
                    return NotFound(new { success = false, message = "Không tìm thấy chi tiết dịch vụ để xóa." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi hệ thống." });
            }
        }
    }
}
