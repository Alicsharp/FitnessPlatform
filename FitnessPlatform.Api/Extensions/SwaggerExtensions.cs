using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessPlatform.Api.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new OpenApiInfo
                {
                    Title = "FitnessPlatform API",
                    Version = "v1",
                    Description = "مستندات ای‌پی‌آی با رابط کاربری Scalar"
                };

                var securityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "یسبشسیشسیشسیشسی!"
                };

                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes["Bearer"] = securityScheme;

                document.Security ??= new List<OpenApiSecurityRequirement>();

                // استفاده از List<string> به جای آرایه
                document.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer")] = new List<string>()
                });

                return Task.CompletedTask;
            });
        });

        return services;
    }
}