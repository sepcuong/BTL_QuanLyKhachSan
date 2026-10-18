using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Model;

namespace API.Controllers
{
    [ApiController]
    [Route("api-admin/[controller]")]
    [Route("api/[controller]")]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonBusiness _business;
        public HoaDonController(IHoaDonBusiness business)
        {
            _business = business;
        }

        [Route("get-all-hoa-don")]
        [HttpGet]
        public List<HoaDonKhachSan> GetAll()
        {
            return _business.GetAllHoaDon();
        }

        [Route("get-by-id/{id}")]
        [HttpGet]
        public IActionResult GetById(string id)
        {
            var item = _business.GetHoaDonById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [Route("get-by-datphong/{maDatPhong}")]
        [HttpGet]
        public IActionResult GetByDatPhong(string maDatPhong)
        {
            var item = _business.GetByDatPhong(maDatPhong);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [Route("search")]
        [HttpGet]
        public IActionResult Search([FromQuery] string keyword)
        {
            var list = _business.Search(keyword);
            return Ok(list);
        }

        [Route("create-hoa-don")]
        [HttpPost]
        public IActionResult Create([FromBody] HoaDonKhachSan thongtin)
        {
            try
            {
                thongtin.Ma_Hoa_Don = Guid.NewGuid().ToString();
                var result = _business.Create(thongtin);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        [Route("update-hoa-don")]
        [HttpPost]
        public IActionResult Update([FromBody] HoaDonKhachSan thongtin)
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

        [Route("delete-hoa-don")]
        [HttpPost]
        public IActionResult Delete([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                string maHoaDon_Id = "";
                if (formData.Keys.Contains("maHoaDon_Id") && !string.IsNullOrEmpty(Convert.ToString(formData["maHoaDon_Id"])))
                {
                    maHoaDon_Id = Convert.ToString(formData["maHoaDon_Id"]);
                }
                var isDelete = _business.Delete(maHoaDon_Id);
                if (isDelete)
                {
                    return Ok(new { message = "Xóa thành công" });
                }
                else
                {
                    return NotFound(new { message = "Xóa thất bại: không tìm thấy hóa đơn." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}
