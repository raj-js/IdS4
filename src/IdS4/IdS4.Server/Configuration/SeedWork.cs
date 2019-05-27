using IdentityServer4.EntityFramework.Entities;
using IdS4.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using static IdentityModel.OidcConstants;
using Constants = IdentityServer4.IdentityServerConstants;

namespace IdS4.Server.Configuration
{
    public static class SeedWork
    {
        public static async Task AddIdentityResources(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var ids4ConfigurationDb = provider.GetRequiredService<IdS4ConfigurationDbContext>();
                var configuration = provider.GetRequiredService<IConfiguration>();
                var logger = provider.GetRequiredService<ILogger<IdS4ConfigurationDbContext>>();

                try
                {
                    if (!await ids4ConfigurationDb.IdentityResources.AnyAsync(s => StandardScopes.OpenId.Equals(s.Name)))
                    {
                        await ids4ConfigurationDb.IdentityResources.AddAsync(new IdentityResource
                        {
                            Name = StandardScopes.OpenId,
                            DisplayName = "Open Id",
                        });
                    }

                    if (!await ids4ConfigurationDb.IdentityResources.AnyAsync(s => StandardScopes.Email.Equals(s.Name)))
                    {
                        await ids4ConfigurationDb.IdentityResources.AddAsync(new IdentityResource
                        {
                            Name = StandardScopes.Email,
                            DisplayName = "Email",
                        });
                    }

                    if (!await ids4ConfigurationDb.IdentityResources.AnyAsync(s => StandardScopes.Profile.Equals(s.Name)))
                    {
                        await ids4ConfigurationDb.IdentityResources.AddAsync(new IdentityResource
                        {
                            Name = StandardScopes.Profile,
                            DisplayName = "Profile",
                        });
                    }

                    await ids4ConfigurationDb.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    logger.LogError(e.ToString());
                }
            }
        }
    }
}
