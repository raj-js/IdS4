using IdentityServer4.EntityFramework.Entities;
using IdS4.Infrastructure.DbContexts;
using IdS4.Wrappers;
using RajsLibs.EfCore.Uow;
using RajsLibs.Repository.EfCore;

namespace IdS4.Repositories
{
    public class IdentityResourceRepository : RepositoryBase<IdS4ConfigurationDbContext, IdS4IdentityResource, int>, IIdentityResourceRepository
    {
        public IdentityResourceRepository(IEfUnitOfWork<IdS4ConfigurationDbContext> unitOfWork) : base(unitOfWork)
        {

        }
    }
}