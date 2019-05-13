using IdentityServer4.EntityFramework.Entities;
using RajsLibs.Repositories;

namespace IdS4.Repositories
{
    public interface IPersistedGrantRepository : IRepository<PersistedGrant, string>
    {

    }
}