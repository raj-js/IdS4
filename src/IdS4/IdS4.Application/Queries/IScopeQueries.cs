using System.Collections.Generic;
using System.Threading.Tasks;
using IdS4.Application.Models.Scope;

namespace IdS4.Application.Queries
{
    public interface IScopeQueries
    {
        Task<List<VmSelectItem>> GetClientScopeSelectItems();
    }
}
