
using Microsoft.EntityFrameworkCore;
using DangKyService.Data;
using DangKyService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Đăng ký HTTP clients để giao tiếp giữa các service
var sinhVienUrl = builder.Configuration["ServiceUrls:SinhVien"];
var deTaiUrl = builder.Configuration["ServiceUrls:DeTai"];

builder.Services.AddHttpClient("SinhVienApi", client =>
{
    client.BaseAddress = new Uri(sinhVienUrl!);
});

builder.Services.AddHttpClient("DeTaiApi", client =>
{
    client.BaseAddress = new Uri(deTaiUrl!);
});

builder.Services.AddScoped<IDangKyService, DangKyService.Services.DangKyService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();