using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIUser.Controllers
{
    [ApiController]
    [Route("api-user/[controller]")]
    public class PhongController : ControllerBase
    {
        private readonly IPhongBusiness _phongBusiness;
        public PhongController(IPhongBusiness phongBusiness)
        {
            _phongBusiness = phongBusiness;
        }
        [Route("get-all-phong")]
        [HttpGet]
        public IActionResult GetAllPhong()
        {
            var result = _phongBusiness.GetAllPhong();
            return Ok(result);
        }
    }
}
