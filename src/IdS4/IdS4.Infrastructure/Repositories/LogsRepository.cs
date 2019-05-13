using IdS4.Infrastructure.DbContexts;
using IdS4.Logs;
using IdS4.Repositories;
using RajsLibs.EfCore.Uow;
using RajsLibs.Repository.EfCore;

namespace IdS4.Infrastructure.Repositories
{
    public class LogsRepository : RepositoryBase<IdS4LogDbContext, Log, int>, ILogRepository
    {
        public LogsRepository(IEfUnitOfWork<IdS4LogDbContext> unitOfWork) 
            : base(unitOfWork)
        {

        }
    }
}
