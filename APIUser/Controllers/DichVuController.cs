using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace APIUser.Controllers
{
    [ApiController]
    [Route("api-user/[controller]")]
    public class DichVuController : ControllerBase
    {
        private readonly IDichVuBusiness _dichvubusiness;
        public DichVuController(IDichVuBusiness dichvubusiness)
        {
            _dichvubusiness = dichvubusiness;
        }
        [Route("get-all-dich-vu")]
        [HttpGet]
        public List<DichVu> GetAllDichVu()
        {
            return _dichvubusiness.GetAllDichVu();
        }
    }
}
