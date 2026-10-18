using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIUser.Controllers
{
    [ApiController]
    [Route("api-user/[controller]")]
    public class LoaiPhongController : ControllerBase
    {
        private readonly ILoaiPhongBusiness _loaiPhongBusiness;
        public LoaiPhongController(ILoaiPhongBusiness loaiPhongBusiness)
        {
            _loaiPhongBusiness = loaiPhongBusiness;
        }

        [Route("get-all-loaiphong")]
        [HttpGet]
        public IActionResult GetAllLoaiPhong()
        {
            var result = _loaiPhongBusiness.GetAllLoaiPhong();
            return Ok(result);
        }
    }
}
