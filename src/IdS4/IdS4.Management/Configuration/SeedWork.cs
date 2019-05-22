using IdS4.DbContexts;
using IdS4.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IdS4.Management.Configuration
{
    public static class SeedWork
    {
        public static async Task AddIdS4Adminstrator(this IServiceProvider serviceProvider)
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
    }
}
