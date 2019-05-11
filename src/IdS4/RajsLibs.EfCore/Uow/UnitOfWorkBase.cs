using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RajsLibs.Uow;

namespace RajsLibs.EfCore.Uow
{
    public abstract class UnitOfWorkBase : DbContext, IUnitOfWork
    {
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
