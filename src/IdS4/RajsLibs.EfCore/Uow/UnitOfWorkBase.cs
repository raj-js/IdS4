using Microsoft.EntityFrameworkCore;
using RajsLibs.Uow;
using System.Threading;
using System.Threading.Tasks;

namespace RajsLibs.EfCore.Uow
{
    public abstract class UnitOfWorkBase<TDbContext> : DbContext, IUnitOfWork
        where TDbContext : DbContext
    {
        public UnitOfWorkBase(DbContextOptions<TDbContext> options)
            : base(options)
        {

        }

        public int Commit()
        {
            return SaveChanges();
        }

        public async Task<int> CommitAsync(CancellationToken cancallationToken = default(CancellationToken))
        {
            return await SaveChangesAsync(cancallationToken);
        }
    }
}
