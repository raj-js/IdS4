using IdentityModel;
using IdentityServer4.EntityFramework.Entities;
using IdS4.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdS4.CoreApi.Configuration
{
    public static class SeedWork
    {
        /// <summary>
        /// 添加 IdS4.Management 客户端
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static async Task AddIdS4CoreApi(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var ids4ConfigurationDb = provider.GetRequiredService<IdS4ConfigurationDbContext>();
                var configuration = provider.GetRequiredService<IConfiguration>();
                var logger = provider.GetRequiredService<ILogger<IdS4ConfigurationDbContext>>();

                try
                {
                    var coreApiName = "coreApi";
                    logger.LogInformation($"Api资源 {coreApiName} 初始化中...");

                    var exists = await ids4ConfigurationDb.ApiResources
                        .AsNoTracking()
                        .AnyAsync(api => coreApiName.Equals(api.Name));

                    if (exists)
                    {
                        logger.LogInformation($"Api资源 {coreApiName} 已存在.");
                        return;
                    }

                    var coreApi = new ApiResource
                    {
                        Name = coreApiName,
                        DisplayName = $"IdS4.{coreApiName}",
                        Description = "针对 IdS4.Management 的核心 Api",
                        UserClaims = new List<ApiResourceClaim>
                        {
                            new ApiResourceClaim { Type = JwtClaimTypes.Name },
                            new ApiResourceClaim { Type = JwtClaimTypes.Email }
                        },
                        Scopes = new List<ApiScope>
                        {
                            new ApiScope
                            {
                                Name = "coreApi.full_access",
                                DisplayName = "Full access to coreApi",
                            }
                        },
                        Secrets = new List<ApiSecret>
                        {
                            new ApiSecret{ Value = "secret".ToSha256() }
                        }
                    };
                    await ids4ConfigurationDb.ApiResources.AddAsync(coreApi);
                    await ids4ConfigurationDb.SaveChangesAsync();


                    await ids4ConfigurationDb.SaveChangesAsync();
                    logger.LogInformation($"Api资源 {coreApiName} 初始化完成.");
                }
                catch (Exception e)
                {
                    logger.LogError(e.ToString());
                }
            }
        }
    }
}
