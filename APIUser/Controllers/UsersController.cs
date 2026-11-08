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
    public class UsersController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;
        public UsersController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }
        [Route("register-user")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register([FromBody] User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Taikhoan) || string.IsNullOrWhiteSpace(user.Matkhau))
                return BadRequest(new { success = false, message = "Tên đăng nhập và mật khẩu không được để trống." });

            try
            {
                user.User_Id = Guid.NewGuid().ToString();
                _userBusiness.Register(user);
                return Ok(new { success = true, message = "Đăng ký thành công." });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại"))
                {
                    return BadRequest(new { success = false, message = "Dữ liệu người dùng đã tồn tại." });
                }
                throw;
            }
        }
        [Route("login-user")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Taikhoan) || string.IsNullOrEmpty(user.Matkhau))
                return BadRequest(new { success = false, message = "Tên đăng nhập và mật khẩu không được để trống." });

            var token = _userBusiness.Login(user.Taikhoan, user.Matkhau);
            if (token == null)
            {
                return Unauthorized(new { success = false, message = "Tên đăng nhập hoặc mật khẩu không đúng." });
            }
            return Ok(new { success = true, token });
        }
        [Route("update-user")]
        [HttpPost]
        public IActionResult UpdateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest(new { success = false, message = "Dữ liệu người dùng không được để trống." });

            user.User_Id = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            user.Role = "Khach";
            if (!_userBusiness.Update(user))
                return NotFound(new { success = false, message = "Người dùng không tồn tại." });

            return Ok(new { success = true, message = "Cập nhật người dùng thành công." });
        }

        [Route("get-user-by-id/{userId}")]
        [HttpGet]
        public IActionResult GetUserById(string userId)
        {
            var ownUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.Equals(userId, ownUserId, StringComparison.Ordinal))
                return Forbid();

            var user = _userBusiness.GetUserById(userId);
            if (user == null)
            {
                return NotFound(new { success = false, message = "Người dùng không tồn tại." });
            }
            return Ok(new { success = true, user });
        }

    }
}
