using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIUser.Controllers
{
    [ApiController]
    [Authorize(Roles = "Khach")]
    [Route("api-user/[controller]")]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonBusiness _business;
        private readonly IDatPhongBusiness _datPhongBusiness;
        public HoaDonController(IHoaDonBusiness business, IDatPhongBusiness datPhongBusiness)
        {
            _business = business;
            _datPhongBusiness = datPhongBusiness;
        }
        [Route("get-by-datphong/{maDatPhong}")]
        [HttpGet]
        public IActionResult GetByDatPhong(string maDatPhong)
        {
            var booking = _datPhongBusiness.GetById(maDatPhong);
            if (booking == null) return NotFound();
            if (!string.Equals(booking.Ma_Khach_Hang, User.FindFirstValue(ClaimTypes.NameIdentifier), StringComparison.Ordinal))
                return Forbid();
            var item = _business.GetByDatPhong(maDatPhong);
            if (item == null) return NotFound();
            return Ok(item);
        }
    }
}
