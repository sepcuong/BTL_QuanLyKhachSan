using BLL;
using BLL.Interfaces;
using DAL;
using DAL.Helper;
using DAL.Helper.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 2. Đăng ký Dependency Injection cho DatabaseHelper, DAL và BLL
builder.Services.AddTransient<IDatabaseHelper, DatabaseHelper>();

builder.Services.AddTransient<IChiTietDvRepository, ChiTietDvRepository>();
builder.Services.AddTransient<IChiTietDvBusiness, ChiTietDvBusiness>();

builder.Services.AddTransient<IDatPhongRepository, DatPhongRepository>();
builder.Services.AddTransient<IDatPhongBusiness, DatPhongBusiness>();

builder.Services.AddTransient<IDichVuRepository, DichVuRepository>();
builder.Services.AddTransient<IDichVuBusiness, DichVuBusiness>();

builder.Services.AddTransient<IGiaPhongRepository, GiaPhongRepository>();
builder.Services.AddTransient<IGiaPhongBusiness, GiaPhongBusiness>();

builder.Services.AddTransient<IHoaDonRepository, HoaDonRepository>();
builder.Services.AddTransient<IHoaDonBusiness, HoaDonBusiness>();

builder.Services.AddTransient<IKhachHangRepository, KhachHangRepository>();
builder.Services.AddTransient<IKhachHangBusiness, KhachHangBusiness>();

builder.Services.AddTransient<ILoaiPhongRepository, LoaiPhongRepository>();
builder.Services.AddTransient<ILoaiPhongBusiness, LoaiPhongBusiness>();

builder.Services.AddTransient<IPhongRepository, PhongRepository>();
builder.Services.AddTransient<IPhongBusiness, PhongBusiness>();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserBusiness, UserBusiness>();

// 3. Bắt buộc phải có AddControllers và Swagger để hiện Path
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseAuthorization();

// 4. Bắt buộc phải có dòng này để ánh xạ các đường dẫn trong Controllers
app.MapControllers();

app.Run();