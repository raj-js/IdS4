﻿using IdS4.DbContexts;
using IdS4.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IdS4.Management.Configuration
{
    public static class IdS4Config
    {
        public static void AddIdS4DbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<IdS4IdentityDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("IdentityDb"),
                    sql => sql.MigrationsAssembly(assembly));
            });

            services
                .AddIdentity<IdS4User, IdS4Role>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<IdS4IdentityDbContext>()
                .AddDefaultTokenProviders();

            // 只是将相关服务注入到DI
            var idsBuilder = services
                .AddIdentityServer()
                .AddAspNetIdentity<IdS4User>();

            idsBuilder.AddConfigurationStore<IdS4ConfigurationDbContext>(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseSqlServer(configuration.GetConnectionString("ConfigurationDb"));
            });
        }
    }
}
