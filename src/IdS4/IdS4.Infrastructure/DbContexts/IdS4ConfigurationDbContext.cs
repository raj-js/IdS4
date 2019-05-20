using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Options;
using IdS4.Abstraction.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace IdS4.Infrastructure.DbContexts
{
    public class IdS4ConfigurationDbContext : ConfigurationDbContext<IdS4ConfigurationDbContext>, IIdS4ConfigurationDbContext
    {
        public IdS4ConfigurationDbContext(DbContextOptions<IdS4ConfigurationDbContext> options, ConfigurationStoreOptions storeOptions)
            : base(options, storeOptions)
        {

        }

        public DbSet<ApiResourceProperty> ApiResourceProperties { get; private set; }

        public DbSet<IdentityResourceProperty> IdentityResourceProperties { get; private set; }

        public DbSet<ApiSecret> ApiSecrets { get; private set; }

        public DbSet<ApiScope> ApiScopes { get; private set; }

        public DbSet<ApiScopeClaim> ApiScopeClaims { get; private set; }

        public DbSet<IdentityClaim> IdentityClaims { get; private set; }

        public DbSet<ApiResourceClaim> ApiResourceClaims { get; private set; }

        public DbSet<ClientGrantType> ClientGrantTypes { get; private set; }

        public DbSet<ClientScope> ClientScopes { get; private set; }

        public DbSet<ClientSecret> ClientSecrets { get; private set; }

        public DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; private set; }

        public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; private set; }

        public DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; private set; }

        public DbSet<ClientRedirectUri> ClientRedirectUris { get; private set; }

        public DbSet<ClientClaim> ClientClaims { get; private set; }

        public DbSet<ClientProperty> ClientProperties { get; private set; }
    }
}
