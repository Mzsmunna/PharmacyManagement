using Application.Abstractions;
using Infrastructure.Auth.Managers;
using Infrastructure.Auth.Services;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.DB;

namespace Infrastructure
{
    public static class DependencyResolver
    {
        public static IServiceCollection AddPharmacyApp(this IServiceCollection services, IConfiguration config, bool isWebApp = true)
        {
            services.AddGlobalExceptionHandler();
            if (isWebApp is false)
            {
                services.AddAppDBContext(config);
                services.AddScoped<IJwtTokenManager, JwtTokenManager>();
                services.AddScoped<IAuthService, AuthService>();
            }
                
            return services;
        }

        public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
        {
            services.AddProblemDetails(config =>
            {
                config.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path.Value}";
                    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
                    var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                    context.ProblemDetails.Extensions.TryAdd("traceId", activity?.TraceId.ToString() ?? string.Empty);
                };
            });
            services.AddExceptionHandler<GlobalExceptionHandler>();
            return services;
        }
    }
}
