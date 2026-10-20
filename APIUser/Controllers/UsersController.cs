using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace APIUser.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api-user/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;

        public UsersController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }
        [Route("create-user")]
        [HttpPost]
        public IActionResult CreateUser([FromBody] User thongtin)
        {
            try
            {
                thongtin.User_Id = Guid.NewGuid().ToString();
                //thongtin.Matkhau = 
                var isCreate = _userBusiness.Create(thongtin);
                return Ok(UserResponse.From(thongtin));
                //return thongtin;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại"))
                {
                    return BadRequest(new { message = ex.Message });
                }
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}
