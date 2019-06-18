using IdS4.CoreApi.Models.Scope;
using IdS4.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdS4.Application.Queries
{
    public class ScopeQueries : IScopeQueries
    {
        private readonly IdS4ConfigurationDbContext _configurationDb;

        public ScopeQueries(IdS4ConfigurationDbContext configurationDb)
        {
            _configurationDb = configurationDb;
        }

        public async Task<List<VmSelectItem>> GetClientScopeSelectItems()
        {
            var identityScope = await _configurationDb.IdentityResources
                .AsNoTracking()
                .Select(s => new VmSelectItem(s.DisplayName, s.Name))
                .ToListAsync();

            var apiScope = await _configurationDb.ApiScopes
                .AsNoTracking()
                .Select(s => new VmSelectItem(s.DisplayName, s.Name))
                .ToListAsync();

            return identityScope.Concat(apiScope).ToList();
        }
    }
}
