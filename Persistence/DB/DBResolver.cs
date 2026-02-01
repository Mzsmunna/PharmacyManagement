using Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.DB.Context;
using Persistence.DB.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Persistence.DB
{
    public static class DBResolver
    {
        public static IServiceCollection AddAppDBContext(
            this IServiceCollection services,
            IConfiguration config,
            ServiceLifetime lifeTime = ServiceLifetime.Scoped)
        {
            var conn = config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(conn)) 
                throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
            //services.AddDbContext<EFContext>(lifeTime);
            services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(conn)
                //), sqlOption =>
                //{
                //    sqlOption.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                //})
                ,lifeTime
            );
            services.AddScoped<IAppDBContext, AppDBContext>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            return services;
        }
    }
}
