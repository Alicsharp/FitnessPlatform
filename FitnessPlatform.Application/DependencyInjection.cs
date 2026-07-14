using FitnessPlatform.Domain.Services.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace FitnessPlatform.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // ثبت MediatR برای تمام Command ها و Query های این لایه
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddScoped<IWorkoutGenerationStrategy, HomeHiitDumbbellStrategy>();
        services.AddScoped<WorkoutGeneratorDomainService>();
        // ⚡️ این خط را از حالت کامنت خارج کردیم!
        services.AddScoped<IWorkoutGenerationStrategy, BodybuilderGymStrategy>();
        return services;
    }
}