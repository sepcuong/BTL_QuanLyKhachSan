using BLL;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Model;

namespace API.Controllers
{
    [Authorize(Roles = "Admin,LeTan")]
    [ApiController]
    [Route("api-admin/[controller]")]
    [Route("api/[controller]")]
    public class KhachHangController : ControllerBase
    {
        private readonly IKhachHangBusiness _khachHangBusiness;
        public KhachHangController(IKhachHangBusiness khachHangBusiness)
        {
            _khachHangBusiness = khachHangBusiness;
        }
        [Route("get-all-khachhang")]
        [HttpGet]
        public List<KhachHang> GetAll()
        {
            return _khachHangBusiness.GetAllKhachHang();
        }

        [Route("create-khachhang")]
        [HttpPost]
        public IActionResult CreateKhachHang([FromBody] KhachHang thongtin)
        {
            try
            {
                thongtin.Ma_Khach_Hang = Guid.NewGuid().ToString();
                var isCreate = _khachHangBusiness.Create(thongtin);
                return Ok(new
                {
                    success = true,
                    message = "Thêm khách hàng thành công."
                });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại"))
                {
                    return BadRequest(new { success = false, message = "Dữ liệu khách hàng đã tồn tại." });
                }
                throw;
            }
        }

        [Route("update-khachhang")]
        [HttpPost]
        public IActionResult UpdateKhachHang([FromBody] KhachHang thongtin)
        {
            try
            {
                var isUpdated = _khachHangBusiness.Update(thongtin);
                return Ok(new
                {
                    success = true,
                    message = "Cập nhật khách hàng thành công."
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("delete-khachhang")]
        [HttpPost]
        public IActionResult DeleteKhachHang([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                string khachHang_Id = "";
                if (formData.Keys.Contains("khachHang_Id") && !string.IsNullOrWhiteSpace(Convert.ToString(formData["khachHang_Id"])))
                {
                    khachHang_Id = Convert.ToString(formData["khachHang_Id"]) ?? string.Empty;
                }
                else
                {
                    return BadRequest(new { success = false, message = "Mã khách hàng không được để trống." });
                }
                var isDelete = _khachHangBusiness.Delete(khachHang_Id);
                if (isDelete)
                {
                    return Ok(new { success = true, message = "Xóa khách hàng thành công." });
                }
                else
                {
                    return NotFound(new { success = false, message = "Không tìm thấy khách hàng để xóa." });
                }
            }
            catch (Exception ex)
            {
                if (ex.Message != null && ex.Message.Contains("Không thể xóa khách hàng"))
                {
                    return BadRequest(new { success = false, message = "Không thể xóa khách hàng khi còn dữ liệu liên quan." });
                }
                throw;
            }
        }

        [Route("get-by-id/{khachHang_Id}")]
        [HttpGet]
        public IActionResult GetById(string khachHang_Id)
        {
            var khachHang = _khachHangBusiness.GetById(khachHang_Id);
            if (khachHang == null)
            {
                return NotFound(new { success = false, message = "Không tìm thấy khách hàng." });
            }
            return Ok(khachHang);
        }

        [Route("search")]
        [HttpGet]
        public List<KhachHang> Search([FromQuery] string keyword)
        {
            try
            {
                return _khachHangBusiness.Search(keyword);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
