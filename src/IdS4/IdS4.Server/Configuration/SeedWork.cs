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
using static IdentityModel.OidcConstants;

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
                            Required = true,
                            Description = "Informs the Authorization Server that the Client is making an OpenID Connect request. If the <c>openid</c> scope value is not present, the behavior is entirely unspecified.",
                            UserClaims = new List<IdentityClaim>
                            {
                                 new IdentityClaim { Type = JwtClaimTypes.Subject }
                            }
                        });
                    }

                    if (!await ids4ConfigurationDb.IdentityResources.AnyAsync(s => StandardScopes.Email.Equals(s.Name)))
                    {
                        await ids4ConfigurationDb.IdentityResources.AddAsync(new IdentityResource
                        {
                            Name = StandardScopes.Email,
                            DisplayName = "Email",
                            Required = false,
                            Description = "This scope value requests access to the <c>email</c> and <c>email_verified</c> Claims.",
                            UserClaims = new List<IdentityClaim>
                            {
                                new IdentityClaim { Type = JwtClaimTypes.Email },
                                new IdentityClaim { Type = JwtClaimTypes.EmailVerified }
                            }
                        });
                    }

                    if (!await ids4ConfigurationDb.IdentityResources.AnyAsync(s => StandardScopes.Profile.Equals(s.Name)))
                    {
                        await ids4ConfigurationDb.IdentityResources.AddAsync(new IdentityResource
                        {
                            Name = StandardScopes.Profile,
                            DisplayName = "Profile",
                            Required = false,
                            Description = "This scope value requests access to the End-User's default profile Claims, which are: <c>name</c>, <c>family_name</c>, <c>given_name</c>, <c>middle_name</c>, <c>nickname</c>, <c>preferred_username</c>, <c>profile</c>, <c>picture</c>, <c>website</c>, <c>gender</c>, <c>birthdate</c>, <c>zoneinfo</c>, <c>locale</c>, and <c>updated_at</c>.",
                            UserClaims = new List<IdentityClaim>
                            {
                                new IdentityClaim { Type = JwtClaimTypes.Name },
                                new IdentityClaim { Type = JwtClaimTypes.FamilyName },
                                new IdentityClaim { Type = JwtClaimTypes.GivenName },
                                new IdentityClaim { Type = JwtClaimTypes.MiddleName },
                                new IdentityClaim { Type = JwtClaimTypes.NickName },
                                new IdentityClaim { Type = JwtClaimTypes.PreferredUserName },
                                new IdentityClaim { Type = JwtClaimTypes.Profile },
                                new IdentityClaim { Type = JwtClaimTypes.Picture },
                                new IdentityClaim { Type = JwtClaimTypes.WebSite },
                                new IdentityClaim { Type = JwtClaimTypes.Gender },
                                new IdentityClaim { Type = JwtClaimTypes.BirthDate },
                                new IdentityClaim { Type = JwtClaimTypes.ZoneInfo },
                                new IdentityClaim { Type = JwtClaimTypes.Locale },
                                new IdentityClaim { Type = JwtClaimTypes.UpdatedAt },
                            }
                        });
                    }

                    if (!await ids4ConfigurationDb.IdentityResources.AnyAsync(s => StandardScopes.Address.Equals(s.Name)))
                    {
                        await ids4ConfigurationDb.IdentityResources.AddAsync(new IdentityResource
                        {
                            Name = StandardScopes.Address,
                            DisplayName = "Address",
                            Required = false,
                            Description = "This scope value requests access to the <c>address</c> Claim.",
                            UserClaims = new List<IdentityClaim>
                            {
                                new IdentityClaim { Type = JwtClaimTypes.Address }
                            }
                        });
                    }

                    if (!await ids4ConfigurationDb.IdentityResources.AnyAsync(s => StandardScopes.Phone.Equals(s.Name)))
                    {
                        await ids4ConfigurationDb.IdentityResources.AddAsync(new IdentityResource
                        {
                            Name = StandardScopes.Phone,
                            DisplayName = "Phone",
                            Required = false,
                            Description = "This scope value requests access to the <c>phone_number</c> and <c>phone_number_verified</c> Claims.",
                            UserClaims = new List<IdentityClaim>
                            {
                                new IdentityClaim { Type = JwtClaimTypes.PhoneNumber },
                                new IdentityClaim { Type = JwtClaimTypes.PhoneNumberVerified },
                            }
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
