using BLL;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Model;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Authorize(Roles = "Admin,LeTan")]
    [ApiController]
    [Route("api-admin/[controller]")]
    [Route("api/[controller]")]
    public class GiaPhongController : ControllerBase
    {
        private readonly IGiaPhongBusiness _giaPhongBusiness;
        public GiaPhongController(IGiaPhongBusiness giaPhongBusiness)
        {
            _giaPhongBusiness = giaPhongBusiness;
        }
        [Route("get-all-gia-phong")]
        [HttpGet]
        public List<GiaPhong> GetAllGiaPhong()
        {
            return _giaPhongBusiness.GetAllGiaPhong();
        }
        [Route("create-gia-phong")]
        [HttpPost]
        public IActionResult CreateGiaPhong([FromBody] GiaPhong thongtin)
        {
            try
            {
                thongtin.Ma_Gia = Guid.NewGuid().ToString(); // Tạo mã giá mới
                var result = _giaPhongBusiness.Create(thongtin);
                return Ok(new { success = true, message = "Thêm giá phòng thành công." });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại"))
                {
                    return BadRequest(new { success = false, message = "Dữ liệu giá phòng đã tồn tại." });
                }
                throw;
            }
        }
        [Route("update-gia-phong")]
        [HttpPost]
        public IActionResult UpdateGiaPhong([FromBody] GiaPhong thongtin)
        {
            try
            {
                var result = _giaPhongBusiness.Update(thongtin);
                return Ok(new { success = true, message = "Cập nhật giá phòng thành công." });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại"))
                {
                    return BadRequest(new { success = false, message = "Dữ liệu giá phòng đã tồn tại." });
                }
                throw;
            }
        }
        [Route("delete-gia-phong")]
        [HttpPost]
        public IActionResult DeleteGiaPhong([FromBody] DeleteGiaPhongRequest request)
        {
            try
            {
                var isDelete = _giaPhongBusiness.Delete(request.maGia_Id);
                if (isDelete)
                {
                    return Ok(new { success = true, message = "Xóa giá phòng thành công." });
                }
                else
                {
                    return NotFound(new { success = false, message = "Không tìm thấy giá phòng để xóa." });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("get-gia-phong-by-id/{maGia}")]
        [HttpGet]
        public IActionResult GetGiaPhongById(string maGia)
        {
            try
            {
                var result = _giaPhongBusiness.GetGiaPhongById(maGia);
                if (result == null)
                {
                    return NotFound(new { success = false, message = $"Không tìm thấy giá phòng với mã: {maGia}." });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("get-gia-phong-by-loai-phong/{maLoaiPhong}")]
        [HttpGet]
        public IActionResult GetGiaPhongByLoaiPhong(string maLoaiPhong)
        {
            try
            {
                var result = _giaPhongBusiness.GetGiaPhongByMaLoaiPhong(maLoaiPhong);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("gia-phong-get-current")]
        [HttpGet]
        public IActionResult GiaPhong_GetCurrent([FromQuery] string maLoaiPhong, [FromQuery] DateTime ngay)
        {
            try
            {
                var result = _giaPhongBusiness.GiaPhong_GetCurrent(maLoaiPhong, ngay);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}

public sealed class DeleteGiaPhongRequest
{
    [Required]
    public string maGia_Id { get; set; } = string.Empty;
}
