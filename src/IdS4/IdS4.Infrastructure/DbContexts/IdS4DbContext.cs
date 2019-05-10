using IdS4.Abstraction.DbContexts;
using IdS4.Logs;
using Microsoft.EntityFrameworkCore;

namespace IdS4.Infrastructure.DbContexts
{
    public class IdS4DbContext : DbContext, IIdS4DbContext
    {
        public IdS4DbContext(DbContextOptions<IdS4DbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Log>(b =>
            {
                b.HasKey(s => s.Id);
                b.Property(s => s.Msg).HasMaxLength(1024);
            });
        }

        public DbSet<Log> Logs { get; private set; }
    }
}
