using IdS4.Repositories;
using IdS4.Wrappers;
using RajsLibs.Repositories.Paging;
using System.Threading.Tasks;

namespace IdS4.Services.Impl
{
    public class IdentityResourceService: IIdentityResourceService
    {
        private readonly IIdentityResourceRepository _identityResourceRepository = null;

        public IdentityResourceService(IIdentityResourceRepository identityResourceRepository)
        {
            _identityResourceRepository = identityResourceRepository;
        }

        public void Add(IdS4IdentityResource resource)
        {
            _identityResourceRepository.Add(resource);
        }

        public void Remove(IdS4IdentityResource resource)
        {
            _identityResourceRepository.Delete(resource);
        }

        public void Modify(IdS4IdentityResource resource)
        {
            _identityResourceRepository.Update(resource);
        }

        public async Task<IdS4IdentityResource> FindAsync(int id)
        {
            return await _identityResourceRepository.FindAsync(id);
        }

        public async Task<IPageResult<IdS4IdentityResource>> SearchAsync(IPageQuery<IdS4IdentityResource> query)
        {
            return await _identityResourceRepository.PagingAsync(query);
        }
    }
}
