using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIUser.Controllers
{
    [ApiController]
    [Route("api-user/[controller]")]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonBusiness _business;
        public HoaDonController(IHoaDonBusiness business)
        {
            _business = business;
        }
        [Route("get-by-datphong/{maDatPhong}")]
        [HttpGet]
        public IActionResult GetByDatPhong(string maDatPhong)
        {
            var item = _business.GetByDatPhong(maDatPhong);
            if (item == null) return NotFound();
            return Ok(item);
        }
    }
}
