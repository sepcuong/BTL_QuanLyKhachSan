using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace APIUser.Controllers
{
    [ApiController]
    [Route("api-user/[controller]")]
    public class KhachHangController : ControllerBase
    {
        private readonly IKhachHangBusiness _khachHangBusiness;
        public KhachHangController(IKhachHangBusiness khachHangBusiness)
        {
            _khachHangBusiness = khachHangBusiness;
        }
        [Route("create-khachhang")]
        [HttpPost]
        public IActionResult CreateKhachHang([FromBody] KhachHang thongtin)
        {
            try
            {
                thongtin.Ma_Khach_Hang = Guid.NewGuid().ToString();
                var isCreate = _khachHangBusiness.Create(thongtin);
                return Ok(thongtin);
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
    }
}
