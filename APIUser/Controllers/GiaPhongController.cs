using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace APIUser.Controllers
{
    [ApiController]
    [Route("api-user/[controller]")]
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
    }
}
