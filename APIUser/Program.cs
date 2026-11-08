using BLL;
using BLL.Interfaces;
using DAL;
using DAL.Helper;
using DAL.Helper.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var secret = builder.Configuration["AppSettings:Secret"];
if (string.IsNullOrWhiteSpace(secret) || Encoding.UTF8.GetByteCount(secret) < 32)
    throw new InvalidOperationException("Cấu hình AppSettings:Secret chưa hợp lệ.");
var key = Encoding.UTF8.GetBytes(secret);

Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.Console().WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day).CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ConfiguredOrigins", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// 2. Đăng ký Dependency Injection cho DatabaseHelper, DAL và BLL
builder.Services.AddScoped<IDatabaseHelper, DatabaseHelper>();

builder.Services.AddScoped<IChiTietDvRepository, ChiTietDvRepository>();
builder.Services.AddScoped<IChiTietDvBusiness, ChiTietDvBusiness>();

builder.Services.AddScoped<IDatPhongRepository, DatPhongRepository>();
builder.Services.AddScoped<IDatPhongBusiness, DatPhongBusiness>();

builder.Services.AddScoped<IDichVuRepository, DichVuRepository>();
builder.Services.AddScoped<IDichVuBusiness, DichVuBusiness>();

builder.Services.AddScoped<IGiaPhongRepository, GiaPhongRepository>();
builder.Services.AddScoped<IGiaPhongBusiness, GiaPhongBusiness>();

builder.Services.AddScoped<IHoaDonRepository, HoaDonRepository>();
builder.Services.AddScoped<IHoaDonBusiness, HoaDonBusiness>();

builder.Services.AddScoped<IKhachHangRepository, KhachHangRepository>();
builder.Services.AddScoped<IKhachHangBusiness, KhachHangBusiness>();

builder.Services.AddScoped<ILoaiPhongRepository, LoaiPhongRepository>();
builder.Services.AddScoped<ILoaiPhongBusiness, LoaiPhongBusiness>();

builder.Services.AddScoped<IPhongRepository, PhongRepository>();
builder.Services.AddScoped<IPhongBusiness, PhongBusiness>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserBusiness, UserBusiness>();

// 3. Bắt buộc phải có AddControllers và Swagger để hiện Path
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization", Type = SecuritySchemeType.Http, Scheme = "bearer", BearerFormat = "JWT", In = ParameterLocation.Header
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        [new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }] = Array.Empty<string>()
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<APIUser.ApiExceptionMiddleware>();
app.UseCors("ConfiguredOrigins");

app.UseAuthentication();
app.UseAuthorization();

// 4. Bắt buộc phải có dòng này để ánh xạ các đường dẫn trong Controllers
app.MapControllers();

app.Run();