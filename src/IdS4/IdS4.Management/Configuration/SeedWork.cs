using IdentityServer4.EntityFramework.Entities;
using IdS4.DbContexts;
using IdS4.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;
using Constants = IdentityServer4.IdentityServerConstants;

namespace IdS4.Management.Configuration
{
    public static class SeedWork
    {
        public static async Task AddIdS4Administrator(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var ids4IdentityDb = provider.GetRequiredService<IdS4IdentityDbContext>();
                var configuration = provider.GetRequiredService<IConfiguration>();
                var logger = provider.GetRequiredService<ILogger<IdS4ConfigurationDbContext>>();
                var userManager = provider.GetRequiredService<UserManager<IdS4User>>();

                logger.LogInformation($"初始化管理员账号...");
                var adminSettings = configuration.GetSection("AdminSettings");
                var adminName = adminSettings["UserName"];
                if ((await userManager.FindByNameAsync(adminName)) != null)
                {
                    logger.LogInformation($"管理员账号已存在.");
                    return;
                }

                var admin = new IdS4User
                {
                    UserName = adminName,
                    Email = adminSettings["Email"],
                    EmailConfirmed = true
                };
                var identityResult = await userManager.CreateAsync(admin, adminSettings["Password"]);

                if (!identityResult.Succeeded)
                {
                    logger.LogError($"初始化管理员账号失败：");
                    foreach (var error in identityResult.Errors)
                        logger.LogError($"{error.Code} --> {error.Description}");
                }

                logger.LogInformation($"管理员账号初始化完成.");
            }
        }

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

                try
                {
                    logger.LogInformation($"客户端 IdS4_Management_Spa 初始化中...");
                    var ids4ManagementSpaId = "IdS4.Management.Spa";
                    if (ids4ConfigurationDb.Clients
                        .AsNoTracking()
                        .Any(c => c.ClientId.Equals(ids4ManagementSpaId)))
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
                        AllowAccessTokensViaBrowser = true,
                        RequirePkce = true,
                        RequireClientSecret = false
                    };

                    await ids4ConfigurationDb.Clients.AddAsync(client);
                    await ids4ConfigurationDb.SaveChangesAsync();

                    var grantType = new ClientGrantType
                    {
                        ClientId = client.Id,
                        GrantType = GrantTypes.AuthorizationCode
                    };
                    await ids4ConfigurationDb.ClientGrantTypes.AddAsync(grantType);

                    var redirectUri = new ClientRedirectUri
                    {
                        ClientId = client.Id,
                        RedirectUri = spaSettings["RedirectUri"]
                    };
                    await ids4ConfigurationDb.ClientRedirectUris.AddAsync(redirectUri);

                    var postLogoutRedirectUri = new ClientPostLogoutRedirectUri
                    {
                        ClientId = client.Id,
                        PostLogoutRedirectUri = spaSettings["PostLogoutRedirectUri"]
                    };
                    await ids4ConfigurationDb.ClientPostLogoutRedirectUris.AddAsync(postLogoutRedirectUri);

                    var allowedCorsOrigin = new ClientCorsOrigin
                    {
                        ClientId = client.Id,
                        Origin = spaSettings["Origin"]
                    };
                    await ids4ConfigurationDb.ClientCorsOrigins.AddAsync(allowedCorsOrigin);

                    var allowedScopes = new List<ClientScope>
                    {
                        new ClientScope
                        {
                            ClientId = client.Id,
                            Scope = StandardScopes.OpenId
                        },
                        new ClientScope
                        {
                            ClientId = client.Id,
                            Scope = StandardScopes.Profile
                        },
                        new ClientScope
                        {
                            ClientId = client.Id,
                            Scope = StandardScopes.Email
                        },
                        new ClientScope
                        {
                            ClientId = client.Id,
                            Scope = "coreApi.full_access"
                        }
                    };
                    await ids4ConfigurationDb.ClientScopes.AddRangeAsync(allowedScopes);

                    await ids4ConfigurationDb.SaveChangesAsync();

                    logger.LogInformation($"客户端 IdS4_Management_Spa 初始化完成.");
                }
                catch (Exception e)
                {
                    logger.LogError(e.ToString());
                }
            }
        }
    }
}
