using BLL;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api-admin/[controller]")]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;

        public UsersController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        [Route("get-all-users")]
        [HttpGet]
        public List<User> GetAll()
        {
            return _userBusiness.GetAllUsers();
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
                return Ok(new
                {
                    success = true,
                    message = "Tạo người dùng thành công."
                });
                //return thongtin;
            } catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại"))
                {
                    return BadRequest(new { success = false, message = "Dữ liệu người dùng đã tồn tại." });
                }
                throw;
            }
        }
        [Route("update-user")]
        [HttpPost]
        public IActionResult UpdateUser([FromBody] User thongtin)
        {
            try
            {
                var isUpdated = _userBusiness.Update(thongtin);
                return Ok(new
                {
                    success = true,
                    message = "Cập nhật người dùng thành công."
                });
            } catch
            {
                throw;
            }
        }
        [Route("delete-user")]
        [HttpPost]
        public IActionResult DeleteUser([FromBody] DeleteUserRequest request)
        {
            try
            {
                bool isDelete = _userBusiness.Delete(request.user_Id);
                if (isDelete)
                {
                    return Ok(new { success = true, message = "Xóa người dùng thành công." });
                }
                else
                {
                    return NotFound(new { success = false, message = "Không tìm thấy người dùng để xóa." });
                }
            }

            catch
            {
                throw;
            }
        }
        [Route("get-by-id/{user_Id}")]
        [HttpGet]
        public IActionResult GetById(string user_Id)
        {
            try
            {
                var user = _userBusiness.GetUserById(user_Id);
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return NotFound(new { success = false, message = "Không tìm thấy người dùng." });
                }
            }
            catch
            {
                throw;
            }
        }
        [Route("search")]
        [HttpGet]
        public IActionResult Search([FromQuery] string keyword)
        {
            try
            {
                var users = _userBusiness.Search(keyword);
                return Ok(users);
            }
            catch
            {
                throw;
            }
        }
        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            var user = _userBusiness.Login(req.TaiKhoan, req.MatKhau);
            if (user == null) return Unauthorized(new { success = false, message = "Tài khoản hoặc mật khẩu không đúng." });
            // trả về user hoặc token tùy implementation
            return Ok(new 
            { 
                User_Id = user.User_Id, 
                Hoten = user.Hoten, 
                TaiKhoan = user.Taikhoan, 
                Token = user.Token 
            });
        }
    }
}

public sealed class DeleteUserRequest
{
    [Required]
    public string user_Id { get; set; } = string.Empty;
}
