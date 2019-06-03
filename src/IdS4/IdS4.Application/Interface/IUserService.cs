using IdS4.Identity;
using System.Threading.Tasks;

namespace IdS4.Application.Interface
{
    public interface IUserService
    {
        Task CreateAsync(IdS4User user);

        Task ModifyAsync(IdS4User user);

        Task RemoveAsync(IdS4User user);

        Task<IdS4User> FindAsync(string id);

        // Paged<IdS4User> SearchAsync(PageQuery<IdS4User> query);
    }
}
