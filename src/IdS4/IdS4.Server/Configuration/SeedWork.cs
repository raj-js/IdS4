using IdentityServer4.EntityFramework.Entities;
using IdS4.DbContexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Constants = IdentityServer4.IdentityServerConstants;

namespace IdS4.Server.Configuration
{
    public static class SeedWork
    {
        /// <summary>
        /// 添加 IdS4.Management 客户端
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static async Task AddIdS4ManagementClient(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var ids4ConfigurationDb = provider.GetRequiredService<IdS4ConfigurationDbContext>();
                var configuration = provider.GetRequiredService<IConfiguration>();
                var logger = provider.GetRequiredService<ILogger<IdS4ConfigurationDbContext>>();

                logger.LogInformation($"客户端 IdS4_Management_Spa 初始化中...");
                var ids4ManagementSpaId = "IdS4.Management.Spa";
                if (ids4ConfigurationDb.Clients.Any(c => c.ClientId.Equals(ids4ManagementSpaId)))
                {
                    logger.LogInformation($"客户端 IdS4_Management_Spa 已存在.");
                    return;
                }

                var spaSettings = configuration.GetSection("IdS4_Management_Spa");
                var client = new Client
                {
                    ClientId = ids4ManagementSpaId,
                    ClientName = "IdS4管理系统",
                    ClientUri = spaSettings["ClientUri"],

                    AllowedGrantTypes = { new ClientGrantType { GrantType = "implicit" } },
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { new ClientRedirectUri { RedirectUri = spaSettings["RedirectUri"] } },
                    PostLogoutRedirectUris = { new ClientPostLogoutRedirectUri { PostLogoutRedirectUri = spaSettings["PostLogoutRedirectUri"] } },
                    AllowedCorsOrigins = { new ClientCorsOrigin { Origin = spaSettings["Origin"] } },

                    AllowedScopes =
                    {
                        new ClientScope { Scope = Constants.StandardScopes.OpenId },
                        new ClientScope { Scope = Constants.StandardScopes.Email },
                        new ClientScope { Scope = Constants.StandardScopes.Profile },
                        new ClientScope { Scope = "roles" },

                        new ClientScope { Scope = "coreApi" }
                    }
                };

                await ids4ConfigurationDb.Clients.AddAsync(client);
                await ids4ConfigurationDb.SaveChangesAsync();

                logger.LogInformation($"客户端 IdS4_Management_Spa 初始化完成.");
            }
        }
    }
}
