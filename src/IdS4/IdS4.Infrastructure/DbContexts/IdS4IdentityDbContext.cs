using IdS4.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RajsLibs.EfCore.Uow;
using RajsLibs.Uow;

namespace IdS4.Infrastructure.DbContexts
{
    public class IdS4IdentityDbContext: UnitOfWorkBase<IdentityDbContext<IdS4User, IdS4Role, string, IdS4UserClaim, IdS4UserRole,  IdS4UserLogin, IdS4RoleClaim, IdS4UserToken>>, IUnitOfWork
    {
        public IdS4IdentityDbContext(DbContextOptions<IdentityDbContext<IdS4User, IdS4Role, string, IdS4UserClaim, IdS4UserRole, IdS4UserLogin, IdS4RoleClaim, IdS4UserToken>> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
