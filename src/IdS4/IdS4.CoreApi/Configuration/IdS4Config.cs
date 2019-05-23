using IdS4.DbContexts;
using IdS4.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IdS4.CoreApi.Configuration
{
    public static class IdS4Config
    {
        public static void AddIdS4DbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<IdS4IdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityDb"));
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
        }

        public static void AddIdS4Authentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = configuration["Authority"];
                    options.RequireHttpsMetadata = false;

                    options.Audience = "coreApi";
                });
        }

        public static void UseIdS4Authentication(this IApplicationBuilder app)
        {
            app.UseAuthentication();
        }

        public static void AddIdS4Cors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins(configuration["CorsOrigin"])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        public static void UseIdS4Cors(this IApplicationBuilder app)
        {
            app.UseCors("default");
        }
    }
}
