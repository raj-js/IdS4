using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdS4.Abstraction.DbContexts
{
    public interface IIdS4ConfigurationDbContext : IConfigurationDbContext
    {
        DbSet<ApiSecret> ApiSecrets { get; } 

        DbSet<ApiScope> ApiScopes { get; }

        DbSet<ApiScopeClaim> ApiScopeClaims { get; }

        DbSet<IdentityClaim> IdentityClaims { get; }

        DbSet<ApiResourceClaim> ApiResourceClaims { get; }

        DbSet<ClientGrantType> ClientGrantTypes { get; }

        DbSet<ClientScope> ClientScopes { get; }

        DbSet<ClientSecret> ClientSecrets { get; }

        DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; }

        DbSet<ClientCorsOrigin> ClientCorsOrigins { get; }

        DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; }

        DbSet<ClientRedirectUri> ClientRedirectUris { get; }

        DbSet<ClientClaim> ClientClaims { get; }

        DbSet<ClientProperty> ClientProperties { get; }

        DbSet<IdentityResourceProperty> IdentityResourceProperties { get; }

        DbSet<ApiResourceProperty> ApiResourceProperties { get; }
    }
}
