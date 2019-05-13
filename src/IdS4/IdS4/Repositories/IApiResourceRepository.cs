using IdentityServer4.EntityFramework.Entities;
using RajsLibs.Repositories;

namespace IdS4.Repositories
{
    public interface IApiResourceRepository : IRepository<ApiResource, int>
    {

    }
}