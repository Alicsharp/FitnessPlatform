using FitnessPlatform.Domain.Repositories;
using FitnessPlatform.Infrastructure.Messaging.Consumers;
using FitnessPlatform.Infrastructure.Persistence;
using FitnessPlatform.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
// در صورت نیاز فضای نام دیتابیس و ریپازیتوری‌ها را هم اضافه کن

namespace FitnessPlatform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // ۱. ثبت دیتابیس (SQL Server)
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // ۲. ثبت Repository ها
        services.AddScoped<IWorkoutProgramRepository, WorkoutProgramRepository>();

        // ۳. ثبت MassTransit و اتصال به RabbitMQ
        services.AddMassTransit(x =>
        {
            // کلاس شنونده‌ای که ساختی را اینجا به سیستم معرفی می‌کنی
            x.AddConsumer<SendCongratulationEmailConsumer>();  
            x.AddConsumer<UpdateLeaderboardConsumer>();
            // اگر کلاس UpdateLeaderboardConsumer را هم ساختی، خط پایین را اضافه کن
            // x.AddConsumer<UpdateLeaderboardConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h => {
                    h.Username("guest");
                    h.Password("guest");
                });

                // این خط جادویی، به طور خودکار صف‌ها را در RabbitMQ می‌سازد 
                // و Consumer های بالا را به آن‌ها متصل می‌کند
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}