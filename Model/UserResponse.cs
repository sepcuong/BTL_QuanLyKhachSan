namespace Model;

public sealed class UserResponse
{
    public string User_Id { get; init; } = null!;
    public string? Hoten { get; init; }
    public DateTime? Ngaysinh { get; init; }
    public string? Diachi { get; init; }
    public string? Gioitinh { get; init; }
    public string? Email { get; init; }
    public string? Taikhoan { get; init; }
    public string? Role { get; init; }
    public string? Image_Url { get; init; }

    public static UserResponse From(User user) => new()
    {
        User_Id = user.User_Id,
        Hoten = user.Hoten,
        Ngaysinh = user.Ngaysinh,
        Diachi = user.Diachi,
        Gioitinh = user.Gioitinh,
        Email = user.Email,
        Taikhoan = user.Taikhoan,
        Role = user.Role,
        Image_Url = user.Image_Url
    };
}