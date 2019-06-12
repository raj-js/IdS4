using IdS4.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdS4.DbContexts
{
    public class IdS4IdentityDbContext: IdentityDbContext<IdS4User, IdS4Role, string, IdS4UserClaim, IdS4UserRole, IdS4UserLogin, IdS4RoleClaim, IdS4UserToken>
    {
        public IdS4IdentityDbContext(DbContextOptions<IdS4IdentityDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdS4UserClaim>()
                .HasOne(s => s.User)
                .WithMany(s => s.UserClaims)
                .HasForeignKey(s => s.UserId);

            builder.Entity<IdS4UserRole>()
                .HasOne(s => s.User)
                .WithMany(s => s.UserRoles)
                .HasForeignKey(s => s.UserId);

            builder.Entity<IdS4UserRole>()
                .HasOne(s => s.Role)
                .WithMany(s => s.UserRoles)
                .HasForeignKey(s => s.RoleId);

            builder.Entity<IdS4RoleClaim>()
                .HasOne(s => s.Role)
                .WithMany(s => s.RoleClaims)
                .HasForeignKey(s => s.RoleId);
        }
    }
}
