using System.Threading.Tasks;
using IdS4.Application.Models.Client;
using IdS4.Application.Models.Paging;

namespace IdS4.Application.Queries
{
    public interface IClientQueries
    {
        Task<Paged<VmClient>> GetClients(PagingQuery query);

        Task<VmSplitClient> GetClient(int id);
    }
}
