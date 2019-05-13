using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace RajsLibs.EfCore.Uow
{
    public class EfUnitOfWork<TDbContext> : IEfUnitOfWork<TDbContext>
        where TDbContext : DbContext
    {
        public EfUnitOfWork(TDbContext context)
        {
            DbContext = context;
        }

        public TDbContext DbContext { get; private set; }

        public virtual int Commit()
        {
            return DbContext.SaveChanges();
        }

        public virtual async Task<int> CommitAsync(CancellationToken cancallationToken = default(CancellationToken))
        {
            return await DbContext.SaveChangesAsync(cancallationToken);
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}
