using IdS4.Application.Models.Paging;
using IdS4.CoreApi.Models.User;
using System.Threading.Tasks;

namespace IdS4.Application.Queries
{
    public interface IUserQueries
    {
        Task<Paged<VmUser>> GetUsers(PagingQuery query);

        Task<VmUser> GetUser(string id);
    }
}
