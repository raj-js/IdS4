using IdentityServer4.EntityFramework.Entities;
using IdS4.Infrastructure.DbContexts;
using IdS4.Repositories;
using RajsLibs.EfCore.Uow;
using RajsLibs.Repository.EfCore;

namespace IdS4.Infrastructure.Repositories
{
    public class ClientRepository : RepositoryBase<IdS4ConfigurationDbContext, Client, int>, IClientRepository
    {
        public ClientRepository(IEfUnitOfWork<IdS4ConfigurationDbContext> unitOfWork) : base(unitOfWork)
        {

        }
    }
}