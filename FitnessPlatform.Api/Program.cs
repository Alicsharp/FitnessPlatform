using FitnessPlatform.Api.Extensions;
using FitnessPlatform.Application;
using FitnessPlatform.Infrastructure;
using Scalar.AspNetCore;
using Serilog;
using Newtonsoft.Json;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

// ۱. تنظیمات اولیه سرilog برای لاگ کردن خطاهای احتمالی هنگام بالا آمدن سرور
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("در حال راه‌اندازی سرور FitnessPlatform...");

    var builder = WebApplication.CreateBuilder(args);

    // ۲. جایگزینی سیستم لاگ پیش‌فرض دات‌نت با Serilog
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
        // لاگ‌ها به صورت روزانه در پوشه logs ذخیره می‌شوند
        .WriteTo.File("logs/fitnessplatform-.txt", rollingInterval: RollingInterval.Day));

    // تزریق سرویس‌های اصلی پروژه
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddApiAuthentication(builder.Configuration);

    // تنظیمات API و مستندات (روش جدید دات‌نت 10)
    builder.Services.AddControllers();
    builder.Services.AddOpenApi(); // سیستم رسمی و داخلی مایکروسافت

    // ==========================================
    // ۳. راه‌اندازی داشبورد سلامت (Health Checks)
    // ==========================================
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
        options.InstanceName = "FitnessPlatform_";
    });
    builder.Services.AddHealthChecks()
           .AddSqlServer(
               builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string is missing."),
               name: "SQL Server DB",
               tags: new[] { "database", "sql" })
           .AddRabbitMQ(
               // در پکیج‌های جدید، باید کانکشن را به صورت Async بسازیم و تحویل HealthCheck بدهیم
               factory: async sp =>
               {
                   var factory = new RabbitMQ.Client.ConnectionFactory
                   {
                       Uri = new Uri(builder.Configuration["RabbitMq:Host"] ?? "amqp://localhost:5672")
                   };
                   return await factory.CreateConnectionAsync();
               },
               name: "RabbitMQ Broker",
               tags: new[] { "message-broker", "rabbitmq" })
           .AddRedis(
               builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379",
               name: "Redis Cache",
               tags: new[] { "cache", "redis" });
    builder.Services.AddExceptionHandler<FitnessPlatform.Api.Middlewares.GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    var app = builder.Build();

    // ۴. ثبت خودکار لاگ‌های مربوط به تمام درخواست‌های HTTP ورودی
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        // ساخت نقشه API و اتصال آن به رابط کاربری مدرن
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    // =====================================
    app.UseHttpsRedirection();
    app.UseExceptionHandler();
    // ترتیب این دو خط بسیار مهم است
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // ۵. مپ کردن نقطه دسترسی برای مانیتورینگ سلامت
    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    app.Run();
}
catch (Exception ex)
{
    // اگر اپلیکیشن کرش کند، این لاگ ثبت می‌شود
    Log.Fatal(ex, "اپلیکیشن با خطای بحرانی متوقف شد!");
}
finally
{
    // اطمینان از نوشته شدن تمام لاگ‌های باقی‌مانده در فایل پیش از بسته شدن برنامه
    Log.CloseAndFlush();
}