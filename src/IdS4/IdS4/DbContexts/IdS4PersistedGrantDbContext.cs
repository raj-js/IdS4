using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace IdS4.DbContexts
{
    public class IdS4PersistedGrantDbContext : PersistedGrantDbContext<IdS4PersistedGrantDbContext>
    {
        public IdS4PersistedGrantDbContext(DbContextOptions<IdS4PersistedGrantDbContext> options, OperationalStoreOptions storeOptions) 
            : base(options, storeOptions)
        {

        }
    }
}
