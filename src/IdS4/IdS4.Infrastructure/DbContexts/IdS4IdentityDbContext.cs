using IdS4.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdS4.Infrastructure.DbContexts
{
    public class IdS4IdentityDbContext: IdentityDbContext<IdS4User, IdS4Role, string, IdS4UserClaim, IdS4UserRole,  IdS4UserLogin, IdS4RoleClaim, IdS4UserToken>
    {
        public IdS4IdentityDbContext(DbContextOptions<IdS4IdentityDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
