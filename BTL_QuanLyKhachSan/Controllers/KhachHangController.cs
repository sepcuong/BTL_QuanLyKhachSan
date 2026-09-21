using BLL;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace API.Controllers
{
    [ApiController]
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
                bool isCreate = _khachHangBusiness.Create(thongtin);
                if (isCreate)
                    return Ok(thongtin);
                return BadRequest(new { message = "Không thể tạo khách hàng!" });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại"))
                {
                    return BadRequest(new { message = ex.Message });
                }
                throw new Exception("Lỗi hệ thống: " + ex.Message);
            }
        }

        [Route("update-khachhang")]
        [HttpPost]
        public KhachHang UpdateKhachHang([FromBody] KhachHang thongtin)
        {
            try
            {
                _khachHangBusiness.Update(thongtin);
                return thongtin;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi cập nhật khách hàng: " + ex.Message);
            }
        }

        [Route("delete-khachhang")]
        [HttpPost]
        public IActionResult DeleteKhachHang([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                string khachHang_Id = "";
                if (formData.Keys.Contains("khachHang_Id") && !string.IsNullOrEmpty(Convert.ToString(formData["khachHang_Id"]))) { khachHang_Id = Convert.ToString(formData["khachHang_Id"]); }
                bool isDelete = _khachHangBusiness.Delete(khachHang_Id);
                if (isDelete)
                {
                    return Ok(new { message = "Xóa khách hàng thành công!" });
                }
                else
                {
                    return NotFound(new { message = "Không tìm thấy khách hàng để xóa!" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        [Route("get-by-id/{khachHang_Id}")]
        [HttpGet]
        public KhachHang GetById(string khachHang_Id)
        {
            try
            {
                return _khachHangBusiness.GetById(khachHang_Id);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy thông tin khách hàng: " + ex.Message);
            }
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
                throw new Exception("Có lỗi xảy ra khi tìm kiếm khách hàng: " + ex.Message);
            }
        }
    }
}
