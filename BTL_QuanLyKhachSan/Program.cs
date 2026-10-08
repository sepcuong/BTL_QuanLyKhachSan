using BLL;
using BLL.Interfaces;
using DAL;
using DAL.Helper;
using DAL.Helper.Interfaces;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
