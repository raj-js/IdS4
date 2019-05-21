using IdS4.DbContexts;
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
        public static void AddIdS4(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // 为 Asp.Net Core Identity 添加 DbContext
            services.AddDbContext<IdS4IdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityDb"),
                    sql => sql.MigrationsAssembly(assembly));
            });

            // 配置 Asp.Net Core Identity
            services
                .AddIdentity<IdS4User, IdS4Role>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<IdS4IdentityDbContext>()
                .AddDefaultTokenProviders();

            var idsBuilder = services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddAspNetIdentity<IdS4User>();

            idsBuilder.AddConfigurationStore<IdS4ConfigurationDbContext>(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseSqlServer(configuration.GetConnectionString("ConfigurationDb"),
                        sql => sql.MigrationsAssembly(assembly));
            });

            idsBuilder.AddOperationalStore<IdS4PersistedGrantDbContext>(options =>
                {
                    options.EnableTokenCleanup = true;
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(configuration.GetConnectionString("PersistedGrantDb"),
                            sql => sql.MigrationsAssembly(assembly));
                });

            // 添加临时证书
            idsBuilder.AddDeveloperSigningCredential();
        }

        public static void AddIdS4Log(this IServiceCollection service, IConfiguration configuration)
        {
            var assembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            service.AddDbContext<IdS4LogDbContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("LogDb"),
                        sql => sql.MigrationsAssembly(assembly));
                });
        }
    }
}
