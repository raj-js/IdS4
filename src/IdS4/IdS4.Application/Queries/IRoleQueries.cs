using IdS4.Application.Models.Paging;
using IdS4.CoreApi.Models.Role;
using System.Threading.Tasks;

namespace IdS4.Application.Queries
{
    public interface IRoleQueries
    {
        Task<Paged<VmRole>> GetRoles(PagingQuery query);

        Task<VmRole> GetRole(string id);
    }
}
