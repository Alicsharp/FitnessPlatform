using FitnessPlatform.Api.Extensions;
using FitnessPlatform.Application;
using FitnessPlatform.Infrastructure;
using Scalar.AspNetCore; // اضافه شدن این خط
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiAuthentication(builder.Configuration);
// تنظیمات API و مستندات (روش جدید دات‌نت 10)
builder.Services.AddControllers();
builder.Services.AddOpenApi(); // سیستم رسمی و داخلی مایکروسافت

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // ساخت نقشه API و اتصال آن به رابط کاربری مدرن
    app.MapOpenApi();
    app.MapScalarApiReference();
}
// =====================================
app.UseHttpsRedirection();

// ترتیب این دو خط بسیار مهم است
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();