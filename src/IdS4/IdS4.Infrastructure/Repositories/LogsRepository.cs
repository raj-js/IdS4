using IdS4.Infrastructure.DbContexts;
using IdS4.Logs;
using RajsLibs.Repositories;
using RajsLibs.Repository.EfCore;
using RajsLibs.Uow;

namespace IdS4.Infrastructure.Repositories
{
    public class LogsRepository : RepositoryBase<IdS4LogDbContext, Log, long>, IRepository<Log, long>
    {
        public LogsRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
    }
}
