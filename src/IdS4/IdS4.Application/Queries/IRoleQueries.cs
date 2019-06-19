using IdS4.Application.Models.Paging;
using System.Threading.Tasks;
using IdS4.Application.Models.Role;

namespace IdS4.Application.Queries
{
    public interface IRoleQueries
    {
        Task<Paged<VmRole>> GetRoles(PagingQuery query);

        Task<VmRole> GetRole(string id);
    }
}
