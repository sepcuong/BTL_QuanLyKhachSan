using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Security.Claims;

namespace APIUser.Controllers
{
    [ApiController]
    [Authorize(Roles = "Khach")]
    [Route("api-user/[controller]")]
    public class DatPhongController : ControllerBase
    {
        private readonly IDatPhongBusiness _datPhongBusiness;
        public DatPhongController(IDatPhongBusiness datPhongBusiness)
        {
            _datPhongBusiness = datPhongBusiness;
        }
        [Route("create-dat-phong")]
        [HttpPost]
        public IActionResult CreateDatPhong([FromBody] DatPhong thongtin)
        {
            thongtin.Ma_Dat_Phong = Guid.NewGuid().ToString();
            thongtin.Ma_Khach_Hang = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            thongtin.Ngay_Dat = DateTime.Now;
            var result = _datPhongBusiness.Create(thongtin);
            if (!result)
                return BadRequest(new { message = "Tạo đặt phòng thất bại." });
            return Ok(thongtin);
        }
        [Route("get-dat-phong-by-khach-hang/{khachHang_Id}")]
        [HttpGet]
        public IActionResult GetDatPhongByKhachHang(string khachHang_Id)
        {
            var ownCustomerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(_datPhongBusiness.GetByKhachHang(ownCustomerId));
        }
    }
}
