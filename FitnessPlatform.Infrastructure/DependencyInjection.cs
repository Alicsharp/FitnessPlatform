using FitnessPlatform.Application.Interfaces;
using FitnessPlatform.Domain.Repositories;
using FitnessPlatform.Infrastructure.BackgroundJobs;
using FitnessPlatform.Infrastructure.Messaging.Consumers;
using FitnessPlatform.Infrastructure.Persistence;
using FitnessPlatform.Infrastructure.Persistence.FitnessPlatform.Infrastructure.Persistence;
using FitnessPlatform.Infrastructure.Persistence.Interceptors;
using FitnessPlatform.Infrastructure.Repositories;
using FitnessPlatform.Infrastructure.Security;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using static FitnessPlatform.Infrastructure.Repositories.UserRepository;
// در صورت نیاز فضای نام دیتابیس و ریپازیتوری‌ها را هم اضافه کن

namespace FitnessPlatform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddSingleton<OutboxInterceptor>();


        // ==========================================
        // ۱. پیکربندی پایگاه داده (SQL Server)
        // ==========================================
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<OutboxInterceptor>();
            // کانکشن استرینگ خودتان را اینجا قرار دهید
            options.UseSqlServer("Your_Connection_String")
                   .AddInterceptors(interceptor);
        });
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(OutboxMessageProcessorJob));

            configure
                .AddJob<OutboxMessageProcessorJob>(jobKey)
                .AddTrigger(trigger =>
                    trigger.ForJob(jobKey)
                           .WithSimpleSchedule(schedule =>
                               schedule.WithIntervalInSeconds(10) // هر ۱۰ ثانیه جارو می‌کند
                                       .RepeatForever()));
        });

        services.AddQuartzHostedService(options =>
        {
            // این خط تضمین می‌کند که اگر سرور در حال خاموش شدن بود، ابتدا کار Job فعلی تمام شود
            options.WaitForJobsToComplete = true;
        });

        // ==========================================
        // ۲. ثبت ریپازیتوری‌ها (Data Access)
        // ==========================================
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<ITrainerRepository, TrainerRepository>();
        services.AddScoped<IWorkoutProgramRepository, WorkoutProgramRepository>();

        // ==========================================
        // ۳. سرویس‌های امنیتی و احراز هویت
        // ==========================================
        // چون سیستم هش هیچ دیتایی را در خود نگه نمی‌دارد، بهترین حالت برای آن Singleton است
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        // ==========================================
        // ۴. پیکربندی کش توزیع‌شده (Redis)
        // ==========================================
        services.AddStackExchangeRedisCache(options =>
        {
            // آدرس سرور ردیس (در پروژه‌های واقعی از appsettings.json خوانده می‌شود)
            options.Configuration = "localhost:6379";
            // یک پیشوند برای کلیدها تا با دیتای پروژه‌های دیگر تداخل نداشته باشد
            options.InstanceName = "FitnessPlatform_";
        });
 

        // خواندن تنظیمات JWT از appsettings.json و بایند کردن آن به کلاس JwtOptions
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        // ثبت JwtProvider برای تولید توکن
        services.AddScoped<IJwtProvider, JwtProvider>();

        // ==========================================
        // ۵. سیستم پیام‌رسان و معماری رویدادمحور (RabbitMQ)
        // ==========================================
        services.AddMassTransit(x =>
        {
            // معرفی Consumer ها (شنوندگان پس‌زمینه) به سیستم
            x.AddConsumer<SendCongratulationEmailConsumer>();
            x.AddConsumer<UpdateLeaderboardConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                // پیکربندی اتصال به سرور محلی RabbitMQ
                cfg.Host("localhost", "/", h => {
                    h.Username("guest");
                    h.Password("guest");
                });

                // این متد به طور خودکار صف‌ها را در RabbitMQ می‌سازد 
                // و Consumer های بالا را به آن‌ها متصل می‌کند
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
} 