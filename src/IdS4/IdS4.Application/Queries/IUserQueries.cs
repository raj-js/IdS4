using IdS4.Application.Models.Paging;
using System.Threading.Tasks;
using IdS4.Application.Models.User;

namespace IdS4.Application.Queries
{
    public interface IUserQueries
    {
        Task<Paged<VmUser>> GetUsers(PagingQuery query);

        Task<VmUser> GetUser(string id);
    }
}
