using BLL;
using BLL.Interfaces;
using DAL;
using DAL.Helper;
using DAL.Helper.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Sinks;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console() // log ra console
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // log ra file
    .CreateLogger();

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>() ?? throw new InvalidOperationException("Chưa cấu hình AppSettings.");
var key = Encoding.UTF8.GetBytes(appSettings.Secret);

builder.Host.UseSerilog();

builder.Services.AddMemoryCache();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddTransient<IDatabaseHelper, DatabaseHelper>();
builder.Services.AddTransient<IUserBusiness, UserBusiness>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IKhachHangBusiness, KhachHangBusiness>();
builder.Services.AddTransient<IKhachHangRepository, KhachHangRepository>();
builder.Services.AddTransient<ILoaiPhongBusiness, LoaiPhongBusiness>();
builder.Services.AddTransient<ILoaiPhongRepository, LoaiPhongRepository>();
builder.Services.AddTransient<IPhongBusiness, PhongBusiness>();
builder.Services.AddTransient<IPhongRepository, PhongRepository>();
builder.Services.AddTransient<IGiaPhongBusiness, GiaPhongBusiness>();
builder.Services.AddTransient<IGiaPhongRepository, GiaPhongRepository>();
builder.Services.AddTransient<IDatPhongBusiness, DatPhongBusiness>();
builder.Services.AddTransient<IDatPhongRepository, DatPhongRepository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
