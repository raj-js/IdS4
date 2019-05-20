using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using IdS4.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace IdS4.Infrastructure.DbContexts
{
    public class IdS4PersistedGrantDbContext : PersistedGrantDbContext<IdS4PersistedGrantDbContext>, IIdS4PersistedGrantDbContext
    {
        public IdS4PersistedGrantDbContext(DbContextOptions options, OperationalStoreOptions storeOptions) 
            : base(options, storeOptions)
        {

        }
    }
}
