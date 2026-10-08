using System.Collections.Generic;
using System.Linq;

namespace Model;

public partial class LoginRequest
{
    public string TaiKhoan { get; set; } = null!;
    public string MatKhau { get; set; } = null!;
}

