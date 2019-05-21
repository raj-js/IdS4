using System.Reflection;
using IdS4.DbContexts;
using IdS4.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdS4.Server.Configuration
{
    public static class IdS4Config
    {
        public static void AddIdS4(this IServiceCollection services, IConfiguration configuration)
        {
            // 为 Asp.Net Core Identity 添加 DbContext
            services.AddDbContext<IdS4IdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityDb"));
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
                    b.UseSqlServer(configuration.GetConnectionString("ConfigurationDb"));
            });

            idsBuilder.AddOperationalStore<IdS4PersistedGrantDbContext>(options =>
                {
                    options.EnableTokenCleanup = true;
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(configuration.GetConnectionString("PersistedGrantDb"));
                });

            // 添加临时证书
            idsBuilder.AddDeveloperSigningCredential();
        }
    }
}
