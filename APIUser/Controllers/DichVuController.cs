using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace APIUser.Controllers
{
    [ApiController]
    [Route("api-user/[controller]")]
    public class DichVuController : ControllerBase
    {
        private readonly IDichVuBusiness _dichvubusiness;
        public DichVuController(IDichVuBusiness dichvubusiness)
        {
            _dichvubusiness = dichvubusiness;
        }
        [AllowAnonymous]
        [Route("get-all-dich-vu")]
        [HttpGet]
        public List<DichVu> GetAllDichVu()
        {
            return _dichvubusiness.GetAllDichVu();
        }
        [Route("create-dich-vu")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] DichVu thongtin)
        {
            if (thongtin == null)
                return BadRequest(new { success = false, message = "Dữ liệu dịch vụ không được để trống." });

            try
            {
                thongtin.Ma_Dich_Vu = Guid.NewGuid().ToString();
                var result = _dichvubusiness.Create(thongtin);
                if (!result)
                    return BadRequest(new { success = false, message = "Thêm dịch vụ thất bại." });

                return Ok(new
                {
                    success = true,
                    message = "Thêm dịch vụ thành công."
                });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại"))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Dữ liệu dịch vụ đã tồn tại."
                    });
                }
                throw;
            }
        }

        [Route("get-dich-vu/{id}")]
        [HttpGet]
        [Authorize(Roles = "Khach")]
        public IActionResult GetById(string id)
        {
            var item = _dichvubusiness.GetDichVuById(id);
            if (item == null)
                return NotFound(new { success = false, message = "Không tìm thấy dịch vụ." });

            return Ok(item);
        }

        [Route("update-dich-vu")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Update([FromBody] DichVu thongtin)
        {
            if (thongtin == null)
                return BadRequest(new { success = false, message = "Dữ liệu dịch vụ không được để trống." });
            if (string.IsNullOrWhiteSpace(thongtin.Ma_Dich_Vu))
                return BadRequest(new { success = false, message = "Mã dịch vụ không được để trống." });

            if (!_dichvubusiness.Update(thongtin))
                return NotFound(new { success = false, message = "Không tìm thấy dịch vụ để cập nhật." });

            return Ok(new { success = true, message = "Cập nhật dịch vụ thành công." });
        }

        [Route("search")]
        [HttpGet]
        [Authorize(Roles = "Khach")]
        public IActionResult Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest(new { success = false, message = "Từ khóa tìm kiếm không được để trống." });

            return Ok(_dichvubusiness.Search(keyword));
        }
    }
}