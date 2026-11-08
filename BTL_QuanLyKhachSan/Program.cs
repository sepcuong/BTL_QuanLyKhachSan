using BLL;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using DAL;
using DAL.Helper;
using DAL.Helper.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Sinks;
using System.Text;
using Microsoft.OpenApi.Models;
using API;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console() // log ra console
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // log ra file
    .CreateLogger();

var secret = builder.Configuration["AppSettings:Secret"];
if (string.IsNullOrWhiteSpace(secret) || Encoding.UTF8.GetByteCount(secret) < 32)
    throw new InvalidOperationException("Cấu hình AppSettings:Secret chưa hợp lệ.");
var key = Encoding.UTF8.GetBytes(secret);

builder.Host.UseSerilog();

builder.Services.AddMemoryCache();
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("ConfiguredOrigins", policy => policy.WithOrigins(allowedOrigins).AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddScoped<IDatabaseHelper, DatabaseHelper>();
builder.Services.AddScoped<IUserBusiness, UserBusiness>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IKhachHangBusiness, KhachHangBusiness>();
builder.Services.AddScoped<IKhachHangRepository, KhachHangRepository>();
builder.Services.AddScoped<ILoaiPhongBusiness, LoaiPhongBusiness>();
builder.Services.AddScoped<ILoaiPhongRepository, LoaiPhongRepository>();
builder.Services.AddScoped<IPhongBusiness, PhongBusiness>();
builder.Services.AddScoped<IPhongRepository, PhongRepository>();
builder.Services.AddScoped<IGiaPhongBusiness, GiaPhongBusiness>();
builder.Services.AddScoped<IGiaPhongRepository, GiaPhongRepository>();
builder.Services.AddScoped<IDichVuBusiness, DichVuBusiness>();
builder.Services.AddScoped<IDichVuRepository, DichVuRepository>();
builder.Services.AddScoped<IDatPhongBusiness, DatPhongBusiness>();
builder.Services.AddScoped<IDatPhongRepository, DatPhongRepository>();
builder.Services.AddScoped<IChiTietDvBusiness, ChiTietDvBusiness>();
builder.Services.AddScoped<IChiTietDvRepository, ChiTietDvRepository>();
builder.Services.AddScoped<IHoaDonBusiness, HoaDonBusiness>();
builder.Services.AddScoped<IHoaDonRepository, HoaDonRepository>();
builder.Services.AddControllers();
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        [new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }] = Array.Empty<string>()
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,   // Set true nếu muốn kiểm tra Issuer
        ValidateAudience = false, // Set true nếu muốn kiểm tra Audience
        ValidateLifetime = true,  // Kiểm tra token hết hạn chưa
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ApiExceptionMiddleware>();
app.UseCors("ConfiguredOrigins");

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
