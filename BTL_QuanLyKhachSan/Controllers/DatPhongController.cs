using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Authorize(Roles = "Admin,LeTan")]
    [ApiController]
    [Route("api-admin/[controller]")]
    [Route("api/[controller]")]
    public class DatPhongController : ControllerBase
    {
        private readonly IDatPhongBusiness _datPhongBusiness;
        public DatPhongController(IDatPhongBusiness datPhongBusiness)
        {
            _datPhongBusiness = datPhongBusiness;
        }

        [Route("get-all-dat-phong")]
        [HttpGet]
        public List<DatPhong> GetAllDatPhong()
        {
            return _datPhongBusiness.GetAllDatPhong();
        }
        [Route("create-dat-phong")]
        [HttpPost]
        public IActionResult CreateDatPhong([FromBody] DatPhong thongtin)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(thongtin.Ma_Dat_Phong))
                    thongtin.Ma_Dat_Phong = Guid.NewGuid().ToString();
                thongtin.Ngay_Dat = DateTime.Now;
                var result = _datPhongBusiness.Create(thongtin);
                return Ok(new
                {
                    success = true,
                    message = "Tạo đặt phòng thành công."
                });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại") || ex.Message.Contains("không hợp lệ") || ex.Message.Contains("không thể") || ex.Message.Contains("không được"))
                {
                    return BadRequest(new { success = false, message = "Thông tin đặt phòng không hợp lệ." });
                }
                throw;
            }
        }
        [Route("update-dat-phong")]
        [HttpPost]
        public IActionResult UpdateDatPhong([FromBody] DatPhong thongtin)
        {
            try
            {
                var result = _datPhongBusiness.Update(thongtin);
                return Ok(new
                {
                    success = true,
                    message = "Cập nhật đặt phòng thành công."
                });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("đã tồn tại") || ex.Message.Contains("không hợp lệ") || ex.Message.Contains("không thể") || ex.Message.Contains("không được") || ex.Message.Contains("Không tìm thấy"))
                {
                    return BadRequest(new { success = false, message = "Không thể cập nhật đặt phòng." });
                }
                throw;
            }
        }
        [Route("delete-dat-phong")]
        [HttpPost]
        public IActionResult DeleteDatPhong([FromBody] DatPhongIdRequest request)
        {
            try
            {
                string maDatPhong_Id = request.GetId();
                if (string.IsNullOrWhiteSpace(maDatPhong_Id))
                {
                    return BadRequest(new { success = false, message = "Mã đặt phòng không được để trống." });
                }
                var result = _datPhongBusiness.Delete(maDatPhong_Id);
                return Ok(new { success = true, message = "Xóa đặt phòng thành công." });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("không được"))
                {
                    return BadRequest(new { success = false, message = "Không thể xóa đặt phòng." });
                }
                throw;
            }
        }
        [Route("cancel-dat-phong")]
        [HttpPost]
        public IActionResult CancelDatPhong([FromBody] DatPhongIdRequest request)
        {
            try
            {
                string maDatPhong_Id = request.GetId();
                if (string.IsNullOrWhiteSpace(maDatPhong_Id))
                {
                    return BadRequest(new { success = false, message = "Mã đặt phòng không được để trống." });
                }
                var result = _datPhongBusiness.Cancel(maDatPhong_Id);
                return Ok(new { success = true, message = "Hủy đặt phòng thành công." });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("không thể") || ex.Message.Contains("không được") || ex.Message.Contains("Không tìm thấy"))
                {
                    return BadRequest(new { success = false, message = "Không thể hủy đặt phòng." });
                }
                throw;
            }
        }
        [Route("checkin-dat-phong")]
        [HttpPost]
        public IActionResult CheckInDatPhong([FromBody] DatPhongIdRequest request)
        {
            try
            {
                string maDatPhong_Id = request.GetId();
                if (string.IsNullOrWhiteSpace(maDatPhong_Id))
                {
                    return BadRequest(new { success = false, message = "Mã đặt phòng không được để trống." });
                }
                var result = _datPhongBusiness.CheckIn(maDatPhong_Id);
                return Ok(new { success = true, message = "Check-in đặt phòng thành công." });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("không thể") || ex.Message.Contains("không được") || ex.Message.Contains("Không tìm thấy"))
                {
                    return BadRequest(new { success = false, message = "Không thể check-in đặt phòng." });
                }
                throw;
            }
        }
        [Route("checkout-dat-phong")]
        [HttpPost]
        public IActionResult CheckOutDatPhong([FromBody] DatPhongIdRequest request)
        {
            try
            {
                string maDatPhong_Id = request.GetId();
                if (string.IsNullOrWhiteSpace(maDatPhong_Id))
                {
                    return BadRequest(new { success = false, message = "Mã đặt phòng không được để trống." });
                }

                var result = _datPhongBusiness.CheckOut(maDatPhong_Id);
                return Ok(new { success = true, message = "Check-out đặt phòng thành công." });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("không thể") || ex.Message.Contains("không được") || ex.Message.Contains("Không tìm thấy"))
                {
                    return BadRequest(new { success = false, message = "Không thể check-out đặt phòng." });
                }
                throw;
            }
        }
        [Route("search-dat-phong")]
        [HttpGet]
        public List<DatPhong> SearchDatPhong([FromQuery] string keyword)
        {
            return _datPhongBusiness.Search(keyword);
        }
        [Route("get-dat-phong-by-id/{phong_Id}")]
        [HttpGet]
        public IActionResult GetDatPhongById(string phong_Id)
        {
            try
            {
                var result = _datPhongBusiness.GetById(phong_Id);
                if (result == null)
                {
                    return NotFound(new { success = false, message = $"Không tìm thấy đặt phòng với mã: {phong_Id}" });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("get-dat-phong-by-khach-hang/{khachHang_Id}")]
        [HttpGet]
        public IActionResult GetDatPhongByKhachHang(string khachHang_Id)
        {
            try
            {
                var result = _datPhongBusiness.GetByKhachHang(khachHang_Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Route("get-dat-phong-by-phong/{phong_Id}")]
        [HttpGet]
        public IActionResult GetDatPhongByPhong(string phong_Id)
        {
            try
            {
                var result = _datPhongBusiness.GetByPhong(phong_Id);
                if (result == null)
                {
                    return NotFound(new { success = false, message = $"Không tìm thấy đặt phòng cho phòng với mã: {phong_Id}." });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

public sealed class DatPhongIdRequest : IValidatableObject
{
    public string? maDatPhong_Id { get; set; }
    public string? maDatPhongId { get; set; }
    public string? Ma_Dat_Phong { get; set; }
    public string? id { get; set; }

    public string GetId() => new[] { maDatPhong_Id, maDatPhongId, Ma_Dat_Phong, id }
        .FirstOrDefault(value => !string.IsNullOrWhiteSpace(value)) ?? string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(GetId()))
            yield return new ValidationResult("Mã đặt phòng là bắt buộc.", new[] { nameof(maDatPhong_Id) });
    }
}
}
