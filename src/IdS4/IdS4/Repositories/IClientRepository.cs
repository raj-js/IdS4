using IdentityServer4.EntityFramework.Entities;
using IdS4.Wrappers;
using RajsLibs.Repositories;

namespace IdS4.Repositories
{
    public interface IClientRepository : IRepository<IdS4Client, int>
    {

	}
}