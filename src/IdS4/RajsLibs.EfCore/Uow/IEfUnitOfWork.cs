using Microsoft.EntityFrameworkCore;
using RajsLibs.Uow;

namespace RajsLibs.EfCore.Uow
{
    public interface IEfUnitOfWork<TDbContext> : IUnitOfWork
        where TDbContext : DbContext
    {
        TDbContext DbContext { get; }
    }
}
