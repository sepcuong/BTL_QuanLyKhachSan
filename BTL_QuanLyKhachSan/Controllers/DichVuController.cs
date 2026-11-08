using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Model;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Authorize(Roles = "Admin,LeTan")]
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
                var result = _dichvubusiness.Create(thongtin);
                return Ok(new
                {
                    success = true,
                    message = "Thêm dịch vụ thành công."
                });
            } catch (Exception ex)
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
        public IActionResult GetById(string id)
        {
            try
            {
                var item = _dichvubusiness.GetDichVuById(id);
                if (item == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy dịch vụ." });
                }
                return Ok(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("update-dich-vu")]
        [HttpPost]
        public IActionResult Update([FromBody] DichVu thongtin)
        {
            try
            {
                if (thongtin == null)
                    return BadRequest(new { success = false, message = "Dữ liệu dịch vụ không được để trống." });
                if (string.IsNullOrEmpty(thongtin.Ma_Dich_Vu))
                    return BadRequest(new { success = false, message = "Mã dịch vụ không được để trống." });

                var result = _dichvubusiness.Update(thongtin);
                return Ok(new
                {
                    success = true,
                    message = "Cập nhật dịch vụ thành công."
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("delete-dich-vu")]
        [HttpPost]
        public IActionResult Delete([FromBody] DeleteDichVuRequest request)
        {
            try
            {
                var result = _dichvubusiness.Delete(request.maDichVu_Id);
                if (!result)
                    return NotFound(new { success = false, message = "Không tìm thấy dịch vụ để xóa." });
                return Ok(new { success = true, message = "Xóa dịch vụ thành công." });

            }
            catch (Exception ex)
            {
                throw;
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
                throw;
            }

        }
        
    }
}

public sealed class DeleteDichVuRequest
{
    [Required]
    public string maDichVu_Id { get; set; } = string.Empty;
}