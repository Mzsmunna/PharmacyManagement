using Application.Abstractions;
using Application.Exceptions;
using Infrastructure.Auth.Configs;
using Infrastructure.Auth.Helpers;
using Infrastructure.Auth.Managers;
using Infrastructure.Auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence.DB;
using System.ClientModel.Primitives;

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
                services.AddAppAuth(config);
            }   
            return services;
        }

        public static IServiceCollection AddAppAuth(this IServiceCollection services, IConfiguration config)
        {
            var jwtAuth = config.GetSection(nameof(JWTAuth));
            var jwtAuthConfig = jwtAuth.Get<JWTAuth>();
            services.Configure<JWTAuth>(jwtAuth);
            var signingKey = JwtHelper.GetSymmetricSecurityKey(jwtAuthConfig?.SecretKey ?? "");
            var tokenParameters = JwtHelper.GetTokenValidationParameters(signingKey, jwtAuthConfig);

            services
            //.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = tokenParameters;
            });
            
            services.AddScoped<IJwtTokenManager, JwtTokenManager>();
            services.AddScoped<IAuthService, AuthService>();   
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
