using System;
using System.Collections.Generic;

namespace Model;

public partial class User
{
    public string User_Id { get; set; } = null!;

    public string? Hoten { get; set; }

    public DateTime? Ngaysinh { get; set; }

    public string? Diachi { get; set; }

    public string? Gioitinh { get; set; }

    public string? Email { get; set; }

    public string? Taikhoan { get; set; }

    public string? Matkhau { get; set; }

    public string? Role { get; set; }
    public string? Token { get; set; }

    public string? Image_Url { get; set; }
}
