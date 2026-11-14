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
        public IActionResult CreateDatPhong([FromBody] List<DatPhong> thongtin)
        {
            foreach (var item in thongtin)
            {
                item.Ma_Dat_Phong = Guid.NewGuid().ToString();
                item.Ma_Khach_Hang = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                item.Ngay_Dat = DateTime.Now;
                var result = _datPhongBusiness.Create(item);
                if (!result)
                    return BadRequest(new { message = "Tạo đặt phòng thất bại." });
            }
            //thongtin.Ma_Dat_Phong = Guid.NewGuid().ToString();
            //thongtin.Ma_Khach_Hang = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            //thongtin.Ngay_Dat = DateTime.Now;
            //var result = _datPhongBusiness.Create(item);
            return Ok(new
            {
                success = true,
                message = "Tạo đặt phòng thành công."
            });
        }
        [Route("get-dat-phong-by-khach-hang/{khachHang_Id}")]
        [HttpGet]
        public IActionResult GetDatPhongByKhachHang(string khachHang_Id)
        {
            var ownCustomerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.Equals(khachHang_Id, ownCustomerId, StringComparison.Ordinal))
                return Forbid();

            return Ok(_datPhongBusiness.GetByKhachHang(ownCustomerId));
        }
    }
}
