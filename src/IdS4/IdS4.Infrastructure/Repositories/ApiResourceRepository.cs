using IdentityServer4.EntityFramework.Entities;
using IdS4.Infrastructure.DbContexts;
using IdS4.Repositories;
using RajsLibs.EfCore.Uow;
using RajsLibs.Repository.EfCore;

namespace IdS4.Infrastructure.Repositories
{
    public class ApiResourceRepository : RepositoryBase<IdS4ConfigurationDbContext, ApiResource, int>, IApiResourceRepository
    {
        public ApiResourceRepository(IEfUnitOfWork<IdS4ConfigurationDbContext> unitOfWork) : base(unitOfWork)
        {

        }
    }
}
