using System.Threading.Tasks;
using IdS4.Wrappers;
using RajsLibs.Repositories.Paging;

namespace IdS4.Services
{
    public interface IIdentityResourceService
    {
        void Add(IdS4IdentityResource resource);

        void Remove(IdS4IdentityResource resource);

        void Modify(IdS4IdentityResource resource);

        Task<IdS4IdentityResource> FindAsync(int id);

        Task<IPageResult<IdS4IdentityResource>> SearchAsync(IPageQuery<IdS4IdentityResource> query);
    }
}
