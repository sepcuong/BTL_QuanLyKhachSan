using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace APIUser.Controllers
{
    [ApiController]
    [Route("api-user/[controller]")]
    public class ChiTietDvController : ControllerBase
    {
        private readonly IChiTietDvBusiness _chiTietDvBusiness;
        public ChiTietDvController(IChiTietDvBusiness chiTietDvBusiness)
        {
            _chiTietDvBusiness = chiTietDvBusiness;
        }
        [Route("get-by-id/{id}")]
        [HttpGet]
        public IActionResult GetById(string id)
        {
            var item = _chiTietDvBusiness.GetChiTietDvById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [Route("get-by-datphong/{maDatPhong}")]
        [HttpGet]
        public List<ChiTietSuDungDv> GetByDatPhong(string maDatPhong)
        {
            return _chiTietDvBusiness.GetByDatPhong(maDatPhong);
        }

        [Route("create-chi-tiet-dv")]
        [HttpPost]
        public IActionResult Create([FromBody] ChiTietSuDungDv thongtin)
        {
            try
            {
                thongtin.Ma_Chi_Tiet_Dv = Guid.NewGuid().ToString();
                var result = _chiTietDvBusiness.Create(thongtin);
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
                var result = _chiTietDvBusiness.Update(thongtin);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}
