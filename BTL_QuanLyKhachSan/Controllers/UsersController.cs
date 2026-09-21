using BLL;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace API.Controllers
{
    //[Authorize]
    [ApiController]
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
                bool isCreate = _userBusiness.Create(thongtin);
                if (isCreate)
                    return Ok(thongtin);
                return BadRequest(new { message = "Không thể tạo người dùng!" });
                //return thongtin;
            } catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại"))
                {
                    return BadRequest(new { message = ex.Message });
                }
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        [Route("update-user")]
        [HttpPost]
        public User UpdateUser([FromBody] User thongtin)
        {
            try
            {
                _userBusiness.Update(thongtin);
                return thongtin;
            } catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi cập nhật người dùng: " + ex.Message);
            }
        }
        [Route("delete-user")]
        [HttpPost]
        public IActionResult DeleteUser([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                string user_Id = "";
                if (formData.Keys.Contains("user_Id") && !string.IsNullOrEmpty(Convert.ToString(formData["user_Id"]))) { user_Id = Convert.ToString(formData["user_Id"]); }
                bool isDelete = _userBusiness.Delete(user_Id);
                if (isDelete)
                {
                    return Ok(new { message = "Xóa người dùng thành công!" });
                }
                else
                {
                    return NotFound(new { message = "Không tìm thấy người dùng để xóa!" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi xảy ra khi xóa người dùng!", error = ex.Message });
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
                    return NotFound(new { message = "Không tìm thấy người dùng!" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi xảy ra khi lấy thông tin người dùng!", error = ex.Message });
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
            catch (Exception ex)
            {
                return BadRequest(new { message = "Có lỗi xảy ra khi tìm kiếm người dùng!", error = ex.Message });
            }
        }
    }
}
